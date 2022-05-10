using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
 public    class CustomerDetailesviewModelcs
    {
        public string FirstName { get; set; } // Name (length: 50)
        public string LastName { get; set; } // Name (length: 50)
       
        //[Display(Name = "Email", ResourceType = typeof(Resources.Global))]
        public string Email { get; set; } // Email (length: 50)   
      
        public string Mobile { get; set; } // Moble (length: 15)      
        public    IEnumerable<RequestModelView> Requests { get; set; }
     public    IEnumerable<AcceptofferViewModel > AcceptedOffers { get; set; }
    }
}
