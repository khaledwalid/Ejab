using Ejab.BAL.UnitOfWork;
using Ejab.Rest.Filters;
using Ejab.RestFull.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ejab.Rest.Controllers
{
    public class BaseController : ApiController
    {
        public UnitOfWork UOW;
        public BasicAuthenticationIdentity _User {
            get {
                if (User == null)
                    return null;
                if (User.Identity.GetType() == typeof(BasicAuthenticationIdentity))
                    return ((BasicAuthenticationIdentity)User.Identity);
                else
                    return null;
            }
        }
    }
}
