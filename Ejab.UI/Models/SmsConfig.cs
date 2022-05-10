using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ejab.UI.Models
{
    public class SmsConfig
    {
        [Required(ErrorMessage ="")]
        public string Return { get; set; }
        [Required(ErrorMessage = "")]
        public string username { get; set; }
        [Required(ErrorMessage = "")]
        public string password { get; set; }
        [Required(ErrorMessage = "")]       
        public string MsgKey { get; set; }
        [Required(ErrorMessage = "")]
        public string Message { get; set; }
        [Required(ErrorMessage = "")]
        public string msgkeytype { get; set; }
        [Required(ErrorMessage = "")]
        public string numbers { get; set; }
        [Required(ErrorMessage = "")]
        public string Sender { get; set; }
        public string datetime { get; set; }
        public string browses { get; set; }
        public string userip { get; set; }


    }
}