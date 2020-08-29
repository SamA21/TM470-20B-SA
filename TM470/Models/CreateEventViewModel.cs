using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TM470.Models
{
    public class CreateEventViewModel
    {
        public string Name { get; set; }
        public string Information { get; set; }
        public string EventDate { get; set; }
        public string EventLiveDate { get; set; }
        public int VenueId { get; set; }
        public int EventTypeId { get; set; }
        public int EventCapacity { get; set; }// if needs to be different to venue capacity
        public decimal TicketPrice { get; set; }
        public string ImageName { get; set; }
    }
}
