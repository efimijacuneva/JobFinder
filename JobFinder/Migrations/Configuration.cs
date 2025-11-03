namespace JobFinder.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using JobFinder.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<JobFinder.Models.JobFinderContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(JobFinder.Models.JobFinderContext context)
        {
            //  This method will be called after migrating to the latest version.

            // Create roles if they don't exist
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            
            if (!roleManager.RoleExists("Company"))
            {
                roleManager.Create(new IdentityRole("Company"));
            }
            
            if (!roleManager.RoleExists("Candidate"))
            {
                roleManager.Create(new IdentityRole("Candidate"));
            }
        }
    }
}
