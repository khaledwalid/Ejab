using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Text;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Ejab.BAL.Utility
{
    public static  class Utility
    {
        public static string Encrypt(string clear_text)
        {
            using (System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create())
            {
                return GetMd5Hash(md5Hash, clear_text);
            }
        }
      

        public static bool Verify(string clear_text, string encrypted_text)
        {
            using (System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create())
            {
                return VerifyMd5Hash(md5Hash, clear_text, encrypted_text);
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        static string GetMd5Hash(System.Security.Cryptography.MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        static bool VerifyMd5Hash(System.Security.Cryptography.MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static string GetFolder(string fileFullPath)
        {
            int indx = fileFullPath.LastIndexOf('\\');
            return fileFullPath.Substring(0, indx);
        }
        internal static bool MoveFile(string sorsFl, string distFl)
        {
            try
            {
                string fldr = GetFolder(distFl);
                if (!Directory.Exists(fldr))
                    Directory.CreateDirectory(fldr);
                if (File.Exists(distFl))
                {
                    File.Delete(distFl);
                }
                File.Move(sorsFl, distFl);
                return true;
            }
            catch (Exception ex)
            {
                //string err = ex.Message;
                return false;
            }
        }
        //public static string GetRootPath()
        //{
            
        //    return HttpContext.Current.Server.MapPath("~");
        //}
        //internal static string GetFullPath(string moduleCK, string fileName)
        //{
        //    string rootPt = Utility.GetRootPath();
        //    string mdl = Utility.GetConfigKey(moduleCK);
        //    string flFullPath = string.Concat(rootPt, mdl, fileName);
        //    rootPt = mdl = null;
        //    return flFullPath;
        //}
        //public static string GetConfigKey(string key)
        //{
        //    return ConfigurationSettings.AppSettings[key];
        //}


        //public static bool SendEmail(List<UserAdmissionDTO> ToEmails)
        //{
        //    bool Issent = false;
        //    try
        //    {
        //        SmtpClient SmtpServer = new SmtpClient(ConfigurationSettings.AppSettings["smtp"]);
        //        string fromEmail = ConfigurationSettings.AppSettings["from"];
        //        string fromPW = ConfigurationSettings.AppSettings["pswrd"];
        //        SmtpServer.Port = int.Parse(ConfigurationSettings.AppSettings["port"]);
        //        SmtpServer.EnableSsl = true;
        //        SmtpServer.Credentials = new System.Net.NetworkCredential("diamodo@asgatech.com", fromPW);
        //        foreach (var item in ToEmails)
        //        {
        //            if (item.Email != null)
        //            {
        //                MailMessage mail = new MailMessage();
        //                mail.From = new MailAddress(fromEmail);
        //                mail.To.Add(item.Email);
        //                mail.Subject = ConfigurationSettings.AppSettings["Subject"];
        //                mail.Body = ConfigurationSettings.AppSettings["Body"] + "\n" + ConfigurationSettings.AppSettings["AppLink"] + "\n" + "Your username is: " + item.UserName + "\n" + "Your password is: " + item.Password;
        //                SmtpServer.Send(mail);
        //                Issent = true;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        Issent = false;
        //    }

        //    return Issent;
        //}
        //public static bool SendEmail(List<ReminderDTO> ToEmails)
        //{
        //    bool Issent = false;
        //    try
        //    {
        //        SmtpClient SmtpServer = new SmtpClient(ConfigurationSettings.AppSettings["smtp"]);
        //        string fromEmail = ConfigurationSettings.AppSettings["from"];
        //        string fromPW = ConfigurationSettings.AppSettings["pswrd"];
        //        SmtpServer.Port = int.Parse(ConfigurationSettings.AppSettings["port"]);
        //        SmtpServer.EnableSsl = true;
        //        SmtpServer.Credentials = new System.Net.NetworkCredential("diamodo@asgatech.com", fromPW);
        //        foreach (var item in ToEmails)
        //        {
        //            if (item.Email != null)
        //            {
        //                MailMessage mail = new MailMessage();
        //                mail.From = new MailAddress(fromEmail);
        //                mail.To.Add(item.Email);
        //                mail.Subject = ConfigurationSettings.AppSettings["SubjectReminder"];
        //                mail.Body = ConfigurationSettings.AppSettings["Body"] + "\n" + ConfigurationSettings.AppSettings["AppLink"] + "\n" + "Your username is: " + item.Email + "\n" + "Your password is: " + item.Password;
        //                SmtpServer.Send(mail);
        //                Issent = true;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        Issent = false;
        //    }
        //    return Issent;
        //}

    }
}
