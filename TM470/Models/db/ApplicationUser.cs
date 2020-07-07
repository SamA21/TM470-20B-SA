using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TM470.Models.db
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        [Required]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
        [InverseProperty("CreatedBy")]
        public List<Event> EventsCreated { get; set; }

        [InverseProperty("UpdatedBy")]
        public List<Event> EventsUpdated { get; set; }

    }
}
