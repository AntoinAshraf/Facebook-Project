using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Contracts;
using MimeKit;

namespace Facebook.Utilities
{
    public class Email : IEmail
    {
        private readonly IWebHostEnvironment env;
        private Contracts.IEmailSender emailSender;
        public Email(IWebHostEnvironment _env, Contracts.IEmailSender _emailSender)
        {
            env = _env;
            emailSender = _emailSender;
        }
        public void SendRecoveryPasswordEmail(string ReseverEmail, int RecoveryCode, string CallBackUrl)
        {
            var webRoot = env.WebRootPath;

            var pathToFile = env.WebRootPath
                + Path.DirectorySeparatorChar.ToString()
                + "Templates"
                + Path.DirectorySeparatorChar.ToString()
                + "EmailTemplate"
                + Path.DirectorySeparatorChar.ToString()
                + "ForgetPassword.html";

            var builder = new BodyBuilder();

            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {

                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            var subject = "Recover Password";

            string messageBody = string.Format(builder.HtmlBody,
                 ReseverEmail,
                 RecoveryCode,
                 CallBackUrl
             );

            emailSender.Send(ReseverEmail, subject, messageBody);
        }

        public void SendAccountActivationEmail(string ReseverEmail, string CallBackUrl)
        {
            var webRoot = env.WebRootPath;

            var pathToFile = env.WebRootPath
                + Path.DirectorySeparatorChar.ToString()
                + "Templates"
                + Path.DirectorySeparatorChar.ToString()
                + "EmailTemplate"
                + Path.DirectorySeparatorChar.ToString()
                + "ActivateAccount.html";

            var builder = new BodyBuilder();

            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {

                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            var subject = "Activate Account";

            string messageBody = string.Format(builder.HtmlBody,
                 ReseverEmail,
                 CallBackUrl
             );

            emailSender.Send(ReseverEmail, subject, messageBody);
        }
    }
}
