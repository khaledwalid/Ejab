using Ejab.BAL.ModelViews;
using Ejab.DAl;
using Ejab.DAl.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.Services
{
    public interface IRequestService
    {
        /// <summary>
        /// Inserting request 
        /// </summary>
        /// <param name="request">Request to Insert</param>
        /// <returns></returns>
        void InsertRequest(RequestModelView request);

        /// <summary>
        /// Inserting request and returning the inserted request
        /// </summary>
        /// <param name="request">Request to Insert</param>
        /// <returns></returns>
        RequestModelView InsertRequestWithReturn(RequestModelView request, RequestType type, int userid);

        /// <summary>
        /// Update Request data
        /// </summary>
        /// <param name="request"> Request to update</param>
        RequestModelView UpdateRequest(int requestid, RequestModelView request, int userid);

        /// <summary>
        /// Delete a specific request.
        /// </summary>
        /// <param name="request">A Request to delete</param>
        RequestModelView DeleteRequest(int id);

        /// <summary>
        ///Get All users Active Requests 
        /// </summary>
        /// <returns></returns>
        IEnumerable<RequestModelView> GetRequests();

        /// <summary>
        /// Get all active requests for a specific user
        /// </summary>
        /// <param name="userId">User id to get requests by</param>
        /// <returns></returns>
        object GetRequests(int userId, RequestStates state, HttpRequestMessage Request, SortingParamsViewModer sortParams, int? page = null);

        /// <summary>
        /// Get request by request id
        /// </summary>
        /// <param name="id">Id of the request</param>
        /// <returns></returns>
        RequestModelView GetRequest(int id);
        // IEnumerable<RequestModelView> RequestsForService(int serviceTypeId);
        object HomeRequests(HttpRequestMessage Request, SortingParamsViewModer sortParams, int? page = null);
        IEnumerable<ProposalPriceModelView> AcceptedRequest();
        IEnumerable<ProposalPriceModelView> NotAcceptedRequest();
        RequestDetailsModelView AddRequestDetails(int requestId, RequestDetailsModelView model, int userId);
        RequestDetailsModelView EditRequestDetails(int detailsId, RequestDetailsModelView model, int userId);
        RequestDetailsModelView DeleteRequestDetails(int detailsId);
        IEnumerable<RequestModelView> RequestDetails(int requestId);
        ProposalPriceModelView AddPropsalPrice(List<RequestDetailesPricesViewModel> pmodel, int UserId);
        RequestDetailesPricesViewModel EditPropsalPrice(int RequestId, RequestDetailesPricesViewModel model, int UserId);
        RequestDetailesPricesViewModel DeletePropsalPrice(int id);
        object RequestProposal(int requestId);

        int UserRequestsCount(int userId);
        //RequestModelView requestWithProposales(int requestId);
        int Count(int reqestid);
        long MaxRequestNumber();
        IEnumerable<RequestDetailsModelView> GetRequestDetiles(int PropsalId);
        bool CheckRequestAccept(int requestId);
        // ProposalPriceModelView GetProposalDetailes(int rewuestId);
        //ProposalPriceModelView AddRequestProposal(ProposalPriceModelView model, int UserId);
        //ProposalPriceModelView EditRequestProposal(int id,ProposalPriceModelView model, int UserId);
        //ProposalPriceModelView DeleteRequestProposal(int id, int UserId);
       object  ServiceProviderProsales(int ServiceProviderId, RequestStates state, HttpRequestMessage Request, SortingParamsViewModer sortParams, int? page = null);
        IEnumerable<RequestModelView> ProposalDetailes(int propsalId);
        //object ProposalDetailesDescription(int ReqestId, int serviceprovider);
        IEnumerable<RequestModelView> CompletedRequests();
        IEnumerable<RequestModelView> CurrentRequests(int requesterId);
        IEnumerable<RequestModelView> ExpiredRequests();
        IEnumerable<RequestModelView> AllRequests(string searchTerm);

        RequestModelView GetallDataforRequest(int id);
        int PropsalesCounts(int requestId);
        ProposalPriceModelView UpadtePropsalState(int userId, int DetailesId, PropsalStat State);
        RequestModelView UpadteRequestState(int userId, UpdateRequestStateViewModel updatestateModel);
        void DeActiveRequest(int offerId, bool state);
        int RequestedNos(int userId);

    }
}
