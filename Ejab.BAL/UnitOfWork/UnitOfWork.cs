using Ejab.DAl;
using System.Linq;
using Ejab.BAL.Reository;
using Ejab.DAl.Models;
using Ejab.BAL.Services;
using System;

namespace Ejab.BAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        EjabContext _context;
        public UnitOfWork(EjabContext ctx)
        {
            _context = ctx;
        }
        public UnitOfWork()
        {
            _context = new EjabContext();
        }

        private IGenericRepository<Offer> _offer;
        public IGenericRepository<Offer> Offer
        {
            get
            {
                if (_offer == null)
                {
                    return new GenericRepository<Offer>(_context);
                }
                return _offer;
            }
        }
        private IGenericRepository<OfferDetail> _offerDetails;
        public IGenericRepository<OfferDetail> OfferDetail
        {
            get
            {
                if (_offerDetails == null)
                {
                    return new GenericRepository<OfferDetail>(_context);
                }
                return _offerDetails;
            }
        }
        private IGenericRepository<ProposalPrice> _proposalPrice;
        public IGenericRepository<ProposalPrice> ProposalPrice
        {
            get
            {
                if (_proposalPrice == null)
                {
                    return new GenericRepository<ProposalPrice>(_context);
                }
                return _proposalPrice;
            }
        }
        private IGenericRepository<Rating> _rating;
        public IGenericRepository<Rating> Rating
        {
            get
            {
                if (_rating == null)
                {
                    return new GenericRepository<Rating>(_context);
                }
                return _rating;
            }
        }
        private IGenericRepository<Request> _request;
        public IGenericRepository<Request> Request
        {
            get
            {
                if (_request == null)
                {
                    return new GenericRepository<Request>(_context);
                }
                return _request;
            }
        }
        private IGenericRepository<RequestDetaile> _requestDetaile;
        public IGenericRepository<RequestDetaile> RequestDetaile
        {
            get
            {
                if (_requestDetaile == null)
                {
                    return new GenericRepository<RequestDetaile>(_context);
                }
                return _requestDetaile;
            }
        }


        private IGenericRepository<SuggestionsComplaint> _suggestionsComplaint;
        public IGenericRepository<SuggestionsComplaint> SuggestionsComplaint
        {
            get
            {
                if (_suggestionsComplaint == null)
                {
                    return new GenericRepository<SuggestionsComplaint>(_context);
                }
                return _suggestionsComplaint;
            }
        }
        private IGenericRepository<Truck> _truck;
        public IGenericRepository<Truck> Truck
        {
            get
            {
                if (_truck == null)
                {
                    return new GenericRepository<Truck>(_context);
                }
                return _truck;
            }
        }
        private IGenericRepository<TruckType> _truckType;
        public IGenericRepository<TruckType> TruckType
        {
            get
            {
                if (_truckType == null)
                {
                    return new GenericRepository<TruckType>(_context);
                }
                return _truckType;
            }
        }

        private IGenericRepository<UserDevice> _userDevices;
        public IGenericRepository<UserDevice> UserDevice
        {
            get
            {
                if (_userDevices == null)
                {
                    return new GenericRepository<UserDevice>(_context);
                }
                return _userDevices;
            }
        }
        private IGenericRepository<PredefinedAction> _predefinedAction;
        public IGenericRepository<PredefinedAction> PredefinedAction
        {
            get
            {
                if (_predefinedAction == null)
                {
                    return new GenericRepository<PredefinedAction>(_context);
                }
                return _predefinedAction;
            }
        }
        //private ISysLog _sysLog;
        //public ISysLog SysLog
        //{
        //    get
        //    {
        //        if (_sysLog == null)
        //        {
        //            return new SysLogBAL();
        //        }
        //        return _sysLog;
        //    }
        //}

        IGenericRepository<User> _user;
        public IGenericRepository<User> User
        {
            get
            {
                if (_user == null)
                {
                    return new GenericRepository<User>(_context);
                }
                return _user;
            }
        }
        IGenericRepository<SysLog> _sysLog;
        public IGenericRepository<SysLog> SysLog
        {
            get
            {
                if (_user == null)
                {
                    return new GenericRepository<SysLog>(_context);
                }
                return _sysLog;
            }
        }
        IGenericRepository<Device> _devices;
        public IGenericRepository<Device> Devices
        {
            get
            {
                if (_devices == null)
                {
                    return new GenericRepository<Device>(_context);
                }
                return _devices;
            }
        }
        IGenericRepository<OfferImage> _offerImages;
        public IGenericRepository<OfferImage> OfferImages
        {
            get
            {
                if (_offerImages == null)
                {
                    return new GenericRepository<OfferImage>(_context);
                }
                return _offerImages;
            }
        }
        IGenericRepository<ServiceType> _serviceType;
        public IGenericRepository<ServiceType> ServiceType
        {
            get
            {
                if (_serviceType == null)
                {
                    return new GenericRepository<ServiceType>(_context);
                }
                return _serviceType;
            }
        }
        IGenericRepository<Message> _message;
        public IGenericRepository<Message> Message
        {
            get
            {
                if (_message == null)
                {
                    return new GenericRepository<Message>(_context);
                }
                return _message;
            }
        }
        IGenericRepository<RequestDetailesPrice> _requestDetailesPrice;
        public IGenericRepository<RequestDetailesPrice> RequestDetailesPrice
        {
            get
            {
                if (_requestDetailesPrice == null)
                {
                    return new GenericRepository<RequestDetailesPrice>(_context);
                }
                return _requestDetailesPrice;
            }
        }
        IGenericRepository<Interest> _interest;
        public IGenericRepository<Interest> Interest
        {
            get
            {
                if (_interest == null)
                {
                    return new GenericRepository<Interest>(_context);
                }
                return _interest;
            }
        }
        IGenericRepository<AcceptOffer> _acceptOffer;
        public IGenericRepository<AcceptOffer> AcceptOffer
        {
            get
            {
                if (_acceptOffer == null)
                {
                    return new GenericRepository<AcceptOffer>(_context);
                }
                return _acceptOffer;
            }
        }
        IGenericRepository<Region> _region;
        public IGenericRepository<Region> Region
        {
            get
            {
                if (_region == null)
                {
                    return new GenericRepository<Region>(_context);
                }
                return _region;
            }
        }
        IGenericRepository<Setting> _setting;
        public IGenericRepository<Setting> Setting
        {
            get
            {
                if (_setting == null)
                {
                    return new GenericRepository<Setting>(_context);
                }
                return _setting;
            }
        }
        IGenericRepository<Rule> _rule;
        public IGenericRepository<Rule> Rule
        {
            get
            {
                if (_rule == null)
                {
                    return new GenericRepository<Rule >(_context);
                }
                return _rule;
            }
        }
       
        IGenericRepository<CommonQuestion> _question;
        public IGenericRepository<CommonQuestion> CommonQuestion
        {
            get
            {
                if (_question == null)
                {
                    return new GenericRepository<CommonQuestion>(_context);
                }
                return _question;
            }
        }
        IGenericRepository<MailSubscribe> _milSubscribe;

        public IGenericRepository<MailSubscribe> MailSubscribe
        {
            get
            {
                if (_milSubscribe == null)
                {
                    return new GenericRepository<MailSubscribe>(_context);
                }
                return _milSubscribe;
            }

        }
        IGenericRepository<Statistics> _statistics;
        public IGenericRepository<Statistics> Statistics
        {
            get
            {
                if (_statistics == null)
                {
                    return new GenericRepository<Statistics>(_context);
                }
                return _statistics;
            }
        }
        //IGenericRepository<ComplaintStatus> _complaintStatus;
        //public IGenericRepository<ComplaintStatus> ComplaintStatus
        //{
        //    get
        //    {
        //        if (_complaintStatus == null)
        //        {
        //            return new GenericRepository<ComplaintStatus>(_context);
        //        }
        //        return _complaintStatus;
        //    }
        //}
        IGenericRepository<Notification> _notification;
        public IGenericRepository<Notification> Notification
        {
            get
            {
                if (_notification == null)
                {
                    return new GenericRepository<Notification>(_context);
                }
                return _notification;
            }
        }
        IGenericRepository<AboutUs> _aboutUs;
        public IGenericRepository<AboutUs> AboutUs
        {
            get
            {
                if (_aboutUs == null)
                {
                    return new GenericRepository<AboutUs>(_context);
                }
                return _aboutUs;
            }
        }
        IGenericRepository<AboutApplication> _aboutApplication;
        public IGenericRepository<AboutApplication> AboutApplication
        {
            get
            {
                if (_aboutApplication == null)
                {
                    return new GenericRepository<AboutApplication>(_context);
                }
                return _aboutApplication;
            }
        }
        IGenericRepository<UserToken> _userToken;
        public IGenericRepository<UserToken> UserTokens
        {
            get
            {
                if (_userToken == null)
                {
                    return new GenericRepository<UserToken>(_context);
                }
                return _userToken;
            }
        }

        public void Rollback()
        {
            _context
                .ChangeTracker
                .Entries()
                .ToList()
                .ForEach(x => x.Reload());
        }
        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
