using Ejab.BAL.ModelViews.SmsMessage;
using Ejab.BAL.Services.SMS;
using Ejab.Rest.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;


namespace Ejab.Rest.Controllers
{
    [Authorize]
    [RoutePrefix("api/V1/Sms")]
    public class SmsMessageV1Controller : ApiController
    {
        ISmsService _smsService;
        public SmsMessageV1Controller(ISmsService smsService)
        {
            this._smsService = smsService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Validate")]
        public ResponseDTO CheckUserMobile(ValidateMobileViwModel validationModel)
        {
            try
            {
                if (_smsService.ValidateMobile(validationModel))
                {
                    throw new Exception("98");
                }
               if (validationModel.Email != null&&_smsService.ValidateEmail(validationModel.Email))
                {
                    throw new Exception("002");
                }

                else 
                {
                    var VerifyCode = _smsService.CreatVertyifyCode();
                    var baseUrl = "http://elec.sa/sms/api/sendsms.php";
                    //http://elec.sa/sms/api/sendsms.php?username=ejab&password=0593344883%20&message=test&numbers=965512365478&sender=test&unicode=e&return=json
                    string _createURL = baseUrl + '?' + "username=" + "ejab" + "&password=" + "0593344883" + "&message=" + "Naqlate verification Code Is :" + VerifyCode + "&numbers=" + validationModel.Mobile + "&sender=" + "Naqlate" + " &unicode=e&return=string"; 
                    var req = HttpWebRequest.Create(_createURL);
                    req.UseDefaultCredentials = true;
                    //req.Credentials = new NetworkCredential("ejab", "0593344883", "http://elec.sa/sms/SendSms/");
                    //req.Proxy.Credentials = new NetworkCredential("ejab", "0593344883", "http://elec.sa/sms/SendSms/");
                    req.Method = "GET";
                    HttpWebResponse myResp = (HttpWebResponse)req.GetResponse();
                    System.IO.StreamReader _responseStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());

                    string responseString = _responseStreamReader.ReadToEnd();
                    myResp.Close();
                    _responseStreamReader.Close();
                 
                  
                    if (myResp.StatusCode == HttpStatusCode.OK)
                    {
                        return new ResponseDTO(VerifyCode);
                      
                    }
                    else
                    {
                        return new ResponseDTO(responseString, "");
                    }
                  
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ReSendSms")]
        public ResponseDTO SendSms(SmsViewModel smsModel)
        {
            try
            {               
                    var VerifyCode = _smsService.CreatVertyifyCode();
                    var baseUrl = "http://elec.sa/sms/api/sendsms.php";
                    //http://elec.sa/sms/api/sendsms.php?username=ejab&password=0593344883%20&message=test&numbers=965512365478&sender=test&unicode=e&return=json
                    string _createURL = baseUrl + '?' + "username=" + "ejab" + "&password=" + "0593344883" + "&message=" + "Naqlate verification Code Is :" + VerifyCode + "&numbers=" + smsModel.number + "&sender=" + "Naqlate" + " &unicode=e&return=string";
                    var req = HttpWebRequest.Create(_createURL);
                    req.UseDefaultCredentials = true;
                    //req.Credentials = new NetworkCredential("ejab", "0593344883", "http://elec.sa/sms/SendSms/");
                    //req.Proxy.Credentials = new NetworkCredential("ejab", "0593344883", "http://elec.sa/sms/SendSms/");
                    req.Method = "GET";
                    HttpWebResponse myResp = (HttpWebResponse)req.GetResponse();
                    System.IO.StreamReader _responseStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());

                    string responseString = _responseStreamReader.ReadToEnd();
                    myResp.Close();
                    _responseStreamReader.Close();


                    if (myResp.StatusCode == HttpStatusCode.OK)
                    {
                        return new ResponseDTO(VerifyCode);

                    }
                    else
                    {
                        return new ResponseDTO(responseString, "");
                    }
            }
            catch (Exception ex)
            {
                return new ResponseDTO("100", "");
            }

        }

    }
}
