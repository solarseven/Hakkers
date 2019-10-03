using System;
using System.Collections.Generic;

namespace Hakkers.Models
{
    public partial class AssignmentPriorities
    {
        public AssignmentPriorities()
        {
            Assignments = new HashSet<Assignments>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Assignments> Assignments { get; set; }
    }
}
