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
    public class DeleteModel : PageModel
    {
        private readonly Data.AspNetProjectContext _context;

        public DeleteModel(Data.AspNetProjectContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Assignments = await _context.Assignments.FindAsync(id);

            if (Assignments != null)
            {
                _context.Assignments.Remove(Assignments);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
