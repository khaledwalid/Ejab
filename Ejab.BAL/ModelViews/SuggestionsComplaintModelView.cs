using Ejab.DAl.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
    public class SuggestionsComplaintModelView
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "031")]
        public System.DateTime Date { get; set; } // Date
        [Required(ErrorMessage = "77")]
        [MaxLength(500)]
        public string Cause { get; set; } // Cause (length: 150)
        //[Required(ErrorMessage = "034")]
        //public string Description { get; set; } // Description (length: 500)
        //public int CustomerId { get; set; } // CustomerId
        //public UserViewModel Customer { get; set; } // FK_SuggestionsComplaint_User      
        //public int? Admin { get; set; } // Admin
        //public int ComplainUserId { get; set; } // ComplainUserId
        //public UserViewModel ComplainUser { get; set; } // FK_SuggestionsComplaint_User2      
        //public UserViewModel User_Admin { get; set; } // FK_SuggestionsComplaint_User1
        public ComplaintStatus ComplaintStatus { get; set; }
        [Required(ErrorMessage = "048")]
        public string Name { get; set; }
        [EmailAddress(ErrorMessage = "022")]
        public string Email { get; set; }
        [Phone(ErrorMessage = "051")]
        [Required(ErrorMessage = "76")]
        public string Phone { get; set; }

    }
}
