using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ejab.BAL.ModelViews.AboutUs;
using Ejab.BAL.Common;
using Ejab.BAL.UnitOfWork;

namespace Ejab.BAL.Services.AboutUs
{
    public class AboutUsService : IAboutUs
    {
        IUnitOfWork _uow;
   
        ModelFactory factory;
        public AboutUsService(IUnitOfWork uow)
        {
            this._uow = uow;          
            factory = new ModelFactory();

        }
        public AboutUsViewModel AddAboutUs(AboutUsViewModel model, int UserId)
        {
                var entity = factory.Parse(model);
                entity.FlgStatus = 1;
            entity.Email = model.Email;
            entity.Region = model.Region ;
            entity.CreatedOn = DateTime.Now.Date;
                entity.CreatedBy = UserId;
                _uow.AboutUs .Add(entity);
                _uow.Commit();
                var aboutUsModel = factory.Create(entity);
              
                return aboutUsModel;
            }

        public AboutUsViewModel DeleteAboutUs(int  id, int UserId)
        {
            var about = _uow.AboutUs .GetById(id);
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
            _uow.AboutUs .Update(id, about);
            _uow.Commit();
            var regionModel = factory.Create(about);
            return regionModel;
        }

        public AboutUsViewModel EditAboutUs(int id, AboutUsViewModel model, int UserId)
        {
            var aboutus = _uow.AboutUs .GetById(id);
            if (aboutus == null)
            {
                throw new Exception("004");
            }
            if (model == null)
            {
                throw new Exception("005");
            }
            aboutus.Region  = model.Region ;
            aboutus.PostalCode  = model.PostalCode ;
            aboutus.Address  = model.Address ;
            aboutus.Longitude  = model.Longitude ;
            aboutus.latitude  = model.latitude ;
            aboutus.phone  = model.phone ;
            aboutus.Email  = model.Email;
            aboutus.UpdatedBy = UserId;
            aboutus.UpdatedOn = DateTime.Now;
            _uow.AboutUs .Update(id, aboutus);
            _uow.Commit();
            var regionModel = factory.Create(aboutus);
           
            return regionModel;
        }

        public AboutUsViewModel Get(int id)
        {
            var aboutUs = _uow.AboutUs .GetById(id);
            if (aboutUs == null)
            {

                throw new Exception("004");
            }
            var aboutModel = factory.Create(aboutUs);
          
            return aboutModel;
        }

        public AboutUsViewModel GetAll()
        {
            var regions = _uow.AboutUs.GetAll(x => x.FlgStatus == 1, null, "").ToList();
            var regionModel = regions.Select(r => new AboutUsViewModel  {Id=r.Id,Region =r.Region ,PostalCode=r.PostalCode   ,Address=r.Address,Longitude=r.Longitude,latitude=r.latitude,phone=r.phone,Email=r.Email,fax=r.fax  }).FirstOrDefault();
            return regionModel;
        }
    }
}
