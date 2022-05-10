using Ejab.BAL.Services;
using Ejab.BAL.UnitOfWork;
using Ejab.DAl;
using Ejab.DAl.Common;

using System;
using System.Web.Http;
using System.Web.Http.Description;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Drawing;
using System.Web;
using System.Drawing.Imaging;
using Ejab.Rest.Common;
using Ejab.BAL.ModelViews;
using Ejab.BAL.Common;

namespace Ejab.Rest.Controllers
{
    [Authorize]
    [RoutePrefix("api/V1/Offer")]
    public class OfferV1Controller : BaseController
    {
        IOfferService _offerService;

        public OfferV1Controller(IOfferService offerService)
        {
            _offerService = offerService;
        }
        #region Offer
      
        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public ResponseDTO Offers()
        {
            try
            {
                var Offers = _offerService.Offers();
                return new ResponseDTO(Offers);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message.ToString());
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Home")]
        public ResponseDTO OffersonHome(SortingParamsViewModer sortParams, int page=1 )
        {
            try
            {
                int? uId = null;
                if (_User != null)
                    uId = _User.UserId;
                // here user rating Is Missing
                var Offers = _offerService.OffersonHome(uId, Request, sortParams,page);
                return new ResponseDTO(Offers);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Details/{OfferNo}")]
        public ResponseDTO OfferData(int OfferNo)
        {
            try
            {
                // here user rating Is Missing
                var Offers = _offerService.OfferData(OfferNo);
                return new ResponseDTO(Offers);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        [HttpPost]
        [Route("UserOffers")]
        public ResponseDTO OffersForUser(SortingParamsViewModer search, int? page = null)
        {
            try
            {
                var Offers = _offerService.OffersForUser(_User.UserId, Request, search, page);
                return new ResponseDTO(Offers);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        //[HttpGet]
        //[Route("PreviousOffers/{UserId}")]
        //public ResponseDTO PreviousOffersForUser(int UserId, SortingParamsViewModer search, int? page = null, int pageSize = 5)
        //{
        //    try
        //    {
        //        var Offers = _offerService.PreviousOffersForUser(UserId,Request,search, pageSize);
        //        return new ResponseDTO(Offers);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseDTO(ex.Message, "");
        //    }
        //}
        [HttpGet]
        [Route("OfferCount")]
        public ResponseDTO OfferCount()
        {
            try
            {
                var count = _offerService.Count(_User.UserId);
                return new ResponseDTO(count);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }


        [HttpPost]
        [Route("ServiceProviderProsales")]
        public ResponseDTO OffersThatAccepted(SortingParamsViewModer sortParams, int? page = null)
        {
            try
            {
                var Offers = _offerService.OffersThatAccepted(_User.UserId, Request, sortParams);
                return new ResponseDTO(Offers);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        [HttpGet]
        [Route("{id}")]
        [AllowAnonymous]
        public ResponseDTO Offer(int id)
        {
            try
            {
                int? uId = null;
                if (_User != null)
                    uId = _User.UserId;
                var model = _offerService.GetOfferById(uId, id);

                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        [HttpPost]
        [Route("AddOffer")]
        public ResponseDTO AddOffer(OfferViewModel offerModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ResponseDTO(ModelState);
                }
                var model = _offerService.AddOffer(offerModel, _User.UserId);
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        [HttpPut]
        [Route("EditOffer/{id}")]
        public ResponseDTO Put(int id, [FromBody]OfferViewModel offerModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ResponseDTO(ModelState);
                }
                var OfferModel = _offerService.EditOffer(id, offerModel, _User.UserId);
                return new ResponseDTO(OfferModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        [HttpDelete]
        [Route("DeleteOffer/{id}")]
        public ResponseDTO DeleteOffer(int id)
        {
            try
            {
                var OfferModel = _offerService.DeleteOffer(id, _User.UserId);
                return new ResponseDTO(OfferModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        #endregion

        #region OfferDetails
        //[HttpPost]
        //[Route("AddDetials/{offerid}")]
        //public ResponseDTO AddDetials(int offerid, [FromBody] OfferDetailViewModel model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return new ResponseDTO (ModelState);
        //        }
        //        var oDetailsModel = _offerService.AddOfferDetailes(offerid, model,_User.UserId);
        //        return new ResponseDTO (oDetailsModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseDTO (ex.Message,"");
        //    }
        //}
        //[HttpPut]
        //[Route("EditDetials/{id}")]
        //public ResponseDTO EditDetials(int id, [FromBody] OfferDetailViewModel model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return new ResponseDTO (ModelState);
        //        }
        //        var detailsModel = _offerService.EditOfferDetailes(id, model, _User.UserId);
        //        return new ResponseDTO (detailsModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseDTO (ex.Message,"");
        //    }
        //}

        //[HttpDelete]
        //[Route("DeleteDetials/{id}")]
        //public ResponseDTO DeleteDetials(int id)
        //{
        //    try
        //    {
        //        var detailModel = _offerService.DeleteOfferDetailes(id,_User.UserId );
        //        return new ResponseDTO (detailModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseDTO (ex.Message,"");
        //    }
        //}
        #endregion
        #region OfferSearchAndSortingAndServiceProviderContact
        // Sort Offers By date Or Price
        [AllowAnonymous]
        [HttpGet]
        [Route("Sort")]
        public ResponseDTO Sort(string sortType)
        {
            var query = _offerService.Sort(sortType);
            return new ResponseDTO(query);
        }
        // search For offer
        [AllowAnonymous]
        [HttpPost]
        [Route("Search")]
        public ResponseDTO Search([FromUri]string searchText)
        {
            if (searchText == null)
            {
                return new ResponseDTO("012");
            }
            var offerdata = _offerService.Search(searchText);
            return new ResponseDTO(offerdata);
        }
        // Get user contacts
        [HttpPost]
        [Authorize]
        [Route("Contacts/{userId}")]
        public ResponseDTO Contacts(int userId)
        {
            try
            {
                var data = _offerService.Contacts(userId);
                return new ResponseDTO(data);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }

        }
        #endregion
        #region Rating
        [HttpPost]
        [Route("Rating/{ServiceProviderId}")]
        public ResponseDTO AddRating(int ServiceProviderId, [FromBody]ServiceProviderRatingViewModel ratingModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ResponseDTO(ModelState);
                }
                var model = _offerService.AddRating(ServiceProviderId, ratingModel, _User.UserId);
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("RatingDetailes/{serviceProviderId}")]
        public object OffersonHome(int serviceProviderId)
        {
            try
            {
                var Offers = _offerService.RatingDetailes(serviceProviderId);
                return new ResponseDTO(Offers);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("OfferDetailes/{offerId}")]
        public object OfferDetailes(int offerId)
        {
            try
            {
                var Offers = _offerService.OfferDetailes(offerId);
                return new ResponseDTO(Offers);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        #endregion
        #region AcceptOffer
        [HttpPost]
        [Route("AcceptOffer/{offerId}")]
        public ResponseDTO AcceptOffer(AcceptofferViewModel model, int offerId)
        {
            try
            {
                var Accpted = _offerService.AddAcceptance(model, _User.UserId, offerId);
                return new ResponseDTO(Accpted);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        [HttpPut]
        [Route("EditAcceptOffer/{id}")]
        public ResponseDTO EditAcceptOffer(AcceptofferViewModel model, int id)
        {
            try
            {
                var Accpted = _offerService.EditAcceptance(id, model, _User.UserId);
                return new ResponseDTO(Accpted);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        [HttpDelete]
        [Route("DeleteAcceptOffer/{id}")]
        public ResponseDTO DeleteAcceptOffer(int id)
        {
            try
            {
                var Accpted = _offerService.DeleteAcceptance(id, _User.UserId);
                return new ResponseDTO(Accpted);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }


        #endregion

        #region ChangeState
        [HttpPut]
        [Authorize]
        [Route("UpdateState/{offerId}/{state}")]
        public ResponseDTO UpdateState(int offerId,OfferState state)
        {
            try
            {
                var Accpted = _offerService.UpadteOfferState(offerId, state,_User.UserId);
                return new ResponseDTO(Accpted);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        #endregion

        #region ServiceProviderRequests
        [Authorize]       
        [HttpPost]
        [Route("ServiceProviderRequests/{state}")]
        public ResponseDTO ServiceProviderRequests(OfferState  state, SortingParamsViewModer search, int? page = null)
        {
            try
            {
                var Offers = _offerService.ServiceProviderRequests(_User.UserId, Request, state, search, page);
                return new ResponseDTO(Offers);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        #endregion


        [HttpGet]
        [Route("MyOffrsCount")]
        public ResponseDTO MyRequestsCount()
        {
            try
            {
                var count = _offerService.OfferNos(_User.UserId);
                if (count == null)
                {
                    return new ResponseDTO("006");
                }
                return new ResponseDTO(count);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
    }
}
