using Ejab.BAL.Common;
using Ejab.BAL.ModelViews;
using Ejab.BAL.Services;
using Ejab.BAL.UnitOfWork;
using Ejab.DAl;
using Ejab.DAl.Models;
using Ejab.Rest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Ejab.Rest.Controllers
{
    [Authorize]
    [RoutePrefix("api/V1/ServiceType")]
    public class ServiceTypeV1Controller : BaseController
    {
        IServiceType _servicetype;

        public ServiceTypeV1Controller(IServiceType servicetype)
        {
            this._servicetype = servicetype;
        }
        #region ServiceTypes

        [HttpGet]
        [Route("")]
        public ResponseDTO Get()
        {
            var types = _servicetype.AllServiceType();
            return new ResponseDTO(types);
        }

        [HttpGet]
        [Route("{Id}")]
        public ResponseDTO Get(int id)
        {
            var model = _servicetype.GetServiceTypebyId(id);
            return new ResponseDTO(model);
        }

        [HttpPost]
        [Route("AddServiceType")]
        public ResponseDTO Post([FromBody]ServiceTypeViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ResponseDTO(ModelState);
                }
                var typemodel = _servicetype.AddServiceType(model, _User.UserId);
                return new ResponseDTO(typemodel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message,"");
            }
        }

        [HttpPut]
        [Route("EditServiceTypes/{id}")]
        [ResponseType(typeof(ServiceType))]
        public ResponseDTO Put(int id, [FromBody]ServiceTypeViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ResponseDTO(ModelState);
                }                
                var typeModel = _servicetype.EditServiceTypes(id,model,_User.UserId );
                return new ResponseDTO(typeModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.ToString());
            }
        }

        [HttpDelete]
        [Route("DeleteServiceTypes/{id}")]
        public ResponseDTO Delete(int id)
        {
            try
            {
                var typeModel = _servicetype.DeleteServiceTypes(id,_User.UserId );
                return new ResponseDTO(typeModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.ToString());
            }
        }
        #endregion
    }
}
