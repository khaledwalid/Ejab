using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ejab.DAl;
using Ejab.BAL.Reository;
using Ejab.BAL.UnitOfWork;
using Ejab.DAl.Common;
using Ejab.BAL.Common;
using Ejab.BAL.ModelViews;
using System.Net.Http;
using System.Web.Http.Routing;
using System.Web;
using System.Data.Entity;
using Ejab.BAL.Helpers;
using System.Configuration;
using Ejab.BAL.Services.Notification;
using Ejab.BAL.ModelViews.Notification;

namespace Ejab.BAL.Services
{
    public class RequestService : IRequestService
    {
        IUnitOfWork _uow;
        ModelFactory factory;
        ICustomerService _customerService;
        IRegionService _IRegionService;
        INotificationService _iNotificationService;      
        int pageSize;
        string BaseURL = "";      
        public RequestService(IUnitOfWork uow, ICustomerService customerservice, IRegionService IRegionService, INotificationService INotificationService)
        {
            this._uow = uow;
            factory = new ModelFactory();
            _customerService = customerservice;
            _iNotificationService = INotificationService;
            _IRegionService = IRegionService;
            BaseURL= ConfigurationManager.AppSettings["BaseURL"].ToString();

        }
        public IEnumerable<ProposalPriceModelView> AcceptedRequest()
        {
            var requests = _uow.ProposalPrice.GetAll(x => x.FlgStatus == 1 && x.IsAccepted == true, null, "RequestDetailes,Request,ServiceType").ToList().Select(r => new ProposalPriceModelView { Date = r.Date, IsAccepted = r.IsAccepted.HasValue ? r.IsAccepted.Value : false, Request = factory.Create(r.Request) });

            if (requests != null)
            {
                throw new Exception("006");

            }
            return requests;
        }

        public RequestModelView DeleteRequest(int id)
        {
            var request = _uow.Request.GetById(id);
            if (request == null)
            {
                throw new Exception("004");
            }
            if (request.FlgStatus == 0)
            {
                throw new Exception("003");
            }
            if (request.RequestDetailes != null)
            {
                foreach (var item in request.RequestDetailes.ToList())
                {
                    var details = _uow.RequestDetaile.GetById(item.Id);
                    details.FlgStatus = 0;
                    _uow.RequestDetaile.Update(details.Id, details);
                    _uow.Commit();
                }
            }
            request.FlgStatus = 0;
            _uow.Request.Update(id, request);
            _uow.Commit();
            return factory.Create(request);
        }

        public RequestModelView GetRequest(int id)
        {
            var request = _uow.Request.GetById(id);

            return factory.Create(request);
        }

        public IEnumerable<RequestModelView> GetRequests()
        {
            var requests = _uow.Request.GetAll(x => x.FlgStatus == 1, null, "RequestDetailes,proposalPrices").ToList()
                .Select(r => new RequestModelView { Id = r.Id, Title = r.Title, Description = r.Description, IsActive = r.IsActive, LocationFrom = r.LocationFrom, LocationTo = r.LocationTo, Requster = factory.Create(r.User) });
            if (requests == null)
            {
                throw new Exception("006");
            }
            return requests;
        }
        //hayam
        public object GetRequests(int userId, RequestStates state, HttpRequestMessage request, SortingParamsViewModer search, int? page = null)
        {
            string text = search.SearchTerm;
            pageSize = search.PageSize;
            int RequestNumber = 0;
            if (search.SearchTerm != null)
            {
                int.TryParse(search.SearchTerm.ToString(), out RequestNumber);
            }
            int RequestId = 0;
            if (search.SearchTerm != null)
            {
                int.TryParse(search.SearchTerm.ToString(), out RequestId);
            }
            var allpropsales = _uow.ProposalPrice.GetAll(x => x.FlgStatus == 1 && DbFunctions.TruncateTime(x.ExpireDate) <  DbFunctions.TruncateTime( DateTime.Now) && x.Request.RequestState !=RequestStates.Accepted && x.Request.RequesterId== userId, null, "");

            if (allpropsales != null)
            {
                foreach (var item in allpropsales.ToList())
                {
                    item.PropsalStatus = PropsalStat.Expired;
                    item.UpdatedBy = userId;
                    item.UpdatedOn = DateTime.Now;
                    _uow.ProposalPrice.Update(item.Id, item);
                    _uow.Commit();
                }

            }
            // var urlHelper = new UrlHelper(request);
            var basQuery = _uow.Request.GetAll(f => f.FlgStatus == 1 && f.RequesterId == userId, null, "RequestDetailes,proposalPrices");

            //(DbFunctions.TruncateTime(x.PermissionDate) <= DbFunctions.TruncateTime(DateTime.Now) && x.ProposalPrices.Any(y => y.ReqestId != x.Id)) ||
            // (x.ProposalPrices.Any(y => y.PropsalStatus != PropsalStat.Accepted && y.ReqestId == x.Id))
            if (state == RequestStates.Open)
            {
                basQuery =
                    basQuery.Where(
                        x => x.RequestState == RequestStates.Open  &&( DbFunctions.TruncateTime(x.PermissionDate) >= DbFunctions.TruncateTime(DateTime.Now) &&  DbFunctions.TruncateTime(x.ExpireDate) >= DbFunctions.TruncateTime(DateTime.Now)));

            }
            if (state == RequestStates.Accepted)
            {
                basQuery =
                    basQuery.Where(
                        x => x.RequestState == RequestStates.Accepted 
                        );

            }
            if (state == RequestStates.Expired || state == RequestStates.Closed || state == RequestStates.Cancelled)
            {
                //&& (DbFunctions.TruncateTime(x.PermissionDate) <= DbFunctions.TruncateTime(DateTime.Now) && DbFunctions.TruncateTime(x.ExpireDate) <= DbFunctions.TruncateTime(DateTime.Now))
                basQuery =
                    basQuery.Where(
                        x => x.RequestState == RequestStates.Expired || x.RequestState == RequestStates.Closed || x.RequestState == RequestStates.Cancelled || x.RequestState == RequestStates.Rejected && (DbFunctions.TruncateTime(x.PermissionDate) <= DbFunctions.TruncateTime(DateTime.Now) && DbFunctions.TruncateTime(x.ExpireDate) <= DbFunctions.TruncateTime(DateTime.Now)));
              
            }
            if (search.fromDate != null)
                basQuery = basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price )).Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value);

            if (search.toDate != null)
                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) <= search.toDate.Value);

            if (search.fromPrice != null)
                basQuery = basQuery.Where(r => r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value));

            if (search.toPrice != null)
                basQuery = basQuery.Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value));
            if (search.Address != null)
               
            basQuery = basQuery.Where(r => r.Region.Name.Contains(search.Address) || r.Region.Name.StartsWith(search.Address) || r.Region.Name.Equals(search.Address));
            if (search.SearchTerm != null)
                basQuery = basQuery.Where(r => r.Title.ToLower().Contains(text.ToLower()) || r.Description.ToLower().Contains(text.ToLower()) || r.Period.ToLower().Contains(text.ToLower()) || r.Quantity.ToLower().Contains(text.ToLower()) || r.LocationFrom.ToLower().Contains(text.ToLower()) || r.LocationTo.ToLower().Contains(text.ToLower()) || r.RequestNumber == RequestNumber || r.Id == RequestId).ToList().AsQueryable();


            if (search.SortPram != null)
            {
                if (search.Asc == false)
                {
                    switch (search.SortPram)
                    {
                        case "Region":
                            if (search.Address != null)
                            {
                                basQuery = basQuery.OrderByDescending(x => x.Region.Name).Where(r => r.Region.Name.StartsWith(search.Address) || r.Region.Name.Contains(search.Address) || r.Region.Name.Equals (search.Address));
                            }
                            if (search.toPrice != null)
                            {
                                basQuery = basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value));
                            }
                            //  basQuery = search.toPrice != null ? basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)) : basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).ToList().AsQueryable();
                            if (search.fromDate != null && search.fromPrice != null)
                            {
                                basQuery = basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).Where(r => DbFunctions.TruncateTime(r.ExpireDate) >= search.fromDate.Value && r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value));
                            }
                            if (search.fromDate != null && search.toPrice != null)
                            {
                                basQuery = basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).Where(r => DbFunctions.TruncateTime(r.ExpireDate) >= search.fromDate.Value && r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value));
                            }
                            if (search.toDate != null)
                            {
                                basQuery = basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).Where(r => DbFunctions.TruncateTime(r.StartingDate) <= DbFunctions.TruncateTime(search.toDate));
                            }
                            basQuery = basQuery.OrderByDescending(x => x.Region.Name).ToList().AsQueryable();
                            break;
                        case "Price":
                            basQuery.OrderByDescending(x => x.ProposalPrices.Max (y => y.Price));
                            if (search.fromPrice != null)
                            {
                                basQuery = basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).Where(r => r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value));
                            }
                            if (search.toPrice != null)
                            {
                                basQuery = basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value));
                            }
                            //  basQuery = search.toPrice != null ? basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)) : basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).ToList().AsQueryable();
                            if (search.fromDate != null && search.fromPrice != null)
                            {
                                basQuery = basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value && r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value));
                            }
                            if (search.fromDate != null && search.toPrice != null)
                            {
                                basQuery = basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value && r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value));
                            }
                            if (search.toDate != null)
                            {
                                basQuery = basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).Where(r => DbFunctions.TruncateTime(r.StartingDate) <= DbFunctions.TruncateTime(search.toDate));
                            }
                           
                            break;
                        case "Date":
                            if (search.fromPrice != null)
                            {
                                basQuery = basQuery.OrderByDescending(x => x.StartingDate).Where(r => r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value));
                            }
                            if (search.toPrice != null)
                            {
                                basQuery = basQuery.OrderByDescending(x => x.StartingDate).Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value));
                            }
                            if (search.fromDate != null)
                            {
                                basQuery = basQuery.OrderByDescending(x => x.StartingDate).Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value);
                            }
                            if (search.toDate != null)
                            {
                                basQuery = basQuery.OrderByDescending(x => x.StartingDate).Where(r => search.toDate != null && DbFunctions.TruncateTime(r.StartingDate) <= search.toDate.Value);
                            }
                            basQuery = basQuery.OrderByDescending(x => x.StartingDate);
                            break;

                        default:
                            basQuery = basQuery.OrderByDescending(x => x.Requestdate);
                            break;
                    }
                }
                if (search.Asc == true)
                {
                    switch (search.SortPram)
                    {
                        case "Price":
                            basQuery.OrderBy(x => x.ProposalPrices.Min(y => y.Price));
                            if (search.fromPrice != null)
                            {
                                basQuery = basQuery.OrderBy(x => x.ProposalPrices.Min(y => y.Price)).Where(r => r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value));
                            }
                            if (search.toPrice != null)
                            {
                                basQuery = basQuery.OrderBy(x => x.ProposalPrices.Min(y => y.Price)).Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value));
                            }
                            if (search.fromDate != null)
                            {
                                basQuery = basQuery.OrderBy(x => x.ProposalPrices.Min(y => y.Price)).Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value);
                            }
                            if (search.toDate != null)
                            {
                                basQuery = basQuery.OrderBy(x => x.ProposalPrices.Min(y => y.Price)).Where(r => DbFunctions.TruncateTime(r.StartingDate) <= search.toDate.Value);
                            }
                            //basQuery = search.toPrice != null ? basQuery.OrderBy(x => x.ProposalPrices.Min (y => y.Price)).Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)) : 
                            if (search.fromDate != null && search.fromPrice != null)
                            {
                                basQuery = basQuery.OrderBy(x => x.ProposalPrices.Min(y => y.Price)).Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value && r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value));
                            }
                            if (search.fromDate != null && search.toPrice != null)
                            {
                                basQuery = basQuery.OrderBy(x => x.ProposalPrices.Min(y => y.Price)).Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value && r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value));
                            }
                           
                            break;
                        case "Date":
                            if (search.fromPrice != null)
                            {
                                basQuery = basQuery.OrderBy(x => x.StartingDate).Where(r => r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value));
                            }
                            if (search.toPrice != null)
                            {
                                basQuery = basQuery.OrderBy(x => x.StartingDate).Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value));
                            }
                            if (search.fromDate != null)
                            {
                                basQuery = basQuery.OrderBy(x => x.StartingDate).Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value);
                            }
                            if (search.toDate != null)
                            {
                                basQuery = basQuery.OrderBy(x => x.StartingDate).Where(r => search.toDate != null && DbFunctions.TruncateTime(r.StartingDate) <= search.toDate.Value);
                            }
                            basQuery = basQuery.OrderByDescending(x => x.ExpireDate);
                            break;
                        case "Region":
                            if (search.Address != null)
                            {
                                basQuery = basQuery.OrderBy(x => x.Region.Name).Where(r => r.Region.Name.StartsWith(search.Address) || r.Region.Name.Contains(search.Address));
                            }
                            if (search.toPrice != null)
                            {
                                basQuery = basQuery.OrderBy(x => x.ProposalPrices.Max(y => y.Price)).Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value));
                            }
                            //  basQuery = search.toPrice != null ? basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)) : basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).ToList().AsQueryable();
                            if (search.fromDate != null && search.fromPrice != null)
                            {
                                basQuery = basQuery.OrderBy(x => x.ProposalPrices.Max(y => y.Price)).Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value && r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value));
                            }
                            if (search.fromDate != null && search.toPrice != null)
                            {
                                basQuery = basQuery.OrderBy(x => x.ProposalPrices.Max(y => y.Price)).Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value && r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value));
                            }
                            if (search.toDate != null)
                            {
                                basQuery = basQuery.OrderBy(x => x.ProposalPrices.Max(y => y.Price)).Where(r => DbFunctions.TruncateTime(r.StartingDate) <= DbFunctions.TruncateTime(search.toDate));
                            }
                            basQuery = basQuery.OrderBy(x => x.Region.Name);
                            break;


                        default:
                            basQuery = basQuery.AsNoTracking().OrderByDescending(x => x.Requestdate);
                            break;
                    }
                }
            }
            var baseQuery = basQuery.OrderByDescending(x => x.Id).ToList().Select
                    (r => new RequestModelView
                    {
                        Id = r.Id,
                        Requster = factory.Create(r.User),
                        Title = r.Title,
                        Description = r.Description,
                        Requestdate = r.Requestdate,
                        RequestNumber = r.RequestNumber,
                        RequestType = r.RequestType,
                        RequestState = r.RequestState,
                        StartingDate = r.StartingDate,
                        ItemsInfo = r.ItemsInfo,
                        ExpireDate = r.ExpireDate,
                        LocationFromLatitude = r.LocationFromLatitude,
                        LocationFromlongitude = r.LocationFromlongitude,
                        LocationToLatitude = r.LocationToLatitude,
                        LocationToLongitude = r.LocationToLongitude,
                        Notes = r.Notes,
                        Period = r.Period,
                        Quantity = r.Quantity,
                        PermissionDate = r.PermissionDate,
                        IsAccepted = r.IsAccepted.HasValue ? r.IsAccepted.Value : false,
                        RegionId= r.Region!=null?  r.RegionId.Value :0,
                        RegionName = r.RegionId.HasValue ? _uow.Region.GetById(r.RegionId.Value).Name:"",
                        // RegionName = r.Region.Name,
                        // Region=factory.Create(r.Region),
                        requestDetails = r.RequestDetailes.Select(RD => new RequestDetailsModelView { Id = RD.Id, truckId = RD.TruckId, truckName = RD.Truck.Name, truckNameArb = RD.Truck.NameArb, truckParentName = RD.Truck.Paranet != null ? RD.Truck.Paranet.Name : "", truckParentNameArb = RD.Truck.Paranet != null ? RD.Truck.Paranet.NameArb : "", trucksNo = RD.NumberOfTrucks, trucksImagePath = RD.Truck.TruckImagePath != null ? BaseURL + RD.Truck.TruckImagePath.ToString() : null }).AsQueryable(),

                        //  Requster = factory.Create(r.User),
                        LocationFrom = r.LocationFrom,
                        LocationTo = r.LocationTo,
                        ProposalPrice = r.ProposalPrices.Select(p => new ProposalPriceModelView { Id = p.Id, UserServiceProvider = factory.Create(p.ServiceProvider), Date = p.Date, Price = p.Price, PropsalStatus = p.PropsalStatus, IsAccepted = p.IsAccepted.HasValue ? p.IsAccepted.Value : false, ExpireDate = p.ExpireDate }).AsQueryable()


                    }
                ).ToList().Skip(pageSize * ((page ?? 1) - 1)).Take(pageSize);
            var totalcount = basQuery.Count();

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
                    Result = baseQuery
                };


        }
        //hayaamm
        public object HomeRequests(HttpRequestMessage request, SortingParamsViewModer search, int? page = null)
        {
            string text = search.SearchTerm;
            pageSize = search.PageSize;
            //  var urlHelper = new UrlHelper(request);
            decimal fromprice;
            decimal.TryParse(search.fromPrice.ToString(), out fromprice);
            decimal toprice;
            decimal.TryParse(search.toPrice.ToString(), out toprice);
            int RequestNumber = 0;
            if (search.SearchTerm != null)
            {
                int.TryParse(search.SearchTerm.ToString(), out RequestNumber);
            }
            int RequestId = 0;
            if (search.SearchTerm != null)
            {
                int.TryParse(search.SearchTerm.ToString(), out RequestId);
            }
            int RatingValue = 0;
            if (search.Rating != null)
            {
                int.TryParse(search.Rating.ToString(), out RatingValue);
            }
            //||(r.RequestState != RequestStates.Accepted && r.ProposalPrices.Any(y=>y.ServiceProviderId==UserId))
            var basQuery = _uow.Request.GetAll(x => x.FlgStatus == 1 && x.IsActive, null, "RequestDetailes,proposalPrices")
                .Where(r => r.RequestState == RequestStates.Open&&( DbFunctions.TruncateTime(r.PermissionDate ) >= DbFunctions.TruncateTime(DateTime.Today) && DbFunctions.TruncateTime(r.ExpireDate) >= DbFunctions.TruncateTime(DateTime.Today))
                 );
            //if (search !=null)
            //{
            //    basQuery = basQuery.OrderByDescending(x=>x.Id);
            //}
            if (search.fromDate != null)
               // basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value).OrderByDescending(x => x.ProposalPrices.Max(y => y.Price));
            basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value);

            if (search.toDate != null)
                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) <= search.toDate.Value);

            if (search.fromPrice != null)
                basQuery = basQuery.Where(r => r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value));

            if (search.toPrice != null)
                basQuery = basQuery.Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value));
            if (search.Address != null)
                basQuery = basQuery.Where(r => r.Region.Name.Contains(search.Address) || r.Region.Name.Equals(search.Address) || r.Region.Name.StartsWith(search.Address));
            if (search.SearchTerm != null)
                basQuery = basQuery.Where(r => r.Title.ToLower().Contains(text.ToLower()) || r.Description.ToLower().Contains(text.ToLower()) || r.Period.ToLower().Contains(text.ToLower()) || r.Quantity.ToLower().Contains(text.ToLower()) || r.LocationFrom.ToLower().Contains(text.ToLower()) || r.LocationTo.ToLower().Contains(text.ToLower()) || r.RequestNumber == RequestNumber || r.Id == RequestId || r.RequestDetailes.Any(y => y.Truck.Name.ToLower() == text.ToLower()));


            if (search.SortPram != null)
            {
                if (search.Asc == false)
                {
                    switch (search.SortPram)
                    {
                        case "Region":
                            if (search.Address != null)
                            {
                                basQuery = basQuery.Where(r => r.Region.Name.StartsWith(search.Address) || r.Region.Name.Contains(search.Address) ||  r.Region.Name.Equals(search.Address)).OrderByDescending(x => x.Region.Name);
                            }
                            if (search.toPrice != null)
                            {
                                basQuery = basQuery.Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)).OrderByDescending(x => x.ProposalPrices.Max(y => y.Price));
                            }
                            //  basQuery = search.toPrice != null ? basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)) : basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).ToList().AsQueryable();
                            if (search.fromDate != null && search.fromPrice != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value && r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value)).OrderByDescending(x => x.ProposalPrices.Max(y => y.Price));
                            }
                            if (search.fromDate != null && search.toPrice != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value && r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value)).OrderByDescending(x => x.ProposalPrices.Max(y => y.Price));
                            }
                            if (search.toDate != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) <= DbFunctions.TruncateTime(search.toDate)).OrderByDescending(x => x.ProposalPrices.Max(y => y.Price));
                            }
                            basQuery = basQuery.OrderByDescending(x => x.Region.Name);
                            break;
                        case "Price":
                             basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price));
                            if (search.fromPrice != null)
                            {
                                basQuery = basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price));
                                //  basQuery = basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).Where(r => r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value));
                            }
                            if (search.toPrice != null)
                            {
                                basQuery = basQuery.Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)).OrderByDescending(x => x.ProposalPrices.Max(y => y.Price));
                            }
                            //  basQuery = search.toPrice != null ? basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)) : basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).ToList().AsQueryable();
                            if (search.fromDate != null && search.fromPrice != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value && r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value)).OrderByDescending(x => x.ProposalPrices.Max(y => y.Price));
                            }
                            if (search.fromDate != null && search.toPrice != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value && r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value)).OrderByDescending(x => x.ProposalPrices.Max(y => y.Price));
                            }
                            if (search.toDate != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) <= DbFunctions.TruncateTime(search.toDate)).OrderByDescending(x => x.ProposalPrices.Max(y => y.Price));
                            }
                           
                            break;
                        case "Date":
                            if (search.fromPrice != null)
                            {
                                basQuery = basQuery.Where(r => r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value)).OrderByDescending(x => x.StartingDate);
                            }
                            if (search.toPrice != null)
                            {
                                basQuery = basQuery.Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)).OrderByDescending(x => x.StartingDate);
                            }
                            if (search.fromDate != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value).OrderByDescending(x => x.StartingDate);
                            }
                            if (search.toDate != null)
                            {
                                basQuery = basQuery.Where(r => search.toDate != null && DbFunctions.TruncateTime(r.StartingDate) <= search.toDate.Value).OrderByDescending(x => x.StartingDate);
                            }
                            basQuery = basQuery.OrderByDescending(x => x.StartingDate);
                            break;

                        default:
                            basQuery = basQuery.OrderByDescending(x => x.Requestdate);
                            break;
                    }
                }
                if (search.Asc == true)
                {
                    switch (search.SortPram)
                    {
                        case "Price":
                            basQuery.OrderBy(x => x.ProposalPrices.Min(y => y.Price));
                            if (search.fromPrice != null)
                            {
                                basQuery = basQuery.Where(r => r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value)).OrderBy(x => x.ProposalPrices.Min(y => y.Price));
                            }
                            if (search.toPrice != null)
                            {
                                basQuery = basQuery.Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)).OrderBy(x => x.ProposalPrices.Min(y => y.Price));
                            }
                            if (search.fromDate != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value).OrderBy(x => x.ProposalPrices.Min(y => y.Price));
                            }
                            if (search.toDate != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) <= search.toDate.Value).OrderBy(x => x.ProposalPrices.Min(y => y.Price));
                            }
                            //basQuery = search.toPrice != null ? basQuery.OrderBy(x => x.ProposalPrices.Min (y => y.Price)).Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)) : 
                            if (search.fromDate != null && search.fromPrice != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value && r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value)).OrderBy(x => x.ProposalPrices.Min(y => y.Price));
                            }
                            if (search.fromDate != null && search.toPrice != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value && r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value)).OrderBy(x => x.ProposalPrices.Min(y => y.Price));
                            }
                          
                            break;
                        case "Date":
                            if (search.fromPrice != null)
                            {
                                basQuery = basQuery.Where(r => r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value)).OrderBy(x => x.StartingDate);
                            }
                            if (search.toPrice != null)
                            {
                                basQuery = basQuery.Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)).OrderBy(x => x.StartingDate);
                            }
                            if (search.fromDate != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value).OrderBy(x => x.StartingDate);
                            }
                            if (search.toDate != null)
                            {
                                basQuery = basQuery.Where(r => search.toDate != null && DbFunctions.TruncateTime(r.StartingDate) <= search.toDate.Value).OrderBy(x => x.StartingDate);
                            }
                            basQuery = basQuery.OrderByDescending(x => x.StartingDate);
                            break;
                        case "Region":
                            if (search.Address != null)
                            {
                                basQuery = basQuery.Where(r => r.Region.Name.StartsWith(search.Address) || r.Region.Name.Contains(search.Address)).OrderBy(x => x.Region.Name);
                            }
                            if (search.toPrice != null)
                            {
                                basQuery = basQuery.Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)).OrderBy(x => x.ProposalPrices.Max(y => y.Price));
                            }
                            //  basQuery = search.toPrice != null ? basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)) : basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).ToList().AsQueryable();
                            if (search.fromDate != null && search.fromPrice != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value && r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value)).OrderBy(x => x.ProposalPrices.Max(y => y.Price));
                            }
                            if (search.fromDate != null && search.toPrice != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value && r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value)).OrderBy(x => x.ProposalPrices.Max(y => y.Price));
                            }
                            if (search.toDate != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) <= DbFunctions.TruncateTime(search.toDate)).OrderBy(x => x.ProposalPrices.Max(y => y.Price));
                            }
                            basQuery = basQuery.OrderBy(x => x.Region.Name);
                            break;


                        default:
                            basQuery = basQuery.AsNoTracking().OrderByDescending(x => x.Requestdate);
                            break;
                    }
                }
            }


            var baseQuery = basQuery.ToList().Select
                    (r => new RequestModelView
                    {
                        Id = r.Id,
                        Requster = factory.Create(r.User),
                        Title = r.Title,
                        Description = r.Description,
                        Requestdate = r.Requestdate,
                        RequestNumber = r.RequestNumber,
                        RequestType = r.RequestType,
                        RequestState = r.RequestState,
                        StartingDate = r.StartingDate,
                        ItemsInfo = r.ItemsInfo,
                        ExpireDate = r.ExpireDate,
                        LocationFromLatitude = r.LocationFromLatitude,
                        LocationFromlongitude = r.LocationFromlongitude,
                        LocationToLatitude = r.LocationToLatitude,
                        LocationToLongitude = r.LocationToLongitude,
                        Notes = r.Notes,
                        Period = r.Period,
                        Quantity = r.Quantity,
                        PermissionDate = r.PermissionDate,
                        // RegionName = r.Region.Name,
                        // Region=factory.Create(r.Region),
                        requestDetails = r.RequestDetailes.Select(RD => new RequestDetailsModelView { Id = RD.Id, truckId = RD.TruckId, truckName = RD.Truck.Name, truckNameArb = RD.Truck.NameArb, truckParentName = RD.Truck.Paranet != null ? RD.Truck.Paranet.Name : "", truckParentNameArb = RD.Truck.Paranet != null ? RD.Truck.Paranet.NameArb : "", trucksNo = RD.NumberOfTrucks, trucksImagePath = RD.Truck.TruckImagePath != null ? BaseURL + RD.Truck.TruckImagePath.ToString() : null, requestTrucks = factory.Create(RD.Truck), Notes = RD.Notes }).AsQueryable(),
                        //  Requster = factory.Create(r.User),
                        LocationFrom = r.LocationFrom,
                        LocationTo = r.LocationTo,
                        ProposalPrice = r.ProposalPrices.Select(p => new ProposalPriceModelView { Id = p.Id, UserServiceProvider = factory.Create(p.ServiceProvider), Date = p.Date, Price = p.Price, PropsalStatus = p.PropsalStatus, ExpireDate = p.ExpireDate, IsAccepted = p.IsAccepted.HasValue ? p.IsAccepted.Value : false,Notes=p.Comments }).AsQueryable()


                    }
                ).OrderByDescending(x => x.Id).Skip(pageSize * ((page ?? 1) - 1)).Take(pageSize);
            var totalcount = basQuery.Count();

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
                    Result = baseQuery
                };

        }
        public RequestModelView InsertRequestWithReturn(RequestModelView request, RequestType type, int userid)
        {
            var setting = _uow.Setting.GetAll(x => x.FlgStatus == 1, null, "").FirstOrDefault();
            var usersToken = new List<string>();
            var userslist = new List<int>();
            var usersTokenTruck = new List<string>();
            NotificationViewModel trucknotymodel = new NotificationViewModel();
            NotificationViewModel regionnotymodel = new NotificationViewModel();
            PushNotification truckpush = new PushNotification();
            PushNotification regionpush = new PushNotification();
            var requests = _uow.Request.GetAll();
            if (requests.Count() == 0)
            {
                request.RequestNumber = 1;
            }
            else
            {
                request.RequestNumber = (int)MaxRequestNumber();
            }
            var entity = factory.Parse(request);
            if (request.PermissionDate.Date > request.StartingDate.Date)
            {
                throw new Exception("68");
            }
            var user = _uow.User.GetById(userid);

            if (user != null && user.CustomerType != CustomerTypes.Requester)
            {
                throw new Exception("046");
            }
            entity.Requestdate = DateTime.Now;
            entity.RequesterId = userid;
            entity.StartingDate = request.StartingDate;

            entity.FlgStatus = 1;
            entity.CreatedBy = userid;
            entity.CreatedOn = DateTime.Now;
           

            if (type == RequestType.Equipment)
            {

                entity.IsActive = true;
                var expiredayes = setting.ExpirDayies;
                entity.ExpireDate = DateTime.UtcNow.AddDays(expiredayes);
                entity.RequestState = RequestStates.Open;
                entity.RequestType = RequestType.Equipment;
                entity.LocationFrom = request.LocationFrom;
                entity.LocationFromLatitude = request.LocationFromLatitude;
                entity.LocationFromlongitude = request.LocationFromlongitude;
                entity.LocationTo = "";
                entity.Quantity = request.Quantity;
                //entity.PeriodTo = request.PeriodTo;
                entity.Notes = request.Notes;
                entity.PermissionDate = request.PermissionDate;
                entity.RegionId = request.RegionId;
                //entity.ExpireDate = request.PermissionDate;
            }
            if (type == RequestType.Trucks)
            {
                entity.RequestState = RequestStates.Open;
                entity.IsActive = true;
                entity.RequestType = RequestType.Trucks;
                entity.LocationFrom = request.LocationFrom;
                entity.LocationFromLatitude = request.LocationFromLatitude;
                entity.LocationFromlongitude = request.LocationFromlongitude;
                entity.LocationTo = request.LocationTo;
                entity.LocationToLatitude = request.LocationToLatitude;
                entity.LocationToLongitude = request.LocationToLongitude;

                //entity.PeriodFrom = request.PeriodFrom;
                //entity.PeriodTo = request.PeriodTo;
                entity.Quantity = request.Quantity;
                //entity.PeriodTo = request.PeriodTo;
                entity.Notes = request.Notes;
                entity.ItemsInfo = request.ItemsInfo;
                var expiredayes = setting.ExpirDayies;

                entity.ExpireDate = DateTime.UtcNow.AddDays(expiredayes);
                entity.PermissionDate = request.PermissionDate;
                entity.RegionId = request.RegionId;
                //  entity.ExpireDate = request.PermissionDate;

            }
            _uow.Request.Add(entity);
            _uow.Commit();
            if (request.RegionId != 0 )
            {
                var region = _IRegionService.getById(request.RegionId);
                var Users = _uow.User.GetAll(x => x.FlgStatus == 1 && x.CustomerType == CustomerTypes.ServiceProvider, null, "Interests").Where(i => i.Interests.Any(y => y.RegionId == request.RegionId && y.FlgStatus==1 )).ToList();
                if (Users != null && Users.Count() > 0)
                {
                    foreach (var item in Users)
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
             
                    foreach (var id in userslist)
                    {
                        regionnotymodel.ReceiverId = id;
                        var regionName = _IRegionService.getById(request.RegionId).Name;
                        regionnotymodel.Body = "تم إضافه طلب جديد فى هذه المنطقه" + "  "+ ":" + regionName + '-' + "New Request is added in this region" + ":" + regionName;
                        //regionnotymodel.BodyArb = "تم إضافه طلب جديد فى هذه المنطقه" + regionName;
                        regionnotymodel.Date = DateTime.Now;
                        regionnotymodel.Title = " إضافه طلب جديد فى هذه المنطقه" +" " + regionName + '-' + "New Request is added in region" + regionName;
                        regionnotymodel.Seen = false;
                        // notymodel.ReceiverId = item.Id;
                       

                        regionnotymodel.Type = NotificationType.AddRequest;
                        trucknotymodel.BodyArb = "تم إضافه طلب جديد فى هذه المنطقه" + " " + ":" + regionName;
                        trucknotymodel.BodyEng = "New Request is added in this region" + ":" + regionName;
                        trucknotymodel.TitleArb = " إضافه طلب جديد فى هذه المنطقه" + " " + ":" + regionName;
                        trucknotymodel.TitleEng = "New Request is added in region" + regionName;
                      
                    }
                    regionnotymodel.registration_ids = usersToken;
                    regionpush.PushNotifications(regionnotymodel, "");
                    _iNotificationService.AddNoty(regionnotymodel, userid);

                }

            }
            //else
            //{
            //    RegionModelView regionModel = new RegionModelView();
            //    if (request.RegionName =="")
            //    {
            //        throw new Exception("107");
            //    }
            //    regionModel.Name = request.RegionName;
            //    var regionEntity = factory.Parse(regionModel);
            //    regionEntity.FlgStatus = 1;
            //    regionEntity.parantId = 1;
            //    regionEntity.CreatedOn = DateTime.UtcNow.Date;
            //    regionEntity.CreatedBy = userid;
            //    _uow.Region.Add(regionEntity);
            //    _uow.Commit();
            //    entity.RegionId = regionEntity.Id;
            //}
          
            if (request.requestDetails != null)
            {
                foreach (var detailes in request.requestDetails.ToList())
                {
                    var truck = _uow.Truck.GetAll(x => x.FlgStatus == 1 && x.Id == detailes.truckId).FirstOrDefault();

                    string trucktype = truck.TruckType.NameArb;
                    string trucktypeeng = truck.TruckType.Name;
                    var allUsers = _uow.User.GetAll(x => x.FlgStatus == 1 && x.CustomerType == CustomerTypes.ServiceProvider, null, "Interests").Where(i => i.Interests.Any(y=> y.TruckId == detailes.truckId && y.FlgStatus == 1)).ToList();
                    if (allUsers.Count > 0)
                    {
                      
                        foreach (var item in allUsers)
                        {
                            userslist.Clear();
                            userslist.Add(item.Id);
                            if (item != null)
                            {
                                var devices = _customerService.GetDeviceByTokenAndType(item.Id);
                                if (devices.Count()>0)
                                {
                                    foreach (var dev in devices)
                                    {
                                        if (dev != null)
                                        {
                                            if (!usersTokenTruck .Contains(dev.Fcmtoken))
                                            {
                                                usersTokenTruck.Add(dev.Fcmtoken);
                                            }
                                            
                                        }
                                    }
                                   
                                }                            
                               
                            }
                        }
                      
                        foreach (var item in userslist)
                        {
                            trucknotymodel.ReceiverId = item;
                            trucknotymodel.Body = " تم اضافه طلب جديد" + " " + trucktype + " " + ":" + truck.NameArb + '-' + "New Request is added by this " + trucktypeeng + "" + "Its Id :" + truck.Id + ":" + "TruckName" + truck.Name;
                            // trucknotymodel.BodyEng = "New Offer is added by this truck" + "Its Id :" + item.truckId + "TruckName" + item.truckName;
                            trucknotymodel.Date = DateTime.Now;
                            trucknotymodel.Title = "تم اضافه طلب جديد " + " " + trucktype + truck.NameArb + '-' + "New Request is added by  " + trucktypeeng + "" + truck.Name;
                            trucknotymodel.Seen = false;
                            // notymodel.ReceiverId = item.Id;              
                            trucknotymodel.Type = NotificationType.AddRequest;
                            trucknotymodel.BodyArb = " تم اضافه طلب جديد" + " " + trucktype + " " + ":" + truck.NameArb;
                            trucknotymodel.BodyEng = "New Request is added by  " + "" + trucktypeeng + "" + "Its Id :" + truck.Id + ":" + "TruckName" + truck.Name;
                            trucknotymodel.TitleArb = "تم اضافه طلب جديد " + " " + ":" + "" + trucktype + " " + truck.NameArb;
                            trucknotymodel.TitleEng = "New Request is added by  " + "" + trucktypeeng + "" + truck.Name;

                        }
                        trucknotymodel.registration_ids = usersTokenTruck;
                        truckpush.PushNotifications(trucknotymodel, "");
                        _iNotificationService.AddNoty(trucknotymodel, userid);
                    }

                   

                    if (detailes.truckId != 0)
                    {
                        var truckmodel = new TrucksViewModel() { Id = detailes.truckId, Name = detailes.truckName };
                        detailes.requestTrucks = truckmodel;

                        var truckentity = _uow.Truck.GetAll(x=>x.FlgStatus==1 && x.Id== detailes.truckId).FirstOrDefault();
                        if (truckentity !=null)
                        {
                            var requestDetails = new RequestDetailsModelView();
                            requestDetails.RequestId = entity.Id;
                            requestDetails.truckId = detailes.truckId;
                            requestDetails.trucksNo = detailes.trucksNo;
                            requestDetails.truckName = truckentity != null ? truckentity.Name : "";
                            requestDetails.truckNameArb = truckentity != null ? truckentity.NameArb : "";


                            var DetailesEntity = factory.Parse(requestDetails);
                            //  DetailesEntity.Price  = detailes.price;
                            DetailesEntity.CreatedBy = userid;
                            DetailesEntity.CreatedOn = DateTime.UtcNow;
                            DetailesEntity.FlgStatus = 1;
                            entity.RequestDetailes.Add(DetailesEntity);
                            _uow.Commit();
                        }
                     
                    }

                }
            }

            var model = factory.Create(entity);
            var requster = _uow.User.GetById(entity.RequesterId);
            model.Requster = factory.Create(requster);

            // model.Requster =factory.Create( requster);
            return model;
        }

        public IEnumerable<ProposalPriceModelView> NotAcceptedRequest()
        {
            var requests = _uow.ProposalPrice.GetAll(x => x.FlgStatus == 1 && x.IsAccepted == false, null, "RequestDetailes,Request").ToList().Select(r => new ProposalPriceModelView { Date = r.Date, IsAccepted = r.IsAccepted.HasValue ? r.IsAccepted.Value : false, Request = factory.Create(r.Request) });

            if (requests != null)
            {
                throw new Exception("006");

            }
            return requests;
        }

        //public IEnumerable<RequestModelView> RequestsForService(int serviceTypeId)
        //{
        //    var requests = _uow.Request.GetAll(x => x.FlgStatus == 1 && x.ServiceTypeId == serviceTypeId, null, "RequestDetailes,proposalPrices,ServiceType").ToList().Select(r => new RequestModelView { UserId = r.RequesterId, RequsterName = r.User.FirstName + "" + r.User.LastName, Title = r.Title, Description = r.Description, DateFrom = r.DateFrom, DateTo = r.DateTo.Date, IsActive = r.IsActive, LocationFrom = r.LocationFrom, LocationTo = r.LocationTo, ServiceTypeId = r.ServiceTypeId, ServiceTypeName = r.ServiceType.Name });
        //    return requests;
        //}

        public RequestModelView UpdateRequest(int requestid, RequestModelView request, int UserId)
        {
            var existedRequest = _uow.Request.GetById(requestid);
            if (existedRequest == null)
            {
                throw new Exception("004");
            }
            existedRequest.Title = request.Title;
            existedRequest.Description = request.Description;
            //existedRequest.ServiceTypeId = request.ServiceTypeId;
            //  existedRequest.DateFrom = request.DateFrom;
            //  existedRequest.DateTo = request.DateTo;
            existedRequest.ExpireDate = request.ExpireDate;
            existedRequest.IsActive = request.IsActive;
            existedRequest.LocationFrom = request.LocationFrom;
            existedRequest.LocationFromLatitude = request.LocationFromLatitude;
            existedRequest.LocationFromlongitude = request.LocationFromlongitude;
            existedRequest.LocationTo = request.LocationTo;
            existedRequest.LocationToLatitude = request.LocationToLatitude;
            existedRequest.LocationToLongitude = request.LocationToLongitude;
            //existedRequest.PeriodFrom = request.PeriodFrom;
            //existedRequest.PeriodTo = request.PeriodTo;
            existedRequest.Requestdate = request.Requestdate;
            existedRequest.RequestState = request.RequestState;
            existedRequest.UpdatedBy = UserId;
            existedRequest.UpdatedOn = DateTime.UtcNow.Date;
            existedRequest.FlgStatus = 1;

            //if (request.RequestDetails != existedRequest.RequestDetailes)
            //{
            //    foreach (var item in existedRequest.RequestDetailes.ToList())
            //    {
            //        if (!request.RequestDetails.Any(d => d.RequestId == item.RequestId))
            //            existedRequest.RequestDetailes.Remove(item);

            //    }



            if (existedRequest.RequestDetailes != null)
            {
                foreach (var item in existedRequest.RequestDetailes.ToList())
                {
                    var details = _uow.RequestDetaile.GetById(item.Id);
                    _uow.RequestDetaile.Delete(details.Id, details);
                    _uow.Commit();
                }
            }


            //}
            foreach (var detailes in request.requestDetails)
            {
                if (!existedRequest.RequestDetailes.Any(t => t.RequestId == detailes.RequestId))
                {

                    var truck = new TrucksViewModel() { Id = detailes.truckId, Name = detailes.truckName };
                    detailes.requestTrucks = truck;

                    var truckentity = _uow.Truck.GetById(detailes.truckId);

                    var requestDetails = new RequestDetailsModelView();
                    requestDetails.RequestId = requestid;
                    requestDetails.truckId = detailes.truckId;
                    requestDetails.trucksNo = detailes.trucksNo;
                    requestDetails.truckName = truckentity.Name;
                    var DetailesEntity = factory.Parse(requestDetails);
                    DetailesEntity.CreatedBy = UserId;
                    DetailesEntity.CreatedOn = DateTime.UtcNow;
                    DetailesEntity.FlgStatus = 1;
                    //  DetailesEntity.Price = detailes.price;
                    existedRequest.RequestDetailes.Add(DetailesEntity);
                }

            }
            _uow.Request.Update(existedRequest.Id, existedRequest);
            _uow.Commit();

            var model = factory.Create(existedRequest);
            var requster = _uow.User.GetById(UserId);
            //model.RequsterName = requster.FirstName + "" + requster.FirstName;
            //var serviceType = _uow.ServiceType.GetById(request.ServiceTypeId);
            //model.ServiceType = factory.Create(serviceType);
            //model.ServiceTypeName = serviceType.Name;
            model.Requster = factory.Create(requster);
            return model;

        }

        public void InsertRequest(RequestModelView request)
        {
            var entity = factory.Parse(request);
            _uow.Request.Add(entity);
            _uow.Commit();
        }

        public RequestDetailsModelView AddRequestDetails(int requestId, RequestDetailsModelView model, int userId)
        {
            var truck = new TrucksViewModel() { Id = model.truckId, Name = model.truckName };
            model.requestTrucks = truck;
            var truckentity = _uow.Truck.GetById(model.truckId);
            var entity = factory.Parse(model);
            entity.RequestId = requestId;
            entity.Truck = truckentity;
            entity.NumberOfTrucks = model.trucksNo;
            entity.CreatedBy = userId;
            entity.TruckId = model.truckId;
            entity.FlgStatus = 1;
            entity.CreatedOn = DateTime.UtcNow;
            entity.UpdatedBy = null;
            entity.UpdatedOn = null;

            _uow.RequestDetaile.Add(entity);
            _uow.Commit();
            var rDetailsModel = factory.Create(entity);
            rDetailsModel.requestTrucks = factory.Create(truckentity);
            rDetailsModel.truckName = truckentity.Name;
            return rDetailsModel;
        }

        public RequestDetailsModelView EditRequestDetails(int detailsId, RequestDetailsModelView model, int userId)
        {
            var existedDetails = _uow.RequestDetaile.GetById(detailsId);
            if (existedDetails == null)
            {
                throw new Exception("this Request Details Is Not Found");
            }
            var parsedEntity = factory.Parse(model);
            if (existedDetails.TruckId != parsedEntity.TruckId && existedDetails.NumberOfTrucks != parsedEntity.NumberOfTrucks)
            {
                existedDetails.TruckId = parsedEntity.TruckId;
                existedDetails.NumberOfTrucks = parsedEntity.NumberOfTrucks;
            }
            existedDetails.RequestId = model.RequestId;
            existedDetails.UpdatedBy = userId;
            existedDetails.UpdatedOn = DateTime.UtcNow;
            _uow.RequestDetaile.Update(detailsId, existedDetails);
            _uow.Commit();

            var detailsModel = factory.Create(existedDetails);
            return detailsModel;
        }

        public RequestDetailsModelView DeleteRequestDetails(int detailsId)
        {
            var details = _uow.RequestDetaile.GetById(detailsId);
            if (details == null)
            {
                throw new Exception(" This Request Does Not Exist");
            }
            details.FlgStatus = 0;
            _uow.RequestDetaile.Update(detailsId, details);
            _uow.Commit();
            var detailModel = factory.Create(details);
            return detailModel;
        }


        public IEnumerable<RequestModelView> RequestDetails(int requestId)
        {
            var requests = _uow.Request.GetAll(x => x.FlgStatus == 1 && x.Id == requestId, null, "RequestDetailes")
                .Select(r => new RequestModelView
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Description,
                    LocationFrom = r.LocationFrom,
                    LocationTo = r.LocationTo,
                    requestDetails = r.RequestDetailes.Select(RD => new RequestDetailsModelView { truckId = RD.TruckId, trucksType = RD.Truck.TruckType.Name, trucksTypeNameArb = RD.Truck.TruckType.NameArb, truckName = RD.Truck.Name, truckNameArb = RD.Truck.NameArb, truckParentName = RD.Truck.Paranet != null ? RD.Truck.Paranet.Name : "", truckParentNameArb = RD.Truck.Paranet != null ? RD.Truck.Paranet.NameArb : "", trucksNo = RD.NumberOfTrucks, PricingDetailes = RD.RequestDetailesPrices.Select(p => new RequestDetailesPricesViewModel { RequestDetails = factory.Create(p.RequestDetaile), Price = p.Price, ServiceProvider = factory.Create(p.User), Notes = p.Notes }) }).AsQueryable()
                });
            return requests;
        }

        public ProposalPriceModelView AddPropsalPrice(List<RequestDetailesPricesViewModel> pmodel, int UserId)
        {
            var modelPrices = new RequestDetailesPricesViewModel();
            var usersToken = new List<string>();
            foreach (var item in pmodel)
            {
                if (item.RequestDetaileId == 0)
                {
                    throw new Exception("047");
                }

                if (item.Price == 0)
                {
                    throw new Exception("035");
                }
                var requestOfDetailes = _uow.Request.GetAll(x => x.FlgStatus == 1 && x.RequestDetailes.Any(y => y.Id == item.RequestDetaileId)).FirstOrDefault();
                var userAddedPropsal = _uow.RequestDetailesPrice.GetAll(X => X.FlgStatus == 1).Where(x => x.RequestDetaileId == item.RequestDetaileId && x.ServiceProviderId == UserId).Count();
                if (userAddedPropsal > 0 && requestOfDetailes.RequestState != RequestStates.Expired)
                {
                    throw new Exception("69");
                }

                var user = _uow.User.GetById(UserId);
                if (user.CustomerType != CustomerTypes.ServiceProvider)
                {
                    throw new Exception("059");
                }

                var comaniesUnderUser = _customerService.GetCompaniesAnderUser(UserId);
                if (comaniesUnderUser.Count() > 0)
                {
                    foreach (var company in comaniesUnderUser)
                    {
                        if (company.Id == item.ServiceProviderId)
                        {
                            throw new Exception("65");
                        }
                    }
                }

                if (requestOfDetailes != null && requestOfDetailes.RequestState == RequestStates.Expired)
                {
                    throw new Exception("103");

                }
                else if (requestOfDetailes != null && requestOfDetailes.RequestState == RequestStates.Open)
                {
                    modelPrices.RequestDetaileId = item.RequestDetaileId;
                    modelPrices.ServiceProviderId = UserId;
                    modelPrices.Price = item.Price;

                    //   RequestDetailesPrices.PropsalStatus = PropsalStat.Pending;
                    modelPrices.ExpireDate = item.ExpireDate;
                    modelPrices.Notes = item.Notes;
                    var RequestDetailesPricesReRequest = factory.Parse(modelPrices);
                    RequestDetailesPricesReRequest.FlgStatus = 1;
                    RequestDetailesPricesReRequest.CreatedOn = DateTime.Now;
                    RequestDetailesPricesReRequest.CreatedBy = UserId;
                    RequestDetailesPricesReRequest.ExpireDate = item.ExpireDate.Value;
                    _uow.RequestDetailesPrice.Add(RequestDetailesPricesReRequest);
                    _uow.Commit();
                  
                }
                else
                {
                    throw new Exception("89");

                }
            }
            var request = _uow.Request.GetAll(x => x.FlgStatus == 1 && x.RequestDetailes.Any(y => y.Id == modelPrices.RequestDetaileId)).FirstOrDefault();
            var totalprice = _uow.RequestDetailesPrice.GetAll(x => x.FlgStatus == 1 && x.ServiceProviderId == UserId && x.RequestDetaile.RequestId == request.Id, null, "").Sum(t => t.Price);
            var Notes = _uow.RequestDetailesPrice.GetAll(x => x.FlgStatus == 1 && x.ServiceProviderId == UserId && x.RequestDetaile.RequestId == request.Id, null, "").FirstOrDefault().Notes;

            // Add Propsal Price For Request
            ProposalPriceModelView propsalmodel = new ProposalPriceModelView();
            propsalmodel.RequestId = request.Id;
            propsalmodel.ServiceProviderId = UserId;
            // var serviceProvider = _uow.User.GetById(item.ServiceProviderId);
            var serviceProvider = _uow.User.GetById(UserId);
            propsalmodel.UserServiceProvider = factory.Create(serviceProvider);
            propsalmodel.Price = totalprice;
            propsalmodel.Date = DateTime.UtcNow.Date;
            propsalmodel.AcceptedBy = 0;
            var propsalentity = factory.Parse(propsalmodel);
            propsalentity.FlgStatus = 1;
            propsalentity.PropsalStatus = PropsalStat.Open;
            propsalentity.CreatedBy = UserId;
            propsalentity.Comments = Notes;
            propsalentity.CreatedOn = DateTime.UtcNow.Date;    
            var expireDate = _uow.RequestDetailesPrice.GetAll(X => X.FlgStatus == 1).Where(x => x.RequestDetaile.RequestId == request.Id && x.ServiceProviderId == UserId).FirstOrDefault().ExpireDate;
            propsalentity.ExpireDate = expireDate;
            _uow.ProposalPrice.Add(propsalentity);
            _uow.Commit();
            NotificationViewModel notymodel = new NotificationViewModel();
            PushNotification push = new PushNotification();
            var devices = _customerService.GetDeviceByTokenAndType(request.RequesterId);
            if (devices.Count() > 0)
            {
                foreach (var device in devices)
                {
                    if (device != null)
                    {
                        usersToken.Add(device.Fcmtoken);
                       
                    }

                }
              
                notymodel.Body = "تم تقديم عرض سعر على الطلب" + ":" +" "+ request.Id + '-' + "add propsal to request" + ":" + " "  + request.Id;
                // notymodel.BodyArb = "تم قبول هذا الطلب" + request.Title;
                notymodel.Date = DateTime.Now;
                notymodel.Title = "تم تقديم عرض سعر على الطلب" + " "  + request.Id + '-' + " add propsal"+" " + request.Id;
                notymodel.Seen = false;
                notymodel.ReceiverId = request.RequesterId;
                notymodel.Type = NotificationType.UpdateState;
                notymodel.BodyArb = "تم تقديم عرض سعر على الطلب" + " "+ " " + ":" + request.Id;
                notymodel.BodyEng = " add propsal to request" + ":" + " "+ request.Id;
                notymodel.TitleArb = "تم تقديم عرض سعر على الطلب" + " " + ":" + request.Id;
                notymodel.TitleEng = " add propsal" + request.Id;
               
                notymodel.registration_ids = usersToken;
                push.PushNotifications(notymodel, "");
                _iNotificationService.AddNoty(notymodel, UserId);
            }

            propsalmodel.Request = factory.Create(request);
            propsalmodel.UserServiceProvider = factory.Create(serviceProvider);
            return propsalmodel;
        }

        public RequestDetailesPricesViewModel EditPropsalPrice(int pricingId, RequestDetailesPricesViewModel pmodel, int UserId)
        {
            var ProposalPricing = _uow.RequestDetailesPrice.GetAll(x => x.FlgStatus == 1 && x.Id == pricingId).FirstOrDefault();

            if (pmodel.ServiceProviderId == 0)
            {
                throw new Exception("049");
            }
            if (pmodel.Price == 0)
            {
                throw new Exception("035");
            }

            //ProposalPricing.IsAccepted = pmodel.IsAccepted;
            //ProposalPricing.AcceptedBy = pmodel.AcceptedBy;
            ProposalPricing.Price = pmodel.Price;
            ProposalPricing.RequestDetaileId = pricingId;
            //  ProposalPricing.PropsalStatus = PropsalStat.Pending;
            ProposalPricing.RequestDetaileId = pmodel.RequestDetaileId;
            ProposalPricing.ServiceProviderId = pmodel.ServiceProviderId;
            ProposalPricing.ExpireDate = pmodel.ExpireDate.Value;
            ProposalPricing.FlgStatus = 1;
            ProposalPricing.UpdatedBy = UserId;
            ProposalPricing.UpdatedOn = DateTime.UtcNow;
            _uow.RequestDetailesPrice.Update(pricingId, ProposalPricing);
            _uow.Commit();

            var request = _uow.Request.GetAll(x => x.FlgStatus == 1 && x.RequestDetailes.Any(y => y.Id == ProposalPricing.RequestDetaileId)).FirstOrDefault();

            if (request.ProposalPrices != null)
            {
                var ProposalPrice = _uow.ProposalPrice.GetAll(x => x.FlgStatus == 1 && x.ReqestId == request.Id).FirstOrDefault();
                _uow.ProposalPrice.Delete(ProposalPrice.Id, ProposalPrice);
                _uow.Commit();
            }
            var requestproposal = new ProposalPriceModelView();
            requestproposal.RequestId = request.Id;
            requestproposal.Request = factory.Create(request);
            requestproposal.Date = DateTime.UtcNow.Date;
            requestproposal.ServiceProviderId = ProposalPricing.ServiceProviderId;
            var totalprice = _uow.RequestDetailesPrice.GetAll(x => x.FlgStatus == 1 && x.ServiceProviderId == ProposalPricing.ServiceProviderId && x.RequestDetaile.RequestId == requestproposal.RequestId, null, "").Sum(t => t.Price);
            //requestproposal.AcceptedBy=
            //    requestproposal.IsAccepted=
            requestproposal.Price = totalprice;
            // AddRequestProposal(requestproposal, UserId);
            var model = factory.Create(ProposalPricing);
            //var request = _uow.Request.GetById(pmodel.RequestId);
            var user = _uow.User.GetById(pmodel.ServiceProviderId);
            var requestDetailes = _uow.RequestDetaile.GetById(pmodel.RequestDetaileId);
            model.RequestDetails = factory.Create(requestDetailes);
            //var AcceptedByUser = _uow.User.GetById(pmodel.AcceptedBy);
            //model.UserAcceptedBy = factory.Create(AcceptedByUser);
            model.ServiceProviderName = user.FirstName + "" + user.LastName;
            //model.AcceptedByName = AcceptedByUser.FirstName + "" + AcceptedByUser.LastName;
            model.ServiceProvider = factory.Create(user);
            return model;
        }

        public RequestDetailesPricesViewModel DeletePropsalPrice(int id)
        {
            var proposalPrice = _uow.RequestDetailesPrice.GetById(id);
            if (proposalPrice == null)
            {
                throw new Exception("60");
            }
            proposalPrice.FlgStatus = 0;
            _uow.RequestDetailesPrice.Update(id, proposalPrice);
            _uow.Commit();
            var request = _uow.Request.GetAll(x => x.FlgStatus == 1 && x.RequestDetailes.Any(y => y.Id == proposalPrice.RequestDetaileId)).FirstOrDefault();

            if (request.ProposalPrices != null)
            {
                var ProposalPrice = _uow.ProposalPrice.GetAll(x => x.FlgStatus == 1 && x.ReqestId == request.Id).FirstOrDefault();
                _uow.ProposalPrice.Delete(ProposalPrice.Id, ProposalPrice);
                _uow.Commit();
            }
            var model = factory.Create(proposalPrice);
            return model;
        }

        public object RequestProposal(int requestId)
        {

            var Proposales = _uow.ProposalPrice.GetAll(x => x.FlgStatus == 1, null,
                "Request,ServiceProvider").Where(y => y.ReqestId == requestId);
            var Out = Proposales.ToList()
            .Select(p => new RequestDetailesPricesViewModel
            {
                ServiceProvider = factory.Create(p.ServiceProvider),
                Price = p.Price,
                ExpireDate = p.ExpireDate

            });


            var totalcount = Proposales.Count();

            //var pagesCount = Math.Ceiling((double)totalcount / pageSize);

            //var pageValue = HttpUtility.ParseQueryString(Request.RequestUri.Query).Get("page");
            //int currentPage;
            //if (!int.TryParse(pageValue, out currentPage))
            //{
            //    currentPage = 0;
            //}

            //var prevLink = currentPage > 0
            //    ? Request.RequestUri.AbsoluteUri.Split('?')[0] + '|' + (currentPage - 1)
            //    : "";
            //var nextLink = currentPage < pagesCount - 1
            //    ? Request.RequestUri.AbsoluteUri.Split('?')[0] + '|' + (currentPage + 1)
            //    : "";

            return
                new
                {
                    totalCount = totalcount,
                    Result = Out
                };
        }

        public int UserRequestsCount(int userId)
        {
            var count = _uow.Request.GetAll(x => x.FlgStatus == 1 && x.RequesterId == userId, null, "RequestDetailes,proposalPrices").ToList().Count();
            if (count == 0)
            {
                throw new Exception("This Customer Do Not Have Any Requests");
            }
            return count;
        }

        public RequestModelView requestWithProposales(int requestId)
        {
            var request = _uow.Request.GetAll(x => x.FlgStatus == 1 && x.Id == requestId, null, "RequestDetailes,proposalPrices")
                .Select(d => new RequestModelView
                {
                    Id = d.Id,
                    Requestdate = d.Requestdate,
                    RequestState = d.RequestState,
                    RequestNumber = d.RequestNumber,
                    Title = d.Title,
                    Description = d.Description,

                    requestDetails = d.RequestDetailes.Select(x => new RequestDetailsModelView { RequestId = x.RequestId, requestTrucks = factory.Create(x.Truck), truckName = x.Truck.Name, truckNameArb = x.Truck.NameArb, truckParentName = x.Truck.Paranet != null ? x.Truck.Paranet.Name : "", truckParentNameArb = x.Truck.Paranet != null ? x.Truck.Paranet.NameArb : "", trucksNo = x.NumberOfTrucks }).AsQueryable(),
                    ProposalPrice = d.ProposalPrices
                    .Select(p => new ProposalPriceModelView { ServiceProviderId = p.ServiceProviderId, Price = p.Price }).AsQueryable()
                }).FirstOrDefault();
            return request;
        }

        public int Count(int reqestid)
        {
            var ProposalesNo = _uow.ProposalPrice.GetAll(x => x.FlgStatus == 1).Count();
            return ProposalesNo;
        }

        public long MaxRequestNumber()
        {
            var no = _uow.Request.GetAll(null, null, "").Max(x => x.RequestNumber);
            return no + 1;
        }

        public IEnumerable<RequestDetailsModelView> GetRequestDetiles(int PropsalId)
        {
           // var imagFullPath = HttpContext.Current.Server.MapPath("~/TrucksImages/");

            var propsalPrice = _uow.ProposalPrice.GetAll(x => x.FlgStatus == 1 && x.Id == PropsalId, null, "Request").FirstOrDefault();
            return _uow.RequestDetaile.GetAll(x => x.FlgStatus == 1 && x.RequestId == propsalPrice.Request.Id, null, "RequestDetailesPrices")

                   .ToList().
                   Select(c => new RequestDetailsModelView
                   {
                       Id = c.Id,
                       truckId = c.TruckId,
                       truckName = c.Truck.Name,
                       truckNameArb = c.Truck.NameArb,
                       truckParentName = c.Truck.Paranet != null ? c.Truck.Paranet.Name : null,
                       truckParentNameArb = c.Truck.Paranet != null ? c.Truck.Paranet.NameArb : null,
                       //trucksType = _uow.Truck.GetById(c.TruckId).TruckType.Name,
                       trucksTypeNameArb = c.Truck.TruckType != null ? c.Truck.TruckType.NameArb : null,
                       trucksImagePath = c.Truck.TruckImagePath != null ? BaseURL + c.Truck.TruckImagePath : null,
                       trucksNo = c.NumberOfTrucks,
                       Price = c.RequestDetailesPrices.SingleOrDefault(x => x.RequestDetaileId == c.Id && x.ServiceProviderId == propsalPrice.ServiceProviderId).Price,
                      Expiredate = c.RequestDetailesPrices.SingleOrDefault(x => x.RequestDetaileId == c.Id && x.ServiceProviderId == propsalPrice.ServiceProviderId).ExpireDate,

                       Notes = c.RequestDetailesPrices.SingleOrDefault(x => x.RequestDetaileId == c.Id && x.ServiceProviderId == propsalPrice.ServiceProviderId).Notes.ToString()
                   });
        }

        public bool CheckRequestAccept(int requestId)
        {
            var requst = _uow.Request.GetById(requestId);

            if (requst.ProposalPrices.Any(x => x.IsAccepted == true))
            {
                return true;
            }
            return false;
        }

        //public ProposalPriceModelView AddRequestProposal(ProposalPriceModelView model, int UserId)
        //{
        //    if (model.AcceptedBy == 0)
        //    {
        //        model.UserAcceptedBy = null;
        //    }
        //    else
        //    {
        //        var acceptedby = _uow.User.GetById(model.AcceptedBy);
        //        model.UserAcceptedBy = factory.Create(acceptedby);
        //    }
        //    var entity = factory.Parse(model);

        //    entity.CreatedBy = UserId;
        //    entity.CreatedOn = DateTime.Now.Date;
        //    entity.ExpireDate = model.ExpireDate;
        //    entity.FlgStatus = 1;

        //    _uow.ProposalPrice.Add(entity);
        //    _uow.Commit();
        //    var Returnmodel = factory.Create(entity);
        //    return Returnmodel;
        //}

        public ProposalPriceModelView EditRequestProposal(int id, ProposalPriceModelView model, int UserId)
        {
            var existedEntity = _uow.ProposalPrice.GetById(id);
            if (existedEntity == null)
            {
                throw new Exception("006");
            }
            existedEntity.UpdatedBy = UserId;
            existedEntity.UpdatedOn = DateTime.UtcNow.Date;
            existedEntity.FlgStatus = 1;
            _uow.ProposalPrice.Update(id, existedEntity);
            _uow.Commit();
            var existedModel = factory.Create(existedEntity);
            return existedModel;
        }

        public ProposalPriceModelView DeleteRequestProposal(int id, int UserId)
        {
            var existedEntity = _uow.ProposalPrice.GetById(id);
            if (existedEntity == null)
            {
                throw new Exception("006");
            }
            existedEntity.UpdatedBy = UserId;
            existedEntity.UpdatedOn = DateTime.UtcNow.Date;
            existedEntity.FlgStatus = 0;
            _uow.ProposalPrice.Update(id, existedEntity);
            _uow.Commit();
            var existedModel = factory.Create(existedEntity);
            return existedModel;
        }

        //hayam
        public object ServiceProviderProsales(int ServiceProviderId, RequestStates state, HttpRequestMessage request, SortingParamsViewModer search, int? page = null)
        {
            string text = search.SearchTerm;

            int RequestNumber = 0;
            if (search.SearchTerm != null)
            {
                int.TryParse(search.SearchTerm.ToString(), out RequestNumber);
            }
            int RequestId = 0;
            if (search.SearchTerm != null)
            {
                int.TryParse(search.SearchTerm.ToString(), out RequestId);
            }
            int pageSize = search.PageSize;
            var allpropsales = _uow.ProposalPrice.GetAll(x => x.FlgStatus == 1 && DbFunctions.TruncateTime(x.ExpireDate)  < DateTime.Today, null, "");
           
            if (allpropsales != null)
            {
                foreach (var item in allpropsales.ToList())
                {
                    item.PropsalStatus = PropsalStat.Expired;
                    item.UpdatedBy = ServiceProviderId;
                    item.UpdatedOn = DateTime.Now;
                    _uow.ProposalPrice.Update(item.Id, item);
                    _uow.Commit();
                }
            }
            var basQuery = _uow.Request.GetAll(x => x.FlgStatus == 1 && x.ProposalPrices.Any(y => y.ServiceProviderId == ServiceProviderId), null, "ProposalPrices");
            if (state == RequestStates.Open)
            {
                basQuery =
                    basQuery.Where(
                        x => x.RequestState == RequestStates.Open && x.ProposalPrices.Any(y=>y.PropsalStatus !=PropsalStat.Expired && y.ServiceProviderId== ServiceProviderId) ).OrderByDescending(x=>x.Id);

            }
            if (state == RequestStates.Accepted)
            {
                basQuery =
                    basQuery.Where(
                        x => x.RequestState == RequestStates.Accepted
                        ).OrderByDescending(x => x.Id);

            }
            if (state == RequestStates.Expired || state == RequestStates.Closed || state == RequestStates.Cancelled)
            {
                basQuery =
                    basQuery.Where(  
                        x => x.RequestState == RequestStates.Expired || x.RequestState == RequestStates.Closed || x.RequestState == RequestStates.Cancelled  || x.ProposalPrices.Any(y => y.PropsalStatus == PropsalStat.Expired && y.ServiceProviderId == ServiceProviderId));

            }

            if (search.fromDate != null)
                
                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.ProposalPrices.FirstOrDefault().ExpireDate) >= search.fromDate.Value && r.ProposalPrices.FirstOrDefault().ServiceProviderId== ServiceProviderId);

            if (search.toDate != null)
                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.ProposalPrices.FirstOrDefault().ExpireDate) <= search.toDate.Value && r.ProposalPrices.FirstOrDefault().ServiceProviderId == ServiceProviderId);

            if (search.fromPrice != null)
                basQuery = basQuery.Where(r => r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value));

            if (search.toPrice != null)
                basQuery = basQuery.Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value));
            if (search.Address != null)
                basQuery = basQuery.Where(r => r.Region.Name.ToLower().Contains(search.Address) || r.Region.Name.ToLower().Equals(search.Address) || r.Region.Name.ToLower().StartsWith(search.Address));
            if (search.SearchTerm != null)
                basQuery = basQuery.Where(r => r.Title.ToLower().Contains(text.ToLower()) || r.Description.ToLower().Contains(text.ToLower()) || r.Period.ToLower().Contains(text.ToLower()) || r.Quantity.ToLower().Contains(text.ToLower()) || r.LocationFrom.ToLower().Contains(text.ToLower()) || r.LocationTo.ToLower().Contains(text.ToLower()) || r.RequestNumber == RequestNumber || r.Id == RequestId || r.RequestDetailes.Any(y => y.Truck.Name.ToLower() == text.ToLower()));


            if (search.SortPram != null)
            {
                if (search.Asc == false)
                {
                    switch (search.SortPram)
                    {
                        case "Region":
                            if (search.Address != null)
                            {
                                basQuery = basQuery.Where(r => r.Region.Name.ToLower().StartsWith(search.Address.ToLower())).OrderByDescending(x => x.Region.Name);
                            }
                            if (search.toPrice != null)
                            {
                                basQuery = basQuery.Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)).OrderByDescending(x => x.ProposalPrices.Max(y => y.Price));
                            }
                            //  basQuery = search.toPrice != null ? basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)) : basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).ToList().AsQueryable();
                            if (search.fromDate != null && search.fromPrice != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value && r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value)).OrderByDescending(x => x.ProposalPrices.Max(y => y.Price));
                            }
                            if (search.fromDate != null && search.toPrice != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value && r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value)).OrderByDescending(x => x.ProposalPrices.Max(y => y.Price));
                            }
                            if (search.toDate != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) <= DbFunctions.TruncateTime(search.toDate)).OrderByDescending(x => x.ProposalPrices.Max(y => y.Price));
                            }
                            basQuery = basQuery.OrderByDescending(x => x.Region.Name);
                            break;
                        case "Price":
                            basQuery = basQuery.OrderByDescending(x => x.ProposalPrices.Where(y=> y.ServiceProviderId == ServiceProviderId).Max(u=>u.Price));
                            if (search.fromPrice != null)
                            {
                                basQuery = basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price));
                                //  basQuery = basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).Where(r => r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value));
                            }
                            if (search.toPrice != null)
                            {
                                basQuery = basQuery.Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)).OrderByDescending(x => x.ProposalPrices.Max(y => y.Price));
                            }
                            //  basQuery = search.toPrice != null ? basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)) : basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).ToList().AsQueryable();
                            if (search.fromDate != null && search.fromPrice != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value && r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value)).OrderByDescending(x => x.ProposalPrices.Max(y => y.Price));
                            }
                            if (search.fromDate != null && search.toPrice != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value && r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value)).OrderByDescending(x => x.ProposalPrices.Max(y => y.Price));
                            }
                            if (search.toDate != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) <= DbFunctions.TruncateTime(search.toDate)).OrderByDescending(x => x.ProposalPrices.Max(y => y.Price));
                            }

                            break;
                        case "Date":
                            if (search.fromPrice != null)
                            {
                                basQuery = basQuery.Where(r => r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value)).OrderByDescending(x => x.StartingDate);
                            }
                            if (search.toPrice != null)
                            {
                                basQuery = basQuery.Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)).OrderByDescending(x => x.StartingDate);
                            }
                            if (search.fromDate != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value).OrderByDescending(x => x.StartingDate);
                            }
                            if (search.toDate != null)
                            {
                                basQuery = basQuery.Where(r => search.toDate != null && DbFunctions.TruncateTime(r.StartingDate) <= search.toDate.Value).OrderByDescending(x => x.StartingDate);
                            }
                            basQuery = basQuery.OrderByDescending(x => x.StartingDate);
                            break;

                        default:
                            basQuery = basQuery.OrderByDescending(x => x.Requestdate);
                            break;
                    }
                }
                if (search.Asc == true)
                {
                    switch (search.SortPram)
                    {
                        case "Price":
                            basQuery = basQuery.OrderBy(x => x.ProposalPrices.Where(y => y.ServiceProviderId == ServiceProviderId).Min(u => u.Price));
                            if (search.fromPrice != null)
                            {
                                basQuery = basQuery.Where(r => r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value)).OrderBy(x => x.ProposalPrices.Min(y => y.Price));
                            }
                            if (search.toPrice != null)
                            {
                                basQuery = basQuery.Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)).OrderBy(x => x.ProposalPrices.Min(y => y.Price));
                            }
                            if (search.fromDate != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value).OrderBy(x => x.ProposalPrices.Min(y => y.Price));
                            }
                            if (search.toDate != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) <= search.toDate.Value).OrderBy(x => x.ProposalPrices.Min(y => y.Price));
                            }
                            //basQuery = search.toPrice != null ? basQuery.OrderBy(x => x.ProposalPrices.Min (y => y.Price)).Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)) : 
                            if (search.fromDate != null && search.fromPrice != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value && r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value)).OrderBy(x => x.ProposalPrices.Min(y => y.Price));
                            }
                            if (search.fromDate != null && search.toPrice != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value && r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value)).OrderBy(x => x.ProposalPrices.Min(y => y.Price));
                            }

                            break;
                        case "Date":
                            if (search.fromPrice != null)
                            {
                                basQuery = basQuery.Where(r => r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value)).OrderBy(x => x.StartingDate);
                            }
                            if (search.toPrice != null)
                            {
                                basQuery = basQuery.Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)).OrderBy(x => x.StartingDate);
                            }
                            if (search.fromDate != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value).OrderBy(x => x.StartingDate);
                            }
                            if (search.toDate != null)
                            {
                                basQuery = basQuery.Where(r => search.toDate != null && DbFunctions.TruncateTime(r.StartingDate) <= search.toDate.Value).OrderBy(x => x.StartingDate);
                            }
                            basQuery = basQuery.OrderByDescending(x => x.StartingDate);
                            break;
                        case "Region":
                            if (search.Address != null)
                            {
                                basQuery = basQuery.Where(r => r.Region.Name.ToLower().StartsWith(search.Address.ToLower())).OrderBy(x => x.Region.Name);
                            }
                            if (search.toPrice != null)
                            {
                                basQuery = basQuery.Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)).OrderBy(x => x.ProposalPrices.Max(y => y.Price));
                            }
                            //  basQuery = search.toPrice != null ? basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).Where(r => r.ProposalPrices.Any(y => y.Price <= (decimal)search.toPrice.Value)) : basQuery.OrderByDescending(x => x.ProposalPrices.Max(y => y.Price)).ToList().AsQueryable();
                            if (search.fromDate != null && search.fromPrice != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value && r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value)).OrderBy(x => x.ProposalPrices.Max(y => y.Price));
                            }
                            if (search.fromDate != null && search.toPrice != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) >= search.fromDate.Value && r.ProposalPrices.Any(y => y.Price >= (decimal)search.fromPrice.Value)).OrderBy(x => x.ProposalPrices.Max(y => y.Price));
                            }
                            if (search.toDate != null)
                            {
                                basQuery = basQuery.Where(r => DbFunctions.TruncateTime(r.StartingDate) <= DbFunctions.TruncateTime(search.toDate)).OrderBy(x => x.ProposalPrices.Max(y => y.Price));
                            }
                            basQuery = basQuery.OrderBy(x => x.Region.Name);
                            break;


                        default:
                            basQuery = basQuery.AsNoTracking().OrderByDescending(x => x.Requestdate);
                            break;
                    }
                }
            }


            var baseQuery = basQuery.ToList().Select
                    (r => new RequestModelView
                    {
                        Id = r.Id,
                        Requster = factory.Create(r.User),
                        Title = r.Title,
                        Description = r.Description,
                        Requestdate = r.Requestdate,
                        RequestNumber = r.RequestNumber,
                        RequestType = r.RequestType,
                        RequestState = r.RequestState,
                        StartingDate = r.StartingDate,
                        ItemsInfo = r.ItemsInfo,
                        ExpireDate = r.ExpireDate,
                        LocationFromLatitude = r.LocationFromLatitude,
                        LocationFromlongitude = r.LocationFromlongitude,
                        LocationToLatitude = r.LocationToLatitude,
                        LocationToLongitude = r.LocationToLongitude,
                        Notes = r.Notes,
                        Period = r.Period,
                        Quantity = r.Quantity,
                        PermissionDate = r.PermissionDate,
                        // RegionName = r.Region.Name,
                        // Region=factory.Create(r.Region),
                        requestDetails = r.RequestDetailes.Select(RD => new RequestDetailsModelView { Id = RD.Id, truckId = RD.TruckId, truckName = RD.Truck.Name, truckNameArb = RD.Truck.NameArb, truckParentName = RD.Truck.Paranet != null ? RD.Truck.Paranet.Name : "", truckParentNameArb = RD.Truck.Paranet != null ? RD.Truck.Paranet.NameArb : "", trucksNo = RD.NumberOfTrucks, trucksImagePath = RD.Truck.TruckImagePath != null ? BaseURL + RD.Truck.TruckImagePath: null, requestTrucks = factory.Create(RD.Truck), Notes = RD.Notes }).AsQueryable(),
                        //  Requster = factory.Create(r.User),
                        LocationFrom = r.LocationFrom,
                        LocationTo = r.LocationTo,
                        ProposalPrice = r.ProposalPrices.Select(p => new ProposalPriceModelView { Id = p.Id, UserServiceProvider = factory.Create(p.ServiceProvider), Date = p.Date, Price = p.Price, PropsalStatus = p.PropsalStatus, ExpireDate = p.ExpireDate, IsAccepted = p.IsAccepted.HasValue ? p.IsAccepted.Value : false, Notes = p.Comments }).AsQueryable()


                    }
                ).Skip(pageSize * ((page ?? 1) - 1)).Take(pageSize);
            var totalcount = basQuery.Count();

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
                    Result = baseQuery
                };



        }

        public IEnumerable<RequestModelView> ProposalDetailes(int RequstId)
        {
            var Proppsal = _uow.Request.GetAll(x => x.FlgStatus == 1 && x.Id == RequstId, null, "RequestDetailes,proposalPrices").Select(a => new RequestModelView
            {
                RequestNumber = a.RequestNumber,
                Requestdate = a.Requestdate,
                Requster = factory.Create(a.User),
                ProposalPrice = a.ProposalPrices.Select(PP => new ProposalPriceModelView
                {
                    ServiceProviderId = PP.ServiceProviderId,
                    UserServiceProvider = factory.Create(_uow.User.GetById(PP.ServiceProviderId)),
                    Rating = factory.Create(_uow.User.GetById(PP.ServiceProviderId)).Rating,
                    Price = PP.Price,
                    IsAccepted = PP.IsAccepted.HasValue ? PP.IsAccepted.Value : false
                }).AsQueryable()
            });

            return Proppsal;
        }

        public object ProposalDetailesDescription(int ReqestId, int serviceprovider)
        {
            var Proppsal = _uow.Request.GetAll(x => x.FlgStatus == 1 && x.Id == ReqestId && x.ProposalPrices.Any(S => S.ServiceProviderId == serviceprovider), null, "RequestDetailes").Select(a => new { requestNumber = a.RequestNumber, RequestDetailes = a.RequestDetailes.Select(t => new { TrunckName = t.Truck.Name, TrucksNumber = t.NumberOfTrucks, Pricicing = t.RequestDetailesPrices.Select(p => new { Pric = p.Price }) }), TotalPrice = a.ProposalPrices.Where(c => c.ReqestId == ReqestId && c.ServiceProviderId == serviceprovider).FirstOrDefault().Price });

            return Proppsal;
        }

        public IEnumerable<RequestModelView> CompletedRequests()
        {
            var compltedRqusts = _uow.Request.GetAll(x => x.FlgStatus == 1 && x.RequestState == RequestStates.Accepted, null, "proposalPrices").ToList()
                .Where(r => r.ProposalPrices.Any(y => y.ReqestId == r.Id && y.IsAccepted == true)).Select(RD => new RequestModelView { RequestNumber = RD.RequestNumber, Requestdate = RD.Requestdate, ProposalPrice = RD.ProposalPrices.Select(p => new ProposalPriceModelView { Price = p.Price, UserServiceProvider = factory.Create(p.ServiceProvider), PropsalStatus = p.PropsalStatus }).AsQueryable() });
            return compltedRqusts;
        }

        public IEnumerable<RequestModelView> CurrentRequests(int requesterId)
        {
            var compltedRqusts = _uow.Request.GetAll(x => x.FlgStatus == 1 && x.RequesterId == requesterId && x.RequestState == RequestStates.Open && x.PermissionDate <= DateTime.UtcNow.Date && x.ProposalPrices == null || x.ProposalPrices.Any(y => y.IsAccepted.Value == false && y.ReqestId == x.Id), null, "proposalPrices")
            .Select(RD => new RequestModelView { Id = RD.Id, RequestNumber = RD.RequestNumber, Requestdate = RD.Requestdate, Count = PropsalesCounts(RD.Id), ProposalPrice = RD.ProposalPrices.Select(p => new ProposalPriceModelView { Price = p.Price }).AsQueryable() });
            return compltedRqusts;
        }

        public IEnumerable<RequestModelView> ExpiredRequests()
        {
            var today = DateTime.Today.Date;
            var compltedRqusts = _uow.Request.GetAll(x => x.FlgStatus == 1 && x.RequestState == RequestStates.Closed, null, "proposalPrices").Where(y => y.ExpireDate >= today).Select(RD => new RequestModelView { RequestNumber = RD.RequestNumber, Requestdate = RD.Requestdate, ProposalPrice = RD.ProposalPrices.Select(p => new ProposalPriceModelView { Price = p.Price }).AsQueryable() });
            return compltedRqusts;
        }

        public IEnumerable<RequestModelView> AllRequests(string searchTerm)
        {
            int requestno = 0;
            int.TryParse(searchTerm,out requestno);
            DateTime date;
            DateTime.TryParse(searchTerm,out date);
            if (searchTerm == null)
            {
                return _uow.Request.GetAll(x => x.FlgStatus == 1).OrderByDescending(x=>x.Id).ThenByDescending(x => x.StartingDate)
                                .Select(R => new RequestModelView { Id = R.Id, RequestNumber = R.RequestNumber, Requestdate = R.Requestdate, Title = R.Title, Description = R.Description, RequestState = R.RequestState, LocationFrom = R.LocationFrom, LocationTo = R.LocationTo, ExpireDate = R.ExpireDate });
            }
            var requests = _uow.Request.GetAll(x => x.FlgStatus == 1).OrderByDescending(x => x.Id).Where(x => x.Title.ToLower().Contains(searchTerm)|| x.Title.ToLower().StartsWith (searchTerm) || x.Description.ToLower().Contains(searchTerm) || x.Description.ToLower().StartsWith (searchTerm) || x.LocationFrom.ToLower().Contains(searchTerm)||x.LocationFrom.ToLower().Contains(searchTerm) || x.LocationFrom.ToLower().StartsWith(searchTerm) || x.LocationTo.ToLower().Contains(searchTerm) || x.Id.Equals(requestno) || x.RequestNumber.Equals(requestno)
            ||(x.ExpireDate.Year.Equals(date.Year)&& x.ExpireDate.Month.Equals(date.Month)&& x.ExpireDate.Day.Equals(date.Day)) || (x.Requestdate.Year.Equals(date.Year) && x.Requestdate.Month.Equals(date.Month) && x.Requestdate.Day.Equals(date.Day)))
                .Select(R => new RequestModelView { Id = R.Id, RequestNumber = R.RequestNumber, Requestdate = R.Requestdate, Title = R.Title, Description = R.Description, RequestState = R.RequestState, LocationFrom = R.LocationFrom, LocationTo = R.LocationTo, ExpireDate = R.ExpireDate });
            return requests;
        }

        public RequestModelView GetallDataforRequest(int id)
        {
            var request = _uow.Request.GetAll(x => x.FlgStatus == 1 && x.Id == id, null, "").ToList()
                .Select(r => new RequestModelView
                {
                    Id=r.Id,
                    RequestNumber = r.RequestNumber,
                    Requestdate = r.Requestdate,
                    //ServiceType = factory.Create(r.ServiceType),
                    LocationFrom = r.LocationFrom,
                    LocationTo = r.LocationTo,
                    RequestState = r.RequestState,
                    IsActive = r.IsActive,
                    Title = r.Title,
                    Description = r.Description,
                    RequestType = r.RequestType,
                    Requster = factory.Create(r.User),
                    requestDetails = r.RequestDetailes.Select(RD => new RequestDetailsModelView
                    {
                        truckName = RD.Truck.Name,
                        truckNameArb = RD.Truck.NameArb,
                        trucksNo = RD.NumberOfTrucks,
                        PricingDetailes = RD.RequestDetailesPrices
.Select(pric => new RequestDetailesPricesViewModel { ServiceProvider = factory.Create(pric.User), Price = pric.Price, Rating = pric.User.OverAllrating.HasValue ? pric.User.OverAllrating.Value : 0 })
                    }).AsQueryable(),
                    ProposalPrice = r.ProposalPrices.Select(propsal => new ProposalPriceModelView { Date = propsal.Date, IsAccepted = propsal.IsAccepted.HasValue ? propsal.IsAccepted.Value : false, Price = propsal.Price, PropsalStatus = propsal.PropsalStatus, UserServiceProvider = factory.Create(propsal.ServiceProvider) }).AsQueryable()
                }).FirstOrDefault();
            return request;
        }
        public int PropsalesCounts(int requestId)
        {
            int count = 0;
            count = _uow.RequestDetailesPrice.GetAll(x => x.FlgStatus == 1 && x.RequestDetaile.RequestId == requestId).Count();
            return count;
        }

        public ProposalPriceModelView UpadtePropsalState(int userId, int PropsalPriceId, PropsalStat State)
        {
            var Proposal = _uow.ProposalPrice.GetById(PropsalPriceId);

            var request = _uow.Request.GetAll(x => x.FlgStatus == 1 && x.ProposalPrices.Any(y => y.Id == PropsalPriceId)).FirstOrDefault();
            var Rejectedpropsales = _uow.ProposalPrice.GetAll(x => x.FlgStatus == 1 && x.ReqestId == request.Id);
            foreach (var item in Rejectedpropsales.ToList())
            {
                if ( item.Equals(Proposal))
                {

                    if (State == PropsalStat.Accepted)
                    {
                        Proposal.IsAccepted = true;
                        request.IsAccepted = true;
                        Proposal.PropsalStatus = PropsalStat.Accepted;
                        request.RequestState = RequestStates.Accepted;
                    }
                    else if (State == PropsalStat.Closed)
                    {
                        request.RequestState = RequestStates.Closed;
                        request.IsAccepted = false;
                        Proposal.IsAccepted = false;
                        Proposal.PropsalStatus = PropsalStat.Closed;
                    }
                    else if (State == PropsalStat.Cancelled)
                    {
                        request.RequestState = RequestStates.Cancelled;
                        request.IsAccepted = false;
                        Proposal.IsAccepted = false;
                        Proposal.PropsalStatus = PropsalStat.Cancelled;
                    }
                    else if (State == PropsalStat.Expired)
                    {
                        request.RequestState = RequestStates.Expired;
                        request.IsAccepted = false;
                        Proposal.IsAccepted = false;
                        Proposal.PropsalStatus = PropsalStat.Expired;
                    }
                    request.UpdatedBy = userId;
                    request.UpdatedOn = DateTime.UtcNow;
                    _uow.Request.Update(request.Id, request);
                    Proposal.UpdatedBy = userId;
                    Proposal.UpdatedOn = DateTime.UtcNow;
                    //Proposal.IsAccepted = true;
                    _uow.ProposalPrice.Update(Proposal.Id, Proposal);
                    _uow.Commit();
                }
                else
                {
                    //  item.IsAccepted = false;
                    request.UpdatedBy = userId;
                    request.RequestState = RequestStates.Rejected;
                    request.UpdatedOn = DateTime.UtcNow;
                    _uow.Request.Update(request.Id, request);
                    item.PropsalStatus = PropsalStat.Rejected;
                    _uow.ProposalPrice.Update(item.Id, item);
                    _uow.Commit();

                }


            }

            var propsalViewModel = factory.Create(Proposal);
            return propsalViewModel;


        }

        //hayam
        public RequestModelView UpadteRequestState(int userId, UpdateRequestStateViewModel updatestateModel)
        {
            var usersToken = new List<string>();
            var request = _uow.Request.GetById(updatestateModel.RequestId);
            if (request != null)
            {


                request.RequestState = updatestateModel.State;
                request.UpdatedBy = userId;
                request.UpdatedOn = DateTime.UtcNow;
                _uow.Request.Update(updatestateModel.RequestId, request);
                _uow.Commit();
                if (updatestateModel.State == RequestStates.Accepted)
                {
                    if (request.RequesterId != 0)
                    {

                        var devices = _customerService.GetDeviceByTokenAndType(request.RequesterId);
                        NotificationViewModel notymodel = new NotificationViewModel();
                        if (devices.Count()>0)
                        {
                            foreach (var item in devices)
                            {
                                if (item !=null)
                                {
                                    usersToken.Add(item.Fcmtoken);
                                 

                                }
                            }
                        }

                        notymodel.Body = "تم قبول هذا الطلب" + ":" + request.Title + '-' + " this request state has changed to accept" + ":" + request.Title;
                        // notymodel.BodyArb = "تم قبول هذا الطلب" + request.Title;
                        notymodel.Date = DateTime.Now;
                        notymodel.Title = "تم قبول الطلب" + request.Title + '-' + " Request was Accepted" + request.Title;
                        notymodel.Seen = false;
                        notymodel.registration_ids = usersToken;
                        notymodel.ReceiverId = userId;
                        notymodel.Type = NotificationType.UpdateState;
                        notymodel.BodyArb = "تم قبول هذا الطلب" + " " + ":" + request.Title;
                        notymodel.BodyEng = " this request state has changed to accept" + ":" + request.Title;
                        notymodel.TitleArb = "تم قبول الطلب" + " " + ":" + request.Title;
                        notymodel.TitleEng = " Request was Accepted" + request.Title;

                        PushNotification push = new PushNotification();
                        push.PushNotifications(notymodel, "");
                        _iNotificationService.AddNoty(notymodel, userId);

                    }
                    request.IsAccepted = true;
                    request.UpdatedBy = userId;
                    request.UpdatedOn = DateTime.UtcNow;
                    request.RequestState = RequestStates.Accepted;
                    _uow.Request.Update(request.Id, request);
                    _uow.Commit();
                }
                var entity = factory.Parse(updatestateModel.RatingModel);
                if (entity == null)
                {
                    throw new Exception("005");
                }
                entity.ServiceProviderId = updatestateModel.ServiceProviderId;
                entity.Date = DateTime.UtcNow.Date; ;
                entity.RequstId = updatestateModel.RequestId;
                entity.ServiceRequestId = userId;
                entity.CreatedBy = userId;
                entity.CreatedOn = DateTime.UtcNow.Date;
                entity.FlgStatus = 1;
                entity.UpdatedBy = null;
                entity.UpdatedOn = null;
                _uow.Rating.Add(entity);
                _uow.Commit();
                var model = factory.Create(entity);
                var ratingCount = _uow.Rating.GetAll(x => x.FlgStatus == 1, null, "").Where(y => y.ServiceProviderId == updatestateModel.ServiceProviderId).Count();
                var ratingavg = _uow.Rating.GetAll(x => x.FlgStatus == 1, null, "").Where(y => y.ServiceProviderId == updatestateModel.ServiceProviderId).Average(s => s.RatingValue);
                var serviceprovider = _uow.User.GetById(updatestateModel.ServiceProviderId);
                serviceprovider.OverAllrating = Convert.ToDecimal(Math.Round(ratingavg));
                _uow.User.Update(updatestateModel.ServiceProviderId, serviceprovider);
                _uow.Commit();
            }
            else
            {
                var propsalePrices = _uow.ProposalPrice.GetAll(x => x.FlgStatus == 1 && x.ReqestId == updatestateModel.RequestId && x.PropsalStatus != PropsalStat.Accepted).ToList();
                if (propsalePrices.Count() > 0)
                {
                    foreach (var item in propsalePrices)
                    {
                        item.UpdatedBy = userId;
                        item.UpdatedOn = DateTime.UtcNow;

                        if (updatestateModel.State == RequestStates.Closed)
                        {
                            item.PropsalStatus = PropsalStat.Closed;
                            if (request.RequesterId != 0)
                            {
                              

                                var devices = _customerService.GetDeviceByTokenAndType(request.RequesterId);
                                if (devices.Count()>0)
                                {
                                    NotificationViewModel notymodel = new NotificationViewModel();
                                    foreach (var device in devices)
                                    {
                                        if (device != null)
                                        {
                                            notymodel.DeviceToken = device.Fcmtoken;
                                        }
                                    }
                                notymodel.Body = "تم اغلاق هذا الطلب" + ":" + request.Title + '-' + " this request state has changed to Closed " + ":" + request.Title;
                                // notymodel.BodyEng = "تم اغلاق هذا الطلب" + request.Title;
                                notymodel.Date = DateTime.Now;
                                notymodel.Title = "اغلاق الطلب" + '-' + " Request was Closed ";
                                notymodel.Seen = false;
                                notymodel.Type = NotificationType.UpdateState;
                                notymodel.BodyArb = "تم اغلاق هذا الطلب" + " " + ":" + request.Title;
                                notymodel.BodyEng = " this request state has changed to Closed " + ":" + request.Title;
                                notymodel.TitleArb = "اغلاق الطلب" + " " + ":" + request.Title;
                                notymodel.TitleEng = " Request was Closed " + request.Title;
                                notymodel.ReceiverId = userId;
                                PushNotification push = new PushNotification();
                                push.PushNotifications(notymodel, "");                                
                                _iNotificationService.AddNoty(notymodel, userId);
                                }
                            }
                        }
                        if (updatestateModel.State == RequestStates.Cancelled)
                        {
                            if (request.RequesterId != 0)
                            {

                                item.PropsalStatus = PropsalStat.Cancelled;
                                var devices= _customerService.GetDeviceByTokenAndType(request.RequesterId);
                                NotificationViewModel notymodel = new NotificationViewModel();

                                notymodel.Body = "تم الغاء هذا الطلب" + ":" + request.Title + '-' + " this request state has changed to Cancelled " + ":" + request.Title;
                                // notymodel.BodyArb = "تم الغاء هذا الطلب" + request.Title;
                                notymodel.Date = DateTime.Now;
                                notymodel.Title = "الغاء طلب" + '-' + " Request Was Cancelled ";
                                notymodel.Seen = false;
                               
                                notymodel.Type = NotificationType.UpdateState;
                                notymodel.BodyArb = "تم الغاء هذا الطلب" + " " + ":" + request.Title;
                                notymodel.BodyEng = " this request state has changed to Cancelled " + ":" + request.Title;
                                notymodel.TitleArb = "الغاء طلب" + " " + ":" + request.Title;
                                notymodel.TitleEng = " Request Was Cancelled " + request.Title;
                                notymodel.ReceiverId = userId;
                                if (devices.Count()>0)
                                {
                                    foreach (var requestDev in devices)
                                    {
                                        if (requestDev != null)
                                        {
                                            notymodel.DeviceToken = requestDev.Fcmtoken;
                                        }
                                    }
                                }

                                PushNotification push = new PushNotification();
                                push.PushNotifications(notymodel, "");
                                _iNotificationService.AddNoty(notymodel, userId);

                            }
                        }
                        if (updatestateModel.State == RequestStates.Expired)
                        {
                            item.PropsalStatus = PropsalStat.Expired;
                            if (request.RequesterId != 0)
                            {
                                NotificationViewModel notymodel = new NotificationViewModel();
                                notymodel.Body = "تم انتهاء صلاحيه هذا الطلب" + ":" + request.Title + '-' + " this request state has changed to Expired " + ":" + request.Title;
                                // notymodel.BodyEng = "تم انتهاء صلاحيه هذا الطلب" + request.Title;
                                notymodel.Date = DateTime.Now;
                                notymodel.Title = "انتهاء صلاحيه الطلب" + '-' + "Request was Expired ";
                                notymodel.Seen = false;                               
                                notymodel.Type = NotificationType.UpdateState;
                                notymodel.BodyArb = "تم انتهاء صلاحيه هذا الطلب" + " " + ":" + request.Title;
                                notymodel.BodyEng = " this request state has changed to Expired " + ":" + request.Title; ;
                                notymodel.TitleArb = "انتهاء صلاحيه الطلب" + " " + ":" + request.Title;
                                notymodel.TitleEng = "Request was Expired " + " " + ":" + request.Title;
                                notymodel.ReceiverId = userId;
                                var devices = _customerService.GetDeviceByTokenAndType(request.RequesterId);
                                if (devices.Count()>0)
                                {
                                    foreach (var Device in devices)
                                    {
                                        if (Device != null)
                                        {

                                            notymodel.DeviceToken = Device.Fcmtoken;
                                           
                                      }
                                    }
                                }
                                PushNotification push = new PushNotification();
                                push.PushNotifications(notymodel, "");
                                //  PushNotification(notymodel, deviceToken.ToString(), deviceType.ToString());
                                _iNotificationService.AddNoty(notymodel, userId);
                            }
                        }

                        item.ServiceProviderId = updatestateModel.ServiceProviderId;
                        item.Comments = updatestateModel.Comment;
                        _uow.ProposalPrice.Update(item.Id, item);
                        _uow.Commit();
                    }
                }

            }


            var requestViewModel = factory.Create(request);
            return requestViewModel;


        }

        public void DeActiveRequest(int offerId, bool state)
        {
            var existingrequest = _uow.Request.GetById(offerId);
            if (existingrequest == null)
            {
                throw new Exception("011");
            }
            existingrequest.IsActive = state;
            _uow.Request.Deactivate(offerId, state);
            _uow.Commit();
        }

        public int RequestedNos(int userId)
        {          
          return   _uow.Request.GetAll(r => r.FlgStatus == 1&& r.IsActive && r.RequesterId == userId && r.RequestState == RequestStates.Open && (DbFunctions.TruncateTime(r.PermissionDate ) >= DbFunctions.TruncateTime(DateTime.Now) && DbFunctions.TruncateTime(r.ExpireDate) >= DbFunctions.TruncateTime(DateTime.Now)), null, "RequestDetailes,proposalPrices").Count();
        }
    }
}
