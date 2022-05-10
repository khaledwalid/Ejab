using Ejab.DAl.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
    public class RequestDetailesPricesViewModel
    {
      
        public int RequestDetaileId { get; set; }
        public RequestDetailsModelView RequestDetails{ get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }       
        public int ServiceProviderId { get; set; }
        public UserViewModel ServiceProvider { get; set; }
        public string  ServiceProviderName { get; set; }
        public decimal?  Rating { get; set; }
      
        public DateTime? ExpireDate { get; set; }
        [DataType(DataType.MultilineText)]
        public string  Notes { get; set; }

    }
}
