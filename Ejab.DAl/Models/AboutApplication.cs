using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.DAl.Models
{
  public   class AboutApplication:BaseModel 
    {
        [Required]
        [StringLength(50000)]
        public string  AboutApp { get; set; }
        [Required]
        [StringLength(50000)]
        public string AboutAppEng { get; set; }
        [Required]
        [DataType(DataType.Url)]
        public string  AppLink { get; set; }
        [Required]
        [DataType(DataType.Url)]
        public string FaceBookLink { get; set; }
        [Required]
        [DataType(DataType.Url)]
        public string TwitterLink { get; set; }
    }
}
