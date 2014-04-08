using AzureVideoStreaming.Core;
using AzureVideoStreaming.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace AzureVideoStreaming.WebACS.Helpers
{
    public static class IdentityHelper
    {
        public static string GetUserToken()
        {
            var identity = Thread.CurrentPrincipal.Identity as System.Security.Claims.ClaimsIdentity;
            var claim = identity.Claims.FirstOrDefault(t => t.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            string token = null;
            if (claim != null)
            {
                token = claim.Value;
                token = token.Replace('/', '_');
            }
            return token;
        }

        public static User GetUserFromIdentity()
        {
            var userRepository = new UserRepository();
            var token = IdentityHelper.GetUserToken();
            if (token == null)
                return null;

            var user = userRepository.Get(token);
            return user;
        }

        public static bool IsUserRegistered()
        {
            var user = IdentityHelper.GetUserFromIdentity();
            if (user == null)
                return false;
            return !string.IsNullOrEmpty(user.Username);
        }
    }
}