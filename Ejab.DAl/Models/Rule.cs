using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.DAl.Models
{
   public  class Rule: BaseModel
    {
        public Rule()
        {
            Users = new HashSet<User>();
        }
        [Required(ErrorMessage ="Role Name Is Required")]
        public string  Name { get; set; }
        [Required(ErrorMessage =" Screen Name Is Required")]
        public string Description { get; set; }

        public string DescriptionEng { get; set; }
        //public bool IsChecked { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
