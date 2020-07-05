using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TM470.Data;
using TM470.Models;

namespace TM470.Controllers
{
    public class EventsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public EventsController(
            UserManager<ApplicationUser> userManager, 
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }


        [HttpGet]
        public EventsViewModel GetEvents()
        {
            EventsViewModel events = new EventsViewModel();
            events.Events = new List<EventViewModel>();
            var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var currentUser = _dbContext.Users.SingleOrDefault(x => x.Id == currentUserId);
            if (currentUser != null)
            {
                var company = _dbContext.Company.SingleOrDefault(x => x.CompanyId == currentUser.CompanyId);
                if (company != null)
                {
                    events.Events = _dbContext.Event.Where(x => x.CompanyId == company.CompanyId).Select(x => new EventViewModel()
                    {
                        EventLiveDate = x.EventLiveDate.ToString("dd/MM/yyyy"),
                        EventCapacity = x.EventCapacity,
                        TicketPrice = x.TicketPrice,
                        EventType = x.EventType,
                        Venue = x.Venue,
                        EventDate = x.EventDate.ToString("dd/MM/yyyy"),
                        Id = x.EventId,
                        Information = x.EventInformation,
                        Name = x.EventName,
                        PeopleIntrested = x.PeopleIntrested,
                        TicketsSold = x.TicketsSold
                    }).ToList();

                }
            }
            return events; // will return an empty venue list if can't find user/ company. 
        }

        [HttpGet]
        public EventTypesViewModel GetEventTypes()
        {
            EventTypesViewModel types = new EventTypesViewModel();
            types.EventTypes = new List<EventTypeViewModel>();
            var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var currentUser = _dbContext.Users.SingleOrDefault(x => x.Id == currentUserId);
            if (currentUser != null)
            {
                var company = _dbContext.Company.SingleOrDefault(x => x.CompanyId == currentUser.CompanyId);
                if (company != null)
                {
                    types.EventTypes = _dbContext.EventType.Where(x => x.CompanyId == company.CompanyId).Select(x => new EventTypeViewModel()
                    {
                        Id = x.EventTypeId,
                        Type = x.EventTypeName
                    }).ToList();
                }
            }
            return types; // will return an empty venue list if can't find user/ company. 
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
        public JsonResult SubmitNewEvent([FromBody] CreateEventViewModel newEvent)
        {
            if(newEvent != null)
            {
                var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var currentUser = _dbContext.Users.SingleOrDefault(x => x.Id == currentUserId);
                if (currentUser != null)
                {
                    var company = _dbContext.Company.SingleOrDefault(x => x.CompanyId == currentUser.CompanyId);
                    if (company != null)
                    {
                        var existing = _dbContext.Event.Where(x => x.EventName.ToLower() == newEvent.Name.ToLower());
                        if (existing.Count() == 0)
                        {
                            bool saved = true;
                            string error = string.Empty;
                            try
                            {
                                if (DateTime.TryParse(newEvent.EventDate, out DateTime eventDate)) {
                                    var parsedDate = DateTime.TryParse(newEvent.EventLiveDate, out DateTime eventLiveDate);
                                    if (!parsedDate)
                                        eventLiveDate = DateTime.Now;

                                    var eventType = _dbContext.EventType.SingleOrDefault(x => x.EventTypeId == newEvent.EventTypeId);
                                    if (eventType != null)
                                    {
                                        var venue = _dbContext.Venue.SingleOrDefault(x => x.VenueId == newEvent.VenueId);
                                        if (venue != null)
                                        {
                                            Event eventEntity = new Event()
                                            {
                                                EventName = newEvent.Name,
                                                EventInformation = newEvent.Information,
                                                CompanyId = company.CompanyId,
                                                Company = company,
                                                TicketPrice = newEvent.TicketPrice,
                                                VenueId = venue.VenueId,
                                                Venue = venue,
                                                EventTypeId = eventType.EventTypeId,
                                                EventType = eventType,
                                                EventCapacity = newEvent.EventCapacity,
                                                EventDate = eventDate,
                                                EventLiveDate = eventLiveDate,
                                                CreatedBy = currentUser,
                                                CreatedDate = DateTime.Now,
                                                UpdateDate = DateTime.Now,
                                                UpdatedBy = currentUser,
                                                EventLive = (eventLiveDate  <= DateTime.Now) ? true : false, //sets event live to be true if date is less than now. 
                                                PeopleIntrested = 0,
                                                TicketsSold = 0
                                            };
                                            _dbContext.Event.Add(eventEntity);
                                            _dbContext.SaveChanges();
                                        }//error handling encased in the else statments 
                                        else
                                        {
                                            saved = false;
                                            error = "No Event Venue set";
                                        }
                                    }
                                    else
                                    {
                                        saved = false;
                                        error = "No Event Type set";
                                    }                                      
                                }
                                else
                                {
                                    saved = false;
                                    error = "No Event Date set";
                                }
                            }
                            catch (Exception ex)
                            {
                                saved = false;
                                error = ex.Message;
                            }

                            if (!saved)
                                return Json(new { Message = "Failed to create Event " + error });
                            else
                                return Json(new { Message = "Created new Event" });

                        }
                        else
                        {
                            return Json(new { Message = "A event already exists with that name" });
                        }
                    }
                }
            }

            return Json(new { Message = "Error finding user Info" });

        }

        [HttpPost]
        public JsonResult SubmitEditEvent([FromBody] VenueEditModel venue)
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