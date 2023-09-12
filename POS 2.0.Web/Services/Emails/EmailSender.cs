//using Clinic.Web.Data;
using POS_2._0.Web.Data;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Clinic.Web.Services.Emails
{
    public class EmailSender : IEmailSender
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext db;
        public EmailSender(/*IWebHostEnvironment env,*/ ApplicationDbContext db)
        {
            //_env = env;
            this.db = db;
        }

        public async Task Send(string to, string subject , string body)
        {
            //var model = db.Settings.FirstOrDefault();
            //string email = model.EmailSend;
            //string password = model.Password;
            //string title = model.Title;
            //string body = model.Body;

            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            message.From = new MailAddress("test@gmail.com", "Test Maessage");
            message.Subject = subject;
            //message.IsBodyHtml = true;
            message.Body = body;

            //var path = Path.Combine(_env.WebRootPath, "Attachments", "ee136a5070c543c596d287d9445c0f87.docx");
            //message.Attachments.Add(new Attachment(new MemoryStream(
            //    await File.ReadAllBytesAsync(path)),
            //    Guid.NewGuid().ToString()));

            using var emailClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("test@gmail.com", "*************")
            }; 
            await emailClient.SendMailAsync(message);
        }
    }
}
