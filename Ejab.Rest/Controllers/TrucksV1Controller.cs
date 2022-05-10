using Ejab.BAL.Common;
using Ejab.BAL.ModelViews;
using Ejab.BAL.Services;
using Ejab.BAL.UnitOfWork;
using Ejab.DAl;
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
    [RoutePrefix("api/V1/Trucks")]
    public class TrucksV1Controller : BaseController
    {
        ITruckService _truckService;
      
        public TrucksV1Controller(ITruckService truckService)
        {
            this._truckService = truckService;
          
        }

        #region Trucks

        [HttpGet]
        [Route("")]
        public ResponseDTO Trucks()
        {
            try
            {
                var Trucks = _truckService.GetallTrucks();
                return new ResponseDTO (Trucks);
            }
            catch (Exception ex)
            {
                return new ResponseDTO (ex.Message,"");
            }
        }
       
        [HttpGet]
        [Route("{Id}")]
        public ResponseDTO Truck(int id)
        {
            try
            {
                var truckModel = _truckService.getById(id);
                return new ResponseDTO (truckModel);
            }
            catch (Exception ex)
            { return new ResponseDTO (ex.Message,""); }
        }
        [HttpGet]
        [Route("Description/{truckId}")]
        public ResponseDTO Description(int truckId)
        {
            try
            {
                var truckModel = _truckService.TruckDescription(truckId);
                return new ResponseDTO(truckModel);
            }
            catch (Exception ex)
            { return new ResponseDTO(ex.Message, ""); }
        }

        [HttpPost]
        [Route("AddTruck")]
        public ResponseDTO AddTruck([FromBody]TrucksViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ResponseDTO(ModelState);
                }
                var truckModel = _truckService.AddTruck(model,_User.UserId );
                return new ResponseDTO (truckModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO (ex.Message,"");
            }
        }
    
        [HttpPut]
        [Route("EditTruck/{id}")]
       public ResponseDTO PutTruck(int id, [FromBody]TrucksViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ResponseDTO(ModelState);
                }
                var truckModel = _truckService.EditTruck(id,model,_User.UserId );
                return new ResponseDTO (truckModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO (ex.Message,"");
            }
        }
    
        [HttpDelete]
        [Route("DeleteTruck/{id}")]
        public ResponseDTO DeleteTruck(int id)
        {
            try
            {
                var truckModel = _truckService.DeleteTruck(id,_User.UserId);
                return new ResponseDTO (truckModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO (ex.Message,"");
            }
        }

       
        [HttpGet]
        [Route("Type/{id}")]
        public ResponseDTO TrucksByType(int id)
        {
            try
            {
                var trucks = _truckService.TrucksByType(id);
                return new ResponseDTO (trucks);
            }
            catch (Exception ex)
            { return new ResponseDTO (ex.Message,""); }
        }
        [HttpGet]
        [Route("allParent")]
        public ResponseDTO allParent()
        {
            try
            {
                var trucks = _truckService.allParent();
                return new ResponseDTO (trucks);
            }
            catch (Exception ex)
            { return new ResponseDTO (ex.Message,""); }
        }

        [HttpGet]
        [Route("Parent/{parentId:int}")]
        public ResponseDTO TrucksByParent(int parentId)
        {
            try
            {
               
                var Trucks = _truckService.TrucksByParent(parentId);
                return new ResponseDTO (Trucks);
            }
            catch (Exception ex)
            { return new ResponseDTO (ex.Message,""); }
        }


        #endregion

        [HttpGet]
        [Route("allParentByType/{TypeId}")]
        public ResponseDTO allParentByType(int TypeId)
        {
            try
            {
                var trucks = _truckService.allParentByType(TypeId);
                return new ResponseDTO(trucks);
            }
            catch (Exception ex)
            { return new ResponseDTO(ex.Message, ""); }
        }


    }
}
