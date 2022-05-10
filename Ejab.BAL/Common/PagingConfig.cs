using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.Common
{
  public static  class PagingConfig
    {

        // no of recordes that i need in page
        public  static    int pageSize = 20;
      //int pages= (int)Math.Ceiling(totalCount / (double)pageSize);
        public static int  totalCount { get; set; }
        public static int  pageindex { get; set; }
        //رقم الصفحه
        public static int PageNumber { get; set; }
        public static string NextPageUrl { get; set; }
     //   public IEnumerable<T> Results { get; set; }
   

    }
}
