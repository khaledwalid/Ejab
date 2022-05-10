using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.AccessControl;
using System.Web;
using System.Web.Configuration;
using System.ServiceModel.Channels;
using System.Net;
using System.Net.NetworkInformation;

namespace Ejab.BAL.Helpers
{
    public static class ServerHelper
    {

        private static  string GetClientIp(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContext)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop;
                prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else
            {
                return null;
            }
        }
        public static IPAddress GetIPAddress(string hostName)
        {
            Ping ping = new Ping();
            var replay = ping.Send(hostName);

            if (replay.Status == IPStatus.Success)
            {
                return replay.Address;
            }
            return null;
        }

        public static string MapPathReverse(string fullServerPath)
        {
            return GetIPAddress(Dns.GetHostName()) + "/" + fullServerPath.Replace(HttpContext.Current.Request.PhysicalApplicationPath, String.Empty).Replace("\\","/");
          
        }
        public static string AdminMapPathReverse(string fullServerPath)
        {
            return GetIPAddress(Dns.GetHostName()) + "/" + fullServerPath.Replace(HttpContext.Current.Request.PhysicalApplicationPath, String.Empty).Replace("\\", "/");

        }

        private static  string[] validTypes = new[] { "image/gif", "image/jpeg", "image/pjpeg", "image/png" };

        public static  string[] ValidTypes
        {
            get
            {
                return validTypes;
            }
            set
            {
                validTypes = value;
            }
        }
        public class PhotoUpload
        {
            public string Url { get; set; }

            public HttpPostedFileBase Content { get; set; }
        }

        public static  string VirtualPath { get; set; }

        public static  HttpServerUtilityBase Mapper { get; set; }
        public static  PhotoUpload Photoup { get; set; }
        public static  string Upload(PhotoUpload photo)
        {
            if (photo.Content == null || photo.Content.ContentLength == 0)
            {
                return null;
            }

            if (!ValidTypes.Contains(photo.Content.ContentType))
            {
                throw new InvalidDataException("Please upload only an image of type GIF, JPG or PNG.");
            }
            else
            {
                var fileName = Path.GetFileName(photo.Content.FileName) ?? photo.Content.ContentType.Replace('/', '.');
                var physicalPath = Path.Combine(Mapper.MapPath(VirtualPath));
                if (!Directory.Exists(physicalPath))
                {
                    var directorySecurity = new DirectorySecurity();
                    directorySecurity.AddAccessRule(new FileSystemAccessRule(@"everyone", FileSystemRights.Read, AccessControlType.Allow));
                    directorySecurity.AddAccessRule(new FileSystemAccessRule(@"everyone", FileSystemRights.FullControl, AccessControlType.Allow));
                    Directory.CreateDirectory(physicalPath, directorySecurity);
                }
                photo.Content.SaveAs(physicalPath + fileName);

                photo.Url = Path.Combine(VirtualPath, fileName);

                return photo.Url;
            }
        }
    }
}