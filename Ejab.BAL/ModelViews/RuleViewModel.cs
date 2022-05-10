using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
   public  class RuleViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string DescriptionEng { get; set; }
        public bool  IsChecked { get; set; }
        public IEnumerable<UserViewModel > Users { get; set; }
    }
}
