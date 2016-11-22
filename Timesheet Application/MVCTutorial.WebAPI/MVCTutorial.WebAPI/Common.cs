using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCTutorial.WebAPI
{
    public class Common
    {
        internal static String CreateToken()
        {
            String guid = Guid.NewGuid().ToString();
            return guid;
        }

        internal static String CreatePassword()
        {
            String guid = Guid.NewGuid().ToString();
            return guid;
        }

    }
}