using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCTutorial.Models
{
    public class BaseModel
    {
        public string LastUpdatedBy { get; set; }

        public string LastUpdatedDate { get; set; }
    }
}
