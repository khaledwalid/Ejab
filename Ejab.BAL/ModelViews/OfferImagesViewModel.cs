using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
  public   class OfferImagesViewModel
    {
        public int OfferId { get; set; }
        public string ImageTitle { get; set; }
        public string ImageDescription { get; set; }
        [Required(ErrorMessage = "Image Url Is Required")]
        public string ImageUrl { get; set; }
        public short FlgStatus { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime? UpdatedOn { get; set; }
        public OfferViewModel  Offer { get; set; }

    }
}
