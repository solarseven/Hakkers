using Hakkers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hakkers.Pages.Admin.UsersRoles
{
    [Authorize(Roles = "Admin")]
    public class AddRoleToUserModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddRoleToUserModel(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<SelectListItem> UsersSelectList { get; set; }
        public List<SelectListItem> RolesSelectList { get; set; }

        [BindProperty]
        public string SelectedUser { get; set; }

        [BindProperty]
        public string SelectedRole { get; set; }

        public async Task OnGetAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            UsersSelectList = new List<SelectListItem>();
            foreach (var user in users)
            {
                UsersSelectList.Add(new SelectListItem()
                {
                    Value = user.Email,
                    Text = user.Email,
                });
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
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(SelectedUser);
                var result = await _userManager.AddToRoleAsync(user, SelectedRole);
            }

            return RedirectToPage("Index");
        }
    }
}