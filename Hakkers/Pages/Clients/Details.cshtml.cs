using Hakkers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Hakkers.Pages.Clients
{
    public class DetailsModel : PageModel
    {
        private readonly Data.AspNetProjectContext _context;

        public DetailsModel(Data.AspNetProjectContext context)
        {
            _context = context;
        }

        public Models.Clients Client { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Client = await _context.Clients.FirstOrDefaultAsync(m => m.Id == id);

            if (Client == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}