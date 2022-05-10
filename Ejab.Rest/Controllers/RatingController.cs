using Ejab.BAL.Common;
using Ejab.BAL.ModelViews;
using Ejab.BAL.UnitOfWork;
using Ejab.DAl.Common;
using Ejab.Rest.Common;
using Ejab.Rest.CommonEmail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ejab.Rest.Controllers
{
    [Authorize]
    [RoutePrefix("api/UserRating")]
    public class RatingController : BaseController
    {
        IUnitOfWork _uow;
        ModelFactory factory;
        public RatingController(IUnitOfWork uow)
        {
            this._uow = uow;
            factory = new ModelFactory();
        }
        [HttpPost]
        [Route("Rating/{ServiceProviderId}")]
        public IHttpActionResult AddRating(int ServiceProviderId,[FromBody]ServiceProviderRatingViewModel ratingModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }              
               var entity = factory.Parse(ratingModel);
                if (entity == null)
                {
                    var myError = new Error
                    {
                        Code = "005",
                        Message = string.Format("No Data Is Supplied in the Request Body")
                    };
                    return new ErrorResult(myError, Request);
                }
                entity.ServiceProviderId = ServiceProviderId;
                entity.RequstId = ratingModel.RequstId;
                entity.ServiceRequestId = ratingModel.RequsterId;
                entity.CreatedBy = _User.UserId;
                entity.CreatedOn = DateTime.Now;
                entity.FlgStatus = 1;
                entity.UpdatedBy = null;
                entity.UpdatedOn = null;               
                _uow.Rating .Add(entity);
                _uow.Commit();
                var model = factory.Create(entity);
                var ratingCount = _uow.Rating.GetAll(x => x.FlgStatus == 1, null, "").Where(y => y.ServiceProviderId == ServiceProviderId).Count();
                var ratingavg = _uow.Rating.GetAll(x => x.FlgStatus == 1, null, "").Where(y => y.ServiceProviderId == ServiceProviderId).Average(s => s.RatingValue);
                var serviceprovider = _uow.User.GetById(ServiceProviderId);
                serviceprovider.OverAllrating=  Convert.ToDecimal( Math.Round(ratingavg));
                _uow.User.Update(ServiceProviderId, serviceprovider);
                _uow.Commit();
                model.OverAllRating = ratingCount;
                model.AvgRating =Math.Round(Convert.ToDouble(ratingavg) );
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

    }
}
