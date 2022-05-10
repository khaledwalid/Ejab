using Ejab.BAL.UnitOfWork;
using Ejab.DAl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ejab.BAL.Services;
using Ejab.BAL.Utility;
using System.Threading.Tasks;
using Ejab.DAl.Common;
using Ejab.Rest.Common;
using System.Web.Http.ModelBinding;
using Ejab.BAL.ModelViews;
using System.Net.Mail;
using System.Configuration;
using System.Net.Configuration;

namespace Ejab.Rest.Controllers
{
    [Authorize]
    [RoutePrefix("api/V1/Customer")]
    public class CustomerV1Controller : BaseController
    {
        ICustomerService _customerService;
        IUserService _userService;
        public CustomerV1Controller(IUserService UserService, ICustomerService cs)
        {
            _userService = UserService;
            _customerService = cs;
        }

        [HttpGet]
        [Route("")]
        public ResponseDTO Get()
        {
            try
            {
             
                var users = _customerService.AllUsers();
                if (users.Count() == 0)
                {
                    return new ResponseDTO("006");
                }
                return new ResponseDTO(users);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        [HttpGet]
        [Route("Users/{type}")]
        public ResponseDTO GetUsers(CustomerTypes type)
        {
            try
            {
                var users = _customerService.AllUsers();
                if (type == CustomerTypes.ServiceProvider)
                {
                    var allServiceProvider = _customerService.GetUsers(type);
                    if (allServiceProvider.Count() == 0)
                    {

                        return new ResponseDTO("006", "");
                    }
                    return new ResponseDTO(allServiceProvider);
                }

                if (type == CustomerTypes.Requester)
                {
                    var AllRequsters = _customerService.GetUsers(type);
                    if (AllRequsters.Count() == 0)
                    {
                        return new ResponseDTO("006", "");
                    }
                    return new ResponseDTO(AllRequsters);
                }
                return new ResponseDTO(users);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        public bool IsUserMobileAvialable(string Mobile)
        {
            return _customerService.IsUserMobileAvialable(Mobile);
        }
        public bool IsUserEmailAvialable(string Email)
        {
            return _customerService.IsUserEmailAvialable(Email);
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("ForgotPassword")]
        public ResponseDTO ForgotPassword(RecoveryPasswordModel recoveryModel)
        {
            try
            {
                if (!_customerService.PasswordRecovery(recoveryModel))
                {
                    return new ResponseDTO("007","");
                }
              
                    var samplePassword = EmailHelper.GeneratePassword(10);
                    string messgeBody = "Dear Our Customer " + recoveryModel.Email + "This Your New Password " + ":" + samplePassword;
                     EmailHelper.SendEmail(recoveryModel.Email, "Your Password Recovery", messgeBody);
                    var encryptedPassWord = Utility.Encrypt(samplePassword);
                    _customerService.UpdateUserPassword(recoveryModel.Email, encryptedPassWord);
                   string message = "Password Was Sent";
                    return new ResponseDTO(message);          

            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message,"");
            }

        }
        [HttpPost]
        [Route("ResetPassword")]
        public ResponseDTO ResetPassword(ResetPasswordViewModel resetpasswordModel)
        {
            try
            {
                var code = ResetPassword(resetpasswordModel);
                return new ResponseDTO(code.ToString(), "");
            }           
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        [HttpPost]
        [Route("ChangePassword")]
        public ResponseDTO ChangePassword(ChangePassWordModel changpasswordModel)
        {
            try
            {
                var model = _customerService.ChangePassword(changpasswordModel, _User.UserId);
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            { return new ResponseDTO(ex.Message, ""); }
          
        }

        [HttpPost]
        [Route("LogOut")]
        public ResponseDTO LogOut(LogOutViewModel model)
        {
            _customerService.LogOut( _User.UserId, model);
            return new ResponseDTO("");
        }

        [AllowAnonymous]
        [Route("UserLogin")]
        public ResponseDTO LogIn(LoginViewModel loginModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ResponseDTO(ModelState);
                }
                if (loginModel == null)
                {
                    return new ResponseDTO("005", "");
                }

                var model = _customerService.LogIn(loginModel);
                //   returnModel = new ResponseDTO(model);
                // returnModel.Message = ModelState.
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message,"");
            }
        }

        [HttpGet]
        [Route("Profile")]
        public ResponseDTO Profile()
        {
            try
            {
                var UserViewModel = _customerService.Profile(_User.UserId);
                return new ResponseDTO(UserViewModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message,"");
            }
        }
        //[AllowAnonymous]
        //[HttpPost]
        //[Route("UserPofile")]
        //public ResponseDTO CreateUserProfile(UserViewModel user)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            var returnModel = new ResponseDTO(ModelState);

        //            return returnModel;// new ResponseDTO(ModelState);
        //        }
        //        if (!_customerService.IsUserMobileAvialable(user.Mobile))
        //        {

        //            return new ResponseDTO("001", "");
        //        }
        //        if (!_customerService.IsUserEmailAvialable(user.Email))
        //        {
        //            return new ResponseDTO("002", "");
        //        }
        //        var result = _customerService.CreateUserProfile(user,file );
        //        return new ResponseDTO(result);

        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseDTO(ex.Message, "");
        //    }
        //}
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public ResponseDTO Register([FromBody]RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var returnModel = new ResponseDTO(ModelState);

                    return returnModel;// new ResponseDTO(ModelState);
                }
                if (model == null)
                {
                    return new ResponseDTO("006", "");
                }
                if (!_customerService.IsUserMobileAvialable(model.Mobile))
                {

                    return new ResponseDTO("001", "");
                }
                if (!_customerService.IsUserEmailAvialable(model.Email))
                {
                    return new ResponseDTO("002", "");
                }
                else
                {
                    var result = _customerService.Register(model);
                    return new ResponseDTO(result);
                }

            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("FaceBookLogin")]
        public ResponseDTO FaceBookLogin(FaceBookLoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var returnModel = new ResponseDTO(ModelState);

                    return returnModel;// new ResponseDTO(ModelState);
                }
                if (model == null)
                {
                    return new ResponseDTO("006", "");
                }             
               
                else
                {
                    var result = _customerService.FaceBookLogin(model);
                    return new ResponseDTO(result);
                }

            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message);
            }
        }
        [HttpGet]
        [Route("AllFaceBookUsers")]
        public ResponseDTO AllFaceBookUsers()
        {
            try
            {
                var users = _customerService.AllFaceBookUsers();
                if (users.Count() == 0)
                {
                    return new ResponseDTO("053");
                }
                return new ResponseDTO(users);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        [HttpGet]
        [Route("GetFaceBookUser/{FaceBookId}")]
        public ResponseDTO GetFaceBookUser(int FaceBookId)
        {
            try
            {
                var user = _customerService.GetFaceBookUser(FaceBookId);
                return new ResponseDTO(user);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        [Authorize]
        [HttpPut]
        [Route("EditUser")]
        public ResponseDTO Put(UserViewModel user)
        {
            try
           {
            //    if (!ModelState.IsValid)
            //    {
            //        var returnModel = new ResponseDTO(ModelState);
            //        return returnModel;
            //    }
                var UserViewModel = _customerService.Put(user, _User.UserId);
                return new ResponseDTO(UserViewModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        [Authorize]
        [HttpDelete]
        [Route("DeleteUser")]
        public ResponseDTO DeleteUser()
        {
            try
            {
                var UserViewModel = _customerService.DeleteUser(_User.UserId);
                return new ResponseDTO(UserViewModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }

        }


        [HttpPost]
        [Route("CreateCompany/{createdby}")]
        public ResponseDTO CreateCompany([FromBody]RegisterViewModel model,int createdby)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var returnModel = new ResponseDTO(ModelState);

                    return returnModel;// new ResponseDTO(ModelState);
                }
                if (model == null)
                {
                    return new ResponseDTO("006", "");
                }
                if (!_customerService.IsUserMobileAvialable(model.Mobile))
                {

                    return new ResponseDTO("001", "");
                }
                if (!_customerService.IsUserEmailAvialable(model.Email))
                {
                    return new ResponseDTO("002", "");
                }
                else
                {
                    var result = _customerService.CreateCompanyBranch(model, createdby);
                    return new ResponseDTO(result);
                }

            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        [HttpGet]
        [Route("GetCompanies/{userId}")]
        public ResponseDTO GetCompanies(int userId)
        {
            try
            {
                var user = _customerService.GetCompaniesAnderUser(userId);
                return new ResponseDTO(user);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        [Authorize]
        [HttpPut]
        [Route("EditUserCompany/{id}")]
        public ResponseDTO EditUserCompany(int id, RegisterViewModel user)
        {
            try
            {
                var companyUser = _customerService.EditCompanyBranch(id,user,_User.UserId );
                return new ResponseDTO(companyUser);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        [HttpDelete]
        [Route("DeleteUserCompany/{id}")]
        public ResponseDTO DeleteUserCompany(int id)
        {
            try
            {
                var companyUser = _customerService.deleteCompanyBranch(id, _User.UserId);
                return new ResponseDTO(companyUser);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        [Authorize]
        [HttpPut]
        [Route("EditUserImage")]
        public ResponseDTO EditUserImage(EditUserImageViewModel ImagePath)
        {
            try
            {
                //    if (!ModelState.IsValid)
                //    {
                //        var returnModel = new ResponseDTO(ModelState);
                //        return returnModel;
                //    }
                var UserViewModel = _customerService.EditUserImage(ImagePath, _User.UserId);
                return new ResponseDTO(UserViewModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        [HttpPost]
        [Route("DeactivateUser/{userId}")]
        public ResponseDTO DeactivateUser(int userId, bool stat)
        {
            try
            {
                if (stat==true)
                {

                }
                _customerService.DeActiveUser(userId, stat);
                return new ResponseDTO("");

            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }


        [Authorize]
        [HttpPut]
        [Route("EditUserMobile")]
        public ResponseDTO EditUserMobile( EditUserMobileViewModel model)
        {
            try
            {
               
                var UserViewModel = _customerService.EditMobile(_User.UserId, model, _User.UserId);
                return new ResponseDTO(UserViewModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        [Authorize]
        [HttpPost]
        [Route("RefreshToken")]
        public ResponseDTO RefreshToken(RefershTokenViewModel securityToken)
        {
            try
            {
               _customerService.RefershToken (_User.UserId, securityToken);
                return new ResponseDTO("");
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        [HttpGet]
        [Authorize]
        [Route("CheckUserActivation")]
        public ResponseDTO CheckUserActivation()
        {
            try
            {
                var isActive = _customerService.IsUserActive(_User.UserId);
                return new ResponseDTO(isActive);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
    }
}
