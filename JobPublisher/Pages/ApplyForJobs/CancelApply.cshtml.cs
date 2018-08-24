using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JobPublisher.Data;
using JobPublisher.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace JobPublisher.Pages.ApplyForJobs
{
    [Authorize(Roles = "Applier")]
    public class CancelApplyModel : PageModel
    {
        private readonly JobPublisher.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CancelApplyModel(JobPublisher.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [BindProperty]
        public ApplyForJob ApplyForJob { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var currentUserId = _userManager.GetUserId(HttpContext.User);
            if (id == null)
            {
                return NotFound();
            }

            ApplyForJob = await _context.ApplyForJob
                .Include(a => a.Job)
                .Include(a => a.User).SingleOrDefaultAsync(m => m.JobId == id&&m.UserId==currentUserId);

            if (ApplyForJob == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var currentUserId = _userManager.GetUserId(HttpContext.User);
            if (id == null)
            {
                return NotFound();
            }

            ApplyForJob = await _context.ApplyForJob
                .Include(a => a.Job)
                .Include(a => a.User).SingleOrDefaultAsync(m => m.JobId == id && m.UserId == currentUserId);
            if (ApplyForJob != null)
            {
                _context.ApplyForJob.Remove(ApplyForJob);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Index");
        }
    }
}
