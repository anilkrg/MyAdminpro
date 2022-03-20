using MyAdminpro.Utils.Enums;
using MyAdminpro.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAdminpro.Models.Repository.Intreface
{
   public interface IUsers
    {
        SignInEnum SignIn(SignInModel model);
        SignUpEnum SignUp(SignUpModel model);
        bool VerifyAccount(string Otp);

    }
}
