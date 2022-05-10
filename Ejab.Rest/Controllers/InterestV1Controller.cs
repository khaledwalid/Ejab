using Ejab.BAL.ModelViews;
using Ejab.BAL.Services;
using Ejab.Rest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ejab.Rest.Controllers
{
    [Authorize ]
    [RoutePrefix("api/V1/Interest")]
    public class InterestV1Controller : BaseController
    {
        Interestservice _Interestservice;
        public InterestV1Controller(Interestservice service)
        {
            this._Interestservice = service;
        }
        [HttpPost]
        [Route("")]
        public ResponseDTO Get(string type)
        {
            var types = _Interestservice.GetAll (_User.UserId, type);
            return new ResponseDTO(types);
        }

        [HttpGet]
        [Route("{Id}")]
        public ResponseDTO Get(int id)
        {
            var model = _Interestservice.Get (id);
            return new ResponseDTO(model);
        }

        [HttpPost ]
        [Route("AddInterest")]
        public ResponseDTO Post([FromBody]InterestViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ResponseDTO(ModelState);
                }
                if (model == null)
                {
                    return new ResponseDTO("005");
                }
                var typesmodel = _Interestservice.AddInterest (model, _User.UserId);
                return new ResponseDTO(typesmodel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        [HttpPut]
        [Route("EditInterest/{id}")]
        public ResponseDTO Put(int id, [FromBody]InterestViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ResponseDTO(ModelState);
                }
                if (model == null)
                {

                    return new ResponseDTO("005");
                }
                var typeModel = _Interestservice.EditInterest(id, model, _User.UserId);
                return new ResponseDTO(typeModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        [HttpPost]
        [Route("DeleteInterest")]
        public ResponseDTO Delete(DeleteInterestViewModel ids)
        {
            try
            {
                _Interestservice.DeleteInterest (ids, _User.UserId);
                return new ResponseDTO("");
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        [HttpGet]
        [Route("CurrentUserInterest")]
        public ResponseDTO UserInterest()
        {
            var model = _Interestservice.UserInterest(_User.UserId );
            return new ResponseDTO(model);
        }
        [HttpGet]
        [Route("UserInterest/{userId}")]
        public ResponseDTO UserInterest(int userId)
        {
            var model = _Interestservice.UserInterest(userId);
            return new ResponseDTO(model);
        }
        [HttpGet]
        [Route("TrucksInterest/{truckId}")]
        public ResponseDTO TrucksInterest(int truckId)
        {
            var model = _Interestservice.trucksInterest(truckId);
            return new ResponseDTO(model);
        }
        [HttpGet]
        [Route("RegionsInterest/{RegionId}")]
        public ResponseDTO RegionsInterest(int RegionId)
        {
            var model = _Interestservice.RegionInterest(RegionId);
            return new ResponseDTO(model);
        }
    }
}
