using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobPublisher.Models;
using Microsoft.AspNetCore.Identity;

namespace JobPublisher.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Job> Jobs { set; get; }
        public string UserType { set; get; }
    }
}
