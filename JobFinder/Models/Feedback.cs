//This is for future updates

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobFinder.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        [Required]
        public string CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual ApplicationUser Company { get; set; }

        [Required]
        public string CandidateId { get; set; }
        [ForeignKey("CandidateId")]
        public virtual ApplicationUser Candidate { get; set; }

        [Required, Range(1, 5)]
        public int Rating { get; set; }

        public string Comment { get; set; }

        public DateTime DatePosted { get; set; } = DateTime.Now;
    }
}
