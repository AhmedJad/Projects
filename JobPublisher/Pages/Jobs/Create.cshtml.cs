using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using JobPublisher.Data;
using JobPublisher.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace JobPublisher.Pages.Jobs
{
    [Authorize(Roles ="Publisher")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        public CreateModel(ApplicationDbContext context, IHostingEnvironment appEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _userManager = userManager;     
        }

        public IActionResult OnGet()
        {
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "CateogryName");
            return Page();
        }

        [BindProperty]
        public Job Job { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //upload Job Image into JobImages Folder in wwwroot
            var Files = HttpContext.Request.Form.Files;
            foreach (var File in Files)
            {
                if (File != null &&File.Length > 0)
                {
                    var _file = File;

                    var JobImagesFolderPath = Path.Combine(_appEnvironment.WebRootPath, "JobImages");
                    if (_file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(_file.FileName);
                        using (var fileStream = new FileStream(Path.Combine(JobImagesFolderPath, fileName), FileMode.Create))
                        {
                            await _file.CopyToAsync(fileStream);
                            Job.JobImage= fileName;
                        }

                    }
                }
            }
            //upload Job Image into JobImages Folder in wwwroot

            var CurrentUserId = _userManager.GetUserId(HttpContext.User);
            Job.UserId = CurrentUserId;

            _context.Job.Add(Job);
            await _context.SaveChangesAsync();
            return RedirectToPage("/Index");
        }
    }
}