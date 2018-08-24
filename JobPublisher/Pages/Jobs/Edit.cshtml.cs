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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace JobPublisher.Pages.Jobs
{
    [Authorize(Roles ="Publisher")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        public EditModel(ApplicationDbContext context, IHostingEnvironment appEnvironment, UserManager<ApplicationUser> userManager)
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
                .Include(j => j.User).SingleOrDefaultAsync(m => m.Id == id && m.UserId == currentUserId);

            if (Job == null)
            {
                return NotFound();
            }
           ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "CategoryDescription");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Job).State = EntityState.Modified;

            try
            {
                //remove the Image of the Job
                var JobImagesFolderPath = Path.Combine(_appEnvironment.WebRootPath, "JobImages");
                System.IO.File.Delete(Path.Combine(JobImagesFolderPath, Job.JobImage));
                //remove the Image of the Job

                //upload Job Image into JobImages Folder in wwwroot
                var Files = HttpContext.Request.Form.Files;
                foreach (var File in Files)
                {
                    if (File != null && File.Length > 0)
                    {
                        var _file = File;
                        if (_file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(_file.FileName);
                            using (var fileStream = new FileStream(Path.Combine(JobImagesFolderPath, fileName), FileMode.Create))
                            {
                                await _file.CopyToAsync(fileStream);
                                Job.JobImage = fileName;
                            }

                        }
                    }
                }
                //upload Job Image into JobImages Folder in wwwroot
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(Job.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Index");
        }

        private bool JobExists(int id)
        {
            return _context.Job.Any(e => e.Id == id);
        }
    }
}
