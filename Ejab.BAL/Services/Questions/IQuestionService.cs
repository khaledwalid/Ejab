using Ejab.BAL.ModelViews.CommonQuestions;
using SMEH.BAL.ViewModeles.CommonQuestions;
using System.Collections.Generic;
using System.Linq;


namespace Ejab.BAL.Services.Questions
{
 public  interface IQuestionService
    {
        Commonquestionsviewmodel AddQuestion(Commonquestionsviewmodel question);
        Commonquestionsviewmodel EditQuestion(int id,Commonquestionsviewmodel question);
        Commonquestionsviewmodel GetQuestion(int id);
       IQueryable<Commonquestionsviewmodel> AllQuestion();
        IQueryable< Commonquestionsviewmodel> Top5Question();
        Commonquestionsviewmodel DeleteQuestion(int id);

    }
}
