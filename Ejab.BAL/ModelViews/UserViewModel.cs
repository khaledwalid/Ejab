using Ejab.DAl.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
   public  class UserViewModel
    {
        public int Id { get; set; }
        [Display(Name = "FirstName", ResourceType = typeof(Resources.Global))]
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
             ErrorMessageResourceName = "UserNameRequired")]
        public string FirstName { get; set; } // Name (length: 50)
        [Required(ErrorMessage = "019")]
        [StringLength(50)]
        //[Display(Name = "LastName", ResourceType = typeof(Resources.Global))]
        public string LastName { get; set; } // Name (length: 50)
        [EmailAddress(ErrorMessage = "023")]
       
        [Required(ErrorMessage ="022")]
        //[Display(Name = "Email", ResourceType = typeof(Resources.Global))]
        public string Email { get; set; } // Email (length: 50)
        [Phone(ErrorMessage = "051")]
        //[RegularExpression(@" ^ (?:\d{8}|00\d{10}|\+\d{2}\d{8})$", ErrorMessage = "051")]
        [Required(ErrorMessage ="020")]
        //[MaxLength(13,ErrorMessage ="رقم الجوال لايزيد عن 13 رقم")]
        //[Display(Name = "Mobile", ResourceType = typeof(Resources.Global))]
        [DataType (DataType.PhoneNumber)]
       [MaxLength(12,ErrorMessage ="29")]
        public string Mobile { get; set; } // Moble (length: 15)       
        public string Address { get; set; } // Address (length: 100)
        public CustomerTypes CustomerType { get; set; } // CustomerTypeId
        public decimal? AddressLatitude { get; set; } // AddressLatitude
        public decimal? AddressLongitude { get; set; } // AddressLongitude
        //[Display(Name = "IsAdmin", ResourceType = typeof(Resources.Global))]
        //public bool? IsAdmin { get; set; } // IsAdmin
        public string ResponsiblePerson { get; set; } // ResponsiblePerson (length: 50)
        public decimal? Rating { get; set; }
        //[Display(Name = "ProfileImgPath", ResourceType = typeof(Resources.Global))]
        public string ProfileImgPath { get; set; }
        // [DataType(DataType.Password)]
        //[Required (ErrorMessage ="016")]
        //[Display(Name = "Password", ResourceType = typeof(Resources.Global))]
        public string Password { get; set; } //User Password       
          public string DeviceToken { get; set; }
         public string SN { get; set; }
        public string DeviceType { get; set; }
        public string   UserToken { get; set; }
        public string  TokenType { get; set; }
        public string Tokenissue { get; set; }
        public string TokenExpire { get; set; }
        public object  UserRoles { get; set; }
        //public RegisteredBy RegisteredBy { get; set; }
        //public string  FaceBookId { get; set; }
        public int FlgStatus { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public UserInterestsViewModel Intersts { get; set; }
        //public IEnumerable<RuleViewModel> Rules { get; set; }
        //public IEnumerable<RuleViewModel> ExistedRules { get; set; }
        public int? UnreadMessagesCount { get; set; }
        public string  FullName { get; set; }
    }
}
