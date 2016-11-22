using MVCTutorial.DAL.ADO;
using MVCTutorial.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MVCTutorial.WebAPI.Manager
{
    public class UserManager
    {
        private static IDatabase _db = new Database();

        internal static List<Employee> Get()
        {
            try
            {
                string sql = _db.getStatement(MSSQLConstants.User_Get);
                DbParameter[] param = { };
                DataSet ds = _db.ExecuteDataSet(sql, param, MSSQLConstants.User_Get);
                return ProcessInfo(ds);
            }
            catch
            {
                return null;
            }
        }

        internal static bool Delete( int employeeId)
        {
            try
            {
                string sql = _db.getStatement(MSSQLConstants.User_Delete);
                IDataParameter[] param = { _db.getDbParameter("?EmployeeId", employeeId) };
                int x = _db.ExecuteNonQuery(sql, param);

                return x > 0 ? true : false;
            }
            catch
            {
                return false;
            }
        }

        internal static int ResetPassword(string email,int employeeId, string password)
        {
            try
            {
                string sql = _db.getStatement(MSSQLConstants.Clear_UserPassword);
                sql += _db.getStatement(MSSQLConstants.TemporaryPassword_Update);
                IDataParameter[] param = { _db.getDbParameter("?EmployeeId", employeeId),
                                           _db.getDbParameter("?EncryptedPassword",password),
                                           _db.getDbParameter("?LastUpdatedBy", email) };
                int result = _db.ExecuteNonQuery(sql, param);

                return result;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        //internal static Employee Get(string userid)
        //{
        //    try
        //    {                
        //        string sql = _db.getStatement(MSSQLConstants.User_Get);
        //        DbParameter[] param = {( _db.getDbParameter("?Email", userid) )};
        //        DataSet ds = _db.ExecuteDataSet(sql, param, MSSQLConstants.User_Get);
        //        if (ds != null)
        //        {
        //            //return ds.Tables[0].AsEnumerable().Select(x => new Employee
        //            //{
        //            //    EmployeeId = x.Field<int>("EmployeeId"),
        //            //    //Question = x.Field<string>("Question")
        //            //    //Answer = x.Field<string>("Answer")
        //            //}).ToList();
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        internal static int Update(Employee emp) {
            try
            {
                string sql = _db.getStatement(MSSQLConstants.User_Update);

                DbParameter[] param = {_db.getDbParameter("?FirstName",emp.FirstName), _db.getDbParameter("?LastName", emp.LastName), _db.getDbParameter("?Email", emp.Email), _db.getDbParameter("?Location", emp.Location), _db.getDbParameter("?EmployeeId", emp.EmployeeId) };
                int result =(int)_db.ExecuteNonQuery(sql, param);
                return result;
            }
            catch (Exception ex)
            {
                return -1;
            }
            }

        internal static int Add(Employee emp)
        {
            try
            {
                string sql = _db.getStatement(MSSQLConstants.User_Add);
                //sql += _db.getStatement(MSSQLConstants.UserRole_Add);
                DbParameter[] param = { _db.getDbParameter("?FirstName", emp.FirstName),
                    _db.getDbParameter("?LastName", emp.LastName),
                    _db.getDbParameter("?Email", emp.Email),
                    _db.getDbParameter("?DOB", emp.DOB),
                    _db.getDbParameter("?EncryptedPassword",string.Empty),
                    _db.getDbParameter("?Location", emp.Location),
                    _db.getDbParameter("?IsActive", emp.IsActive),
                    _db.getDbParameter("?IsRegistered", "false"),
                    _db.getDbParameter("?IsDeleted", emp.IsDeleted),
                    _db.getDbParameter("?IsLocked", emp.IsLocked),
                    _db.getDbParameter("?LastUpdatedBy", emp.LastUpdatedBy),
                    _db.getDbParameter("?LastUpdatedDate", DateTime.Now),
                    _db.getDbParameter("?EmployeeId", emp.EmployeeId),
                    _db.getDbParameter("?RoleId",3) };
                    //_db.getDbParameter("?Role","NormalUser")
                //int result = _db.ExecuteNonQuery(sql, param);

                int result = _db.ExecuteScalarInt(sql, param);

                return result;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        internal static int AddTempPassword(Employee employee,string tempPassword)
        {
            try
            {
                string sql = _db.getStatement(MSSQLConstants.TemporaryPassword_Add);
                DbParameter[] param = { _db.getDbParameter("?EmployeeId", employee.EmployeeId),
                    _db.getDbParameter("?EncryptedPassword", tempPassword),
                    _db.getDbParameter("?LastUpdatedBy", employee.LastUpdatedBy),
                    _db.getDbParameter("?LastUpdatedDate", DateTime.Now)};

                int result = _db.ExecuteNonQuery(sql, param);

                return result > 0 ? result : -1;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        private static List<Employee> ProcessInfo(DataSet ds)
        {
            return ds.Tables[0].AsEnumerable().Select(x => new Employee
            {
                EmployeeId = x.Field<int>("EmployeeId"),
                FirstName = x.Field<string>("FirstName"),
                LastName = x.Field<string>("LastName"),
                Email = x.Field<string>("Email"),
                Password = "",
                DOB = DateTime.Now,
                Location = x.Field<string>("Location"),
                IsActive = true,
                IsDeleted = false,
                IsLocked = false,
                IsRegistered = true,
                FirstTimeLogin = false
            }).ToList();
        }
    }
}