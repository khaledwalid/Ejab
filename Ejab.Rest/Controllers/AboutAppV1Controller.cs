using Ejab.BAL.Services.AboutApp;
using Ejab.Rest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ejab.Rest.Controllers
{
    [Authorize]
    [RoutePrefix("api/V1/AboutApp")]
      public class AboutAppV1Controller : ApiController
    {
        IAboutAppService _aboutApp;
        public AboutAppV1Controller(IAboutAppService AboutApp)
        {
            this._aboutApp = AboutApp;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public ResponseDTO AboutUs()
        {
            try
            {

                var model = _aboutApp.GetAll();
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }

        }

     

    }
}
