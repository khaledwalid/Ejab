using Ejab.BAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ejab.BAL.CustomDTO;
using Ejab.DAl;
using Ejab.DAl.Common;
using Ejab.DAl.Models;
using Ejab.BAL.Utility;
using Ejab.BAL.ModelViews;
using Ejab.BAL.Common;
using System.Drawing;
using System.Web;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Configuration;
using Ejab.BAL.Helpers;
using System.Configuration;
using Ejab.BAL.ModelViews.Notification;
using Ejab.BAL.Services.Notification;

namespace Ejab.BAL.Services
{
    public class CustomerService : ICustomerService
    {
        IUnitOfWork _uow;
        IUserService _userService;
        ModelFactory factory;
        string path = ConfigurationManager.AppSettings["UserProfilepath"];
        string AdminPath = ConfigurationManager.AppSettings["AdminPath"];
        string OfferPath = ConfigurationManager.AppSettings["Offerpath"];
        IRuleService _iRuleService;
        INotificationService _iNotificationService;
        public CustomerService(IUnitOfWork uow, IUserService UserService, IRuleService IRuleService, INotificationService INotificationService)
        {
            this._uow = uow;
            this._userService = UserService;
            _iRuleService = IRuleService;
            _iNotificationService = INotificationService;
            factory = new ModelFactory();
        // path = ConfigurationManager.AppSettings["BaseServiceURL"].ToString() + "UsersProfile" + '/';
        //AdminPath = ConfigurationManager.AppSettings["AdminPath"].ToString() + "AdminProfileImgs" + '/';
        }
        public IEnumerable<UserViewModel> AllUsers()
        {
            var users = _uow.User.GetAll().ToList().Where(x => x.FlgStatus == 1).Select(U => new UserViewModel { Id = U.Id, FirstName = U.FirstName, LastName = U.LastName, Email = U.Email, Mobile = U.Mobile, CustomerType = U.CustomerType, Rating = (U.OverAllrating.HasValue) ? U.OverAllrating.Value : 0, Address = U.Address, AddressLatitude = U.AddressLatitude, AddressLongitude = U.AddressLongitude, ResponsiblePerson = U.ResponsiblePerson , FullName =U.FirstName +""+ U.LastName});
            return users;
        }
        public IEnumerable<UserViewModel> GetUsers(CustomerTypes type)
        {
            var users = _uow.User.GetAll().Select(U => new UserViewModel { Id = U.Id, FirstName = U.FirstName, LastName = U.LastName, Email = U.Email, Mobile = U.Mobile, CustomerType = U.CustomerType, Rating = (U.OverAllrating.HasValue) ? U.OverAllrating.Value : 0, Address = U.Address, AddressLatitude = U.AddressLatitude, AddressLongitude = U.AddressLongitude, ResponsiblePerson = U.ResponsiblePerson }); ;
            if (type == CustomerTypes.ServiceProvider)
            {
                var allServiceProvider = users.Where(x => x.FlgStatus == 1 && x.CustomerType == CustomerTypes.ServiceProvider).Select(U => new UserViewModel { Id = U.Id, FirstName = U.FirstName, LastName = U.LastName, Email = U.Email, Mobile = U.Mobile, CustomerType = U.CustomerType, Rating = U.Rating, Address = U.Address, AddressLatitude = U.AddressLatitude, AddressLongitude = U.AddressLongitude, ResponsiblePerson = U.ResponsiblePerson, FlgStatus = U.FlgStatus });

                return allServiceProvider.ToList();
            }
            if (type == CustomerTypes.Requester)
            {
                var AllRequsters = users.Where(x => x.FlgStatus == 1 && x.CustomerType == CustomerTypes.Requester).Select(U => new UserViewModel { Id = U.Id, FirstName = U.FirstName, LastName = U.LastName, Email = U.Email, Mobile = U.Mobile, CustomerType = U.CustomerType, Rating = U.Rating, Address = U.Address, AddressLatitude = U.AddressLatitude, AddressLongitude = U.AddressLongitude, ResponsiblePerson = U.ResponsiblePerson, FlgStatus = U.FlgStatus });

                return AllRequsters.ToList();
            }
            return users;
        }
        public bool PasswordRecovery(RecoveryPasswordModel recoveryModel)
        {
           return  _uow.User.GetAll (x=>x.FlgStatus == 1,null,"").Any(x => x.Email == recoveryModel.Email || x.Mobile == recoveryModel.Mobile );
           
        }
        public void UpdateUserPassword(string Email, string newPassword)
        {
            var user = _uow.User.SearchByPredicate(x => x.Email == Email && x.FlgStatus == 1);
            if (user != null)
            {
                user.Password = newPassword;
                user.UpdatedBy = user.Id;
                user.UpdatedOn = DateTime.Now;
                _uow.User.Update(user.Id, user);
                _uow.Commit();
            }

        }
        public Random ResetPassword(ResetPasswordViewModel resetpasswordModel)
        {
            var user = _uow.User.SearchByPredicate(x => x.Email == resetpasswordModel.Email && x.FlgStatus == 1);
            if (user == null)
            {
                return null;
            }
            Random random = new Random(15);
            return random;
        }
        public UserViewModel ChangePassword(ChangePassWordModel changpasswordModel, int id)
        {
            if (changpasswordModel.NewPassWord == null || string.IsNullOrEmpty(changpasswordModel.NewPassWord))
            {
                throw new Exception("71");
            }
            if (changpasswordModel.OldPassWord == null || string.IsNullOrEmpty(changpasswordModel.OldPassWord))
            {
                throw new Exception("70");
            }
          
            //  var userByMail=_uow.User.SearchByPredicate(x=>x.Email== changpasswordModel.e)
            var encryptedPassWord = Utility.Utility.Encrypt(changpasswordModel.NewPassWord);
            var encryptedOldPassWord = Utility.Utility.Encrypt(changpasswordModel.OldPassWord);
           
            var user = _uow.User.GetById(id);
            if (user.Password  != encryptedOldPassWord)
            {
                throw new Exception("72");
            }
            if (user == null)
            {
                throw new Exception("054");
            }
           
            bool verify;
            verify = Utility.Utility.Verify(changpasswordModel.OldPassWord, user.Password);
           
            if (verify)
            {

                user.Password = encryptedPassWord;
                _uow.User.Update(id, user);
                _uow.Commit();
            }

            var model = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Mobile = user.Mobile,
                Email = user.Email,
                Address = user.Address,
                AddressLatitude = user.AddressLatitude,
                AddressLongitude = user.AddressLongitude,
                CreatedBy = user.CreatedBy,
                CreatedOn = user.CreatedOn,
                CustomerType = user.CustomerType,
                FlgStatus = user.FlgStatus,

                ResponsiblePerson = user.ResponsiblePerson,
                UpdatedBy = user.UpdatedBy,
                UpdatedOn = user.UpdatedOn,
                Password = user.Password
            };
            return model;

        }
        public void LogOut( int UserId, LogOutViewModel model)
        {
            var userdevices = _uow.UserTokens.GetAll(u =>u.FlgStatus==1 && u.UserId == UserId && u.Fcmtoken==model.Token).FirstOrDefault();
            if (userdevices !=null)
            {
                _uow.UserTokens.Remove(userdevices);
                //userdevices.FlgStatus = 0;
                //userdevices.UpdatedBy = UserId;
                //userdevices.UpdatedOn = DateTime.Now;
                //    _uow.UserTokens.Update(userdevices.Id, userdevices);
                _uow.Commit();
            }
          
        }
        public UserViewModel LogIn(LoginViewModel loginModel)
        {
            bool verify;
            if (!ChecKEmail(loginModel.Email))
            {
                throw new Exception("007");
            }
            var userbyemaile = _uow.User.GetAll().FirstOrDefault(x => x.Email == loginModel.Email );
            if (userbyemaile.CustomerType != loginModel.CustomerTypes && userbyemaile.CustomerType == CustomerTypes.ServiceProvider)
            {
                throw new Exception("101");
            }
            if (userbyemaile.CustomerType != loginModel.CustomerTypes && userbyemaile.CustomerType==CustomerTypes.Requester )
            {
                throw new Exception("102");
            }
        
            if (userbyemaile != null && userbyemaile.IsActive == false)
            {
                throw new Exception("88");
            }
            else
            {
                var encryptedPassWord = Utility.Utility.Encrypt(loginModel.Password);
                var UnderServiceProvider = _uow.User.GetAll().SingleOrDefault(x => x.Email == loginModel.Email && x.IsActive == false && x.ResponsiblePerson != null);

                if (UnderServiceProvider != null)
                {
                    throw new Exception("88");
                }
                if (userbyemaile.Password != encryptedPassWord)
                {
                    throw new Exception("72");
                }

                if (userbyemaile != null)
                {

                    verify = Utility.Utility.Verify(loginModel.Password, userbyemaile.Password);

                    if (verify)
                    {
                        var user = _uow.User.GetAll().SingleOrDefault(x => x.IsActive == true && x.Email == loginModel.Email && x.Password == userbyemaile.Password && x.CustomerType == loginModel.CustomerTypes);
                        //if (!CheckPassword(loginModel.Email, loginModel.Password))
                        //{
                        //    throw new Exception("82");
                        //}
                        if (user != null)
                        {
                            var device = new UserToken
                            {
                                UserId = user .Id,
                                Fcmtoken = loginModel.Token,
                                CreatedBy = user.Id,
                                CreatedOn = DateTime.Now,
                                FlgStatus = 1,
                                UpdatedBy = null,
                                UpdatedOn = null
                            };
                            _uow.UserTokens.Add(device);
                            _uow.Commit();
                        }
                        var result = _userService.GenerateToken(user);

                        var userModel = factory.Create(user);
                        userModel.ProfileImgPath = user.ProfileImgPath != null ? path + user.ProfileImgPath : null;                       
                        userModel.DeviceToken = loginModel.Token;                       
                        userModel.UserToken = result.token;
                        userModel.TokenType = result.type;
                        userModel.Tokenissue = result.issued_on.ToString();
                        userModel.TokenExpire = result.expires_on.ToString();
                        userModel.UserRoles = result.roles;
                        var unreadmessages = _uow.Message.GetAll(x => x.FlgStatus == 1 && x.Status == false && x.ReciverId == user.Id, null, "").Count();
                        userModel.UnreadMessagesCount = unreadmessages;
                        return userModel;
                    }

                }
            }



                throw new Exception("008");

        }
        public UserViewModel Profile(int id)
        {
            var user = _uow.User.GetById(id);
            if (user.FlgStatus == 0)
            {
                throw new Exception("003");
            }
            if (user == null)
            {
                throw new Exception("007");
            }

           // var imagFullPath = HttpContext.Current.Server.MapPath("~/UsersProfile/");

            var UserViewModel = new UserViewModel();
            UserViewModel.ProfileImgPath = user.ProfileImgPath != null ? path + user.ProfileImgPath : null;

            UserViewModel.Id = id;
            UserViewModel.FirstName = user.FirstName;
            UserViewModel.LastName = user.LastName;
            UserViewModel.Email = user.Email;
            UserViewModel.Address = user.Address;
            UserViewModel.AddressLongitude = user.AddressLongitude;
            UserViewModel.AddressLatitude = user.AddressLatitude;
            UserViewModel.CustomerType = user.CustomerType;
            UserViewModel.Mobile = user.Mobile;

            UserViewModel.ResponsiblePerson = user.ResponsiblePerson;
            UserViewModel.CreatedBy = user.CreatedBy;
            UserViewModel.CreatedOn = user.CreatedOn;
            UserViewModel.FlgStatus = user.FlgStatus;
            UserViewModel.UpdatedBy = id;
            UserViewModel.UpdatedOn = DateTime.Now;
            return UserViewModel;
        }
        public AdminviewModel CreateUserProfile(AdminviewModel user, HttpPostedFileBase file, int userId, IList<RuleViewModel> rules, int[] selectedObjects)
        {
            var encryptedPassWord = Utility.Utility.Encrypt(user.Password);

            if (CheckAccountExist(user.Email,user.Password))
            {
                throw new Exception("039");
            }
           
            else
            {
                var UserData = new User();
                if (file != null)
                {

                    string ImageName = System.IO.Path.GetFileName(file.FileName);
                    UserData.ProfileImgPath = ImageName;
                }
                UserData.CreatedBy = userId;
                UserData.FirstName = user.FirstName;
                UserData.LastName = user.LastName;
                UserData.Email = user.Email;
                UserData.Mobile = user.Mobile;               
                UserData.Password = encryptedPassWord;
                UserData.IsAdmin = true;
                UserData.IsActive  = true ;
                UserData.CustomerType = CustomerTypes.Admin;
                UserData.UpdatedBy = null;
                UserData.CreatedOn = DateTime.Now;
                UserData.UpdatedOn = null;
                UserData.FlgStatus = 1;
                _uow.User.Add(UserData);
                _uow.Commit();
                if (selectedObjects !=null )
                {
                    foreach (var item in selectedObjects)
                    {
                        var rule = _uow.Rule.GetById(item);
                        UserData.UserRules.Add(rule);
                        rule.Users.Add(UserData);
                        _uow.Commit();
                    }


                }

                var usermodel = factory.Createadmin(UserData);
                usermodel.ProfileImgPath = usermodel.ProfileImgPath != null ?path+ usermodel.ProfileImgPath : null;
                return usermodel;
            }

        }
        public UserViewModel Register(RegisterViewModel model)
        {
            var encryptedPassWord = Utility.Utility.Encrypt(model.Password);
            if (model.CustomerType == 0)
            {
                throw new Exception("74");
            }
            if (Utility.Utility.Verify(model.ConfirmPassword, encryptedPassWord))
            {
                model.Password = encryptedPassWord;
            }
            var user = new User
            {
                Email = model.Email,
                IsActive = true,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = "",
                CustomerType = model.CustomerType,
                Mobile = model.Mobile,
                Password = model.Password,
                CreatedBy = 0,
                CreatedOn = DateTime.Now,
                FlgStatus = 1,
                UpdatedBy = null,
                UpdatedOn = null
            };
            _uow.User.Add(user);
            _uow.Commit();
            var userentity = factory.Create(user);
            var device = new UserToken
            {
              UserId= userentity.Id,
                Fcmtoken = model.Token !=""? model.Token:"none",
                CreatedBy = user.Id,
                CreatedOn = DateTime.Now,
                FlgStatus = 1,
                UpdatedBy = null,
                UpdatedOn = null
            };
            _uow.UserTokens.Add(device);
            _uow.Commit();

            var registermoddel = factory.Create(user);

            if (user.ProfileImgPath == "")
            {
                registermoddel.ProfileImgPath = null;
            }
            //var imagFullPath = HttpContext.Current.Server.MapPath("~/UsersProfile/" + user.ProfileImgPath);
            registermoddel.ProfileImgPath = user.ProfileImgPath != null ? path + user.ProfileImgPath : null;
            var result = _userService.GenerateToken(user);
            registermoddel.UserToken = result.token;
            registermoddel.TokenExpire = result.expires_on.ToString();
            registermoddel.TokenType = result.type;
            registermoddel.Tokenissue = result.issued_on.ToString();
            return registermoddel;
        }
        public UserViewModel FaceBookLogin(FaceBookLoginViewModel faceBookModel)
        {
            if (faceBookModel.RegisteredBy == RegisteredBy.FaceBookId && faceBookModel.FaceBookId == null)
            {
                throw new Exception("61");
            }
            if (faceBookModel.FaceBookId != null)
            {
                var userAccount = _uow.User.GetAll(x => x.FlgStatus == 1 && x.FaceBookId.Equals(faceBookModel.FaceBookId), null, "").FirstOrDefault();
                if (userAccount != null)
                {
                    var userAccountmoddel = factory.Create(userAccount);
                    // userAccountmoddel.FaceBookId = userAccount.FaceBookId;
                    userAccountmoddel.CustomerType = userAccount.CustomerType;
                    var result = _userService.GenerateToken(userAccount);
                    userAccountmoddel.UserToken = result.token;
                    userAccountmoddel.TokenExpire = result.expires_on.ToString();
                    userAccountmoddel.TokenType = result.type;
                    userAccountmoddel.Tokenissue = result.issued_on.ToString();
                    return userAccountmoddel;
                }
            }
            var usermailisExist = _uow.User.GetAll(x => x.FlgStatus == 1 && x.Email == faceBookModel.Email, null, "").FirstOrDefault();
            if (usermailisExist != null)
            {
                var existedmoddel = factory.Create(usermailisExist);
                // existedmoddel.FaceBookId = usermailisExist.FaceBookId;
                existedmoddel.CustomerType = usermailisExist.CustomerType;
                var result = _userService.GenerateToken(usermailisExist);
                existedmoddel.UserToken = result.token;
                existedmoddel.TokenExpire = result.expires_on.ToString();
                existedmoddel.TokenType = result.type;
                existedmoddel.Tokenissue = result.issued_on.ToString();
                return existedmoddel;
            }

            if (faceBookModel.RegisteredBy.Value == RegisteredBy.FaceBookId)
            {

                var user = new User
                {

                    Email = faceBookModel.Email,
                    FirstName = faceBookModel.FirstName,
                    LastName = faceBookModel.LastName,
                    Address = "",
                    Password = "",
                    Mobile = "",
                    CustomerType = faceBookModel.CustomerType,
                    RegisteredBy = RegisteredBy.FaceBookId,
                    FaceBookId = faceBookModel.FaceBookId.ToString(),
                    CreatedBy = 0,
                    CreatedOn = DateTime.Now,
                    FlgStatus = 1,
                    UpdatedBy = null,
                    UpdatedOn = null
                };
                _uow.User.Add(user);
                _uow.Commit();
                var device = new Device
                {
                    SerialNumber = faceBookModel.SN,
                    DeviceToken = faceBookModel.Token,
                    DeviceType = faceBookModel.DeviceType,
                    CreatedBy = user.Id,
                    CreatedOn = DateTime.Now,
                    FlgStatus = 1,
                    UpdatedBy = null,
                    UpdatedOn = null
                };

                _uow.Devices.Add(device);
                _uow.Commit();
                var UserDevice = new UserDevice
                {
                    CreatedBy = user.Id,
                    CreatedOn = DateTime.Now,
                    UserId = user.Id,
                    FlgStatus = 1,
                    UpdatedBy = null,
                    UpdatedOn = null,
                    DeviceId = device.Id
                };
                _uow.UserDevice.Add(UserDevice);
                _uow.Commit();
                var registermoddel = factory.Create(user);
                // registermoddel.FaceBookId = user.FaceBookId;
                registermoddel.CustomerType = user.CustomerType;
                var result = _userService.GenerateToken(user);
                registermoddel.UserToken = result.token;
                registermoddel.TokenExpire = result.expires_on.ToString();
                registermoddel.TokenType = result.type;
                registermoddel.Tokenissue = result.issued_on.ToString();
                return registermoddel;
            }
            // here will register by normal registration
            var normaluser = new User
            {
                Email = faceBookModel.Email,
                FirstName = faceBookModel.FirstName,
                LastName = faceBookModel.LastName,
                Address = "",
                Password = faceBookModel.Password,
                CustomerType = faceBookModel.CustomerType,
                FaceBookId = "",
                CreatedBy = 0,
                CreatedOn = DateTime.Now,
                FlgStatus = 1,
                UpdatedBy = null,
                UpdatedOn = null
            };
            _uow.User.Add(normaluser);
            _uow.Commit();
            var normaldevice = new Device
            {
                SerialNumber = faceBookModel.SN,
                DeviceToken = faceBookModel.Token,
                DeviceType = faceBookModel.DeviceType,
                CreatedBy = 0,
                CreatedOn = DateTime.Now,
                FlgStatus = 1,
                UpdatedBy = null,
                UpdatedOn = null
            };
            _uow.Devices.Add(normaldevice);
            _uow.Commit();
            var NormalUserDevice = new UserDevice
            {
                CreatedBy = 0,
                CreatedOn = DateTime.Now,
                UserId = normaluser.Id,
                FlgStatus = 1,
                UpdatedBy = null,
                UpdatedOn = null,
                DeviceId = normaldevice.Id
            };
            _uow.UserDevice.Add(NormalUserDevice);
            _uow.Commit();
            var normalResult = _userService.GenerateToken(normaluser);
            var normalmoddel = factory.Create(normaluser);
            normalmoddel.UserToken = normalResult.token;
            normalmoddel.TokenExpire = normalResult.expires_on.ToString();
            normalmoddel.TokenType = normalResult.type;
            normalmoddel.Tokenissue = normalResult.issued_on.ToString();
            return normalmoddel;
        }
        public UserViewModel Put(UserViewModel user, int id)
        {
            var existedUser = _uow.User.GetById(id);
            if (existedUser == null)
            {
                throw new Exception("004");

            }
            if (existedUser.FlgStatus == 0)
            {
                throw new Exception("003");
            }
            //  var result = _userService.GenerateToken(existedUser);

            if ( user.Email==null )
            {
                throw new Exception("022");
            }
            if (user.Mobile==null)
            {
                throw new Exception("020");
            }

            if (user.Password  == null)
            {
                existedUser.Password = existedUser.Password;
            }
            else
            {
                existedUser.Password = user.Password;

            }
            if (user.ProfileImgPath  == null)
            {
                existedUser.ProfileImgPath  = existedUser.ProfileImgPath;
            }
            else
            {
                existedUser.ProfileImgPath = user .ProfileImgPath != null ?  user .ProfileImgPath : null;                        

            }
            existedUser.Address = user.Address;
            existedUser.Mobile = user.Mobile;
            existedUser.FirstName = user.FirstName;
            existedUser.LastName = user.LastName;
            existedUser.Email = user.Email;
            existedUser.AddressLatitude = user.AddressLatitude;
            existedUser.AddressLongitude = user.AddressLongitude;
            existedUser.CustomerType = existedUser.CustomerType;
          

            //if (user.ProfileImgPath == null)
            //{
            //    existedUser.ProfileImgPath = "";

            //}
            //if (user.ProfileImgPath != null)
            //{
            //    var Phyiscalpath = HttpContext.Current.Server.MapPath("~/UsersProfile/");
            //    {
            //        Directory.CreateDirectory(Phyiscalpath);
            //    }
            //    string converted = user.ProfileImgPath.Substring(user.ProfileImgPath.IndexOf(",") + 1);
            //    Image img = ImageHelper.Base64ToImage(converted);
            //    string ext = Path.GetExtension(user.ProfileImgPath);
            //    string filename = Guid.NewGuid().ToString() + DateTime.Today.ToString("ddMMyyyy") + user.FirstName + "" + user.LastName + ".Jpeg";

            //    var path = Path.Combine(Phyiscalpath, filename);
            //    img.Save(path, ImageFormat.Jpeg);
            //    existedUser.ProfileImgPath = filename;
            //}

            existedUser.ResponsiblePerson = user.ResponsiblePerson;
            //  existedUser.ProfileName = filename;           
            existedUser.FlgStatus = 1;
            existedUser.UpdatedBy = id;
            if (!existedUser.UpdatedOn.HasValue)
            {
                existedUser.UpdatedOn = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            _uow.User.Update(id, existedUser);
            _uow.Commit();
            //if (existedUser.UserDevice == null)
            //{
            //    var device = new Devices();
            //    device.SerialNumber = user.SN;
            //    device.DeviceToken = user.DeviceToken;
            //    device.DeviceType = user.DeviceType;
            //    device.CreatedBy = id;
            //    device.CreatedOn = DateTime.Now;
            //    device.FlgStatus = 1;
            //    device.UpdatedBy = null;
            //    device.UpdatedOn = null;

            //    _uow.Devices.Add(device);
            //    _uow.Commit();
            //    var UserDevice = new UserDevice
            //    {
            //        CreatedBy = id,
            //        CreatedOn = DateTime.Now,
            //        UserId = existedUser.Id,
            //        FlgStatus = 1,
            //        UpdatedBy = null,
            //        UpdatedOn = null,
            //        DeviceId = device.Id
            //    };
            //    _uow.UserDevice.Add(UserDevice);
            //    _uow.Commit();
            //}
            //else if (existedUser.UserDevice != null)
            //{
            //    var userDevice = _uow.UserDevice.GetAll(x => x.FlgStatus == 1, null, "").Where(u => u.UserId == id);
            //    foreach (var device in userDevice)
            //    {
            //        // here i get every device by its id
            //        var devicedata = _uow.Devices.GetById(device.DeviceId);
            //        if (devicedata == null)
            //        {
            //            throw new Exception("015");
            //        }
            //        devicedata.SerialNumber = user.SN;
            //        devicedata.DeviceToken = user.DeviceToken;
            //        devicedata.DeviceType = user.DeviceType;
            //        _uow.Devices.Update(devicedata.Id, devicedata);
            //        _uow.Commit();
            //    }
            //}

            var UserViewModel = factory.Create(existedUser);
            //if (user .ProfileImgPath == "" || string.IsNullOrEmpty(user.ProfileImgPath))
            //{
            //    UserViewModel.ProfileImgPath = null; 
            //}
            //else
            //{
            //    var imagFullPath = HttpContext.Current.Server.MapPath("~/UsersProfile/" + existedUser.ProfileImgPath);
            //    UserViewModel.ProfileImgPath = ServerHelper.MapPathReverse(imagFullPath);
            //}
            UserViewModel.ProfileImgPath = UserViewModel.ProfileImgPath != null ?existedUser.ProfileImgPath : null;
            var result = _userService.GenerateToken(existedUser);
            UserViewModel.UserToken = result.token;
            UserViewModel.TokenExpire = result.expires_on.ToString();
            UserViewModel.TokenType = result.type;
            UserViewModel.Tokenissue = result.issued_on.ToString();
            return UserViewModel;
        }
        public UserViewModel DeleteUser(int id)
        {
            var existedUser = _uow.User.GetAll(x=>x.FlgStatus==1).FirstOrDefault(x=>x.Id==id);
            if (existedUser == null)
            {
                throw new Exception("004");
            }
            if (existedUser.FlgStatus == 0)
            {
                throw new Exception("003");
            }

            //  existedUser.FlgStatus = 0;
            //existedUser.UpdatedBy = 1;
            //existedUser.UpdatedOn = DateTime.Now;
            //if (IsReated(id))
            //{
            //    throw new Exception("109");
            //}
            if (existedUser.AcceptedBy !=null)
            {

              var acceptedOffers=  _uow.AcceptOffer.GetAll(x => x.AcceptedUserId == id).ToList();
                foreach (var item in acceptedOffers)
                {
                    _uow.AcceptOffer.Delete(item.Id, item);
                    _uow.Commit();
                }
                
            }
            if (existedUser.NotificationReciver != null)
            {

                var noties = _uow.Notification .GetAll(x => x.ReceiverId == id).ToList();
                foreach (var noty in noties)
                {
                    _uow.Notification.Delete(noty.Id, noty);
                    _uow.Commit();
                }

            }
            if (existedUser.NotificationSender!= null)
            {

                var noties = _uow.Notification.GetAll(x => x.SenderId == id).ToList();
                foreach (var noty in noties)
                {
                    _uow.Notification.Delete(noty.Id, noty);
                    _uow.Commit();
                }

            }
            if (existedUser.NotificationSender != null)
            {

                var noties = _uow.Notification.GetAll(x => x.SenderId == id).ToList();
                foreach (var noty in noties)
                {
                    _uow.Notification.Delete(noty.Id, noty);
                    _uow.Commit();
                }

            }
            if (existedUser.ServiceRequest != null)
            {

                var rating = _uow.Rating.GetAll(x => x.ServiceRequestId == id).ToList();
                foreach (var r in rating)
                {
                    _uow.Rating.Delete(r.Id, r);
                    _uow.Commit();
                }

            }
            var requesterrating = _uow.Rating.GetAll(x => x.ServiceProviderId == id).ToList();
            foreach (var r in requesterrating)
            {
                _uow.Rating.Delete(r.Id, r);
                _uow.Commit();
            }
            if (existedUser.ServiceProviderId != null)
            {

                var rating = _uow.Rating.GetAll(x => x.ServiceProviderId == id).ToList();
                foreach (var r in rating)
                {
                    _uow.Rating.Delete(r.Id, r);
                    _uow.Commit();
                }

            }
            if (existedUser.Offers != null)
            {

                var Offers = _uow.Offer.GetAll(x => x.UserId == id).ToList();
                foreach (var offer in Offers)
                {
                    if (existedUser.OfferDetails != null)
                    {

                        var offerdetiales = _uow.OfferDetail.GetAll(x => x.OfferId == offer.Id).ToList();
                        foreach (var detiles in offerdetiales)
                        {
                            _uow.OfferDetail.Delete(detiles.Id, detiles);
                            _uow.Commit();
                        }

                    }
                   
                    if (offer.Messages != null)
                    {

                        var offermessage = _uow.Message.GetAll(x => x.OfferId == offer.Id).ToList();
                        foreach (var mess in offermessage)
                        {
                            _uow.Message.Delete(mess.Id, mess);
                            _uow.Commit();
                        }

                    }
                    _uow.Offer.Delete(offer.Id, offer);
                    _uow.Commit();
                }

            }
            var propsales = _uow.ProposalPrice.GetAll(x => x.ServiceProviderId == id).ToList();
            foreach (var pp in propsales)
            {
                _uow.ProposalPrice .Delete(pp.Id, pp);
                _uow.Commit();
            }
            if (existedUser.Requests != null)
            {

                var requests = _uow.Request.GetAll(x => x.RequesterId == id).ToList();
                foreach (var req in requests)
                {
                   
                    if (req.ProposalPrices != null)
                    {

                        var pps = _uow.ProposalPrice.GetAll(x => x.ReqestId  == req.Id).ToList();
                        foreach (var pro in pps)
                        {
                            _uow.ProposalPrice.Delete(pro.Id, pro);
                            _uow.Commit();

                        }

                    }
                    if (req.Messages != null)
                    {

                        var requestmessage = _uow.Message.GetAll(x => x.RequestId == req.Id).ToList();
                        foreach (var mess in requestmessage)
                        {
                            _uow.Message.Delete(mess.Id, mess);
                            _uow.Commit();
                        }

                    }
                    if (req.RequestDetailes != null)
                    {

                        var detailes = _uow.RequestDetaile.GetAll(x => x.RequestId == req.Id).ToList();
                        foreach (var det in detailes)
                        {
                            if (det.RequestDetailesPrices != null)
                            {

                                var prices = _uow.RequestDetailesPrice.GetAll(x => x.RequestDetaileId == det.Id).ToList();
                                foreach (var pr  in prices)
                                {
                                    _uow.RequestDetailesPrice.Delete(pr.Id, pr);
                                    _uow.Commit();

                                }

                            }
                            _uow.RequestDetaile.Delete(det.Id, det);
                            _uow.Commit();

                        }

                    }
                   
                    _uow.Request.Delete(req.Id, req);
                    _uow.Commit();

                }

            }
          
            if (existedUser.Interests != null)
            {

                var interestes = _uow.Interest.GetAll(x => x.UserId == id).ToList();
                foreach (var interst in interestes)
                {
                    _uow.Interest.Delete(interst.Id, interst);
                    _uow.Commit();
                }

            }
            if (existedUser.Sender != null)
            {

                var mess = _uow.Message.GetAll(x => x.SenderId == id).ToList();
                foreach (var m in mess)
                {
                    _uow.Message.Delete(m.Id, m);
                    _uow.Commit();
                }

            }
           
            if (existedUser.Reciver != null)
            {

                var message = _uow.Message.GetAll(x => x.ReciverId == id).ToList();
                foreach (var m in message)
                {
                    _uow.Message.Delete(m.Id, m);
                    _uow.Commit();
                }

            }
            _uow.User.Delete(id, existedUser);
            _uow.Commit();
            if (existedUser.UserDevices != null)
            {
                var userDevice = _uow.UserTokens.GetAll(x => x.FlgStatus == 1, null, "").Where(u => u.UserId == existedUser.Id).ToList();
                if (userDevice.Count() >0)
                {
                    foreach (var device in userDevice.ToList())
                    {
                        // here i get every device by its id
                        var devicedata = _uow.UserTokens.GetById(device.Id);
                        _uow.UserTokens.Delete(device.Id, devicedata);
                        _uow.Commit();
                    }
                }
              
            }
            var UserViewModel = new UserViewModel();
            UserViewModel.Id = existedUser.Id;
            UserViewModel.FirstName = existedUser.FirstName;
            UserViewModel.LastName = existedUser.LastName;
            UserViewModel.Email = existedUser.Email;
            UserViewModel.Address = existedUser.Address;
            UserViewModel.AddressLongitude = existedUser.AddressLongitude;
            UserViewModel.AddressLatitude = existedUser.AddressLatitude;
            UserViewModel.CustomerType = existedUser.CustomerType;
            UserViewModel.Mobile = existedUser.Mobile;

            UserViewModel.ResponsiblePerson = existedUser.ResponsiblePerson;
            UserViewModel.CreatedBy = existedUser.CreatedBy;
            UserViewModel.CreatedOn = existedUser.CreatedOn;
            UserViewModel.FlgStatus = existedUser.FlgStatus;
            UserViewModel.UpdatedBy = id;
            UserViewModel.UpdatedOn = DateTime.Now;
            return UserViewModel;
        }
        public bool IsUserMobileAvialable(string Mobile)
        {
            return !_uow.User.GetAll().Any(m => m.Mobile == Mobile && m.FlgStatus==1);
        }
        public bool IsUserEmailAvialable(string Email)
        {
            return !_uow.User.GetAll().Any(m => m.Email == Email);
        }
        public IEnumerable<object> AllFaceBookUsers()
        {
            var FaceBookUsers = _uow.User.GetAll(x => x.FlgStatus == 1 && x.RegisteredBy == RegisteredBy.FaceBookId).Select(FU => new { FirstName = FU.FirstName, LastName = FU.LastName, Email = FU.Email, Mobile = FU.Mobile, CustomerType = FU.CustomerType });
            if (FaceBookUsers == null)
            {
                throw new Exception("053");
            }
            return FaceBookUsers;
        }
        public IEnumerable<AdminviewModel> AdminUsers(string search, int? page)
        {
            //var imagFullPath = HttpContext.Current.Server.MapPath("~/AdminProfileImgs/");

            if (search == null)
            {
                return _uow.User.GetAll(x => x.FlgStatus == 1 && x.IsAdmin == true).OrderByDescending(x => x.FirstName).ThenByDescending(x => x.LastName).ToList().Select(U => new AdminviewModel { Id = U.Id, FirstName = U.FirstName, LastName = U.LastName, Email = U.Email, Mobile = U.Mobile, CustomerType = U.CustomerType, Address = U.Address, AddressLatitude = U.AddressLatitude, AddressLongitude = U.AddressLongitude, ProfileImgPath = U.ProfileImgPath != null ? AdminPath + U.ProfileImgPath : null, ExistedRules = U.UserRules.Select(r => new RuleViewModel { Id = r.Id, Name = r.Name, Description = r.Description, DescriptionEng = r.DescriptionEng }) });
            }
            return _uow.User.GetAll(x => x.FlgStatus == 1 && x.IsAdmin == true).OrderByDescending(x => x.FirstName).ThenByDescending(x => x.LastName).ToList()
            .Select(U => new AdminviewModel { Id = U.Id, FirstName = U.FirstName, LastName = U.LastName, Email = U.Email, Mobile = U.Mobile, CustomerType = U.CustomerType, Address = U.Address, AddressLatitude = U.AddressLatitude, AddressLongitude = U.AddressLongitude, ProfileImgPath = U.ProfileImgPath != null ? AdminPath + U.ProfileImgPath : null, ExistedRules = U.UserRules.Select(r => new RuleViewModel { Id = r.Id, Name = r.Name, Description = r.Description, DescriptionEng = r.DescriptionEng }) })
            .Where(x => x.FirstName.Contains(search)  || x.FirstName.StartsWith(search) || x.Email.Equals(search) || x.Mobile.Equals(search));

        }
        public object GetFaceBookUser(int FaceBookId)
        {
            var FaceBookUser = _uow.User.GetAll(x => x.FlgStatus == 1 && x.FaceBookId.Equals(FaceBookId)).Select(FU => new { FirstName = FU.FirstName, LastName = FU.LastName, Email = FU.Email, Mobile = FU.Mobile, CustomerType = FU.CustomerType });
            if (FaceBookUser == null)
            {
                throw new Exception("006");
            }
            return FaceBookUser;
        }
        public UserViewModel GetUser(int id)
        {
            var user = _uow.User.GetById(id);
            var model = factory.Create(user);
            return model;
        }
        public AdminviewModel EditAdminData(AdminviewModel user, int id, HttpPostedFileBase file, IList<RuleViewModel> rules, int[] selectedObjects)
        {
            var existedUser = _uow.User.GetAll(x=>x.Id== id).FirstOrDefault();
            if (existedUser == null)
            {
                throw new Exception("004");

            }
            if (existedUser.FlgStatus == 0)
            {
                throw new Exception("003");
            }

            if (file != null && file.ContentLength > 0)
            {
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                existedUser.ProfileImgPath = ImageName;
            }
            if (user.Password !=null )
            {
                var encryptedPassWord = Utility.Utility.Encrypt(user.Password);
                existedUser.Password = encryptedPassWord;
            }
            if (user.Password == null)
            {
             
                existedUser.Password = existedUser.Password;
            }
            existedUser.Mobile = user.Mobile;
            existedUser.FirstName = user.FirstName;
            existedUser.LastName = user.LastName;
            existedUser.Email = user.Email;
          
            existedUser.CustomerType = CustomerTypes.Admin;
            existedUser.IsAdmin = true;
            existedUser.FlgStatus = 1;
            existedUser.UpdatedBy = id;
            if (!existedUser.UpdatedOn.HasValue)
            {
                existedUser.UpdatedOn = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            _uow.User.Update(id, existedUser);
            _uow.Commit();
            var existed = _iRuleService.ExistedRules(id).ToList();
            foreach (var item in existed)
            {
                var rule = _uow.Rule.GetById(item.Id);
                existedUser.UserRules.Remove(rule);
                rule.Users.Remove(existedUser);
                _uow.Commit();
            }
            if (selectedObjects !=null)
            {
          
            foreach (var item in selectedObjects)
            {
                var rule = _uow.Rule.GetById(item);
                existedUser.UserRules.Add(rule);
                rule.Users.Add(existedUser);
                _uow.Commit();
            }
            }

            var UserViewModel = factory.Createadmin(existedUser);
            return UserViewModel;
        }
        public UserViewModel AdminLogIn(AdminLogin Adminlogin)
        {
            bool verify;
            var imagFullPath = HttpContext.Current.Server.MapPath("~/AdminProfileImgs/");
            var userbyemaile = _uow.User.GetAll(x=>x.FlgStatus==1,null,"").Where (x => x.Email == Adminlogin.Email &&x.CustomerType == CustomerTypes.Admin && x.IsActive==true ).SingleOrDefault();
            //if (userbyemaile == null)
            //{
            //    throw new Exception("007");
            //}
            if (userbyemaile != null)
            {
                if (Adminlogin.Password !=null )
                {
                    verify = Utility.Utility.Verify(Adminlogin.Password, userbyemaile.Password);
                    if (verify)
                    {
                        var user = _uow.User.GetAll(x => x.FlgStatus == 1, null, "").SingleOrDefault(x => x.Email == Adminlogin.Email && x.CustomerType == CustomerTypes.Admin && x.Password == userbyemaile.Password && x.IsActive == true);
                        if (checkUserdata(Adminlogin.Email, userbyemaile.Password))
                        {
                            var currentAdmin = factory.Create(user);
                            currentAdmin.ProfileImgPath = user.ProfileImgPath != null ? AdminPath + user.ProfileImgPath : null;
                            return currentAdmin;

                        }

                    }
                }
              
               
                }

            return null;
        }
        public UserViewModel AdminProfile(int id)
        {
            var user = _uow.User.GetById(id);
            if (user.FlgStatus == 0)
            {
                throw new Exception("003");
            }
            if (user == null)
            {
                throw new Exception("007");
            }
            var imagFullPath = HttpContext.Current.Server.MapPath("~/AdminProfileImgs/");
            var UserViewModel = new UserViewModel();
            UserViewModel.Id = id;
            UserViewModel.FirstName = user.FirstName;
            UserViewModel.LastName = user.LastName;
            UserViewModel.Email = user.Email;
            UserViewModel.CustomerType = CustomerTypes.Admin;
            UserViewModel.Mobile = user.Mobile;

            UserViewModel.ProfileImgPath = ServerHelper.MapPathReverse(imagFullPath + user.ProfileImgPath);
            UserViewModel.CreatedBy = user.CreatedBy;
            UserViewModel.CreatedOn = user.CreatedOn;
            UserViewModel.FlgStatus = user.FlgStatus;
            UserViewModel.UpdatedBy = id;
            UserViewModel.UpdatedOn = DateTime.Now;
            return UserViewModel;
        }

        public UserViewModel CreateCompanyBranch(RegisterViewModel model, int CreatedBy)
        {
            var ServiceProviderUser = _uow.User.GetById(CreatedBy);
            if (ServiceProviderUser.CustomerType == CustomerTypes.Requester)
            {
                throw new Exception("014");
            }
            var encryptedPassWord = Utility.Utility.Encrypt(model.Password);
            if (Utility.Utility.Verify(model.ConfirmPassword, encryptedPassWord))
            {
                model.Password = encryptedPassWord;
            }
            var user = new User
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsActive = true,
                Address = "",
                CustomerType = CustomerTypes.ServiceProvider,
                Mobile = model.Mobile,
                Password = model.Password,
                ResponsiblePerson = model.ResponsiblePerson,
                CreatedBy = CreatedBy,
                CreatedOn = DateTime.Now,
                FlgStatus = 1,
                UpdatedBy = null,
                UpdatedOn = null
            };
            _uow.User.Add(user);
            _uow.Commit();

            var device = new UserToken
            {
                Fcmtoken = model.Token,
                UserId = user.Id,
               CreatedBy = user.Id,
                CreatedOn = DateTime.Now,
                FlgStatus = 1,
                UpdatedBy = null,
                UpdatedOn = null
            };

            _uow.UserTokens.Add(device);
            _uow.Commit();
           
            var registermoddel = factory.Create(user);
            registermoddel.CreatedBy = CreatedBy;
    
            var result = _userService.GenerateToken(user);
            registermoddel.UserToken = result.token;
            registermoddel.TokenExpire = result.expires_on.ToString();
            registermoddel.TokenType = result.type;
            registermoddel.Tokenissue = result.issued_on.ToString();
            return registermoddel;
        }
        public IEnumerable<UserViewModel> GetCompaniesAnderUser(int CreatedBy)
        {
            var Companies = _uow.User.GetAll(x => x.FlgStatus == 1 && x.CreatedBy == CreatedBy, null, "").Select(U => new UserViewModel { Id = U.Id, FirstName = U.FirstName, LastName = U.LastName, Email = U.Email, Mobile = U.Mobile, Address = U.Address, ResponsiblePerson = U.ResponsiblePerson, CustomerType = U.CustomerType, IsActive = U.IsActive });
            //if (Companies.Count()==0)
            //{
            //    throw new Exception("64");
            //}
            return Companies;
        }

        public UserViewModel EditCompanyBranch(int id, RegisterViewModel user, int userid)
        {

            var existedUser = _uow.User.GetById(id);
            if (existedUser == null)
            {
                throw new Exception("004");

            }
            if (existedUser.FlgStatus == 0)
            {
                throw new Exception("003");
            }
            //  var result = _userService.GenerateToken(existedUser);

           
            //existedUser.Address = user.Address;
            existedUser.Mobile = user.Mobile;
            existedUser.FirstName = user.FirstName;
            existedUser.LastName = user.LastName;
            existedUser.Email = user.Email;
            if (user.Password == null)
            {
                existedUser.Password = existedUser.Password;
            }
            else
            {
                var encryptedPassWord = Utility.Utility.Encrypt(user.Password);
                existedUser.Password = encryptedPassWord;

            }
            if (user.ConfirmPassword  == null)
            {
                user.ConfirmPassword = existedUser.Password;
            }
            else
            {
                user.ConfirmPassword = user.ConfirmPassword;
            }

            var Phyiscalpath = HttpContext.Current.Server.MapPath("~/UsersProfile/");
            {
                Directory.CreateDirectory(Phyiscalpath);
            }
            //if (user.ProfileImgPath != null || user.ProfileImgPath.Length > 0)
            //{
            //    string converted = user.ProfileImgPath.Substring(user.ProfileImgPath.IndexOf(",") + 1);
            //    Image img = ImageHelper.Base64ToImage(converted);
            //    string ext = Path.GetExtension(user.ProfileImgPath);
            //    string filename = Guid.NewGuid().ToString() + DateTime.Today.ToString("ddMMyyyy") + user.FirstName + "" + user.LastName + ".Jpeg";

            //    var path = Path.Combine(Phyiscalpath, filename);
            //    img.Save(path, ImageFormat.Jpeg);
            //    existedUser.ProfileImgPath = filename;
            //}

            //existedUser.IsAdmin = user.IsAdmin;
            //  existedUser.ResponsiblePerson = user.ResponsiblePerson;
            //  existedUser.ProfileName = filename;           
            existedUser.FlgStatus = 1;
            existedUser.UpdatedBy = userid;
            if (!existedUser.UpdatedOn.HasValue)
            {
                existedUser.UpdatedOn = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            _uow.User.Update(id, existedUser);
            _uow.Commit();
            //if (existedUser.UserDevices == null)
            //{
            //    var device = new Device();
            //    device.SerialNumber = user.SN;
            //    device.DeviceToken = user.Token;
            //    device.DeviceType = user.DeviceType;
            //    device.CreatedBy = id;
            //    device.CreatedOn = DateTime.Now;
            //    device.FlgStatus = 1;
            //    device.UpdatedBy = null;
            //    device.UpdatedOn = null;

            //    _uow.Devices.Add(device);
            //    _uow.Commit();
            //    var UserDevice = new UserDevice
            //    {
            //        CreatedBy = userid,
            //        CreatedOn = DateTime.Now,
            //        UserId = existedUser.Id,
            //        FlgStatus = 1,
            //        UpdatedBy = null,
            //        UpdatedOn = null,
            //        DeviceId = device.Id
            //    };
            //    _uow.UserDevice.Add(UserDevice);
            //    _uow.Commit();
            //}
            //else if (existedUser.UserDevices != null)
            //{
            //    var userDevice = _uow.UserDevice.GetAll(x => x.FlgStatus == 1, null, "").Where(u => u.UserId == id);
            //    foreach (var device in userDevice.ToList())
            //    {
            //        // here i get every device by its id
            //        var devicedata = _uow.Devices.GetById(device.DeviceId);
            //        if (devicedata == null)
            //        {
            //            throw new Exception("015");
            //        }
            //        devicedata.SerialNumber = user.SN;
            //        devicedata.DeviceToken = user.Token;
            //        devicedata.DeviceType = user.DeviceType;
            //        _uow.Devices.Update(device.Id, devicedata);
            //        _uow.Commit();
            //    }

            //}
           // var imagFullPath = HttpContext.Current.Server.MapPath("~/UsersProfile/" + existedUser.ProfileImgPath);
            var UserViewModel = factory.Create(existedUser);
            UserViewModel.ProfileImgPath = UserViewModel.ProfileImgPath !=null ?path+ UserViewModel.ProfileImgPath:null;
            UserViewModel.ResponsiblePerson = existedUser.ResponsiblePerson;
            var result = _userService.GenerateToken(existedUser);
            UserViewModel.UserToken = result.token;
            UserViewModel.TokenExpire = result.expires_on.ToString();
            UserViewModel.TokenType = result.type;
            UserViewModel.Tokenissue = result.issued_on.ToString();
            return UserViewModel;
        }

        public UserViewModel deleteCompanyBranch(int id, int CreatedBy)
        {
            var existedUser = _uow.User.GetById(id);
            if (existedUser == null)
            {
                throw new Exception("004");
            }
            if (existedUser.FlgStatus == 0)
            {
                throw new Exception("003");
            }
          //  existedUser.FlgStatus = 0;
            _uow.User.Delete(id, existedUser);
            _uow.Commit();
            var userModel = factory.Create(existedUser);
            return userModel;
        }

        public IEnumerable<UserViewModel> Allrequesters(string search)
        {
            int id;
            int.TryParse(search, out id);
            if (search == null)
            {
                return _uow.User.GetAll(x => x.FlgStatus == 1 && x.CustomerType == CustomerTypes.Requester).OrderByDescending(x => x.FirstName).ThenByDescending(x => x.LastName).Select(U => new UserViewModel { Id = U.Id, FirstName = U.FirstName, LastName = U.LastName, Email = U.Email, Mobile = U.Mobile, Rating = U.OverAllrating.HasValue ? U.OverAllrating.Value : 0, IsActive = U.IsActive , FullName =U.FirstName +""+""+U.LastName});
            }
            return _uow.User.GetAll(x => x.FlgStatus == 1 && x.CustomerType == CustomerTypes.Requester).OrderByDescending(x => x.FirstName).ThenByDescending(x => x.LastName).Where(y => y.FirstName.Contains (search) || y.FirstName .StartsWith(search)|| y.LastName.Contains(search) || y.LastName.StartsWith(search) || y.Id== id || y.Email.Equals(search) || y.Email.StartsWith(search) || y.Mobile.Equals(search) || y.Mobile.Contains(search)).Select(U => new UserViewModel { Id = U.Id, FirstName = U.FirstName, LastName = U.LastName, Email = U.Email, Mobile = U.Mobile, Rating = U.OverAllrating.HasValue ? U.OverAllrating.Value : 0, IsActive = U.IsActive, FullName = U.FirstName + "   " + " " + U.LastName });


        }

        public IEnumerable<UserViewModel> AllServiceProviders(string search)
        {
            int id;
            int.TryParse(search,out  id);
            if (search == null)
            {
                return _uow.User.GetAll(x => x.FlgStatus == 1 && x.CustomerType == CustomerTypes.ServiceProvider).OrderByDescending(x => x.Id).Select(U => new UserViewModel { Id = U.Id, FirstName = U.FirstName, LastName = U.LastName, Email = U.Email, Mobile = U.Mobile, Rating = U.OverAllrating.HasValue ? U.OverAllrating.Value : 0, IsActive = U.IsActive,FullName = U.FirstName+" "+"  "+ U.LastName });
            }
            var servicrProviderss = _uow.User.GetAll(x => x.FlgStatus == 1 && x.CustomerType == CustomerTypes.ServiceProvider).OrderByDescending(x => x.Id).Where(y => y.FirstName.StartsWith (search) || y.FirstName.Contains(search)|| y.LastName.Contains(search)|| y.LastName.StartsWith(search) || y.Email.Equals(search) || y.Email.StartsWith(search) || y.Mobile.Equals(search) || y.Mobile.Contains(search) || y.Id==id ).Select(U => new UserViewModel { Id = U.Id, FirstName = U.FirstName, LastName = U.LastName, Email = U.Email, Mobile = U.Mobile, Rating = U.OverAllrating.HasValue ? U.OverAllrating.Value : 0, IsActive = U.IsActive });
           
            return servicrProviderss;
        }

        public UserViewModel CreateServiceProvider(UserViewModel user, HttpPostedFileBase file, int UserId)
        {
            var encryptedPassWord = Utility.Utility.Encrypt(user.Password);

            if (_uow.User.GetAll(x => x.FlgStatus == 1, null, "").Any(y => y.Email == user.Email && y.Password == encryptedPassWord))
            {
                throw new Exception("039");
            }
            else
            {
                var UserData = new User();
                if (file != null)
                {

                    string ImageName = System.IO.Path.GetFileName(file.FileName);
                    UserData.ProfileImgPath = ImageName;
                }
                UserData.CreatedBy = UserId;
                UserData.FirstName = user.FirstName;
                UserData.LastName = user.LastName;
                UserData.Email = user.Email;
                UserData.Mobile = user.Mobile;
                UserData.Password = encryptedPassWord;

                UserData.CustomerType = CustomerTypes.ServiceProvider;
                UserData.UpdatedBy = null;
                UserData.CreatedOn = DateTime.Now;
                UserData.UpdatedOn = null;
                UserData.FlgStatus = 1;
                _uow.User.Add(UserData);
                _uow.Commit();
                var usermodel = factory.Create(UserData);
                return usermodel;
            }
        }

        public UserViewModelFromAdmin EditProvider(int id, UserViewModelFromAdmin user, HttpPostedFileBase file, int UserId)
        {
            var existedUser = _uow.User.GetById(id);
            if (existedUser == null)
            {
                throw new Exception("004");

            }
            if (existedUser.FlgStatus == 0)
            {
                throw new Exception("003");
            }
            if (file == null )
            {
               
                existedUser.ProfileImgPath = existedUser.ProfileImgPath;
            }
            if (file != null && file.ContentLength > 0)
            {
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                existedUser.ProfileImgPath = ImageName;
            }
            if (user.Password !=null )
            {
                var encryptedPassWord = Utility.Utility.Encrypt(user.Password);
                existedUser.Password = encryptedPassWord;
            }
            if (user.Password == null)
            {
             
                existedUser.Password = existedUser.Password;
            }
           
            existedUser.Mobile = user.Mobile;
            existedUser.FirstName = user.FirstName;
            existedUser.LastName = user.LastName;
            existedUser.Email = user.Email;
          
            existedUser.CustomerType = CustomerTypes.ServiceProvider;

            existedUser.FlgStatus = 1;
            existedUser.UpdatedBy = UserId;
            if (!existedUser.UpdatedOn.HasValue)
            {
                existedUser.UpdatedOn = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            _uow.User.Update(id, existedUser);
            _uow.Commit();

            var UserViewModel = factory.CreateUserfromadmin(existedUser);
         
         //UserViewModel.ProfileImgPath = existedUser.ProfileImgPath != null ? path+existedUser.ProfileImgPath:null;
            return UserViewModel;
        }


        public UserViewModel CreateRequester(UserViewModel user, HttpPostedFileBase file, int UserId)
        {
            var encryptedPassWord = Utility.Utility.Encrypt(user.Password);

            if (_uow.User.GetAll(x => x.FlgStatus == 1, null, "").Any(y => y.Email == user.Email && y.Password == encryptedPassWord))
            {
                throw new Exception("039");
            }
            else
            {
                var UserData = new User();
                if (file != null)
                {

                    string ImageName = System.IO.Path.GetFileName(file.FileName);
                    UserData.ProfileImgPath = ImageName;
                }
                UserData.CreatedBy = UserId;
                UserData.FirstName = user.FirstName;
                UserData.LastName = user.LastName;
                UserData.Email = user.Email;
                UserData.Mobile = user.Mobile;
                UserData.Password = encryptedPassWord;

                UserData.CustomerType = CustomerTypes.Requester;
                UserData.UpdatedBy = null;
                UserData.CreatedOn = DateTime.Now;
                UserData.UpdatedOn = null;
                UserData.FlgStatus = 1;
                _uow.User.Add(UserData);
                _uow.Commit();
                var usermodel = factory.Create(UserData);
                return usermodel;
            }
        }

        public UserViewModelFromAdmin EditRequrster(int id, UserViewModelFromAdmin user, HttpPostedFileBase file, int UserId)
        {
            var existedUser = _uow.User.GetById(id);
            if (existedUser == null)
            {
                throw new Exception("004");

            }
            if (existedUser.FlgStatus == 0)
            {
                throw new Exception("003");
            }
            if (file == null)
            {

                existedUser.ProfileImgPath = existedUser.ProfileImgPath;
            }
            if (file != null && file.ContentLength > 0)
            {
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                existedUser.ProfileImgPath = ImageName;
            }
            if (user.Password != null)
            {
                var encryptedPassWord = Utility.Utility.Encrypt(user.Password);
                existedUser.Password = encryptedPassWord;
            }
            if (user.Password == null)
            {

                existedUser.Password = existedUser.Password;
            }

            existedUser.Mobile = user.Mobile;
            existedUser.FirstName = user.FirstName;
            existedUser.LastName = user.LastName;
            existedUser.Email = user.Email;

            existedUser.CustomerType = CustomerTypes.Requester;

            existedUser.FlgStatus = 1;
            existedUser.UpdatedBy = UserId;
            if (!existedUser.UpdatedOn.HasValue)
            {
                existedUser.UpdatedOn = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            _uow.User.Update(id, existedUser);
            _uow.Commit();

            var UserViewModel = factory.CreateUserfromadmin(existedUser);

            //UserViewModel.ProfileImgPath = existedUser.ProfileImgPath != null ? path+existedUser.ProfileImgPath:null;
            return UserViewModel;
        }

        public void DeActiveUser(int userId, bool state)
        {
            var existingUser = _uow.User.GetById(userId);
            var usersToken = new List<string>();
            if (existingUser == null)
            {
                throw new Exception("011");
            }
            if (state == true)
            {
                existingUser.IsActive = true;
                existingUser.UpdatedOn = DateTime.Now;
                existingUser.UpdatedBy = userId;
                _uow.User.Update (userId, existingUser);
                _uow.Commit();
              
            }
            else
            {
                existingUser.IsActive = false;
                existingUser.UpdatedOn = DateTime.Now;
                existingUser.UpdatedBy = userId;
                _uow.User.Update(userId, existingUser);
                _uow.Commit();
                NotificationViewModel notymodel = new NotificationViewModel();
                PushNotification push = new PushNotification();
                var User = _uow.User.GetAll(x => x.FlgStatus == 1 && x.Id == userId).FirstOrDefault();
                var devices = GetDeviceByTokenAndType(User.Id);
                if (devices.Count() > 0)
                {
                    foreach (var device in devices)
                    {
                        if (device != null)
                        {
                            usersToken.Add(device.Fcmtoken);
                        }
                    }
                }
                notymodel.Body = "تم الغاء تفعيل حسابكم من ناقلات" + ":" + existingUser.FirstName + " " + existingUser.LastName + '-' + "Your Account has been blocked" + ":" + existingUser.FirstName + " " + existingUser.LastName;
                notymodel.Date = DateTime.Now;
                notymodel.Title = "تم الغاء تفعيل حسابكم من ناقلات" + ":" + existingUser.FirstName + " " + existingUser.LastName + '-' + "Your Account has been blocked" + ":" + existingUser.FirstName + " " + existingUser.LastName;
                notymodel.Seen = false;
                // notymodel.ReceiverId = item.Id;
                notymodel.registration_ids = usersToken;
                notymodel.Type = NotificationType.UserDeActivate;
                notymodel.BodyArb = "تم الغاء تفعيل حسابكم من ناقلات" + ":" + existingUser.FirstName + " " + existingUser.LastName;
                notymodel.BodyEng = "Your Account has been blocked" + ":" + existingUser.FirstName + " " + existingUser.LastName;
                notymodel.TitleArb = "تم الغاء تفعيل حسابكم من ناقلات" + ":" + existingUser.FirstName + " " + existingUser.LastName;
                notymodel.TitleEng = "Your Account has been blocked" + ":" + existingUser.FirstName + " " + existingUser.LastName;
                push.PushNotifications(notymodel, "");
                _iNotificationService.AddNoty(notymodel, userId);
            }
          

        }

        public UserViewModel EditUserImage(EditUserImageViewModel ImagePath, int id)
        {
            var existedUser = _uow.User.GetById(id);
           // var imagFullPath = HttpContext.Current.Server.MapPath("~/UsersProfile/" + existedUser.ProfileImgPath);
            if (existedUser == null)
            {
                throw new Exception("004");

            }
            if (existedUser.FlgStatus == 0)
            {
                throw new Exception("003");
            }
            //  var result = _userService.GenerateToken(existedUser);



            if (ImagePath.ProfileImgPath == null)
            {
                existedUser.ProfileImgPath =null;

            }
            if (ImagePath.ProfileImgPath != null)
            {
                var Phyiscalpath = HttpContext.Current.Server.MapPath("~/UsersProfile/");
                {
                    Directory.CreateDirectory(Phyiscalpath);
                }
                string converted = ImagePath.ProfileImgPath.Substring(ImagePath.ProfileImgPath.IndexOf(",") + 1);
                Image img = ImageHelper.Base64ToImage(converted);
                string ext = Path.GetExtension(ImagePath.ProfileImgPath);
                string filename = Guid.NewGuid().ToString() + DateTime.Today.ToString("ddMMyyyy") + existedUser.FirstName + "" + existedUser.LastName + ".Jpeg";

                var path = Path.Combine(Phyiscalpath, filename);
                img.Save(path, ImageFormat.Jpeg);
                existedUser.ProfileImgPath = filename;
            }

            existedUser.FlgStatus = 1;
            existedUser.UpdatedBy = id;
            if (!existedUser.UpdatedOn.HasValue)
            {
                existedUser.UpdatedOn = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            _uow.User.Update(id, existedUser);
            _uow.Commit();

            var UserViewModel = factory.Create(existedUser);
           
            var result = _userService.GenerateToken(existedUser);
            UserViewModel.UserToken = result.token;
            UserViewModel.TokenExpire = result.expires_on.ToString();
            UserViewModel.TokenType = result.type;
            UserViewModel.ProfileImgPath = existedUser.ProfileImgPath != null ? path + existedUser.ProfileImgPath : null;
            UserViewModel.Tokenissue = result.issued_on.ToString();

            return UserViewModel;
        }
        public IEnumerable<RuleViewModel> AllRules(string search = null)
        {
            return _uow.Rule.GetAll(x => x.FlgStatus == 1).Where(x => search == null || x.Name.ToLower().Contains(search)).Select(r => new RuleViewModel { Id = r.Id, Description = r.Description, DescriptionEng = r.DescriptionEng });
        }

        public ServiceProviderDetailesViewModel ServiceProviderDetailes(int UserId)
        {
           // string imagFullPath = HttpContext.Current.Server.MapPath("~/OffersImages/");
            var providerDetailes = _uow.User.GetAll(x => x.FlgStatus == 1 && x.CustomerType == CustomerTypes.ServiceProvider && x.Id == UserId).ToList().Select(s => new ServiceProviderDetailesViewModel
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Mobile = s.Mobile,
                Email = s.Email,
                Address = s.Address,
                Rating = s.OverAllrating,
                comapnies = GetCompaniesAnderUser(s.Id),
                ServiceProvidersOffers = s.Offers.Select(o => new OfferViewModel
                {
                    OfferNumber = o.OfferNumber,
                    OfferDate = o.OfferDate,
                    PublishDate = o.PublishDate,
                    Price = o.Price,
                    quantity = o.Quantity,
                    Period = o.Period,
                    Region = factory.Create(o.Region),
                    ImageUrl =  o.ImageUrl!=null ? OfferPath+o.ImageUrl: o.ImageUrl,
                    TruckType = factory.Create(o.TruckType),
                    OfferDetails = o.OfferDetails
.Select(od => new OfferDetailViewModel { truckId = od.Id, OfferTrucks = factory.Create(od.Truck), trucksNo = od.NumberOfTrucks })
                })
  ,
                AcceptedRequests = PropsalPrices(UserId)


            })
          .FirstOrDefault();
            return providerDetailes;
        }

        public IEnumerable<ProposalPriceModelView> PropsalPrices(int UserId)
        {
            var propsales = _uow.ProposalPrice.GetAll(x => x.FlgStatus == 1 && x.ServiceProviderId == UserId).ToList().Select(p => new ProposalPriceModelView { Price = p.Price, ExpireDate = p.ExpireDate, Date = p.Date, Request = factory.Create(p.Request), PropsalStatus = p.PropsalStatus, IsAccepted = p.IsAccepted.Value, UserAcceptedBy = factory.Create(p.ServiceProvider) });
            return propsales;
        }

        public CustomerDetailesviewModelcs CustomerDetailes(int UserId)
        {
            var customerDetailes = _uow.User.GetAll(x => x.FlgStatus == 1 && x.CustomerType == CustomerTypes.Requester && x.Id == UserId).ToList().Select(d => new CustomerDetailesviewModelcs { FirstName = d.FirstName, LastName = d.LastName, Mobile = d.Mobile, Email = d.Email, Requests = d.Requests.Select(r => new RequestModelView { Id = r.Id, Requestdate = r.Requestdate, ExpireDate = r.ExpireDate, StartingDate = r.StartingDate, Quantity = r.Quantity, Period = r.Period, ItemsInfo = r.ItemsInfo, LocationFrom = r.LocationFrom, LocationTo = r.LocationTo, RequestState = r.RequestState,IsActive=r.IsActive  }), AcceptedOffers = d.AcceptOffers.Select(a => new AcceptofferViewModel { AcceptedDate = a.AcceptedDate, Notes = a.Notes, Offer = factory.Create(a.Offer), OfferState = a.OfferState, AcceptedUserId = a.AcceptedUserId, AcceptedUserName = a.User.FirstName + a.User.LastName }) }).FirstOrDefault();
            return customerDetailes;
        }

        public int ProviderConnts(int month)
        {

            return _uow.User.GetAll(c => c.FlgStatus == 1).Where(y => y.CustomerType == CustomerTypes.ServiceProvider && y.CreatedOn.Month == month).ToList().Count();

        }

        public bool CheckEmaileExit(string email)
        {
           return  _uow.User.GetAll(x => x.FlgStatus == 1, null, "").Any(y => y.Email == email );
            
        }

        public bool checkUserdata(string email, string password)
        {
            var user = _uow.User.GetAll().SingleOrDefault(x => x.Email == email && x.Password == password);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public bool ChecKEmail(string email)
        {
            return   _uow.User.GetAll(x => x.FlgStatus == 1, null, "").Any(y => y.Email == email);
           
        }

        public bool CheckPassword(string email, string password)
        {

            return  _uow.User.GetAll (x => x.FlgStatus == 1  && x.IsActive==true , null, "").Any(x=>  x.Email == email && x.Password == password);
        }

        public AdminviewModel GetAdmin(int id)
        {
            var user = _uow.User.GetById(id);
            var model = factory.Createadmin(user);
            return model;
        }

        //public int ProviderConnts(int month)
        //{
        //    throw new NotImplementedException();
        //}

        public int CustomerConnts(int month)
        {
            return _uow.User.GetAll(c => c.FlgStatus == 1).Where(y => y.CustomerType == CustomerTypes.Requester && y.CreatedOn.Month == month).ToList().Count();
        }

        public IEnumerable<UserViewModel> NotyAllServiceProvider()
        {
            var users = _uow.User.GetAll(x => x.FlgStatus == 1 && x.CustomerType == CustomerTypes.ServiceProvider, null, "UserDevices").Select(U => new UserViewModel { Id = U.Id, FirstName = U.FirstName, LastName = U.LastName, Email = U.Email, Mobile = U.Mobile });
            return users.ToList();
        }

        public IEnumerable<UserViewModel> NotyAllCustomer()
        {
            var users = _uow.User.GetAll(x => x.FlgStatus == 1&& x.CustomerType == CustomerTypes.Requester, null, "UserDevices").Select(U => new UserViewModel { Id = U.Id, FirstName = U.FirstName, LastName = U.LastName, Email = U.Email, Mobile = U.Mobile });
            return users;

        }

        public IEnumerable<UserToken> GetDeviceByTokenAndType(int UserId)
        {
                      
            var devices = _uow.UserTokens .GetAll(x => x.FlgStatus == 1 && x.UserId== UserId, null, "Users");

            return devices.ToList();
        }

        public int GetUserBYDeviceId(int deviceId)
        {
            var DeviceUser = _uow.User.GetAll(x => x.FlgStatus == 1, null, "UserDevices").Where(y => y.UserDevices.Any(d => d.DeviceId == deviceId)).SingleOrDefault().Id;
            return DeviceUser;
        }

        public DeviceVewModel GetDeviceById(int deviceId)
        {
            var deviceData = _uow.Devices.GetAll(x => x.FlgStatus == 1, null, "").Where(y => y.Id == deviceId).Select(d => new DeviceVewModel {Id=d.Id, DeviceToken = d.DeviceToken, DeviceType = d.DeviceType, SerialNumber = d.SerialNumber }).SingleOrDefault();
            return deviceData;
        }

        public bool CheckAccountExist( string email ,string password)
        {

            return _uow.User.GetAll(x => x.FlgStatus == 1, null, "").Any(y => y.Email == email && y.Password == password);
           
        }

        public AdminviewModel EditAdminProfile(AdminviewModel adminModel, HttpPostedFileBase file, IList<RuleViewModel> rules, int[] selectedObjects)
        {
            throw new NotImplementedException();
        }

        public UserViewModel EditMobile(int id, EditUserMobileViewModel model, int userId)
        {
            var existedUser = _uow.User.SearchByPredicate (x=>x.Id==id);
          
            if (existedUser == null)
            {
                throw new Exception("004");

            }
            if (existedUser.FlgStatus == 0)
            {
                throw new Exception("003");
            }
            //  var result = _userService.GenerateToken(existedUser);



            existedUser.Mobile = model.Mobile;
            existedUser.FlgStatus = 1;
            existedUser.UpdatedBy = userId;
            if (!existedUser.UpdatedOn.HasValue)
            {
                existedUser.UpdatedOn = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            _uow.User.Update(id, existedUser);
            _uow.Commit();

            var UserViewModel = factory.Create(existedUser);
          
            var result = _userService.GenerateToken(existedUser);
            UserViewModel.UserToken = result.token;
            UserViewModel.TokenExpire = result.expires_on.ToString();
            UserViewModel.TokenType = result.type;
            UserViewModel.ProfileImgPath = UserViewModel.ProfileImgPath = UserViewModel.ProfileImgPath != null ? path + existedUser.ProfileImgPath : null;
            UserViewModel.Tokenissue = result.issued_on.ToString();

            return UserViewModel;
        }

        public UserViewModelFromAdmin GetUserFromadmin(int Id)
        {
            var user = _uow.User.GetById(Id);
            var model = factory.CreateUserfromadmin(user);
            return model;
        }

        public bool CheckvalidPassword(string password)
        {
            throw new NotImplementedException();
        }

        public void RefershToken(int UserId, RefershTokenViewModel securityToken)
        {
            var devices = _uow.UserTokens.GetAll(x => x.FlgStatus == 1&& x.UserId== UserId && x.Fcmtoken== securityToken.oldToken, null, "").FirstOrDefault();
             if (devices != null  )
                {

                devices.Fcmtoken = securityToken.SecurityToken ;
                devices.FlgStatus = 1;
                devices.CreatedBy = UserId;
                devices.CreatedOn = DateTime.Now;
                devices.UpdatedBy = UserId;
                devices.UpdatedOn = DateTime.Now;
                    _uow.UserTokens.Update(devices.Id, devices);
                    _uow.Commit();
                }

         
        }

        public bool IsUserActive(int userId)
        {
            var existingUser = _uow.User.GetById(userId);
            if (existingUser == null)
            {
                throw new Exception("011");
            }
            var IsActive = existingUser.IsActive;
            return IsActive;


        }

        public bool IsReated(int userid)
        {
            var existedUser = _uow.User.GetAll(x => x.FlgStatus == 1).FirstOrDefault(x => x.Id == userid);
            if (existedUser == null)
            {
                throw new Exception("004");
            }
            if (existedUser.FlgStatus == 0)
            {
                throw new Exception("003");
            }
            if (existedUser.Interests != null || existedUser.NotificationReciver != null || existedUser.NotificationSender != null || existedUser.Offers != null || existedUser.Reciver != null || existedUser.Requests != null)
            {
                return true;
            }
            return false;
        }
    }
}
