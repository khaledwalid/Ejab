using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Ejab.Rest.CommonEmail 
{
    public class ErrorResult : IHttpActionResult
    {
        Error _error;
        HttpRequestMessage _request;
        public ErrorResult(Error error, HttpRequestMessage request)
        {
            _error = error;
            _request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage()
            {
                Content = new ObjectContent<Error>(_error, new JsonMediaTypeFormatter()),
                RequestMessage = _request
            };
            return Task.FromResult(response);
        }
    }
    public static class ErrorMsg
    {
        public static string Message { get; set; }
        public static string SetMessage(string statusCode, string lang)
        {
            if (lang == "ar")
            {
                switch (statusCode)
                {

                    case "000": Message = "تمت العمليه بنجاح"; break;
                    case "001": Message = "رقم الجوال موجود مسبقا"; break;
                    case "002": Message = "البريد الالكترونى موجود مسبقا"; break;
                    case "003": Message = "تم حذفه من قبل"; break;
                    case "004": Message = "غير مسجل من قبل"; break;
                    case "005": Message = "يجب ان تملأ البيانات"; break;
                    case "006": Message = "لا يود بيانات"; break;
                    case "007": Message = "ها البريد الالكترونى غير موجود"; break;
                    case "008": Message = "غير معرف"; break;
                    case "009": Message = "الجهاز غير موجود"; break;
                    case "010": Message = "هذا المستخدم لا يملك هذا الجهاز"; break;
                    case "011": Message = "هذا العرض غير موجود"; break;
                    case "012": Message = "لا يود بيانات تخص هذا البحث"; break;
                    case "013": Message = "لا توجد عروض تم قبولها لهذا المستخدم"; break;
                    case "014": Message = "تم التسجيل كغير مزود للخدمه"; break;
                }
            }
            else if (lang == "en")
            {
                switch (statusCode)
                {

                    case "000": Message = "Success"; break;
                    case "001": Message = "mobile Is already  Exist Or Dublicated"; break;
                    case "002": Message = "Email Is already Exist"; break;
                    case "003": Message = "Is Deleted"; break;
                    case "004": Message = "Do Not Register"; break;
                    case "005": Message = "body can not be empty"; break;
                    case "006": Message = "No Data Exist"; break;
                    case "007": Message = "Email doesn't exist"; break;
                    case "008": Message = "not authenticated"; break;
                    case "009": Message = "Device Do Not Exist"; break;
                    case "010": Message = "current User Do Not have this Device"; break;
                    case "011": Message = "Offer Do Not exist"; break;
                    case "012": Message = "No Data Matches Your Search"; break;
                    case "013": Message = "There Is No Accepted Offers For This Customer"; break;
                    case "014": Message = "Sorry You Have  Login Do not As Service Provider"; break;
                    case "015": Message = "You Must Put Device Token"; break;
                }
            }
            return Message;
        }
    }
    public class Error
    {
        public Error()
        {

        }
        public Error(string statusCode, string lang)
        {
            this.Code = statusCode;
            SetMessage(statusCode, lang);

        }
        public string Code { get; set; }
        public string Message { get; set; }

        private void SetMessage(string statusCode, string lang)
        {
            if (lang == "ar")
            {
                switch (statusCode)
                {

                    case "000": this.Message = "تمت العمليه بنجاح"; break;
                    case "001": this.Message = "رقم الجوال موجود مسبقا"; break;
                    case "002": this.Message = "البريد الالكترونى موجود مسبقا"; break;
                    case "003": this.Message = "تم حذفه من قبل"; break;
                    case "004": this.Message = "غير مسجل من قبل"; break;
                    case "005": this.Message = "يجب ان تملأ البيانات"; break;
                    case "006": this.Message = "لا يوجد بيانات"; break;
                    case "007": this.Message = " البريد الالكترونى غير موجود"; break;
                    case "008": this.Message = "غير معرف"; break;
                    case "009": this.Message = "الجهاز غير موجود"; break;
                    case "010": this.Message = "هذا المستخدم لا يملك هذا الجهاز"; break;
                    case "011": this.Message = "هذا العرض غير موجود"; break;
                    case "012": this.Message = "لا يوجد بيانات تخص هذا البحث"; break;
                    case "013": this.Message = "لا توجد عروض تم قبولها لهذا المستخدم"; break;
                    case "014": this.Message = "تم التسجيل كغير مزود للخدمه"; break;
                    case "015": this.Message = "انت غير مصرح لك بالدخول"; break;
                    case "016": this.Message = "كلمة المرور مطلوبه"; break;
                    case "017": this.Message = "نوع الجهاز مطلوب"; break;
                    case "018": this.Message = "الاسم الاول مطلوب"; break;
                    case "019": this.Message = "الاسم الثانى مطلوب"; break;
                    case "020": this.Message = "رقم الجوال مطلوب"; break;
                    case "021": this.Message = "العنوان مطلوب"; break;
                    case "022": this.Message = "البريد الالكترونى مطلوب"; break;
                    case "023": this.Message = "البريد الالكترونى غير صحيح"; break;
                    case "024": this.Message = "لا توجد عرض الى الان"; break;
                    case "025": this.Message = "من فضلك ادخل مستلم الرساله"; break;
                    case "026": this.Message = "هذا الطلب ليس لديه اى رسائل"; break;
                    case "027": this.Message = "هذا المستخدم لم يقم بارسال اى رسائل"; break;
                    case "028": this.Message = "هذا المستخدم لم يقم باستلام اى رسائل"; break;
                    case "029": this.Message = "رقم الجوال يجب ان لايزيد عن 13 رقم"; break;
                    case "030": this.Message = "رقم العرض مطلوب"; break;
                    case "031": this.Message = "التاريخ مطلوب"; break;
                    case "032": this.Message = "تاريخ النشر مطلوب"; break;
                    case "033": this.Message = "عنوان العرض مطلوب"; break;
                    case "034": this.Message = "تفاصيل العرض مطلوبه"; break;
                    case "035": this.Message = " السعر مطلوب"; break;
                    case "036": this.Message = "الكميه مطلوبه"; break;
                    case "037": this.Message = "كلمتى المرور غير متطابقتين"; break;
                    case "038": this.Message = "كلمة المرور لابد وان تكون على الاقل 6 احرف"; break;
                    case "039": this.Message = "هذا البريد الالكترونى لديه بالفعل حساب "; break;
                    case "040": this.Message = "من فضلك اعد كتابه كلمه المرور "; break;
                    case "041": this.Message = "عنوان الرساله مطلوب"; break;
                    case "042": this.Message = "من فضلك ادخل تفاصيل الرساله"; break;
                    case "043": this.Message = "لا يوجد رسائل غير مقرؤه"; break;
                    case "044": this.Message = "نوع الخدمه مطلوب"; break;
                    case "045": this.Message = " من فضلك حدد المكان"; break;
                    case "046": this.Message = "عفوا لقد تم تسجيلكم كمزود خدمة ولا يمكنكم اضافه اى طلبات"; break;
                    case "047": this.Message = "من فضلك ادخل رقم الطلب"; break;
                    case "049": this.Message = "من فضلك ادخل مزود الخدمه"; break;
                    case "050": this.Message = "من فضلك ادخل التقييم"; break;
                    case "051": this.Message = "من فضلك ادخل رقم الجوال بطريقه صحيحه"; break;
                    case "052": this.Message = "Registerby is required"; break;
                    case "053": this.Message = "لا يوجد مستخدمين  اتموا التسجيل من خلال الفيس بوك"; break;
                    case "054": this.Message = "كلمة المرور او البريد الالكترونى غير صحيح"; break;
                    case "055": this.Message = "لا توجد عروض مفعله "; break;
                    case "056": this.Message = "هذا المستخدم غير مدير للنظام"; break;
                    case "057": this.Message = "تم تسجيل الدخول بنجاح"; break;
                    case "058": this.Message = "هذا العرض تم اختياره"; break;
                    case "059": this.Message = "لابد من تسجيل الدخول كمزود خدمه لكى تتمكن من وضع سعر"; break;
                    case "60": this.Message = "لا توجد تفاصيل للطلب"; break;
                    case "61": this.Message = "من فضلك ادخل FaceBookId"; break;
                    case "62": this.Message = "العميل مطلوب"; break;
                    case "63": this.Message = "عدد الشاحنات مطلوب"; break;
                    case "64": this.Message = " مزود الخدمه ليس لديه اى شركات"; break;
                    case "65": this.Message = " لا يمكن لشركه واحده ان تقدم اكتر من عرض سعر"; break;
                    case "66": this.Message = " لا يوجد مزودى الخدمه حتى الان بالنظام"; break;
                    case "67": this.Message = " لا يوجد عملاء  حتى الان بالنظام"; break;
                    case "68": this.Message = " عفوا لابد ان تاريخ صلاحيه الطلب  اقل من تاريخ البدء"; break;
                    case "69": this.Message = " عفوا لا يمكن وضع اكتر من عرض سعر لنفس الطلب"; break;
                    case "70": this.Message = " كلمة المرور القديمه غير صحيحه"; break;
                    case "71": this.Message = " من فضلك ادخل كلمه المرور الجديده"; break;
                    case "72": this.Message = " كلمة المرور غير صحيحه"; break;
                    case "73": this.Message = " لقد تم قبول هذا العرض"; break;
                    case "74": this.Message = " من فضلك ادخل نوع المستخدم"; break;
                    case "75": this.Message = " عفوا لا يمكنك  ارسال الرساله بعد انتهاء الطلب"; break;
                    case "76": this.Message = "من فضلك ادخل رقم الهاتف"; break;
                    case "77": this.Message = "من فضلك ادخل الشكوى"; break;
                    case "78": this.Message = "خط الطول مطلوب"; break;
                    case "79": this.Message = "خط العرض مطلوب"; break;
                    case "80": this.Message = "خط العرض مطلوب"; break;
                    case "81": this.Message = "هذه المنطقه تم ادخالها من قبل"; break;
                    case "82": this.Message = "لقد تم ادخال بريد الكترونى او كلمة مرور غير صحيحه من فضلك حاول مره اخرى"; break;
                    case "83": this.Message = "الاسم الاول لابد وان لا يزيد عن 50 حرفا"; break;
                    case "84": this.Message = "الاسم الثانى لابد وان لا يزيد عن 50 حرفا"; break;
                    case "85": this.Message = "كلمة المرور لابد وان لا تقل عن 6 احرف وارقام"; break;
                    case "86": this.Message = "ادخل اسم المنطقه"; break;
                    case "87": this.Message = "لقد تم قبولكم لهذا الاعلان من قبل"; break;
                    case "88": this.Message = "لابد من تفعيل  الحساب قبل تسجيل الدخول"; break;
                    case "89": this.Message = "عفوا هذا الطلب تم قبوله "; break;
                    case "90": this.Message = "لقد تم وضع بيانات هذا الطلب من قبل"; break;
                    case "91": this.Message = "البريد الالكترونى غير مسجل من قبل"; break;
                    case "92": this.Message = "رقم الجوال او البريد الالكترونى موجود بالفعل"; break;
                    case "93": this.Message = "الاسم مطلوب"; break;
                    case "94": this.Message = "الرساله مطلوبه"; break;
                    case "95": this.Message = "اسم المرسل مطلوب"; break;
                    case "96": this.Message = "رقم الجوال غير صحيح"; break;
                    case "97": this.Message = "من فضلك قم بالتعديل فى معلومات الطلب لاعادته مره اخرى كطلب جديد"; break;
                    case "98": this.Message = "رقم الجوال موجود بالفعل"; break;
                    case "99": this.Message = "تم ارسال كلمة المرور الجديده الى بريدكم الالكترونى"; break;
                    case "100": this.Message = "حدث خطأ فى السيرفر برجاء المحاوله فى وقت لاحق "; break;
                    case "101": this.Message = "لقد تم تسجيلكم كموزد خدمه برجاء الدخول كمزود للخدمه"; break;
                    case "102": this.Message = "لقد تم تسجيلكم كطالب خدمه برجاء الدخول كطالب للخدمه"; break;
                    case "103": this.Message = "لا يمكنكم وضع سعر لطلب منتهى الصلاحيه"; break;
                    case "104": this.Message = "تم ارسال كلمة المرور الى البريد الالكترونى"; break;
                    case "105": this.Message = " لقد تم اضافتكم لهذه الشاحنه /المعده لقائمةالاهتمام من قبل"; break;
                    case "106": this.Message = "لقد تم اضافتكم لهذة المنطقه لقائمة الاهتمام من قبل"; break;
                    case "107": this.Message = "لابد من ادخال اسم المنطقه"; break;
                    case "108": this.Message = "تم إضافه الجديد "; break;
                    case "109": this.Message = "لا يمكن الحذف لانه مرتب ببيانات اخرى "; break;

                }

            }
            else if (lang == "en")
            {
                switch (statusCode)
                {

                    case "000": this.Message = "Success"; break;
                    case "001": this.Message = "mobile Is already  Exist Or Dublicated"; break;
                    case "002": this.Message = "this email is already registered"; break;
                    case "003": this.Message = "Is Deleted"; break;
                    case "004": this.Message = "Do Not Register"; break;
                    case "005": this.Message = "body can not be empty"; break;
                    case "006": this.Message = "No Data Exist"; break;
                    case "007": this.Message = "Email doesn't exist"; break;
                    case "008": this.Message = "Please Enter Device Token"; break;
                    case "009": this.Message = "Device Do Not Exist"; break;
                    case "010": this.Message = "current User Do Not have this Device"; break;
                    case "011": this.Message = "Offer Do Not exist"; break;
                    case "012": this.Message = "No Data Matches Your Search"; break;
                    case "013": this.Message = "There Is No Accepted Offers For This Customer"; break;
                    case "014": this.Message = "Sorry You Have  Login Do not As Service Provider"; break;
                    case "015": this.Message = "Please Enter Device Token"; break;
                    case "016": this.Message = "Password is required"; break;
                    case "017": this.Message = "Device type is required"; break;
                    case "018": this.Message = "First name is required"; break;
                    case "019": this.Message = "last name is required"; break;
                    case "020": this.Message = "mobile is required"; break;
                    case "021": this.Message = "Address is required"; break;
                    case "022": this.Message = "Email is required"; break;
                    case "023": this.Message = "Email is not valid"; break;
                    case "024": this.Message = "No Offers Exist"; break;
                    case "025": this.Message = "Please Put Message Reciver"; break;
                    case "026": this.Message = "this Request Do Not Any Message"; break;
                    case "027": this.Message = "this User Do Not Send   Any Message"; break;
                    case "028": this.Message = "this User Do Not Recive Any Message"; break;
                    case "029": this.Message = "Mobile Number Must Be Equal 13 Number"; break;
                    case "030": this.Message = "Offer Number Is Required"; break;
                    case "031": this.Message = " Date Is Required"; break;
                    case "032": this.Message = "Publish Date Is Required"; break;
                    case "033": this.Message = " Title Is Required"; break;
                    case "034": this.Message = " Description Is Required"; break;
                    case "035": this.Message = "Price Is Required"; break;
                    case "036": this.Message = "Offer quantity Is Required"; break;
                    case "037": this.Message = "The password and confirmation password do  not match."; break;
                    case "038": this.Message = "The password must be at least {6} characters long.."; break;
                    case "039": this.Message = "you Allready Have Account With This Email"; break;
                    case "040": this.Message = "Please Confirm Password "; break;
                    case "041": this.Message = "Title Is required"; break;
                    case "042": this.Message = "Description Is Required"; break;
                    case "043": this.Message = "there is no unread Messages"; break;
                    case "044": this.Message = "Service Type Is Required"; break;
                    case "045": this.Message = " Location To  Is Required"; break;
                    case "046": this.Message = "Sorry You Have Registered As Service Provider You Can not  Add Any Requests"; break;
                    case "047": this.Message = "Request Is Required"; break;
                    case "048": this.Message = "Name Is Required"; break;
                    case "049": this.Message = "Sevice Provider Is Required"; break;
                    case "050": this.Message = "Rating Is Required"; break;
                    case "051": this.Message = "The Mobile field is not a valid phone number"; break;
                    case "052": this.Message = "Registerby is required"; break;
                    case "053": this.Message = "There is no faceBook Users"; break;
                    case "054": this.Message = "Email or password You have Entered is  not coorect"; break;
                    case "055": this.Message = "There is no active offers"; break;
                    case "056": this.Message = "this user is not admin"; break;
                    case "057": this.Message = "login Success"; break;
                    case "058": this.Message = "This Request Is Allready Accepted"; break;
                    case "059": this.Message = "Sorry You Must register As Service Provider To Put Priceing To Request"; break;
                    case "60": this.Message = "There Is No Request Detailes"; break;
                    case "61": this.Message = "Please Put your FacebookId"; break;
                    case "62": this.Message = "Accepted User Is Required"; break;
                    case "63": this.Message = " Number Of Trucks  Is Required"; break;
                    case "64": this.Message = " This service provider do not have any Companies"; break;
                    case "65": this.Message = " Any company can not put more than one proposal "; break;
                    case "66": this.Message = " There is no service Providers"; break;
                    case "67": this.Message = " There is no Customers"; break;
                    case "68": this.Message = " Request Starting Date Must Be Greater than Permission Date"; break;
                    case "69": this.Message = "Can't add a proposal to the same order more than once"; break;
                    case "70": this.Message = " Old password is not Correct"; break;
                    case "71": this.Message = " Please enter your new Password"; break;
                    case "72": this.Message = " Please enter correct password"; break;
                    case "73": this.Message = " Sorry this offer has been accepted"; break;
                    case "74": this.Message = " please enter customer type" ; break;
                    case "75": this.Message = " Sorry you can not send message after request is expired"; break;
                    case "76": this.Message = " Phone Is Required"; break;
                    case "77": this.Message = "Complaint Is Required"; break;
                    case "78": this.Message = "Longitude Is Required"; break;
                    case "79": this.Message = "Latitude Is Required"; break;
                    case "81": this.Message = "this region already exist"; break;
                    case "82": this.Message = "wrong username or password"; break;
                    case "83": this.Message = "First name Must be not  greater than 50 char"; break;
                    case "84": this.Message = "Second name Must be not  greater than 50 char"; break;
                    case "85": this.Message = "Min length 6 digits"; break;
                    case "86": this.Message = "Region is required"; break;
                    case "87": this.Message = "You have accepted this offer before"; break;
                    case "88": this.Message = "Your account must be active before login"; break;
                    case "89": this.Message = "Sorry this request is accepted "; break;
                    case "90": this.Message = "you put this request before"; break;
                    case "91": this.Message = "email not registered before"; break;
                    case "92": this.Message = "Mobile number or email is already exist"; break;
                    case "93": this.Message = "Name Is required"; break;
                    case "94": this.Message = "Message Is required"; break;
                    case "95": this.Message = "Sender Is required"; break;
                    case "96": this.Message = "Mobile No Is invalid"; break;
                    case "97": this.Message = "Please Edit Request Data To Be in CurrentRequests"; break;
                    case "98": this.Message = "Mobile Number Already Exist"; break;
                    case "99": this.Message = "Message With New Password has been Sent to your email"; break;
                    case "100": this.Message = "An error Occured In Server Please Try Again"; break;
                    case "101": this.Message = "You have registered as service provider so that login as service provider"; break;
                    case "102": this.Message = "You have registered as customer so that login as customer"; break;
                    case "103": this.Message = "Can not Add price to request that expire"; break;
                    case "104": this.Message = "new password sent in your email"; break;
                    case "105": this.Message = " You have add this truck/equipment before to your interests"; break;
                    case "106": this.Message = " You have add this region before to your interests"; break;
                    case "107": this.Message = "Region Name cn not be empty"; break;
                    case "108": this.Message = " new added"; break;
                    case "109": this.Message = "Cant not Delete "; break;
                }
            }

        }
    }
}