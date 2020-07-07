using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TM470.Data;
using TM470.Models;
using TM470.Models.db;

namespace TM470.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }


        [HttpPost]
        public JsonResult SubmitNewInterest([FromBody] CreateInterestViewModel interest)
        {
            if (interest != null)
            {

                var existing = _dbContext.PeopleInterest.Where(x => x.Eventld == interest.EventId && x.EmailAddress == interest.Email );
                if (existing.Count() == 0)
                {
                    bool saved = true;
                    string error = string.Empty;
                    try
                    {
                        var selectedEvent = _dbContext.Event.SingleOrDefault(x => x.EventId == interest.EventId);
                        if (selectedEvent != null)
                        {
                            var selectedInterestLevel = _dbContext.InterestLevel.SingleOrDefault(x => x.InterestLevelId == interest.Level.InterestLevelId);
                            if (selectedInterestLevel != null) {
                                PeopleInterest newInterest = new PeopleInterest()
                                {
                                    Name = interest.Name,
                                    EmailAddress = interest.Email,
                                    Eventld = selectedEvent.EventId,
                                    Event = selectedEvent,
                                    InterestLeveld = selectedInterestLevel.InterestLevelId,
                                    InterestLevel = selectedInterestLevel   
                                };
                                _dbContext.PeopleInterest.Add(newInterest);
                                _dbContext.SaveChanges();
                                selectedEvent.PeopleIntrested += 1;
                                _dbContext.Entry(selectedEvent).State = EntityState.Modified;
                                _dbContext.SaveChanges();
                            }
                            else
                            {
                                saved = false;
                                error = "Interest level not found";
                            }
                        }
                        else
                        {
                            saved = false;
                            error = "Event not found";
                        }
                    }
                    catch (Exception ex)
                    {
                        saved = false;
                        error = ex.Message;
                    }

                    if (!saved)
                        return Json(new { Message = "Failed to create Interest " + error });
                    else
                        return Json(new { Message = "Created new Interest" });

                }
            else
            {
                return Json(new { Message = "A venue already exists with that name" });
            }

        }

            return Json(new { Message = "Faild to submit interest" });

        }

        [HttpGet]
        public InterestLevels GetInterestLevels()
        {
            InterestLevels interestLevels = new InterestLevels();
            interestLevels.levels = new List<InterestLevel>();
            interestLevels.levels = _dbContext.InterestLevel.ToList();
            return interestLevels;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
