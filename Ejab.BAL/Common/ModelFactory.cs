using Ejab.BAL.Helpers;
using Ejab.BAL.ModelViews;
using Ejab.BAL.ModelViews.AboutApplication;
using Ejab.BAL.ModelViews.AboutUs;
using Ejab.BAL.ModelViews.CommonQuestions;
using Ejab.BAL.ModelViews.Email;
using Ejab.BAL.ModelViews.Notification;
using Ejab.BAL.ModelViews.Statictics;
using Ejab.DAl;
using Ejab.DAl.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ejab.BAL.Common
{
    public class ModelFactory
    {
       string BaseURL = ConfigurationManager.AppSettings["BaseURL"].ToString();
        string serviceBaseUrl= ConfigurationManager.AppSettings["BaseServiceURL"].ToString();
        #region Offer
        public Offer Parse(OfferViewModel offerModel)
        {
            try
            {
                var offer = new Offer();
                if (offer.IsDiscount == true)
                {
                    offer.DiscountAmount = offerModel.DiscountAmount;
                    offer.DiscountPecent = offerModel.DiscountPecent;
                }
                else
                {
                    offer.DiscountAmount = null;
                    offer.DiscountPecent = null;
                }
                offer.Address = offerModel.Address;
                offer.AddressLongitude = offerModel.AddressLongitude;
                offer.AdressLatitude = offerModel.AdressLatitude;
                offer.CreatedOn = DateTime.Now;
                offer.Description = offerModel.Description;
                offer.ExpireDate = offerModel.ExpireDate;
                offer.IsActive = offerModel.IsActive;
                offer.MaxExpireDate = offerModel.MaxExpireDate;
                offer.OfferDate = DateTime.Now;
                offer.OfferNumber = offerModel.OfferNumber;
                offer.PublishDate = offerModel.PublishDate;
                offer.Quantity = offerModel.quantity;
                offer.Title = offerModel.Title;
                offer.Price = offerModel.Price;
                offer.TruckTypeId = offerModel.TruckTypeId;
                offer.ImageUrl = offerModel.ImageUrl;
                offer.RegionId = offerModel.RegionId;
                offer.Period = offerModel.Period;
               
                return offer;
            }
            catch (Exception ex)
            { throw ex; }
        }
        public OfferViewModel Create(Offer offer)
        {
            string imagFullPath = HttpContext.Current.Server.MapPath("~/OffersImages/");
            return new OfferViewModel()
            {
                Id = offer.Id,
                OfferNumber = offer.OfferNumber,
                Address = offer.Address,
                AddressLongitude = offer.AddressLongitude,
                AdressLatitude = offer.AdressLatitude,
                DiscountAmount = offer.DiscountAmount,
                Description = offer.Description,
                DiscountPecent = offer.DiscountPecent,
                ExpireDate = offer.ExpireDate.Value,
                IsActive = offer.IsActive,
                IsDiscount = offer.IsDiscount,
                MaxCustomerNumbers = offer.MaxCustomerNumbers,
                MaxExpireDate = offer.MaxExpireDate,
                OfferDate = offer.OfferDate,
                // OfferDetails = offer.OfferDetails.Select(Details => new OfferDetailViewModel { AcceptedDate = Details.AcceptedDate, AcceptedUserId = Details.AcceptedUserId, Notes = Details.Notes, OfferId = Details.OfferId }),
                OfferDetails = offer.OfferDetails.Select(Details => new  OfferDetailViewModel {truckId=Details.TruckId,trucksNo=Details.NumberOfTrucks  }),
                //OfferImages = offer.OfferImages.Select(img => new OfferImagesViewModel { OfferId = img.OfferId, ImageDescription = img.ImageDescription, ImageTitle = img.ImageTitle, ImageUrl = img.ImageUrl }),
                //  OfferImages = offer.OfferImages.Select(img => Create(img)),

                Price = offer.Price,
                PublishDate = offer.PublishDate,
                quantity = offer.Quantity,

                TruckTypeId = offer.TruckTypeId,
                Title = offer.Title,
                ServiceProviderUserId = offer.UserId,
                RegionId = offer.RegionId,
               // ImageUrl = offer.ImageUrl != null ? ServerHelper.MapPathReverse(imagFullPath + offer.ImageUrl) : null,
               
                Period = offer.Period,
                FlgStatus=offer.FlgStatus
                



            };
        }
        #endregion
        #region TruckType
        public TruckType Parse(TruckTypeViewModel trucksTypeModel)
        {
            var trucktype = new TruckType
            {
                Id = trucksTypeModel.TypeId,
                Name = trucksTypeModel.Name,
                NameArb = trucksTypeModel.NameArb,
                UpdatedOn = trucksTypeModel.UpdatedOn,
                UpdatedBy = trucksTypeModel.UpdatedBy,
                CreatedBy = trucksTypeModel.CreatedBy,
                CreatedOn = trucksTypeModel.CreatedOn,
                FlgStatus = trucksTypeModel.FlgStatus
                //   Trucks= trucksTypeModel.Trucks.Select(x=> new 
                //  TrucksViewModel { Name=x.Name ,Capacity=x.Capacity ,AvialableNo=x.AvialableNo,Description=x.Description,CreatedBy=x.CreatedBy,CreatedOn=x.CreatedOn,FlgStatus=x.FlgStatus,Height=x.Height,IsOcuppied=x.IsOcuppied ,ParanetId=x.ParanetId ,Weight=x.Weight,UpdatedBy=x.UpdatedBy ,UpdatedOn=x.UpdatedOn })
            };
            return trucktype;
        }
        public Truck Parse(TrucksViewModel trucksModel)
        {
            string imagFullPath = HttpContext.Current.Server.MapPath("~/TrucksImages/");
            ServerHelper.MapPathReverse(imagFullPath);
            var truck = new Truck
            {
                Name = trucksModel.Name,
                NameArb = trucksModel.NameArb,
                //TypeId = trucksModel.TruckTypeId,
                ////  ParanetId = trucksModel.ParanetId.HasValue ? trucksModel.ParanetId.Value : 0,
                //Weight = trucksModel.Weight,
                //Height = trucksModel.Height,
                //Description = trucksModel.Description,
                //TruckImagePath = string.IsNullOrEmpty(trucksModel.TruckImagePath) ? null
                //: ServerHelper.MapPathReverse(imagFullPath + trucksModel.TruckImagePath)

            };
            return truck;
        }
        public TruckTypeViewModel Create(TruckType truckType)
        {
            return new TruckTypeViewModel()
            {
                
                TypeId = truckType.Id,
                NameArb = truckType.NameArb,
                Name=truckType.Name ,
                CreatedBy = truckType.CreatedBy,
                CreatedOn = truckType.CreatedOn,
                FlgStatus = truckType.FlgStatus,
                UpdatedBy = truckType.UpdatedBy,
                UpdatedOn = truckType.UpdatedOn,
                Trucks = truckType.Trucks.Select(trucks => Create(trucks)),
                

            };
        }
        public TrucksViewModel Create(Truck trucks)
        {
            //string imagFullPath = HttpContext.Current.Server.MapPath("~/TrucksImages/");
            //ServerHelper.MapPathReverse(imagFullPath);
            return new TrucksViewModel()
            {
             Id = trucks.Id,
                Name = trucks.Name,
                NameArb = trucks.NameArb,
                TruckTypeId = trucks.TypeId,
                Height = trucks.Height,
                Description = trucks.Description,
                Capacity = trucks.Capacity,
                AvialableNo = trucks.AvialableNo,
                IsOcuppied = trucks.IsOcuppied,
                Weight = trucks.Weight,
                Width = trucks.Width,
                //ParenetName= trucks.ParanetId !=null?  trucks.Paranet.Name:"" ,
                //ParenetNameArb = trucks.ParanetId != null ? trucks.Paranet.NameArb:"",
                TruckImagePath = trucks.TruckImagePath !=null ? BaseURL + trucks.TruckImagePath : null,
                FlgStatus=trucks.FlgStatus
              

            };
        }
        #endregion
        #region ServiceType

        public ServiceType Parse(ServiceTypeViewModel serviceTypeModel)
        {
            var servicetype = new ServiceType
            {
                Id = serviceTypeModel.Id,
                Name = serviceTypeModel.Name
                //UpdatedOn = serviceTypeModel.UpdatedOn,
                //UpdatedBy = serviceTypeModel.UpdatedBy,
                //CreatedBy = serviceTypeModel.CreatedBy,
                //CreatedOn = serviceTypeModel.CreatedOn,
                //FlgStatus = serviceTypeModel.FlgStatus

            };
            return servicetype;
        }

        public ServiceTypeViewModel Create(ServiceType serviceType)
        {
            return new ServiceTypeViewModel()
            {
                Id = serviceType.Id,
                Name = serviceType.Name
            };
        }
        #endregion
        #region CommonQuestion

        public CommonQuestion Parse(Commonquestionsviewmodel questionModel)
        {
            var entity = new CommonQuestion
            {
                QuestionArb = questionModel.QuestionArb,
                AnswerArb = questionModel.AnswerArb,
                QuestionEng = questionModel.QuestionEng,
                AnswerEng = questionModel.AnswerEng
            };
            return entity;
        }

        public Commonquestionsviewmodel Create(CommonQuestion question)
        {
            return new Commonquestionsviewmodel()
            {
                Id = question.Id,
                QuestionArb = question.QuestionArb,
                AnswerArb = question.AnswerArb,
                QuestionEng = question.QuestionEng,
                AnswerEng = question.AnswerEng
            };
        }
        #endregion
        #region OfferImages
        public OfferImage Parse(OfferImagesViewModel imageModel)
        {
            var offerImages = new OfferImage
            {
                CreatedBy = imageModel.CreatedBy,
                CreatedOn = imageModel.CreatedOn,
                // OfferId = imageModel.OfferId,
                ImageTitle = imageModel.ImageTitle,
                ImageDescription = imageModel.ImageDescription,
                ImageUrl = imageModel.ImageUrl,
                FlgStatus = imageModel.FlgStatus,
                UpdatedBy = imageModel.UpdatedBy,
                UpdatedOn = imageModel.UpdatedOn

            };
            return offerImages;
        }
        public OfferImagesViewModel Create(OfferImage imgs)
        {
            return new OfferImagesViewModel()
            {
                OfferId = imgs.OfferId,
                ImageUrl = imgs.ImageUrl,
                ImageDescription = imgs.ImageDescription,
                ImageTitle = imgs.ImageTitle,
                CreatedBy = imgs.CreatedBy,
                CreatedOn = imgs.CreatedOn,
                FlgStatus = imgs.FlgStatus,
                UpdatedBy = imgs.UpdatedBy,
                UpdatedOn = imgs.UpdatedOn
            };
        }
        #endregion
        #region OfferDetails
        public OfferDetail Parse(OfferDetailViewModel detailModel)
        {
            var details = new OfferDetail
            {
                //AcceptedDate = detailModel.AcceptedDate,
                //AcceptedUserId = detailModel.AcceptedUserId,
                OfferId = detailModel.OfferId,
                //Notes = detailModel.Notes,
                //   Offer =Parse (detailModel.Offer) ,
                // Truck = Parse(detailModel.OfferTrucks),
                TruckId = detailModel.truckId,
                NumberOfTrucks = detailModel.trucksNo,

            };
            return details;
        }
        public OfferDetailViewModel Create(OfferDetail offerDetails)
        {
            return new OfferDetailViewModel()
            {
                //AcceptedDate = offerDetails.AcceptedDate,
                //AcceptedUserId = offerDetails.AcceptedUserId,
                //Notes = offerDetails.Notes,
                OfferId = offerDetails.OfferId,
                trucksNo = offerDetails.NumberOfTrucks,
                truckId = offerDetails.TruckId,
               OfferTrucks =Create(offerDetails.Truck )
                // truckName=offerDetails.Truck.Name,
                // Weight=offerDetails.Truck.Weight

                // Offer=offerDetails.Offer 
            };
        }

        #endregion
        #region AcceptOffer
        public AcceptOffer Parse(AcceptofferViewModel acceptoffer)
        {
            var accptOff = new AcceptOffer
            {

                AcceptedDate = acceptoffer.AcceptedDate.HasValue ? acceptoffer.AcceptedDate.Value : DateTime.Today.Date,
                AcceptedUserId = acceptoffer.AcceptedUserId,
                OfferId = acceptoffer.OfferId,
                Notes = acceptoffer.Notes,

            };
            return accptOff;
        }
        public AcceptofferViewModel Create(AcceptOffer acceptOffer)
        {
            return new AcceptofferViewModel()
            {
                AcceptedDate = acceptOffer.AcceptedDate,
                AcceptedUserId = acceptOffer.AcceptedUserId,
                Notes = acceptOffer.Notes,
                OfferId = acceptOffer.OfferId,

            };
        }
        #endregion
        #region Rating
        public Rating Parse(ServiceProviderRatingViewModel ratingModel)
        {
            var rating = new Rating
            {
                RatingValue = ratingModel.Rating,
                Description = ratingModel.Description,
                ServiceRequestId = ratingModel.RequsterId,
                RequstId = ratingModel.RequstId,

            };
            return rating;
        }
        public ServiceProviderRatingViewModel Create(Rating rating)
        {
            return new ServiceProviderRatingViewModel()
            {
                RequstId = rating.RequstId.Value,
                RequsterId = rating.ServiceRequestId,
                Description = rating.Description
            };
        }
        #endregion
        #region Message
        public Message Parse(MessageModelView messageModel)
        {
            var message = new Message
            {
                Date = messageModel.Date,
                // Description = messageModel.Description,
                MessageType = messageModel.MessageType,
                ReciverId = messageModel.ReciverId,
                RequestId = messageModel.RequestId,
                SenderId = messageModel.SenderId,
                Status = messageModel.Status,
                Title = messageModel.Title,
                SendingTime = messageModel.Date.ToShortTimeString()

            };
            return message;
        }
        public MessageModelView Create(Message message)
        {
            return new MessageModelView()
            {
                MessageId = message.Id,
                Title = message.Title,
                // Description = message.Description,
                Date = message.Date,
                Status = message.Status,
                // RequestId = message.RequestId.HasValue? message.RequestId.Value:0  ,
                // OfferId = message.OfferId.HasValue? message.OfferId .Value:0,
                ReciverId = message.ReciverId,
                SendingTime = message.Date.ToShortTimeString(),
                //  Reciver=  Create ( message.Reciver),
                // Sender=message.Sender,

                MessageType = message.MessageType
            };
        }
        #endregion
        #region Complaint
        public SuggestionsComplaint Parse(SuggestionsComplaintModelView compaintModel)
        {
            var Complaint = new SuggestionsComplaint
            {
                Date = compaintModel.Date,
                Cause = compaintModel.Cause,
                //ComplainUserId = compaintModel.ComplainUserId,
                //// ComplainUser= compaintModel.ComplainUser,
                //CustomerId = compaintModel.CustomerId,
                ////  Customer=compaintModel.Customer,
                Name = compaintModel.Name,
                Email = compaintModel.Email,
                Phone = compaintModel.Phone


            };

            return Complaint;
        }
        public SuggestionsComplaintModelView Create(SuggestionsComplaint suggestionComplaint)
        {
            return new SuggestionsComplaintModelView()
            {
                // Admin = suggestionComplaint.Admin,
                Cause = suggestionComplaint.Cause,
                Date = suggestionComplaint.Date,
                Name = suggestionComplaint.Name,
                //ComplainUserId = suggestionComplaint.ComplainUserId,
                //CustomerId = suggestionComplaint.CustomerId,
                Email = suggestionComplaint.Email,
                Phone = suggestionComplaint.Phone


            };
        }
        #endregion

        #region Request
        public Request Parse(RequestModelView requestModel)
        {
            var details = new Request
            {
                Id = requestModel.Id,
                RequesterId = requestModel.UserId,
                Requestdate = requestModel.Requestdate.Date,
                Title = requestModel.Title,
                Description = requestModel.Description,
                //ServiceTypeId = requestModel.ServiceTypeId,
                LocationFromlongitude = requestModel.LocationFromlongitude,
                LocationFromLatitude = requestModel.LocationFromLatitude,
                LocationFrom = requestModel.LocationFrom,
                LocationToLongitude = requestModel.LocationToLongitude,
                LocationToLatitude = requestModel.LocationToLatitude,
                LocationTo = requestModel.LocationTo,

                //PeriodFrom = requestModel.PeriodFrom,
                //PeriodTo = requestModel.PeriodTo,
                RequestState = requestModel.RequestState,
                IsActive = requestModel.IsActive,
                ExpireDate = requestModel.ExpireDate,
                RequestNumber = requestModel.RequestNumber,
                Period = requestModel.Period,
                Quantity = requestModel.Quantity,
                // RegionId= requestModel.RegionId,
                ItemsInfo = requestModel.ItemsInfo,
                Notes = requestModel.Notes,
                RequestType = requestModel.RequestType

            };
            return details;
        }

        public RequestModelView Create(Request request)
        {
           // string imagFullPath = HttpContext.Current.Server.MapPath("~/TrucksImages/");

            return new RequestModelView()
            {
                Id = request.Id,

                Requster = Create(request.User),
                Requestdate = request.Requestdate.Date,
                Title = request.Title,
                Description = request.Description,
                //ServiceTypeId = request.ServiceTypeId,
                LocationFromlongitude = request.LocationFromlongitude,
                LocationFromLatitude = request.LocationFromLatitude,
                LocationFrom = request.LocationFrom,
                LocationToLongitude = request.LocationToLongitude,
                LocationToLatitude = request.LocationToLatitude,
                LocationTo = request.LocationTo,

                //PeriodFrom = request.PeriodFrom,
                //PeriodTo = request.PeriodTo,
                RequestState = request.RequestState,
                IsActive = request.IsActive,
                ExpireDate = request.ExpireDate,
                // AcceptedBy = request.AcceptedBy,
                RequestNumber = request.RequestNumber,
                Period = request.Period,
                Quantity = request.Quantity,
                // RegionName  = request.Region.Name,
                ItemsInfo = request.ItemsInfo,
                Notes = request.Notes,
                RequestType = request.RequestType,
                requestDetails = request.RequestDetailes.Select(rd => new RequestDetailsModelView { Id = rd.Id, truckId = rd.TruckId, truckName = rd.Truck.Name, trucksNo = rd.NumberOfTrucks })
          
               // trucksImagePath = rd.Truck.TruckImagePath != null ? BaseURL + rd.Truck.TruckImagePath.ToString() : null
            };
        }
        #endregion

        #region RequstDetails
        public RequestDetaile Parse(RequestDetailsModelView detailModel)
        {
            var details = new RequestDetaile
            {
                Id = detailModel.Id,
                RequestId = detailModel.RequestId,
                TruckId = detailModel.truckId,
                NumberOfTrucks = detailModel.trucksNo,

            };
            return details;
        }
        public RequestDetailsModelView Create(RequestDetaile requestDetails)
        {
            return new RequestDetailsModelView()
            {
                Id = requestDetails.Id,
                RequestId = requestDetails.RequestId,
                trucksNo = requestDetails.NumberOfTrucks,
                truckId = requestDetails.TruckId
                

            };
        }

        #endregion

        #region RequsetPropsalPrice
        public ProposalPrice Parse(ProposalPriceModelView priceingModel)
        {
            var oProposalPrice = new ProposalPrice
            {
                Id = priceingModel.Id,
                Date = priceingModel.Date,
                Price = priceingModel.Price,
                ServiceProviderId = priceingModel.ServiceProviderId,
                ReqestId = priceingModel.RequestId,
                //  AcceptedBy = priceingModel.AcceptedBy,
                IsAccepted = priceingModel.IsAccepted,
                ExpireDate = priceingModel.ExpireDate  ,
                PropsalStatus = priceingModel.PropsalStatus

            };
            return oProposalPrice;
        }

        public ProposalPriceModelView Create(ProposalPrice pricing)
        {
            return new ProposalPriceModelView()
            {
                Id = pricing.Id,
                RequestId = pricing.ReqestId,
                Date = pricing.Date,
                Price = pricing.Price,
                ServiceProviderId = pricing.ServiceProviderId,
                //  AcceptedBy = pricing.AcceptedBy.Value,
                IsAccepted = pricing.IsAccepted.Value,
                ExpireDate = pricing.ExpireDate ,
                PropsalStatus = pricing.PropsalStatus
            };
        }

        public RequestDetailesPrice Parse(RequestDetailesPricesViewModel priceingDetailesModel)
        {
            var RequestDetailesPrice = new RequestDetailesPrice
            {
                Price = priceingDetailesModel.Price,

                RequestDetaileId = priceingDetailesModel.RequestDetaileId,
                ServiceProviderId = priceingDetailesModel.ServiceProviderId,
                //ExpireDate = priceingDetailesModel.ExpireDate.HasValue? priceingDetailesModel.ExpireDate.Value :DateTime.Now ,
                Notes = priceingDetailesModel.Notes
            };
            return RequestDetailesPrice;
        }
        public RequestDetailesPricesViewModel Create(RequestDetailesPrice pricingDetailes)
        {
            return new RequestDetailesPricesViewModel()
            {
                Price = pricingDetailes.Price,

                RequestDetaileId = pricingDetailes.RequestDetaileId,
                ServiceProviderId = pricingDetailes.ServiceProviderId,
                ExpireDate = pricingDetailes.ExpireDate,
                Notes = pricingDetailes.Notes
            };
        }
        #endregion
        #region User
        public User Parse(UserViewModel userModel)
        {
            var user = new User
            {
                Id = userModel.Id,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Mobile = userModel.Mobile,
                Email = userModel.Email,
                Address = userModel.Address,
                AddressLatitude = userModel.AddressLatitude,
                AddressLongitude = userModel.AddressLongitude,

                CustomerType = userModel.CustomerType,
                OverAllrating = userModel.Rating,
                //RegisteredBy = userModel.RegisteredBy,
                //FaceBookId = userModel.FaceBookId


            };
            return user;
        }

        public UserViewModel Create(User user)
        {
            string path = ConfigurationManager.AppSettings["UserProfilepath"];
            return new UserViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Mobile = user.Mobile,
                Email = user.Email,
                Address = user.Address,
                AddressLatitude = user.AddressLatitude,
                AddressLongitude = user.AddressLongitude,

                CustomerType = user.CustomerType,
                Rating = user.OverAllrating.HasValue ? user.OverAllrating.Value : 0,
                //RegisteredBy = user.RegisteredBy,
                //FaceBookId = user.FaceBookId,
              ProfileImgPath = user.ProfileImgPath !=null ? path+ user.ProfileImgPath:null ,
                ResponsiblePerson=user.ResponsiblePerson ,
                IsActive=user.IsActive ,
                FlgStatus=user.FlgStatus





            };
        }
        #endregion
        #region Interest
        public Interest Parse(InterestViewModel inerestModel)
        {
            var interest = new Interest
            {
                Id = inerestModel.InterestId,
                UserId = inerestModel.UserId,
                TruckId = inerestModel.TruckId,
                Date = inerestModel.Date,
                Notes = inerestModel.Notes,
               // RegionId = inerestModel.RegionId
                //User=Parse( inerestModel.User),
                //Truck =Parse(inerestModel.Truck )
            };
            return interest;
        }

        public InterestViewModel Create(Interest interest)
        {
            return new InterestViewModel()
            {
                InterestId = interest.Id,
                UserId = interest.UserId,
                TruckId = interest.TruckId.HasValue ? interest.TruckId.Value : 0,
                Date = interest.Date,
                Notes = interest.Notes,
              //  RegionId = interest.RegionId.HasValue ? interest.RegionId.Value : 0

                //  User   = Create(interest.User),
                // Truck = Create(interest.Truck)
            };
        }
        #endregion
        #region Regions

        public Region Parse(RegionModelView regionModel)
        {
            var region = new Region
            {
                Name = regionModel.Name
                // parantId=regionModel.ParanetId 
            };
            return region;
        }

        public RegionModelView Create(Region region)
        {
            return new RegionModelView()
            {
                Id = region.Id,
                Name = region.Name



                // ParanetId=region.parantId 
            };
        }
        #endregion
        #region rule

        public Rule Parse(RuleViewModel ruleModel)
        {
            var rule = new Rule
            {
                Name = ruleModel.Name
            };
            return rule;
        }

        public RuleViewModel Create(Rule rule)
        {
            return new RuleViewModel()
            {
                Id = rule.Id,
                Name = rule.Name
            };
        }
        #endregion

        #region Emailes
        public MailSubscribe Parse(EmailSubscriptionViewModel emaileModel)
        {
            var interest = new MailSubscribe
            {
                Email = emaileModel.Email
            };
            return interest;
        }

        public EmailSubscriptionViewModel Create(MailSubscribe email)
        {
            return new EmailSubscriptionViewModel()
            {
                Email = email.Email
            };
        }
        #endregion
        #region Staticitcs
        public Statistics Parse(StaticticsViewModel statModel)
        {
            var statistics = new Statistics
            {
                AppDownloadsNo = statModel.AppDownloadsNo,
                CustomerNo = statModel.CustomerNo,
                OfferNo = statModel.OfferNo,
                TrucksOrdersNo = statModel.TrucksOrdersNo

            };
            return statistics;
        }

        public StaticticsViewModel Create(Statistics statictics)
        {
            return new StaticticsViewModel()
            {
                Id = statictics.Id,
                AppDownloadsNo = statictics.AppDownloadsNo,
                CustomerNo = statictics.CustomerNo,
                OfferNo = statictics.OfferNo,
                TrucksOrdersNo = statictics.TrucksOrdersNo
            };
        }
        #endregion

        #region Notification
        public Notification Parse(NotificationViewModel notyModel)
        {
            var message = new Notification
            {
                Date = notyModel.Date,
                Body = notyModel.Body ,
                Title = notyModel.Title,
                SenderId = notyModel.SenderId,
                ReceiverId = notyModel.ReceiverId,
           //   DeviceType = notyModel.DeviceType ,
                Seen = notyModel.Seen

            };
            return message;
        }
        public NotificationViewModel Create(Notification noty)
        {
            return new NotificationViewModel()
            {
                Date = noty.Date,
                Body=  noty.Body  ,
              
                Title = noty.Title,
                //  SenderId = noty.SenderId,
                // ReceiverId = noty.ReceiverId,
               // DeviceType = noty.DeviceType,
                Seen = noty.Seen
                //SenderUser = Create(noty.SenderUser ),
                //ReciverUser=Create(noty.ReciverUser )
            };
        }
        #endregion

        #region AboutUs
        public AboutUs Parse(AboutUsViewModel aboutModel)
        {
            var message = new AboutUs
            {
                Address = aboutModel.Address,
                latitude = aboutModel.latitude,
                Longitude = aboutModel.Longitude,
                phone = aboutModel.phone

            };
            return message;
        }
        public AboutUsViewModel Create(AboutUs about)
        {
            return new AboutUsViewModel()
            {
              Address=about.Address ,
                latitude = about.latitude,
                Longitude = about.Longitude,
                phone = about.phone,
                Region=about.Region,
                PostalCode=about.PostalCode,
                fax=about.fax,
                Email=about.Email
            };
        }
        #endregion

        #region AboutApp
        public AboutApplication Parse(AboutAppViewModel aboutModel)
        {
            var about = new AboutApplication
            {
                AboutApp = aboutModel.AboutApp,
                AboutAppEng = aboutModel.AboutAppEng,
                AppLink = aboutModel.AppLink,
                FaceBookLink = aboutModel.FaceBookLink,
                TwitterLink = aboutModel.TwitterLink

            };
            return about;
        }
        public AboutAppViewModel Create(AboutApplication about)
        {
            return new AboutAppViewModel()
            {
                AboutApp = about.AboutApp,
                AboutAppEng = about.AboutAppEng,
                AppLink = about.AppLink,
                FaceBookLink = about.FaceBookLink,
                TwitterLink = about.TwitterLink

            };
        }
        #endregion


        #region User
        public User Parse(AdminviewModel userModel)
        {
            var user = new User
            {
                Id = userModel.Id,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Mobile = userModel.Mobile,
                Email = userModel.Email,
                Address = userModel.Address,
                AddressLatitude = userModel.AddressLatitude,
                AddressLongitude = userModel.AddressLongitude,
                CustomerType = userModel.CustomerType



            };
            return user;
        }

        public AdminviewModel Createadmin(User user)
        {
            string imagFullPath = HttpContext.Current.Server.MapPath("~/UsersProfile/");
            return new AdminviewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Mobile = user.Mobile,
                Email = user.Email,
                Address = user.Address,
                AddressLatitude = user.AddressLatitude,
                AddressLongitude = user.AddressLongitude,
                FlgStatus = user.FlgStatus,
                CustomerType = user.CustomerType,
                ProfileImgPath = string.IsNullOrEmpty(user.ProfileImgPath) ? null : "http://naqelat.sa/AdminProfileImgs/"+user.ProfileImgPath



            };
        }
        #endregion
           #region UserFromadmin
        public User Parse(UserViewModelFromAdmin userModel)
        {
            var user = new User
            {
              
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Mobile = userModel.Mobile,
                Email = userModel.Email

            };
            return user;
        }

        public UserViewModelFromAdmin CreateUserfromadmin(User user)
        {
           // string imagFullPath = HttpContext.Current.Server.MapPath("~/UsersProfile/");
            return new UserViewModelFromAdmin()
            {
               
                FirstName = user.FirstName,
                LastName = user.LastName,
                Mobile = user.Mobile,
                Email = user.Email,
                ProfileImgPath = user.ProfileImgPath 

            };
        }
        #endregion



        public Device  Parse(DeviceVewModel deviceModel)
        {
         
            var device = new Device
            {
                Id= deviceModel.Id ,               
              DeviceToken = deviceModel.DeviceToken,
              SerialNumber= deviceModel.SerialNumber,
              DeviceType= deviceModel.DeviceType 

            };
            return device;
        }



        #region setiing 
        public Setting  Parse(SettingViewModel settingModel)
        {
            var setting = new Setting
            {
              AdminPeriod= settingModel.AdminPeriod,
              MaxAcceptNo= settingModel.MaxAcceptNo,
              MaxExpirDayies= settingModel.MaxExpirDayies,
              ExpirDayies= settingModel.ExpirDayies

            };
            return setting;
        }
        public SettingViewModel Create(Setting setting)
        {
            return new SettingViewModel()
            {
                Id = setting.Id,
                AdminPeriod = setting.AdminPeriod,
                MaxAcceptNo = setting.MaxAcceptNo,
                MaxExpirDayies = setting.MaxExpirDayies,
                ExpirDayies = setting.ExpirDayies

            };
        }
        #endregion

        #region MessagesFromAdmin
        public Message Parse(MessagesFromAdmin messageModel)
        {
            var message = new Message
            {
                Date = messageModel.Date,
                // Description = messageModel.Description,
                MessageType = messageModel.MessageType,
                ReciverId = messageModel.ReciverId,

                SenderId = messageModel.SenderId,
                Status = messageModel.Status,
                Title = messageModel.MessageTitle,
                Description = messageModel.Description,
                SendingTime = messageModel.Date.ToShortTimeString()

            };
            return message;
        }
        public MessagesFromAdmin CreateFromAdmin(Message message)
        {
            return new MessagesFromAdmin()
            {
                MessageId = message.Id,
                MessageTitle  = message.Title,
               Description = message.Description,
                Date = message.Date,
                Status = message.Status,
                // RequestId = message.RequestId.HasValue? message.RequestId.Value:0  ,
                // OfferId = message.OfferId.HasValue? message.OfferId .Value:0,
                ReciverId = message.ReciverId,
                SendingTime = message.Date.ToShortTimeString(),
                //  Reciver=  Create ( message.Reciver),
                // Sender=message.Sender,

                MessageType = message.MessageType
            };
        }
        #endregion
    }
}
