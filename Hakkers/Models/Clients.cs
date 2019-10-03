using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Hakkers.Models
{
    public partial class Clients
    {
        public Clients()
        {
            Assignments = new HashSet<Assignments>();
        }

        public int Id { get; set; }

        [DisplayName("Name")]
        public string FirstName { get; set; }

        [DisplayName("Surname")]
        public string LastName { get; set; }

        [DisplayName("Phone")]
        public int TelephoneNumber { get; set; }

        public string Email { get; set; }
        public string Street { get; set; }

        [DisplayName("House Number")]
        public string HouseNumber { get; set; }

        public string Location { get; set; }

        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }


        public virtual ICollection<Assignments> Assignments { get; set; }
    }
}
