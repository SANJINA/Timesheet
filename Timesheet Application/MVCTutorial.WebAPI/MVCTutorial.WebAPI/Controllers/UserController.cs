using MVCTutorial.Models;
using MVCTutorial.WebAPI.Filters;
using MVCTutorial.WebAPI.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVCTutorial.WebAPI.Controllers
{
    public class UserController : ApiController
    {
        [AuthorizeTokenFilter]
        [Route("User")]
        public HttpResponseMessage Get()
        {
            try
            {
                List<Employee> empList = UserManager.Get();

                if (empList == null)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.User.Exception);
                }
                else if (empList.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.User.NoData);
                }
                else
                {
                    //return Request.CreateResponse(JsonConvert.SerializeObject(emp));
                    return Request.CreateResponse(empList);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.User.Exception);
            }
        }


        [Route("User/create")]
        public HttpResponseMessage Post(Employee employee)
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
                    //token = token.Substring(token.IndexOf("Bearer "));
                    string[] tokens = token.Split(' ');
                    if (tokens != null)
                    {
                        token = tokens[1];
                    }

                    /* Validate token */
                    TokenCache data = CacheManager.GetTokenCache(token);

                    if (data != null && employee == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.User.NoSource);
                    }

                    if (data != null)
                    {
                        employee.LastUpdatedBy = data.UserId;
                        int response = UserManager.Add(employee);

                        if (response > 0)
                        {
                            // new password is generated (create a GUID and encrypt it and save it in Temporary Password Table)
                            employee.EmployeeId = response;
                            string tempPassword = Common.CreatePassword();
                            string encryptedPassword = Cryptography.Encryption(tempPassword);

                            int tempPasswordAdded = UserManager.AddTempPassword(employee, encryptedPassword);

                            TemporaryPassword tempPwd = new TemporaryPassword()
                            {
                                EmployeeId = employee.EmployeeId,
                                Password = tempPassword
                            };

                            if (response > 0)
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, tempPwd);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.User.Exception);
                            }                                                    
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.User.Exception);
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.Token.InValid);
                    }
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

        [Route("User/update")]
        public HttpResponseMessage Put(Employee emp)
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
                    //token = token.Substring(token.IndexOf("Bearer "));
                    string[] tokens = token.Split(' ');
                    if (tokens != null)
                    {
                        token = tokens[1];
                    }
                    /* Validate token */
                    TokenCache data = CacheManager.GetTokenCache(token);

                    if (data != null)
                    {
                        if (emp.EmployeeId == 0 || emp == null)
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound, Constants.Error.User.NoData);
                        }

                        int response = UserManager.Update(emp);
                        if (response > 0)
                        {
                            return Request.CreateResponse("Success");
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.User.Exception);
                        }


                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.Token.InValid);
                    }
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
         
        //[AuthorizeTokenFilter]
        [Route("User/delete/{employeeId}")]
        public HttpResponseMessage Delete(int employeeId)
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
                    //token = token.Substring(token.IndexOf("Bearer "));
                    string[] tokens = token.Split(' ');
                    if (tokens != null)
                    {
                        token = tokens[1];
                    }
                    /* Validate token */
                    TokenCache data = CacheManager.GetTokenCache(token);
                    if (data != null)
                    {
                        if (employeeId == 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.User.NoSource);

                        }
                        bool result = UserManager.Delete(employeeId);

                        return Request.CreateResponse(result);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.Token.InValid);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.Token.InValid);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,Constants.Error.User.NotDeleted);
            }
        }

        [AuthorizeTokenFilter]
        [Route("User/resetPassword")]
        public HttpResponseMessage ResetPassword(ResetPassword resetPasswordInfo)
        {
            try
            {
                string tempPassword = Common.CreatePassword();
                string encryptedPassword = Cryptography.Encryption(tempPassword);
                int response = UserManager.ResetPassword(resetPasswordInfo.LastUpdatedBy,resetPasswordInfo.EmployeeId,encryptedPassword);
                if (response > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.PasswordCreation.PasswordReset);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.User.NoPasswordReset);
                }
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.User.NoPasswordReset);
            }


        }

        [AllowAnonymous]
        [Route("User/getquestions")]
        public HttpResponseMessage ForgotPassword(ForgotPasswordInfo userInfo)
        {
            try
            {
                string isValid = AccountManager.IsValidUser(userInfo.Email);
                if (isValid == Constants.Error.Login.NoUser)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.Login.NoUser);
                }

                if (isValid == string.Empty)
                {
                    List<SecurityQuestion> userSQlist = QuestionManager.GetQuestions(userInfo.Email);
                    if (userSQlist == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.User.Exception);
                    }
                    else
                    {                        
                        return Request.CreateResponse(userSQlist);
                    }
                }
                else if (isValid == Constants.Error.Login.InActive) {return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.Login.InActive); }
                else if (isValid == Constants.Error.Login.IsLocked) {return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.Login.IsLocked); }
                else { return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.Login.IsDeleted); }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.User.Exception);
            }
        }

        [AllowAnonymous]
        [Route("User/VerifyUserAnswers")]
        public HttpResponseMessage VerifyUserAnswers(ForgotPasswordInfo userInfo)
        {
            try
            {
                int ansVerified = QuestionManager.VerifyUserAnswers(userInfo);
              
                    if (ansVerified > 0)
                    {
                        string password = Common.CreatePassword();
                        string tempPassword = Cryptography.Encryption(password);

                        Employee emp = AccountManager.GetEmployeeByUsername(userInfo.Email);

                        int response = UserManager.ResetPassword(userInfo.Email, emp.EmployeeId, tempPassword);
                            if (response > 0)
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, tempPassword);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.User.NoPasswordReset);
                            }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.User.AnswerNotVerified);
                    }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.User.AnswerNotVerified);
            }
        }
    }
}

