using Hakkers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hakkers.Pages.Admin.UsersRoles
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IList<UserAndRoles> UsersAndRoles { get; set; }

        public async Task OnGet()
        {
            var users = await _userManager.Users.ToListAsync();
            UsersAndRoles = new List<UserAndRoles>();
            foreach (var user in users)
            {
                UsersAndRoles.Add(new UserAndRoles()
                {
                    Id = user.Id,
                    UserName = user.Email,
                    Email = user.Email,
                    Roles = await _userManager.GetRolesAsync(user)
                });
            }

            //TODO: Sorting result for better user experience.
            //List<UserAndRole> SortedUsersAndRole = UsersAndRole.OrderBy(x => x.UserName).ToList();
        }
    }

    public class UserAndRoles
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }
}