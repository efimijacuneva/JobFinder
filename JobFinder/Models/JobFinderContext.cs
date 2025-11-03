using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace JobFinder.Models
{
    public class JobFinderContext : ApplicationDbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder
                .Entity<Feedback>()
                .HasRequired(f => f.Company)
                .WithMany()                   
                .HasForeignKey(f => f.CompanyId)
                .WillCascadeOnDelete(false);

            
            modelBuilder
                .Entity<Feedback>()
                .HasRequired(f => f.Candidate)
                .WithMany()                   
                .HasForeignKey(f => f.CandidateId)
                .WillCascadeOnDelete(false);
        }

        public DbSet<JobPost> JobPosts { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<SavedJob> SavedJobs { get; set; }
    }
}
