using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ejab.BAL.ModelViews;
using Ejab.BAL.UnitOfWork;
using Ejab.BAL.Common;
using System.Data.Entity;
using Ejab.BAL.ModelViews.Reports;
using Ejab.DAl.Common;
using System.Configuration;

namespace Ejab.BAL.Services.Reports
{
    public class ReportService : IReportService
    {
        IUnitOfWork _uow;
        ModelFactory Factory;
        public ReportService(IUnitOfWork Uow)
        {
            _uow = Uow;
            Factory = new ModelFactory();
        }
        public IEnumerable<RequestViewModel> AllRequestsForCustomer(int? CustomerId)
        {
            var requests = _uow.Request.GetAll(x => x.FlgStatus == 1 ).OrderByDescending(x=>x.Id);
            if (CustomerId==null)
            {
                return requests.Select(p => new RequestViewModel
                {
                    Id = p.Id,
                    ReuestNumber = p.RequestNumber,
                    Customer = p.User.FirstName + "  " + p.User.LastName,
                    RequestDate = p.Requestdate,
                    Description = p.Description,
                    LocationFrom = p.LocationFrom,
                    LocationTo = p.LocationTo,
                    RegionName = p.Region != null ? p.Region.Name : "  ",
                    Quantity = p.Quantity,
                    Period = p.Period,

                    ItemInfo = p.ItemsInfo,
                    Notes = p.Notes,
                    RequestState = p.RequestState.ToString()

                }); ;
            }
            if (CustomerId !=null)
            {
               return  requests.Where(x => x.RequesterId == CustomerId).Select(p => new RequestViewModel
                {
                   Id = p.Id,
                   ReuestNumber = p.RequestNumber,
                    Customer = p.User.FirstName + "  " + p.User.LastName,
                    RequestDate = p.Requestdate,
                    Description = p.Description,
                    LocationFrom = p.LocationFrom,
                    LocationTo = p.LocationTo,
                    RegionName = p.Region != null ? p.Region.Name : "  ",
                    Quantity = p.Quantity,
                    Period = p.Period,

                    ItemInfo = p.ItemsInfo,
                    Notes = p.Notes,
                   RequestState = p.RequestState.ToString()

               }); ;
            }
            return null;
        }

        public IEnumerable<ModelViews.Reports.OfferViewModel> AllOfferForServiceProvider(int? ServiceProviderId)
        {
            var offers = _uow.Offer.GetAll(x => x.FlgStatus == 1);
            if (ServiceProviderId != null)
            {
           offers=offers.Where(x=> x.UserId == ServiceProviderId);
            }
           
           return offers.Select(o => new ModelViews.Reports.OfferViewModel { OfferNumber = o.OfferNumber,OfferDate=o.PublishDate, ServiceProvider = o.User.FirstName + "  " + o.User.LastName, Price = o.Price, Title = o.Title, Description = o.Description, RegionName = o.Region != null ? o.Region.Name : "  " , quantity = o.Quantity, Period = o.Period, OfferState = o.AcceptOffers.FirstOrDefault().OfferState.ToString() });

        }

        public IEnumerable<UserDTO> AllServiceProviders()
        {
            var allServiceProvider = _uow.User.GetAll(x => x.FlgStatus == 1 && x.CustomerType == CustomerTypes.ServiceProvider).OrderByDescending(x=>x.Id);
            return allServiceProvider.Select(U => new UserDTO { FirstName = U.FirstName, LastName = U.LastName, Email = U.Email, Mobile = U.Mobile, Rating = U.OverAllrating.HasValue ? U.OverAllrating.Value : 0, IsActive = U.IsActive });
        }

        public IEnumerable<CustomerDTO> AllCustomer()
        {
            var allCustomers = _uow.User.GetAll(x => x.FlgStatus == 1 && x.CustomerType == CustomerTypes.Requester).OrderByDescending(x => x.Id);
                        
            return  allCustomers.Select(U => new CustomerDTO { FirstName = U.FirstName, LastName = U.LastName, Email = U.Email, Mobile = U.Mobile });
        }

      

        public IEnumerable<TruckDTO> AllEquipment(string name, int? parentId)
        {
            var BaseURL = ConfigurationManager.AppSettings["BaseURL"].ToString();
            var trucks = _uow.Truck.GetAll(x => x.FlgStatus == 1 && x.TypeId == 2);
            if (name != null || name == "  ")
            {
                trucks = trucks.Where(y => y.Name.Contains(name) || y.NameArb.Contains(name));
            }
            if (parentId.HasValue)
            {
                trucks = trucks.Where(y => y.ParanetId == parentId);
            }
            return trucks.Select(t => new TruckDTO { Id=t.Id, TruckNameArb = t.NameArb, TruckNameEng = t.Name, TruckParentNameArb = t.Paranet != null ? t.Paranet.NameArb : "  ", TruckParentNameEng = t.Paranet != null ? t.Paranet.Name : "  ", ImagePath = t.TruckImagePath != null ? BaseURL + t.TruckImagePath : null });
        }

        public IEnumerable<PropsalesViewModel> PropsalesOFServiceProvider(int? providerId)
        {
            var Propsales = _uow.ProposalPrice.GetAll(x => x.FlgStatus == 1);
            if (providerId !=null)
            {
                Propsales = Propsales.OrderByDescending(x => x.Id).Where(y => y.ServiceProviderId == providerId);

            }           
         
            return Propsales.OrderByDescending(x => x.Id).ToList().Select(p => new PropsalesViewModel
            {
                RequestNumber = p.Request.RequestNumber,
                ServicrProvider = p.ServiceProvider.FirstName + " " + p.ServiceProvider.LastName,
                Request = p.Request != null ? Factory.Create(p.Request) : null,
                Rating = p.ServiceProvider.OverAllrating,
                RequestTitle = p.Request != null ? p.Request.Title : "  ",
                Date = p.Date,
                Price = p.Price,

                CustomerName = p.Request.User.FirstName + "  " + p.Request.User.LastName,
                PropsalStat = p.PropsalStatus.ToString()
            });


        }

        public IEnumerable<PropsalesViewModel> PropsalesinDates(DateTime? fromDate, DateTime? toDate)
        {
            var Propsales = _uow.ProposalPrice.GetAll(x => x.FlgStatus == 1);
            if (!fromDate.HasValue && !toDate.HasValue)
            {
                return Propsales.OrderByDescending(x => x.Id).ToList().Select(p => new PropsalesViewModel
                {
                    ServicrProvider = p.ServiceProvider.FirstName + " " + p.ServiceProvider.LastName,
                    Date = p.Date,
                    Price = p.Price,
                    RequestNumber = p.Request.RequestNumber,
                    CustomerName = p.Request.User.FirstName + "  " + p.Request.User.LastName,
                    Rating = p.ServiceProvider.OverAllrating,
                    Request = p.Request != null ? Factory.Create(p.Request) : null,
                    PropsalStat = p.PropsalStatus.ToString()
                });
            }
            return Propsales.OrderByDescending(x => x.Id).Where(y => DbFunctions.TruncateTime(y.Date) >= fromDate.Value && DbFunctions.TruncateTime(y.Date) <= toDate.Value).ToList()
                  .Select(p => new PropsalesViewModel { ServicrProvider = p.ServiceProvider.FirstName + " " + p.ServiceProvider.LastName, Date = p.Date, Price = p.Price, RequestNumber = p.Request.RequestNumber, CustomerName = p.Request.User.FirstName + "  " + p.Request.User.LastName, Rating = p.ServiceProvider.OverAllrating, Request = p.Request != null ? Factory.Create(p.Request) : null, PropsalStat = p.PropsalStatus.ToString() });
        }
       
        public IEnumerable<PropsalesViewModel> PropsalesBtState(int state)
        {
            var Propsales = _uow.ProposalPrice.GetAll(x => x.FlgStatus == 1 );
            if (state==2)
            {
                Propsales = Propsales.OrderByDescending(x => x.Id).Where(x => x.PropsalStatus == PropsalStat.Accepted);
            }
           
            if (state == 6)
            {
                Propsales = Propsales.OrderByDescending(x => x.Id).Where(x => x.PropsalStatus == PropsalStat.Expired || x.PropsalStatus == PropsalStat.Cancelled|| x.PropsalStatus == PropsalStat.Closed || x.PropsalStatus == PropsalStat.Rejected);
            }
            if (state == 1)
            {
                Propsales = Propsales.OrderByDescending(x => x.Id).Where(x => x.PropsalStatus == PropsalStat.Open);
            }
           
            return Propsales.ToList().Select(p => new PropsalesViewModel
            {
                ServicrProvider = p.ServiceProvider.FirstName + " " + p.ServiceProvider.LastName,
               
                Rating = p.ServiceProvider.OverAllrating,
                Date = p.Date,
                Price = p.Price,
                CustomerName = p.Request.User.FirstName + "  " + p.Request.User.LastName,
                RequestNumber = p.Request.RequestNumber,
                RequestTitle = p.Request != null ? p.Request.Title : "  ",
                Request = p.Request != null ? Factory.Create(p.Request) : null,
                PropsalStat= p.PropsalStatus.ToString()
            });
        }

        public IEnumerable<UserDTO >ServiceProvidersByName(string Name)
        {
            var serviceProvider = _uow.User.GetAll(x => x.FlgStatus == 1 && x.CustomerType == CustomerTypes.ServiceProvider );
            if (Name==null)
            {
                return serviceProvider.Select(U => new UserDTO { FirstName = U.FirstName, LastName = U.LastName, Email = U.Email, Mobile = U.Mobile, Rating = U.OverAllrating.HasValue ? U.OverAllrating.Value : 0, IsActive = U.IsActive });
            }
            return serviceProvider.Where(x=> x.FirstName.Contains(Name) || x.FirstName.StartsWith(Name) || x.LastName.Contains(Name) || x.LastName.StartsWith(Name) || x.Email.Equals(Name) || x.Mobile.Equals(Name)).Select(U => new UserDTO { FirstName = U.FirstName, LastName = U.LastName, Email = U.Email, Mobile = U.Mobile, Rating = U.OverAllrating.HasValue ? U.OverAllrating.Value : 0, IsActive = U.IsActive });
        }
      
        public  IEnumerable<CustomerDTO> CustomerByName(string Name)
        {
            var allCustomers = _uow.User.GetAll(x => x.FlgStatus == 1 && x.CustomerType == CustomerTypes.Requester);
            if (Name==null )
            {
                return allCustomers.Select(U => new CustomerDTO { FirstName = U.FirstName, LastName = U.LastName, Email = U.Email, Mobile = U.Mobile });
            }
            return allCustomers.Where(x => x.FirstName.Contains(Name) || x.FirstName.StartsWith(Name) || x.LastName.Contains(Name) || x.LastName .StartsWith(Name) || x.Email.Equals(Name) || x.Mobile.Equals(Name)).Select(U => new CustomerDTO { FirstName = U.FirstName, LastName = U.LastName, Email = U.Email, Mobile = U.Mobile});
        }

        public IEnumerable<ComplaintViewModel> AllComplaints()
        {
            var allComplaint = _uow.SuggestionsComplaint.GetAll(x => x.FlgStatus == 1);
           
                return allComplaint.OrderByDescending(x => x.Id).Select(c => new ComplaintViewModel { Date = c.Date, Cause = c.Cause, Name = c.Name, Email = c.Email, Phone = c.Phone });
           
        }

        public IEnumerable<ComplaintViewModel> ComplaintsByDate(DateTime? fromDate, DateTime? toDate)
        {
            var allComplaint = _uow.SuggestionsComplaint.GetAll(x => x.FlgStatus == 1);
            if (fromDate.HasValue && toDate.HasValue)
            {
                return allComplaint.OrderByDescending(x => x.Id).Where(x => DbFunctions.TruncateTime(x.Date) >= fromDate.Value && 
                DbFunctions.TruncateTime(x.Date)<= toDate.Value).Select(c => new ComplaintViewModel
                { Date = c.Date, Cause = c.Cause, Name = c.Name, Email = c.Email, Phone = c.Phone });
            }
            return allComplaint.OrderByDescending(x => x.Id).Select(c => new ComplaintViewModel { Date = c.Date, Cause = c.Cause, Name = c.Name, Email = c.Email, Phone = c.Phone });
        }

        public IEnumerable<TruckDTO> AllTrucksUnderParent(int? parentId)
        {
            var BaseURL = ConfigurationManager.AppSettings["BaseURL"].ToString();           

            List<TruckDTO> ChildTrucks = new List<TruckDTO>();
            var ChildNodes = _uow.Truck.GetAll(x => x.ParanetId == parentId && x.FlgStatus == 1 && x.TypeId == 1).ToList();
            foreach (var item in ChildNodes)
            {
                if (item != null)
                {
                    TruckDTO truckObj = new TruckDTO();
                    truckObj.Id = item.Id;
                    truckObj.TruckNameArb = item.NameArb;
                    truckObj.TruckNameEng = item.Name;
                    truckObj.TruckParentNameArb = item.Paranet!=null? item.Paranet.NameArb :"";
                    truckObj.TruckParentNameEng = item.Paranet != null ? item.Paranet.Name : "";
                  
                    truckObj.ImagePath = item.TruckImagePath != null ? BaseURL+item.TruckImagePath : null;
                    truckObj.ParentImagePath = item.Paranet != null ? BaseURL + item.Paranet.TruckImagePath : null;
                    if (_uow.Truck.GetAll(x => x.ParanetId == item.Id && x.FlgStatus == 1).ToList().Count() > 0)
                    {
                        var childNode = GetChildsOfNode(item.Id);
                        truckObj.Childs = childNode;

                    }
                    ChildTrucks.Add(truckObj);

                }
            }

            if (ChildTrucks == null)
            {
                throw new Exception("006");
            }

            List<TruckDTO> Trucks = new List<TruckDTO>();
            if (ChildTrucks.Count() > 0)
            {
                foreach (var item in ChildTrucks.ToList())
                {
                    TruckDTO truckObj = new TruckDTO();
                    truckObj.Id = item.Id;
                    truckObj.TruckNameArb = item.TruckNameArb;
                    truckObj.TruckNameEng = item.TruckNameEng;
                    truckObj.TruckParentNameArb = item.TruckParentNameArb  != null ? item.TruckParentNameArb  : "";
                    truckObj.TruckParentNameEng = item.TruckParentNameEng  != null ? item.TruckParentNameEng : "";
                    truckObj.ImagePath = item.ImagePath != null ? BaseURL+item.ImagePath : null;
                   // truckObj.ParentImagePath = item.pa != null ? BaseURL + item.Paranet.TruckImagePath : null;
                    if (_uow.Truck.GetAll(x => x.ParanetId == item.Id && x.FlgStatus == 1).ToList().Count() > 0)
                    {
                        var childNode = GetChildsOfNode(item.Id);
                        truckObj.Childs = childNode;

                    }
                    ChildTrucks.Add(truckObj);
                }
            }
            return ChildTrucks;

        }

        public List<TruckDTO> GetChildsOfNode(int id)
        {
            var BaseURL = ConfigurationManager.AppSettings["BaseURL"].ToString();
            List<TruckDTO> Trucks = new List<TruckDTO>();
            var ChildNodes = _uow.Truck.GetAll(x => x.ParanetId == id && x.FlgStatus == 1).ToList();
            foreach (var item in ChildNodes)
            {
                TruckDTO truckObj = new TruckDTO();
                truckObj.Id = item.Id;
                truckObj.TruckNameArb  = item.NameArb;
                truckObj.TruckNameEng  = item.Name;
              
                truckObj.TruckParentNameArb  = item.Paranet != null ? item.Paranet.NameArb : "  ";
                truckObj.TruckParentNameEng  = item.Paranet != null ? item.Paranet.Name : "  ";

                truckObj.ImagePath  = item.TruckImagePath != null ? BaseURL + item.TruckImagePath : null;
                truckObj.ParentImagePath = item.Paranet != null ? BaseURL + item.Paranet.TruckImagePath : null;
                if (_uow.Truck.GetAll(x => x.ParanetId == item.Id && x.FlgStatus == 1).ToList().Count() > 0)
                {
                    truckObj.Childs = GetChildsOfNode(item.Id);
                }
                Trucks.Add(truckObj);
            }
            return Trucks;
        }

        public IEnumerable< TruckDTO> TruckByName(string name)
        {
            var BaseURL = ConfigurationManager.AppSettings["BaseURL"].ToString();
            var trucks = _uow.Truck.GetAll(x => x.FlgStatus == 1 && x.TypeId == 1);
            if (name != null )
            {
                trucks = trucks.Where(y => y.Name.Contains(name) || y.NameArb.Contains(name) || y.Name.StartsWith(name));
            }
           
            return trucks.Select(t => new TruckDTO {Id=t.Id, TruckNameArb = t.NameArb, TruckNameEng = t.Name, TruckParentNameArb = t.Paranet != null ? t.Paranet.NameArb : "  ", TruckParentNameEng = t.Paranet != null ? t.Paranet.Name : "  ", ImagePath = t.TruckImagePath != null ? BaseURL + t.TruckImagePath : null ,ParentImagePath= t.Paranet != null&& t.Paranet.TruckImagePath!=null ? BaseURL +t.Paranet.TruckImagePath  : null });
        }

        public IEnumerable<TruckDTO> EquipmentByName(string name)
        {
            var BaseURL = ConfigurationManager.AppSettings["BaseURL"].ToString();
            var trucks = _uow.Truck.GetAll(x => x.FlgStatus == 1 && x.TypeId == 2);
            if (name != null )
            {
                trucks = trucks.Where(y => y.Name.Contains(name) || y.NameArb.Contains(name) || y.Name.StartsWith(name));
            }

            return trucks.Select(t => new TruckDTO { Id = t.Id, TruckNameArb = t.NameArb, TruckNameEng = t.Name, TruckParentNameArb = t.Paranet != null ? t.Paranet.NameArb : "  ", TruckParentNameEng = t.Paranet != null ? t.Paranet.Name : "  ", ImagePath = t.TruckImagePath != null ? BaseURL + t.TruckImagePath : null, ParentImagePath = t.Paranet != null && t.Paranet.TruckImagePath != null ? BaseURL + t.Paranet.TruckImagePath : null });
        }

        public IEnumerable<TruckDTO> AllEquipmentUnderParent(int? parentId)
        {
            var BaseURL = ConfigurationManager.AppSettings["BaseURL"].ToString();
            //var trucks = _uow.Truck.GetAll(x => x.FlgStatus == 1 && x.TypeId == 2);
            //if (parentId.HasValue)
            //{
            //    trucks = trucks.Where(y => y.ParanetId == parentId);
            //}
            //return trucks.Select(t => new TruckDTO { TruckNameArb = t.NameArb, TruckNameEng = t.Name, TruckParentNameArb = t.Paranet != null ? t.Paranet.NameArb : "  ", TruckParentNameEng = t.Paranet != null ? t.Paranet.Name : "  ", ImagePath = t.TruckImagePath != null ? BaseURL + t.TruckImagePath : null });
           
            List<TruckDTO> ChildTrucks = new List<TruckDTO>();
            var ChildNodes = _uow.Truck.GetAll(x => x.ParanetId == parentId && x.FlgStatus == 1 && x.TypeId == 2).ToList();
            foreach (var item in ChildNodes)
            {
                if (item != null)
                {

                    TruckDTO truckObj = new TruckDTO();
                    truckObj.Id = item.Id;
                    truckObj.TruckNameArb = item.NameArb;
                    truckObj.TruckNameEng = item.Name;
                    truckObj.TruckParentNameArb = item.Paranet != null ? item.Paranet.NameArb : "";
                    truckObj.TruckParentNameEng = item.Paranet != null ? item.Paranet.Name : "";
                    truckObj.ImagePath = item.TruckImagePath != null ? BaseURL+item.TruckImagePath : null;
                    truckObj.ParentImagePath = item.Paranet != null ? BaseURL + item.Paranet.TruckImagePath : null;
                    if (_uow.Truck.GetAll(x => x.ParanetId == item.Id && x.FlgStatus == 1).ToList().Count() > 0)
                    {
                        var childNode = GetChildsOfNode(item.Id);
                        truckObj.Childs = childNode;

                    }
                    ChildTrucks.Add(truckObj);

                }
            }

            if (ChildTrucks == null)
            {
                throw new Exception("006");
            }

            List<TruckDTO> Trucks = new List<TruckDTO>();
            if (ChildTrucks.Count() > 0)
            {
                foreach (var item in ChildTrucks.ToList())
                {
                    TruckDTO truckObj = new TruckDTO();
                    truckObj.TruckNameArb = item.TruckNameArb;
                    truckObj.TruckNameEng = item.TruckNameEng;
                    truckObj.TruckParentNameArb = item.TruckParentNameArb != null ? item.TruckParentNameArb : "";
                    truckObj.TruckParentNameEng = item.TruckParentNameEng != null ? item.TruckParentNameEng : "";
                    truckObj.ImagePath = item.ImagePath != null ? BaseURL+item.ImagePath : null;
                    if (_uow.Truck.GetAll(x => x.ParanetId == item.Id && x.FlgStatus == 1).ToList().Count() > 0)
                    {
                        var childNode = GetChildsOfNode(item.Id);
                        truckObj.Childs = childNode;

                    }
                    ChildTrucks.Add(truckObj);
                }
            }
            return ChildTrucks;
        }

        public IEnumerable<NamesDTO> SearchProviderName()
        {
            var names = _uow.User.GetAll(x => x.FlgStatus == 1 && x.CustomerType == CustomerTypes.ServiceProvider ).Select(y=>new NamesDTO {Id=y.Id,FullName=y.FirstName+ " "+y.LastName });
            return names;
        }

        public IEnumerable<RequestViewModel> AllRequestsInIntervale(DateTime? fromDate, DateTime? toDate)
        {
            var requests = _uow.Request.GetAll(x => x.FlgStatus == 1 );
            if (fromDate.HasValue && toDate.HasValue)
            {
                return requests.OrderByDescending(x => x.Id).Where(x => DbFunctions.TruncateTime(x.StartingDate) >= fromDate.Value &&
                DbFunctions.TruncateTime(x.StartingDate) <= toDate.Value).Select(p => new RequestViewModel
                {
                    Id = p.Id,
                    ReuestNumber = p.RequestNumber,
                    Customer = p.User.FirstName + "  " + p.User.LastName,
                    RequestDate = p.Requestdate,
                    Description = p.Description,
                    LocationFrom = p.LocationFrom,
                    LocationTo = p.LocationTo,
                    RegionName = p.Region != null ? p.Region.Name : "  ",
                    Quantity = p.Quantity,
                    Period = p.Period,

                    ItemInfo = p.ItemsInfo,
                    Notes = p.Notes,
                    RequestState = p.RequestState.ToString()

                });
            }
            return requests.OrderByDescending(x => x.Id).Select(p => new RequestViewModel
            {
                Id = p.Id,
                ReuestNumber = p.RequestNumber,
                Customer = p.User.FirstName + "  " + p.User.LastName,
                RequestDate = p.Requestdate,
                Description = p.Description,
                LocationFrom = p.LocationFrom,
                LocationTo = p.LocationTo,
                RegionName = p.Region != null ? p.Region.Name : "  ",
                Quantity = p.Quantity,
                Period = p.Period,
               
                ItemInfo = p.ItemsInfo,
                Notes = p.Notes,
                RequestState=p.RequestState.ToString()

            });
        }

        public IEnumerable<RequestViewModel> AllRequestsInRegion(int? RegionId)
        {
            var requests = _uow.Request.GetAll(x => x.FlgStatus == 1 );
            if (RegionId !=null)
            {
                requests = requests.OrderByDescending(x => x.Id).Where(x=>x.RegionId== RegionId);
            }
            return requests.OrderByDescending(x => x.Id).Select(p => new RequestViewModel
            {
                Id = p.Id,
                ReuestNumber =p.RequestNumber,
                    Customer=p.User.FirstName+"  "+ p.User.LastName,
                    RequestDate=p.Requestdate,
                    Description =p.Description,
                    LocationFrom=p.LocationFrom,
                    LocationTo=p.LocationTo,
                    RegionName=p.Region!=null ? p.Region.Name :"  ",
                    Quantity=p.Quantity,
                    Period=p.Period,
                
                    ItemInfo=p.ItemsInfo,
                Notes = p.Notes,
                RequestState = p.RequestState.ToString()

            });
        }

        public IEnumerable<RequestViewModel> AllRequestsByState(int? StateId)
        {
            var requests = _uow.Request.GetAll(x => x.FlgStatus == 1 );
            if (StateId == 2)
            {
                requests = requests.OrderByDescending(x => x.Id).Where(x => x.RequestState == RequestStates.Accepted);
            }
          
            if (StateId == 6)
            {
                requests = requests.OrderByDescending(x => x.Id).Where(x => x.RequestState == RequestStates.Expired || x.RequestState == RequestStates.Cancelled || x.RequestState == RequestStates.Closed || x.RequestState == RequestStates.Rejected);
            }
            if (StateId == 1)
            {
                requests = requests.OrderByDescending(x => x.Id).Where(x => x.RequestState == RequestStates.Open);
            }
           
            return requests.Select(p => new RequestViewModel
            {Id=p.Id,
                ReuestNumber = p.RequestNumber,
                Customer = p.User.FirstName + "  " + p.User.LastName,
                RequestDate = p.Requestdate,
                Description = p.Description,
                LocationFrom = p.LocationFrom,
                LocationTo = p.LocationTo,
                RegionName = p.Region != null ? p.Region.Name : "  ",
                Quantity = p.Quantity,
                Period = p.Period,
                ItemInfo = p.ItemsInfo,
                Notes = p.Notes,
                RequestState = p.RequestState.ToString()
            });
        }
        public IEnumerable<ModelViews.Reports.OfferViewModel> AllOfferInInterval(DateTime? fromDate, DateTime? toDate)
        {
            var offers = _uow.Offer.GetAll(x => x.FlgStatus == 1 );

            if (fromDate != null )
            { offers = offers.OrderByDescending(x => x.Id).Where(r => DbFunctions.TruncateTime(r.PublishDate) >= fromDate.Value); }

            if (toDate != null)
            {
                offers = offers.OrderByDescending(x => x.Id).Where(r =>  DbFunctions.TruncateTime(r.PublishDate) <= toDate.Value);
           
        }
            return offers.OrderByDescending(x => x.Id).Select(o => new ModelViews.Reports.OfferViewModel { OfferNumber = o.OfferNumber, OfferDate = o.PublishDate, ServiceProvider = o.User.FirstName + "  " + o.User.LastName, Title = o.Title, Description = o.Description, Period = o.Period, quantity = o.Quantity, Price = o.Price, RegionName = o.Region != null ? o.Region.Name : "  ", OfferState = o.AcceptOffers.FirstOrDefault().OfferState.ToString() });

        }

        public IEnumerable<ModelViews.Reports.OfferViewModel> AllOfferInRegion(int? RegionId)
        {
            var offers = _uow.Offer.GetAll(x => x.FlgStatus == 1 );
            if (RegionId==null)
            {
                return offers.OrderByDescending(x => x.Id).Select(o => new ModelViews.Reports.OfferViewModel { OfferNumber = o.OfferNumber, ServiceProvider = o.User.FirstName + "  " + o.User.LastName, Title = o.Title, Description = o.Description, Period = o.Period, quantity = o.Quantity, Price = o.Price, RegionName = o.Region != null ? o.Region.Name : "  ", OfferState = o.AcceptOffers.FirstOrDefault().OfferState.ToString() });
            }
            offers = offers.OrderByDescending(x => x.Id).Where(x=>x.RegionId== RegionId);

            return offers.Select(o => new ModelViews.Reports.OfferViewModel { OfferNumber = o.OfferNumber, OfferDate = o.PublishDate, ServiceProvider = o.User.FirstName + "  " + o.User.LastName, Title = o.Title, Description = o.Description, Period = o.Period, quantity = o.Quantity, Price = o.Price, RegionName = o.Region != null ? o.Region.Name : "  ", OfferState = o.AcceptOffers.FirstOrDefault().OfferState.ToString() });
        }

        public IEnumerable<ModelViews.Reports.OfferViewModel> AllOfferByState(int? StateId)
        {
            var offers = _uow.Offer.GetAll(x => x.FlgStatus == 1);
            if (StateId == 1)
            {
                offers = offers.OrderByDescending(x => x.Id).Where(x => x.AcceptOffers.Any(y =>  y.OfferState == OfferState.Done  ));
            }
            if (StateId == 2)
            {
                offers = offers.OrderByDescending(x => x.Id).Where(x => x.AcceptOffers.Any(y=> y.OfferState == OfferState.Rejected));
            }
           
            return offers.Select(o => new ModelViews.Reports.OfferViewModel { OfferNumber = o.OfferNumber, OfferDate = o.PublishDate, ServiceProvider=o.User.FirstName+ "  "+o.User.LastName,Title = o.Title, Description = o.Description, Period = o.Period, quantity = o.Quantity, Price = o.Price, RegionName = o.Region != null ? o.Region.Name : "  ",OfferState=o.AcceptOffers.FirstOrDefault().OfferState.ToString() });

        }




        public IEnumerable<TruckDTO> TruckByNameWithOutParent(string name)
        {
            var BaseURL = ConfigurationManager.AppSettings["BaseURL"].ToString();
            var trucks = _uow.Truck.GetAll(x => x.FlgStatus == 1 && x.TypeId == 1);
            if (name != null || name == "  ")
            {
                trucks = trucks.Where(y => y.ParanetId!=null   &&y.Name.Contains(name) || y.NameArb.Contains(name) || y.Name.StartsWith(name));
            }

            return trucks.Where(x=>x.ParanetId!=null).Select(t => new TruckDTO { Id = t.Id, TruckNameArb = t.NameArb, TruckNameEng = t.Name, TruckParentNameArb = t.Paranet != null ? t.Paranet.NameArb : "  ", TruckParentNameEng = t.Paranet != null ? t.Paranet.Name : "  ", ImagePath = t.TruckImagePath != null ? BaseURL + t.TruckImagePath : null, ParentImagePath = t.Paranet != null && t.Paranet.TruckImagePath != null ? BaseURL + t.Paranet.TruckImagePath : null });
        }
        public IEnumerable<TruckDTO> EquipmentByNameOutParent(string name)
        {
            var BaseURL = ConfigurationManager.AppSettings["BaseURL"].ToString();
            var trucks = _uow.Truck.GetAll(x => x.FlgStatus == 1 && x.TypeId == 2);
            if (name != null || name == "  ")
            {
                trucks = trucks.Where(y => y.ParanetId !=null &&y.Name.Contains(name) || y.NameArb.Contains(name) || y.Name.StartsWith(name));
            }

            return trucks.Where(x=>x.ParanetId !=null).Select(t => new TruckDTO { Id = t.Id, TruckNameArb = t.NameArb, TruckNameEng = t.Name, TruckParentNameArb = t.Paranet != null ? t.Paranet.NameArb : "  ", TruckParentNameEng = t.Paranet != null ? t.Paranet.Name : "  ", ImagePath = t.TruckImagePath != null ? BaseURL + t.TruckImagePath : null, ParentImagePath = t.Paranet != null && t.Paranet.TruckImagePath != null ? BaseURL + t.Paranet.TruckImagePath : null });
        }
    }
}
