//This is for future updates
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobFinder.Models
{
    public class SavedJob
    {
        public int Id { get; set; }

        public string CandidateId { get; set; }
        [ForeignKey("CandidateId")]
        public virtual ApplicationUser Candidate { get; set; }

        public int JobPostId { get; set; }
        [ForeignKey("JobPostId")]
        public virtual JobPost JobPost { get; set; }

        public DateTime SavedOn { get; set; } = DateTime.Now;
    }
}
