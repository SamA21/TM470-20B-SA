using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TM470.Data;
using TM470.Models;

namespace TM470.Controllers
{
    public class VenuesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public VenuesController(
            UserManager<ApplicationUser> userManager, 
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }


        [HttpGet]
        public List<Venue> GetVenues()
        {
            List<Venue> venues = new List<Venue>();
            var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var currentUser = _dbContext.Users.SingleOrDefault(x => x.Id == currentUserId);
            if (currentUser != null)
            {
                var company = _dbContext.Company.SingleOrDefault(x => x.CompanyId == currentUser.CompanyId);
                if (company != null)
                {
                    venues = _dbContext.Venue.Where(x => x.CompanyId == company.CompanyId).ToList();
                }
            }
            return venues; // will return an empty venue list if can't find user/ company. 
        }       
        
        [HttpPost]
        public JsonResult SubmitNewVenue([FromBody] VenueViewModel venue)
        {
            if(venue != null)
            {
                var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var currentUser = _dbContext.Users.SingleOrDefault(x => x.Id == currentUserId);
                if (currentUser != null)
                {
                    var company = _dbContext.Company.SingleOrDefault(x => x.CompanyId == currentUser.CompanyId);
                    if (company != null)
                    {
                        var existing = _dbContext.Venue.Where(x => x.VenueName.ToLower() == venue.Name.ToLower());
                        if (existing.Count() == 0)
                        {
                            bool saved = true;
                            string error = string.Empty;
                            try
                            {
                                Venue newVenue = new Venue()
                                {
                                    VenueName = venue.Name,
                                    VenueLocation = venue.Location,
                                    CompanyId = company.CompanyId,
                                    Company = company
                                };
                                _dbContext.Venue.Add(newVenue);
                                _dbContext.SaveChanges();
                            }
                            catch(Exception ex)
                            {
                                saved = false;
                                error = ex.Message;
                            }

                            if (!saved)
                                return Json(new { Message = "Failed to create Venue " + error });
                            else
                                return Json(new { Message = "Created new Venue" });

                        }
                    }
                }
            }

            return Json(new { Message = "Error finding user Info" });

        }
    }
}