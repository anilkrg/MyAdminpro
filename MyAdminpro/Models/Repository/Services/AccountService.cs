using MyAdminpro.Models.Repository.Intreface;
using MyAdminpro.Utils.Enums;
using MyAdminpro.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAdminpro.Models.Repository.Services
{
    public class AccountService : IUsers
    {
        private MSDBContext dbContext;

        public AccountService()
        {
            dbContext = new MSDBContext();
        }

        public SignInEnum SignIn(SignInModel model)
        {
            var user = dbContext.Tbl_Users.SingleOrDefault(e => e.Email == model.Email && e.Password == model.Password);
            if (user != null)
            {
                if (user.IsVerified)
                {
                    if (user.IsActive)
                    {
                        return SignInEnum.Sucess;
                    }
                    else
                    {
                        return SignInEnum.Inactive;
                    }
                }
                else
                {
                    return SignInEnum.NotVerified;
                }
            }
            else
            {
                return SignInEnum.WrongCredentials;
            }
        }

        public SignUpEnum SignUp(SignUpModel model)
        {
            throw new NotImplementedException();
        }
    }
}
