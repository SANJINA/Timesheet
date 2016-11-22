using MVCTutorial.Models;
using MVCTutorial.WebAPI.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;


namespace MVCTutorial.WebAPI.Filters
{
    public class AuthorizeTokenFilterAttribute: AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            // return base.IsAuthorized(actionContext);

            string token = "";
            if (actionContext.ControllerContext.Request.Headers.Contains("Authorization"))
            {
                token = actionContext.ControllerContext.Request.Headers.GetValues("Authorization").FirstOrDefault();

                if (!string.IsNullOrEmpty(token))
                {
                    var tokens = token.Split(' ');
                    if (tokens != null && tokens.Length > 1)
                    {
                        token = tokens[1];
                    }

                    TokenCache tokenCacheData = CacheManager.GetTokenCache(token);
                    if (tokenCacheData == null)
                    {
                        HandleUnauthorizedRequest(actionContext);
                    }
                    return true;
                }                
            }
            return false;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = new System.Net.Http.HttpResponseMessage() { StatusCode = HttpStatusCode.Unauthorized, ReasonPhrase = "Invalid token." };
        }


    }
}