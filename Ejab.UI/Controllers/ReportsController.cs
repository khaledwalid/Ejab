using Ejab.BAL.ModelViews.Reports;
using Ejab.BAL.Services;
using Ejab.BAL.Services.Reports;
using PagedList;
using Rotativa.MVC;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Globalization;
using Ejab.UI.Helpers;
using System.Web;
using Ejab.UI.Models;

namespace Ejab.UI.Controllers
{
    public class ReportsController : Controller
    {
        IReportService _iService;
        ICustomerService _IcustomerService;
        ITruckService _truckService;
        IRegionService _regionService;
        public ReportsController(IReportService IService, ICustomerService Icustomerservice, ITruckService truckService, IRegionService regionService)
        {
            _iService = IService;
            _IcustomerService = Icustomerservice;
            _truckService = truckService;
            _regionService = regionService;
        }

        // GET: Reports
        #region Propsal
        [HttpGet]
        public ActionResult Propsales(int? providerId, int? page)
        {
            TempData["providerId"] = providerId;
            TempData["providerId1"] = providerId;
            var allnames = _iService.SearchProviderName();
            ViewBag.providers = new SelectList(allnames, "Id", "FullName");

            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
            var propsoles = _iService.PropsalesOFServiceProvider(providerId).ToList().ToPagedList(page ?? 1, pageSize); ;

            return View(propsoles);
        }


        [HttpGet]
        public ActionResult PropsalesInDate(string fromDate, string toDate, int? page)
        {
            TempData["fromDate"] = fromDate;
            TempData["toDate"] = toDate;
            TempData["propsalfromDate"] = fromDate;
            TempData["propsaltoDate"] = toDate;
            DateTime? fromDateDT = null;
            DateTime? toDateDT = null;
            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
            if (fromDate != null && toDate != null && fromDate.Length > 9 && toDate.Length > 9)
            {
                fromDateDT = DateTime.ParseExact(fromDate.ToString(), "dd/MM/yyyy", null);
                toDateDT = DateTime.ParseExact(toDate.ToString(), "dd/MM/yyyy", null);

            }
            if (fromDate == null && toDate == null)
            {
                fromDateDT = null;
                toDateDT = null;
            }
            var propsoles = _iService.PropsalesinDates(fromDateDT, toDateDT).ToList().ToPagedList(page ?? 1, pageSize);

            return View(propsoles);
        }

        public void ExportPropsales()
        {
            int? providerId = null;
            if (TempData["providerId"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {

                    RedirectToAction("Propsales", new { providerId = 0 });

                }
                if (query != "")
                {
                    if (query.Length > 12)
                    {
                        string x = query.Substring(12);
                        providerId = int.Parse(x);
                    }
                    if (providerId == 0)
                    {
                        RedirectToAction("Propsales", new { providerId = 0 });
                    }

                }
            }
            if (TempData["providerId"] != null && int.Parse(TempData["providerId"].ToString()) != 0)
            {
                providerId = int.Parse(TempData["providerId"].ToString());
            }
            var propsoles = _iService.PropsalesOFServiceProvider(providerId).ToList();
            ExportHTML(propsoles);
            //GridView gv = new GridView();

            //gv.DataSource = propsoles;
            //gv.DataBind();
            //var lang = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();

            //if (lang == "ar-EG")
            //{

            //    gv.HeaderRow.Cells[0].Text = "رقم الطلب";
            //    gv.HeaderRow.Cells[1].Text = "مقدم الخدمة";
            //    gv.HeaderRow.Cells[2].Text = "التقييم";
            //    gv.HeaderRow.Cells[7].Text = "عنوان العرض";
            //    gv.HeaderRow.Cells[3].Text = "التاريخ";
            //    gv.HeaderRow.Cells[4].Text = "السعر";
            //    gv.HeaderRow.Cells[5].Text = "طالب الخدمة";
            //    gv.HeaderRow.Cells[6].Text = "حالة عرض السعر";
            //}
            //if (lang == "en-US")
            //{
            //    gv.HeaderRow.Cells[0].Text = "Request Number";
            //    gv.HeaderRow.Cells[1].Text = "Service Provider";
            //    gv.HeaderRow.Cells[2].Text = "Rating";
            //    gv.HeaderRow.Cells[7].Text = "Request Title";
            //    gv.HeaderRow.Cells[3].Text = "Date";
            //    gv.HeaderRow.Cells[4].Text = "Price";
            //    gv.HeaderRow.Cells[5].Text = "Customer";
            //    gv.HeaderRow.Cells[6].Text = "Propsal State";
            //}

            //gv.ShowHeader = true;
            //gv.ShowFooter = true;
            //Response.ClearContent();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment; filename=Propsales.xls");
            //Response.ContentType = "application/ms-excel";
            //Response.Charset = "";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter htw = new HtmlTextWriter(sw);
            //gv.RenderControl(htw);
            //Response.Output.Write(sw.ToString());
            //Response.Flush();
            //Response.End();


        }
        private void ExportHTML(List<PropsalesViewModel> Propsal)
        {
            StringBuilder bldr = new StringBuilder();
            bldr.AppendLine(@"<table cellspacing='0' rules='all' border='1' style='border - collapse:collapse; '>");
            bldr.AppendLine("<tr>");
            var lang = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();

            if (lang == "ar-EG")
            {
                AddProppsaHeaderCells(bldr, "رقم الطلب", "مقدم الخدمة", "التقييم", "عنوان العرض", "التاريخ", "السعر", "طالب الخدمة", "حالة عرض السعر");
            }
            if (lang == "en-US")
            {
                AddProppsaHeaderCells(bldr, "Request Number", "Service Provider", "Rating", "Request Title", "Date", "Price", "Customer", "Propsal State");
            }

            AddRows(bldr, Propsal);
            bldr.AppendLine("</tr>");

            bldr.AppendLine("</table>");
            Response.AddHeader("content-disposition", "attachment;filename=Propsales.xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new HtmlTextWriter(sw);
            Response.Write(sw.ToString());
            Response.Output.Write(bldr.ToString());
            Response.Write("\t");
            Response.Flush();
            Response.Close();
            Response.End();
        }

        private void AddRows(StringBuilder bldr, List<PropsalesViewModel> Propsales)
        {
            string state = "";

            foreach (var p in Propsales)
            {
                if (p.PropsalStat == "Open")
                {
                    state = @Resources.Global.Open;
                }
                if (p.PropsalStat == "Accepted")
                {
                    state = @Resources.Global.Accepted;
                }
                if (p.PropsalStat == "Rejected")
                {
                    state = @Resources.Global.Rejected;
                }
                if (p.PropsalStat == "Closed")
                {
                    state = @Resources.Global.Closed;
                }
                if (p.PropsalStat == "Cancelled")
                {
                    state = @Resources.Global.Cancelled;
                }
                if (p.PropsalStat == "Expired")
                {
                    state = @Resources.Global.Expired;
                }

                bldr.AppendLine("<tr>");
                bldr.AppendLine($"<td>{p.RequestNumber}</td><td>{p.ServicrProvider}</td><td>{p.Rating}</td><td>{p.Request.Title}</td><td>{p.Date.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)}</td><td>{p.Price}</td><td>{p.CustomerName}</td><td>" + state + "</td>");

                bldr.AppendLine("</tr>");
            }
        }

        private static void AddProppsaHeaderCells(StringBuilder bldr, params string[] headers)
        {
            foreach (var header in headers)
            {
                bldr.AppendLine($"<td>{header}</td>");
            }
        }
        public void ExportPropsalesInDate()
        {
            DateTime? fromDate = null;
            DateTime? toDate = null;

            if (TempData["fromDate"] == null || TempData["toDate"] == null)
            {
                RedirectToAction("PropsalesInDate", new { fromDate = "", toDate = "" });
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    ViewBag.Error = "error";
                    RedirectToAction("PropsalesInDate", new { fromDate = "", toDate = "" });

                }
                if (query != "")
                {
                    string[] words = query.Split('&');
                    if (words.Contains("fromDate") && words[0].Length > 10)
                    {
                        string f = words[0].Substring(10);
                        string t = words[1].Substring(7);
                        fromDate = DateTime.ParseExact(f, "dd/MM/yyyy", null).Date;
                        toDate = DateTime.ParseExact(t, "dd/MM/yyyy", null).Date;

                    }

                    if (fromDate == null && toDate == null)
                    {
                        RedirectToAction("PropsalesInDate", new { fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date });
                    }

                }
            }



            if (TempData["fromDate"] != null && TempData["fromDate"].ToString().Length > 8)
            {

                fromDate = DateTime.ParseExact(TempData["fromDate"].ToString(), "dd/MM/yyyy", null);
            }
            if (TempData["toDate"] != null && TempData["toDate"].ToString().Length > 6)
            {

                toDate = DateTime.ParseExact(TempData["toDate"].ToString(), "dd/MM/yyyy", null);
            }

            var propsoles = _iService.PropsalesinDates(fromDate, toDate).ToList();
            ExportHTML(propsoles);


        }
        public ActionResult DownloadPropsalesPDF()
        {
            int? providerId = null;
            if (TempData["providerId1"] == null)
            {
                RedirectToAction("Propsales", new { providerId = 0 });
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    ViewBag.Error = "error";
                    RedirectToAction("Propsales", new { providerId = 0 });

                }

                if (query != "")
                {
                    string x = (query.Length > 12) ? query.Substring(12) : "0";
                    int tmp = 0;
                    providerId = (int.TryParse(x, out tmp)) ? tmp : (Nullable<int>)null;
                    if (providerId == 0)
                    {
                        RedirectToAction("Propsales", new { providerId = 0 });
                    }
                }

            }
            if (TempData["providerId1"] != null && int.Parse(TempData["providerId1"].ToString()) != 0)
            {
                providerId = int.Parse(TempData["providerId1"].ToString());
            }
            var root = Server.MapPath("~/Reports/");
            var pdfname = String.Format("{0}.pdf", Guid.NewGuid().ToString());
            var path = Path.Combine(root, pdfname);
            path = Path.GetFullPath(path);
            var propsoles = _iService.PropsalesOFServiceProvider(providerId).ToList();
            return new Rotativa.MVC.ViewAsPdf("GeneratePDF", propsoles)
            {
                FileName = "Propsales.pdf",
                SaveOnServerPath = path,
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageSize = Rotativa.Core.Options.Size.A4,
                    PageOrientation = Rotativa.Core.Options.Orientation.Landscape,
                    CustomSwitches =
                    "--footer-center \"Name: " + "Propsales" + "  Date: " +
                    DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" + " --header-spacing 20 -T 30mm" +
                    " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"",
                    PageMargins = new Rotativa.Core.Options.Margins { Left = 1, Right = 1 },
                    MinimumFontSize = 12,
                    //PageMargins = new Margins(3, 3, 3, 3),
                    IsGrayScale = true,
                    IsJavaScriptDisabled = true,
                    IsBackgroundDisabled = true,



                }





            };
        }
        public ActionResult DownloadPropsalesInDatePDF()
        {
            DateTime? fromDate = null;
            DateTime? toDate = null;

            if (TempData["propsalfromDate"] == null || TempData["propsaltoDate"] == null)
            {
                string query = Request.UrlReferrer.Query;

                if (query == "")
                {
                    RedirectToAction("Propsales", new { providerId = 0 });
                }
                if (query != "")
                {
                    string[] words = query.Split('&');
                    if (words.Contains("fromDate") && words[0].Length > 10)
                    {
                        string f = words[0].Substring(10);
                        string t = words[1].Substring(7);
                        fromDate = DateTime.ParseExact(f, "dd/MM/yyyy", null).Date;
                        toDate = DateTime.ParseExact(t, "dd/MM/yyyy", null).Date;

                    }

                    if (fromDate == null && toDate == null)
                    {
                        RedirectToAction("PropsalesInDate", new { fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date });
                    }

                }
            }
            if (TempData["propsalfromDate"] != null && TempData["propsalfromDate"].ToString().Length > 8)
            {

                fromDate = DateTime.ParseExact(TempData["propsalfromDate"].ToString(), "dd/MM/yyyy", null);
            }

            if (TempData["propsaltoDate"] != null && TempData["propsaltoDate"].ToString().Length > 6)
            {
                toDate = DateTime.ParseExact(TempData["propsaltoDate"].ToString(), "dd/MM/yyyy", null);

            }
            var root = Server.MapPath("~/Reports/");
            var pdfname = String.Format("{0}.pdf", Guid.NewGuid().ToString());
            var path = Path.Combine(root, pdfname);
            var propsoles = _iService.PropsalesinDates(fromDate, toDate).ToList();
            return new Rotativa.MVC.ViewAsPdf("GeneratePDF", propsoles)
            {
                FileName = "Propsales.pdf",
                SaveOnServerPath = path,
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageSize = Rotativa.Core.Options.Size.A4,
                    PageOrientation = Rotativa.Core.Options.Orientation.Landscape,
                    CustomSwitches =
                    "--footer-center \"Name: " + "Propsales" + "  Date: " +
                    DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" + " --header-spacing 20 -T 30mm" +
                    " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"",
                    PageMargins = new Rotativa.Core.Options.Margins { Left = 1, Right = 1 },
                    MinimumFontSize = 12,
                    //PageMargins = new Margins(3, 3, 3, 3),
                    IsGrayScale = true,
                    IsJavaScriptDisabled = true,
                    IsBackgroundDisabled = true,
                }
            };

        }


        [HttpGet]
        public ActionResult PropsalesByState(int? state, int? page)
        {
            if (state != null && state != 0)
            {
                TempData["state"] = state;
                TempData["state1"] = state;
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
            var propsoles = _iService.PropsalesBtState(state.HasValue ? state.Value : 0).ToList().ToPagedList(page ?? 1, pageSize);
            if (propsoles.Count() == 0)
            {
                ViewBag.disablebuttons = "no data";
            }
            return View(propsoles);
        }


        public void ExportPropsalesByState()
        {
            int state = 0;
            if (TempData["state"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("PropsalesByState", new { state = 1 });
                }
                if (query != "")
                {
                    if (query.Length > 5)
                    {
                        string x = query.Substring(7);
                        state = int.Parse(x);
                    }

                    if (state == 0)
                    {
                        RedirectToAction("PropsalesByState", new { state = 1 });
                    }
                }

            }
            if (TempData["state"] != null && int.Parse(TempData["state"].ToString()) != 0)
            {
                state = int.Parse(TempData["state"].ToString());
            }
            var propsoles = _iService.PropsalesBtState(state).ToList();
            ExportHTML(propsoles);


        }

        public ActionResult DownloadPropsalesByStatePDF()
        {
            int state = 0;
            if (TempData["state1"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("PropsalesByState", new { state = 1 });
                }
                if (query != "")
                {
                    if (query.Length > 7)
                    {
                        string x = query.Substring(7);
                        state = int.Parse(x);
                    }

                    if (state == 0)
                    {
                        RedirectToAction("PropsalesByState", new { state = 1 });
                    }
                }

            }
            if (TempData["state1"] != null && int.Parse(TempData["state1"].ToString()) != 0)
            {
                state = int.Parse(TempData["state1"].ToString());
            }
            var root = Server.MapPath("~/Reports/");
            var pdfname = String.Format("{0}.pdf", Guid.NewGuid().ToString());
            var path = Path.Combine(root, pdfname);
            var propsoles = _iService.PropsalesBtState(state).ToList();
            return new Rotativa.MVC.ViewAsPdf("GeneratePDF", propsoles)
            {
                FileName = "PropsalesByState.pdf",
                SaveOnServerPath = path,

                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageSize = Rotativa.Core.Options.Size.A4,
                    PageOrientation = Rotativa.Core.Options.Orientation.Landscape,
                    CustomSwitches =
                    "--footer-center \"Name: " + "Propsales" + "  Date: " +
                    DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" +
                    " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"",
                    PageMargins = new Rotativa.Core.Options.Margins { Left = 1, Right = 1 },
                    MinimumFontSize = 12,
                    //PageMargins = new Margins(3, 3, 3, 3),
                    IsGrayScale = true,
                    IsJavaScriptDisabled = true,
                    IsBackgroundDisabled = true,
                }
            };
        }
        #endregion
        #region Request
        [HttpGet]
        public ActionResult Requests(int? CustomerId, int? page)
        {
            TempData["ReqCustomerId"] = CustomerId;
            TempData["ReqCustomerId1"] = CustomerId;

            var allCustomers = _IcustomerService.Allrequesters(null);
            ViewBag.AllUsers = new SelectList(allCustomers, "Id", "FullName");
            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
            var requests = _iService.AllRequestsForCustomer(CustomerId).ToList().ToPagedList(page ?? 1, pageSize);

            return View(requests);
        }
        [HttpGet]
        public ActionResult RequestsInInterval(string fromDate, string toDate, int? page)
        {


            if (fromDate != null)
            {
                TempData["RequestfromDate"] = fromDate;
                TempData["requestfromDate1"] = fromDate;
            }
            if (toDate != null)
            {
                TempData["RequesttoDate"] = toDate;
                TempData["RequesttoDate1"] = toDate;
            }
            DateTime? fromDateDT = null;
            DateTime? toDateDT = null;
            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
            if (fromDate != null && toDate != null && fromDate.Length > 9 && toDate.Length > 9)
            {
                fromDateDT = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                toDateDT = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);

            }
            if (fromDate == "" && toDate == "")
            {
                fromDateDT = null;
                toDateDT = null;
            }
            var requests = _iService.AllRequestsInIntervale(fromDateDT, toDateDT).ToList().ToPagedList(page ?? 1, pageSize);
            if (requests.Count() == 0)
            {
                ViewBag.disablebuttons = "no data";
            }
            return View(requests);
        }
        [HttpGet]
        public ActionResult RequestsInRegion(int? RegionId, int? page)
        {
            TempData["RegionId"] = RegionId;
            TempData["RegionId1"] = RegionId;

            var allRegions = _regionService.Regions(null);
            ViewBag.AllUsers = new SelectList(allRegions, "Id", "Name");
            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
            var requests = _iService.AllRequestsInRegion(RegionId).ToList().ToPagedList(page ?? 1, pageSize);

            return View(requests);
        }
        [HttpGet]
        public ActionResult RequestsByStateId(int? StateId, int? page)
        {
            TempData["StateId"] = StateId;
            TempData["StateId1"] = StateId;
            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
            var requests = _iService.AllRequestsByState(StateId).ToList().ToPagedList(page ?? 1, pageSize);
            if (requests.Count() == 0)
            {
                ViewBag.disablebuttons = "no data";
            }
            return View(requests);
        }
        [HttpGet]
        public void ExportRequestData()
        {

            int? customerId = null;
            if (TempData["ReqCustomerId"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query != "")
                {

                    RedirectToAction("Requests", new { CustomerId = customerId });

                }
                if (query == "")
                {
                    RedirectToAction("Requests", new { CustomerId = 0 });
                }
                if (query != "")
                {
                    if (query.Length > 12)
                    {
                        string x = query.Substring(12);
                        customerId = int.Parse(x);
                    }

                    if (customerId == null)
                    {
                        RedirectToAction("Requests", new { CustomerId = 0 });
                    }
                }

            }
            if (TempData["ReqCustomerId"] != null)
            {
                customerId = int.Parse(TempData["ReqCustomerId"].ToString());
            }

            var requests = _iService.AllRequestsForCustomer(customerId).ToList();
            ExportRequestsHTML(requests);

        }
        [HttpGet]
        public void ExportRequestByStateData()
        {

            int? stateId = null;
            if (TempData["StateId"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("RequestsByStateId", new { StateId = 1 });

                }
                if (query != "")
                {
                    if (query.Length > 10)
                    {
                        string x = query.Substring(9);
                        stateId = int.Parse(x);
                    }

                    if (stateId == 0)
                    {
                        RedirectToAction("RequestsByStateId", new { StateId = 1 });
                    }
                }

            }
            if (TempData["StateId"] != null)
            {
                stateId = int.Parse(TempData["StateId"].ToString());
            }

            var requests = _iService.AllRequestsByState(stateId).ToList();
            ExportRequestsHTML(requests);

        }
        public ActionResult DownloadRequestStateViewPDF()
        {
            int? stateId = null;
            if (TempData["StateId1"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("RequestsByStateId", new { StateId = 0 });

                }
                if (query != "")
                {
                    if (query.Length > 7)
                    {
                        string x = query.Substring(9);
                        stateId = int.Parse(x);
                    }

                    if (stateId == 0)
                    {
                        RedirectToAction("RequestsByStateId", new { StateId = 0 });
                    }
                }

            }
            if (TempData["StateId1"] != null)
            {
                stateId = int.Parse(TempData["StateId1"].ToString());
            }
            var root = Server.MapPath("~/Reports/");
            var pdfname = String.Format("{0}.pdf", Guid.NewGuid().ToString());
            var path = Path.Combine(root, pdfname);
            var requests = _iService.AllRequestsByState(stateId).ToList();
            return new Rotativa.MVC.ViewAsPdf("DownloadRequestViewPDF", requests)
            {
                FileName = "Requests.pdf",
                SaveOnServerPath = path
                ,
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageSize = Rotativa.Core.Options.Size.A4,
                    PageOrientation = Rotativa.Core.Options.Orientation.Landscape,
                    CustomSwitches =
                    "--footer-center \"Name: " + "Requests" + "  Date: " +
                    DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" + " --header-spacing 20 -T 30mm" +
                    " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"",
                    PageMargins = new Rotativa.Core.Options.Margins { Left = 1, Right = 1 },
                    MinimumFontSize = 12,

                    IsGrayScale = true,
                    IsJavaScriptDisabled = true,
                    IsBackgroundDisabled = true,
                }

            };

        }
        public ActionResult DownloadRequestViewPDF()
        {
            int? customerId = null;
            if (TempData["ReqCustomerId1"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("Requests", new { CustomerId = 0 });
                }
                if (query != "")
                {
                    if (query.Length > 12)
                    {
                        string x = query.Substring(12);
                        customerId = int.Parse(x);
                    }

                    if (customerId == null)
                    {
                        RedirectToAction("Requests", new { CustomerId = 0 });
                    }
                }

            }
            if (TempData["ReqCustomerId1"] != null)
            {
                customerId = int.Parse(TempData["ReqCustomerId1"].ToString());
            }
            var root = Server.MapPath("~/Reports/");
            var pdfname = String.Format("{0}.pdf", Guid.NewGuid().ToString());
            var path = Path.Combine(root, pdfname);
            var requests = _iService.AllRequestsForCustomer(customerId).ToList();
            return new Rotativa.MVC.ViewAsPdf("DownloadRequestViewPDF", requests)
            {
                FileName = "Requests.pdf",
                SaveOnServerPath = path,
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageSize = Rotativa.Core.Options.Size.A4,
                    PageOrientation = Rotativa.Core.Options.Orientation.Landscape,
                    CustomSwitches =
                    "--footer-center \"Name: " + "Requests" + "  Date: " +
                    DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" + " --header-spacing 20 -T 30mm" +
                    " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"",
                    PageMargins = new Rotativa.Core.Options.Margins { Left = 1, Right = 1 },
                    MinimumFontSize = 12,
                    //PageMargins = new Margins(3, 3, 3, 3),
                    IsGrayScale = true,
                    IsJavaScriptDisabled = true,
                    IsBackgroundDisabled = true,
                }

            };

        }
        [HttpGet]
        public void ExportRequestINIntervalData()
        {
            DateTime? fromDate = null;
            DateTime? toDate = null;
            if (TempData["RequestfromDate"] == null || TempData["RequesttoDate"] == null)
            {
                RedirectToAction("RequestsInInterval", new { fromDate = "", toDate = "" });
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("RequestsInInterval", new { fromDate = "", toDate = "" });
                }
                if (query != "" && query.Length > 8)
                {
                    string[] words = query.Split('&');
                    if (words.Contains("fromDate") && words[0].Length > 10)
                    {
                        string f = words[0].Substring(10);
                        string t = words[1].Substring(7);
                        fromDate = DateTime.ParseExact(f, "dd/MM/yyyy", null).Date;
                        toDate = DateTime.ParseExact(t, "dd/MM/yyyy", null).Date;
                    }

                    if (fromDate == null && toDate == null)
                    {
                        RedirectToAction("RequestsInInterval", new { fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date });
                    }

                }

            }
            if (TempData["RequestfromDate"] != null && TempData["RequestfromDate"].ToString().Length > 8)
            {

                fromDate = DateTime.ParseExact(TempData["RequestfromDate"].ToString(), "dd/MM/yyyy", null);
            }

            if (TempData["RequesttoDate"] != null && TempData["RequestfromDate"].ToString().Length > 6)
            {

                toDate = DateTime.ParseExact(TempData["RequestfromDate"].ToString(), "dd/MM/yyyy", null);
            }

            var requests = _iService.AllRequestsInIntervale(fromDate, toDate).ToList();
            ExportRequestsHTML(requests);

        }
        public ActionResult DownloadRequestInIntervalViewPDF()
        {
            DateTime? fromDate = null;
            DateTime? toDate = null;
            if (TempData["RequestfromDate1"] == null || TempData["RequesttoDate1"] == null)
            {
                RedirectToAction("RequestsInInterval", new { fromDate = "", toDate = "" });
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("RequestsInInterval", new { fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date });
                }
                if (query != "" && query.Length > 8)
                {
                    string[] words = query.Split('&');
                    if (words.Contains("fromDate") && words[0].Length > 10)
                    {
                        string f = words[0].Substring(10);
                        string t = words[1].Substring(7);
                        fromDate = DateTime.ParseExact(f, "dd/MM/yyyy", null).Date;
                        toDate = DateTime.ParseExact(t, "dd/MM/yyyy", null).Date;
                    }
                    if (fromDate == null && toDate == null)
                    {
                        RedirectToAction("RequestsInInterval", new { fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date });
                    }
                }

            }
            if (TempData["RequestfromDate1"] != null && TempData["RequestfromDate1"].ToString().Length > 8)
            {

                fromDate = DateTime.ParseExact(TempData["RequestfromDate1"].ToString(), "dd/MM/yyyy", null);
            }

            if (TempData["RequesttoDate1"] != null && TempData["RequesttoDate1"].ToString().Length > 6)
            {
                toDate = DateTime.ParseExact(TempData["RequesttoDate1"].ToString(), "dd/MM/yyyy", null);
            }
            string cusomtSwitches = string.Format("--print-media-type --allow {0} --footer-html {0} --footer-spacing -10",
          Url.Action("Footer", "Document", new { area = "" }, "https"));
            var root = Server.MapPath("~/Reports/");
            var pdfname = String.Format("{0}.pdf", Guid.NewGuid().ToString());
            var path = Path.Combine(root, pdfname);
            var requests = _iService.AllRequestsInIntervale(fromDate, toDate).ToList();
            return new Rotativa.MVC.ViewAsPdf("DownloadRequestViewPDF", requests)
            {
                FileName = "Requests.pdf"
                ,
                SaveOnServerPath = path,
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageSize = Rotativa.Core.Options.Size.A4,
                    PageOrientation = Rotativa.Core.Options.Orientation.Landscape,
                    CustomSwitches =
                    "--footer-center \"Name: " + "Requests" + "  Date: " +
                    DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" + " --header-spacing 20 -T 30mm" +
                    " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"",
                    PageMargins = new Rotativa.Core.Options.Margins { Left = 1, Right = 1 },
                    MinimumFontSize = 12,
                    //PageMargins = new Margins(3, 3, 3, 3),
                    IsGrayScale = true,
                    IsJavaScriptDisabled = true,
                    IsBackgroundDisabled = true,
                }

            };

        }

        [HttpGet]
        public void ExportRequestInRegionData()
        {
            int? regionId = null;
            if (TempData["RegionId"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("RequestsInRegion", new { RegionId = 1 });
                }
                if (query != "")
                {
                    if (query.Length > 10)
                    {
                        string x = query.Substring(10);
                        if (x != "")
                        {
                            regionId = int.Parse(x);
                        }


                    }
                    if (regionId == null)
                    {
                        RedirectToAction("RequestsInRegion", new { RegionId = 1 });
                    }

                }

            }
            if (TempData["RegionId"] != null)
            {
                regionId = int.Parse(TempData["RegionId"].ToString());
            }

            var requests = _iService.AllRequestsInRegion(regionId).ToList();
            ExportRequestsHTML(requests);

        }
        public ActionResult DownloadRequestInRegionViewPDF()
        {
            int? regionId = null;
            if (TempData["RegionId1"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("RequestsInRegion", new { RegionId = 1 });
                }
                if (query != "")
                {
                    if (query.Length > 10)
                    {
                        string x = query.Substring(10);
                        if (x != "")
                        {
                            regionId = int.Parse(x);
                        }


                    }
                    if (regionId == null)
                    {
                        RedirectToAction("RequestsInRegion", new { RegionId = 1 });
                    }

                }

            }
            if (TempData["RegionId1"] != null)
            {
                regionId = int.Parse(TempData["RegionId1"].ToString());
            }
            var root = Server.MapPath("~/Reports/");
            var pdfname = String.Format("{0}.pdf", Guid.NewGuid().ToString());
            var path = Path.Combine(root, pdfname);
            var requests = _iService.AllRequestsInRegion(regionId).ToList();
            return new Rotativa.MVC.ViewAsPdf("DownloadRequestViewPDF", requests)
            {
                FileName = "Requests.pdf",
                SaveOnServerPath = path,
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageSize = Rotativa.Core.Options.Size.A4,
                    PageOrientation = Rotativa.Core.Options.Orientation.Landscape,
                    CustomSwitches =
                    "--footer-center \"Name: " + "Requests" + "  Date: " +
                    DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" + " --header-spacing 20 -T 30mm" +
                    " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"",
                    PageMargins = new Rotativa.Core.Options.Margins { Left = 1, Right = 1 },
                    MinimumFontSize = 12,
                    //PageMargins = new Margins(3, 3, 3, 3),
                    IsGrayScale = true,
                    IsJavaScriptDisabled = true,
                    IsBackgroundDisabled = true,
                }

            };

        }

        private void ExportRequestsHTML(List<RequestViewModel> requests)
        {
            StringBuilder bldr = new StringBuilder();
            bldr.AppendLine(@"<table cellspacing='0' rules='all' border='1' style='border - collapse:collapse; '>");
            bldr.AppendLine("<tr>");
            var lang = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();
            if (lang == "ar-EG")
            {
                AddRequestHeaderCells(bldr, "رقم الطلب", "تاريخ الطلب", "طالب الخدمة", "التفاصيل", "اسم المنطقه", "المكان من", "المكان الى", "الكميه", "المده", "معلومات البضاعه", "ملاحظات", "حالة الطلب");
            }
            if (lang == "en-US")
            {
                AddRequestHeaderCells(bldr, "Request Number", "Date", "Customer", "Detailes", "Region", "From", "To", "Quantity", "Period", "Items Info", "Notes", "Status");
            }

            bldr.AppendLine("</tr>");

            bldr.AppendLine("<tr>");
            AddRows(bldr, requests);
            bldr.AppendLine("</tr>");

            bldr.AppendLine("</table>");
            Response.AddHeader("content-disposition", "attachment;filename=Requests.xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new HtmlTextWriter(sw);
            Response.Write(sw.ToString());
            Response.Output.Write(bldr.ToString());
            Response.Write("\t");
            Response.Flush();
            Response.Close();
            Response.End();
        }

        private void AddRows(StringBuilder bldr, List<RequestViewModel> requests)
        {
            string state = "";
            foreach (var request in requests)
            {
                if (request.RequestState == "Open")
                {
                    state = @Resources.Global.Open;
                }
                if (request.RequestState == "Accepted")
                {
                    state = @Resources.Global.Accepted;
                }
                if (request.RequestState == "Rejected")
                {
                    state = @Resources.Global.Rejected;
                }
                if (request.RequestState == "Closed")
                {
                    state = @Resources.Global.Closed;
                }
                if (request.RequestState == "Cancelled")
                {
                    state = @Resources.Global.Cancelled;
                }
                if (request.RequestState == "Expired")
                {
                    state = @Resources.Global.Expired;
                }

                bldr.AppendLine("<tr>");
                bldr.AppendLine($"<td>{request.Id }</td><td>{request.RequestDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)}</td><td>{request.Customer }</td> <td>{request.Description}</td><td>{request.RegionName}</td><td>{request.LocationFrom}</td><td>{request.LocationTo}</td><td>{request.Quantity}</td><td>{request.Period}</td><td>{request.ItemInfo}</td><td>{request.Notes}</td> <td>" + state + "</td>");
                bldr.AppendLine("</tr>");
            }
        }

        private static void AddRequestHeaderCells(StringBuilder bldr, params string[] headers)
        {
            foreach (var header in headers)
            {
                bldr.AppendLine($"<td>{header}</td>");
            }
        }
        #endregion
        #region Offers
        [HttpGet]
        public ActionResult Offers(int? Id, int? page)
        {
            TempData["OfferServiceProviderId"] = Id;
            TempData.Keep("OfferServiceProviderId");
            var allserviceProviders = _IcustomerService.AllServiceProviders(null);
            ViewBag.AllUsers = new SelectList(allserviceProviders, "Id", "FullName");
            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
            var offers = _iService.AllOfferForServiceProvider(Id).ToList().ToPagedList(page ?? 1, pageSize);
            if (offers.Count() == 0)
            {
                ViewBag.disablebuttons = "no data";
            }
            return View(offers);
        }
        [HttpGet]
        public ActionResult OffersInInterval(string fromDate, string toDate, int? page)
        {

            TempData["OfferfromDate"] = fromDate;
            TempData.Keep("OfferfromDate");
            TempData["OffertoDate"] = toDate;
            TempData.Keep("OffertoDate");
            TempData["OfferfromDate1"] = fromDate;
            TempData.Keep("OfferfromDate1");
            TempData["OffertoDate1"] = toDate;
            TempData.Keep("OffertoDate1");
            DateTime? fromDateDT = null;
            DateTime? toDateDT = null;
            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
            if (fromDate != null && toDate != null && fromDate.Length > 9 && toDate.Length > 9)
            {
                fromDateDT = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                toDateDT = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);

            }
            if (fromDate == "" && toDate == "")
            {
                fromDateDT = null;
                toDateDT = null;
            }
            var offers = _iService.AllOfferInInterval(fromDateDT, toDateDT).ToList().ToPagedList(page ?? 1, pageSize);

            return View(offers);
        }
        [HttpGet]
        public void ExportOfferInIntervalData(int? page)
        {
            DateTime? fromDate = null;
            DateTime? toDate = null;
            if (TempData["OfferfromDate"] == null || TempData["OffertoDate"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("OffersInInterval", new { fromDate = DateTime.Now.Date, DateTime = DateTime.Now.Date });
                }
                if (query != "")
                {
                    string[] words = query.Split('&');
                    if (words.Contains("fromDate") && words[0].Length > 10)
                    {
                        string f = words[0].Substring(10);
                        string t = words[1].Substring(7);
                        fromDate = DateTime.ParseExact(f, "dd/MM/yyyy", null).Date;
                        toDate = DateTime.ParseExact(t, "dd/MM/yyyy", null).Date;
                    }

                    if (fromDate == null && toDate == null)
                    {
                        RedirectToAction("OffersInInterval", new { fromDate = DateTime.Now.Date, DateTime = DateTime.Now.Date });
                    }

                }

            }
            if (TempData["OfferfromDate"] != null && TempData["OfferfromDate"].ToString().Length > 8)
            {

                fromDate = DateTime.ParseExact(TempData["OfferfromDate"].ToString(), "dd/MM/yyyy", null);
            }

            if (TempData["OffertoDate"] != null && TempData["OffertoDate"].ToString().Length > 6)
            {
                toDate = DateTime.ParseExact(TempData["OffertoDate"].ToString(), "dd/MM/yyyy", null);

            }

            var offers = _iService.AllOfferInInterval(fromDate, toDate).ToList();
            ExportOfferHTML(offers);
        }
        public ActionResult DownloadOfferINIntervalViewPDF(int? page)
        {
            DateTime? fromDate = null;
            DateTime? toDate = null;
            if (TempData["OfferfromDate1"] == null || TempData["OffertoDate1"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("OffersInInterval", new { fromDate = DateTime.Now.Date, DateTime = DateTime.Now.Date });
                }
                if (query != "")
                {
                    string[] words = query.Split('&');
                    if (words.Contains("fromDate") && words[0].Length > 10)
                    {
                        string f = words[0].Substring(10);
                        string t = words[1].Substring(7);
                        fromDate = DateTime.ParseExact(f, "dd/MM/yyyy", null).Date;
                        toDate = DateTime.ParseExact(t, "dd/MM/yyyy", null).Date;
                    }

                    if (fromDate == null && toDate == null)
                    {
                        RedirectToAction("OffersInInterval", new { fromDate = DateTime.Now.Date, DateTime = DateTime.Now.Date });
                    }

                }

            }
            if (TempData["OfferfromDate1"] != null && TempData["OfferfromDate1"].ToString().Length > 8)
            {
                fromDate = Convert.ToDateTime(TempData["OfferfromDate1"]);
            }

            if (TempData["OffertoDate1"] != null && TempData["OffertoDate1"].ToString().Length > 6)
            {
                toDate = Convert.ToDateTime(TempData["OffertoDate1"]);
            }
            var root = Server.MapPath("~/Reports/");
            var pdfname = String.Format("{0}.pdf", Guid.NewGuid().ToString());
            var path = Path.Combine(root, pdfname);
            var offers = _iService.AllOfferInInterval(fromDate, toDate).ToList();
            return new Rotativa.MVC.ViewAsPdf("DownloadOfferViewPDF", offers)
            {
                FileName = "Offers.pdf"
                //,
                //SaveOnServerPath = path,
                //RotativaOptions = new Rotativa.Core.DriverOptions()
                //{
                //    PageSize = Rotativa.Core.Options.Size.A4,
                //    PageOrientation = Rotativa.Core.Options.Orientation.Landscape,
                //    CustomSwitches =
                //    "--footer-center \"Name: " + "Offers" + "  Date: " +
                //    DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" + " --header-spacing 20 -T 30mm" +
                //    " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"",
                //    PageMargins = new Rotativa.Core.Options.Margins { Left = 1, Right = 1 },
                //    MinimumFontSize = 12,
                //    //PageMargins = new Margins(3, 3, 3, 3),
                //    IsGrayScale = true,
                //    IsJavaScriptDisabled = true,
                //    IsBackgroundDisabled = true,
                //}

            };

        }
        [HttpGet]
        public ActionResult OffersInRegion(int? regionId, int? page)
        {
            TempData["OfferRegionId"] = regionId;
            TempData.Keep("OfferRegionId");
            var allRegions = _regionService.Regions(null);
            var lang = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();

            if (lang == "ar-EG")
            {
                ViewBag.AllUsers = new SelectList(allRegions, "Id", "Name");
            }
            if (lang == "en-US")
            {
                ViewBag.AllUsers = new SelectList(allRegions, "Id", "Name");
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
            var offers = _iService.AllOfferInRegion(regionId).ToList().ToPagedList(page ?? 1, pageSize);
            return View(offers);
        }
        [HttpGet]
        public void ExportOfferInRegionData()
        {
            int? regionId = null;
            if (TempData["OfferRegionId"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("OffersInRegion", new { regionId = 1 });
                }
                if (query != "")
                {
                    if (query.Length > 10)
                    {
                        string x = query.Substring(10);
                        if (x != "")
                        {
                            regionId = int.Parse(x);

                        }


                    }
                    if (regionId == null)
                    {
                        RedirectToAction("OffersInRegion", new { regionId = 1 });
                    }

                }
            }
            if (TempData["OfferRegionId"] != null)
            {
                regionId = int.Parse(TempData["OfferRegionId"].ToString());
            }

            var offers = _iService.AllOfferInRegion(regionId).ToList().ToList();
            ExportOfferHTML(offers);

        }
        public ActionResult DownloadOfferInRegionViewPDF()
        {
            int? regionId = null;
            if (TempData["RegionId"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("OffersInRegion", new { regionId = 1 });
                }
                if (query != "")
                {
                    if (query.Length > 10)
                    {
                        string x = query.Substring(10);
                        if (x != "")
                        {
                            regionId = int.Parse(x);
                        }


                    }
                    if (regionId == null)
                    {
                        RedirectToAction("OffersInRegion", new { regionId = 1 });
                    }

                }

            }
            if (TempData["RegionId"] != null)
            {
                regionId = int.Parse(TempData["RegionId"].ToString());
            }
            var root = Server.MapPath("~/Reports/");
            var pdfname = String.Format("{0}.pdf", Guid.NewGuid().ToString());
            var path = Path.Combine(root, pdfname);
            var offers = _iService.AllOfferInRegion(regionId).ToList();
            return new Rotativa.MVC.ViewAsPdf("DownloadOfferViewPDF", offers)
            {
                FileName = "Offers.pdf",

                SaveOnServerPath = path,
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageSize = Rotativa.Core.Options.Size.A4,
                    PageOrientation = Rotativa.Core.Options.Orientation.Landscape,
                    CustomSwitches =
                    "--footer-center \"Name: " + "Offers" + "  Date: " +
                    DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" + " --header-spacing 20 -T 40mm" +
                    " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"",
                    PageMargins = new Rotativa.Core.Options.Margins { Left = 1, Right = 1 },
                    MinimumFontSize = 12,
                    //PageMargins = new Margins(3, 3, 3, 3),
                    IsGrayScale = true,
                    IsJavaScriptDisabled = true,
                    IsBackgroundDisabled = true,
                }

            };

        }
        [HttpGet]
        public ActionResult OffersByState(int? state, int? page)
        {

            TempData["OfferState"] = state;
            TempData.Keep("OfferState");

            var allserviceProviders = _IcustomerService.AllServiceProviders(null);
            ViewBag.AllUsers = new SelectList(allserviceProviders, "Id", "FullName");
            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
            var offers = _iService.AllOfferByState(state).ToList().ToPagedList(page ?? 1, pageSize);
           return View(offers);
        }
        [HttpGet]
        public void ExportOfferByStateData()
        {

            int? stateId = null;
            if (TempData["OfferState"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("OffersByState", new { state = 1 });
                }
                if (query != "")
                {
                    if (query.Length > 7)
                    {
                        string x = query.Substring(7);
                        stateId = int.Parse(x);

                    }
                    if (stateId == null)
                    {
                        RedirectToAction("OffersByState", new { state = 1 });
                    }
                }

            }
            if (TempData["OfferState"] != null)
            {
                stateId = int.Parse(TempData["OfferState"].ToString());
            }

            var requests = _iService.AllOfferByState(stateId).ToList();
            ExportOfferHTML(requests);
        }
        public ActionResult DownloadOfferStateViewPDF()
        {
            int? stateId = null;
            if (TempData["OfferState"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("OffersByState", new { state = 1 });
                }
                if (query != "")
                {
                    if (query.Length > 7)
                    {
                        string x = query.Substring(7);
                        stateId = int.Parse(x);

                    }
                    if (stateId == null)
                    {
                        RedirectToAction("OffersByState", new { state = 1 });
                    }

                }

            }
            if (TempData["OfferState"] != null)
            {
                stateId = int.Parse(TempData["OfferState"].ToString());
            }
            var root = Server.MapPath("~/Reports/");
            var pdfname = String.Format("{0}.pdf", Guid.NewGuid().ToString());
            var path = Path.Combine(root, pdfname);
            var offers = _iService.AllOfferByState(stateId).ToList().ToList();
            return new Rotativa.MVC.ViewAsPdf("DownloadOfferViewPDF", offers)
            {
                FileName = "Offers.pdf",
                SaveOnServerPath = path,
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageSize = Rotativa.Core.Options.Size.A4,
                    PageOrientation = Rotativa.Core.Options.Orientation.Landscape,
                    CustomSwitches =
                    "--footer-center \"Name: " + "Offers" + "  Date: " +
                    DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" + " --header-spacing 20 -T 40mm" +
                    " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"",
                    PageMargins = new Rotativa.Core.Options.Margins { Left = 1, Right = 1 },
                    MinimumFontSize = 12,
                    //PageMargins = new Margins(3, 3, 3, 3),
                    IsGrayScale = true,
                    IsJavaScriptDisabled = true,
                    IsBackgroundDisabled = true

                }

            };

        }
        [HttpGet]
        public void ExportOfferData(int? page)
        {
            int? id = null;
            if (TempData["OfferServiceProviderId"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("Offers", new { id = 0 });
                }
                if (query != "")
                {
                    if (query.Length > 5)
                    {
                        string x = query.Substring(4);
                        id = int.Parse(x);
                    }
                    if (id == null)
                    {
                        RedirectToAction("Offers", new { id = 0 });
                    }

                }


            }
            if (TempData["OfferServiceProviderId"] != null)
            {
                id = int.Parse(TempData["OfferServiceProviderId"].ToString());
            }

            var offers = _iService.AllOfferForServiceProvider(id).ToList();

            ExportOfferHTML(offers);

        }
        public ActionResult DownloadOfferViewPDF(int? page)
        {
            int? id = null;
            if (TempData["OfferServiceProviderId"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("Offers", new { id = 0 });
                }
                if (query != "")
                {
                    if (query.Length > 5)
                    {
                        string x = query.Substring(4);
                        id = int.Parse(x);
                    }
                    if (id == null)
                    {
                        RedirectToAction("Offers", new { id = 0 });
                    }

                }

            }
            if (TempData["OfferServiceProviderId"] != null)
            {
                id = int.Parse(TempData["OfferServiceProviderId"].ToString());
            }
            var root = Server.MapPath("~/Reports/");
            var pdfname = String.Format("{0}.pdf", Guid.NewGuid().ToString());
            var path = Path.Combine(root, pdfname);
            var offers = _iService.AllOfferForServiceProvider(id).ToList();
            return new Rotativa.MVC.ViewAsPdf("DownloadOfferViewPDF", offers)
            {
                FileName = "Offers.pdf",
                SaveOnServerPath = path,
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageSize = Rotativa.Core.Options.Size.A4,
                    PageOrientation = Rotativa.Core.Options.Orientation.Landscape,
                    CustomSwitches =
                    "--footer-center \"Name: " + "Offers" + "  Date: " +
                    DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" + " --header-spacing 20 -T 40mm" +
                    " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"",
                    PageMargins = new Rotativa.Core.Options.Margins { Left = 1, Right = 1 },
                    MinimumFontSize = 12,
                    //PageMargins = new Margins(3, 3, 3, 3),
                    IsGrayScale = true,
                    IsJavaScriptDisabled = true,
                    IsBackgroundDisabled = true,
                }

            };

        }
        private void ExportOfferHTML(List<OfferViewModel> offers)
        {
            StringBuilder bldr = new StringBuilder();

            bldr.AppendLine(@"<table cellspacing='0' rules='all' border='1' style='border - collapse:collapse; '>");
            bldr.AppendLine("<tr>");
            var lang = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();

            if (lang == "ar-EG")
            {
                AddProppsaHeaderCells(bldr, "رقم الاعلان", "التاريخ", "مقدم الخدمة", "السعر", "عنوان الاعلان", "التفاصيل", "المنطقه", "الكمية", "المده", "حالة الاعلان");
            }
            if (lang == "en-US")
            {
                AddProppsaHeaderCells(bldr, "Offer Number", "Date", "Sevice Provider", "Price", "Title", "Description", "Region Name", "quantity", "Period", "Status");
            }
            bldr.AppendLine("</tr>");

            AddOfferRows(bldr, offers);

            bldr.AppendLine("</table>");


            Response.AddHeader("content-disposition", "attachment;filename=Offers.xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new HtmlTextWriter(sw);
            Response.Write(sw.ToString());
            Response.Output.Write(bldr.ToString());
            Response.Write("\t");
            Response.Flush();
            Response.Close();
            Response.End();

        }

        private void AddOfferRows(StringBuilder bldr, List<OfferViewModel> offers)
        {
            string state = "";
            foreach (var offer in offers)
            {
                if (offer.OfferState == "Accepted")
                {
                    state = @Resources.Global.Accepted;
                }
                if (offer.OfferState == "Done")
                {
                    state = @Resources.Global.Done;
                }
                if (offer.OfferState == "Rejected")
                {
                    state = @Resources.Global.Rejected;
                }
                Response.ContentEncoding = Encoding.UTF8;
                bldr.AppendLine("<tr>");
                bldr.AppendLine($"<td>{offer.OfferNumber}</td><td>{offer.OfferDate.ToShortDateString()}</td><td>{offer.ServiceProvider}</td><td>{offer.Price }</td><td>{offer.Title}</td><td>{offer.Description}</td><td>{offer.RegionName}</td><td>{offer.quantity}</td><td>{offer.Period}</td> <td>" + state + "</td>");
                bldr.AppendLine("</tr>");
            }
        }

        private static void AddOfferHeaderCells(StringBuilder bldr, params string[] headers)
        {
            foreach (var header in headers)
            {
                bldr.AppendLine($"<td>{header}</td>");
            }
        }
        #endregion
        #region ServiceProvider
        [HttpGet]
        public ActionResult ServiceProviders(int? page)
        {
            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
            var serviceProviders = _iService.AllServiceProviders().ToList().ToPagedList(page ?? 1, pageSize);
            if (serviceProviders.Count() == 0)
            {
                ViewBag.disablebuttons = "no data";
            }
            return View(serviceProviders);
        }
        [HttpGet]
        public ActionResult ServiceProvidersById(string Name)
        {
            TempData["tempServiceProviderId"] = Name;
            //var allserviceProviders = _IcustomerService.AllServiceProviders(null);
            //ViewBag.AllUsers = new SelectList(allserviceProviders, "Id", "FullName");
            var serviceProviders = _iService.ServiceProvidersByName(Name).ToList();
            if (serviceProviders.Count() == 0)
            {
                ViewBag.disablebuttons = "no data";
            }
            return View(serviceProviders);
        }
        [HttpGet]
        public void ExportServiceProvidersData()
        {

            var serviceProviders = _iService.AllServiceProviders().ToList();
            ExportServiceProHTML(serviceProviders);
        }

        public ActionResult DownloadServiceProviderViewPDF()
        {
            var root = Server.MapPath("~/Reports/");
            var pdfname = String.Format("{0}.pdf", Guid.NewGuid().ToString());
            var path = Path.Combine(root, pdfname);
            var serviceProviders = _iService.AllServiceProviders().ToList();
            return new Rotativa.MVC.ViewAsPdf("DownloadServiceProviderViewPDF", serviceProviders)
            {
                FileName = "ServiceProviders.pdf",
                SaveOnServerPath = path,
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageSize = Rotativa.Core.Options.Size.A4,
                    PageOrientation = Rotativa.Core.Options.Orientation.Landscape,
                    CustomSwitches =
                    "--footer-center \"Name: " + "Service Provider" + "  Date: " +
                    DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" + " --header-spacing 20 -T 30mm" +
                    " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"",
                    PageMargins = new Rotativa.Core.Options.Margins { Left = 1, Right = 1 },
                    MinimumFontSize = 12,
                    //PageMargins = new Margins(3, 3, 3, 3),
                    IsGrayScale = true,
                    IsJavaScriptDisabled = true,
                    IsBackgroundDisabled = true,
                }

            };

        }

        public void ExportServiceProvidersByIdData()
        {
            string name = null;
            if (TempData["tempServiceProviderId"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("ServiceProvidersById", new { Name = "" });
                }
                if (query != "")
                {
                    if (query.Length > 5)
                    {
                        string x = query.Substring(6);
                        name = x;
                    }
                    if (name == null)
                    {
                        RedirectToAction("CustomersById", new { Name = "" });
                    }

                }

            }
            if (TempData["tempServiceProviderId"] != null)
            {
                name = TempData["tempServiceProviderId"].ToString();
            }

            var serviceProviders = _iService.ServiceProvidersByName(name).ToList();
            ExportServiceProHTML(serviceProviders);
        }
        public ActionResult DownloadServiceProviderByIdViewPDF()
        {
            string name = null;
            if (TempData["tempServiceProviderId"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("ServiceProvidersById", new { Name = "" });
                }
                if (query != "")
                {
                    if (query.Length > 5)
                    {
                        string x = query.Substring(6);
                        name = x;
                    }
                    if (name == null)
                    {
                        RedirectToAction("CustomersById", new { Name = "" });
                    }

                }

            }
            if (TempData["tempServiceProviderId"] != null)
            {
                name = TempData["tempServiceProviderId"].ToString();
            }
            var root = Server.MapPath("~/Reports/");
            var pdfname = String.Format("{0}.pdf", Guid.NewGuid().ToString());
            var path = Path.Combine(root, pdfname);
            var serviceProviders = _iService.ServiceProvidersByName(name).ToList();
            return new Rotativa.MVC.ViewAsPdf("DownloadServiceProviderViewPDF", serviceProviders)
            {
                FileName = "ServiceProviders.pdf",
                SaveOnServerPath = path,
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageSize = Rotativa.Core.Options.Size.A4,
                    PageOrientation = Rotativa.Core.Options.Orientation.Landscape,
                    CustomSwitches =
                    "--footer-center \"Name: " + "Service Provider" + "  Date: " +
                    DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" +
                    " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"",
                    PageMargins = new Rotativa.Core.Options.Margins { Left = 1, Right = 1 },
                    MinimumFontSize = 12,
                    //PageMargins = new Margins(3, 3, 3, 3),
                    IsGrayScale = true,
                    IsJavaScriptDisabled = true,
                    IsBackgroundDisabled = true,
                }

            };

        }
        private void ExportServiceProHTML(List<UserDTO> customers)
        {
            StringBuilder bldr = new StringBuilder();
            bldr.AppendLine(@"<table cellspacing='0' rules='all' border='1' style='border - collapse:collapse; '>");
            bldr.AppendLine("<tr>");
            var lang = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();

            if (lang == "ar-EG")
            {
                //Response.ContentEncoding = Encoding.UTF8;
                AddServiceProviderHeaderCells(bldr, "الاسم الاول", "الاسم الاخير", "جوال", "البريد الالكترونى", "التقييم", "فعال او غير فعال");
            }
            if (lang == "en-US")
            {
                AddServiceProviderHeaderCells(bldr, "First Name", "Last Name", "Mobile", "Email", "Rating", "State");
            }
            AddServiceProviderRows(bldr, customers);
            bldr.AppendLine("</tr>");

            bldr.AppendLine("</table>");


            Response.AddHeader("content-disposition", "attachment;filename=ServiceProviders.xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new HtmlTextWriter(sw);
            Response.Write(sw.ToString());
            Response.Output.Write(bldr.ToString());
            Response.Write("\t");
            Response.Flush();
            Response.Close();
            Response.End();
        }
        private void AddServiceProviderRows(StringBuilder bldr, List<UserDTO> customers)
        {
            foreach (var customer in customers)
            {
                var state = "";
                if (customer.IsActive == true)
                {
                    state = @Resources.Global.IsActive;
                }
                if (customer.IsActive == false)
                {
                    state = @Resources.Global.NotActive;
                }

                bldr.AppendLine("<tr>");
                bldr.AppendLine($"<td>{customer.FirstName}</td><td>{customer.LastName}</td><td>{customer.Mobile}</td><td>{customer.Email}</td><td>{customer.Rating}</td> <td><span>" + state + "</span></td>");
                bldr.AppendLine("</tr>");
            }
        }
        private static void AddServiceProviderHeaderCells(StringBuilder bldr, params string[] headers)
        {
            foreach (var header in headers)
            {
                bldr.AppendLine($"<td>{header}</td>");
            }
        }
        #endregion
        #region Customer
        [HttpGet]
        public ActionResult Customers(int? page)
        {
            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
            var customers = _iService.AllCustomer().ToList().ToPagedList(page ?? 1, pageSize);
            if (customers.Count() == 0)
            {
                ViewBag.disablebuttons = "no data";
            }
            return View(customers);

        }
        [HttpGet]
        public ActionResult CustomersById(string Name)
        {
            TempData["tempCustomerId"] = Name;
            //var allCustomers = _IcustomerService.Allrequesters(null);
            //ViewBag.AllUsers = new SelectList(allCustomers, "Id", "FullName");
            var customers = _iService.CustomerByName(Name).ToList();
            if (customers.Count() == 0)
            {
                ViewBag.disablebuttons = "no data";
            }
            return View(customers);

        }
        [HttpGet]
        public void ExportCustomersData()
        {

            var customers = _iService.AllCustomer().ToList();
            ExportCustomerHTML(customers);
        }

        public ActionResult DownloadCustomersViewPDF()
        {
            var customers = _iService.AllCustomer().ToList();
            return new Rotativa.MVC.ViewAsPdf("DownloadCustomerViewPDF", customers)
            {
                FileName = "Customers.pdf",
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageSize = Rotativa.Core.Options.Size.A4,
                    PageOrientation = Rotativa.Core.Options.Orientation.Landscape,
                    CustomSwitches =
                    "--footer-center \"Name: " + "Customer" + "  Date: " +
                    DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" + " --header-spacing 20 -T 30mm" +
                    " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"",
                    PageMargins = new Rotativa.Core.Options.Margins { Left = 1, Right = 1 },
                    MinimumFontSize = 12,
                    //PageMargins = new Margins(3, 3, 3, 3),
                    IsGrayScale = true,
                    IsJavaScriptDisabled = true,
                    IsBackgroundDisabled = true,
                }

            };
        }

        [HttpGet]
        public void ExportCustomersByIdData()
        {
            string name = null;
            if (TempData["tempCustomerId"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "" || query.Length < 6)
                {
                    RedirectToAction("CustomersById", new { Name = "" });
                }
                if (query != "")
                {
                    if (query.Length > 6)
                    {
                        string x = query.Substring(6);
                        name = x;
                    }
                    if (name == null)
                    {
                        RedirectToAction("CustomersById", new { Name = "" });
                    }

                }

            }
            if (TempData["tempCustomerId"] != null)
            {
                name = TempData["tempCustomerId"].ToString();
            }

            var serviceProviders = _iService.CustomerByName(name).ToList();
            ExportCustomerHTML(serviceProviders);
        }
        public ActionResult DownloadCustomerByIdViewPDF()
        {
            string name = null;
            if (TempData["tempCustomerId"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("CustomersById", new { Name = "" });
                }
                if (query != "")
                {
                    if (query.Length > 4)
                    {
                        string x = query.Substring(6);
                        name = x;
                    }
                    if (name == null)
                    {
                        RedirectToAction("CustomersById", new { Name = "" });
                    }

                }

            }
            if (TempData["tempCustomerId"] != null)
            {
                name = TempData["tempCustomerId"].ToString();
            }
            var root = Server.MapPath("~/Reports/");
            var pdfname = String.Format("{0}.pdf", Guid.NewGuid().ToString());
            var path = Path.Combine(root, pdfname);
            var Customers = _iService.CustomerByName(name).ToList();
            return new Rotativa.MVC.ViewAsPdf("DownloadCustomerViewPDF", Customers)
            {
                FileName = "Customer.pdf",
                SaveOnServerPath = path,
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageSize = Rotativa.Core.Options.Size.A4,
                    PageOrientation = Rotativa.Core.Options.Orientation.Landscape,
                    CustomSwitches =
                    "--footer-center \"Name: " + "Customer" + "  Date: " +
                    DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" + " --header-spacing 20 -T 30mm" +
                    " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"",
                    PageMargins = new Rotativa.Core.Options.Margins { Left = 1, Right = 1 },
                    MinimumFontSize = 12,
                    //PageMargins = new Margins(3, 3, 3, 3),
                    IsGrayScale = true,
                    IsJavaScriptDisabled = true,
                    IsBackgroundDisabled = true,
                }

            };

        }

        private void ExportCustomerHTML(List<CustomerDTO> customers)
        {
            StringBuilder bldr = new StringBuilder();
            bldr.AppendLine(@"<table cellspacing='0' rules='all' border='1' style='border - collapse:collapse; '>");
            bldr.AppendLine("<tr>");
            var lang = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();

            if (lang == "ar-EG")
            {
                AddCustomerHeaderCells(bldr, "الاسم الاول", "الاسم الاخير", "الجوال", " البريد الالكترونى");
            }
            if (lang == "en-US")
            {
                AddCustomerHeaderCells(bldr, "First Name", "Last Name", "Mobile", "Email");
            }
            bldr.AppendLine("</tr>");

            bldr.AppendLine("<tr>");
            AddCustomerRows(bldr, customers);
            bldr.AppendLine("</tr>");

            bldr.AppendLine("</table>");

            Response.AddHeader("content-disposition", "attachment;filename=Customers.xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new HtmlTextWriter(sw);
            Response.Write(sw.ToString());
            Response.Output.Write(bldr.ToString());
            Response.Write("\t");
            Response.Flush();
            Response.Close();
            Response.End();
        }
        private void AddCustomerRows(StringBuilder bldr, List<CustomerDTO> customers)
        {
            foreach (var customer in customers)
            {
                bldr.AppendLine("<tr>");
                bldr.AppendLine($"<td>{customer.FirstName}</td><td>{customer.LastName}</td> <td>{customer.Mobile }</td><td>{customer.Email}</td>");
                bldr.AppendLine("</tr>");
            }
        }


        private static void AddCustomerHeaderCells(StringBuilder bldr, params string[] headers)
        {
            foreach (var header in headers)
            {
                bldr.AppendLine($"<td>{header}</td>");
            }
        }

        #endregion
        #region Complaints
        public ActionResult Complaints(int? page)
        {
            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
            var complints = _iService.AllComplaints().ToList().ToPagedList(page ?? 1, pageSize);
            if (complints.Count() == 0)
            {
                ViewBag.disablebuttons = "no data";
            }
            return View(complints);
        }
        [HttpGet]
        public ActionResult ComplaintsInIntervale(int? page, string fromDate, string toDate)
        {
            TempData["ComplintfromDate"] = fromDate;
            TempData["ComplinttoDate"] = toDate;
            TempData["ComplintfromDate1"] = fromDate;
            TempData["ComplinttoDate1"] = toDate;
            DateTime? fromDateDT = null;
            DateTime? toDateDT = null;
            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
            if (fromDate == null && toDate == null || fromDate == "" && toDate == "")
            {
                fromDateDT = null;
                toDateDT = null;
            }
            if (fromDate != null && toDate != null && fromDate.Length > 9 && toDate.Length > 9)
            {
                fromDateDT = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                toDateDT = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
            }
            var complints = _iService.ComplaintsByDate(fromDateDT, toDateDT).ToList().ToPagedList(page ?? 1, pageSize);
            if (complints.Count() == 0)
            {
                ViewBag.disablebuttons = "no data";
            }
            return View(complints);
        }
        [HttpGet]
        public void ExportComplaintsData()
        {
            GridView gv = new GridView();
            var complints = _iService.AllComplaints().ToList();
            ExportHTML(complints);/*
            gv.DataSource = complints;
            gv.DataBind();
            //var lang = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();
            //if (lang == "ar-EG")
            //{
                gv.HeaderRow.Cells[1].Text = "الشكوى";
                gv.HeaderRow.Cells[0].Text = "التاريخ";
                gv.HeaderRow.Cells[2].Text = "الاسم";
                gv.HeaderRow.Cells[3].Text = "البريد الالكترونى";
                gv.HeaderRow.Cells[4].Text = "الجوال";
               
            //}
            //else if (lang == "en-US")
            //{
            //    gv.HeaderRow.Cells[1].Text = "Complaint";
            //    gv.HeaderRow.Cells[0].Text = "Date";
            //    gv.HeaderRow.Cells[2].Text = "Name";
            //    gv.HeaderRow.Cells[3].Text = "Email";
            //    gv.HeaderRow.Cells[4].Text = "Phone";
              
            //}

            Response.ClearContent();
           
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=complints.xls");
            //Response.AddHeader("Content-Disposition", "attachment;filename=complints.csv");
            Response.ContentType = "application/ms-excel";
            // Response.Charset = "";

            

            Response.ContentEncoding = Encoding.UTF8;
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            var content = sw.ToString();

            Encoding encoding = Encoding.UTF8;
            content = content.Replace("<div>", "");
            content = content.Replace("</div>", "");
            var bytes = encoding.GetBytes(content);
            //MemoryStream stream = new MemoryStream(bytes);
            //StreamReader reader = new StreamReader(stream);

            
            //Response.Output.Write(reader.ReadToEnd());
            Response.Flush();
            Response.End();*/
        }

        private void ExportHTML(List<ComplaintViewModel> complints)
        {
            StringBuilder bldr = new StringBuilder();
            bldr.AppendLine(@"<table cellspacing='0' rules='all' border='1' style='border - collapse:collapse; '>");
            bldr.AppendLine("<tr>");
            AddHeaderCells(bldr, "الشكوي", "التاريخ", "الاسم", "البريد الالكترونى", "الجوال");
            bldr.AppendLine("</tr>");

            bldr.AppendLine("<tr>");
            AddRows(bldr, complints);
            bldr.AppendLine("</tr>");

            bldr.AppendLine("</table>");

            Response.AddHeader("content-disposition", "attachment;filename=Complints.xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new HtmlTextWriter(sw);
            Response.Write(sw.ToString());
            Response.Output.Write(bldr.ToString());
            Response.Write("\t");
            Response.Flush();
            Response.Close();
            Response.End();
        }

        private void AddRows(StringBuilder bldr, List<ComplaintViewModel> complints)
        {
            foreach (var complain in complints)
            {
                bldr.AppendLine("<tr>");
                bldr.AppendLine($"<td>{complain.Date.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)}</td><td>{complain.Cause}</td><td>{complain.Name}</td><td>{complain.Email}</td><td>{complain.Phone.ToString()}</td>");
                bldr.AppendLine("</tr>");
            }
        }

        private static void AddHeaderCells(StringBuilder bldr, params string[] headers)
        {
            foreach (var header in headers)
            {
                bldr.AppendLine($"<td>{header}</td>");
            }
        }

        [HttpGet]
        public void ExportComplaintsIntervalData()
        {
            DateTime? fromDate = null;
            DateTime? toDate = null;
            if (TempData["ComplintfromDate1"] == null || TempData["ComplinttoDate1"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("ComplaintsInIntervale", new { fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date });

                }
                if (query != "")
                {
                    string[] words = query.Split('&');
                    if (words.Contains("fromDate") && words[0].Length > 10)
                    {
                        string f = words[0].Substring(10);
                        string t = words[1].Substring(7);
                        fromDate = DateTime.ParseExact(f, "dd/MM/yyyy", null).Date;
                        toDate = DateTime.ParseExact(t, "dd/MM/yyyy", null).Date;
                    }

                    if (fromDate == null && toDate == null)
                    {
                        RedirectToAction("PropsalesInDate", new { fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date });
                    }

                }

            }
            if (TempData["ComplintfromDate1"] != null && TempData["ComplintfromDate1"].ToString().Length > 8)
            {

                fromDate = DateTime.ParseExact(TempData["ComplintfromDate1"].ToString(), "dd/MM/yyyy", null);
            }

            if (TempData["ComplinttoDate1"] != null && TempData["ComplinttoDate1"].ToString().Length > 6)
            {

                toDate = DateTime.ParseExact(TempData["ComplinttoDate1"].ToString(), "dd/MM/yyyy", null);
            }

            var complints = _iService.ComplaintsByDate(fromDate, toDate).ToList();
            ExportHTML(complints);
        }
        public ActionResult DownloadComplaintsViewPDF()
        {
            var complints = _iService.AllComplaints().ToList();
            var root = Server.MapPath("~/Reports/");
            var pdfname = String.Format("{0}.pdf", Guid.NewGuid().ToString());
            var path = Path.Combine(root, pdfname);
            return new Rotativa.MVC.ViewAsPdf("DownloadComplaintsViewPDF", complints)
            {
                FileName = "Complaints.pdf",
                SaveOnServerPath = path,
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageSize = Rotativa.Core.Options.Size.A4,
                    PageOrientation = Rotativa.Core.Options.Orientation.Landscape,
                    CustomSwitches =
                    "--footer-center \"Name: " + "Complints" + "  Date: " +
                    DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" + " --header-spacing 20 -T 30mm" +
                    " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"",
                    PageMargins = new Rotativa.Core.Options.Margins { Left = 1, Right = 1 },
                    MinimumFontSize = 12,
                    //PageMargins = new Margins(3, 3, 3, 3),
                    IsGrayScale = true,
                    IsJavaScriptDisabled = true,
                    IsBackgroundDisabled = true,
                }

            };
        }
        public ActionResult DownloadComplaintsIntervalesViewPDF()
        {
            DateTime? fromDate = null;
            DateTime? toDate = null;
            if (TempData["ComplintfromDate"] == null || TempData["ComplinttoDate"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("ComplaintsInIntervale", new { fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date });

                }
                if (query != "")
                {
                    string[] words = query.Split('&');
                    if (words.Contains("fromDate") && words[0].Length > 10)
                    {
                        string f = words[0].Substring(10);
                        string t = words[1].Substring(7);
                        fromDate = DateTime.ParseExact(f, "dd/MM/yyyy", null).Date;
                        toDate = DateTime.ParseExact(t, "dd/MM/yyyy", null).Date;
                    }

                    if (fromDate == null && toDate == null)
                    {
                        RedirectToAction("PropsalesInDate", new { fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date });
                    }

                }

            }
            if (TempData["ComplintfromDate"] != null && TempData["ComplintfromDate"].ToString().Length > 8)
            {

                fromDate = DateTime.ParseExact(TempData["ComplintfromDate"].ToString(), "dd/MM/yyyy", null);
            }

            if (TempData["ComplinttoDate"] != null && TempData["ComplinttoDate"].ToString().Length > 6)
            {

                toDate = DateTime.ParseExact(TempData["ComplinttoDate"].ToString(), "dd/MM/yyyy", null);
            }
            var root = Server.MapPath("~/Reports/");
            var pdfname = String.Format("{0}.pdf", Guid.NewGuid().ToString());
            var path = Path.Combine(root, pdfname);
            var complints = _iService.ComplaintsByDate(fromDate, toDate).ToList();
            return new Rotativa.MVC.ViewAsPdf("DownloadComplaintsViewPDF", complints)
            {
                FileName = "Complaints.pdf",
                SaveOnServerPath = path,
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageSize = Rotativa.Core.Options.Size.A4,
                    PageOrientation = Rotativa.Core.Options.Orientation.Landscape,
                    CustomSwitches =
                    "--footer-center \"Name: " + "Complaints" + "  Date: " +
                    DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" + " --header-spacing 20 -T 30mm" +
                    " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"",
                    PageMargins = new Rotativa.Core.Options.Margins { Left = 1, Right = 1 },
                    MinimumFontSize = 12,
                    //PageMargins = new Margins(3, 3, 3, 3),
                    IsGrayScale = true,
                    IsJavaScriptDisabled = true,
                    IsBackgroundDisabled = true,
                }

            };

        }
        #endregion
        #region Equipment
        [HttpGet]
        public ActionResult Equipments(int? page, int? parentId)
        {
            TempData["ParentId"] = parentId;
            TempData["ParentId1"] = parentId;
            var Allparents = _truckService.allParentByType(2);
            ViewBag.allParents = new SelectList(Allparents, "Id", "NameArb");
            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
            var equipments = _iService.AllEquipmentUnderParent(parentId).ToList().ToPagedList(page ?? 1, pageSize);
            if (parentId == null)
            {
                var trucksxx = _iService.AllEquipmentUnderParent(null).ToList().ToPagedList(page ?? 1, pageSize); ;
                return View(trucksxx);
            }
            return View(equipments);
        }
        [HttpGet]
        public ActionResult EquipmentsByName(int? page, string name)
        {
            TempData["equipmentName"] = name;

            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
            var equipments = _iService.EquipmentByName(name).ToList().ToPagedList(page ?? 1, pageSize);
            if (equipments.Count() == 0)
            {
                ViewBag.disablebuttons = "no data";
            }
            return View(equipments);
        }
        [HttpGet]
        public void ExportEquipmentsByParentData()
        {
            int? parentId = null;
            if (TempData["ParentId1"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("Equipments", new { parentId = 0 });

                }
                if (query != "")
                {
                    if (query.Length > 10)
                    {
                        string x = query.Substring(10);
                        if (x != "")
                        {
                            parentId = int.Parse(x);
                        }

                    }
                    if (parentId == null)
                    {
                        if (query == "")
                        {
                            RedirectToAction("Equipments", new { parentId = 0 });

                        }
                    }
                }

            }

            if (TempData["ParentId1"] != null)
            {
                parentId = int.Parse(TempData["ParentId1"].ToString());
            }
            if (parentId == null)
            {

                var zzz = _iService.EquipmentByNameOutParent("").ToList();
                ExportTruckHTML(zzz);
            }

            var equipments = _iService.AllEquipmentUnderParent(parentId).ToList();
            ExportTruckHTML(equipments);
        }
        public ActionResult DownloadEquipmentsByParentViewPDF()
        {
            int? parentId = null;
            if (TempData["ParentId"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("Equipments", new { parentId = 0 });

                }
                if (query != "")
                {
                    if (query.Length > 10)
                    {
                        string x = query.Substring(10);
                        if (x != "")
                        {
                            parentId = int.Parse(x);
                        }

                    }
                    if (parentId == null)
                    {
                        if (query == "")
                        {
                            RedirectToAction("Trucks", new { parentId = 0 });

                        }
                    }
                }

            }
            if (TempData["ParentId"] != null)
            {
                parentId = int.Parse(TempData["ParentId"].ToString());
            }
            var root = Server.MapPath("~/Reports/");
            var pdfname = String.Format("{0}.pdf", Guid.NewGuid().ToString());
            var path = Path.Combine(root, pdfname);
            var equipments = _iService.AllEquipmentUnderParent(parentId).ToList();
            return new Rotativa.MVC.ViewAsPdf("DownloadTrucksViewPDF", equipments)
            {
                FileName = "equipments.pdf",
                SaveOnServerPath = path,
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageSize = Rotativa.Core.Options.Size.A4,
                    PageOrientation = Rotativa.Core.Options.Orientation.Landscape,
                    CustomSwitches =
                    "--footer-center \"Name: " + "Equipment" + "  Date: " +
                    DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" + " --header-spacing 20 -T 30mm" +
                    " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"",
                    PageMargins = new Rotativa.Core.Options.Margins { Left = 1, Right = 1 },
                    MinimumFontSize = 12,
                    //PageMargins = new Margins(3, 3, 3, 3),
                    IsGrayScale = true,
                    IsJavaScriptDisabled = true,
                    IsBackgroundDisabled = true,
                }

            };
        }

        [HttpGet]
        public void ExportEquipmentsByNameData()
        {
            string name = "";
            if (TempData["equipmentName"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("EquipmentsByName", new { name = "" });

                }
                if (query != "")
                {
                    if (query.Length > 5)
                    {
                        string x = query.Substring(6);
                        name = x;
                    }
                    if (name == null)
                    {
                        RedirectToAction("TrucksByName", new { name = "" });
                    }
                }


            }
            if (TempData["equipmentName"] != null)
            {
                name = TempData["equipmentName"].ToString();
            }
            var equipments = _iService.EquipmentByName(name).ToList();
            ExportTruckHTML(equipments);

        }
        public ActionResult DownloadEquipmentsByNameViewPDF()
        {
            string name = "";
            if (TempData["equipmentName"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("EquipmentsByName", new { name = "" });

                }
                if (query != "")
                {
                    if (query.Length > 5)
                    {
                        string x = query.Substring(6);
                        name = x;
                    }
                    if (name == null)
                    {
                        RedirectToAction("TrucksByName", new { name = "" });
                    }
                }


            }
            if (TempData["equipmentName"] != null)
            {
                name = TempData["equipmentName"].ToString();
            }
            var root = Server.MapPath("~/Reports/");
            var pdfname = String.Format("{0}.pdf", Guid.NewGuid().ToString());
            var path = Path.Combine(root, pdfname);
            var equipments = _iService.EquipmentByName(name).ToList();
            return new Rotativa.MVC.ViewAsPdf("DownloadTrucksBYNameViewPDF", equipments)
            {
                FileName = "equipments.pdf",
                SaveOnServerPath = path,
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageSize = Rotativa.Core.Options.Size.A4,
                    PageOrientation = Rotativa.Core.Options.Orientation.Landscape,
                    CustomSwitches =
                    "--footer-center \"Name: " + "Equipment" + "  Date: " +
                    DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" + " --header-spacing 20 -T 30mm" +
                    " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"",
                    PageMargins = new Rotativa.Core.Options.Margins { Left = 1, Right = 1 },
                    MinimumFontSize = 12,
                    //PageMargins = new Margins(3, 3, 3, 3),
                    IsGrayScale = true,
                    IsJavaScriptDisabled = true,
                    IsBackgroundDisabled = true,
                }

            };

        }
        #endregion
        #region Trucks
        [HttpGet]
        public ActionResult Trucks(int? page, int? parentId)

        {
            TempData["truckParentId"] = parentId;
            TempData["truckParentId1"] = parentId;
            var Allparents = _truckService.allParentByType(1);
            ViewBag.allParents = new SelectList(Allparents, "Id", "NameArb");
            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
            if (parentId==null)
            {
                var trucksxx = _iService.AllTrucksUnderParent(null).ToList().ToPagedList(page ?? 1, pageSize); ;
                return View(trucksxx);
            }
            var trucks = _iService.AllTrucksUnderParent(parentId).ToList()
            .ToPagedList(page ?? 1, pageSize);
            //if (trucks.Count() == 0)
            //{
            //    ViewBag.disablebuttons = "no data";
            //}
            return View(trucks);
        }
        [HttpGet]
        public ActionResult TrucksByName(int? page, string name)
        {
            TempData["truckName"] = name;
            TempData["truckName1"] = name;
            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
            var trucks = _iService.TruckByName(name).ToList().ToPagedList(page ?? 1, pageSize);
            //if (trucks.Count() == 0)
            //{
            //    ViewBag.disablebuttons = "no data";
            //}
            return View(trucks);
        }
        [HttpGet]
        public void ExportTrucksByParentData()
        {
            int? parentId = null;

            if (TempData["truckParentId1"] == null)
            {

                //string query = Request.UrlReferrer.Query;
                //if (query == "")
                //{
                //    RedirectToAction("Trucks", new  { parentId = 0 });

                //}
                //if (query != "")
                //{
                //    if ( query.Length > 8)
                //    {
                //        string x = query.Substring(10);
                //        if (x !="")
                //        {
                //            parentId = int.Parse(x);
                //        }

                //    }
                //    if (parentId ==null)
                //    {
                //            var zzz = _iService.TruckByName(null).ToList();
                //            ExportTruckHTML(zzz);
                //    }
                //}
                var zzz = _iService.TruckByNameWithOutParent("").ToList();
                ExportTruckHTML(zzz);

            }
            if (TempData["truckParentId1"] != null)
            {
                parentId = int.Parse(TempData["truckParentId1"].ToString());
            }

            if (parentId != null)
            {
                var trucks = _iService.AllTrucksUnderParent(parentId).ToList();
                ExportTruckHTML(trucks);
            }





        }
        public ActionResult DownloadTrucksByParentViewPDF()
        {
            int? parentId = null;
            if (TempData["ParentId"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("Trucks", new { parentId = 0 });

                }

                if (query != "")
                {
                    if (query.Length > 10)
                    {
                        string x = query.Substring(10);
                        if (x != "")
                        {
                            parentId = int.Parse(x);
                        }

                    }
                    if (parentId == null)
                    {
                        if (query == "")
                        {
                            RedirectToAction("Trucks", new { parentId = 0 });

                        }
                    }
                }

            }
            if (TempData["truckParentId"] != null)
            {
                parentId = int.Parse(TempData["truckParentId"].ToString());
            }

            var Trucks = _iService.AllTrucksUnderParent(parentId).ToList();
            var root = Server.MapPath("~/Reports/");
            var pdfname = String.Format("{0}.pdf", Guid.NewGuid().ToString());
            var path = Path.Combine(root, pdfname);
            var lang = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();
            string reportHeader = "";
            if (lang == "ar-EG")
            {
                reportHeader = "الشاحنات";
                reportHeader = HttpUtility.HtmlEncode(reportHeader);


            }
            if (lang == "en-US")
            {
                reportHeader = "Trucks";
            }
            return new Rotativa.MVC.ViewAsPdf("DownloadTrucksViewPDF", Trucks)
            {
                FileName = "Trucks.pdf",
                SaveOnServerPath = path,
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageSize = Rotativa.Core.Options.Size.A4,
                    PageOrientation = Rotativa.Core.Options.Orientation.Landscape,
                    CustomSwitches =
                    "--footer-center \"Name: " + "Trucks" + "  Date: " +
                    DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" + " --header-spacing 20 -T 30mm" +
                    " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"",
                    PageMargins = new Rotativa.Core.Options.Margins { Left = 1, Right = 1 },
                    MinimumFontSize = 12,
                    //PageMargins = new Margins(3, 3, 3, 3),
                    IsGrayScale = true,
                    IsJavaScriptDisabled = true,
                    IsBackgroundDisabled = true,
                }

            };

        }


        [HttpGet]
        public void ExportTrucksByNameData()
        {
            string name = "";
            if (TempData["truckName"] == null)
            {
                string query = Request.UrlReferrer.Query;
                if (query == "")
                {
                    RedirectToAction("TrucksByName", new { name = "" });

                }
                if (query != "")
                {
                    if (query.Length > 5)
                    {
                        string x = query.Substring(6);
                        name = x;
                    }
                    if (name == null)
                    {
                        RedirectToAction("tr", new { name = "" });
                    }
                }


            }
            if (TempData["truckName"] != null)
            {
                name = TempData["truckName"].ToString();
            }

            var trucks = _iService.TruckByName(name).ToList();
            ExportTruckHTML(trucks);
        }
        public ActionResult DownloadTrucksByNameViewPDF()
        {
            string name = "";
            string truckname = "";
            if (TempData["truckName1"] == null)
            {
                //string query = Request.UrlReferrer.Query;
                //if (query == "")
                //{

                RedirectToAction("TrucksByName", new { name = "" });

                //}


                //if (query !="")
                //{
                //    if ( query.Length > 5)
                //    {
                //        string x = query.Substring(6);
                //        name = x;
                //    }
                //    if (name==null)
                //    {
                //        RedirectToAction("TrucksByName", new { name = "" });
                //    }
                //}


            }
            if (TempData["truckName1"] != null)
            {
                truckname = TempData["truckName1"].ToString();
            }
            var root = Server.MapPath("~/Reports/");
            var pdfname = String.Format("{0}.pdf", Guid.NewGuid().ToString());
            var path = Path.Combine(root, pdfname);
            var trucks = _iService.TruckByName(truckname).ToList();
            return new Rotativa.MVC.ViewAsPdf("DownloadTrucksBYNameViewPDF", trucks)
            {
                FileName = "Trucks.pdf",
                SaveOnServerPath = path,
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageSize = Rotativa.Core.Options.Size.A4,
                    PageOrientation = Rotativa.Core.Options.Orientation.Landscape,
                    CustomSwitches =
                      "--footer-center \"Name: " + "Trucks" + "  Date: " +
                      DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" + " --header-spacing 20 -T 30mm" +
                      " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"",
                    PageMargins = new Rotativa.Core.Options.Margins { Left = 1, Right = 1 },
                    MinimumFontSize = 12,
                    //PageMargins = new Margins(3, 3, 3, 3),
                    IsGrayScale = true,
                    IsJavaScriptDisabled = true,
                    IsBackgroundDisabled = true,
                }

            };


        }
        private void ExportTruckHTML(List<TruckDTO> trucks)
        {
            StringBuilder bldr = new StringBuilder();
            bldr.AppendLine(@"<table cellspacing='0' rules='all' border='1' style='border - collapse:collapse; '>");
            bldr.AppendLine("<tr>");
            var lang = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();
            if (lang == "ar-EG")
            {

                AddTrucksHeaderCells(bldr, "اسم الشاحنه بالعربيه", "اسم الشاحنه بالانجليزيه", "الشاحنه الرئيسية بالعربيه", "الشاحنه الرئيسية بالانجليزيه");

            }
            else if (lang == "en-US")
            {
                AddTrucksHeaderCells(bldr, "Truck Name", "Truck Name Eng", "Main Truck Name", "Main Truck Name Eng");

            }

            bldr.AppendLine("</tr>");

            bldr.AppendLine("<tr>");
            AddRows(bldr, trucks);
            bldr.AppendLine("</tr>");

            bldr.AppendLine("</table>");

            Response.AddHeader("content-disposition", "attachment;filename=Trucks.xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new HtmlTextWriter(sw);
            Response.Write(sw.ToString());
            Response.Output.Write(bldr.ToString());
            Response.Write("\t");
            Response.Flush();
            Response.Close();
            Response.End();
        }

        private void AddRows(StringBuilder bldr, List<TruckDTO> trucks)
        {
            foreach (var truck in trucks)
            {
                bldr.AppendLine("<tr>");
                bldr.AppendLine($" <td>{truck.TruckNameArb}</td><td>{truck.TruckNameEng}</td><td>{truck.TruckParentNameArb}</td><td>{truck.TruckParentNameEng}</td> ");
                bldr.AppendLine("</tr>");

            }
        }

        private static void AddTrucksHeaderCells(StringBuilder bldr, params string[] headers)
        {
            foreach (var header in headers)
            {
                bldr.AppendLine($"<td >{header}</td>");
            }
        }
        #endregion


    }
}