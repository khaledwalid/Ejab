using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.DAl.Models
{
  public   class CommonQuestion : BaseModel
    {
        [Required]
        public string QuestionArb { get; set; }
        [Required]
        [StringLength(10000)]
        public string AnswerArb { get; set; }
        public string QuestionEng { get; set; }
        [StringLength(10000)]
        public string AnswerEng { get; set; }
    }
}
