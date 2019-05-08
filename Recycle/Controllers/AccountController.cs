using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using iowa.entaa.session.config;
using iowa.entaa.client;

namespace Recycle.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult AAHandler()
        {
            try
            {
                //After the user returns with the token, redirect them to the menu and lose the token
                if (HttpContext.Request["tokenId"] != null)
                {
                    string token = HttpContext.Request["tokenId"];
                    ENTAAConfigSectionHandler entaa = WebConfigurationManager.GetSection(@"entaa") as ENTAAConfigSectionHandler;
                    AAUser aaUser = (AAUser)AAService.Instance.getUserObject(token, entaa.AppId.ToString());

                    // Add roles to the cookie
                    List<string> rolesList = new List<string>();
                    foreach (var i in aaUser.Privileges)
                    {
                        rolesList.Add(i.PrivilegeCode);
                    }
                    string roles = string.Join(",", rolesList.ToArray());

                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, aaUser.Id, DateTime.Now, DateTime.Now.AddMinutes(20), false, roles);
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(authCookie);

                    // JMM ADDED
                    // Check to see if the user has a returnURL in a cookie.
                    // Don't know of a better way to do this right now- everything I see
                    // says stay away from session variables.
                    HttpCookie cookie = Request.Cookies.Get(WebConfigurationManager.AppSettings["AppCookieName"]);
                    if (cookie == null || cookie.Values.Get("returnURL").ToString() == "")
                        return RedirectToAction("Index", "Home", new { area = "" });
                    else
                        return Redirect(cookie.Values.Get("returnURL").ToString());
                }
                else
                {
                   // Logger.Write("Null Token Error", "CWDReporting", 3, 900, TraceEventType.Error, "Application FAILED to login properly.");
                    return RedirectToAction("Index", "Error");
                }
            }
            catch (Exception ex)
            {
                //Logger.Write(ex.ToString(), "CWDReporting", 3, 900, TraceEventType.Error, "Application FAILED to login properly.");
                return RedirectToAction("Index", "Error");
            }
        }

        /// <summary>
        /// ENTAA Login functionality 
        /// </summary>
        /// <returns>redirects user to proper page after login</returns>
        public RedirectResult ENTAALogin()
        {
            if (!Request.IsAuthenticated)
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                // Set a cookie with the url for the sending page.
                // We'll access this later to return the user to the
                // proper URL after ENTAA login
                string cookieName = WebConfigurationManager.AppSettings["AppCookieName"];
                HttpCookie cookie = Request.Cookies.Get(cookieName) == null ? new HttpCookie(cookieName) : Request.Cookies.Get(cookieName);
                string retUrl = "";

                // Here we handle the case where user clicked a link to go to a restricted page.
                // Request.UrlReferrer.AbsoluteUri will be empty, but luckily the framework is smart enough
                // to send a returnURL querystring in the address, so we'll just grab that.
                Uri uri = Request.Url;
                if (uri.Query.Contains("ReturnUrl"))
                {
                    retUrl = HttpUtility.ParseQueryString(Request.Url.Query)["ReturnUrl"].ToString();
                }
                else
                {
                    retUrl = Request.UrlReferrer.AbsoluteUri;
                }

                // set the return url in a cookie
                cookie.Values.Set("returnURL", retUrl);
                Response.Cookies.Add(cookie);

                // get the host, appid, and return values from the entaa tag in the web.config
                // send the application to the AAHandler after authenticated in A&A
                ENTAAConfigSectionHandler entaa = WebConfigurationManager.GetSection(@"entaa") as ENTAAConfigSectionHandler;
                if (entaa != null)
                {
                    AAService.Host = entaa.Host;
                    return Redirect(AAService.Instance.getSignonUrl(entaa.AppId, entaa.Return));
                }
                else
                {
                    //Logger.Write("Missing Sign On URL", "CWDReporting", 3, 900, TraceEventType.Error, "Application FAILED to login properly.");
                    return Redirect("/");
                }
            }
            else
            {
                //Logger.Write("IsAuthenticated Error: Shouldn't get here when user is logged in.", "CWDReporting", 3, 900, TraceEventType.Error, "Application FAILED to login properly.");
                return Redirect(Url.Action("Index", "Error", new { area = "" }));
            }
        }

        /// <summary>
        /// ENTAA Logout functionality
        /// </summary>
        /// <returns></returns>
        public ActionResult ENTAALogout()
        {
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();

                //clear their session
                Session.Clear();
                Session.Abandon();
            }

            // Return user to home/index
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}