using Ejab.BAL.Common;
using Ejab.BAL.ModelViews;
using Ejab.BAL.UnitOfWork;
using Ejab.DAl.Common;
using Ejab.Rest.Common;
using Ejab.Rest.CommonEmail;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Ejab.Rest.Controllers
{
    [Authorize]
    [RoutePrefix("api/Images")]
    public class OfferImagesController : BaseController
    {
        IUnitOfWork _uow;
        ModelFactory factory;
        public OfferImagesController(IUnitOfWork uow)
        {
            this._uow = uow;
            factory = new ModelFactory();
        }

        static string base64String = null;
        public string ImageToBase64()
        {
            var path = HttpContext.Current.Server.MapPath("~/OffersImages/" + "" + DateTime.Today.ToString("ddMMyyyy") + "");
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
            }
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }
        public System.Drawing.Image Base64ToImage()
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            return image;
        }
      
        [HttpGet]
        [Route("")]
        public IHttpActionResult Images()
        {
            var Imges = _uow.OfferImages.GetAll(x => x.FlgStatus == 1, null, "Offer").ToList().Select(o => new { ImageTitle = o.ImageTitle, Descripion = o.ImageDescription, ImageUrl = o.ImageUrl, OfferId = o.OfferId });
            return Ok(Imges);
        }
      
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Images(int id)
        {
            try
            {
                var img = _uow.OfferImages.GetById(id);
                if (img == null)
                {
                    var myError = new Error
                    {
                        Code = "004",
                        Message = string.Format("Do Not Register,Not Found")
                    };
                    return new ErrorResult(myError, Request);

                }
                var ImagsModel = factory.Create(img);
                return Ok(ImagsModel);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }


        [HttpPost]
        [Route("CreateImage")]
        public IHttpActionResult CreateImage([FromBody]OfferImagesViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (model == null)
                {
                    var myError = new Error
                    {
                        Code = "001",
                        Message = string.Format("body can not be empty")
                    };
                    return new ErrorResult(myError, Request);
                }

                #region SaveImageAs64

                string converted = model.ImageUrl.Substring(model.ImageUrl.IndexOf(",") + 1);
                Image img = ImageHelper.Base64ToImage(converted);
                string filename = Guid.NewGuid().ToString() + model.ImageTitle;
                var path = HttpContext.Current.Server.MapPath("~/OffersImages/" + "" + DateTime.Today.ToString("ddMMyyyy") + filename);
                img.Save(path, ImageFormat.Jpeg);
                // here i save image
                // but araby want to save url on server
                var entity = factory.Parse(model);
                entity.ImageUrl = path;
                entity.ImageTitle = filename;
                entity.CreatedBy = _User.UserId;
                entity.CreatedOn = DateTime.Now;
                entity.FlgStatus = 1;
                entity.UpdatedBy = null;
                entity.UpdatedOn = null;
                _uow.OfferImages.Add(entity);
                _uow.Commit();
                return Ok(factory.Create(entity));
                #endregion
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

      
        [HttpPut]
        [Route("EditImage/{id}")]
        public IHttpActionResult EditImage(int id, [FromBody]OfferImagesViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (model == null)
                {
                    var myError = new Error
                    {
                        Code = "001",
                        Message = string.Format("body can not be empty")
                    };
                    return new ErrorResult(myError, Request);
                }
                var image = _uow.OfferImages.GetById(id);
                if (image == null)
                {
                    var myError = new Error
                    {
                        Code = "004",
                        Message = string.Format(" This Type  Do Not Registred in System")
                    };
                    return new ErrorResult(myError, Request);
                }
                string converted = model.ImageUrl.Substring(model.ImageUrl.IndexOf(",") + 1);
                Image img = ImageHelper.Base64ToImage(converted);
                string filename = Guid.NewGuid().ToString() + model.ImageTitle;
                var path = HttpContext.Current.Server.MapPath("~/OffersImages/" + "" + DateTime.Today.ToString("ddMMyyyy") + filename);
                img.Save(path, ImageFormat.Jpeg);
                image.OfferId = model.OfferId;
                image.ImageDescription = model.ImageDescription;
                image.ImageTitle = filename;
                image.ImageUrl = path;
                image.UpdatedBy = _User.UserId;
                image.UpdatedOn = DateTime.Now;
                _uow.OfferImages.Update(id, image);
                _uow.Commit();
                var typeModel = factory.Create(image);
                return Ok(typeModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
       
        [HttpDelete]
        [Route("DeleteImage/{id}")]
        public IHttpActionResult DeleteImage(int id)
        {
            try
            {
                var image = _uow.OfferImages.GetById(id);
                if (image == null)
                {
                    var myError = new Error
                    {
                        Code = "004",
                        Message = string.Format(" This Type  Do Not Registred in System")
                    };
                    return new ErrorResult(myError, Request);
                }
                image.FlgStatus = 0;
                _uow.OfferImages.Update(id, image);
                _uow.Commit();
                var imageModel = factory.Create(image);
                return Ok(imageModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
