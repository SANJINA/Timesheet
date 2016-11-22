using MVCTutorial.DAL.ADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCTutorial.WebAPI.Manager
{
    public class PasswordManager
    {
        private static IDatabase _db = new Database();


        internal static int Put(string password)
        {
            try
            {
                /* TODO: Add password to server.  */
                //string sql = _db.getStatement(MSSQLConstants.User_Get);
                //DbParameter[] param = { };
                //DataSet ds = _db.ExecuteDataSet(sql, param, MSSQLConstants.User_Get);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

    }
}