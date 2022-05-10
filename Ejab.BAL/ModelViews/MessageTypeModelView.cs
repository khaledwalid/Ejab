using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
   public  class MessageTypeModelView
    {
        public int TypeId { get; set; }
        [Required(ErrorMessage = "Message Type Name Is Required")]
        public string Name { get; set; }
    }
}
