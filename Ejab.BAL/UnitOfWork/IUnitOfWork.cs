using Ejab.BAL.Reository;
using Ejab.DAl;
using Ejab.DAl.Models;
using System;
namespace Ejab.BAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        IGenericRepository<Message> Message { get; }
       IGenericRepository<Offer> Offer { get; }
        IGenericRepository<OfferDetail> OfferDetail { get; }
        IGenericRepository<ProposalPrice> ProposalPrice { get; }
        IGenericRepository<Rating> Rating { get; }
        IGenericRepository<Request> Request { get; }
        IGenericRepository<RequestDetaile> RequestDetaile { get; }
        //   IGenericRepository<RequestState> RequestState { get; }
       IGenericRepository<SuggestionsComplaint> SuggestionsComplaint { get; }
        IGenericRepository<Truck> Truck { get; }
        IGenericRepository<TruckType> TruckType { get; }
     //   UserService UserService { get; }
        IGenericRepository<User> User { get; }
        IGenericRepository<UserDevice> UserDevice { get; }
        IGenericRepository<Device> Devices { get; }
        IGenericRepository<PredefinedAction> PredefinedAction { get; }
        IGenericRepository<SysLog> SysLog { get; }
        IGenericRepository<OfferImage> OfferImages { get; }
        IGenericRepository<ServiceType> ServiceType { get; }
        IGenericRepository<RequestDetailesPrice> RequestDetailesPrice { get; }
        IGenericRepository<Interest> Interest { get; }
        IGenericRepository<AcceptOffer > AcceptOffer { get; }
        IGenericRepository<Region > Region { get; }
        IGenericRepository<Setting > Setting { get; }
        IGenericRepository<Rule > Rule { get; }
        IGenericRepository<MailSubscribe> MailSubscribe { get; }
        IGenericRepository<CommonQuestion> CommonQuestion { get; }
        IGenericRepository<Statistics> Statistics { get; }
        //IGenericRepository<ComplaintStatus> ComplaintStatus { get; }
        IGenericRepository<Notification > Notification { get; }
        IGenericRepository<AboutUs> AboutUs { get; }
        IGenericRepository<AboutApplication > AboutApplication { get; }
        IGenericRepository<UserToken> UserTokens { get; }
        //  ISysLog SysLog { get; }
        void Commit();

    }
}
