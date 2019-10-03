using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hakkers.Pages.Assignments
{
    public class IndexModel : PageModel
    {
        private readonly Data.AspNetProjectContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(Data.AspNetProjectContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Models.Assignments> Assignments { get; set; }

        public List<AssignmentViewModel> AssignmentViewModels = new List<AssignmentViewModel> { };

        public new IdentityUser User;
        public bool IsMechanic;

        public async Task OnGetAsync()
        {
            var User = await _userManager.GetUserAsync(HttpContext.User);
            IsMechanic = await _userManager.IsInRoleAsync(User, "Mechanic");

            if (IsMechanic)
            {
                var MechanicId = User.Id;
                Assignments = await _context.Assignments.ToListAsync();

                foreach (var assignment in Assignments)
                {
                    if (assignment.FkMechanic == MechanicId)
                    {
                        var client = await _context.Clients.FindAsync(assignment.FkClient);
                        var mechanic = await _userManager.FindByIdAsync(assignment.FkMechanic);
                        var category = await _context.AssignmentCategories.FindAsync(assignment.FkCategory);
                        var priority = await _context.AssignmentPriorities.FindAsync(assignment.FkPriority);

                        AssignmentViewModels.Add(new AssignmentViewModel
                        {
                            Id = assignment.Id,
                            Client = (client.FirstName + " " + client.LastName),
                            ClientPhone = client.TelephoneNumber.ToString(),
                            Mechanic = mechanic.UserName,
                            MechanicPhone = mechanic.PhoneNumber,
                            Category = category.Name,
                            Priority = priority.Name,
                            Description = assignment.Description,
                            Appointment = assignment.Appointment,
                        });
                    }
                }
            }
            else
            {
                Assignments = await _context.Assignments.ToListAsync();
                foreach (var assignment in Assignments)
                {
                    var client = await _context.Clients.FindAsync(assignment.FkClient);
                    var mechanic = await _userManager.FindByIdAsync(assignment.FkMechanic);
                    var category = await _context.AssignmentCategories.FindAsync(assignment.FkCategory);
                    var priority = await _context.AssignmentPriorities.FindAsync(assignment.FkPriority);

                    AssignmentViewModels.Add(new AssignmentViewModel
                    {
                        Id = assignment.Id,
                        Client = (client.FirstName + " " + client.LastName),
                        ClientPhone = client.TelephoneNumber.ToString(),
                        Mechanic = mechanic.UserName,
                        MechanicPhone = mechanic.PhoneNumber,
                        Category = category.Name,
                        Priority = priority.Name,
                        Description = assignment.Description,
                        Appointment = assignment.Appointment,
                    });
                }
            }
        }
    }

    public class AssignmentViewModel
    {
        public int Id { get; set; }
        public string Client { get; set; }
        public string ClientPhone { get; set; }
        public string Mechanic { get; set; }
        public string MechanicPhone { get; set; }
        public string Category { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public DateTime Appointment { get; set; }
    }
}