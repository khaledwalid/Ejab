using Ejab.DAl.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
  public   class ServiceProviderDetailesViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } // Name (length: 50)
        [StringLength(50)]
        public string LastName { get; set; } // Name (length: 50)
        [EmailAddress]
        [Required(ErrorMessage = "022")]
        public string Email { get; set; } // Email (length: 50)
        [Phone(ErrorMessage = "051")]
        //[RegularExpression(@" ^ (?:\d{8}|00\d{10}|\+\d{2}\d{8})$", ErrorMessage = "051")]
        [Required(ErrorMessage = "020")]
        public string Mobile { get; set; } // Moble (length: 15)       
        public string Address { get; set; } // Address (length: 100)       
        public IEnumerable<UserViewModel> ResponsiblePersons { get; set; } // ResponsiblePerson (length: 50)
        public decimal? Rating { get; set; }
        public string ProfileImgPath { get; set; }
        public IEnumerable<OfferViewModel> ServiceProvidersOffers { get; set; }
        public IEnumerable<ProposalPriceModelView> AcceptedRequests { get; set; }
      
        public IEnumerable<UserViewModel> comapnies { get; set; }
        public bool IsActive { get; set; }
    }
}
