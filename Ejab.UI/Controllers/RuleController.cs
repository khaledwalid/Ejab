using Ejab.BAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Configuration;
using Ejab.BAL.ModelViews;

namespace Ejab.UI.Controllers
{
    public class RuleController : BaseController 
    {
        private readonly IRuleService _iRuleService;
        int pagesize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
        public RuleController(IRuleService ruleService )
        {
            _iRuleService = ruleService;
            
        }
        // GET: Rule
        public ActionResult Index(int? page ,string search)
        {
            var model = _iRuleService.AllRules(search).ToList().ToPagedList(page?? 1, pagesize);
            return View(model);
        }
        [HttpGet]
        public ActionResult GetBYId(int id)
        {
            var rule = _iRuleService.GetRule(id);
            return Json(rule, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult AddRule()
        {
            return PartialView("~/Views/Rule/_AddRule.cshtml");
        }
        [HttpPost]
        public ActionResult AddRule(RuleViewModel rule)
        {
            if (ModelState.IsValid )
            {
                var entity = _iRuleService.AddRule(rule, 1);
                return RedirectToAction("Index");
            }
           
            return PartialView("~/Views/Rule/_AddRule.cshtml", rule);
        }
        [HttpGet]
        public ActionResult EditRule(int id)
        {
            var entity = _iRuleService.GetRule(id);
            return View( entity);
        }
        [HttpPost]
        public ActionResult EditRule(int id, RuleViewModel rule)
        {

            var entity = _iRuleService.GetRule(id);
            if (ModelState.IsValid )
            {
                var model = _iRuleService.EditRule(id,rule,1);
                return RedirectToAction("Index");
            }
            return View(rule);
        }
        [HttpGet]
        public ActionResult DeleteRule(int id)
        {
            var entity = _iRuleService.GetRule(id);
            _iRuleService.DeleteRule(id, 1);
            return RedirectToAction("Index");
        }
      

    }
}