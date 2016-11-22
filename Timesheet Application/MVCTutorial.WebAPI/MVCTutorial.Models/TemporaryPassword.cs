using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCTutorial.Models
{
    public class TemporaryPassword
    {
        public int EmployeeId { get; set; }

        public string EncryptedPassword { get; set; }

        public string Password { get; set; }
    }
}
