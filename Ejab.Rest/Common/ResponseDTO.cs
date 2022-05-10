using Ejab.Rest.CommonEmail;
using Ejab.Rest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace Ejab.Rest.Common
{
    /// <summary>
    /// Used to return a templated API response (status can be 200 in success or -1 in failure
    /// </summary>
    public class ResponseDTO
    {
        public ResponseDTO(ModelStateDictionary modelState)
        {
            var errorList = modelState
                .ToDictionary(kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());
            Status = -1;
            Errors = new List<Error>();
            foreach (var item in errorList.Values)
            {
                foreach (var valu  in item)
                {
                    var singleError = new Error(valu, lang);
                    Errors.Add(singleError);
                    AddToMessage(singleError.Message);
                }  
            }
        }

        private void AddToMessage(string msg)
        {
            if (!string.IsNullOrEmpty(Message))
                Message += ',';

            Message += msg;
        }

        /// <summary>
        /// will be used only in errors
        /// </summary>
        /// <param name="statusCodes"> A list of errors status codes</param>
        public ResponseDTO(params object[] codes)
        {
            this.Status = -1;
            Errors = new List<Error>();
            foreach (string code in codes)
            {
                var singleError = new Error(code, lang);
                Errors.Add(singleError);
                AddToMessage(singleError.Message);
            }
        }

        public ResponseDTO(object data)
        {
            this.Status = 200;
            this.Data = data;
            this.Message = ErrorMsg.SetMessage("000", lang);
        }

        public int Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public List<Error> Errors { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string lang { get { return GetLangFromHeader(); } }

        private string GetLangFromHeader()
        {
            if (HttpContext.Current.Request.Headers["Accept-Language"] != null)
                return HttpContext.Current.Request.Headers["Accept-Language"].ToString();
            else
                return "ar";
        }
    }
}