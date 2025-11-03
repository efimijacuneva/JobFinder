using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobFinder.Models
{
    public class JobApplication
    {
        public int Id { get; set; }

        [Required]
        public string CandidateId { get; set; }
        [ForeignKey("CandidateId")]
        public virtual ApplicationUser Candidate { get; set; }

        [Required]
        public int JobPostId { get; set; }
        [ForeignKey("JobPostId")]
        public virtual JobPost JobPost { get; set; }

        [Display(Name = "CV File")]
        public string CvFilePath { get; set; }

        public DateTime AppliedOn { get; set; } = DateTime.Now;

        
        [Display(Name = "Application Status")]
        public string Status { get; set; } = "Pending"; // Pending, Reviewed, Accepted, Rejected

        [Display(Name = "Company Response")]
        public string CompanyResponse { get; set; }

        [Display(Name = "Response Date")]
        public DateTime? ResponseDate { get; set; }

        [Display(Name = "Response By")]
        public string ResponseBy { get; set; }
    }
}
