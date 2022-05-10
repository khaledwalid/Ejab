
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
        IUserService _UserService;
        public AuthorizeController(UnitOfWork _uow, IUserService UserService)
        {
            UOW = _uow;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Token")]
        public IHttpActionResult Token([FromBody] LoginVM vm)
        {
            if (vm != null)
            {
                var user = UOW.User.GetAll().SingleOrDefault(x => x.FirstName+""+x.LastName == vm.username && x.Password == vm.password);
                if (user != null)
                {
                    switch (vm.grant_type)
                    {
                        case "user_web_auth"://web aut
                          //  UOW.SysLog.(ActionData.insert, " دخول المستخدم " + vm.username + " الي النظام   ", user.Id);
                            UOW.Commit();
                            return Ok(_UserService.GenerateToken(user));
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
