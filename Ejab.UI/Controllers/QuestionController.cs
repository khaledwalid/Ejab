using Ejab.BAL.ModelViews.CommonQuestions;
using Ejab.BAL.Services.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Configuration;

namespace Ejab.UI.Controllers
{
    public class QuestionController : Controller
    {
        IQuestionService _iQuestionService;
        public QuestionController(IQuestionService IQuestionService)
        {
            _iQuestionService = IQuestionService;

        }

        // GET: Question
        public ActionResult Index(int? page, int pagesize = 10)
        {
            var model = _iQuestionService.AllQuestion().ToList().ToPagedList(page ?? 1, pagesize);
            ViewBag.totalCounts = _iQuestionService.AllQuestion().ToList().Count();
            return View(model);
        }
        [HttpGet]
        public ActionResult AddQuestion()
        {
            //return PartialView ("~/Views/Director/_AddDirector.cshtml");
            return View();
        }
        //[Authorize ]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddQuestion(Commonquestionsviewmodel questione)
        {

            if (!ModelState.IsValid)
            {
                return View(questione);
            }
            _iQuestionService.AddQuestion(questione);
            return RedirectToAction("Index");

        }

        [HttpGet]
        //[Authorize]
        public ActionResult EditQuestion(int id)
        {
            var director = _iQuestionService.GetQuestion(id);
            return View(director);
        }
        [HttpPost]
        //[Authorize]
        public ActionResult EditQuestion(int id, Commonquestionsviewmodel question)
        {

            _iQuestionService.EditQuestion(id, question);
            return RedirectToAction("Index");
            if (!ModelState.IsValid)
            {
                return View(question);
            }

        }
        [Authorize]
        [HttpGet]
        public ActionResult Deletequestion(int id)
        {
            _iQuestionService.DeleteQuestion(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult AllFAQ(int? page,int pagesize=10)
        {

            ViewBag.tiwitter = ConfigurationManager.AppSettings["tiwitter"];
            ViewBag.pinteres = ConfigurationManager.AppSettings["pinteres"];
            ViewBag.google = ConfigurationManager.AppSettings["google"];
            ViewBag.facebook = ConfigurationManager.AppSettings["facebook"];
            ViewBag.instagram = ConfigurationManager.AppSettings["instagram"];
            var model = _iQuestionService.AllQuestion().ToList().ToPagedList(page??1, pagesize);
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult TopQuestions()
        {
            var model = _iQuestionService.Top5Question().ToList();
            return PartialView("~/Views/Question/_TopQuestions.cshtml", model);
        }
    }
}