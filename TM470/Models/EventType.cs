using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TM470.Models
{
    public class EventType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventTypeId { get; set; }
        public string EventTypeName { get; set; }
        public int Capcity { get; set; }
        public int VenueId { get; set; }
        [ForeignKey("VenueId")]
        public Venue Venue { get; set; }
    }
}
