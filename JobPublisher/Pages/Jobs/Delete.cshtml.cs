using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JobPublisher.Data;
using JobPublisher.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace JobPublisher.Pages.Jobs
{
    [Authorize(Roles ="Admin,Publisher")]
    public class DeleteModel : PageModel
    {

        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        public DeleteModel(ApplicationDbContext context, IHostingEnvironment appEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _userManager = userManager;
        }

        [BindProperty]
        public Job Job { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var currentUserId = _userManager.GetUserId(HttpContext.User);
            if (id == null)
            {
                return NotFound();
            }

            Job = await _context.Job
                .Include(j => j.Category)
                .Include(j => j.User).SingleOrDefaultAsync(m => m.Id == id&&m.UserId==currentUserId);

            if (Job == null&&!HttpContext.User.IsInRole("Admin"))
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Job = await _context.Job.FindAsync(id);

            if (Job != null)
            {
                var JobImagesFolderPath = Path.Combine(_appEnvironment.WebRootPath, "JobImages");
                //remove the Image of the Job
                System.IO.File.Delete(Path.Combine(JobImagesFolderPath,Job.JobImage));
                //remove the Image of the Job
                _context.Job.Remove(Job);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(HttpContext.Session.GetString("LastUrl").ToString());
        }
    }
}
