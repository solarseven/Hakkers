using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hakkers.Models;

namespace Hakkers.Pages.Assignments
{
    public class EditModel : PageModel
    {
        private readonly Data.AspNetProjectContext _context;

        public EditModel(Data.AspNetProjectContext context)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Assignments).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssignmentsExists(Assignments.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AssignmentsExists(int id)
        {
            return _context.Assignments.Any(e => e.Id == id);
        }
    }
}
