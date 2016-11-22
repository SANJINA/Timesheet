using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MVCTutorial.DAL.ADO
{
    public class Database : MSSQLDatabase
    {
        private static readonly string connstr = ConfigurationManager.ConnectionStrings["MSSQLConnection"].ConnectionString;

        public Database() : base(connstr) { }
    }
}
