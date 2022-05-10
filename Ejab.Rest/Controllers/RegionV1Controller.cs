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
    [Authorize]
    [RoutePrefix("api/V1/Region")]
    public class RegionV1Controller : BaseController
    {
        IRegionService _iRegionservice;
        public RegionV1Controller(IRegionService _IRegionService)
        {
            _iRegionservice = _IRegionService;
        }
        [HttpGet]
        [Route("Cities/{parentid}")]
        public ResponseDTO Cities(int parentid)
        {
            try
            {
                var users = _iRegionservice .regionByParent(parentid);
                if (users.Count() == 0)
                {
                    return new ResponseDTO("006");
                }
                return new ResponseDTO(users);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        [HttpPost]
        [Route("AddRegion")]
        public ResponseDTO AddRegion([FromBody]RegionModelView model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var returnModel = new ResponseDTO(ModelState);

                    return returnModel;// new ResponseDTO(ModelState);
                }
                if (model == null)
                {
                    return new ResponseDTO("006", "");
                }
              
                    var result = _iRegionservice.AddRegion(model,_User.UserId);
                    return new ResponseDTO(result);          
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
      
        [HttpPut]
        [Route("Edit/{id}")]
        public ResponseDTO Edit(int id,RegionModelView region)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var regionViewModel = _iRegionservice.EditRegion(id, region, _User.UserId);
                    return new ResponseDTO(regionViewModel);
                }
                var returnModel = new ResponseDTO(ModelState);
                  return returnModel;
            }

            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        [HttpDelete]
        [Route("DeleteRegion/{id}")]
        public ResponseDTO DeleteRegion(int id)
        {
            try
            {
                var regionViewModel = _iRegionservice.DeleteRegion (id ,_User.UserId);
                return new ResponseDTO(regionViewModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }

        }
        [HttpGet]
        [Route("{id}")]
        public ResponseDTO Get(int id)
        {
            try
            {
                var region = _iRegionservice.getById (id);               
                return new ResponseDTO(region);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("")]
        public ResponseDTO Get(string search)
        {
            try
            {
                var region = _iRegionservice.Regions(search);
                return new ResponseDTO(region);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("")]
        public ResponseDTO Get()
        {
            try
            {
                var region = _iRegionservice.Regions(null );
                return new ResponseDTO(region);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

    }
}
