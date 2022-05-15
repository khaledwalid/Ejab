using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Ejab.BAL.CustomDTO;
using Ejab.DAl;
using Ejab.BAL.Reository;
using Ejab.DAl.Models;

namespace Ejab.BAL.Services
{
    public class UserService: IUserService
    {
        GenericRepository<User> _repo;
        public UserService()
        {
          
        }
        public TokenDTO GenerateToken(User user)
        {
            var _issued_on = System.DateTime.Now.Ticks.ToString();
            var _expires_on = System.DateTime.Now.AddDays(30).Ticks.ToString();
            string _roles = string.Empty;
            string[] _rolesArr = null;

            //if (user.SysUserRoles != null)
            //{
            //    int r_c = user.SysUserRoles.Count;
            //    _rolesArr = new string[r_c];
            //    for (int i = 0; i < r_c; i++)
            //    {
            //        string i_role = user.SysUserRoles.ElementAt(i).SysRole.Name;
            //        _rolesArr[i] = i_role;
            //        _roles += string.Format("'{0}'", i_role);
            //        if (i < (r_c - 1))
            //            _roles += ',';
            //    }
            //}

            string tokenData = string.Format("user_id:{0},username:'{1}',email:'{2}',issued_on:'{3}',expires_on:'{4}'",
                user.Id, user.FirstName+""+ user.LastName, user.Email, _issued_on, _expires_on);

            CustomDTO.TokenDTO dto = new CustomDTO.TokenDTO
            {
                token = Utility.Utility.Base64Encode(tokenData),
                user_id = user.Id,
                type = "user_auth",
                full_name = user.FirstName+"" + user.LastName,
                email = user.Email,
                issued_on = _issued_on,
                expires_on = _expires_on
            };

            return dto;
        }
        public User Verify(string username, string password)
        {
           var user= _repo.GetAll().SingleOrDefault(x => x.FirstName+""+ x.LastName == username && x.Password == password);

            if (user !=null )
            {
                throw new System.Exception("this  User Is Allready Exist");          
            }
            return user;
        }
    }
}
