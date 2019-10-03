using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Hakkers.Models;

namespace Hakkers.Pages.Assignments
{
    public class DetailsModel : PageModel
    {
        private readonly Data.AspNetProjectContext _context;

        public DetailsModel(Data.AspNetProjectContext context)
        {
            _context = context;
        }

        public Models.Assignments Assignments { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Assignments = await _context.Assignments.FirstOrDefaultAsync(m => m.Id == id);

            if (Assignments == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
