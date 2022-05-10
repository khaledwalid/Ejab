using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.UI.Models
{
  public   class MyPrincipal : GenericPrincipal
    {
        public MyPrincipal(IIdentity id, string[] roles) : base(id, roles)
        {
            var frmsIdntity = (System.Web.Security.FormsIdentity)id;
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<MyPrincipalClone>(frmsIdntity.Ticket.UserData);
            this.Id = obj.Id;
            this.Name = obj.Name;
            this.Email = obj.Email;
            this.ProfileImge  = obj.ProfileImage ;
            this.Roles = obj.Roles;
           this.language  = obj.language ;
        }

        public MyPrincipal(IIdentity id, string[] roles, int Id, string name, string email,string profileImage ,string language) : base(id, roles)
        {
            this.Id = Id;
            this.Name = name;
            this.Email = email;
            this.ProfileImge = profileImage;
            Roles = roles;
            this.language = language;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ProfileImge { get; set; }
        public string language { get; set; }
        public string[] Roles { get; set; }
    }

    public class MyPrincipalClone
    {
        public MyPrincipalClone(int Id, string name, string email,string profileImage,string[] roles,string language)
        {
            this.Id = Id;
            this.Name = name;
            this.Email = email;
            this.ProfileImage = profileImage;
            Roles = roles;
            this.language = language;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ProfileImage { get; set; }
        public string language { get; set; }
        public string[] Roles { get; set; }
    }
}
