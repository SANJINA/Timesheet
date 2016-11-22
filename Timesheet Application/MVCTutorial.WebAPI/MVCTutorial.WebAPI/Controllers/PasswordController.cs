using MVCTutorial.Models;
using MVCTutorial.WebAPI.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MVCTutorial.WebAPI.Controllers
{
    public class PasswordController : ApiController
    {
        [Route("ResetPassword")]
        public HttpResponseMessage Post(string token)
        {
            try
            {
                /* Validate token */
                TokenCache data = CacheManager.GetTokenCache(token);
                if (data != null)
                {
                    int val = CreatePassword();

                    if(val == -1)
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.Password.SaveFailed);

                    return Request.CreateResponse(Constants.Success);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.Token.InValid);
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.Login.Exception);
            }
        }

        //[Route("ForgotPassword")]
        //public HttpResponseMessage Get(string userid)
        //{
        //    try
        //    {
        //        /* Validate userid against database. */
        //        Employee data = UserManager.Get(userid);
        //        if (data != null)
        //        {
        //            List<Question> questions =  QuestionManager.GetQuestions(data.Email);

        //            if (questions == null)
        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.SecurityQuestions.Exception);
        //            else if (questions.Count == 0)
        //                return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.SecurityQuestions.NoData_User);
        //            return Request.CreateResponse(questions);
        //        }
        //        else
        //        {
        //            return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.User.InValid);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.Password.Exception);
        //    }
        //}

        //[Route("ForgotPassword")]
        //public HttpResponseMessage Post(string userid, List<SecurityQuestion> questions)
        //{
        //    if(questions == null)
        //        return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.SecurityQuestions.NoSource);

        //    /* Validate userid against database. */
        //    Employee data = UserManager.Get(userid);
        //    if (data != null)
        //    {
        //        /* TODO: Validate security question/answer against database. */


        //        int val = CreatePassword();

        //        if (val == -1)
        //            return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.Password.SaveFailed);

        //        return Request.CreateResponse(Constants.Success);
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.User.InValid);
        //    }
        //}

        private int CreatePassword()
        {
            string newPassword = Common.CreateToken();

            /* TODO: Encrypt the password*/
            string encyptedNewPassword = newPassword;

            return PasswordManager.Put(encyptedNewPassword);
        }
    }
}