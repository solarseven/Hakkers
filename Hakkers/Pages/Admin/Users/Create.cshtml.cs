using Hakkers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Hakkers.Pages.Admin.Users
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(UserManager<IdentityUser> userManager)
        {

            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            //TODO: If there is time consider the following.
            // UserName is used to login into the application, however the logging field validates for an EmailAddress
            // Therefore to not have to change code in the scaffolded Identity Framework front end,
            // the UserName AND the EmailAddress are set to the same value.

            //[Required]
            //public string UserName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            public string PhoneNumber { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public IActionResult OnGet()
        {
            return Page();
        }        

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Input.Password != Input.ConfirmPassword)
            {
                return Page();
            }

            var user = new IdentityUser()
            {
                //TODO: If there is time consider the following.
                // UserName is used to login into the application, however the logging field validates for an EmailAddress
                // Therefore to not have to change code in the scaffolded Identity Framework front end,
                // the UserName AND the EmailAddress are set to the same value.

                UserName = Input.Email,
                NormalizedUserName = Input.Email.ToUpper(),
                Email = Input.Email,
                NormalizedEmail = Input.Email.ToUpper(),
                EmailConfirmed = false,
                //PasswordHash = new PasswordHasher<>,
                //SecurityStamp = new SecurityStamp,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                PhoneNumber = Input.PhoneNumber,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = true,
                AccessFailedCount = 0,
            };
            
            var result = _userManager.CreateAsync(user, Input.Password).Result;

            if (result.Succeeded)
            {
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}