
using Ejab.BAL.Services;
using Ejab.BAL.UnitOfWork;
using Ejab.Rest.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ejab.Rest.Controllers
{
    public class AuthorizeController : BaseController
    {
        private readonly IUserService _userService;
        public AuthorizeController(UnitOfWork _uow, IUserService userService)
        {
            _userService = userService;
            UOW = _uow;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Token")]
        public IHttpActionResult Token([FromBody] LoginVM vm)
        {
            if (vm != null)
            {
                var encryptedPassWord = BAL.Utility.Utility.Encrypt(vm.password);
                var user = UOW.User.GetAll().FirstOrDefault(x => x.Email == vm.Email && x.Password == encryptedPassWord);
                if (user != null)
                {
                    switch (vm.grant_type)
                    {
                        case "user_web_auth"://web aut
                          //  UOW.SysLog.(ActionData.insert, " دخول المستخدم " + vm.username + " الي النظام   ", user.Id);
                            UOW.Commit();
                            return Ok(_userService.GenerateToken(user));
                            break;
                        //case "user_auth"://mobile
                        //    UOW.SysLog.AddNewLog(ActionData.insert, " دخول المستخدم " + vm.username + " الي النظام   ", user.Id);
                        //    UOW.Commit();
                        //    return Ok(_UserService.GenerateToken(user));
                        //    break;
                    }

                    return BadRequest("Invalid grant type");
                }
                else
                    return BadRequest("Invalid login combination");
            }
            else
                return BadRequest("Invalid login combination");
        }
    }
}
