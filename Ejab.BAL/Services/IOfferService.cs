using Ejab.BAL.ModelViews;
using Ejab.DAl.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.Services
{
   public  interface IOfferService
    {
        OfferViewModel AddOffer(OfferViewModel offerModel, int UserId);
        OfferViewModel EditOffer(int id, OfferViewModel offerModel, int UserId);
        OfferViewModel DeleteOffer(int id, int UserId);
        OfferViewModel GetOfferById(int? UserId, int id);
        object OffersThatAccepted(int AcceptedUserId, HttpRequestMessage Request, SortingParamsViewModer sortParams, int? page = null);
        object OffersForUser(int UserId, HttpRequestMessage request, SortingParamsViewModer search, int? page = null);
        OfferViewModel OfferData(int OfferNo);
        object OffersonHome(int? UserId, HttpRequestMessage Request, SortingParamsViewModer sortParams, int? page = null);
        IEnumerable<OfferViewModel> Offers();
        //OfferDetailViewModel AddOfferDetailes(int offerid, OfferDetailViewModel model,int UserId);
        //OfferDetailViewModel EditOfferDetailes(int id, OfferDetailViewModel model, int UserId);
        //OfferDetailViewModel DeleteOfferDetailes(int id, int UserId);
        IEnumerable<OfferViewModel> Sort(string sortType);
        IEnumerable<OfferViewModel> Search(string searchText);
        object Contacts(int userId);
        ServiceProviderRatingViewModel AddRating(int ServiceProviderId, ServiceProviderRatingViewModel ratingModel, int UserId);
        object RatingDetailes(int ServiceProviderId);
        long MaxOfferNumber();
        int Count(int UserId);
        OfferViewModel OfferDetailes(int OfferId);
        AcceptofferViewModel AddAcceptance(AcceptofferViewModel model, int UserId, int OfferId);
        AcceptofferViewModel EditAcceptance(int id, AcceptofferViewModel model, int UserId);
        AcceptofferViewModel DeleteAcceptance(int id, int UserId);
        IEnumerable<OfferViewModel> AllOffers(string search);
        OfferViewModel GetOfferData(int id);
        void DeActiveOffer(int offerId, bool stat);
        IEnumerable<OfferViewModel> SearchAdmin(string searchText);
        AcceptofferViewModel UpadteOfferState(int OfferId, OfferState State,int userId);
       IEnumerable< AcceptofferViewModel> Offeracceptance(int offerId);
        int OfferAcceptanceCout(int offerId);
        //object  PreviousOffersForUser(int UserId, HttpRequestMessage request, SortingParamsViewModer search, int? page = null, int pageSize = 5);
        object ServiceProviderRequests(int UserId, HttpRequestMessage request, OfferState state, SortingParamsViewModer search, int? page);
        int OfferNos(int userId);
    }
}
