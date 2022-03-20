using MyAdminpro.Models.Repository.Intreface;
using MyAdminpro.Utils.Enums;
using MyAdminpro.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
            
            if(dbContext.Tbl_Users.Any(e => e.Email==model.Email))
            {
                return SignUpEnum.EmailExist;
            }
            else
            {
                var user = new Tbl_Users()
                {
                    Fname = model.Fname,
                    Lname = model.Lname,
                    Password = model.Password,
                    Gender = model.Gender,
                    Email = model.Email,
                };
                dbContext.Tbl_Users.Add(user);
                string Otp = GenerateOTP();
                SendMail(model.Email, Otp);
                var VAccount = new VerifyAccount()
                {
                    Otp = Otp,
                    Userid = model.Email,
                    SendTime = DateTime.Now
                };
                dbContext.VerifyAccounts.Add(VAccount);
                dbContext.SaveChanges();
                return SignUpEnum.Sucess;


            }
            return SignUpEnum.Failure;


        }


        private void SendMail(string to,string Otp)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(to);
            mail.From = new MailAddress("aicretions@gmail.com");
            mail.Subject = "Verify your account";
            string Body = $"Your OTP is <b> {Otp}</b> <br/> Thanks for choosing us.";
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("aicretions@gmail.com","s@1234556");
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

        private string GenerateOTP()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var list = Enumerable.Repeat(0, 8).Select(x => chars[random.Next(chars.Length)]);
            var r = string.Join("", list);
            return r;
           
        }

        public bool VerifyAccount(string Otp)
        {
            if(dbContext.VerifyAccounts.Any(e => e.Otp ==Otp))
            {
                var Acc = dbContext.VerifyAccounts.SingleOrDefault(e => e.Otp == Otp);
                var User = dbContext.Tbl_Users.SingleOrDefault(e => e.Email == Acc.Userid);
                User.IsVerified = true;
                User.IsActive = true;
                dbContext.VerifyAccounts.Remove(Acc);
                dbContext.Tbl_Users.Update(User);
                dbContext.SaveChanges();
                return true;

                
            }
            else
            {
                return false;
            }
        }
    }
}
