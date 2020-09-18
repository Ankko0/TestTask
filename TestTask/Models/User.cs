using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string GivenName { get; set; }

        public string Surname { get; set; }

        public string MiddleName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string PasswordHash { get; set; }

    }
}
