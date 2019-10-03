using Hakkers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hakkers.Pages.Admin.UsersRoles
{
    [Authorize(Roles = "Admin")]
    public class AddModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddModel(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public new IdentityUser User { get; set; }
        public List<SelectListItem> RolesSelectList { get; set; }

        [BindProperty]
        public string SelectedRole { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User = await _userManager.FindByIdAsync(id);

            if (User == null)
            {
                return NotFound();
            }

            var roles = await _roleManager.Roles.ToListAsync();
            RolesSelectList = new List<SelectListItem>();
            foreach (var role in roles)
            {
                RolesSelectList.Add(new SelectListItem()
                {
                    Value = role.Name,
                    Text = role.Name,
                });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
                var result = await _userManager.AddToRoleAsync(user, SelectedRole);
            }

            return RedirectToPage("./Index");
        }
    }
}