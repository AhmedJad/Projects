using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobPublisher.Data;
using JobPublisher.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace JobPublisher.Pages.ApplyForJobs
{
    [Authorize(Roles = "Applier")]
    public class EditModel : PageModel
    {
        private readonly JobPublisher.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public EditModel(JobPublisher.Data.ApplicationDbContext context,UserManager<ApplicationUser> userManager)
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
                .Include(a => a.User).SingleOrDefaultAsync(m => m.JobId == id && m.UserId == currentUserId);

            if (ApplyForJob == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var currentUserId = _userManager.GetUserId(HttpContext.User);
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ApplyForJob).State = EntityState.Modified;

            try
            {
                ViewData["EditMessage"] = "Has updated Successfully";
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplyForJobExists(ApplyForJob.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Page();
        }

        private bool ApplyForJobExists(int id)
        {
            return _context.ApplyForJob.Any(e => e.Id == id);
        }
    }
}
