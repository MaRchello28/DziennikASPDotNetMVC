using DziennikASPDotNetMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using DziennikASPDotNetMVC.Models.ResetPasswordModels;

namespace DziennikASPDotNetMVC.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private readonly MyDbContext db;

        public ForgotPasswordController(MyDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await db.User.FirstOrDefaultAsync(u => u.login == model.login);

                if (user != null)
                {
                    var token = GeneratePasswordResetToken(user);

                    var callbackUrl = Url.Action(nameof(ResetPassword), "ForgotPassword", new { token, login = model.login }, Request.Scheme);

                    await SendEmailAsync(user.email, "Reset Password", $"Naciśnij, żeby zresetować hasło: <a href='{callbackUrl}'>Resetuj Hasło</a>");

                    return View("ForgotPasswordConfirmation");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Nie znaleziono użytkownika z podanym loginem.");
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
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(login))
            {
                return View("Error");
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