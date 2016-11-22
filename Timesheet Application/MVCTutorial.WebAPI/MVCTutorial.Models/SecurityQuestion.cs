using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCTutorial.Models
{
    public class SecurityQuestion
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        public bool CanUpdate { get; set; }

    }
}
