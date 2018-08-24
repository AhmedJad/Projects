using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JobPublisher.Data;
using JobPublisher.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace JobPublisher.Pages.ApplyForJobs
{
    [Authorize(Roles = "Applier,Publisher")]
    public class DetailsModel : PageModel
    {
        private readonly JobPublisher.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public DetailsModel(JobPublisher.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public ApplyForJob ApplyForJob { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var CurrentUserId = _userManager.GetUserId(HttpContext.User);
            if (id == null)
            {
                return NotFound();
            }

            ApplyForJob = await _context.ApplyForJob
                .Include(a => a.Job)
                .Include(a => a.User)
                .SingleOrDefaultAsync(m => m.JobId == id&&(m.UserId==CurrentUserId||m.Job.UserId==CurrentUserId));

            if (ApplyForJob == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
