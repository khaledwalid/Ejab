using Ejab.DAl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.DAl.DbContext
{
   public  class EjabContextForUI: System.Data.Entity.DbContext
    {
        public EjabContextForUI():base("EjabContextForUI")
        {

        }
        public System.Data.Entity.DbSet<AcceptOffer> AcceptOffers { get; set; } // AcceptOffers
        public System.Data.Entity.DbSet<Device> Devices { get; set; } // Devices
        public System.Data.Entity.DbSet<Interest> Interests { get; set; } // Interests
        public System.Data.Entity.DbSet<Message> Messages { get; set; } // Message
        public System.Data.Entity.DbSet<Offer> Offers { get; set; } // Offer
        public System.Data.Entity.DbSet<OfferDetail> OfferDetails { get; set; } // OfferDetails
        public System.Data.Entity.DbSet<OfferImage> OfferImages { get; set; } // OfferImages
        public System.Data.Entity.DbSet<PredefinedAction> PredefinedActions { get; set; } // PredefinedActions
        public System.Data.Entity.DbSet<ProposalPrice> ProposalPrices { get; set; } // proposalPrice
        public System.Data.Entity.DbSet<Rating> Ratings { get; set; } // Rating
        public System.Data.Entity.DbSet<Request> Requests { get; set; } // Request
        public System.Data.Entity.DbSet<RequestDetaile> RequestDetailes { get; set; } // RequestDetaile
        public System.Data.Entity.DbSet<RequestDetailesPrice> RequestDetailesPrices { get; set; } // RequestDetailesPrices
        public System.Data.Entity.DbSet<ServiceType> ServiceTypes { get; set; } // ServiceType
        public System.Data.Entity.DbSet<SuggestionsComplaint> SuggestionsComplaints { get; set; } // SuggestionsComplaint
        public System.Data.Entity.DbSet<SysLog> SysLogs { get; set; } // SysLogs
        public System.Data.Entity.DbSet<Truck> Trucks { get; set; } // Trucks
        public System.Data.Entity.DbSet<TruckType> TruckTypes { get; set; } // TruckType
        public System.Data.Entity.DbSet<User> Users { get; set; } // User
        public System.Data.Entity.DbSet<UserDevice> UserDevices { get; set; } // UserDevices
        public System.Data.Entity.DbSet<Region> Regions { get; set; } // UserDevices
        public System.Data.Entity.DbSet<Setting> Settings { get; set; } // UserDevices
        public System.Data.Entity.DbSet<MailSubscribe> MailSubscribes { get; set; } // UserDevices
        public System.Data.Entity.DbSet<CommonQuestion> CommonQuestions { get; set; } // UserDevices
        public System.Data.Entity.DbSet<Statistics> Statistics { get; set; }
        public System.Data.Entity.DbSet<Notification> Notification { get; set; }
        public System.Data.Entity.DbSet<AboutUs> AboutUs { get; set; }
    }
  

}
