
using SendMailProject.Data;
using SendMailProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Net;
using System.Net.Mail;

namespace SendMailProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
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

                // ✅ Send email
                SendConfirmationEmail(registerView.Email, registerView.FullName);

                TempData["SuccessMessage"] = "User created successfully!";
                return RedirectToAction("Index");
            }

            return View(registerView);
        }

        // ✅ Add this private method
        private void SendConfirmationEmail(string toEmail, string fullName)
        {
            var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("3e59e45d57cb23", "6955de50f9d07b"),
                EnableSsl = true
            };

            string subject = "Welcome to Our App";
            string body = $"Hello {fullName},\n\nThank you for registering with us!";

            MailMessage message = new MailMessage(
                from: "from@example.com",         // You can change this to your Mailtrap "from"
                to: toEmail,
                subject,
                body
            );

            client.Send(message);
        }

    }
}




