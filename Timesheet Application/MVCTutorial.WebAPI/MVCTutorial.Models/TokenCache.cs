using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCTutorial.Models
{
    public class TokenCache
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTime LastAccessedOn { get; set; }
    }
}
