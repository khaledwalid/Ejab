using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ejab.BAL.ModelViews.AboutApplication;
using Ejab.BAL.UnitOfWork;
using Ejab.BAL.Common;

namespace Ejab.BAL.Services.AboutApp
{
    public class AboutAppService : IAboutAppService
    {
        IUnitOfWork _uow;
        ModelFactory factory;
        public AboutAppService(IUnitOfWork uow)
        {
            this._uow = uow;
            factory = new ModelFactory();

        }
        public AboutAppViewModel AddAboutApp(AboutAppViewModel model, int UserId)
        {
            var entity = factory.Parse(model);
            entity.FlgStatus = 1;
          
            entity.CreatedOn = DateTime.Now.Date;
            entity.CreatedBy = UserId;
            _uow.AboutApplication .Add(entity);
            _uow.Commit();
            var aboutUsModel = factory.Create(entity);

            return aboutUsModel;
        }

        public AboutAppViewModel DeleteAboutApp(int id, int UserId)
        {
            var about = _uow.AboutApplication .GetById(id);
            if (about == null)
            {
                throw new Exception("004");
            }
            if (about.FlgStatus == 0)
            {
                throw new Exception("003");
            }
            about.FlgStatus = 0;
            about.UpdatedBy = UserId;
            about.UpdatedOn = DateTime.Now.Date;
            _uow.AboutApplication .Update(id, about);
            _uow.Commit();
            var regionModel = factory.Create(about);
            return regionModel;
        }

        public AboutAppViewModel EditAboutApp(int id, AboutAppViewModel model, int UserId)
        {
            var aboutapp = _uow.AboutApplication .GetById(id);
            if (aboutapp == null)
            {
                throw new Exception("004");
            }
            if (model == null)
            {
                throw new Exception("005");
            }
            aboutapp.AboutApp  = model.AboutApp ;
            aboutapp.AboutAppEng = model.AboutAppEng;
            aboutapp.AppLink  = model.AppLink ;
            aboutapp.FaceBookLink = model.FaceBookLink;
            aboutapp .TwitterLink  = model.TwitterLink ;
            aboutapp.UpdatedBy = UserId;
            aboutapp.UpdatedOn = DateTime.Now;
            _uow.AboutApplication .Update(id, aboutapp);
            _uow.Commit();
            var regionModel = factory.Create(aboutapp);

            return regionModel;
        }

        public AboutAppViewModel Get(int id)
        {
            var aboutApp = _uow.AboutApplication .GetById(id);
            if (aboutApp == null)
            {

                throw new Exception("004");
            }
            var aboutModel = factory.Create(aboutApp);

            return aboutModel;
        }

        public AboutAppViewModel GetAll()
        {
            var aboutApp = _uow.AboutApplication .GetAll(x => x.FlgStatus == 1, null, "");
            var regionModel = aboutApp.Select(r => new AboutAppViewModel {Id=r.Id, AboutAppEng = r.AboutAppEng, AboutApp  = r.AboutApp , AppLink  = r.AppLink , TwitterLink  = r.TwitterLink , FaceBookLink  = r.FaceBookLink  }).FirstOrDefault();
            return regionModel;
        }
    }
}
