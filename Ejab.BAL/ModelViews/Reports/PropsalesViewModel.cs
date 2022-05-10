using Ejab.DAl.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews.Reports
{
  public   class PropsalesViewModel
    {
        public int RequestNumber { get; set; }
        [Display(Name = "ServiceProvider", ResourceType = typeof(Resources.Global))]
        public string ServicrProvider { get; set; }
        [Display(Name = "ServiceProviderRating", ResourceType = typeof(Resources.Global))]
        public decimal? Rating { get; set; }
        public RequestModelView Request { get; set; }
        [Display(Name = "Date", ResourceType = typeof(Resources.Global))]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public System.DateTime Date { get; set; } // Date
        [DataType(DataType.Currency)]
        public decimal Price { get; set; } // Price
        public string  CustomerName { get; set; }
        public string  PropsalStat { get; set; }
        public string  RequestTitle { get; set; }
    }
}
