using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.DAl.Models
{
   public  class Region: BaseModel
    {
        public Region()
        {
            Offers = new HashSet<Offer>();
         Requests = new HashSet<Request>();
            Interests = new HashSet<Interest>();
        }
        public string Name { get; set; }
        public int? parantId { get; set; }
        public string  NameArb { get; set; }

        public virtual System.Collections.Generic.ICollection<Region> Regions { get; set; }
        public virtual System.Collections.Generic.ICollection<Offer > Offers { get; set; }
       public virtual System.Collections.Generic.ICollection<Request > Requests { get; set; }
        public virtual System.Collections.Generic.ICollection<Interest > Interests { get; set; }
    }
}
