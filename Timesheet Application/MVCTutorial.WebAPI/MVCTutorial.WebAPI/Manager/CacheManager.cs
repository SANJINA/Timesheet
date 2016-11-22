using MVCTutorial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCTutorial.WebAPI.Manager
{
    public class CacheManager
    {
        static Dictionary<string, TokenCache> userCache;
        static Dictionary<string, TokenCache> tokenCache;

        static CacheManager()
        {
            userCache = new Dictionary<string, TokenCache>();
            tokenCache = new Dictionary<string, TokenCache>();
        }
        
        /* Return the UserInfo for the requested userid if any.  Else return null. */
        internal static TokenCache GetUserCache(String userid)
        {
            if (userCache.ContainsKey(userid))
            {
                return userCache[userid];
            }
            else
                return null;
        }

        /* Return the UserInfo for the requested token if any.  Else return null. */
        internal static TokenCache GetTokenCache(String token)
        {
            if (tokenCache.ContainsKey(token))
            {
                return ValidateTokenTime(token);
            }
            else
                return null;
        }

        /* Add the user to the cache. */
        internal static void PostUserCache(String userid, TokenCache userData)
        {
            /* Ensure whether this contains data.
             * If not continue and add the info. */
            if (!userCache.ContainsKey(userid))
            {
                /* Add the info to the cache. */
                userCache.Add(userid, userData);
                //PostTokenCache(userid, userData);
                PostTokenCache(userData.Token, userData);
                return;
            }

            if (userCache.ContainsKey(userid))
            {
                userCache.Remove(userid);
            }
            userCache.Add(userid, userData);

            //PostTokenCache(userid, userData);
            PostTokenCache(userData.Token, userData);
        }

        internal static void ClearCache(string userid, string token)
        {

            if (userCache.ContainsKey(userid))
            {
                userCache.Remove(userid);
            }
            if (tokenCache.ContainsKey(token))
            {
                tokenCache.Remove(token);
            }
            
           
        }

        /* Add the token to the cache. */
        private static void PostTokenCache(String token, TokenCache userData)
        {
            /* Ensure whether this contains data.
             * If not continue and add the info. */
            if (!tokenCache.ContainsKey(token))
            {
                /* Add the info to the cache. */
                tokenCache.Add(token, userData);
                return;
            }

            if (tokenCache.ContainsKey(token))
            {
                tokenCache.Remove(token);
            }
            tokenCache.Add(token, userData);
        }

        /* Update the DateTime for token cache. */
        private static void PutTokenCache(String token)
        {
            ((TokenCache)tokenCache[token]).LastAccessedOn = DateTime.Now;
        }

        /* Validate the DateTime for token cache. */
        private static TokenCache ValidateTokenTime(String token)
        {
            if (tokenCache.ContainsKey(token))
            {
                TokenCache data = (TokenCache)tokenCache[token];
                DateTime then = ((TokenCache)tokenCache[token]).LastAccessedOn;
                DateTime now = DateTime.Now;

                /* Check whether the token is expired? */
                if ((now - then).TotalHours >= Constants.TokenExpiryHours)
                {
                    /* Clear the cache */
                    tokenCache.Remove(token);
                    userCache.Remove(data.UserId);
                    return null;
                }

                PutTokenCache(token);

                return (TokenCache)tokenCache[token];
                
            }
            return null;
        }



    }
}