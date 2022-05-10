using Ejab.BAL.ModelViews;
using Ejab.BAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ejab.UI.Controllers
{
    public class ChartController : Controller
    {
        IOfferService _iOfferService;
        IRequestService _iRequestService;
        ICustomerService _iCustomerService;
        ITruckService _iTruckService;
        public ChartController(IOfferService IOfferService, IRequestService IRequestService, ICustomerService ICustomerService, ITruckService ITruckService)
        {
            _iOfferService = IOfferService;
            _iRequestService = IRequestService;
            _iCustomerService = ICustomerService;
            _iTruckService = ITruckService;

        }
        // GET: Chart
        public ActionResult Index()
        {
            if (Request.Cookies["language"] == null)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ar-EG");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ar-EG");
            }
            chartViewModel model = new chartViewModel();
            var requestCount = _iRequestService.AllRequests(null).ToList().Count();
            model.RequestsCounts = requestCount;
            var offerCount = _iOfferService.AllOffers(null).ToList().Count();
            model.OfferCounts  = offerCount;
           var trucksCount = _iTruckService.TrucksCount();
            model.Trucks  = _iTruckService.GetallTrucks().Where(x=>x.truckType.TypeId==1).ToList().Count;
            model.Equipment = _iTruckService.GetallTrucks().Where(x => x.truckType.TypeId  == 2).ToList().Count;
            var serviceProviderCount = _iCustomerService .AllServiceProviders(null).ToList().Count();
            model.ServiceProviders  = serviceProviderCount;
            var customerCount = _iCustomerService.Allrequesters(null).ToList().Count();
            model.Customers = customerCount;

            var JanuaryProvider = _iCustomerService.ProviderConnts(1);
            model.ServiceProvidersJanuary = JanuaryProvider;
            var FebruaryProvider = _iCustomerService.ProviderConnts(2);
            model.ServiceProvidersFebruary  = FebruaryProvider;
            var MarchProvider = _iCustomerService.ProviderConnts(3);
            model.ServiceProvidersMarch  = MarchProvider;
            var AprilProvider = _iCustomerService.ProviderConnts(4);
            model.ServiceProvidersApril  = AprilProvider;
            var MayProvider = _iCustomerService.ProviderConnts(5);
            model.ServiceProvidersMay  = MayProvider;
            var JuneProvider = _iCustomerService.ProviderConnts(6);
            model.ServiceProvidersJune  = JuneProvider;
            var JulyProvider = _iCustomerService.ProviderConnts( 7);
            model.ServiceProvidersJuly  = JulyProvider;
            var AugustProvider = _iCustomerService.ProviderConnts(8);
            model.ServiceProvidersAugust  = AugustProvider;
            var SeptemberProvider = _iCustomerService.ProviderConnts(9);
            model.ServiceProvidersSeptember  = SeptemberProvider;
            var OctoberProvider = _iCustomerService.ProviderConnts( 10);
            model.ServiceProvidersOctober  = OctoberProvider;
            var NovemeberProvider = _iCustomerService.ProviderConnts( 11);
            model.ServiceProvidersNovemeber  = NovemeberProvider;
            var DecemberProvider = _iCustomerService.ProviderConnts( 12);
            model.ServiceProvidersDecember  = DecemberProvider;
            //////
            var JanuaryCustomer = _iCustomerService.CustomerConnts(1);
            model.CustomerJanuary = JanuaryCustomer;
            var FebruaryCustomer = _iCustomerService.CustomerConnts( 2);
            model.CustomerFebruary = FebruaryCustomer;
            var MarchCustomer = _iCustomerService.CustomerConnts(3);
            model.CustomerMarch = MarchCustomer;
            var AprilCustomer = _iCustomerService.CustomerConnts( 4);
            model.CustomerApril = AprilCustomer;
            var MayCustomer = _iCustomerService.CustomerConnts( 5);
            model.CustomerMay = MayCustomer;
            var JuneCustomer = _iCustomerService.CustomerConnts( 6);
            model.CustomerJune = JuneCustomer;
            var JulyCustomer = _iCustomerService.CustomerConnts( 7);
            model.CustomerJuly = JulyCustomer;
            var AugustCustomer = _iCustomerService.CustomerConnts( 8);
            model.CustomerAugust = AugustCustomer;
            var SeptemberCustomer = _iCustomerService.CustomerConnts( 9);
            model.CustomerSeptember = SeptemberCustomer;
            var OctoberCustomer = _iCustomerService.CustomerConnts( 10);
            model.CustomerOctober = OctoberCustomer;
            var NovemeberCustomer = _iCustomerService.CustomerConnts( 11);
            model.CustomerNovemeber = NovemeberCustomer;
            var DecemberCustomer = _iCustomerService.CustomerConnts( 12);
            model.CustomerDecember = DecemberCustomer;
            /////////////////
            var JanuaryTrucks= _iTruckService .trucks(1, 1);
            model.trucksJanuary = JanuaryTrucks;
            var FebruaryTrucks = _iTruckService.trucks(1, 2);
            model.trucksFebruary = FebruaryTrucks;
            var MarchTrucks = _iTruckService.trucks(1, 3);
            model.trucksMarch = MarchTrucks;
            var AprilTrucks = _iTruckService.trucks(1, 4);
            model.trucksApril = AprilTrucks;
            var MayTrucks = _iTruckService.trucks(1, 5);
            model.trucksMay = MayTrucks;
            var JuneTrucks = _iTruckService.trucks(1, 6);
            model.trucksJune = JuneTrucks;
            var JulyTrucks = _iTruckService.trucks(1, 7);
            model.trucksJuly = JulyTrucks;
            var AugustTrucks = _iTruckService.trucks(1, 8);
            model.trucksAugust = AugustTrucks;
            var SeptemberTrucks = _iTruckService.trucks(1, 9);
            model.trucksSeptember = SeptemberTrucks;
            var OctoberTrucks =_iTruckService .trucks(1, 10);
            model.trucksOctober = OctoberTrucks;
            var NovemeberTrucks = _iTruckService.trucks(1, 11);
            model.trucksNovemeber = NovemeberTrucks;
            var DecemberTrucks = _iTruckService.trucks(1, 12);
            model.trucksDecember = DecemberTrucks;
            /////////////////////////////////
            var JanuaryEquipment = _iTruckService.trucks(2, 1);
            model.equipmentJanuary = JanuaryEquipment;
            var FebruaryEquipment = _iTruckService.trucks(2, 2);
            model.equipmentFebruary = FebruaryEquipment;
            var MarchEquipment = _iTruckService.trucks(2, 3);
            model.equipmentMarch = MarchEquipment;
            var AprilEquipment = _iTruckService.trucks(2, 4);
            model.equipmentApril = AprilEquipment;
            var MayEquipment = _iTruckService.trucks(2, 5);
            model.equipmentMay = MayEquipment;
            var JuneEquipment = _iTruckService.trucks(2, 6);
            model.equipmentJune = JuneEquipment;
            var JulyEquipment = _iTruckService.trucks(2, 7);
            model.equipmentJuly = JulyEquipment;
            var AugustEquipment = _iTruckService.trucks(2, 8);
            model.equipmentAugust = AugustEquipment;
            var SeptemberEquipment = _iTruckService.trucks(2, 9);
            model.equipmentSeptember = SeptemberEquipment;
            var OctoberEquipment = _iTruckService.trucks(2, 10);
            model.equipmentOctober = OctoberEquipment;
            var NovemeberEquipment = _iTruckService.trucks(2, 11);
            model.equipmentNovemeber = NovemeberEquipment;
            var DecemberEquipment = _iTruckService.trucks(2, 12);
            model.equipmentDecember = DecemberEquipment;

            return View(model);
        }
    }
}