using Hakkers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


//TODO:Fix frontend username and role overlay each other.
namespace Hakkers.Pages.Admin.UsersRoles
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public DetailsModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IList<UserAndRoles> UsersAndRoles { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _userManager.Users.FirstOrDefault(m => m.Id == id);

            if (User == null)
            {
                return NotFound();
            }

            UsersAndRoles = new List<UserAndRoles>
            {
                new UserAndRoles()
                {
                    Id = user.Id,
                    UserName = user.Email,
                    Email = user.Email,
                    Roles = await _userManager.GetRolesAsync(user)
                }
            };

            return Page();
        }
    }
}