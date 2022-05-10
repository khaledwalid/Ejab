using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMEH.BAL.ViewModeles.CommonQuestions;
using Ejab.BAL.UnitOfWork;
using Ejab.BAL.Common;
using Ejab.BAL.ModelViews.CommonQuestions;

namespace Ejab.BAL.Services.Questions
{
    public class QuestionService : IQuestionService
    {
        int LoginUserId = 1;
        IUnitOfWork _uow;
        ModelFactory Factory;
        public QuestionService(IUnitOfWork Uow)
        {
            _uow = Uow;
            Factory = new ModelFactory();
        }
        public Commonquestionsviewmodel AddQuestion(Commonquestionsviewmodel question)
        {
            var entity = Factory.Parse(question);
           
            entity.FlgStatus = 1;
            entity.CreatedBy = 1;
            entity.CreatedOn = DateTime.Now.Date;
            _uow.CommonQuestion.Add( entity);
            _uow.Commit();
            var model = Factory.Create(entity);
            return model;
        }

        public IQueryable<Commonquestionsviewmodel> AllQuestion()
        {
            return _uow.CommonQuestion.GetAll(x => x.FlgStatus == 1,null,"").Select(q => new Commonquestionsviewmodel { Id=q.Id,QuestionArb=q.QuestionArb,AnswerArb=q.AnswerArb ,QuestionEng=q.QuestionEng ,AnswerEng=q.AnswerEng});
        }

        public Commonquestionsviewmodel EditQuestion(int id, Commonquestionsviewmodel question)
        {
            var entity = _uow.CommonQuestion.GetById(id);
           
            entity.QuestionArb  = question.QuestionArb ;
            entity.AnswerArb  = question.AnswerArb ;
            entity.QuestionEng  = question.QuestionEng ;
            entity.AnswerEng = question.AnswerEng;
            entity.FlgStatus = 1;
            entity.UpdatedBy = LoginUserId;
            entity.UpdatedOn = DateTime.Now.Date;
            _uow.CommonQuestion .Update(id, entity);
            _uow.Commit();
            var model = Factory.Create(entity);
            return model;
        }

        public Commonquestionsviewmodel GetQuestion(int id)
        {
            var entity = _uow.CommonQuestion .GetById(id);
            var model = Factory.Create(entity);
            return model;
        }

        public Commonquestionsviewmodel DeleteQuestion(int id)
        {
            var entity = _uow.CommonQuestion .GetById(id);
            entity.FlgStatus = 0;
            entity.UpdatedBy = LoginUserId;
            entity.UpdatedOn = DateTime.Now.Date;
            _uow.CommonQuestion.Update(id,entity);
            _uow.Commit();
            var model = Factory.Create(entity);
            return model;
        }

        public IQueryable<Commonquestionsviewmodel> Top5Question()
        {
            var topQuestions = _uow.CommonQuestion.GetAll(x => x.FlgStatus == 1,null,"").Take(5).Select(q => new Commonquestionsviewmodel { Id = q.Id, QuestionArb = q.QuestionArb, AnswerArb = q.AnswerArb, QuestionEng = q.QuestionEng, AnswerEng = q.AnswerEng });
            return topQuestions;
        }
    }
}
