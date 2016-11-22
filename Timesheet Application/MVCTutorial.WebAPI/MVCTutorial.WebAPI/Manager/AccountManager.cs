using MVCTutorial.DAL.ADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCTutorial.Models;
using System.Data.Common;
using System.Data;

namespace MVCTutorial.WebAPI.Manager
{
    public class AccountManager
    {
        private static IDatabase _db = new Database();              

        private static List<Employee> ProcessInfo(DataSet ds)
        {
            return ds.Tables[0].AsEnumerable().Select(x => new Employee
            {
                EmployeeId = x.Field<int>("EmployeeId"),
                FirstName = x.Field<string>("FirstName"),
                LastName = x.Field<string>("LastName"),
                Email = x.Field<string>("Email"),
                Password = x.Field<string>("EncryptedPassword"),
                IsActive = x.Field<bool>("IsActive"),
                IsDeleted = x.Field<bool>("IsDeleted"),
                IsLocked = x.Field<bool>("IsLocked"),
                IsRegistered = x.Field<bool>("IsRegistered")
            }).ToList();
        }

        private static int InsertWrongPassword(string userName)
        {
            try
            {
                string sql = _db.getStatement(MSSQLConstants.Insert_WrongPassword);
                DbParameter[] param = { _db.getDbParameter("?UserName",userName),
                                        _db.getDbParameter("?LastUpdatedBy",userName),
                                        _db.getDbParameter("?LastUpdatedDate",DateTime.Now) };

                int result = _db.ExecuteNonQuery(sql, param);

                return result;
            }
            catch
            {
                return -1;
            }
        }

        private static int CountWrongPassword(string userName)
        {
            try
            {
                string sql = _db.getStatement(MSSQLConstants.Count_WrongPassword);
                DbParameter[] param = { _db.getDbParameter("?Username",userName)
                                        };

                int result = _db.ExecuteScalarInt(sql, param);

                return result;
            }
            catch
            {
                return -1;
            }

        }

        private static int UpdateUserLocked(string userName)
        {
            try
            {
                string sql = _db.getStatement(MSSQLConstants.Update_UserLocked);
                DbParameter[] param = { _db.getDbParameter("?UserName", userName) };
                int result = _db.ExecuteNonQuery(sql, param);
                return result;
            }
            catch
            {
                return -1;
            }
        }

        private static int AddRegistration(string email, SecurityQuestion securityQuestion)
        {
            try
            {
                string sql = _db.getStatement(MSSQLConstants.RegisterNewUser);
                DbParameter[] param = { _db.getDbParameter("?Email",email),
                                    _db.getDbParameter("?QuestionId",securityQuestion.QuestionId),
                                    _db.getDbParameter("?Answer",securityQuestion.Answer),
                                    _db.getDbParameter("?LastUpdatedBy",email),
                                    _db.getDbParameter("?LastUpdatedDate",DateTime.Now)};
                int result = _db.ExecuteNonQuery(sql, param);

                return result;
            }
            catch (Exception ex)
            {
                //ignore exception
                return -1;
            }
        }

        internal static void AddWrongPassword(string userName)
        {
            try
            {
                int result = InsertWrongPassword(userName);
                if (result == 1)
                {
                    int lockCount = CountWrongPassword(userName);
                    if (lockCount >= Constants.CountLimit)
                    {
                        int UserLocked = UpdateUserLocked(userName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static void DeleteWrongPassword(int employeeId)
        {
            try
            {
                string sql = _db.getStatement(MSSQLConstants.Delete_WrongPassword);
                DbParameter[] param = { _db.getDbParameter("?EmployeeId", employeeId) };
                int result = _db.ExecuteNonQuery(sql, param);
                //return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static IList<string> GetUserRoles(string username)
        {
            IList<string> roles = new List<string>();
            try
            {
                /* TODO - */
                string sql = _db.getStatement(MSSQLConstants.Get_User_Roles);
                DbParameter[] param = { _db.getDbParameter("?username", username) };
                DataSet ds = _db.ExecuteDataSet(sql, param, MSSQLConstants.Get_User_Roles);

                if (ds != null)
                {
                    return ds.Tables[0].AsEnumerable().Select(x => x.Field<string>("Description")).ToList();
                }
            }
            catch (Exception ex)
            {
                // TODO: handle exception
                return null;
            }

            return null;
        }

        internal static int CheckTempPassword(int empId, string tempPswd)
        {
            try
            {
                string sql = _db.getStatement(MSSQLConstants.CheckTempPassword);
                DbParameter[] param = { _db.getDbParameter("?EmployeeId", empId), _db.getDbParameter("?EncryptedPassword",tempPswd)};
                int x = _db.ExecuteScalarInt(sql, param);
                return x > 0 ? x : -1;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        internal static int InsertRegistration(Registration newUser)
        {
            try
            {
                int x = 0;
                foreach (SecurityQuestion question in newUser.SecurityQuestions)
                {
                    x = AddRegistration(newUser.Email, question);
                }
                return x;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        internal static int UpdateUserPassword(int employeeId, string newPassword, bool isRegistration)
        {
            try
            {
                string sql = _db.getStatement(MSSQLConstants.UserPassword_Update);

                if (isRegistration)
                {
                    sql += _db.getStatement(MSSQLConstants.ClearTemporaryPassword_Update);
                }

                DbParameter[] param = { _db.getDbParameter("?EmployeeId",employeeId),
                                        _db.getDbParameter("?EncryptedPassword",newPassword) };

                int result = _db.ExecuteNonQuery(sql, param);

                return result;
            }
            catch
            {
                return -1;
            }
        }

        
        internal static string IsValidUser(string userName)
        {
            try
            {
                string sql = _db.getStatement(MSSQLConstants.CheckValidUser);
                DbParameter[] param = { _db.getDbParameter("?UserName", userName) };
                DataSet ds = _db.ExecuteDataSet(sql, param, MSSQLConstants.CheckValidUser);
                        
                if (ds != null && ds.Tables[0] != null)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (dr != null)
                    {
                        bool IsActive = (bool)dr["IsActive"];
                        bool IsLocked = (bool)dr["IsLocked"];
                        bool IsDeleted = (bool)dr["IsDeleted"];

                        if (!IsActive)
                        {
                            return Constants.Error.Login.InActive;
                        }
                        else if (IsLocked)
                        {
                            return Constants.Error.Login.IsLocked; ;
                        }
                        else if (IsDeleted)
                        {
                            return Constants.Error.Login.IsDeleted; ;
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                }
                return Constants.Error.Login.NoUser;           

            }
            catch (Exception ex)
            {
                return Constants.Error.Login.Exception;
            }
        }

        internal static User ValidateUser(LoginCredential logincredential)
        {
            try
            {
                string sql = _db.getStatement(MSSQLConstants.ValidateUser);
                DbParameter[] param = { _db.getDbParameter("?UserName", logincredential.UserName),
                                        _db.getDbParameter("?Password", logincredential.Password)};
                DataSet ds = _db.ExecuteDataSet(sql, param, MSSQLConstants.ValidateUser);

                User loggedInUser = ds.Tables[0].AsEnumerable().Select(x => new User
                {
                    EmployeeId = x.Field<int>("EmployeeId"),
                    FirstName = x.Field<string>("FirstName"),
                    LastName = x.Field<string>("LastName"),                    
                    ShouldChangePassword = x.Field<bool>("ShouldChangePassword"),                    
                    IsRegistered = x.Field<bool>("IsRegistered")
                }).FirstOrDefault();

                return loggedInUser;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static Employee GetEmployeeByUsername(string userName)
        {
            try
            {
                string sql = _db.getStatement(MSSQLConstants.GetEmployeeByUsername);
                DbParameter[] param = { _db.getDbParameter("?username", userName) };
                DataSet ds = _db.ExecuteDataSet(sql, param, MSSQLConstants.GetEmployeeByUsername);
                return ProcessInfo(ds).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }





    }
}