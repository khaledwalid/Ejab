using Ejab.BAL.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.Services
{
  public   interface IComplaintService
    {
        SuggestionsComplaintModelView AddComplaint(SuggestionsComplaintModelView model);
        SuggestionsComplaintModelView EditComplaint(int id, SuggestionsComplaintModelView model, int UserId);
        SuggestionsComplaintModelView DeleteComplaint(int id);
        IQueryable <SuggestionsComplaintModelView> GetAllComplaints();
      IQueryable<SuggestionsComplaintModelView> getComplaintForUser(int UserId);
        void MakeSeen(int ComplaintId);
    }
}
