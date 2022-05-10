using Ejab.BAL.ModelViews;
using Ejab.BAL.ModelViews.Reports;
using Ejab.DAl.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.Services.Reports
{
 public  interface IReportService
    {
        IEnumerable<PropsalesViewModel> PropsalesOFServiceProvider(int? providerId);
        IEnumerable<PropsalesViewModel> PropsalesinDates( DateTime? fromDate, DateTime? toDate);
        IEnumerable<PropsalesViewModel> PropsalesBtState(int state);
        IEnumerable<RequestViewModel> AllRequestsForCustomer(int? CustomerId);
        IEnumerable<RequestViewModel> AllRequestsInIntervale( DateTime? fromDate, DateTime? toDate);
        IEnumerable<RequestViewModel> AllRequestsInRegion(int? RegionId);
        IEnumerable<RequestViewModel> AllRequestsByState(int? StateId);
        IEnumerable<ModelViews.Reports.OfferViewModel> AllOfferForServiceProvider(int? ServiceProviderId);
        IEnumerable<ModelViews.Reports.OfferViewModel> AllOfferInInterval( DateTime? fromDate, DateTime? toDate);
        IEnumerable<ModelViews.Reports.OfferViewModel> AllOfferInRegion(int? RegionId);
        IEnumerable<ModelViews.Reports.OfferViewModel> AllOfferByState(int? StateId);
        IEnumerable<UserDTO> ServiceProvidersByName(string  Name);
        IEnumerable<UserDTO> AllServiceProviders();
        IEnumerable<CustomerDTO> AllCustomer();
        IEnumerable<CustomerDTO> CustomerByName(string Name);
        IEnumerable<ComplaintViewModel> AllComplaints();
        IEnumerable<ComplaintViewModel> ComplaintsByDate(DateTime? fromDate, DateTime? toDate);
        IEnumerable<TruckDTO> AllTrucksUnderParent(int? parentId);
        IEnumerable<TruckDTO> TruckByName(string name);
        IEnumerable<TruckDTO> EquipmentByName(string name);
        IEnumerable<TruckDTO> AllEquipmentUnderParent( int? parentId);
        IEnumerable<NamesDTO> SearchProviderName();
        IEnumerable<TruckDTO> TruckByNameWithOutParent(string name);
        IEnumerable<TruckDTO> EquipmentByNameOutParent(string name);
    }
}
