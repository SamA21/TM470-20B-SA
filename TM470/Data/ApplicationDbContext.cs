using TM470.Models.db;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TM470.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {


        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {

        }


        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Venue> Venue { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<EventType> EventType { get; set; }
        public virtual DbSet<PeopleInterest> PeopleInterest { get; set; }
        public virtual DbSet<InterestLevel> InterestLevel { get; set; }

    }

}
