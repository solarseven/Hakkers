using Hakkers.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hakkers.Pages.Clients
{
    public class IndexModel : PageModel
    {
        private readonly Data.AspNetProjectContext _context;

        public IndexModel(Data.AspNetProjectContext context)
        {
            _context = context;
        }

        public IList<Models.Clients> Client { get; set; }

        public async Task OnGetAsync()
        {
            Client = await _context.Clients.ToListAsync();
        }
    }
}