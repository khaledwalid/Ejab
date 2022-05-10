using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews.SmsMessage
{
    public enum Returntype
    {
        Json=1,
        Xml=2,
        String =3
    }
  public  class SmsViewModel
    { 
        //[Required(ErrorMessage = "94")]
        //public string Message { get; set; }
        [Required(ErrorMessage = "020")]
        public string number { get; set; }
        //[Required(ErrorMessage = "95")]
        //public string sender { get; set; }
        //public Returntype Return  { get; set; }
        //public string  UniCode { get; set; }
    }
}
