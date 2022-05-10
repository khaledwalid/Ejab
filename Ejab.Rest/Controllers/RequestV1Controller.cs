using Ejab.Rest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ejab.BAL.Services;
using Ejab.DAl;
using Ejab.BAL.UnitOfWork;
using Ejab.BAL.ModelViews;
using Ejab.DAl.Common;

namespace Ejab.Rest.Controllers
{
    [Authorize]
    [RoutePrefix("api/V1/Request")]
    public class RequestV1Controller : BaseController
    {
        IRequestService _requestService;

        public RequestV1Controller(IRequestService requestService)
        {
            this._requestService = requestService;

        }
     
        [HttpGet]
        [Route("")]
        public ResponseDTO All()
        {
            try
            {
                var requests = _requestService.GetRequests();
                if (requests == null)
                {
                    return new ResponseDTO("006");
                }
                return new ResponseDTO(requests);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        [HttpGet]
        [Route("{id}")]
        public ResponseDTO GetRequest(int id)
        {
            try
            {
                var requests = _requestService.GetRequest(id);
                if (requests == null)
                {
                    return new ResponseDTO("006");
                }
                return new ResponseDTO(requests);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        [HttpGet]
        [Route("Description/{id}")]
        public ResponseDTO GetRequestDescription(int id)
        {
            try
            {
                var requests = _requestService.GetRequestDetiles(id);
                if (requests == null)
                {
                    return new ResponseDTO("006");
                }
                return new ResponseDTO(requests);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        [HttpPost]
        [Route("CustomerRequests/{state}")]
        public ResponseDTO Get(RequestStates state, SortingParamsViewModer search, int? page = null)
        {
            try
            {
                var userRequests = _requestService.GetRequests(_User.UserId, state, Request, search, page);
                if (userRequests == null)
                {
                    return new ResponseDTO("006");
                }
                return new ResponseDTO(userRequests);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }

        }
        [AllowAnonymous]
        [HttpPost]
        [Route("HomeRequests")]
        public ResponseDTO HomeRequests(HttpRequestMessage Request, SortingParamsViewModer sortParams, int? page = null)
        {
            try
            {
                var homeRequests = _requestService.HomeRequests(Request, sortParams, page);
                if (homeRequests == null)
                {
                    return new ResponseDTO("006");
                }
                return new ResponseDTO(homeRequests);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }

        }
        //[HttpGet]
        //[Route("ServiceType/{serviceTypeId}")]
        //public ResponseDTO RequestsForServiceType(int serviceTypeId)
        //{
        //    try {
        //        var requestsForType = _requestService.RequestsForService(serviceTypeId);
        //        if (requestsForType == null)
        //        {
        //            return new ResponseDTO("006");
        //        }
        //        return new ResponseDTO(requestsForType);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseDTO(ex.Message, "");
        //    }

        //}
        [HttpGet]
        [Route("NotAccepted")]
        public ResponseDTO NotAccepted()
        {
            try
            {
                var requestsForType = _requestService.NotAcceptedRequest();
                if (requestsForType == null)
                {
                    return new ResponseDTO("006");
                }
                return new ResponseDTO(requestsForType);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        [HttpPost]
        [Route("ServiceProviderProsales/{state}")]
        public ResponseDTO ServiceProviderProsales(RequestStates state, SortingParamsViewModer search, int? page = null)
        {
            try
            {
                var propsales = _requestService.ServiceProviderProsales(_User.UserId, state, Request,search, page);
                if (propsales == null)
                {
                    return new ResponseDTO("006");
                }
                return new ResponseDTO(propsales);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        [HttpPost]
        [Route("AddRequest/{type}")]
        public ResponseDTO AddRequest(RequestModelView request, RequestType type)
        {
            try
            {
                var model = _requestService.InsertRequestWithReturn(request, type, _User.UserId);
                if (model == null)
                {
                    return new ResponseDTO("006");
                }
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");

            }
        }

        [HttpPut]
        [Route("EditRequest/{id}")]
        public ResponseDTO EditRequest(int id, RequestModelView request)
        {
            try
            {
                var model = _requestService.UpdateRequest(id, request, _User.UserId);
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        [HttpDelete]
        [Route("DeleteRequest/{id}")]
        public ResponseDTO Delete(int id)
        {
            try
            {
                var model = _requestService.DeleteRequest(id);
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }

        }

        #region RequestDetails
        [HttpPost]
        [Route("AddDetailes/{requestId}")]
        public ResponseDTO AddDetailes(int requestId, RequestDetailsModelView model)
        {
            var detialsModel = _requestService.AddRequestDetails(requestId, model, _User.UserId);

            return new ResponseDTO(detialsModel);
        }
        [HttpGet]
        [Route("RequestDetails/{requestId}")]
        public ResponseDTO AddDetailes(int requestId)
        {
            var model = _requestService.RequestDetails(requestId);
            return new ResponseDTO(model);
        }

        #endregion
        #region PropsalPrice
        [HttpPost]
        [Route("AddPricing")]
        public ResponseDTO AddPricing(List<RequestDetailesPricesViewModel> pmodel)
        {
            try
            {
                var model = _requestService.AddPropsalPrice(pmodel, _User.UserId);
                if (model == null)
                {
                    return new ResponseDTO("006");
                }
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        [HttpPut]
        [Route("EditPricing/{pricingId}")]
        public ResponseDTO EditPricing(int pricingId, RequestDetailesPricesViewModel pmodel)
        {
            try
            {
                var model = _requestService.EditPropsalPrice(pricingId, pmodel, _User.UserId);
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }


        [HttpPut]
        [Route("UpdatePropsal/{pricingId}/{State}")]
        public ResponseDTO UpdatePropsal(int pricingId, PropsalStat State)
        {
            try
            {
                var model = _requestService.UpadtePropsalState(_User.UserId,pricingId, State);
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        [HttpPost]
        [Route("UpdateState")]
        public ResponseDTO UpdateRequestState( UpdateRequestStateViewModel updatestateModel)
        {
            try
            {
                var model = _requestService.UpadteRequestState(_User.UserId, updatestateModel);
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        [HttpDelete]
        [Route("DeletePricing/{id}")]
        public ResponseDTO DeletePricing(int id)
        {
            try
            {
                var model = _requestService.DeletePropsalPrice(id);
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }

        }
        #endregion
        [HttpGet]
        [Route("Proposals/{requestId}")]
        public ResponseDTO Proposals(int requestId)
        {
            try
            {
                var model = _requestService.RequestProposal(requestId);
                if (model == null)
                {
                    return new ResponseDTO("006");
                }
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        [HttpGet]
        [Route("User/{id}")]
        public ResponseDTO UserRequests(int id)
        {
            try
            {
                var count = _requestService.UserRequestsCount(id);
                if (count == null)
                {
                    return new ResponseDTO("006");
                }
                return new ResponseDTO("User Requests:" + count.ToString());
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        //[HttpGet]
        //[Route("Proposal/{requestId}")]
        //public ResponseDTO RequestWithProposal(int requestId)
        //{
        //    try
        //    {
        //        var model = _requestService.requestWithProposales(requestId);
        //        if (model == null)
        //        {
        //            return new ResponseDTO("006");
        //        }
        //        return new ResponseDTO(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseDTO(ex.Message, "");
        //    }
        //}
        [HttpGet]
        [Route("ProposalsCounts/{requestId}")]
        public ResponseDTO ProposalsCounts(int requestId)
        {
            try
            {
                var model = _requestService.PropsalesCounts(requestId);

                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        //[HttpGet]
        //[Route("RequestPricing/{RequestId}")]
        //public ResponseDTO RequistPricing(int RequestId)
        //{
        //    try
        //    {
        //        var model = _requestService.ProposalDetailes(RequestId);

        //        return new ResponseDTO(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseDTO(ex.Message, "");
        //    }
        //}
        //[HttpGet]
        //[Route("RequistPricingDetailes/{RequestId}/{serviceprovider}")]
        //public ResponseDTO RequistPricingDetailes(int RequestId, int serviceprovider)
        //{
        //    try
        //    {
        //        var model = _requestService.ProposalDetailesDescription(RequestId, serviceprovider);

        //        return new ResponseDTO(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseDTO(ex.Message, "");
        //    }
        //}

        [HttpGet]
        [Route("CompletedRequests")]
        public ResponseDTO CompletedRequests()
        {
            try
            {
                var model = _requestService.CompletedRequests();

                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        [HttpGet]
        [Route("CurrentRequests")]
        public ResponseDTO CurrentRequests()
        {
            try
            {
                var model = _requestService.CurrentRequests(_User.UserId);

                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        // [HttpGet]
        // [Route("ExpiredRequests")]
        // public ResponseDTO ExpiredRequests()
        // {
        //     try
        //     {
        //         var model = _requestService.ExpiredRequests();

        //         return new ResponseDTO(model);
        //     }
        //     catch (Exception ex)
        //     {
        //         return new ResponseDTO(ex.Message, "");
        //     }
        // }

        [HttpGet]
        [Route("MyRequestsCount")]
        public ResponseDTO MyRequestsCount()
        {
            try
            {
                var count = _requestService.RequestedNos(_User.UserId);
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
