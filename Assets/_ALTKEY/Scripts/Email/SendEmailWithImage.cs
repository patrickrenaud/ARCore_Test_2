using UnityEngine;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net.Mime;

namespace ALTKEY
{
    public class SendEmailWithImage : MonoBehaviour
    {
        public string emailAdress;
        public string emailPassword;
        public string debugEmailDest;
        public string emailSubject;
        public string emailBody;

        string filePath;

        public void SetFilePath(string filePath)
        {
            this.filePath = filePath;
        }

        public void SendMail(string emailDest)
        {
            MailMessage mailWithImg = CreateMailWithImage(emailDest);

            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            smtpServer.Port = 587;
            smtpServer.Credentials = new NetworkCredential(emailAdress, emailPassword) as ICredentialsByHost;
            smtpServer.EnableSsl = true;
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };
            smtpServer.Send(mailWithImg);
        }

        private MailMessage CreateMailWithImage(string emailDest)
        {
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;
            Attachment att = new Attachment(filePath);
            mail.Attachments.Add(att);
            mail.AlternateViews.Add(GetEmbeddedImage(filePath));
            mail.From = new MailAddress(emailAdress);
            mail.Subject = emailSubject;
            mail.Body = String.Format(emailBody, att.ContentId);

            // Change this when ready
            mail.To.Add(debugEmailDest);
            return mail;
        }

        private AlternateView GetEmbeddedImage(String filePath)
        {
            LinkedResource res = new LinkedResource(filePath, MediaTypeNames.Image.Jpeg);
            res.ContentId = Guid.NewGuid().ToString();
            string htmlBody = @"<img src='cid:" + res.ContentId + @"'/>";
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(res);
            return alternateView;
        }

    }
}
