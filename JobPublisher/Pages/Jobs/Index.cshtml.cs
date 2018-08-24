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
using Microsoft.AspNetCore.Http;

namespace JobPublisher.Pages.Jobs
{
    [Authorize(Roles ="Admin")]
    public class IndexModel : PageModel
    {
        private readonly JobPublisher.Data.ApplicationDbContext _context;

        public IndexModel(JobPublisher.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<Job> Job { get;set; }

        public async Task OnGetAsync()
        {
            HttpContext.Session.SetString("LastUrl","/Jobs/Index");
            Job = await _context.Job
                .Include(j => j.Category)
                .Include(j => j.User).ToListAsync();
        }
    }
}
