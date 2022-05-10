using Ejab.BAL.ModelViews.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;


namespace Ejab.BAL.Helpers
{
    public class PushNotification
    {
        private string FireBase_URL = "https://fcm.googleapis.com/fcm/send";
        private string key_server;
        private string _to;
        private List<string> _registration_ids;
        private string _deviceType;
        public PushNotification()
        {

        }
        public PushNotification(String Key_Server, List<string> to, string deviceType)
        {
            this.key_server = Key_Server;
            this._deviceType = deviceType;
            this._registration_ids = to;
        }


        //public dynamic SendPush(NotificationViewModel obj)
        //{
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(FireBase_URL);
        //    request.Method = "POST";
        //    request.Headers.Add("Authorization", "key=" + this.key_server);
        //    request.ContentType = "application/json";
        //    var data = new
        //    {
        //       // to = this._to,
        //        deviceType = this._deviceType,
        //        //devicetype = deviceType,
        //        //senderId = SenderId,
        //        notification = new

        //        {
        //            Date = DateTime.Now,
        //            //senderId = SenderId,
        //            body = obj.Body,

        //            title = obj.Title

        //        },
        //        priority = "high",
        //        projectId = "naqelat-a5186"


        //    };
        //    string json = JsonConvert.SerializeObject(obj);
        //    //json = json.Replace("content_available", "content-available");
        //    byte[] byteArray = Encoding.UTF8.GetBytes(json);
        //    request.UseDefaultCredentials = true;
        //    request.PreAuthenticate = true;
        //    request.Credentials = CredentialCache.DefaultCredentials;
        //    request.ContentLength = byteArray.Length;
        //    Stream dataStream = request.GetRequestStream();
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    dataStream.Close();
        //    HttpWebResponse respuesta = (HttpWebResponse)request.GetResponse();
        //    if (respuesta.StatusCode == HttpStatusCode.Accepted || respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.Created)
        //    {
        //        StreamReader read = new StreamReader(respuesta.GetResponseStream());
        //        String result = read.ReadToEnd();
        //        read.Close();
        //        respuesta.Close();
        //        dynamic stuff = JsonConvert.DeserializeObject(result);

        //        return stuff;
        //    }
        //    else
        //    {
        //        throw new Exception(" Error Occured: " + respuesta.StatusCode);
        //    }

        //}


        public void PushNotifications(NotificationViewModel obj, string deviceType)
        {
            try
            {
                //  var key2 = "AAAAVnqfvjo:APA91bEvPV3Nm_FgiXzy6ErrB1bSXYDIrlVeS05fxwPkj_BxnQAUH0ni2c-9Ex_BDf2qNH5ou6zqdUuk52x9BXaNpJLGeqPZLk8ORWOYNguYc_PAgOzoiaz1USH7Wd09sl0JjWhE3lZP";
                var apiKey = "AAAAVnqfvjo:APA91bEvPV3Nm_FgiXzy6ErrB1bSXYDIrlVeS05fxwPkj_BxnQAUH0ni2c-9Ex_BDf2qNH5ou6zqdUuk52x9BXaNpJLGeqPZLk8ORWOYNguYc_PAgOzoiaz1USH7Wd09sl0JjWhE3lZP";
                var SenderId = "371424476730";              
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    //to = "cm78LICodFU:APA91bEn6pQdnRaB7gc0vq-JW_c8KwrXxL8H_blItc9DfCYhSfbyZUt4orIyF-ltOp3rz9KNLWlvyaLb65gkxI654V_jRBkOi0NZVoKFfOckizOIMsc7Mg848wqxMvbBvJ3NVa7B13mY",
                    registration_ids = obj.registration_ids,
                    devicetype = deviceType,
                    senderId = SenderId,
                  
                    notification = new
                    {
                        Date = DateTime.Now,
                        body = obj.Body,
                        // body = $"[{{'BodyArb': '{obj.BodyArb}' }} , {{'BodyEng': '{obj.BodyEng}' }}]",
                        title = obj.Title,
                        sound = "Enabled"

                    },
                    data = new
                    {
                        BodyArb = obj.BodyArb,
                        BodyEng = obj.BodyEng,
                        titleArb = obj.TitleArb,
                        titleEng = obj.TitleEng,
                        Type = obj.Type
                    },
                    priority = "high",
                    projectId = "naqelat-a5186",
                    mutable_content = true

                };
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.UseDefaultCredentials = true;
                tRequest.PreAuthenticate = true;
                tRequest.Credentials = CredentialCache.DefaultCredentials;
                tRequest.Headers.Add(string.Format("Authorization:key=" + apiKey));
                // tRequest.Headers.Add(string.Format("Sender: id={0}", SenderId));
                tRequest.ContentLength = byteArray.Length;
                tRequest.UseDefaultCredentials = true;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {

                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                File.WriteAllText("c:/sendmess.html", sResponseFromServer);
                                //   .Write(sResponseFromServer);
                                //   Label3.Text = sResponseFromServer;
                            }
                        }
                    }
                }
            }

            //}
            catch (Exception ex)
            {
                //ex.Message;
            }



        }

    }
}
