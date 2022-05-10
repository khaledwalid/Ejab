using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews.CommonQuestions
{
   public class Commonquestionsviewmodel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Question Arabic is required")]
        public string QuestionArb { get; set; }
        [Required(ErrorMessage ="Answer arabic is required")]
        [StringLength(10000)]
        public string AnswerArb { get; set; }
        public string QuestionEng { get; set; }
        [StringLength(10000)]
        public string AnswerEng { get; set; }
    }
}
