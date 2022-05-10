using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews.AboutApplication
{
  public  class AboutAppViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Global),
         ErrorMessageResourceName = "AbouAppArabicRequired")]
      
        [StringLength(50000)]
        public string AboutApp { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
        ErrorMessageResourceName = "AbouAppEnglishRequired")]
        [StringLength(50000)]
        public string AboutAppEng { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
      ErrorMessageResourceName = "AppURlRequired")]
        
        [DataType(DataType.Url)]
        public string AppLink { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
      ErrorMessageResourceName = "AppFacbookUrlRequird")]
        
        [DataType(DataType.Url)]
        public string FaceBookLink { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
      ErrorMessageResourceName = "AppTwitterUrlRequird")]
      
        [DataType(DataType.Url)]
        public string TwitterLink { get; set; }
    }
}
