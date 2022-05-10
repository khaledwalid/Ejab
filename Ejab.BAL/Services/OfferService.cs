using Ejab.BAL.Common;
using Ejab.BAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using Ejab.BAL.ModelViews;
using Ejab.DAl.Common;
using System.Drawing.Imaging;
using System.Drawing;
using System.Web;
using System.IO;
using Ejab.BAL.Helpers;

using System.Net.Http;
using System.Configuration;
using System.Data.Entity;
using Ejab.BAL.Services.Notification;
using Ejab.BAL.ModelViews.Notification;

namespace Ejab.BAL.Services
{
    public class OfferService : IOfferService
    {
        //int pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());
        IUnitOfWork _uow;
        IUserService _userService;
        ModelFactory factory;

        string imagFullPath;
        string _token = "";
        bool _isAuthorized;
        string BaseURL = "";
        string path = "";
        ICustomerService _customerService;
        IRegionService _IRegionService;
        INotificationService _iNotificationService;
        string Userpath = ConfigurationManager.AppSettings["UserProfilepath"];
        public OfferService(IUnitOfWork uow, IUserService UserService, ICustomerService customerservice, IRegionService IRegionService, INotificationService INotificationService)
        {
            this._uow = uow;
            _userService = UserService;
            factory = new ModelFactory();
            _customerService = customerservice;
            _iNotificationService = INotificationService;
            _IRegionService = IRegionService;
            imagFullPath = HttpContext.Current.Server.MapPath("~/OffersImages/");
            path = ConfigurationManager.AppSettings["BaseServiceURL"] + "OffersImages" + '/';
            BaseURL = ConfigurationManager.AppSettings["BaseURL"].ToString();
        }
        public OfferViewModel AddOffer(OfferViewModel offerModel, int UserId)
        {
            var setting = _uow.Setting.GetAll(x => x.FlgStatus == 1, null, "").FirstOrDefault();
            var usersToken = new List<string>();
            var usersTokenForTrucks = new List<string>();
            var userslist = new List<int>();
            var Truckuserslist = new List<int>();
            NotificationViewModel trucknotymodel = new NotificationViewModel();
            NotificationViewModel regionnotymodel = new NotificationViewModel();
            PushNotification truckpush = new PushNotification();
            PushNotification regionpush = new PushNotification();
            var provider = _uow.User.GetById(UserId);
            if (provider.CustomerType != CustomerTypes.ServiceProvider)
            {
                throw new Exception("014");
            }
            var Phyiscalpath = HttpContext.Current.Server.MapPath("~/OffersImages/");
            if (!Directory.Exists(Phyiscalpath))
            {
                Directory.CreateDirectory(Phyiscalpath);
            }
            if (offerModel.ImageUrl != null || offerModel.ImageUrl.Length > 0)
            {
                string converted = offerModel.ImageUrl.Substring(offerModel.ImageUrl.IndexOf(",") + 1);
                Image img = ImageHelper.Base64ToImage(converted);
                string filename = Guid.NewGuid().ToString() + DateTime.Today.ToString("ddMMyyyy") + ".Jpeg";
                var path = Path.Combine(Phyiscalpath, filename);
                img.Save(path, ImageFormat.Jpeg);
                offerModel.ImageUrl = filename;
            }
            var entity = factory.Parse(offerModel);
            if (offerModel.quantity == null)
            {
                entity.Quantity = "";
            }
            if (offerModel.quantity != null)
            {
                entity.Quantity = offerModel.quantity;

            }
            if (offerModel.Period == null)
            {
                entity.Period = "";
            }
            if (offerModel.Period != null)
            {
                entity.Period = offerModel.Period;
            }

            var expiredayes = setting.ExpirDayies;
            var MaxExpireDate = setting.MaxExpirDayies;
            var maxusers = setting.MaxAcceptNo;
            entity.ExpireDate = DateTime.UtcNow.AddDays(expiredayes);
            entity.MaxExpireDate = DateTime.UtcNow.AddDays(MaxExpireDate);
            entity.MaxCustomerNumbers = maxusers;
            entity.IsActive = true;
            entity.RegionId = offerModel.RegionId;
            entity.ImageUrl = offerModel.ImageUrl;
            entity.Notes = offerModel.Notes;
            if (entity == null)
            {
                throw new Exception("005");
            }
            var offers = _uow.Offer.GetAll();
            if (offers.Count() == 0)
            {
                entity.OfferNumber = 1;
            }
            else
            {
                entity.OfferNumber = (int)MaxOfferNumber();
            }
            entity.UserId = UserId;
            entity.CreatedBy = UserId;
            entity.CreatedOn = DateTime.Now;
            entity.FlgStatus = 1;
            entity.UpdatedBy = null;
            entity.UpdatedOn = null;
            _uow.Offer.Add(entity);
            _uow.Commit();

            if (offerModel.RegionId.Value != 0)
            {

                var region = _IRegionService.getById(offerModel.RegionId.Value);
                var allUsers = _uow.User.GetAll(x => x.FlgStatus == 1 && x.CustomerType == CustomerTypes.Requester, null, "Interests").Where(i=> i.Interests.Any(r =>r.RegionId == offerModel.RegionId.Value && r.FlgStatus==1)).Distinct().ToList();
                if (allUsers != null && allUsers.Count() > 0)
                {
                    foreach (var item in allUsers)
                    {
                        userslist.Add(item.Id);
                        var devices = _customerService.GetDeviceByTokenAndType(item.Id);
                        if (devices.Count() > 0)
                        {
                            foreach (var device in devices)
                            {
                                if (device != null)
                                {
                                    if (!usersToken.Contains(device.Fcmtoken))
                                    {
                                        usersToken.Add(device.Fcmtoken);
                                    }

                                }
                            }
                        }

                    }


                }
                if (userslist.Count() > 0)
                {
                    foreach (var id in userslist)
                    {
                        regionnotymodel.ReceiverId = id;
                        var regionName = _IRegionService.getById(offerModel.RegionId.Value).Name;
                        regionnotymodel.Body = "تم إضافه عرض جديد فى هذه المنطقه" + "  " + ":" + regionName + '-' + "New Offer is added in this region" + ":" + regionName;
                        //regionnotymodel.BodyArb = "تم إضافه طلب جديد فى هذه المنطقه" + regionName;
                        regionnotymodel.Date = DateTime.Now;
                        regionnotymodel.Title = " إضافه عرض جديد فى هذه المنطقه" + " " + regionName + '-' + "New Offer is added in region" + regionName;
                        regionnotymodel.Seen = false;
                        // notymodel.ReceiverId = item.Id;


                        regionnotymodel.Type = NotificationType.AddRequest;
                        trucknotymodel.BodyArb = "تم إضافه عرض جديد فى هذه المنطقه" + " " + ":" + regionName;
                        trucknotymodel.BodyEng = "New offer is added in this region" + ":" + regionName;
                        trucknotymodel.TitleArb = " إضافه عرض جديد فى هذه المنطقه" + " " + ":" + regionName;
                        trucknotymodel.TitleEng = "New Offer is added in region" + regionName;
                    }
                    regionnotymodel.registration_ids = usersToken;
                    _iNotificationService.AddNoty(regionnotymodel, UserId);
                    regionpush.PushNotifications(regionnotymodel, "");

                }

            }
            foreach (var item in offerModel.OfferDetails)
            {
                var offerdetailEntity = factory.Parse(item);
                offerdetailEntity.OfferId = entity.Id;
                offerdetailEntity.TruckId = item.truckId;
                offerdetailEntity.NumberOfTrucks = item.trucksNo;
                offerdetailEntity.CreatedBy = UserId;
                offerdetailEntity.CreatedOn = DateTime.Now;
                offerdetailEntity.FlgStatus = 1;
                offerdetailEntity.UpdatedBy = null;
                offerdetailEntity.UpdatedOn = null;
                entity.OfferDetails.Add(offerdetailEntity);
                _uow.Commit();
                var truck = _uow.Truck.GetAll(x => x.FlgStatus == 1 && x.Id == item.truckId).FirstOrDefault();
              
                var allTrucsUsers = _uow.User.GetAll(x => x.FlgStatus == 1 && x.CustomerType == CustomerTypes.Requester, null, "Interests").Where(i => i.Interests.Any(r => r.TruckId == truck.Id && r.FlgStatus==1)).Distinct().ToList();
                if (allTrucsUsers.Count()>0)
                {

                foreach (var user in allTrucsUsers)
                {

                    Truckuserslist.Add(user.Id);
                    var devices = _customerService.GetDeviceByTokenAndType(user.Id);
                    if (devices.Count() > 0)
                    {
                        foreach (var Device in devices)
                        {
                            if (Device != null)
                            {
                                if (!usersTokenForTrucks.Contains(Device.Fcmtoken))
                                {
                                    usersTokenForTrucks.Add(Device.Fcmtoken);

                                }

                            }
                        }

                    }

                }


                    string trucktype = truck.TruckType.NameArb;
                    string trucktypeeng = truck.TruckType.Name;
                    foreach (var user in Truckuserslist)
                {
                    trucknotymodel.ReceiverId = user;
                    trucknotymodel.Body = " تم اضافه عرض جديد"+ " "+ trucktype + " " + ":" + truck.NameArb + '-' + "New Offer is added by this "+ trucktypeeng + "" + "Its Id :" + truck.Id + ":" + "TruckName" + truck.Name;
                    // trucknotymodel.BodyEng = "New Offer is added by this truck" + "Its Id :" + item.truckId + "TruckName" + item.truckName;
                    trucknotymodel.Date = DateTime.Now;
                    trucknotymodel.Title = "تم اضافه عرض جديد "+ " "+ trucktype + truck .NameArb + '-' + "New Offer is added by  " + trucktypeeng + "" + truck.Name;
                    trucknotymodel.Seen = false;
                    // notymodel.ReceiverId = item.Id;              
                    trucknotymodel.Type = NotificationType.AddOffer;
                    trucknotymodel.BodyArb = " تم اضافه عرض جديد"+ " "+ trucktype + " " + ":" + truck .NameArb;
                    trucknotymodel.BodyEng = "New Offer is added by  " + ""+ trucktypeeng + ""+ "Its Id :" + truck.Id  + ":" + "TruckName" + truck.Name;
                    trucknotymodel.TitleArb = "تم اضافه عرض جديد " + " " + ":" + ""+ trucktype + " "+ truck.NameArb;
                    trucknotymodel.TitleEng = "New Offer is added by  "+ ""+ trucktypeeng + ""+ truck .Name;

                }
                trucknotymodel.registration_ids = usersTokenForTrucks;
                truckpush.PushNotifications(trucknotymodel, "");
                _iNotificationService.AddNoty(trucknotymodel, UserId);
                }
            }

            var model = factory.Create(entity);
            //var serviceType = _uow.ServiceType.GetById(offerModel.ServiceTypeId);
            //var serviceTypemodel = factory.Create(serviceType);

            model.ServiceProvider = factory.Create(provider);
            //  model.ServiceType = serviceTypemodel;
            model.ImageUrl = entity.ImageUrl != null ? path + entity.ImageUrl : null;
            return model;

        }

        public OfferViewModel EditOffer(int id, OfferViewModel offerModel, int UserId)
        {
            var setting = _uow.Setting.GetAll(x => x.FlgStatus == 1, null, "").FirstOrDefault();
            var existingOffer = _uow.Offer.GetAll(x => x.FlgStatus == 1, null, "").Where(y => y.Id == id).FirstOrDefault();
            var Phyiscalpath = HttpContext.Current.Server.MapPath("~/OffersImages/");
            if (!Directory.Exists(Phyiscalpath))
            {
                Directory.CreateDirectory(Phyiscalpath);
            }

            if (existingOffer == null)
            {
                throw new Exception("011");
            }
            if (offerModel == null)
            {
                throw new Exception("005");
            }
            if (offerModel.quantity == null)
            {
                existingOffer.Quantity = "";
            }
            else if (offerModel.quantity != null)
            {
                existingOffer.Quantity = offerModel.quantity;

            }
            if (offerModel.PublishDate.ToShortDateString() == "1/1/0001")
            {
                existingOffer.PublishDate = existingOffer.PublishDate;

            }
            else if (offerModel.PublishDate != null)
            {
                existingOffer.PublishDate = offerModel.PublishDate;
            }

            if (offerModel.OfferDate.ToShortDateString() == "1/1/0001")
            {
                existingOffer.OfferDate = existingOffer.OfferDate;

            }
            else if (offerModel.OfferDate != null)
            {
                existingOffer.OfferDate = offerModel.OfferDate;
            }

            if (offerModel.Period == null)
            {
                existingOffer.Period = "";

            }
            else if (offerModel.Period != null)
            {
                existingOffer.Period = offerModel.Period;
            }
            if (offerModel.ImageUrl == null)
            {
                existingOffer.ImageUrl = existingOffer.ImageUrl;

            }
            else if (offerModel.ImageUrl != null || offerModel.ImageUrl.Length > 0)
            {
                string converted = offerModel.ImageUrl.Substring(offerModel.ImageUrl.IndexOf(",") + 1);
                Image img = ImageHelper.Base64ToImage(converted);
                string filename = Guid.NewGuid().ToString() + DateTime.Today.ToString("ddMMyyyy") + ".Jpeg";

                var path = Path.Combine(Phyiscalpath, filename);
                img.Save(path, ImageFormat.Jpeg);
                existingOffer.ImageUrl = filename;
            }
            var expiredayes = setting.ExpirDayies;
            var MaxExpireDate = setting.MaxExpirDayies;
            var maxusers = setting.MaxAcceptNo;
            existingOffer.ExpireDate = DateTime.UtcNow.AddDays(expiredayes);
            existingOffer.MaxExpireDate = DateTime.UtcNow.AddDays(MaxExpireDate);
            existingOffer.MaxCustomerNumbers = maxusers;
            existingOffer.Title = offerModel.Title;
            existingOffer.Description = offerModel.Description;
            existingOffer.Price = offerModel.Price;
            existingOffer.TruckTypeId = offerModel.TruckTypeId;
            existingOffer.MaxCustomerNumbers = offerModel.MaxCustomerNumbers.HasValue ? offerModel.MaxCustomerNumbers.Value : 0;
            existingOffer.Price = offerModel.Price;

            existingOffer.Address = offerModel.Address;
            existingOffer.AdressLatitude = offerModel.AdressLatitude;
            existingOffer.AddressLongitude = offerModel.AddressLongitude;
            existingOffer.IsDiscount = offerModel.IsDiscount;
            existingOffer.DiscountPecent = offerModel.DiscountPecent;
            existingOffer.DiscountAmount = offerModel.DiscountAmount;
            existingOffer.IsActive = existingOffer.IsActive;
            existingOffer.Notes = offerModel.Notes;
            existingOffer.FlgStatus = 1;
            existingOffer.UpdatedBy = UserId;
            existingOffer.UpdatedOn = DateTime.Now;
            existingOffer.RegionId = offerModel.RegionId;
            if (offerModel.OfferDetails != null)
            {
                if (existingOffer.OfferDetails.ToList().Count > 0)
                {
                    foreach (var detiales in existingOffer.OfferDetails.ToList())
                        if (detiales != null)
                        {
                            // existingOffer.OfferDetails.Remove(detiales);
                            //detiales.UpdatedBy = UserId;
                            //detiales.UpdatedOn = DateTime.Now;
                            //detiales.FlgStatus = 0;
                            _uow.OfferDetail.Delete(detiales.Id, detiales);
                            _uow.Commit();
                        }
                }
                foreach (var item in offerModel.OfferDetails)
                {
                    //var detailes = _uow.OfferDetail.GetById(item.Id);
                    var detailesentity = factory.Parse(item);
                    detailesentity.OfferId = existingOffer.Id;
                    detailesentity.TruckId = item.truckId;
                    detailesentity.NumberOfTrucks = item.trucksNo;
                    detailesentity.FlgStatus = 1;
                    detailesentity.CreatedBy = UserId;
                    detailesentity.CreatedOn = DateTime.Now;
                    detailesentity.UpdatedBy = null;
                    detailesentity.UpdatedOn = null;
                    _uow.OfferDetail.Add(detailesentity);
                    _uow.Commit();
                }

            }
            else
            {
                existingOffer.OfferDetails = existingOffer.OfferDetails;
            }
            _uow.Offer.Update(id, existingOffer);
            _uow.Commit();
            var OfferModel = factory.Create(existingOffer);
            OfferModel.ImageUrl = existingOffer.ImageUrl != null ? path + existingOffer.ImageUrl : null;
            return OfferModel;
        }

        public OfferViewModel DeleteOffer(int id, int UserId)
        {
            var offer = _uow.Offer.GetById(id);
            if (offer == null)
            {
                throw new Exception("011");
            }
            offer.FlgStatus = 0;
            offer.UpdatedBy = UserId;
            offer.UpdatedOn = DateTime.Now.Date;
            _uow.Offer.Update(id, offer);
            _uow.Commit();
            var OfferModel = factory.Create(offer);
            return OfferModel;
        }

        public OfferViewModel GetOfferById(int? UserId, int id)
        {
            var offer = _uow.Offer.GetAll(x => x.Id == id).ToList().Select(o => new OfferViewModel
            {
                Id = o.Id,
                OfferDate = o.OfferDate,
                Title = o.Title,
                Description = o.Description,
                Address = o.Address,
                AddressLongitude = o.AddressLongitude,
                AdressLatitude = o.AdressLatitude,
                Period = o.Period,
                quantity = o.Quantity,
                Price = o.Price,
                ExpireDate = o.ExpireDate.Value,
                MaxExpireDate = o.MaxExpireDate,
                Region = factory.Create(o.Region),
                Notes = o.Notes,
                IsActive = o.IsActive,
                ImageUrl = o.ImageUrl != null ? path + o.ImageUrl : null,
                OfferNumber = o.OfferNumber,
                PublishDate = o.PublishDate,
                ServiceProvider = factory.Create(o.User),
                Accepted = UserId.HasValue ? checkaccepatance(UserId.Value, o.Id) : false,
                OfferDetails = o.OfferDetails.Select(d => new OfferDetailViewModel { truckNameArb = d.Truck.NameArb, truckName = d.Truck.Name, ParenttruckName = d.Truck.ParanetId != null ? d.Truck.Paranet.Name : "", ParenttruckNameArb = d.Truck.ParanetId != null ? d.Truck.Paranet.NameArb : "", trucksNo = d.NumberOfTrucks,ImagePath=d.Truck.TruckImagePath!=null? BaseURL+d.Truck.TruckImagePath :null })
            }).SingleOrDefault();
            if (offer == null)
            {
                throw new Exception("011");
            }
            return offer;
        }


        public object OffersThatAccepted(int AcceptedUserId, HttpRequestMessage Request, SortingParamsViewModer sortParams, int? page = null)
        {
            string truckimage = HttpContext.Current.Server.MapPath("~/TrucksImages/");
            int pageSize = sortParams.PageSize;
            //decimal fromprice;
            //decimal.TryParse(sortParams.fromPrice.ToString(), out fromprice);
            //decimal toprice;
            //decimal.TryParse(sortParams.toPrice.ToString(), out toprice);

            var offers =
                _uow.Offer.GetAll(
                    x => x.FlgStatus == 1 && x.AcceptOffers.Any(y => y.AcceptedUserId == AcceptedUserId && y.OfferState == sortParams.Stat), null, "AcceptOffers").OrderByDescending(x => x.Id);
            if (sortParams.fromDate != null)
                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value).OrderByDescending(x => x.Price);

            if (sortParams.toDate != null)
                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= sortParams.toDate.Value).OrderByDescending(x => x.Price);

            if (sortParams.fromPrice != null)
                offers = offers.Where(r => r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.Price); ;

            if (sortParams.toPrice != null)
                offers = offers.Where(r => r.Price <= (decimal)sortParams.toPrice.Value).OrderByDescending(x => x.Price); ;
            if (sortParams.Address != null)
                offers = offers.Where(r => r.Region.Name.StartsWith(sortParams.Address)).OrderByDescending(x => x.Region.NameArb);
            if (sortParams.SearchTerm != null)
                offers = offers.Where(r => r.Title.StartsWith(sortParams.SearchTerm)).OrderByDescending(x => x.Title);
            if (sortParams.SearchTerm != null)
                offers = offers.Where(r => r.Description.StartsWith(sortParams.SearchTerm)).OrderByDescending(x => x.Description);

            if (sortParams.SortPram != null)
            {
                if (sortParams.Asc == false)
                {

                    switch (sortParams.SortPram)
                    {
                        case "Price":
                            offers = offers.OrderByDescending(x => x.Price);
                            if (sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.Price);
                            }
                            offers = sortParams.toPrice != null ? offers.Where(r => r.Price <= (decimal)sortParams.toPrice.Value).OrderByDescending(x => x.Price) : offers.OrderByDescending(x => x.Price);
                            if (sortParams.fromDate != null && sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.Price);
                            }
                            if (sortParams.fromDate != null && sortParams.toPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.Price);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderByDescending(x => x.Price);
                            }

                            break;
                        case "Rating":
                            if (sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.User.OverAllrating);
                            }
                            offers = sortParams.toPrice != null ? offers.Where(r => r.Price <= (decimal)sortParams.toPrice.Value).OrderByDescending(x => x.Price) : offers.OrderByDescending(x => x.User.OverAllrating);
                            if (sortParams.fromDate != null && sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.User.OverAllrating);
                            }
                            if (sortParams.fromDate != null && sortParams.toPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.User.OverAllrating);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderByDescending(x => x.User.OverAllrating);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderByDescending(x => x.User.OverAllrating);
                            }

                            offers = offers.OrderByDescending(x => x.User.OverAllrating);
                            break;
                        case "Date":
                            if (sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.PublishDate);
                            }
                            if (sortParams.toPrice != null)
                            {
                                offers = offers.Where(r => r.Price <= (decimal)sortParams.toPrice.Value).OrderByDescending(x => x.PublishDate);
                            }
                            if (sortParams.fromDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value).OrderByDescending(x => x.PublishDate);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => sortParams.toDate != null && DbFunctions.TruncateTime(r.PublishDate) <= sortParams.toDate.Value).OrderByDescending(x => x.PublishDate);
                            }
                            offers = offers.OrderByDescending(x => x.PublishDate);
                            break;

                        default:
                            offers = offers.OrderByDescending(x => x.PublishDate);
                            break;
                    }
                }
                if (sortParams.Asc == true)
                {
                    switch (sortParams.SortPram)
                    {
                        case "Price":
                            offers = offers.OrderBy(x => x.Price);
                            if (sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => r.Price >= (decimal)sortParams.fromPrice.Value).OrderBy(x => x.Price);
                            }
                            offers = sortParams.toPrice != null ? offers.Where(r => r.Price <= (decimal)sortParams.toPrice.Value).OrderBy(x => x.Price) : offers.OrderBy(x => x.Price);
                            if (sortParams.fromDate != null && sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderBy(x => x.Price);
                            }
                            if (sortParams.fromDate != null && sortParams.toPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderBy(x => x.Price);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderBy(x => x.Price);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderBy(x => x.Price);
                            }

                            break;
                        case "Rating":
                            if (sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => r.Price >= (decimal)sortParams.fromPrice.Value).OrderBy(x => x.User.OverAllrating);
                            }
                            offers = sortParams.toPrice != null ? offers.Where(r => r.Price <= (decimal)sortParams.toPrice.Value).OrderBy(x => x.Price) : offers.OrderBy(x => x.User.OverAllrating);
                            if (sortParams.fromDate != null && sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderBy(x => x.User.OverAllrating);
                            }
                            if (sortParams.fromDate != null && sortParams.toPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderBy(x => x.User.OverAllrating);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderBy(x => x.User.OverAllrating);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderBy(x => x.User.OverAllrating);
                            }
                            offers = offers.OrderBy(x => x.User.OverAllrating);
                            break;
                        case "Date":
                            if (sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => r.Price >= (decimal)sortParams.fromPrice.Value).OrderBy(x => x.PublishDate);
                            }
                            if (sortParams.toPrice != null)
                            {
                                offers = offers.Where(r => r.Price <= (decimal)sortParams.toPrice.Value).OrderBy(x => x.PublishDate);
                            }
                            if (sortParams.fromDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value).OrderBy(x => x.PublishDate);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => sortParams.toDate != null && DbFunctions.TruncateTime(r.PublishDate) <= sortParams.toDate.Value).OrderBy(x => x.PublishDate);
                            }
                            offers = offers.OrderByDescending(x => x.PublishDate);
                            break;
                        default:
                            offers = offers.OrderBy(x => x.PublishDate);
                            break;
                    }
                }
            }

            // sort direction

            var baseQuery = offers.Skip(pageSize * ((page ?? 1) - 1)).Take(pageSize).Select(x => new OfferViewModel
            {
                Id = x.Id,
                OfferDate = x.OfferDate,
                OfferNumber = x.OfferNumber,
                Title = x.Title,
                Description = x.Description,
                Price = x.Price,
                quantity = x.Quantity,
                Period = x.Period,
                ServiceProvider = factory.Create(x.User),
                //   ServiceProviderRating = x.User.OverAllrating.HasValue ? x.User.OverAllrating.Value : 0,
                PublishDate = x.PublishDate,
                ExpireDate = x.ExpireDate.Value,
                RegionId = x.RegionId.HasValue ? x.RegionId.Value : 0,
                ImageUrl = x.ImageUrl != null ? path + x.ImageUrl : null,
                TruckTypeId = x.TruckTypeId,
                TruckTypeName = x.TruckType.Name,
                MaxCustomerNumbers = x.MaxCustomerNumbers,
                MaxExpireDate = x.MaxExpireDate,
                Region = factory.Create(x.Region),
                OfferDetails = x.OfferDetails.Select(ODetails => new OfferDetailViewModel { truckId = ODetails.TruckId, truckNameArb = ODetails.Truck != null ? ODetails.Truck.NameArb : "", truckName = ODetails.Truck != null ? ODetails.Truck.Name : "", ParenttruckName = ODetails.Truck.Paranet != null ? ODetails.Truck.Paranet.Name : "", ParenttruckNameArb = ODetails.Truck.Paranet != null ? ODetails.Truck.Paranet.NameArb : "", trucksNo = ODetails.NumberOfTrucks, ImagePath = ODetails.Truck.TruckImagePath != null ? BaseURL + ODetails.Truck.TruckImagePath : null })

            });
            var totalcount = offers.Count();

            var pagesCount = Math.Ceiling((double)totalcount / pageSize);

            var sortpageValue = HttpUtility.ParseQueryString(Request.RequestUri.Query).Get("page");
            int currentPage;
            if (!int.TryParse(sortpageValue, out currentPage))
            {
                currentPage = 0;
            }

            var prevLink = currentPage > 0
                ? Request.RequestUri.AbsoluteUri.Split('?')[0] + '/' + (currentPage - 1)
                : "";
            var nextLink = currentPage < pagesCount - 1
                ? Request.RequestUri.AbsoluteUri.Split('?')[0] + '/' + (currentPage + 1)
                : "";

            return
                new
                {
                    totalCount = totalcount,
                    pagesNumber = pagesCount,
                    CurrentPage = currentPage,
                    PrevPage = prevLink,
                    NextPage = nextLink,
                    Result = baseQuery
                };
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Request"></param>
        /// <param name="sortParams"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public object OffersForUser(int UserId, HttpRequestMessage Request, SortingParamsViewModer sortParams, int? page = null)
        {
            // var urlHelper = new UrlHelper(request);
            int pageSize = sortParams.PageSize;
            string text = sortParams.SearchTerm;
            decimal fromprice;
            decimal.TryParse(sortParams.fromPrice.ToString(), out fromprice);
            decimal toprice;
            decimal.TryParse(sortParams.toPrice.ToString(), out toprice);
            decimal offerPrice = 0;
            if (sortParams.SearchTerm != null)
            {

                decimal.TryParse(sortParams.SearchTerm.ToString(), out offerPrice);
            }
            int OfferNumber = 0;
            if (sortParams.SearchTerm != null)
            {

                int.TryParse(sortParams.SearchTerm.ToString(), out OfferNumber);
            }
            int OfferId = 0;
            if (sortParams.SearchTerm != null)
            {

                int.TryParse(sortParams.SearchTerm.ToString(), out OfferId);
            }
            //  int flage = 0;

            //&& (x.AcceptOffers.Any(y => y.OfferState == sortParams.Stat)
            var offers =
                _uow.Offer.GetAll(
                    x =>
                        x.FlgStatus == 1 && x.UserId == UserId && x.User.CustomerType == CustomerTypes.ServiceProvider
                        , null, "OfferDetails ");
            // Get Current Offer
            if (sortParams.flage == 0)
            {
                offers = offers.Where(x => DbFunctions.TruncateTime(x.ExpireDate) >= DbFunctions.TruncateTime(DateTime.Now)).OrderByDescending(x => x.Id);
            }
            //Get Expire Offer
            if (sortParams.flage == 1)
            {
                offers = offers.Where(x => DbFunctions.TruncateTime(x.ExpireDate) <= DbFunctions.TruncateTime(DateTime.Now)).OrderByDescending(x => x.Id);
            }

            if (sortParams.Rating.HasValue && sortParams.Rating.Value != 0)
            {
                offers = offers.Where(r => r.User.OverAllrating == int.Parse(sortParams.Rating.ToString())).OrderByDescending(x => x.Id);
            }
            if (sortParams.Rating.HasValue && sortParams.Rating.Value == 0)
            {
                offers = offers.Where(r => r.User.OverAllrating == null);
            }
            if (sortParams.fromDate != null)
                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value).OrderByDescending(x => x.Id);

            if (sortParams.toDate != null)
                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= sortParams.toDate.Value).OrderByDescending(x => x.Id);

            if (sortParams.fromPrice != null)
                offers = offers.Where(r => r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.Id);

            if (sortParams.toPrice != null)
                offers = offers.Where(r => r.Price <= (decimal)sortParams.toPrice.Value);
            if (sortParams.Address != null)
                offers = offers.Where(r => r.Region.Name.Contains(sortParams.Address) || r.Region.Name.Equals(sortParams.Address)).OrderByDescending(x => x.Id);
            if (sortParams.SearchTerm != null)
                offers = offers.Where(r => r.Title.ToLower().Contains(sortParams.SearchTerm.ToLower()) || r.Description.ToLower().Contains(text.ToLower()) || r.Period.ToLower().Contains(text.ToLower()) || r.Quantity.ToLower().Contains(text.ToLower()) || r.Price == offerPrice || r.OfferNumber == OfferNumber || r.Id == OfferId).OrderByDescending(x => x.Id);


            //Sorting
            if (sortParams.SortPram != null)
            {
                if (sortParams.Asc == false)
                {

                    switch (sortParams.SortPram)
                    {
                        case "Price":

                            if (sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.Price).ThenByDescending(x => x.Id);
                            }
                            offers = sortParams.toPrice != null ? offers.Where(r => r.Price <= (decimal)sortParams.toPrice.Value).OrderByDescending(x => x.Price).ThenByDescending(x => x.Id) : offers.OrderByDescending(x => x.Price).ThenByDescending(x => x.Id);
                            if (sortParams.fromDate != null && sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.Price).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.fromDate != null && sortParams.toPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.Price).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderByDescending(x => x.Price).ThenByDescending(x => x.Id);
                            }
                            offers = offers.OrderByDescending(x => x.Price).ThenByDescending(x => x.Id);
                            break;
                        case "Rating":
                            if (sortParams.Rating != 0)
                            {
                                offers = offers.Where(r => r.User.OverAllrating == sortParams.Rating).OrderByDescending(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            offers = sortParams.toPrice != null ? offers.Where(r => r.Price <= (decimal)sortParams.toPrice.Value).OrderByDescending(x => x.Price).ThenByDescending(x => x.Id) : offers.OrderByDescending(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            if (sortParams.fromDate != null && sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.fromDate != null && sortParams.toPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderByDescending(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderByDescending(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }

                            offers = offers.OrderByDescending(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            break;
                        case "Date":
                            if (sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.PublishDate).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toPrice != null)
                            {
                                offers = offers.Where(r => r.Price <= (decimal)sortParams.toPrice.Value).OrderByDescending(x => x.PublishDate).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.fromDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value).OrderByDescending(x => x.PublishDate).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= sortParams.toDate.Value).OrderByDescending(x => x.PublishDate).ThenByDescending(x => x.Id);
                            }
                            offers = offers.OrderByDescending(x => x.PublishDate);
                            break;
                        case "Region":

                            if (sortParams.Address != null)
                            {
                                offers = offers.Where(r => r.Address.ToLower().StartsWith(sortParams.Address.ToLower())).OrderByDescending(x => x.Id);
                            }

                            //if (sortParams.fromDate != null && sortParams.fromPrice != null)
                            //{
                            //    offers = offers.OrderByDescending(x => x.Price).Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).AsQueryable();
                            //}
                            //if (sortParams.fromDate != null && sortParams.toPrice != null)
                            //{
                            //    offers = offers.OrderByDescending(x => x.Price).Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).AsQueryable();
                            //}
                            //if (sortParams.toDate != null)
                            //{
                            //    offers = offers.OrderByDescending(x => x.Price).Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).AsQueryable();
                            //}
                            //if (sortParams.toDate != null)
                            //{
                            //    offers = offers.OrderByDescending(x => x.Price).Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).AsQueryable();
                            //}
                            offers = offers.OrderByDescending(x => x.Region.Name).ThenByDescending(x => x.Id);
                            break;

                        default:
                            offers = offers.OrderByDescending(x => x.PublishDate).ThenByDescending(x => x.Id);
                            break;
                    }
                }
                if (sortParams.Asc == true)
                {
                    switch (sortParams.SortPram)
                    {
                        case "Region":

                            if (sortParams.Address != null)
                            {
                                offers = offers.Where(r => r.Address.StartsWith(sortParams.Address) || r.Address.Contains(sortParams.Address) || r.Address.Equals(sortParams.Address)).OrderByDescending(x => x.Id);
                            }

                            //if (sortParams.fromDate != null && sortParams.fromPrice != null)
                            //{
                            //    offers = offers.OrderBy(x => x.Price).Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).AsQueryable();
                            //}
                            //if (sortParams.fromDate != null && sortParams.toPrice != null)
                            //{
                            //    offers = offers.OrderBy(x => x.Price).Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).AsQueryable();
                            //}
                            //if (sortParams.toDate != null)
                            //{
                            //    offers = offers.OrderBy(x => x.Price).Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).AsQueryable();
                            //}
                            //if (sortParams.toDate != null)
                            //{
                            //    offers = offers.OrderBy(x => x.Price).Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).AsQueryable();
                            //}
                            offers = offers.OrderBy(x => x.Region.Name).OrderByDescending(x => x.Id);
                            break;
                        case "Price":

                            if (sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => r.Price >= (decimal)sortParams.fromPrice.Value).OrderBy(x => x.Price).OrderByDescending(x => x.Id);
                            }
                            //offers = sortParams.toPrice != null ? offers.OrderBy(x => x.Price).Where(r => r.Price <= (decimal)sortParams.toPrice.Value).AsQueryable() : offers.OrderBy(x => x.Price).AsQueryable();
                            //if (sortParams.fromDate != null && sortParams.fromPrice != null)
                            //{
                            //    offers = offers.OrderBy(x => x.Price).Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).AsQueryable();
                            //}
                            //if (sortParams.fromDate != null && sortParams.toPrice != null)
                            //{
                            //    offers = offers.OrderBy(x => x.Price).Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).AsQueryable();
                            //}
                            if (sortParams.toPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderBy(x => x.Price).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderBy(x => x.Price).ThenByDescending(x => x.Id);
                            }
                            offers = offers.OrderBy(x => x.Price).ThenByDescending(x => x.Id);
                            break;
                        case "Rating":
                            if (sortParams.Rating != 0)
                            {
                                offers = offers.Where(r => r.User.OverAllrating == int.Parse(sortParams.Rating.ToString())).OrderBy(x => x.User.OverAllrating).OrderByDescending(x => x.Id);
                            }
                            if (sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => r.Price >= (decimal)sortParams.fromPrice.Value).OrderBy(x => x.User.OverAllrating).OrderByDescending(x => x.Id);
                            }
                            offers = sortParams.toPrice != null ? offers.Where(r => r.Price <= (decimal)sortParams.toPrice.Value).OrderBy(x => x.Price).ThenByDescending(x => x.Id) : offers.OrderBy(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            if (sortParams.fromDate != null && sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderBy(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.fromDate != null && sortParams.toPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderBy(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderBy(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderBy(x => x.User.OverAllrating).OrderByDescending(x => x.Id);
                            }
                            offers = offers.OrderBy(x => x.User.OverAllrating).OrderByDescending(x => x.Id);
                            break;
                        case "Date":
                            if (sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => r.Price >= (decimal)sortParams.fromPrice.Value).OrderBy(x => x.PublishDate).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toPrice != null)
                            {
                                offers = offers.Where(r => r.Price <= (decimal)sortParams.toPrice.Value).OrderBy(x => x.PublishDate).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.fromDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value).OrderBy(x => x.PublishDate).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => sortParams.toDate != null && DbFunctions.TruncateTime(r.PublishDate) <= sortParams.toDate.Value).OrderBy(x => x.PublishDate).ThenByDescending(x => x.Id);
                            }
                            offers = offers.OrderBy(x => x.PublishDate).ThenByDescending(x => x.Id);
                            break;
                        default:
                            offers = offers.OrderBy(x => x.PublishDate).ThenByDescending(x => x.Id);
                            break;
                    }
                }

            }

            var baseQuery = offers.ToList().Skip(((page ?? 1) - 1) * pageSize).Take(pageSize).Select(x => new OfferViewModel
            {
                Id = x.Id,
                OfferDate = x.OfferDate,
                OfferNumber = x.OfferNumber,
                Title = x.Title,
                Description = x.Description,
                Price = x.Price,
                quantity = x.Quantity,
                Period = x.Period,
                ServiceProvider = factory.Create(x.User),
                //   ServiceProviderRating = x.User.OverAllrating.HasValue ? x.User.OverAllrating.Value : 0,
                PublishDate = x.PublishDate,
                ExpireDate = x.ExpireDate.Value,
                RegionId = x.RegionId.HasValue ? x.RegionId.Value : 0,
                ImageUrl = x.ImageUrl != null ? path + x.ImageUrl : null,
                TruckTypeId = x.TruckTypeId,
                TruckTypeName = x.TruckType.Name,
                MaxCustomerNumbers = x.MaxCustomerNumbers,
                MaxExpireDate = x.MaxExpireDate,
                Region = factory.Create(x.Region),
                OfferDetails = x.OfferDetails.Select(ODetails => new OfferDetailViewModel { truckId = ODetails.TruckId, truckName = ODetails.Truck != null ? ODetails.Truck.Name : "", truckNameArb = ODetails.Truck != null ? ODetails.Truck.NameArb : "", ParenttruckName = ODetails.Truck.Paranet != null ? ODetails.Truck.Paranet.Name : "", ParenttruckNameArb = ODetails.Truck.Paranet != null ? ODetails.Truck.Paranet.NameArb : "", trucksNo = ODetails.NumberOfTrucks, ImagePath = ODetails.Truck.TruckImagePath != null ? BaseURL + ODetails.Truck.TruckImagePath : null, OfferTrucks = factory.Create(ODetails.Truck) }),
                Count = x.AcceptOffers.Where(y => y.FlgStatus == 1 && y.OfferId == x.Id && (y.OfferState == OfferState.Accepted || y.OfferState == OfferState.Done)).Count(),
                State = x.AcceptOffers.Where(y => y.AcceptedUserId == UserId).Select(c => c.OfferState).FirstOrDefault(),
                AcceptedUsers = x.AcceptOffers.Where(y => y.FlgStatus == 1 && y.OfferId == x.Id && (y.OfferState == OfferState.Accepted || y.OfferState == OfferState.Done)).Select(a => new AcceptUserviewModel { UserId = a.AcceptedUserId, UserName = a.User.FirstName + "" + a.User.LastName, UserPhone = a.User.Mobile, UserImage = a.User.ProfileImgPath != null ? Userpath + a.User.ProfileImgPath : null, acceptedDate = a.AcceptedDate.ToShortDateString() })



            });
            var totalcount = offers.Count();

            var pagesCount = Math.Ceiling((double)totalcount / pageSize);

            var pageValue = HttpUtility.ParseQueryString(Request.RequestUri.Query).Get("page");
            int currentPage;
            if (!int.TryParse(pageValue, out currentPage))
            {
                currentPage = 0;
            }

            var prevLink = currentPage > 0
                ? Request.RequestUri.AbsoluteUri.Split('?')[0] + '/' + (currentPage - 1)
                : "";
            var nextLink = currentPage < pagesCount - 1
                ? Request.RequestUri.AbsoluteUri.Split('?')[0] + '/' + (currentPage + 1)
                : "";

            return
                new
                {
                    totalCount = totalcount,
                    pagesNumber = pagesCount,

                    CurrentPage = currentPage,
                    PrevPage = prevLink,
                    NextPage = nextLink,
                    Result = baseQuery.ToList()

                };


        }


        public OfferViewModel OfferData(int OfferNo)
        {
            // here user rating Is Missing
            //var imagFullPath = HttpContext.Current.Server.MapPath("~/OffersImages/");
            //  OfferImages = x.OfferImages.Select(I => new { Image = ServerHelper.MapPathReverse(imagFullPath + I.ImageUrl) }),
            var Offer = _uow.Offer.GetAll(x => x.FlgStatus == 1 && x.OfferNumber == OfferNo, null, "OfferDetails").ToList().Select(x => new OfferViewModel
            {
                Title = x.Title,
                Description = x.Description,
                IsActive = x.IsActive,
                Price = x.Price,
                quantity = x.Quantity,
                Period = x.Period,
                PublishDate = x.PublishDate,
                ExpireDate = x.ExpireDate.Value,
                ImageUrl = x.ImageUrl != null ? path + x.ImageUrl : null,
                RegionId = x.RegionId.HasValue ? x.RegionId.Value : 0,
                Address = x.Address,
                OfferDetails = x.OfferDetails.Select(od => new OfferDetailViewModel { truckId = od.TruckId, truckName = od.Truck != null ? od.Truck.Name : "", trucksNo = od.NumberOfTrucks }),
                ServiceProviderUserId = x.UserId,
                ServiceProvider = factory.Create(x.User)
            }).FirstOrDefault();
            if (Offer == null)
            {
                throw new Exception("004");
            }
            return Offer;
        }
        private void ExtractAuthHeader()
        {
            try
            {
                var headers = System.Web.HttpContext.Current.Request.Headers;
                var tokn = headers.Get("Authorization");

                _token = tokn;
                //string lang = headers.Get("Accept-Language");


                //if (lang != null && lang.ToLower() == "ar-sa")   //check for null to ensure no errors if lang doen't supplied
                //    _languageId = 2;
                //else
                //    _languageId = 1;

                //_workContext.WorkingLanguage = _languageService.GetLanguageById(_languageId);

                ////var msg = _localizationService.GetResource("account.accountactivation", _languageId);
                if (!string.IsNullOrEmpty(tokn))
                {
                    ExtractUser(tokn);
                    //_deviceToken = _User.DeviceToken;//headers.Get("DeviceToken");
                    //_serviceProvider = _User.ServiceProvider;
                }
            }
            catch (Exception ex)
            {
                _isAuthorized = false;
            }
        }
        private void ExtractUser(string tokn)
        {
            string clear = Base64Decode(tokn);
            //  _User = JsonConvert.DeserializeObject<RegisterationModel>(clear);

        }



        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }



        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        //hayam
        public object OffersonHome(int? UserId, HttpRequestMessage Request, SortingParamsViewModer sortParams, int? page)
        {
            string text = sortParams.SearchTerm;
            int pageSize = sortParams.PageSize;
            bool? userAuth = false;

            decimal fromprice;
            decimal.TryParse(sortParams.fromPrice.ToString(), out fromprice);
            decimal toprice;
            decimal.TryParse(sortParams.toPrice.ToString(), out toprice);
            decimal offerPrice = 0;
            if (sortParams.SearchTerm != null)
            {

                decimal.TryParse(sortParams.SearchTerm.ToString(), out offerPrice);
            }
            int OfferNumber = 0;
            if (sortParams.SearchTerm != null)
            {

                int.TryParse(sortParams.SearchTerm.ToString(), out OfferNumber);
            }
            int OfferId = 0;
            if (sortParams.SearchTerm != null)
            {

                int.TryParse(sortParams.SearchTerm.ToString(), out OfferId);
            }
            int RatingValue = 0;
            if (sortParams.Rating != null)
            {

                int.TryParse(sortParams.Rating.ToString(), out RatingValue);
            }

            var headers = System.Web.HttpContext.Current.Request.Headers;
            var tokn = headers.Get("Authorization");
            if (tokn != null)
            {
                userAuth = true;
            }
            var offers = _uow.Offer.GetAll(x => x.FlgStatus == 1 && x.IsActive == true, null, "OfferDetails,AcceptOffers")
                .Where(r => DbFunctions.TruncateTime(r.ExpireDate) >= DbFunctions.TruncateTime(DateTime.Now) && (r.AcceptOffers.Any(af => af.AcceptedUserId == UserId.Value) == false));

            if (sortParams != null)
            {
                offers = offers.OrderByDescending(x => x.Id);

            }
            // searching
            if (sortParams.Rating.HasValue)
            {
                offers = offers.Where(r => r.User.OverAllrating == RatingValue).OrderByDescending(x => x.Id);
            }
            if (sortParams.fromDate != null)
                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate).OrderByDescending(x => x.Id);

            if (sortParams.toDate != null)
                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= sortParams.toDate.Value).OrderByDescending(x => x.Id);

            if (sortParams.fromPrice != null)
                offers = offers.Where(r => r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.Id);

            if (sortParams.toPrice != null)
                offers = offers.Where(r => r.Price <= (decimal)sortParams.toPrice.Value).OrderByDescending(x => x.Id);
            if (sortParams.Address != null)
                offers = offers.Where(r => r.Region.Name.Contains(sortParams.Address) || r.Region.Name.StartsWith(sortParams.Address) || r.Region.Name.Equals(sortParams.Address)).OrderByDescending(x => x.Id);
            if (sortParams.SearchTerm != null)
                offers = offers.Where(r => r.Title.ToLower().Contains(sortParams.SearchTerm.ToLower()) || r.OfferNumber == OfferNumber || r.Description.ToLower().Contains(text.ToLower()) || r.Period.ToLower().Contains(text.ToLower()) || r.Quantity.ToLower().Contains(text.ToLower()) || r.Price == offerPrice || r.Id == OfferId).OrderByDescending(x => x.Id);


            //Sorting
            if (sortParams.SortPram != null)
            {
                if (sortParams.Asc == false)
                {

                    switch (sortParams.SortPram)
                    {
                        case "Price":

                            if (sortParams.fromDate != null && sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.Price).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.fromDate != null && sortParams.toPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.Price).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderByDescending(x => x.Price).ThenByDescending(x => x.Id);
                            }
                            offers = offers.OrderByDescending(x => x.Price);
                            break;
                        case "Rating":
                            if (sortParams.Rating == 0)
                            {
                                offers = offers.OrderByDescending(x => x.User.OverAllrating);
                            }
                            if (sortParams.Rating != 0)
                            {
                                offers = offers.OrderByDescending(x => x.User.OverAllrating);
                            }
                            if (sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            offers = sortParams.toPrice != null ? offers.Where(r => r.Price <= (decimal)sortParams.toPrice.Value).OrderByDescending(x => x.Price) : offers.OrderByDescending(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            if (sortParams.fromDate != null && sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.fromDate != null && sortParams.toPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderByDescending(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderByDescending(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }

                            offers = offers.OrderByDescending(x => x.User.OverAllrating);
                            break;
                        case "Date":
                            if (sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.PublishDate).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toPrice != null)
                            {
                                offers = offers.Where(r => r.Price <= (decimal)sortParams.toPrice.Value).OrderByDescending(x => x.PublishDate).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.fromDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value).OrderByDescending(x => x.PublishDate).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= sortParams.toDate.Value).OrderByDescending(x => x.PublishDate).ThenByDescending(x => x.Id);
                            }
                            offers = offers.OrderByDescending(x => x.PublishDate);
                            break;
                        case "Region":

                            if (sortParams.Address != null)
                            {
                                offers = offers.Where(r => r.Address.StartsWith(sortParams.Address)).OrderByDescending(x => x.Region.Name).ThenByDescending(x => x.Id);
                            }

                            if (sortParams.fromDate != null && sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.Price).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.fromDate != null && sortParams.toPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderByDescending(x => x.Price).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderByDescending(x => x.Price).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderByDescending(x => x.Price).ThenByDescending(x => x.Id);
                            }
                            offers = offers.OrderByDescending(x => x.Region.Name).ThenByDescending(x => x.Id);
                            break;

                        default:
                            offers = offers.OrderByDescending(x => x.PublishDate).ThenByDescending(x => x.Id);
                            break;
                    }
                }
                if (sortParams.Asc == true)
                {
                    switch (sortParams.SortPram)
                    {
                        case "Region":

                            if (sortParams.Address != null)
                            {
                                offers = offers.Where(r => r.Address.StartsWith(sortParams.Address)).OrderBy(x => x.Region.Name).ThenByDescending(x => x.Id);
                            }

                            if (sortParams.fromDate != null && sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderBy(x => x.Price).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.fromDate != null && sortParams.toPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderBy(x => x.Price).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderBy(x => x.Price).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderBy(x => x.Price).ThenByDescending(x => x.Id);
                            }
                            offers = offers.OrderBy(x => x.Region.Name);
                            break;
                        case "Price":

                            if (sortParams.fromPrice != null)
                            {
                                offers = offers.OrderBy(x => x.Price);
                                // offers = offers.OrderBy(x => x.Price).Where(r => r.Price >= (decimal)sortParams.fromPrice.Value);
                            }
                            offers = sortParams.toPrice != null ? offers.Where(r => r.Price <= (decimal)sortParams.toPrice.Value).OrderBy(x => x.Price) : offers.OrderBy(x => x.Price).ThenByDescending(x => x.Id);
                            if (sortParams.fromDate != null && sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderBy(x => x.Price).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.fromDate != null && sortParams.toPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderBy(x => x.Price).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderBy(x => x.Price).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderBy(x => x.Price).ThenByDescending(x => x.Id);
                            }
                            offers = offers.OrderBy(x => x.Price);
                            break;
                        case "Rating":
                            if (sortParams.Rating == 0)
                            {
                                offers = offers.OrderBy(x => x.User.OverAllrating).ThenBy(x => x.Id);

                            }
                            if (sortParams.Rating != 0)
                            {

                                offers = offers.Where(r => r.User.OverAllrating == int.Parse(sortParams.Rating.ToString())).OrderBy(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => r.Price >= (decimal)sortParams.fromPrice.Value).OrderBy(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            offers = sortParams.toPrice != null ? offers.Where(r => r.Price <= (decimal)sortParams.toPrice.Value).OrderBy(x => x.Price) : offers.OrderBy(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            if (sortParams.fromDate != null && sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderBy(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.fromDate != null && sortParams.toPrice != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value && r.Price >= (decimal)sortParams.fromPrice.Value).OrderBy(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderBy(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(sortParams.fromDate)).OrderBy(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            offers = offers.OrderBy(x => x.User.OverAllrating).ThenBy(x => x.Id);
                            break;
                        case "Date":
                            if (sortParams.fromPrice != null)
                            {
                                offers = offers.Where(r => r.Price >= (decimal)sortParams.fromPrice.Value).OrderBy(x => x.PublishDate).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toPrice != null)
                            {
                                offers = offers.Where(r => r.Price <= (decimal)sortParams.toPrice.Value).OrderBy(x => x.PublishDate).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.fromDate != null)
                            {
                                offers = offers.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= sortParams.fromDate.Value).OrderBy(x => x.PublishDate).ThenByDescending(x => x.Id);
                            }
                            if (sortParams.toDate != null)
                            {
                                offers = offers.Where(r => sortParams.toDate != null && DbFunctions.TruncateTime(r.PublishDate) <= sortParams.toDate.Value).OrderBy(x => x.PublishDate).ThenByDescending(x => x.Id);
                            }
                            offers = offers.OrderBy(x => x.PublishDate).ThenBy(x => x.Id);
                            break;
                        default:
                            offers = offers.OrderBy(x => x.PublishDate).ThenBy(x => x.Id);
                            break;

                    }

                }

            }

            var baseQuery = offers.ToList().Skip(((page ?? 1) - 1) * pageSize).Take(pageSize).Select(x => new OfferViewModel
            {
                Id = x.Id,
                OfferDate = x.OfferDate,
                OfferNumber = x.OfferNumber,
                Title = x.Title,
                Description = x.Description,
                Price = x.Price,
                quantity = x.Quantity,
                Period = x.Period,
                ServiceProvider = factory.Create(x.User),
                //   ServiceProviderRating = x.User.OverAllrating.HasValue ? x.User.OverAllrating.Value : 0,
                PublishDate = x.PublishDate,
                ExpireDate = x.ExpireDate.Value,
                RegionId = x.RegionId.HasValue ? x.RegionId.Value : 0,

                ImageUrl = x.ImageUrl != null ? path + x.ImageUrl : null,
                TruckTypeId = x.TruckTypeId,
                TruckTypeName = x.TruckType.Name,
                MaxCustomerNumbers = x.MaxCustomerNumbers,
                MaxExpireDate = x.MaxExpireDate,
                Region = factory.Create(x.Region),
                Accepted = UserId.HasValue ? checkaccepatance(UserId.Value, x.Id) : false,
                OfferDetails = x.OfferDetails.Select(ODetails => new OfferDetailViewModel { truckId = ODetails.TruckId, truckName = ODetails.Truck != null ? ODetails.Truck.Name : "", truckNameArb = ODetails.Truck != null ? ODetails.Truck.NameArb : "", ParenttruckName = ODetails.Truck.Paranet != null ? ODetails.Truck.Paranet.Name : "", ParenttruckNameArb = ODetails.Truck.Paranet != null ? ODetails.Truck.Paranet.NameArb : "", trucksNo = ODetails.NumberOfTrucks, ImagePath = ODetails.Truck.TruckImagePath != null ? BaseURL + ODetails.Truck.TruckImagePath : null, OfferTrucks = factory.Create(ODetails.Truck) }),
                Count = OfferAcceptanceCout(x.Id),
                State = x.AcceptOffers.Where(a => a.AcceptedUserId == UserId).Select(y => y.OfferState).FirstOrDefault()
            });
            var totalcount = offers.Count();

            var pagesCount = Math.Ceiling((double)totalcount / pageSize);

            var pageValue = HttpUtility.ParseQueryString(Request.RequestUri.Query).Get("page");
            int currentPage;
            if (!int.TryParse(pageValue, out currentPage))
            {
                currentPage = 0;
            }

            var prevLink = currentPage > 0
                ? Request.RequestUri.AbsoluteUri.Split('?')[0] + '/' + (currentPage - 1)
                : "";
            var nextLink = currentPage < pagesCount - 1
                ? Request.RequestUri.AbsoluteUri.Split('?')[0] + '/' + (currentPage + 1)
                : "";

            return
                new
                {
                    totalCount = totalcount,
                    pagesNumber = pagesCount,

                    CurrentPage = currentPage,
                    PrevPage = prevLink,
                    NextPage = nextLink,
                    Result = baseQuery

                };

        }

        public IEnumerable<OfferViewModel> Offers()
        {
            var today = DateTime.Today;
            var Offers = _uow.Offer.GetAll(x => x.FlgStatus == 1 && x.ExpireDate <= today, null, "OfferDetails").ToList()
                    .Select(x => new OfferViewModel
                    {
                        Id = x.Id,
                        OfferDate = x.OfferDate,
                        OfferNumber = x.OfferNumber,
                        Title = x.Title,
                        Description = x.Description,
                        Price = x.Price,
                        quantity = x.Quantity,
                        Period = x.Period,
                        ServiceProviderUserId = x.UserId,
                        ServiceProvidername = x.User.FirstName + "" + x.User.LastName,
                        ServiceProviderRating = x.User.OverAllrating.HasValue ? x.User.OverAllrating.Value : 0,
                        PublishDate = x.PublishDate,
                        ExpireDate = x.ExpireDate.Value,
                        RegionId = x.RegionId.HasValue ? x.RegionId.Value : 0,
                        ImageUrl = path + x.ImageUrl,
                        TruckTypeId = x.TruckTypeId,
                        TruckTypeName = x.TruckType.Name,
                        MaxCustomerNumbers = x.MaxCustomerNumbers,
                        MaxExpireDate = x.MaxExpireDate,
                        OfferDetails = x.OfferDetails.Select(ODetails => new OfferDetailViewModel { truckId = ODetails.TruckId, truckName = ODetails.Truck != null ? ODetails.Truck.Name : "", trucksNo = ODetails.NumberOfTrucks })
                    });
            if (Offers == null)
            {
                throw new Exception("024");
            }
            return Offers;
        }

        //public OfferDetailViewModel AddOfferDetailes(int offerid, OfferDetailViewModel model, int UserId)
        //{
        //    if (model == null)
        //    {

        //        throw new Exception("005");
        //    }
        //    if (offerid == 0)
        //    {
        //        offerid = model.OfferId;
        //    }
        //    var truck = new TrucksViewModel() { Id = model.truckId, Name = model.truckName };
        //    model.OfferTrucks = truck;
        //    var truckentity = _uow.Truck.GetById(model.truckId);
        //    var entity = factory.Parse(model);
        //    entity.OfferId = offerid;
        //    //var offer = _uow.Offer.GetById(offerid);
        //    //entity.Offer = offer;
        //    //var user = _uow.User.GetById(model.AcceptedUserId);

        //    entity.Truck = truckentity;
        //    entity.NumberOfTrucks = model.trucksNo;
        //    entity.CreatedBy = UserId;
        //    entity.TruckId = model.truckId;
        //    entity.FlgStatus = 1;
        //    entity.CreatedOn = DateTime.Now;
        //    entity.UpdatedBy = null;
        //    entity.UpdatedOn = null;

        //    _uow.OfferDetail.Add(entity);
        //    _uow.Commit();
        //    var oDetailsModel = factory.Create(entity);
        //    var offer = _uow.Offer.GetById(offerid);
        //    oDetailsModel.Offer = factory.Create(offer);
        //    oDetailsModel.AcceptedUserName = user.FirstName + "" + user.LastName;
        //    return oDetailsModel;
        //}

        //public OfferDetailViewModel EditOfferDetailes(int id, OfferDetailViewModel model, int UserId)
        //{
        //    var existedDetails = _uow.OfferDetail.GetById(id);
        //    if (existedDetails == null)
        //    {

        //        throw new Exception("004");
        //    }
        //    if (model == null)
        //    {
        //        throw new Exception("005");
        //    }
        //    var parsedEntity = factory.Parse(model);
        //    //existedDetails.AcceptedDate = parsedEntity.AcceptedDate;
        //    //existedDetails.AcceptedUserId = parsedEntity.AcceptedUserId;
        //    existedDetails.OfferId = parsedEntity.OfferId;
        //    //existedDetails.Notes = parsedEntity.Notes;
        //    if (existedDetails.TruckId != parsedEntity.TruckId && existedDetails.NumberOfTrucks != parsedEntity.NumberOfTrucks)
        //    {
        //        existedDetails.TruckId = parsedEntity.TruckId;
        //        existedDetails.NumberOfTrucks = parsedEntity.NumberOfTrucks;
        //    }
        //    existedDetails.UpdatedBy = UserId;
        //    existedDetails.UpdatedOn = DateTime.Now;
        //    _uow.OfferDetail.Update(id, existedDetails);
        //    _uow.Commit();

        //    var detailsModel = factory.Create(existedDetails);
        //    var user = _uow.User.GetById(model.AcceptedUserId);
        //    var offer = _uow.Offer.GetById(parsedEntity.OfferId);
        //    detailsModel.Offer = factory.Create(offer);
        //    detailsModel.AcceptedUserName = user.FirstName + "" + user.LastName;
        //    return detailsModel;
        //}

        //public OfferDetailViewModel DeleteOfferDetailes(int id, int UserId)
        //{
        //    var details = _uow.OfferDetail.GetById(id);
        //    if (details == null)
        //    {
        //        throw new Exception("004");
        //    }
        //    details.FlgStatus = 0;
        //    details.UpdatedBy = UserId;
        //    details.UpdatedOn = DateTime.Now.Date;
        //    _uow.OfferDetail.Update(id, details);
        //    _uow.Commit();
        //    var detailModel = factory.Create(details);
        //    return detailModel;
        //}

        public IEnumerable<OfferViewModel> Sort(string sortType)
        {
            var query = _uow.Offer.GetAll(x => x.FlgStatus == 1, null, "OfferDetails").ToList().Select(x => new OfferViewModel { Id = x.Id, OfferDate = x.OfferDate, OfferNumber = x.OfferNumber, Title = x.Title, Description = x.Description, Price = x.Price, quantity = x.Quantity, Period = x.Period, ImageUrl = path + x.ImageUrl, PublishDate = x.PublishDate, ExpireDate = x.ExpireDate.Value, TruckTypeId = x.TruckTypeId, TruckTypeName = x.TruckType.Name, MaxCustomerNumbers = x.MaxCustomerNumbers, MaxExpireDate = x.MaxExpireDate });
            if (sortType == "Desc")
            {
                // to Filter Data By MultiColumns
                query = query.AsQueryable().Sort(new[] { "OfferDate", "Price", "Address" }).ToList();
            }
            else { query.AsQueryable().Sort(new[] { "-OfferDate", "-Price", "-Address" }).ToList(); }
            return query;
        }

        public IEnumerable<OfferViewModel> SearchAdmin(string searchText)
        {
            decimal price;
            decimal.TryParse(searchText, out price);
            DateTime date;
            DateTime.TryParse(searchText, out date);
            int offerno = 0;
            int.TryParse(searchText, out offerno);
            var offer = _uow.Offer.GetAll(x => x.FlgStatus == 1).ToList().OrderByDescending(x => x.Id).ThenByDescending(x => x.PublishDate);

            if (searchText == null)
            {
                return offer.Select(o => new OfferViewModel
                {
                    Id = o.Id,
                    OfferDate = o.OfferDate,
                    OfferNumber = o.OfferNumber,
                    Title = o.Title,
                    Description = o.Description,
                    Price = o.Price,
                    quantity = o.Quantity,
                    Period = o.Period,
                    RegionName = o.Region.Name,
                    PublishDate = o.PublishDate,
                    ExpireDate = o.ExpireDate.Value,
                    TruckType = factory.Create(o.TruckType),
                    MaxExpireDate = o.MaxExpireDate,
                    IsActive = o.IsActive,
                    ServiceProvider = factory.Create(o.User),
                    Region = factory.Create(o.Region)

                });


            }
            else
            {
                return offer.Where(x => x.Id.Equals(offerno) || x.OfferNumber.Equals(offerno) || x.Title.Contains(searchText) || x.Title.StartsWith(searchText) || x.Description.ToLower().Contains(searchText.ToLower()) || x.Description.ToLower().StartsWith(searchText.ToLower()) || x.Quantity.ToLower().Contains(searchText.ToLower()) || x.Period.ToLower().Contains(searchText.ToLower()) || x.Price == price || x.User.FirstName.Contains(searchText) || x.User.FirstName.StartsWith(searchText) || x.User.LastName.Contains(searchText) || x.User.LastName.StartsWith(searchText) || x.Region.Name.StartsWith(searchText) || x.Region.Name.Contains(searchText) || (x.OfferDate.Year.Equals(date.Year) && x.OfferDate.Month.Equals(date.Month) && x.OfferDate.Day.Equals(date.Day)) || (x.ExpireDate.Value.Year.Equals(date.Year) && x.ExpireDate.Value.Month.Equals(date.Month) && x.ExpireDate.Value.Day.Equals(date.Day))).Select(o => new OfferViewModel
                {
                    Id = o.Id,
                    OfferDate = o.OfferDate,
                    OfferNumber = o.OfferNumber,
                    Title = o.Title,
                    Description = o.Description,
                    Price = o.Price,
                    quantity = o.Quantity,
                    Period = o.Period,
                    RegionName = o.Region.Name,
                    PublishDate = o.PublishDate,
                    ExpireDate = o.ExpireDate.Value,
                    TruckType = factory.Create(o.TruckType),
                    MaxExpireDate = o.MaxExpireDate,
                    IsActive = o.IsActive,
                    ServiceProvider = factory.Create(o.User),
                    Region = factory.Create(o.Region)

                });
            }


            //  return null;
            //List<OfferViewModel> offerobj = null;

            //// DateTime searchdate = DateTime.Parse(searchText);
            //if (searchText != null)
            //{

            //    decimal price = 0;
            //    decimal.TryParse(searchText.ToString(), out price);
            //    DateTime searchdate;
            //    DateTime.TryParse(searchText.ToString(), out searchdate);
            //    var offersData = _uow.Offer.GetAll(x => x.FlgStatus == 1).ToList().Where(x => x.Title.ToLower().Contains(searchText.ToLower()) || x.Description.ToLower().Contains(searchText.ToLower()) || x.Price <= price
            //    || x.ExpireDate.HasValue ? x.ExpireDate.Value.Equals(searchdate) : false).Select(o => new OfferViewModel
            //    {
            //        Id = o.Id,
            //        OfferDate = o.OfferDate,
            //        OfferNumber = o.OfferNumber,
            //        Title = o.Title,
            //        Description = o.Description,
            //        Price = o.Price,
            //        quantity = o.Quantity,
            //        PublishDate = o.PublishDate,
            //        ExpireDate = o.ExpireDate.Value,
            //        TruckTypeId = o.TruckTypeId,
            //        TruckTypeName = o.TruckType.Name,
            //        MaxExpireDate = o.MaxExpireDate,
            //        IsActive = o.IsActive,
            //        //TruckType = factory.Create(o.TruckType),                   
            //        ServiceProvider = factory.Create(o.User),
            //        Region = factory.Create(o.Region),
            //        ImageUrl = ServerHelper.MapPathReverse(imagFullPath) + o.ImageUrl

            //    });
            //    return offersData;
            //}
            //return _uow.Offer.GetAll(x => x.FlgStatus == 1).ToList().Select(c => new OfferViewModel { });

        }

        public object Contacts(int userId)
        {
            var userContacts = _uow.User.GetAll(x => x.FlgStatus == 1, null, "").Where(u => u.Id == userId && u.CustomerType == CustomerTypes.ServiceProvider).Select(U => new { Mobile = U.Mobile, Email = U.Email, Address = U.Address }).FirstOrDefault();
            if (userContacts == null)
            {
                throw new Exception("014");
            }
            return userContacts;
        }

        public ServiceProviderRatingViewModel AddRating(int ServiceProviderId, ServiceProviderRatingViewModel ratingModel, int UserId)
        {

            var entity = factory.Parse(ratingModel);
            if (entity == null)
            {
                throw new Exception("005");
            }
            entity.ServiceProviderId = ServiceProviderId;
            entity.Date = DateTime.Now;
            entity.RequstId = ratingModel.RequstId;
            entity.ServiceRequestId = ratingModel.RequsterId;
            entity.CreatedBy = UserId;
            entity.CreatedOn = DateTime.Now.Date;
            entity.FlgStatus = 1;
            entity.UpdatedBy = null;
            entity.UpdatedOn = null;
            _uow.Rating.Add(entity);
            _uow.Commit();
            var model = factory.Create(entity);
            var ratingCount = _uow.Rating.GetAll(x => x.FlgStatus == 1 && x.ServiceProviderId == ServiceProviderId, null, "").Count();
            var ratingavg = _uow.Rating.GetAll(x => x.FlgStatus == 1 && x.ServiceProviderId == ServiceProviderId, null, "").Average(s => s.RatingValue);
            var serviceprovider = _uow.User.GetById(ServiceProviderId);
            serviceprovider.OverAllrating = Convert.ToDecimal(Math.Round(ratingavg));
            _uow.User.Update(ServiceProviderId, serviceprovider);
            _uow.Commit();
            model.OverAllRating = ratingCount;
            model.AvgRating = Math.Round(Convert.ToDouble(ratingavg));
            return model;
        }

        public object RatingDetailes(int ServiceProviderId)
        {
            var detailes = _uow.Rating.GetAll(x => x.FlgStatus == 1 && x.ServiceProviderId == ServiceProviderId).Select(D => new { SerivceProvider = D.ServiceProvider.FirstName + "" + D.ServiceProvider.LastName, RequesterId = D.RequstId, RequesterName = D.ServiceRequest.FirstName + "" + D.ServiceRequest.LastName, Rating = D.RatingValue, Date = D.Date, Description = D.Description });
            return detailes;
        }

        public long MaxOfferNumber()
        {
            var no = _uow.Offer.GetAll(null, null, "").Max(x => x.OfferNumber);
            return no + 1;
        }

        public int Count(int UserId)
        {
            var count = _uow.Offer.GetAll(x => x.FlgStatus == 1).Count(y => y.UserId == UserId);
            return count;
        }

        public OfferViewModel OfferDetailes(int OfferId)
        {
            var Offer = _uow.Offer.GetAll(x => x.FlgStatus == 1 && x.Id == OfferId, null, "OfferDetails").ToList().Select(x => new OfferViewModel
            {
                Title = x.Title,
                Description = x.Description,
                Price = x.Price,
                quantity = x.Quantity,
                Period = x.Period,
                PublishDate = x.PublishDate,
                ExpireDate = x.ExpireDate.Value,
                ImageUrl = path + x.ImageUrl,
                Address = x.Address,
                OfferDetails = x.OfferDetails.Select(od => new OfferDetailViewModel { truckId = od.TruckId, truckName = od.Truck != null ? od.Truck.Name : "", truckNameArb = od.Truck != null ? od.Truck.NameArb : "", ParenttruckName = od.Truck.Paranet != null ? od.Truck.Paranet.Name : "", ParenttruckNameArb = od.Truck.Paranet != null ? od.Truck.Paranet.NameArb : "", truckTypeName = _uow.Truck.GetById(od.TruckId).TruckType.Name, trucksNo = od.NumberOfTrucks }),
                ServiceProviderUserId = x.UserId,
                ServiceProvidername = x.User.FirstName + "" + x.User.LastName
            }).FirstOrDefault();
            if (Offer == null)
            {
                throw new Exception("004");
            }
            return Offer;
        }

        public AcceptofferViewModel AddAcceptance(AcceptofferViewModel model, int UserId, int OfferId)
        {
            var checkuseraccepancebefore = _uow.AcceptOffer.GetAll(x => x.FlgStatus == 1).Any(y => y.AcceptedUserId == UserId && y.OfferId == OfferId);
            if (checkuseraccepancebefore)
            {
                throw new Exception("87");
            }

            var offer = _uow.Offer.GetById(OfferId);
            //if (offer.AcceptOffers.Any(a => a.OfferState == OfferState.Accepted))
            //{
            //    throw new Exception("73");
            //}
            var entity = factory.Parse(model);
            entity.AcceptedUserId = UserId;
            entity.OfferState = OfferState.Accepted;
            entity.OfferId = OfferId;
            entity.CreatedBy = UserId;
            // entity.OfferState = model.OfferState;
            entity.CreatedOn = DateTime.Now.Date;
            entity.FlgStatus = 1;
            _uow.AcceptOffer.Add(entity);
            _uow.Commit();
            var accepteduser = _uow.User.GetById(UserId);

            offer.ImageUrl = offer.ImageUrl != null ? path + offer.ImageUrl : null;
            var entitymodel = factory.Create(entity);
            entitymodel.AcceptedUserName = accepteduser.FirstName + "" + accepteduser.LastName;
            entitymodel.OfferState = entity.OfferState;
            entitymodel.Offer = factory.Create(offer);

            return entitymodel;
        }

        public AcceptofferViewModel EditAcceptance(int id, AcceptofferViewModel model, int UserId)
        {
            var existed = _uow.AcceptOffer.GetById(id);
            if (existed == null)
            {
                throw new Exception("005");
            }
            existed.AcceptedDate = model.AcceptedDate.Value;
            existed.AcceptedUserId = model.AcceptedUserId;
            existed.Notes = model.Notes;
            existed.UpdatedBy = UserId;
            existed.OfferState = model.OfferState;
            existed.UpdatedOn = DateTime.Now.Date;
            _uow.AcceptOffer.Update(existed.Id, existed);
            _uow.Commit();
            var accepteduser = _uow.User.GetById(model.AcceptedUserId);
            var offer = _uow.Offer.GetById(model.OfferId);
            var existmodel = factory.Create(existed);
            existmodel.AcceptedUserName = accepteduser.FirstName + "" + accepteduser.LastName;
            existmodel.Offer = factory.Create(offer);
            return existmodel;
        }

        public AcceptofferViewModel DeleteAcceptance(int id, int UserId)
        {
            var existed = _uow.AcceptOffer.GetById(id);
            if (existed == null)
            {
                throw new Exception("005");
            }
            existed.UpdatedBy = UserId;
            existed.UpdatedOn = DateTime.Now.Date;
            _uow.AcceptOffer.Update(existed.Id, existed);
            _uow.Commit();
            var existmodel = factory.Create(existed);
            return existmodel;
        }

        public IEnumerable<OfferViewModel> AllOffers(string search)
        {
            var allOffers = _uow.Offer.GetAll(x => x.FlgStatus == 1, null, "").Where(x => x.Title.Contains(search)).Select(o => new OfferViewModel { });
            return allOffers;
        }

        public OfferViewModel GetOfferData(int id)
        {
            //,OfferImages = o.OfferImages.Select(oI => new OfferImagesViewModel { ImageUrl = ServerHelper.MapPathReverse(imagFullPath) + oI.ImageUrl })

            var offerDetailes = _uow.Offer.GetAll(x => x.FlgStatus == 1 && x.Id == id, null, "OfferDetails,AcceptOffers").ToList().Select(o => new OfferViewModel
            {
                OfferNumber = o.OfferNumber,
                OfferDate = o.OfferDate,
                PublishDate = o.PublishDate,
                Title = o.Title,
                Description = o.Description,
                Price = o.Price,
                quantity = o.Quantity,
                Period = o.Period,
                RegionId = o.RegionId,
                RegionName = o.Region.Name,
                ImageUrl = o.ImageUrl != null ? path + o.ImageUrl : null,
                ServiceProvider = factory.Create(o.User),
                ServiceProviderRating = o.User.OverAllrating.HasValue ? o.User.OverAllrating.Value : 0,
                TruckTypeName = _uow.TruckType.GetById(o.TruckTypeId).Name,
                TruckTypeNameArb = _uow.TruckType.GetById(o.TruckTypeId).NameArb,
                TruckTypeId = o.TruckTypeId,
                OfferDetails = o.OfferDetails.Select(od => new OfferDetailViewModel { truckId = od.TruckId, truckName = od.Truck != null ? od.Truck.Name : "", truckNameArb = od.Truck != null ? od.Truck.NameArb : "", ParenttruckName = od.Truck.Paranet != null ? od.Truck.Paranet.Name : "", ParenttruckNameArb = od.Truck.Paranet != null ? od.Truck.Paranet.NameArb : "", truckTypeName = _uow.Truck.GetById(od.TruckId).TruckType.Name, trucksNo = od.NumberOfTrucks }),


                AcceptedBy = Offeracceptance(o.Id)
            }).FirstOrDefault();
            return offerDetailes;
        }

        public void DeActiveOffer(int offerId, bool state)
        {
            var existingOffer = _uow.Offer.GetById(offerId);
            if (existingOffer == null)
            {
                throw new Exception("011");
            }
            existingOffer.IsActive = state;
            _uow.Offer.Deactivate(offerId, state);
            _uow.Commit();

        }

        public IEnumerable<OfferViewModel> Search(string searchText)
        {
            var offer = _uow.Offer.GetAll(x => x.FlgStatus == 1).ToList();
            if (searchText == null)
            {
                var offers = _uow.Offer.GetAll(x => x.FlgStatus == 1).ToList().Select(o => new OfferViewModel
                {
                    Id = o.Id,
                    OfferNumber = o.OfferNumber,
                    OfferDate = o.OfferDate,
                    Title = o.Title,
                    Description = o.Description,
                    Price = o.Price,
                    quantity = o.Quantity,
                    Period = o.Period,
                    RegionName = o.Region.Name,
                    PublishDate = o.PublishDate,
                    ExpireDate = o.ExpireDate.Value,
                    TruckTypeId = o.TruckTypeId,
                    TruckTypeName = o.TruckType.Name,
                    MaxExpireDate = o.MaxExpireDate,
                    ServiceProvider = factory.Create(o.User),
                    ImageUrl = o.ImageUrl != null ? path + o.ImageUrl : null
                });
                return offers;

            }


            // DateTime searchdate = DateTime.Parse(searchText);
            if (searchText != null)
            {
                var ServiceType = _uow.ServiceType.GetAll();
                decimal price = 0;
                decimal.TryParse(searchText.ToString(), out price);
                DateTime searchdate;
                DateTime.TryParse(searchText.ToString(), out searchdate);
                var offersData = offer.Where(x => x.Title.ToLower().Contains(searchText.ToLower()) || x.Description.ToLower().Contains(searchText.ToLower()) || x.Price <= price
                || x.ExpireDate.HasValue ? x.ExpireDate.Value.Date.Equals(searchdate) : false).Select(o => new OfferViewModel
                {
                    Id = o.Id,
                    OfferDate = o.OfferDate,
                    OfferNumber = o.OfferNumber,
                    Title = o.Title,
                    Description = o.Description,
                    Price = o.Price,
                    quantity = o.Quantity,
                    PublishDate = o.PublishDate,
                    ExpireDate = o.ExpireDate.Value,
                    TruckTypeId = o.TruckTypeId,
                    TruckTypeName = o.TruckType.Name,
                    MaxExpireDate = o.MaxExpireDate,
                    ServiceProvider = factory.Create(o.User),
                    ImageUrl = path + o.ImageUrl
                });
                return offersData;
            }

            return offer.ToList().Select(c => new OfferViewModel { });

        }
        public AcceptofferViewModel UpadteOfferState(int OfferId, OfferState State, int userId)
        {
            var usersToken = new List<string>();
            var offer = _uow.AcceptOffer.GetAll(x => x.FlgStatus == 1 && x.OfferId == OfferId && x.AcceptedUserId == userId).FirstOrDefault();

            if (offer != null)
            {
                var offerdata = _uow.Offer.GetAll(x => x.FlgStatus == 1 && x.Id == OfferId, null, "").FirstOrDefault();
                if (State == OfferState.Accepted)
                {
                    offer.OfferState = OfferState.Accepted;
                    _uow.AcceptOffer.Update(offer.Id, offer);
                    _uow.Commit();

                }
                if (State == OfferState.Done)
                {
                    offer.OfferState = OfferState.Done;
                    _uow.AcceptOffer.Update(offer.Id, offer);
                    _uow.Commit();

                }
                if (State == OfferState.Rejected)
                {
                    offer.OfferState = OfferState.Rejected;
                    _uow.AcceptOffer.Update(offer.Id, offer);
                    _uow.Commit();

                }

                if (offerdata.UserId.HasValue)
                {

                    var serveiceProviderid = offerdata.UserId.Value;
                    if (serveiceProviderid != 0)
                    {

                        var devices = _customerService.GetDeviceByTokenAndType(serveiceProviderid);
                        NotificationViewModel notymodel = new NotificationViewModel();
                        if (devices.Count() > 0)
                        {
                            foreach (var item in devices)
                            {
                                if (item != null)
                                {
                                    usersToken.Clear();
                                    usersToken.Add(item.Fcmtoken);
                                    notymodel.registration_ids = usersToken;


                                }

                            }
                            notymodel.Body = "تم تغيير حاله العرض" + ":" + " " + offerdata.Title + '-' + "This Offer State has been changes " + ":" + offerdata.Title;
                            // notymodel.BodyArb = "تم تغيير حاله العرض" + offerdata.Title;
                            notymodel.Date = DateTime.Now;
                            notymodel.Title = " تغيير حاله العرض" + " " + offerdata.Title + '-' + " Offer State has been changes :" + " " + offerdata.Title;
                            notymodel.Seen = false;
                            //notymodel.DeviceToken = deviceToken;
                            notymodel.Type = NotificationType.UpdateState;
                            notymodel.BodyArb = "تم تغيير حاله العرض" + ":" + "" + offerdata.Title;
                            notymodel.BodyEng = "This Offer State has been changes " + ":" + "" + offerdata.Title;
                            notymodel.TitleArb = " تغيير حاله العرض" + " " + ":" + offerdata.Title;
                            notymodel.TitleEng = " Offer State has been changes" + " : " + offerdata.Title;
                            if (userId != 0)
                            {
                                notymodel.ReceiverId = userId;
                            }
                            PushNotification push = new PushNotification();
                            push.PushNotifications(notymodel, "");
                            _iNotificationService.AddNoty(notymodel, userId);

                        }


                    }

                }
            }
            var offermodel = factory.Create(offer);
            offermodel.OfferState = State;
            return offermodel;
        }

        public IEnumerable<AcceptofferViewModel> Offeracceptance(int offerId)
        {
            var offer = _uow.AcceptOffer.GetAll(x => x.FlgStatus == 1 && x.OfferId == offerId && x.OfferState == OfferState.Accepted).Select(ac => new AcceptofferViewModel { AcceptedDate = ac.AcceptedDate, Notes = ac.Notes, AcceptedUserId = ac.User.Id, AcceptedUserName = ac.User.FirstName + ac.User.LastName });
            return offer;

        }

        public bool checkaccepatance(int UserId, int OfferId)
        {
            return _uow.AcceptOffer.GetAll(x => x.FlgStatus == 1, null, "Offer,User").Any(y => y.AcceptedUserId == UserId && y.OfferId == OfferId && y.OfferState == OfferState.Accepted);

        }

        public int OfferAcceptanceCout(int offerId)
        {
            return _uow.AcceptOffer.GetAll(x => x.FlgStatus == 1 && x.OfferState == OfferState.Done, null, "").Count(x => x.OfferId == offerId);
        }

        public object ServiceProviderRequests(int UserId, HttpRequestMessage request, OfferState state, SortingParamsViewModer search, int? page)
        {
            string text = search.SearchTerm;
            int pageSize = search.PageSize;
            int OfferNumber = 0;
            if (search.SearchTerm != null)
            {
                int.TryParse(search.SearchTerm.ToString(), out OfferNumber);
            }
            int offerId = 0;
            if (search.SearchTerm != null)
            {
                int.TryParse(search.SearchTerm.ToString(), out offerId);
            }
            int RatingValue = 0;
            if (search.Rating != null)
            {

                int.TryParse(search.Rating.ToString(), out RatingValue);
            }
            //.OrderByDescending(x => x.Id).OrderByDescending(x=>x.AcceptedDate)
            var basQuery = _uow.AcceptOffer.GetAll(f => f.FlgStatus == 1 && f.AcceptedUserId == UserId, null, "Offer");
            if (state == OfferState.Accepted)
            {
                basQuery =
                    basQuery.Where(
                        x => x.OfferState == OfferState.Accepted);

            }
            if (state == OfferState.Done)
            {
                basQuery =
                    basQuery.Where(
                         x => x.OfferState == OfferState.Done || x.OfferState == OfferState.Rejected);

            }
            var outPut = basQuery.Select(y => y.Offer);

            if (search.Rating.HasValue)
            {
                outPut = outPut.Where(r => r.User.OverAllrating == RatingValue);
            }
            if (search.fromDate != null)
                outPut = outPut.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= search.fromDate.Value);

            if (search.toDate != null)
                outPut = outPut.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= search.toDate.Value);


            if (search.fromPrice != null)
                outPut = outPut.Where(r => r.Price >= (decimal)search.fromPrice.Value);

            if (search.toPrice != null)
                outPut = outPut.Where(r => r.Price <= (decimal)search.toPrice.Value);
            if (search.Address != null)
                outPut = outPut.Where(r => r.Region.Name.Contains(search.Address) || r.Region.Name.ToLower().StartsWith(search.Address.ToLower()) || r.Region.Name.ToLower().Equals(search.Address.ToLower()));
            if (search.SearchTerm != null)
                outPut = outPut.Where(r => r.Title.ToLower().Contains(text.ToLower()) || r.Description.ToLower().Contains(text.ToLower()) || r.Period.ToLower().Contains(text.ToLower()) || r.Quantity.ToLower().Contains(text.ToLower()) || r.OfferNumber == OfferNumber || r.Id == offerId);

            if (search.SortPram != null)
            {
                if (search.Asc == false)
                {
                    switch (search.SortPram)
                    {
                        case "Region":
                            if (search.Address != null)
                            {
                                outPut = outPut.Where(r => r.Region.Name.StartsWith(search.Address) || r.Region.Name.Contains(search.Address)).OrderByDescending(x => x.Region.Name);
                            }
                            if (search.toPrice != null)
                            {
                                outPut = outPut.Where(r => r.Price <= (decimal)search.toPrice.Value).OrderByDescending(x => x.Price);
                            }
                            //  basQuery = search.toPrice != null ? basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)) : basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).ToList().AsQueryable();
                            if (search.fromDate != null && search.fromPrice != null)
                            {
                                outPut = outPut.Where(r => DbFunctions.TruncateTime(r.ExpireDate) >= search.fromDate.Value && r.Price >= (decimal)search.fromPrice.Value).OrderByDescending(x => x.Price);
                            }
                            if (search.fromDate != null && search.toPrice != null)
                            {
                                outPut = outPut.Where(r => DbFunctions.TruncateTime(r.ExpireDate) >= search.fromDate.Value && r.Price >= (decimal)search.fromPrice.Value).OrderByDescending(x => x.Price);
                            }
                            if (search.toDate != null)
                            {
                                outPut = outPut.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(search.toDate)).OrderByDescending(x => x.Price);
                            }
                            outPut = outPut.OrderByDescending(x => x.Region.Name);
                            break;
                        case "Price":
                            outPut = outPut.OrderByDescending(x => x.Price);
                            if (search.fromPrice != null)
                            {
                                outPut = outPut.Where(r => r.Price >= (decimal)search.fromPrice.Value).OrderByDescending(x => x.Price);
                            }
                            if (search.toPrice != null)
                            {
                                outPut = outPut.Where(r => r.Price <= (decimal)search.toPrice.Value).OrderByDescending(x => x.Price);
                            }
                            //  basQuery = search.toPrice != null ? basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)) : basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).ToList().AsQueryable();
                            if (search.fromDate != null && search.fromPrice != null)
                            {
                                outPut = outPut.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= search.fromDate.Value && r.Price >= (decimal)search.fromPrice.Value).OrderByDescending(x => x.Price);
                            }
                            if (search.fromDate != null && search.toPrice != null)
                            {
                                outPut = outPut.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= search.fromDate.Value && r.Price >= (decimal)search.fromPrice.Value).OrderByDescending(x => x.Price);
                            }
                            if (search.toDate != null)
                            {
                                outPut = outPut.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(search.toDate)).OrderByDescending(x => x.Price);
                            }
                            outPut = outPut.OrderByDescending(x => x.Price);
                            break;
                        case "Rating":
                            if (search.Rating == 0)
                            {
                                outPut = outPut.OrderByDescending(x => x.User.OverAllrating);
                            }
                            if (search.Rating != 0)
                            {
                                outPut = outPut.OrderByDescending(x => x.User.OverAllrating);
                            }
                            if (search.fromPrice != null)
                            {
                                outPut = outPut.Where(r => r.Price >= (decimal)search.fromPrice.Value).OrderByDescending(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            outPut = search.toPrice != null ? outPut.Where(r => r.Price <= (decimal)search.toPrice.Value).OrderByDescending(x => x.Price) : outPut.OrderByDescending(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            if (search.fromDate != null && search.fromPrice != null)
                            {
                                outPut = outPut.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= search.fromDate.Value && r.Price >= (decimal)search.fromPrice.Value).OrderByDescending(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            if (search.fromDate != null && search.toPrice != null)
                            {
                                outPut = outPut.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= search.fromDate.Value && r.Price >= (decimal)search.fromPrice.Value).OrderByDescending(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            if (search.toDate != null)
                            {
                                outPut = outPut.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(search.fromDate)).OrderByDescending(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            if (search.toDate != null)
                            {
                                outPut = outPut.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(search.fromDate)).OrderByDescending(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }

                            outPut = outPut.OrderByDescending(x => x.User.OverAllrating);
                            break;


                        case "Date":
                            if (search.fromPrice != null)
                            {
                                outPut = outPut.Where(r => r.Price >= (decimal)search.fromPrice.Value).OrderByDescending(x => x.PublishDate);
                            }
                            if (search.toPrice != null)
                            {
                                outPut = outPut.Where(r => r.Price <= (decimal)search.toPrice.Value).OrderByDescending(x => x.PublishDate);
                            }
                            if (search.fromDate != null)
                            {
                                outPut = outPut.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= search.fromDate.Value).OrderByDescending(x => x.PublishDate);
                            }
                            if (search.toDate != null)
                            {
                                outPut = outPut.Where(r => search.toDate != null && DbFunctions.TruncateTime(r.PublishDate) <= search.toDate.Value).OrderByDescending(x => x.PublishDate);
                            }
                            outPut = outPut.OrderByDescending(x => x.PublishDate);
                            break;

                        default:
                            outPut = outPut.OrderByDescending(x => x.PublishDate);
                            break;
                    }
                }
                if (search.Asc == true)
                {
                    switch (search.SortPram)
                    {
                        case "Region":
                            if (search.Address != null)
                            {
                                outPut = outPut.Where(r => r.Region.Name.StartsWith(search.Address) || r.Region.Name.Contains(search.Address)).OrderBy(x => x.Region.Name);
                            }
                            if (search.toPrice != null)
                            {
                                outPut = outPut.Where(r => r.Price <= (decimal)search.toPrice.Value).OrderBy(x => x.Price);
                            }
                            //  basQuery = search.toPrice != null ? basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)) : basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).ToList().AsQueryable();
                            if (search.fromDate != null && search.fromPrice != null)
                            {
                                outPut = outPut.Where(r => DbFunctions.TruncateTime(r.ExpireDate) >= search.fromDate.Value && r.Price >= (decimal)search.fromPrice.Value).OrderBy(x => x.Price);
                            }
                            if (search.fromDate != null && search.toPrice != null)
                            {
                                outPut = outPut.Where(r => DbFunctions.TruncateTime(r.ExpireDate) >= search.fromDate.Value && r.Price >= (decimal)search.fromPrice.Value).OrderBy(x => x.Price);
                            }
                            if (search.toDate != null)
                            {
                                outPut = outPut.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(search.toDate)).OrderBy(x => x.Price);
                            }
                            outPut = outPut.OrderBy(x => x.Region.Name);
                            break;
                        case "Price":
                            outPut = outPut.OrderBy(x => x.Price);
                            if (search.fromPrice != null)
                            {
                                outPut = outPut.Where(r => r.Price >= (decimal)search.fromPrice.Value).OrderBy(x => x.Price);
                            }
                            if (search.toPrice != null)
                            {
                                outPut = outPut.Where(r => r.Price <= (decimal)search.toPrice.Value).OrderBy(x => x.Price);
                            }
                            //  basQuery = search.toPrice != null ? basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)) : basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).ToList().AsQueryable();
                            if (search.fromDate != null && search.fromPrice != null)
                            {
                                outPut = outPut.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= search.fromDate.Value && r.Price >= (decimal)search.fromPrice.Value).OrderBy(x => x.Price);
                            }
                            if (search.fromDate != null && search.toPrice != null)
                            {
                                outPut = outPut.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= search.fromDate.Value && r.Price >= (decimal)search.fromPrice.Value).OrderBy(x => x.Price);
                            }
                            if (search.toDate != null)
                            {
                                outPut = outPut.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(search.toDate)).OrderBy(x => x.Price);
                            }
                            outPut.OrderBy(x => x.Price);
                            break;
                        case "Date":
                            if (search.fromPrice != null)
                            {
                                outPut = outPut.Where(r => r.Price >= (decimal)search.fromPrice.Value).OrderBy(x => x.PublishDate);
                            }
                            if (search.toPrice != null)
                            {
                                outPut = outPut.Where(r => r.Price <= (decimal)search.toPrice.Value).OrderBy(x => x.PublishDate);
                            }
                            if (search.fromDate != null)
                            {
                                outPut = outPut.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= search.fromDate.Value).OrderBy(x => x.PublishDate);
                            }
                            if (search.toDate != null)
                            {
                                outPut = outPut.Where(r => search.toDate != null && DbFunctions.TruncateTime(r.PublishDate) <= search.toDate.Value).OrderBy(x => x.PublishDate);
                            }
                            outPut = outPut.OrderBy(x => x.PublishDate);
                            break;

                        default:
                            outPut = outPut.OrderBy(x => x.PublishDate);
                            break;
                        case "Rating":
                            if (search.Rating == 0)
                            {
                                outPut = outPut.OrderBy(x => x.User.OverAllrating);
                            }
                            if (search.Rating != 0)
                            {
                                outPut = outPut.OrderBy(x => x.User.OverAllrating);
                            }
                            if (search.fromPrice != null)
                            {
                                outPut = outPut.Where(r => r.Price >= (decimal)search.fromPrice.Value).OrderByDescending(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            outPut = search.toPrice != null ? outPut.Where(r => r.Price <= (decimal)search.toPrice.Value).OrderByDescending(x => x.Price) : outPut.OrderBy(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            if (search.fromDate != null && search.fromPrice != null)
                            {
                                outPut = outPut.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= search.fromDate.Value && r.Price >= (decimal)search.fromPrice.Value).OrderBy(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            if (search.fromDate != null && search.toPrice != null)
                            {
                                outPut = outPut.Where(r => DbFunctions.TruncateTime(r.PublishDate) >= search.fromDate.Value && r.Price >= (decimal)search.fromPrice.Value).OrderBy(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            if (search.toDate != null)
                            {
                                outPut = outPut.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(search.fromDate)).OrderBy(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }
                            if (search.toDate != null)
                            {
                                outPut = outPut.Where(r => DbFunctions.TruncateTime(r.PublishDate) <= DbFunctions.TruncateTime(search.fromDate)).OrderBy(x => x.User.OverAllrating).ThenByDescending(x => x.Id);
                            }

                            outPut = outPut.OrderBy(x => x.User.OverAllrating);
                            break;


                    }
                }
            }
            var final = outPut.ToList().Skip(((page ?? 1) - 1) * pageSize).Take(pageSize).Select(x => new OfferViewModel
            {
                Id = x.Id,
                OfferDate = x.OfferDate,
                OfferNumber = x.OfferNumber,
                Title = x.Title,
                Description = x.Description,
                Price = x.Price,
                quantity = x.Quantity,
                Period = x.Period,
                ServiceProvider = factory.Create(x.User),
                //   ServiceProviderRating = x.User.OverAllrating.HasValue ? x.User.OverAllrating.Value : 0,
                PublishDate = x.PublishDate,
                ExpireDate = x.ExpireDate.Value,
                RegionId = x.RegionId.HasValue ? x.RegionId.Value : 0,

                ImageUrl = x.ImageUrl != null ? path + x.ImageUrl : null,
                TruckTypeId = x.TruckTypeId,
                TruckTypeName = x.TruckType.Name,
                MaxCustomerNumbers = x.MaxCustomerNumbers,
                MaxExpireDate = x.MaxExpireDate,
                Region = factory.Create(x.Region),
                //OfferDetails = x.OfferDetails.Select(ODetails => new OfferDetailViewModel { truckId = ODetails.TruckId, truckName = ODetails.Truck !=null? ODetails.Truck.Name :"", truckNameArb = ODetails.Truck!=null? ODetails.Truck.NameArb:"", ParenttruckName = ODetails.Truck!=null? ODetails.Truck.Paranet.Name:"", ParenttruckNameArb = ODetails.Truck!=null? ODetails.Truck.Paranet.NameArb:"", Weight = _uow.Truck.GetById(ODetails.TruckId).Weight, trucksNo = ODetails.NumberOfTrucks, ImagePath = ODetails.Truck.TruckImagePath != null ? BaseURL + ODetails.Truck.TruckImagePath : null }),
                OfferDetails = x.OfferDetails.Select(ODetails => new OfferDetailViewModel { truckId = ODetails.TruckId, truckName = ODetails.Truck != null ? ODetails.Truck.Name : "", truckNameArb = ODetails.Truck != null ? ODetails.Truck.NameArb : "", trucksNo = ODetails.NumberOfTrucks, ImagePath = ODetails.Truck.TruckImagePath != null ? BaseURL + ODetails.Truck.TruckImagePath : null, OfferTrucks = factory.Create(ODetails.Truck) }),
                Count = OfferAcceptanceCout(x.Id),
                State = x.AcceptOffers.Where(a => a.AcceptedUserId == UserId).Select(y => y.OfferState).FirstOrDefault()

            });
            //
            var totalcount = final.Count();

            var pagesCount = Math.Ceiling((double)totalcount / pageSize);

            var pageValue = HttpUtility.ParseQueryString(request.RequestUri.Query).Get("page");
            int currentPage;
            if (!int.TryParse(pageValue, out currentPage))
            {
                currentPage = 0;
            }

            var prevLink = currentPage > 0
                ? request.RequestUri.AbsoluteUri + (page - 1)
                : "";
            var nextLink = currentPage < pagesCount - 1
                ? request.RequestUri.AbsoluteUri + (page + 1)
                : "";



            return
                   new
                   {
                       totalCount = totalcount,
                       pagesNumber = pagesCount,
                       CurrentPage = currentPage,
                       PrevPage = prevLink,
                       NextPage = nextLink,
                       Result = final
                   };

        }
        public int OfferNos(int userId)
        {

            return _uow.ProposalPrice.GetAll(
                  p => p.FlgStatus == 1 && p.ServiceProviderId == userId && p.PropsalStatus == PropsalStat.Open, null, "").ToList().Count();


        }
    }
}
