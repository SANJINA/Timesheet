using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCTutorial.Models
{
    public class LoggedInInfo
    {
        public Employee Employee { get; set; }

        public string Token { get; set; }

        public DateTime LastLoggedIn { get; set; }

        public bool  NoUser { get; set; }
    }
}
