using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TM470.Models
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventInformation { get; set; }
        public DateTime EventDate { get; set; }
        public bool EventLive { get; set; }
        public DateTime EventLiveDate { get; set; }
        public int VenueId { get; set; }
        [ForeignKey("VenueId")]
        public Venue Venue { get; set; }
        public int EventTypeId { get; set; }
        [ForeignKey("EventTypeId")]
        public EventType EventType { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public ApplicationUser UpdatedBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public int PeopleIntrested { get; set; } 
        public int EventCapacity { get; set; }// if needs to be different to venue capacity
        public decimal TicketPrice { get; set; }
        public int TicketsSold { get; set; }
        public int CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public Company Company { get; set; }

    }
}
