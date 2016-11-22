using MVCTutorial.Models;
using MVCTutorial.WebAPI.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVCTutorial.WebAPI.Controllers
{
    public class SecurityQuestionController : ApiController
    {

        [Route("SecurityQuestion/Get")]
        public HttpResponseMessage Get()
        {
            try
            {
                string token = " ";

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

                    /* Validate token */
                    TokenCache data = CacheManager.GetTokenCache(token);      

                    if (data != null)
                    {

                        List<SecurityQuestion> dbQuestionList = QuestionManager.GetQuestions();

                        if (dbQuestionList == null)
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.User.Exception);
                        else if (dbQuestionList.Count == 0)
                            return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.User.NoData);
                        else
                            //return Request.CreateResponse(JsonConvert.SerializeObject(emp));
                            return Request.CreateResponse(dbQuestionList);
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


        [Route("SecurityQuestion/CreateQuestion")]
        public HttpResponseMessage Post(SecurityQuestion securityQuestion)
        {
            try
            {
                string email = GetUserId();
                int response = QuestionManager.AddQuestion(securityQuestion, email);
                if (response > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, Constants.Error.SecurityQuestions.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.SecurityQuestions.NotAdded);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.SecurityQuestions.Exception);
            }
        }


        [Route("SecurityQuestion/UpdateQuestion")]
        public HttpResponseMessage Put(SecurityQuestion sq)
        {
            if (sq.QuestionId == 0 || sq == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, Constants.Error.User.NoData);
            }

            string email = GetUserId();
            int response = QuestionManager.UpdateQuestion(sq, email);
            if (response > 0)
            {
                return Request.CreateResponse("Success");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.SecurityQuestions.Exception);
            }
        }

        private string GetUserId()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();  
              
            string[] tokens = token.Split(' ');
            if (tokens != null)
            {
                token = tokens[1];
            }                       
            TokenCache data = CacheManager.GetTokenCache(token);
            return (data.UserId);        
        }






    }
}
