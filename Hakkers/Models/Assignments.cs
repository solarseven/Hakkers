using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hakkers.Models
{
    public partial class Assignments
    {
        public int Id { get; set; }

        [DisplayName("Planner")]
        public string FkPlanner { get; set; }

        [DisplayName("Client")]
        public int FkClient { get; set; }

        [DisplayName("Mechanic")]
        public string FkMechanic { get; set; }

        [DisplayName("Category")]
        public int FkCategory { get; set; }

        [DisplayName("Priority")]
        public int FkPriority { get; set; }

        [DisplayName("Status")]
        public int FkStatus { get; set; }
        public string Description { get; set; }
        public DateTime Appointment { get; set; }

        [DisplayFormat(NullDisplayText="null")]
        public DateTime? Departure { get; set; }

        [DisplayFormat(NullDisplayText="null")]
        public DateTime? Arrival { get; set; }

        [DisplayFormat(NullDisplayText="null")]
        public DateTime? Finished { get; set; }

        [DisplayFormat(NullDisplayText="null")]
        public string Note { get; set; }

        [DisplayName("Creation Date")]
        public DateTime Created { get; set; }

        public virtual AssignmentCategories FkCategoryNavigation { get; set; }
        public virtual Clients FkClientNavigation { get; set; }
        public virtual AssignmentPriorities FkPriorityNavigation { get; set; }
        public virtual AssignmentStatuses FkStatusNavigation { get; set; }
    }
}
