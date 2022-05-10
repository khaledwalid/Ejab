using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ejab.BAL.ModelViews.SmsMessage;
using Ejab.BAL.UnitOfWork;
using Ejab.BAL.Common;

namespace Ejab.BAL.Services.SMS
{
    public class SmsService : ISmsService
    {
        IUnitOfWork _uow;
         ModelFactory factory;
        private static string _numbers = "0123456789";
        Random random = new Random();
        public SmsService(IUnitOfWork uow)
        {
            this._uow = uow;
            factory = new ModelFactory();

        }
        public bool ValidateMobile(ValidateMobileViwModel validationModel)
        {
            //if (validationModel.Mobile == null)
            //{
            //    throw new Exception("020");
            //}
            return _uow.User.GetAll(x => x.FlgStatus == 1, null, "").Any(y => (y.Mobile == validationModel.Mobile) );
        }
       
        public string CreatVertyifyCode()
        {
            StringBuilder builder = new StringBuilder(6);
            string numberAsString = "";
            int numberAsNumber = 0;

            for (var i = 0; i < 4; i++)
            {
                builder.Append(_numbers[random.Next(0, _numbers.Length)]);
            }

            numberAsString = builder.ToString();
            numberAsNumber = int.Parse(numberAsString);
            return numberAsString;

        }

        public bool ValidateEmail(string  email )
        {
            if (email == null)
            {
                throw new Exception("022");
            }
            return _uow.User.GetAll(x => x.FlgStatus == 1, null, "").Any(y => (y.Email  == email) );
        }



        //public string  CreatVertyifyCode()
        //{
        //    int min = 5;
        //    int max =9;
        //    Random mycode = new Random(1);
        //    return mycode.Next(min,max ).ToString("D4");

        //}


    }
}
