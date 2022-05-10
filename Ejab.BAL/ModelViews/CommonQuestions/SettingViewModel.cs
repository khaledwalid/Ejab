using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMEH.BAL.ViewModeles.CommonQuestions
{
  public class SettingViewModel
    {
        public int Id { get; set; }
        [Required]
        public string FilePath { get; set; }
    }
}
