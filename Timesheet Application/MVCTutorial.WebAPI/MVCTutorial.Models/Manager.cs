using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCTutorial.Models
{
    public class Manager
    {
        public int Id { get; set; }
        public int ManagerId { get; set; }
        public int EmployeeId { get; set; }
    }
}
