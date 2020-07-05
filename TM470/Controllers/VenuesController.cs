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
        public VenuesViewModel GetVenues()
        {
            VenuesViewModel venues = new VenuesViewModel();
            venues.Venues = new List<VenueViewModel>();
            var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var currentUser = _dbContext.Users.SingleOrDefault(x => x.Id == currentUserId);
            if (currentUser != null)
            {
                var company = _dbContext.Company.SingleOrDefault(x => x.CompanyId == currentUser.CompanyId);
                if (company != null)
                {
                    venues.Venues = _dbContext.Venue.Where(x => x.CompanyId == company.CompanyId).Select(x => new VenueViewModel()
                    {
                        Capacity = x.Capacity,
                        Id = x.VenueId,
                        Location = x.VenueLocation,
                        Name = x.VenueName,
                        NumberOfEvents = _dbContext.Event.Where(e => e.VenueId == x.VenueId).Count()
                    }).ToList();

                }
            }
            return venues; // will return an empty venue list if can't find user/ company. 
        }       
        
        [HttpPost]
        public JsonResult SubmitNewVenue([FromBody] CreateVenueViewModel venue)
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
                                    Company = company,
                                    Capacity = venue.Capacity
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
                                return Json(new { Message = "Edited new Venue" });

                        }
                        else
                        {
                            return Json(new { Message = "A venue already exists with that name" });
                        }
                    }
                }
            }

            return Json(new { Message = "Error finding user Info" });

        }

        [HttpPost]
        public JsonResult SubmitEditVenue([FromBody] VenueEditModel venue)
        {
            if (venue != null)
            {
                var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var currentUser = _dbContext.Users.SingleOrDefault(x => x.Id == currentUserId);
                if (currentUser != null)
                {
                    var company = _dbContext.Company.SingleOrDefault(x => x.CompanyId == currentUser.CompanyId);
                    if (company != null)
                    {
                        var existing = _dbContext.Venue.SingleOrDefault(x => x.VenueId == venue.Id);
                        if (existing != null)
                        {
                            bool saved = true;
                            string error = string.Empty;
                            try
                            {
                                existing.Capacity = venue.Capacity;
                                existing.VenueName = venue.Name;
                                existing.VenueLocation = venue.Location;

                                _dbContext.Entry(existing).State = EntityState.Modified;
                                _dbContext.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                saved = false;
                                error = ex.Message;
                            }

                            if (!saved)
                                return Json(new { Message = "Failed to edit Venue " + error });
                            else
                                return Json(new { Message = "Edited new Venue" });

                        }
                        else
                        {
                            return Json(new { Message = "Can't find venue to be edited" });
                        }
                    }
                }
            }

            return Json(new { Message = "Error finding user Info" });

        }
    }
}