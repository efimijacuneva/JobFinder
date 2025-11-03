using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using JobFinder.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JobFinder
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            
            Database.SetInitializer<JobFinderContext>(new CreateDatabaseIfNotExists<JobFinderContext>());
            
            
            using (var context = new JobFinderContext())
            {
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
            
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
