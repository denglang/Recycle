using iowa.entaa.client.api;
using iowa.entaa.session.config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;

namespace Recycle.Code
{
    public static class Security
    {
        private static ENTAAConfigSectionHandler _entaa;

        /// <summary>
        /// Gets the Enterprise A&amp;A configuration object
        /// </summary>
        public static ENTAAConfigSectionHandler Entaa => _entaa ?? (_entaa = WebConfigurationManager.GetSection("entaa") as ENTAAConfigSectionHandler);

        /// <summary>
        /// Gets the administrator login ID for managing Enterprise A&amp;A
        /// </summary>
        public static string EntaaAdminID => WebConfigurationManager.AppSettings["AdminID"];

        /// <summary>
        /// Gets the administrator login password for managing Enterprise A&amp;A
        /// </summary>
        public static string EntaaAdminPW => WebConfigurationManager.AppSettings["AdminPW"];

        /// <summary>
        /// Gets the name of the cookie used by A&amp;A
        /// </summary>
        public static string EntaaCookieName => "tokenId";

        /// <summary>
        /// Gets the name of the cookie used by forms authentication
        /// </summary>
        public static string FormsAuthCookieName => FormsAuthentication.FormsCookieName;

        /// <summary>
        /// Gets the e-mail domain for DNR internal users
        /// </summary>
        public static string DNRInternalEmailDomain => WebConfigurationManager.AppSettings["DNRInternalEmailDomain"];

        /// <summary>
        /// Expires a cookie to clear it from subsequent requests.
        /// </summary>
        /// <param name="cookie">The cookie to expire.</param>
        /// <param name="response">The HTTP response stream.</param>
        public static void Expire(this HttpCookie cookie, HttpResponseBase response)
        {
            if (cookie != null)
            {
                response.Cookies.Add(new HttpCookie(cookie.Name) { Expires = DateTime.Now.AddDays(-1) });
            }
        }

        /// <summary>
        /// Expires a cookie to clear it from subsequent requests.
        /// </summary>
        /// <param name="cookie">The cookie to expire.</param>
        /// <param name="response">The HTTP response stream.</param>
        public static void Expire(this HttpCookie cookie, HttpResponse response)
        {
            if (cookie != null)
            {
                response.Cookies.Add(new HttpCookie(cookie.Name) { Expires = DateTime.Now.AddDays(-1) });
            }
        }

        /// <summary>
        /// Specifies whether the current user is logged in or bypassing.
        /// </summary>
        /// <param name="user">The A&amp;A user.</param>
        /// <returns>True if the user is logged in or bypassing, otherwise false.</returns>
        public static bool IsLoggedIn(this AAUserInterface user)
        {
            return !string.IsNullOrEmpty(user.GetId());
        }

        /// <summary>
        /// Gets the full name of the current logged in user.
        /// </summary>
        /// <param name="user">The A&amp;A user.</param>
        /// <returns>The full name of the current logged in user.</returns>
        public static string FullName(this AAUserInterface user)
        {
            if (Security.Entaa.Bypass)
            {
                return Security.Entaa.BypassFname + " " + Security.Entaa.BypassLname;
            }

            if (user == null)
            {
                return string.Empty;
            }

            return user.FirstName + " " + user.LastName;
        }

        /// <summary>
        /// Gets the user ID of the given A&amp;A user.
        /// </summary>
        /// <param name="user">The A&amp;A user.</param>
        /// <returns>The user ID of the current logged in user.</returns>
        public static string GetId(this AAUserInterface user)
        {
            return Entaa.Bypass ? Entaa.BypassId : user?.Id ?? string.Empty;
        }

        /// <summary>
        /// Gets the list of security roles of the given A&amp;A user.
        /// </summary>
        /// <param name="user">The A&amp;A user.</param>
        /// <returns>The list of security roles of the current logged in user.</returns>
        public static IEnumerable<string> GetRoles(this AAUserInterface user)
        {
            var roles = new List<string>();

            //Load from bypass or user (if either exists)
            if (Entaa.Bypass)
            {
                //Get from Entaa configuration section
                roles = Entaa.BypassPrivileges.Split(',').Select(GetRole).ToList();
            }
            else if (user != null)
            {
                //First get roles from A&A user
                roles = user.Privileges.Select(p => Security.GetRole(p.PrivilegeCode)).ToList();

                //Query database to find any extra data about the role and user
                //
            }

            //Return roles
            return roles.Where(r => r != null);
        }

        /// <summary>
        /// Loads the permissions of the user on the given HTTP context.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>A list of permissions</returns>
        public static IEnumerable<int> GetPermissions(this HttpContextBase context)
        {
            //Get the authorization cookie or build one if it's not found
            HttpCookie authCookie = context.Request.Cookies[Security.FormsAuthCookieName] ?? context.BuildAuthorizationCookie();

            //If an authorization cookie didn't get built, send back
            // no permissions
            if (authCookie == null)
            {
                return new List<int>();
            }

            //Decrypt the cookie to get the authorization ticket
            FormsAuthenticationTicket authTicket = null;
            try
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch
            {
                // ignored - if there is an error decrypting, we'll expire the
                // cookie and force a re-login.
            }

            //If we couldn't get the authorization ticket out of the cookie,
            // expire the cookie and send back no permissions
            if (authTicket == null)
            {
                authCookie.Expire(context.Response);
                return new List<int>();
            }

            //Extract permissions from the authorization ticket and return
            return authTicket
                .UserData
                .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .Cast<int>();
        }

        /// <summary>
        /// Builds a cookie containing authorization information (permissions) for the user.
        /// </summary>
        /// <param name="httpContext">The current HTTP context containing the user, request, and response.</param>
        /// <returns>An HttpCookie used for authorization.</returns>
        private static HttpCookie BuildAuthorizationCookie(this HttpContextBase httpContext)
        {
            AAUserInterface user = httpContext.User.Identity as AAUserInterface;
            string userId = user.GetId();
            HttpResponseBase response = httpContext.Response;

            // Add roles to the cookie
            List<string> rolesList = new List<string>();
            foreach (var Privileges in user.Privileges)
            {
                rolesList.Add(Privileges.PrivilegeCode);
            }
            string roles = string.Join(",", rolesList.ToArray());

            //If no user id by now (not bypassing and not logged in),
            // don't create the cookie
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            //Setup forms auth cookie with permissions
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, userId, DateTime.Now, DateTime.Now.AddMinutes(20), false, string.Join(",", roles));
            HttpCookie authCookie = new HttpCookie(Security.FormsAuthCookieName, FormsAuthentication.Encrypt(authTicket));
            authCookie.Expires = DateTime.Now.AddMinutes(20);
            response.Cookies.Add(authCookie);

            //Return cookie
            return authCookie;
        }

        /// <summary>
        /// Gets a <see cref="Role"/> from a role name.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <returns>The <see cref="Role"/> having the given name.</returns>
        private static string GetRole(string roleName)
        {
            return ""; // Security.Roles.SingleOrDefault(role => role.CodeName == roleName);
        }

        ///// <summary>
        ///// Gets a <see cref="Role"/> from a role identifier.
        ///// </summary>
        ///// <param name="roleId">The enum identifier of the role.</param>
        ///// <returns>The <see cref="Role"/> having the given identifier.</returns>
        //private static Role GetRole(RoleEnum roleId)
        //{
        //    return Security.Roles.SingleOrDefault(role => role.RoleID == (int)roleId);
        //}
    }
}