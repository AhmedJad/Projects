using JobPublisher.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JobPublisher.Models
{
    public class Job
    {
        public int Id { set; get; }
        [Required]
        [Display(Name = "Job Title")]
        public string jobTitle { set; get; }
        [Required]
        [Display(Name = "Job Content")]
        public string JobContent { set; get; }
        [Display(Name = "Job Image")]
        public string JobImage { set; get; }
        [Display(Name = "Job Category")]
        public int CategoryId { set; get; }
        public string UserId { set; get; }
        public Category Category { set; get; }
        [Display(Name = "Publisher")]
        public ApplicationUser User { set; get; }
    }
}
