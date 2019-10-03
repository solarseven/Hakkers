using Hakkers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hakkers.Pages.Admin.UsersRoles
{
    [Authorize(Roles = "Admin")]
    public class RevokeModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RevokeModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public new IdentityUser User { get; set; }
        public List<SelectListItem> UserRolesSelectList { get; set; }

        [BindProperty]
        public string SelectedUserRole { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User = _userManager.Users.FirstOrDefault(x => x.Id == id);

            if (User == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(User);
            UserRolesSelectList = new List<SelectListItem>();
            foreach (var userRole in userRoles)
            {
                UserRolesSelectList.Add(new SelectListItem()
                {
                    Value = userRole,
                    Text = userRole,
                });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
                var result = await _userManager.RemoveFromRoleAsync(user, SelectedUserRole);
            }

            return RedirectToPage("./Index");
        }
    }
}