using Ejab.DAl.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
    public class SortingParamsViewModer
    {
        public string SortPram { get; set; }
        public string SearchTerm { get; set; }
        public decimal? fromPrice { get; set; }
        public decimal? toPrice { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public string Address { get; set; }
        public bool Asc { get; set; }
        public string action { get; set; }
        public OfferState Stat { get; set; }
        public int? Rating { get; set; }
        public int PageSize { get; set; }
      public   int flage = 0;
        public bool ? IsAuth { get; set; }
        public bool  CanEdit { get; set; }
        //public bool? AcceptanceState { get; set; }
        //public RequestStates RequestStates { get; set; }
    }
}
