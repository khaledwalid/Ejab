using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ejab.BAL.ModelViews;
using Ejab.BAL.UnitOfWork;
using Ejab.BAL.Common;
using Ejab.DAl.Common;

namespace Ejab.BAL.Services
{
    public class ComplaintService : IComplaintService
    {
        IUnitOfWork _uow;
        ModelFactory factory;
        int pageSize = PagingConfig.pageSize;
        public ComplaintService(IUnitOfWork uow)
        {
            this._uow = uow;
            factory = new ModelFactory();
        }
        public SuggestionsComplaintModelView AddComplaint(SuggestionsComplaintModelView model)
        {
            if (model == null)
            {
                throw new Exception("005");
            }
              
            var entity = factory.Parse(model);
            //entity.CustomerId  = UserId;

            entity.FlgStatus = 1;
            entity.Date = DateTime.Now;
            entity.CreatedBy = 1;
            entity.CreatedOn = DateTime.UtcNow.Date;
            entity.ComplaintStatus = ComplaintStatus.NotSeen;
            _uow.SuggestionsComplaint .Add(entity);
            _uow.Commit();
            //var CompliantUser = _uow.User.GetById(model.ComplainUserId);
            //var customer = _uow.User .GetById(UserId);
            var complaintModel = factory.Create(entity);
            //complaintModel.Customer = factory.Create (customer);
            //complaintModel.ComplainUser = factory.Create(CompliantUser);

            return complaintModel;
        }

        public SuggestionsComplaintModelView DeleteComplaint(int id)
        {
            var existedentity = _uow.SuggestionsComplaint.GetById(id);
            existedentity.FlgStatus = 0;
            _uow.SuggestionsComplaint.Update(id, existedentity);
            _uow.Commit();
            var messageModel = factory.Create(existedentity);
            return messageModel;
        }

        public SuggestionsComplaintModelView EditComplaint(int id, SuggestionsComplaintModelView model, int UserId)
        {
            if (model == null)
            {
                throw new Exception("005");
            }
           
            // TimeSpan myDateResult = model.SendingTime .TimeOfDay;
            var existedentity = _uow.SuggestionsComplaint .GetById(id);
            existedentity.Date = model.Date;
            existedentity.ComplaintStatus = ComplaintStatus.NotSeen;
            existedentity.Cause = model.Cause;
            existedentity.Name  = model.Name ;
            existedentity.Email  = model.Email ;
            existedentity.Phone  = model.Phone ;
            //existedentity.CustomerId = UserId;
            existedentity.FlgStatus = 1;
            existedentity.UpdatedBy = UserId;
            existedentity.UpdatedOn = DateTime.Now.Date;
            _uow.SuggestionsComplaint .Update(id, existedentity);
            _uow.Commit();
            var messageModel = factory.Create(existedentity);
           return messageModel;
        }

        public IQueryable <SuggestionsComplaintModelView> GetAllComplaints()
        {
            var complaints = _uow.SuggestionsComplaint.GetAll(x => x.FlgStatus == 1 ).OrderByDescending(x=>x.Id).Select(C=> new SuggestionsComplaintModelView {Id=C.Id, Date=C.Date,Name=C.Name,Email=C.Email,Phone=C.Phone ,Cause=C.Cause,ComplaintStatus=C.ComplaintStatus });
            return complaints;
        }

        public  IQueryable< SuggestionsComplaintModelView> getComplaintForUser(int UserId)
        {
            var complaints = _uow.SuggestionsComplaint.GetAll(x => x.FlgStatus == 1).Select(C => new SuggestionsComplaintModelView { Id = C.Id, Date = C.Date, Name = C.Name, Email = C.Email, Phone = C.Phone, Cause = C.Cause, ComplaintStatus = C.ComplaintStatus });
            return complaints;
        }

        public void MakeSeen(int ComplaintId)
        {
            var existingcomplint = _uow.SuggestionsComplaint.GetById(ComplaintId);
            if (existingcomplint == null)
            {
                throw new Exception("011");
            }
            existingcomplint.ComplaintStatus  = ComplaintStatus.Seen;
            _uow.SuggestionsComplaint.Update(ComplaintId, existingcomplint);
            _uow.Commit();
        }
    }
}
