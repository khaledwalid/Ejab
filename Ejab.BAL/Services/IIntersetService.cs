using Ejab.BAL.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.Services
{
    public interface IIntersetService
    {
        InterestViewModel AddInterest(InterestViewModel model, int UserId);
        InterestViewModel EditInterest(int id, InterestViewModel model, int UserId);
        UserInterestsViewModel DeleteInterest(DeleteInterestViewModel ids, int UserId);
        InterestViewModel Get(int id);
        UserInterestsViewModel GetAll(int userId, string type);
        IEnumerable<InterestViewModel> UserInterest(int userId);
        IEnumerable<InterestViewModel> trucksInterest(int truckId);
        IEnumerable<InterestViewModel> RegionInterest(int regionId);
        int userInterestsCount(int UserId, string type);
        bool checktruckExist(int truckId,int userId);
        bool checkRegionExist(int RegionId, int userId);


    }
}
