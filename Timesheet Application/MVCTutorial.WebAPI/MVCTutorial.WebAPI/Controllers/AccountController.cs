using MVCTutorial.Models;
using MVCTutorial.WebAPI.Manager;
using MVCTutorial.WebAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVCTutorial.WebAPI.Controllers
{
    public class AccountController : ApiController
    {
        private string CreateTokenCache(string userName)
        {
            TokenCache data = new TokenCache()
            {
                UserId = userName,
                Token = Common.CreateToken(),
                LastAccessedOn = DateTime.Now
            };
            CacheManager.PostUserCache(userName, data);

            return data.Token;
        }     

        [Route("Account/PostRegistrationInfo")]
        public HttpResponseMessage SaveRegistrationInfo(Registration regInfo)
        {
            try
            {              
                if (regInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.User.NoSource);
                }

                int response = 0;
                if (regInfo.SecurityQuestions != null && regInfo.SecurityQuestions.Count > 0)
                {
                    response = AccountManager.InsertRegistration(regInfo);
                    if (response > 0)
                    {
                        try
                        {
                            response = AccountManager.UpdateUserPassword(regInfo.EmployeeId, regInfo.NewPassword, regInfo.IsRegistration);
                        }
                        catch (Exception ex)
                        {
                            // TODO: delete security questions from registraiton table
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, response);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.User.UpdatePasswordError);
                    }
                }
                else
                {
                    response = AccountManager.UpdateUserPassword(regInfo.EmployeeId, regInfo.NewPassword, regInfo.IsRegistration);
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.User.Exception);
            }
        }

        [Route("Account/ClearCache")]
        public HttpResponseMessage ClearCache()
        {
            try
            {
                string token = "";

                if (!Request.Headers.Contains("Authorization"))
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.Token.InValid);
                }

                token = Request.Headers.GetValues("Authorization").FirstOrDefault();

                if (!string.IsNullOrEmpty(token))
                {
                    string[] tokens = token.Split(' ');
                    if (tokens != null)
                    {
                        token = tokens[1];
                    }

                    TokenCache data = CacheManager.GetTokenCache(token);

                    CacheManager.ClearCache(data.UserId, token);

                    return Request.CreateResponse("Deleted");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.Token.InValid);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.User.Exception);
            }
        }

        [Route("Account/ValidateLogin")]
        public HttpResponseMessage ValidateLoginTest(LoginCredential loginCredential)
        {
            try
            {
                string isValid = AccountManager.IsValidUser(loginCredential.UserName);
                if (isValid == Constants.Error.Login.NoUser)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.Login.NoUser);
                }

                if (isValid == string.Empty)
                {
                    User loggedInUser = AccountManager.ValidateUser(loginCredential);

                    if (loggedInUser == null) //Password wrong condition
                    {
                        AccountManager.AddWrongPassword(loginCredential.UserName);
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.Login.WrongPassword);
                    }
                    else
                    {
                        AccountManager.DeleteWrongPassword(loggedInUser.EmployeeId);
                        string token = CreateTokenCache(loginCredential.UserName);
                        loggedInUser.Roles = AccountManager.GetUserRoles(loginCredential.UserName);
                        loggedInUser.Token = token;
                        loggedInUser.Email = loginCredential.UserName;

                        if (loggedInUser.ShouldChangePassword && !loggedInUser.IsRegistered)
                        {
                            loggedInUser.SecurityQuestions = QuestionManager.GetQuestions();   
                        }

                        return Request.CreateResponse(HttpStatusCode.OK, loggedInUser);
                    }
                }
                else
                {
                    if (isValid == Constants.Error.Login.InActive)
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.Login.InActive);
                    }
                   else  if (isValid == Constants.Error.Login.IsLocked)
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.Login.IsLocked);
                    }
                    else 
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.Login.IsDeleted);
                    }                    
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.Login.Exception);
            }          
        }


        //private bool IsValidUser(Employee employee)
        //{
        //    if (!employee.IsActive || employee.IsDeleted || employee.IsLocked)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        //[Route("Account/checkLoginCredential")]
        //public HttpResponseMessage ValidateLogin(LoginCredential loginCredential)
        //{
        //    Guard.ArgumentNotNull(loginCredential, "loginCredential");

        //    try
        //    {
        //        Employee employee = AccountManager.GetEmployeeByUsername(loginCredential.UserName);

        //        LoggedInInfo loggedInInfo = new LoggedInInfo();
        //        loggedInInfo.Employee = employee;

        //        if (employee == null)
        //        {
        //            loggedInInfo.NoUser = true;
        //            return Request.CreateResponse(HttpStatusCode.OK, loggedInInfo);                    
        //        }         
        //        else
        //        {
        //            bool isValid = IsValidUser(employee);
        //            if (isValid)
        //            {
        //                if (string.IsNullOrEmpty(employee.Password))
        //                {
        //                    loggedInInfo.Employee.ShouldChangePassword = true;
        //                }
        //                if (employee.IsRegistered)
        //                {
        //                    if (employee.Password == loginCredential.Password)
        //                    {
        //                        string token = CreateTokenCache(loginCredential.UserName);
        //                        employee.Roles = AccountManager.GetUserRoles(loginCredential.UserName);
        //                        loggedInInfo.Token = token;
        //                    }
        //                    else
        //                    {
        //                        loggedInInfo.Employee.IsPasswordMismatch = true;
        //                        AccountManager.AddWrongPassword(employee.EmployeeId,employee.Email);
        //                        loggedInInfo.Employee.IsLocked = true;                              
        //                    }
        //                }
        //                else
        //                {
        //                    int tempPasswordPresent = AccountManager.CheckTempPassword(employee.EmployeeId, loginCredential.Password);

        //                    if (tempPasswordPresent > 0)
        //                    {
        //                        string token = CreateTokenCache(loginCredential.UserName);
        //                        employee.Roles = new List<string>() { "NormalUser" };
        //                        employee.IsRegistered = false; //MAY NOT Be NECESSARY!
        //                        loggedInInfo.Token = token;                                
        //                    }
        //                    else
        //                    {
        //                        loggedInInfo.Employee.IsPasswordMismatch = true;
        //                        AccountManager.AddWrongPassword(employee.EmployeeId, employee.Email);
        //                        loggedInInfo.Employee.IsLocked = true;
        //                    }

        //                }
        //            }                    
        //        }
        //        return Request.CreateResponse(HttpStatusCode.OK, loggedInInfo);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.User.Exception);
        //    }
        //}


    }
}
       