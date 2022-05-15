using System;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Diagnostics.Contracts;
using System.Web.Http;
using Ejab.BAL.Utility;

namespace Ejab.Rest.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class FIAAuthenticationFilter : AuthorizationFilterAttribute
    {

        /// <summary>
        /// Public default Constructor
        /// </summary>
        public FIAAuthenticationFilter()
        {
        }

        private readonly bool _isActive = true;

        /// <summary>
        /// parameter isActive explicitly enables/disables this filetr.
        /// </summary>
        /// <param name="isActive"></param>
        public FIAAuthenticationFilter(bool isActive)
        {
            _isActive = isActive;
        }

        /// <summary>
        /// Checks basic authentication request
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            bool skipAuth = false;
            if (SkipAuthorization(filterContext)) skipAuth = true;

            //if (!_isActive && skipAuth) return;
            var identity = FetchAuthHeader(filterContext);
            if (identity == null && !skipAuth)
            {
                ChallengeAuthRequest(filterContext);
                return;
            }
            if (identity != null )
            {
                var genericPrincipal = new GenericPrincipal(identity, identity.Roles);
                Thread.CurrentPrincipal = genericPrincipal;
                filterContext.RequestContext.Principal = genericPrincipal;
                if (!OnAuthorizeUser(identity.Email, filterContext))
                {
                    ChallengeAuthRequest(filterContext);
                    return;
                }
            }
            base.OnAuthorization(filterContext);
        }

        /// <summary>
        /// Virtual method.Can be overriden with the custom Authorization.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        protected virtual bool OnAuthorizeUser(string user, HttpActionContext filterContext)
        {
            if (string.IsNullOrEmpty(user))
                return false;
            return true;
        }

        /// <summary>
        /// Checks for autrhorization header in the request and parses it, creates user credentials and returns as BasicAuthenticationIdentity
        /// </summary>
        /// <param name="filterContext"></param>
        protected virtual BasicAuthenticationIdentity FetchAuthHeader(HttpActionContext filterContext)
        {
            string authHeaderValue = null;
            var authRequest = filterContext.Request.Headers.Authorization;
            if (authRequest != null && !string.IsNullOrEmpty(authRequest.Scheme) && authRequest.Scheme == "user_auth")
                authHeaderValue = authRequest.Parameter;
            if (string.IsNullOrEmpty(authHeaderValue))
                return null;
            authHeaderValue = Utility.Base64Decode(authHeaderValue);
            authHeaderValue = '{' + authHeaderValue + '}';
            authHeaderValue = authHeaderValue.Replace("�", "");
            var credentials = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Security.TokenVM>(authHeaderValue);

            return credentials == null ? null : new BasicAuthenticationIdentity(credentials);
        }

        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            Contract.Assert(actionContext != null);

            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                       || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }

        /// <summary>
        /// Send the Authentication Challenge request
        /// </summary>
        /// <param name="filterContext"></param>
        private static void ChallengeAuthRequest(HttpActionContext filterContext)
        {
            var dnsHost = filterContext.Request.RequestUri.DnsSafeHost;
            filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            filterContext.Response.Headers.Add("WWW-Authenticate", string.Format("user_auth realm=\"{0}\"", dnsHost));
        }
    }

    public class BasicAuthenticationIdentity : IIdentity
    {
        Models.Security.TokenVM _token;
        public BasicAuthenticationIdentity(Models.Security.TokenVM token)
        {
            _token = token;
        }
        public int UserId { get { return _token.user_id; } }
        public string Name { get { return _token.username; } }
        public string Email { get { return _token.Email ; } }
        //public int SurveyId { get { return _token.survey_id; } }
        public string AuthenticationType
        {
            get { return "user_auth"; }
        }
      public string[] Roles { get { return _token.roles; } }
        public bool IsAuthenticated
        {
            get { return _token != null; }
        }
    }
}