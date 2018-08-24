﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JobPublisher.Data;
using JobPublisher.Models;
using Microsoft.AspNetCore.Authorization;

namespace JobPublisher.Pages.Categories
{
    [Authorize(Roles="Admin")]
    public class IndexModel : PageModel
    {
        private readonly JobPublisher.Data.ApplicationDbContext _context;

        public IndexModel(JobPublisher.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Category> Category { get;set; }

        public async Task OnGetAsync()
        {
            Category = await _context.Category.ToListAsync();
        }
    }
}
