using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using JobPublisher.Models;
namespace JobPublisher.Models
{
    public class Category
    {
        public int Id { set; get; }

        [Required]
        [Display(Name = "Category Name")]
        public string CateogryName { set; get; }

        [Required]
        [Display(Name = "Category Description")]
        public string CategoryDescription { set; get; }
        public ICollection<Job> Jobs { set; get; }
    }
}
