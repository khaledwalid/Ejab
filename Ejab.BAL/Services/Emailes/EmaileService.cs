using System;
using System.Linq;
using Ejab.BAL.ModelViews.Email;
using Ejab.BAL.UnitOfWork;
using Ejab.BAL.Common;

namespace Ejab.BAL.Services.Emailes
{
    public class EmaileService : IEmaileService
    {
        IUnitOfWork _uow;
        ModelFactory factory;
        public EmaileService(IUnitOfWork uow)
        {
            this._uow = uow;
            factory = new ModelFactory();
        }

        public bool CheckEmail(string email)
        {
           return  _uow.MailSubscribe.GetAll(x => x.FlgStatus == 1 , null, "").Any(y=>y.Email == email);
           
        }

        public EmailSubscriptionViewModel AddEmaile(EmailSubscriptionViewModel model, int userId)
        {
            if (model == null)
            {
                throw new Exception("005");
            }
            var entity = factory.Parse(model);
          
            entity.CreatedBy = userId;
            entity.CreatedOn = DateTime.Now;
            entity.FlgStatus = 1;
            entity.UpdatedBy = null;
            entity.UpdatedOn = null;
            _uow.MailSubscribe .Add(entity);
            _uow.Commit();
          
           
            return factory.Create(entity);
        }

        public IQueryable< EmailSubscriptionViewModel> AllEmailes()
        {
            var mailes = _uow.MailSubscribe .GetAll(x => x.FlgStatus == 1).Select(x => new EmailSubscriptionViewModel { Email=x.Email});
            if (mailes == null)
            {
                throw new Exception("006");
            }
            return mailes;
        }

        public EmailSubscriptionViewModel DeleteEmaile(int id, int userId)
        {
            var emaile = _uow.MailSubscribe .GetById(id);
            if (emaile == null)
            {
                throw new Exception("004");
            }
            if (emaile.FlgStatus == 0)
            {
                throw new Exception("003");
            }
            emaile.FlgStatus = 0;
            emaile.UpdatedBy = userId;
            emaile.UpdatedOn = DateTime.Now.Date;
            _uow.MailSubscribe.Update(id, emaile);
            _uow.Commit();
            var typeModel = factory.Create(emaile);
            return typeModel;
        }

        public EmailSubscriptionViewModel EditEmaile(int id, EmailSubscriptionViewModel model, int userId)
        {
            if (model == null)
            {
                throw new Exception("005");
            }
            var emaile = _uow.MailSubscribe.GetById(id);
            if (emaile == null)
            {
                throw new Exception("004");
            }
            emaile.Email = model.Email;
            _uow.MailSubscribe .Update(id, emaile);
            _uow.Commit();
            var maileModel = factory.Create(emaile);
            return maileModel;
        }

        public EmailSubscriptionViewModel GetEmailById(int id)
        {
            var emaile = _uow.MailSubscribe.GetById(id);
            if (emaile == null)
            {

                throw new Exception("006");
            }
            var model = factory.Create(emaile);
            return model;
        }
    }
}
