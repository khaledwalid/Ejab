using Ejab.BAL.ModelViews.AboutUs;
using Ejab.BAL.Services.AboutUs;
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
    [RoutePrefix("api/V1/AboutUs")]
    public class AboutUsController : BaseController 
    {

        IAboutUs _aboutUs;
        public AboutUsController(IAboutUs AboutUs)
        {
            this._aboutUs = AboutUs;
        }
        [AllowAnonymous ]
        [HttpGet]
        [Route("")]
        public ResponseDTO AboutUs()
        {
            try
            {

                var model = _aboutUs.GetAll();
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }

        }
      
        [HttpPost]
        [Route("AddAboutUs")]
        public ResponseDTO Post(AboutUsViewModel model)
        {
            try
            {
                var MessageModel = _aboutUs.AddAboutUs (model, _User.UserId);
                return new ResponseDTO(MessageModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
    }
}
