using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobFinder.Models
{
    public class JobPost
    {
        public int Id { get; set; }

        [Required, Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        public DateTime DatePosted { get; set; } = DateTime.Now;

        public string CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual ApplicationUser Company { get; set; }

        [Required]
        public string Category { get; set; }
    }
}
