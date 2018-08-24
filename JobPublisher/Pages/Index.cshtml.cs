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
using Microsoft.AspNetCore.Http;

namespace JobPublisher.Pages.Home
{
    public class IndexModel : PageModel
    {
        private  string CurrentUserId;
        private readonly JobPublisher.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public IndexModel(JobPublisher.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<bool> IsApplied(int id)
        {
            CurrentUserId = _userManager.GetUserId(HttpContext.User);
            var AppliedJob=await _context.ApplyForJob
                .Where(m => (m.UserId == CurrentUserId|| m.Job.UserId == CurrentUserId) && m.JobId == id).ToListAsync();
            if (AppliedJob.Count() > 0)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> IsPublished(int id)
        {
            CurrentUserId = _userManager.GetUserId(HttpContext.User);
            var Published = await _context.Job.Where(m => m.UserId == CurrentUserId && m.Id == id).ToListAsync();
            if (Published.Count() > 0)
            {
                return true;
            }
            return false;
        }
        [BindProperty]
        public IList<Job> Job { get;set; }

        //display all Jobs
        public async Task OnGetAsync()
        {
            HttpContext.Session.SetString("LastUrl", "/Index");
            //get All Jobs
            Job = await _context.Job
                .Include(j => j.Category)
                .Include(j => j.User).ToListAsync();
        }
        public async Task OnPostPublishedJobsAsync()
        {
            CurrentUserId = _userManager.GetUserId(HttpContext.User);
            Job = await _context.Job
              .Include(j => j.Category)
              .Include(j => j.User).Where(m=>m.UserId==CurrentUserId).ToListAsync();
            
        }
        public async Task OnPostPublisherAppliedJobsAsync()
        {
            CurrentUserId = _userManager.GetUserId(HttpContext.User);
            var AppliedJobs = await _context.ApplyForJob
                    .Include(a => a.Job)
                    .Include(a => a.User)
                    .Where(m => m.Job.UserId == CurrentUserId).ToListAsync();

            Job = (from AppliedJob
                  in AppliedJobs
                  select AppliedJob.Job).ToList();
        }

        public async Task OnPostUserAppliedJobsAsync()
        {
            CurrentUserId = _userManager.GetUserId(HttpContext.User);
            var AppliedJobs = await _context.ApplyForJob
                    .Include(a => a.Job)
                    .Include(a => a.User)
                    .Where(m => m.UserId == CurrentUserId).ToListAsync();

            Job = (from AppliedJob
                  in AppliedJobs
                   select AppliedJob.Job).ToList();
        }       
    }
}
