using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Hakkers.Models;
using Microsoft.AspNetCore.Identity;
using Hakkers.Data;
using Microsoft.EntityFrameworkCore;

namespace Hakkers.Pages.Assignments
{
    public class CreateModel : PageModel
    {
        private readonly Data.AspNetProjectContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(Data.AspNetProjectContext context, 
                            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public List<SelectListItem> ClientsSelectList { get; set; }
        public List<SelectListItem> CatagoriesSelectList { get; set; }
        public List<SelectListItem> PrioritiesSelectList { get; set; }
        public List<SelectListItem> MechanicsSelectList { get; set; }        

        [BindProperty]
        public string SelectedClient { get; set; }

        [BindProperty]
        public string SelectedCategory { get; set; }

        [BindProperty]
        public string SelectedPriority { get; set; }

        [BindProperty]
        public string SelectedMechanic { get; set; }

        [BindProperty]
        public string Description { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var clients = await _context.Clients.ToListAsync();
            ClientsSelectList = new List<SelectListItem>();
            foreach (var client in clients)
            {
                ClientsSelectList.Add(new SelectListItem()
                {
                    Value = client.Id.ToString(),
                    Text = client.FirstName + " " + client.LastName,
                });
            }

            var assignmentCategories = await _context.AssignmentCategories.ToListAsync();
            CatagoriesSelectList = new List<SelectListItem>();
            foreach (var assignmentCategory in assignmentCategories)
            {
                CatagoriesSelectList.Add(new SelectListItem()
                {
                    Value = assignmentCategory.Id.ToString(),
                    Text = assignmentCategory.Name,
                });
            }

            var assignmentPriorities = await _context.AssignmentPriorities.ToListAsync();
            PrioritiesSelectList = new List<SelectListItem>();
            foreach (var assignmentPriority in assignmentPriorities)
            {
                PrioritiesSelectList.Add(new SelectListItem()
                {
                    Value = assignmentPriority.Id.ToString(),
                    Text = assignmentPriority.Name,
                });
            }

            var users = await _userManager.Users.ToListAsync();
            MechanicsSelectList = new List<SelectListItem>();
            foreach (var user in users)
            {
                bool isMechanic = _userManager.IsInRoleAsync(user, "Mechanic").Result;

                if (isMechanic == true)
                {
                    MechanicsSelectList.Add(new SelectListItem()
                    {
                        Value = user.Id,
                        Text = user.UserName,
                    });
                }
            }

            return Page();
        }

        [BindProperty]
        public Models.Assignments Assignment { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var firstStatus = await _context.AssignmentStatuses.FirstOrDefaultAsync();

            var user = await _userManager.GetUserAsync(HttpContext.User);
            Assignment.FkPlanner = user.Id;
            Assignment.FkClient = Convert.ToInt32(SelectedClient);
            Assignment.Description = Description;            
            Assignment.FkCategory = Convert.ToInt32(SelectedCategory);
            Assignment.FkPriority = Convert.ToInt32(SelectedPriority);
            Assignment.FkMechanic = SelectedMechanic;
            Assignment.Created = DateTime.Now;                     
            Assignment.Departure = null;
            Assignment.Arrival = null;
            Assignment.Finished = null;
            Assignment.Note = null;
            Assignment.FkStatus = firstStatus.Id;            

            _context.Assignments.Add(Assignment);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}