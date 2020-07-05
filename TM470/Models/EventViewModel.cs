﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TM470.Models
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime EventDate { get; set; }
        public string Information { get; set; }
        public DateTime? EventLiveDate { get; set; }
        public Venue Venue { get; set; }
        public EventType EventType { get; set; }
        public int EventCapacity { get; set; }
        public decimal TicketPrice { get; set; }
        public int TicketsSold { get; set; }
        public int PeopleIntrested { get; set; }

    }
}