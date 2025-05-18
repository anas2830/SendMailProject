
using SendMailProject.Data;
using SendMailProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace SendMailProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AccountController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            var users = _context.Users.ToList();

            return View(users);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Models.RegisterViewModel registerView)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(registerView);
                _context.SaveChanges();

                SendConfirmationEmail(registerView.Email, registerView.FullName);

                TempData["SuccessMessage"] = "User created successfully!";
                return RedirectToAction("Index");
            }

            return View(registerView);
        }

        private void SendConfirmationEmail(string toEmail, string fullName)
        {
            string host = _configuration["MailSettings:Host"];
            int port = int.Parse(_configuration["MailSettings:Port"]);
            string username = _configuration["MailSettings:Username"];
            string password = _configuration["MailSettings:Password"];
            string fromEmail = _configuration["MailSettings:FromEmail"];

            var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };

            var mail = new MailMessage();
            mail.From = new MailAddress(fromEmail);
            mail.To.Add(toEmail);
            mail.Subject = "Welcome to Our App!";
            mail.Body = $"Hello {fullName},\n\nYour registration was successful.";

            client.Send(mail);
        }

    }
}




