using Ejab.BAL.CustomDTO;
using Ejab.BAL.ModelViews;
using Ejab.DAl;
using Ejab.DAl.Common;
using Ejab.DAl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ejab.BAL.Services
{
    public interface ICustomerService
    {
        IEnumerable<UserViewModel> AllUsers();
        IEnumerable<UserViewModel> GetUsers(CustomerTypes type);
        bool PasswordRecovery(RecoveryPasswordModel recoveryModel);
        Random ResetPassword(ResetPasswordViewModel resetpasswordModel);
        UserViewModel ChangePassword(ChangePassWordModel changpasswordModel, int id);
        void LogOut(int userId, LogOutViewModel model);
        UserViewModel LogIn(LoginViewModel loginModel);
        UserViewModel Profile(int id);
        AdminviewModel CreateUserProfile(AdminviewModel user, HttpPostedFileBase file, int userId, IList<RuleViewModel> rules, int[] selectedObjects);
        UserViewModel Register(RegisterViewModel model);
        UserViewModel Put(UserViewModel user, int id);
        UserViewModel DeleteUser(int id);
        bool IsUserMobileAvialable(string Mobile);
        bool IsUserEmailAvialable(string Email);
        void UpdateUserPassword(string Email, string newPassword);
        UserViewModel FaceBookLogin(FaceBookLoginViewModel faceBookModel);
        IEnumerable<object> AllFaceBookUsers();
        object GetFaceBookUser(int FaceBookId);
        IEnumerable<AdminviewModel> AdminUsers(string search, int? page);
        UserViewModel GetUser(int id);
        AdminviewModel GetAdmin(int id);
        AdminviewModel EditAdminData(AdminviewModel user, int id, HttpPostedFileBase file, IList<RuleViewModel> rules, int[] selectedObjects);
        UserViewModel AdminLogIn(AdminLogin Adminlogin);
        UserViewModel AdminProfile(int id);
        UserViewModel CreateCompanyBranch(RegisterViewModel user, int CreatedBy);
        IEnumerable<UserViewModel> GetCompaniesAnderUser(int CreatedBy);
        UserViewModel EditCompanyBranch(int id, RegisterViewModel user, int userid);
        UserViewModel deleteCompanyBranch(int id, int CreatedBy);
        IEnumerable<UserViewModel> Allrequesters(string search);
        IEnumerable<UserViewModel> AllServiceProviders(string search);
        UserViewModel CreateServiceProvider(UserViewModel user, HttpPostedFileBase file, int UserId);
        UserViewModelFromAdmin EditProvider(int id, UserViewModelFromAdmin user, HttpPostedFileBase file, int UserId);
        UserViewModel CreateRequester(UserViewModel user, HttpPostedFileBase file, int UserId);
        UserViewModelFromAdmin EditRequrster(int id, UserViewModelFromAdmin user, HttpPostedFileBase file, int UserId);
        void DeActiveUser(int userId, bool stat);
        UserViewModel EditUserImage(EditUserImageViewModel ImagePath, int id);
        IEnumerable<RuleViewModel> AllRules(string search);
        ServiceProviderDetailesViewModel ServiceProviderDetailes(int UserId);
        IEnumerable<ProposalPriceModelView> PropsalPrices(int UserId);
        CustomerDetailesviewModelcs CustomerDetailes(int UserId);
        int ProviderConnts(int month);
        bool CheckEmaileExit(string email);
        bool checkUserdata(string email, string password);
        bool ChecKEmail(string email);
        bool CheckPassword(string email, string password);
        int CustomerConnts(int month);
        IEnumerable<UserViewModel> NotyAllServiceProvider();
        IEnumerable<UserViewModel> NotyAllCustomer();
      IEnumerable<UserToken> GetDeviceByTokenAndType(int UserId);
        int GetUserBYDeviceId(int deviceId);

        DeviceVewModel GetDeviceById(int deviceId);
        bool CheckAccountExist(string email, string password);
        AdminviewModel EditAdminProfile(AdminviewModel adminModel, HttpPostedFileBase file, IList<RuleViewModel> rules, int[] selectedObjects);
        //IEnumerable<RuleViewModel> UserRules(int UserId);

        UserViewModel EditMobile(int id, EditUserMobileViewModel model,int userId);
        UserViewModelFromAdmin GetUserFromadmin(int Id);
        bool CheckvalidPassword(string password);
        void RefershToken(int UserId, RefershTokenViewModel securityToken);
        bool IsUserActive(int userId);
        bool IsReated(int userid);
       


    }
}
