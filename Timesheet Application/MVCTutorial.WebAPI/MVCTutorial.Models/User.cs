using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCTutorial.Models
{
    public class User
    {
        public int EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool IsRegistered { get; set; }

        public bool ShouldChangePassword { get; set; }

        public IList<string> Roles { get; set; }

        public IList<SecurityQuestion> SecurityQuestions { get; set; }

        public string Token { get; set; }

        public DateTime LastLoggedIn { get; set; }
    }
}
