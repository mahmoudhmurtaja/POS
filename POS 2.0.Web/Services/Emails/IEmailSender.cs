using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Web.Services.Emails
{
    public interface IEmailSender
    {
        Task Send(string to, string subject, string body);
    }
}
