using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TM470.Data;
using TM470.Models.db;
using TM470.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace TM470.Controllers
{
    public class EventsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private IHostingEnvironment _env;

        public EventsController(
            UserManager<ApplicationUser> userManager, 
            ApplicationDbContext dbContext,
            IHostingEnvironment environment)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _env = environment;
        }


        [HttpGet]
        public EventsViewModel GetEvents()
        {
            EventsViewModel events = new EventsViewModel();
            events.Events = new List<EventViewModel>();
            var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = _dbContext.Users.SingleOrDefault(x => x.Id == currentUserId);
            string path = $"https://{Request.Host}/uploads/";
            string webRootPath = _env.WebRootPath;

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
                        TicketsSold = x.TicketsSold,
                        ImageURL = x.MainImageName
                    }).ToList();

                }
            }
            else
            {
                events.Events = _dbContext.Event.Where(x => (x.EventLive || x.EventLiveDate < DateTime.Now)
                    && x.EventDate > DateTime.Now).OrderBy(x => x.EventDate)
                    .Select(x => new EventViewModel()
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
                    TicketsSold = x.TicketsSold,
                    Company = _dbContext.Company.SingleOrDefault(y => y.CompanyId == x.CompanyId),
                    CompanyId = x.CompanyId,
                    ImageURL = x.MainImageName
                }).ToList();
            }

            foreach(var update in events.Events)
            {
                if (!string.IsNullOrWhiteSpace(update.ImageURL))
                {
                    bool exists = System.IO.File.Exists(Path.Combine(webRootPath, "Uploads", update.ImageURL));
                    if (exists)
                        update.ImageURL = path + update.ImageURL;
                    else
                        update.ImageURL = string.Empty;
                }
                else
                {
                    update.ImageURL = string.Empty;
                }
            }

            return events; 
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
            if (newEvent != null)
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
                                                TicketsSold = 0,
                                                MainImageName = newEvent.ImageName
                                            };
                                            _dbContext.Event.Add(eventEntity);
                                            _dbContext.SaveChanges();
                                        } 
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
        public JsonResult SubmitEditEvent([FromBody] EditEventModel editEvent)
        {
            if (editEvent != null)
            {
                var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var currentUser = _dbContext.Users.SingleOrDefault(x => x.Id == currentUserId);
                if (currentUser != null)
                {
                    var company = _dbContext.Company.SingleOrDefault(x => x.CompanyId == currentUser.CompanyId);
                    if (company != null)
                    {
                        var existing = _dbContext.Event.SingleOrDefault(x => x.EventId == editEvent.Id);
                        if (existing != null)
                        {
                            bool saved = true;
                            string error = string.Empty;
                            if (DateTime.TryParse(editEvent.EventDate, out DateTime eventDate))
                            {
                                var parsedDate = DateTime.TryParse(editEvent.EventLiveDate, out DateTime eventLiveDate);
                                if (!parsedDate)
                                    eventLiveDate = DateTime.Now;

                                bool alreadyExists = false;
                                if (existing.EventName.ToLower() != editEvent.Name.ToLower())
                                {
                                    var sameNames = _dbContext.Event.Where(x => x.EventName == editEvent.Name);
                                    if (sameNames.Count() > 0)
                                        alreadyExists = true;
                                }

                                var venue = _dbContext.Venue.SingleOrDefault(x => x.VenueId == editEvent.Venue.VenueId);
                                if (venue != null)
                                {
                                    var eventType = _dbContext.EventType.SingleOrDefault(x => x.EventTypeId == editEvent.EventType.EventTypeId);
                                    if (eventType != null)
                                    {
                                        if (!alreadyExists)
                                        {
                                            try
                                            {
                                                existing.EventCapacity = editEvent.EventCapacity;
                                                existing.EventInformation = editEvent.Information;
                                                existing.EventDate = eventDate;
                                                existing.EventLiveDate = eventLiveDate;
                                                existing.EventName = editEvent.Name;
                                                existing.TicketsSold = editEvent.TicketsSold;
                                                existing.TicketPrice = editEvent.TicketPrice;
                                                existing.EventType = eventType;
                                                existing.EventTypeId = eventType.EventTypeId;
                                                existing.Venue = venue;
                                                existing.VenueId = venue.VenueId;

                                                existing.UpdateDate = DateTime.Now;
                                                existing.UpdatedBy = currentUser;

                                                _dbContext.Entry(existing).State = EntityState.Modified;
                                                _dbContext.SaveChanges();
                                            }
                                            catch (Exception ex)
                                            {
                                                saved = false;
                                                error = ex.Message;
                                            }
                                        }
                                        else
                                        {
                                            saved = false;
                                            error = "Name already in use";
                                        }
                                    }
                                    else
                                    {
                                        saved = false;
                                        error = "Event Type invalid";
                                    }
                                }
                                else
                                {
                                    saved = false;
                                    error = "Venue invalid";
                                }
                            }
                            else
                            {
                                saved = false;
                                error = "No valid event date";
                            }

                            if (!saved)
                                return Json(new { Message = "Failed to edit Event " + error });
                            else
                                return Json(new { Message = "Edited new Event" });

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