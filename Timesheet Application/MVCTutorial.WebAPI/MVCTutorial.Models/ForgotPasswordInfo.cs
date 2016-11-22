using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCTutorial.Models
{
    public class ForgotPasswordInfo
    {
        public string Email { get; set; }

        public IList<SecurityQuestion> SecurityQuestions { get; set; }


    }
}
