using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCTutorial.WebAPI.Utilities
{
    public static class Guard
    {
        public static void ArgumentNotNull(object argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException("Argument is null: " + argumentName);
            }
        }

        public static void ArgumentNotNullOrEmpty(string argumentValue, string argumentName)
        {
            if (string.IsNullOrEmpty(argumentValue))
            {
                throw new ArgumentNullException("Argument is null: " + argumentName);
            }
        }
    }
}