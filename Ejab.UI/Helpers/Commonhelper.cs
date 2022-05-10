using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Ejab.UI.Helpers
{
    public static  class Commonhelper
    {
        public static string ResolveServerUrl(string serverUrl, bool forceHttps)
        {
            if (serverUrl.IndexOf("://") > -1)
                return serverUrl;

            string newUrl = serverUrl;
            Uri originalUri = System.Web.HttpContext.Current.Request.Url;
            newUrl = (forceHttps ? "https" : originalUri.Scheme) +
                "://" + originalUri.Authority + newUrl;
            return newUrl;
        }
        public  static  string    ConvertDate(string  mydate)
        {
            CultureInfo arSA = new CultureInfo("ar-SA");       
            var dateValue = DateTime.ParseExact(mydate, "dd/MM/yyyy", arSA.DateTimeFormat, DateTimeStyles.AllowInnerWhite);
            return dateValue.ToString("dd/MM/yyyy");
        }

    }
}