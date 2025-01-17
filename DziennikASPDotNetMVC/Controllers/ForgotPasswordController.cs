using DziennikASPDotNetMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Net.Mail;
using System.Net;
using DziennikASPDotNetMVC.Models.ResetPasswordModels;

namespace DziennikASPDotNetMVC.Controllers
{
    public class ForgotPasswordController : Controller
    {
        MyDbContext db;
        public ForgotPasswordController(MyDbContext db) 
        { 
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ForgotPassword(Models.ResetPasswordModels.ForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await db.User.FirstOrDefaultAsync(u => u.login == model.login);

                if (user != null)
                {
                    var token = GeneratePasswordResetToken(user);

                    var callbackUrl = Url.Action(nameof(ResetPassword), "ForgotPassword", new { token, login = model.login }, Request.Scheme);

                    await SendEmailAsync(model.email, "Reset Password",
                        $"Please reset your password by clicking <a href='{callbackUrl}'>here</a>.");

                    return View("ForgotPasswordConfirmation");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Nie znaleziono użytkownika z podanym loginem i adresem e-mail.");
                }
            }

            return View(model);
        }

        private string GeneratePasswordResetToken(User user)
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        private async Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("mateoprokop@gmail.com", "zhnk exxu vrwt njfq"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("mateoprokop@gmail.com"),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }

        public IActionResult ResetPassword(string token, string login)
        {
            // Jeśli token lub email są null, zwróć błąd
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(login))
            {
                return View();
            }
            else
            {
                var model = new ResetPasswordViewModel
                {
                    token = token,
                    login = login
                };
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.newPassword != model.confirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Hasła nie są zgodne.");
                    return View(model);
                }

                var user = await db.User.FirstOrDefaultAsync(u => u.login == model.login);

                if (user != null)
                {
                    user.password = model.newPassword;

                    await db.SaveChangesAsync();
                    return RedirectToAction("ResetPasswordConfirmation");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Nie znaleziono użytkownika.");
                    return View(model);
                }
            }

            return View(model);
        }
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}
