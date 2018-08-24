using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using JobPublisher.Data;
using JobPublisher.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace JobPublisher.Pages.ApplyForJobs
{
    [Authorize(Roles ="Applier")]
    public class ApplyJobModel : PageModel
    {
        private readonly JobPublisher.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ApplyJobModel(JobPublisher.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [BindProperty]
        public ApplyForJob ApplyForJob { get; set; }
        public IActionResult OnGet( int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            HttpContext.Session.SetInt32("AppliedJobId",(int)Id);
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var CurrentUserId = _userManager.GetUserId(HttpContext.User);
            var AppliedJobId = (int)HttpContext.Session.GetInt32("AppliedJobId");
            // check if user applied before
            var IsUserApplyBefore = await _context.ApplyForJob
                .Include(a => a.Job)
                .Include(a => a.User).SingleOrDefaultAsync(m => m.JobId == AppliedJobId && m.UserId==CurrentUserId);
            // check if user applied before
            if (IsUserApplyBefore == null)
            {
                ApplyForJob.ApplyDate = DateTime.Now;
                ApplyForJob.UserId = CurrentUserId;
                ApplyForJob.JobId =AppliedJobId;
                _context.ApplyForJob.Add(ApplyForJob);
                await _context.SaveChangesAsync();
                ViewData["ApplyMessage"] = "You have applied successfully";
            }
            else
            {
                ViewData["ApplyMessage"]="You applied to this job before ";
            }
            return Page();
        }
    }
}