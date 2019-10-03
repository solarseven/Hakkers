using Hakkers.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//TODO: In the Startup.cs file,
//      in the configure method,
//      after: app.UseAuthentication();
//      add: ApplicationDbInitializer.Seed(roleManager, userManager);

namespace Hakkers.Data
{
    public static class ApplicationDbInitializer
    {
        private static readonly string DefaultEmailExtension = "@hakkers.nl";
        private static readonly string DefaultPassword = "Password1!";
        private static readonly string DefaultPhoneNumber = "0123456789";

        private class UserInput
        {
            public string RoleName { get; set; }
            public string Email { get; set; }
        }

        private static readonly List<UserInput> UserInputList = new List<UserInput>
        {
            new UserInput{RoleName = "Admin", Email = "admin" + DefaultEmailExtension},
            new UserInput{RoleName = "Planner", Email = "planner1" + DefaultEmailExtension},
            new UserInput{RoleName = "Planner", Email = "planner2" + DefaultEmailExtension},
            new UserInput{RoleName = "Mechanic", Email = "mechanic1" + DefaultEmailExtension},
            new UserInput{RoleName = "Mechanic", Email = "mechanic2" + DefaultEmailExtension},
        };

        public static readonly List<string> AssignmentCategoriesList = new List<string>
        {
            "Gas",
            "Water",
            "Electricity",
        };

        public static readonly List<string> AssignmentPrioritiesList = new List<string>
        {
            "Maintenance",
            "Malfunction",
            "Critical",
        };

        public static readonly List<string> AssignmentStatusesList = new List<string>
        {
            "Inactive",
            "Left",
            "Arrived",
            "Started",
            "OnHold",
            "Finished",
        };

        public static async void Seed(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, AspNetProjectContext context)
        {
            //TODO: Run Update-DataBase in Package Manager Console to create database before running the application for the first time.
            context.Database.EnsureCreated();

            SeedAssignmentCategories(context);
            SeedAssignmentPriorities(context);
            SeedAssignmentStatuses(context);

            await SeedRole(roleManager);
            SeedUser(userManager);
            await SeedUserRoles(userManager);
        }

        private static void SeedAssignmentStatuses(AspNetProjectContext context)
        {
            foreach (var status in AssignmentStatusesList)
            {
                var result = context.AssignmentStatuses.FirstOrDefault(x => x.Name == status);

                if (result == null)
                {
                    context.AssignmentStatuses.Add(new AssignmentStatuses() { Name = status });
                    context.SaveChanges();
                };
            }
        }

        private static void SeedAssignmentPriorities(AspNetProjectContext context)
        {
            foreach (var priority in AssignmentPrioritiesList)
            {
                var result = context.AssignmentPriorities.FirstOrDefault(x => x.Name == priority);

                if (result == null)
                {
                    context.AssignmentPriorities.Add(new AssignmentPriorities() { Name = priority });
                    context.SaveChanges();
                };
            }
        }

        private static void SeedAssignmentCategories(AspNetProjectContext context)
        {
            foreach (var category in AssignmentCategoriesList)
            {
                var result = context.AssignmentCategories.FirstOrDefault(x => x.Name == category);

                if (result == null)
                {
                    context.AssignmentCategories.Add(new AssignmentCategories() { Name = category });
                    context.SaveChanges();
                };
            }
        }

        public static async Task SeedRole(RoleManager<IdentityRole> roleManager)
        {
            using (roleManager)
            {
                foreach (var UserInput in UserInputList)
                {
                    bool roleExists = roleManager.RoleExistsAsync(UserInput.RoleName).Result;

                    if (roleExists == false)
                    {
                        await roleManager.CreateAsync(new IdentityRole { Name = UserInput.RoleName });
                    }
                }
            }
        }

        public static void SeedUser(UserManager<IdentityUser> userManager)
        {
            using (userManager)
            {
                foreach (var UserInput in UserInputList)
                {
                    var user =  userManager.FindByEmailAsync(UserInput.Email).Result;

                    if (user == null)
                    {
                        IdentityUser identityUser = new IdentityUser
                        {
                            UserName = UserInput.Email,
                            NormalizedUserName = UserInput.Email.ToUpper(),
                            Email = UserInput.Email,
                            NormalizedEmail = UserInput.Email.ToUpper(),
                            EmailConfirmed = false,
                            //PasswordHash = new PasswordHasher<>,
                            SecurityStamp = Guid.NewGuid().ToString(),
                            ConcurrencyStamp = Guid.NewGuid().ToString(),
                            PhoneNumber = DefaultPhoneNumber,
                            PhoneNumberConfirmed = false,
                            TwoFactorEnabled = false,
                            LockoutEnd = null,
                            LockoutEnabled = true,
                            AccessFailedCount = 0,
                        };

                        IdentityResult result = userManager.CreateAsync(identityUser, DefaultPassword).Result;
                    }
                }
            }
        }

        private static async Task SeedUserRoles(UserManager<IdentityUser> userManager)
        {
            using (userManager)
            {
                foreach (var UserInput in UserInputList)
                {                    
                    IdentityUser identityUser = new IdentityUser
                    {
                        UserName = UserInput.Email,
                        NormalizedUserName = UserInput.Email.ToUpper(),
                        Email = UserInput.Email,
                        NormalizedEmail = UserInput.Email.ToUpper(),
                        EmailConfirmed = false,
                        //PasswordHash = new PasswordHasher<>,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        PhoneNumber = DefaultPhoneNumber,
                        PhoneNumberConfirmed = false,
                        TwoFactorEnabled = false,
                        LockoutEnd = null,
                        LockoutEnabled = true,
                        AccessFailedCount = 0,
                    };

                    //TODO: Context BUG 1
                    // After the next code of line the database context (userManager) is disposed or maybe some other bug happens.
                    // Context needs to stay intact and not be disposed while in surrounding Using(userManager) block.
                    // For some good reason (of course), it doesn't do what I expected it to do, the reason why escapes me though.

                    var user = await userManager.FindByEmailAsync(UserInput.Email);

                    var rolesList = await userManager.GetRolesAsync(identityUser);
                    
                    bool addRole = true;                    
                    if (rolesList.Count == 0)
                    {
                        _ = await userManager.AddToRoleAsync(user, UserInput.RoleName);
                    }
                    else
                    {
                        foreach (var role in rolesList)
                        {
                            if (role == UserInput.RoleName)
                            {
                                addRole = false;
                            }
                        }

                        if (addRole == true)
                        {
                            _ = await userManager.AddToRoleAsync(user, UserInput.RoleName);
                        }
                    }
                }
            }
        }
    }
}