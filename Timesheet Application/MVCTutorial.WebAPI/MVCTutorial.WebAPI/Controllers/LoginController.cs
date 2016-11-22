using MVCTutorial.Models;
using MVCTutorial.WebAPI.Manager;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MVCTutorial.WebAPI.Controllers
{
    public class LoginController : ApiController
    {
    //    [Route("Login")]
        //public HttpResponseMessage Get(string userid, string password)
        //{
        //    string enc = Cryptography.Encryption("test");
        //    string dec = Cryptography.Decryption(enc);

        //    try
        //    {
        //        /* TODO: Validate user */
        //        if (1 == 1)
        //        {
        //            TokenCache data = new TokenCache()
        //            {
        //                UserId = userid,
        //                Token = Common.CreateToken(),
        //                LastAccessedOn = DateTime.Now
        //            };
        //            CacheManager.PostUserCache(userid, data);

        //            return Request.CreateResponse(data.Token);
        //        }
        //        else
        //        {
        //            return null;
        //            /* If userid not matching */
        //            return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.Login.InValid);
        //            /* If password not matching */
        //            return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.Login.WrongPassword);
        //            /* If any other result or exception */
        //            return Request.CreateResponse(HttpStatusCode.Unauthorized, Constants.Error.Login.Exception);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.Error.Login.Exception);
        //    }            
        //}
    }
}