using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCTutorial.Models
{
    public class ChangePassword
    {
        public string UserName { get; set; }
        public string NewPassword { get; set; }
        public bool IsRegistration { get; set; }
    }
}
