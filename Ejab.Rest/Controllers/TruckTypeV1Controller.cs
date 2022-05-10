using Ejab.BAL.Common;
using Ejab.BAL.ModelViews;
using Ejab.BAL.Services;
using Ejab.BAL.UnitOfWork;
using Ejab.DAl;
using Ejab.DAl.Common;
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
    [RoutePrefix("api/V1/TruckType")]
    public class TruckTypeV1Controller : BaseController
    {
        ITruckTypeService _truckTypeservice;
    
        public TruckTypeV1Controller(ITruckTypeService truckTypeService)
        {
            this._truckTypeservice = truckTypeService;
          
        }
        #region Types
       
        [HttpGet]
        [Route("")]
        public ResponseDTO Get()
        {
            var types = _truckTypeservice.AllServiceType();
            return new ResponseDTO (types);
        }
   
        [HttpGet]
        [Route("{Id}")]
        public ResponseDTO Get(int id)
        {            
            var model = _truckTypeservice.GetTruckTypebyId(id);
            return new ResponseDTO (model);
        }
       
        [HttpPost]
        [Route("AddTruckType")]      
        public ResponseDTO Post([FromBody]TruckTypeViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ResponseDTO (ModelState);
                }
                if (model == null)
                {                
                    return new ResponseDTO("005");
                }
                var typesmodel = _truckTypeservice.AddTruckType(model,_User.UserId );
                return new ResponseDTO (typesmodel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO (ex.Message,"");
            }
        }
     
        [HttpPut]
        [Route("EditTruckTypes/{id}")]
        public ResponseDTO Put(int id, [FromBody]TruckTypeViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ResponseDTO (ModelState);
                }
                if (model == null)
                {
                   
                    return new ResponseDTO("005");
                }              
                var typeModel = _truckTypeservice.EditTruckTypes(id,model,_User.UserId );
                return new ResponseDTO (typeModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO (ex.Message,"");
            }
        }
       
        [HttpDelete]
        [Route("DeleteTruckTypes/{id}")]
        public ResponseDTO Delete(int id)
        {
            try
            {              
                var typeModel = _truckTypeservice.DeleteTruckTypes(id,_User.UserId ) ;
                return new ResponseDTO (typeModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO (ex.Message,"");
            }
        }
        #endregion
    }
}
