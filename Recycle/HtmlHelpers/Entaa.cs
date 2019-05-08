//---------------------------------------------------------------------------------------------------------
// <copyright file="Entaa.cs" company="Iowa Department of Natural Resources"></copyright>
// <author>Matt Plathe</author> <date>12/10/2014</date>
// <summary>MVC5 Application</summary>
// <remarks>
//      Created By ?
//      Updated By Matt Plathe
//      Version 1.0
// </remarks>
//---------------------------------------------------------------------------------------------------------
using System.Web.Mvc;

namespace Recycle.HtmlHelpers
{
    /// <summary>
    /// entaa class
    /// </summary>
    public static class Entaa
    {
        /// <summary>SignInSignOut is a method in the ENTAA class. 
        /// <para>This is a html helper to display a link to A and A as a button.</para>
        /// </summary> 
        /// <param name="html">This HtmlHelper</param>
        /// <param name="authenticated">Boolean value indicating whether the user is authenticated.</param>
        /// <remarks>DRY vs. seperation of concerns
        ///          to eliminate html here we need to put redundant logic on multiple views
        ///          might be able to do it with a reusable HTMLHelper but what's needed for getting the current user</remarks>
        /// <returns>An MvcHtmlString with a Sign In A and A button image or " Sign Out, depending on the authenticated status of the user.</returns>
        public static MvcHtmlString SignInSignOut(this HtmlHelper html, bool authenticated, string id)
        {
            // go through ENTAA
            MvcHtmlString result;
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            if (!authenticated)
            {
                var logonurl = urlHelper.Action("ENTAALogin", "Account", new { area = "" });
                var imageurl = urlHelper.Content("~/Content/img/AA-logo-in.png");
                result = new MvcHtmlString(string.Format("<a class='nav-link' href='{0}'><img src='{1}' alt='Log In' style='border:0px;position:relative;bottom:2px;' /> Log In</a>", logonurl, imageurl));
            }
            else
            {
                var logouturl = urlHelper.Action("ENTAALogout", "Account", new { area = "" });
                var imageurl = urlHelper.Content("~/Content/img/AA-logo-in.png");
                result = new MvcHtmlString(string.Format("<a class='nav-link' href='{0}'><img src='{1}' alt='Log Out' style='border:0px;position:relative;bottom:2px;' /> Log Out - {2}</a>", logouturl, imageurl, FullName(id)));
            }

            return result;
        }

        /// <summary>
        /// get the first and last name from the email string
        /// </summary>
        /// <param name="entaaEmail"></param>
        /// <returns></returns>
        public static MvcHtmlString GetENTAAName(this HtmlHelper html, string id)
        {
            return new MvcHtmlString(FullName(id));
        }

        /// <summary>
        /// Format to full name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static string FullName(string id)
        {
            string[] email = id.Split('@');
            string[] fullname = email[0].Split('.');
            var name = fullname[0].Remove(1).ToUpper() + fullname[0].Substring(1).ToLower() + " " + fullname[1].Remove(1).ToUpper() + fullname[1].Substring(1).ToLower();
            return name;
        }
    }
}