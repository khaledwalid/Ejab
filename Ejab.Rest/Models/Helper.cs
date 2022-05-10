using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;



namespace Ejab.Rest.Models
{
    public static class Helper
    {
        //static List<LangMDL> Resources;

        ///// <summary>
        ///// Get physical and URL folder path for user by Id and module
        ///// </summary>
        ///// <param name="userId">User id</param>
        ///// <param name="module">(Optional) Orginization level to folder</param>
        ///// <param name="name">File name</param>
        ///// <returns></returns>
        //public static string GetFilePath(int userId, string name, ref string path, string module = null)
        //{
        //    string uploadFolder = string.Format("~/File_Uploads/{0}/", userId);

        //    if (!string.IsNullOrEmpty(module))
        //        uploadFolder = string.Format(uploadFolder + "{0}/", module);
        //    path = GetPhysicalPath(name, uploadFolder);

        //    return uploadFolder;
        //}

        //private static string GetPhysicalPath(string name, string uploadFolder)
        //{
        //    string path;
        //    var root = HttpContext.Current.Server.MapPath(uploadFolder);
        //    Directory.CreateDirectory(root);
        //    path = root + name;
        //    return path;
        //}

        //public static string GetFilePath(string name, ref string path, string module = "Profiles")
        //{
        //    string uploadFolder = "~/File_Uploads/";

        //    if (!string.IsNullOrEmpty(module))
        //        uploadFolder = string.Format(uploadFolder + "{0}/", module);

        //    path = GetPhysicalPath(name, uploadFolder);

        //    return uploadFolder;
        //}

        //public static bool DeleteFile(string filePath)
        //{
        //    try
        //    {
        //        var root = HttpContext.Current.Server.MapPath(filePath);
        //        File.Delete(root);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// Save image and construct an object
        ///// </summary>
        //public static void SaveImage(string photo, ref Image imgObj, ObjectState status, byte FlgType, byte FlgUserType)
        //{
        //    if (imgObj == null)
        //        imgObj = new Image();
        //    string coverPath = string.Empty;
        //    string filename = Guid.NewGuid().ToString();
        //    string photoURL = GetFilePath(filename, ref coverPath);
        //    string ext = BAL.Utility.Utility.Base64ToImage(photo, coverPath);
        //    imgObj.Path = photoURL + filename + ext;
        //    imgObj.FlgUserType = FlgUserType;
        //    imgObj.FlgType = FlgType;
        //    imgObj.ObjectState = status;
        //}
        //public static string GetSserializedSMSBody(string Mobile, string message)
        //{
        //    var myObject = new
        //    {
        //        Username = "966535353550",
        //        Password = "Tbm2017",
        //        Tagname = "BM",
        //        RecepientNumber = Mobile,
        //        VariableList = "",
        //        ReplacementList = "",
        //        Message = message,
        //        SendDateTime = 0,
        //        EnableDR = false
        //    };
        //    var serialized = Newtonsoft.Json.JsonConvert.SerializeObject(myObject).ToString();

        //    return serialized;
        //}
        ////public static string GetSserializedSMSBody(SysUser user, string message)
        ////{
        ////    var myObject = new
        ////    {
        ////        Username = "966535353550",
        ////        Password = "Tbm2017",
        ////        Tagname = "BM",
        ////        RecepientNumber = user.Mobile,
        ////        VariableList = "",
        ////        ReplacementList = "",
        ////        Message = message,
        ////        SendDateTime = 0,
        ////        EnableDR = false
        ////    };
        ////    var serialized = Newtonsoft.Json.JsonConvert.SerializeObject(myObject).ToString();

        ////    return serialized;
        ////}

        //public static string OpenReadIOFile(string name)
        //{
        //    var content = string.Empty;
        //    var physicalPath = HttpContext.Current.Server.MapPath(name);
        //    try
        //    {
        //        using (var fs = File.OpenRead(physicalPath))
        //        {
        //            StreamReader str = new StreamReader(fs);
        //            content = str.ReadToEnd();
        //        }
        //    }
        //    catch (Exception ex) { }
        //    return content;
        //}

        //internal static List<LangMDL> FillResources(string v)
        //{
        //    var content = OpenReadIOFile(v);
        //    var resFile = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LangMDL>>(content);
        //    Resources = resFile;

        //    return resFile;
        //}

        //public static string LocalMSG(string key)
        //{
        //    if (Resources == null)
        //        return "";

        //    if (!Resources.Exists(r => r.Key == key))
        //        return "";

        //    return Resources.Find(r => r.Key == key).Value;
        //}
    }
}