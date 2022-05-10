using Ejab.DAl.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
    public class UpdateRequestStateViewModel
    {
        public int RequestId { get; set; }
        public RequestStates State { get; set; }
        public string Comment { get; set; }
        public int ServiceProviderId { get; set; }
        public ServiceProviderRatingViewModel RatingModel { get; set; }
    }
}
