using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TM470.Models.db;

namespace TM470.Models
{
    public class CreateInterestViewModel
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public InterestLevel Level { get; set; }
    }
}
