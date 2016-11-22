using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCTutorial.Models
{
    public class UserSessionInfo
    {
        public int EmployeeId { get; set; }

        public string  UserEmail { get; set; }

        public string  FullName { get; set; }

        public string Token { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsManager { get; set; }

        public bool IsNormalUser { get; set; }
    }
}
