using Ejab.BAL.ModelViews.SmsMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.Services.SMS
{
  public interface ISmsService
    {
        bool ValidateMobile(ValidateMobileViwModel validationModel);
        bool ValidateEmail(string email );
        string  CreatVertyifyCode();
      
    }
}
