using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TM470.Models.db
{
    public class PeopleInterest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PeopleInterestId { get; set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public int InterestLeveld { get; set; }
        [ForeignKey("InterestLeveld")]
        public InterestLevel InterestLevel { get; set; }
        public int Eventld { get; set; }
        [ForeignKey("Eventld")]
        public Event Event { get; set; }
    }
}
