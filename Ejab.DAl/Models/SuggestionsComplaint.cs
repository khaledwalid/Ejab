// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable EmptyNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.5
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning


using Ejab.DAl.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ejab.DAl.Models
{

    // SuggestionsComplaint
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class SuggestionsComplaint : BaseModel
    {
        public System.DateTime Date { get; set; } // Date
        [Required(ErrorMessage = "77")]
        public string Cause { get; set; } // Cause (length: 150)
                                          //  public string Description { get; set; } // Description (length: 500)
                                          //public int CustomerId { get; set; } // CustomerId
                                          //public int? Admin { get; set; } // Admin
                                          //public int ComplainUserId { get; set; } // ComplainUserId

        public ComplaintStatus ComplaintStatus { get; set; }
        [Required(ErrorMessage = "048")]
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "022")]
        [EmailAddress(ErrorMessage = "023")]
        public string Email { get; set; }
        [Required(ErrorMessage = "76")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "051")]
        [StringLength(14)]
        public string Phone { get; set; }


    }
       

}
// </auto-generated>
