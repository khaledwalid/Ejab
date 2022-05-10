using Ejab.BAL.ModelViews;
using Ejab.BAL.Services;
using Ejab.BAL.UnitOfWork;
using Ejab.DAl;
using Ejab.Rest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Ejab.Rest.Controllers
{
    [Authorize]
    [RoutePrefix("api/V1/Complaint")]
    public class SuggestionsComplaintV1Controller : BaseController
    {

        IComplaintService _complaint;      
        public SuggestionsComplaintV1Controller(IComplaintService complaint)
        {
            this._complaint = complaint;          
        }
        [HttpGet]
        [Route("")]
        public ResponseDTO Get()
        {
            var comlaints = _complaint.GetAllComplaints().ToList();
            return new ResponseDTO(comlaints);
        }

        [HttpGet]
        [Route("{id}")]
        public ResponseDTO Get(int id)
        {
            var model = _complaint.getComplaintForUser(id).ToList();
            return new ResponseDTO(model);
        }
        [AllowAnonymous ]
        [HttpPost]
        [Route("AddComplaint")]
        public ResponseDTO Post(SuggestionsComplaintModelView model)
        {
            try
            {
                var MessageModel = _complaint.AddComplaint(model);
                return new ResponseDTO(MessageModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message,"");
            }
        }
        [AllowAnonymous]
        [HttpPut]
        [Route("EditComplaint/{id}")]
        public ResponseDTO Put(int id, SuggestionsComplaintModelView model)
        {
            try
            {
                var complaintModel = _complaint.EditComplaint(id, model,_User.UserId );
                return new ResponseDTO(complaintModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }          
        }

        [HttpDelete]
        [AllowAnonymous]
        [Route("DeleteComplaint/{id}")]
        public ResponseDTO DeleteComplaint(int id)
        {
            try
            {
                var complaintModel = _complaint.DeleteComplaint (id);
                return new ResponseDTO(complaintModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }

        }

    }
}
