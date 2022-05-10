using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;

namespace Ejab.Rest.Common
{
    public static class EmailHelper
    {
       
        public static string GeneratePassword(int length)
        {
            //string tempPassword = System.Web.Security.Membership.GeneratePassword(12, 1);
            string allowedLetterChars = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
            string allowedNumberChars = "23456789";
            char[] chars = new char[length];
            Random rd = new Random();

            bool useLetter = true;
            for (int i = 0; i < length; i++)
            {
                if (useLetter)
                {
                    chars[i] = allowedLetterChars[rd.Next(0, allowedLetterChars.Length)];
                    useLetter = false;
                }
                else
                {
                    chars[i] = allowedNumberChars[rd.Next(0, allowedNumberChars.Length)];
                    useLetter = true;
                }

            }

            return new string(chars);
            // return tempPassword;
        }
        public static void SendEmail(string To, String Subject, string MsgBody)
        {


            SmtpClient SmtpServer = new SmtpClient(ConfigurationSettings.AppSettings["smtp"]);
            string fromEmail = ConfigurationSettings.AppSettings["from"];
            string fromPW = ConfigurationSettings.AppSettings["pswrd"];
            SmtpServer.Port = int.Parse(ConfigurationSettings.AppSettings["port"]);
            SmtpServer.EnableSsl = true;
            SmtpServer.Credentials = new System.Net.NetworkCredential("diamodo@asgatech.com", fromPW);

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromEmail);
            mail.To.Add(To);
            mail.Subject = Subject;
            // mail.Body = ConfigurationSettings.AppSettings["Body"] + "\n" + ConfigurationSettings.AppSettings["AppLink"] + "\n" + "Your username is: " ;
            mail.Body = MsgBody;
            SmtpServer.Send(mail);


        }
        ////////////////////////////////////////////////////
        // var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
        // string from = smtpSection.From;
        // string username = smtpSection.Network.UserName;
        //string host = smtpSection.Network.Host.ToString();
        // int port =int.Parse ( smtpSection.Network.Port.ToString());
        // string password = smtpSection.Network.Password.ToString();
        // // here i will define these in confi
        // var m = new MailMessage()
        // {

        //     Subject = Subject ,
        //     Body = MsgBody,
        //     IsBodyHtml = true
        // };


        // m.To.Add(new MailAddress(To));
        // m.From = new MailAddress(from);
        // SmtpClient smtp = new SmtpClient
        // {
        //     Host = host,
        //     Port = port,
        //    EnableSsl = true,
        //     DeliveryMethod = SmtpDeliveryMethod.Network,
        //     UseDefaultCredentials = false,
        //     Credentials = new NetworkCredential("devloperhayam@gmail.com", "hasbyraby"),
        //     Timeout = 30 * 1000,
        // };
        //    try
        //    {
        //        smtp.Send(m);

        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message);              
        //    }


        //}


    }
}