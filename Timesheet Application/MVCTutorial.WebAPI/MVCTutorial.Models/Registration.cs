using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCTutorial.Models
{
    public class Registration
    {

        public Registration()
        {
            SecurityQuestions = new List<SecurityQuestion>();
        }
        public int EmployeeId { get; set; }

        public string Email { get; set; }
        public IList<SecurityQuestion> SecurityQuestions { get; set; }

        public string NewPassword { get; set; }

        public bool IsRegistration { get; set; }
    }
}
