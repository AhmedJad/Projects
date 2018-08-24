using JobPublisher.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JobPublisher.Models
{
    public class ApplyForJob
    {
        public int Id { set; get; }
        [Required]
        public string Message { set; get; }
        public DateTime ApplyDate { set; get; }
        public string UserId { set; get; }
        public int JobId { set; get; }
        
        public  Job Job { set; get; }
        public  ApplicationUser User { set; get; }
    }
}
