using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using ch.thommenmedia.common.Extensions;

namespace ch.thommenmedia.common.Utils
{
    public static class MailHelper
    {
        public static string SmtpServer { get; set; } = "mg.rtonline.ch";

        public static string DefaultSender { get; set; } = "aigis support@aigis.ch";

        public static string DefaultUser { get; set; } = "";

        public static string DefaultPass { get; set; } = "";


        /// <summary>
        ///     send message to one receipient
        /// </summary>
        /// <param name="address">List of adresses to send the message to</param>
        /// <param name="body">The body.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="from">From. (default: emedia@tpcag.ch)</param>
        /// <param name="html">Gibt an ob der Body HTML ist oder nicht (default: true)</param>
        /// <param name="encoding">The Encoding of the message (default: utf8)</param>
        /// <returns></returns>
        public static bool SendMail(string address, string body, string subject,
            string from = null, bool html = true,
            Encoding encoding = null)
        {
            return SendMail(new[] {address}, body, subject, from, html, encoding);
        }

        /// <summary>
        ///     Sends the mail.
        /// </summary>
        /// <param name="addresses">List of adresses to send the message to</param>
        /// <param name="body">The body.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="from">From. (default: emedia@tpcag.ch)</param>
        /// <param name="html">Gibt an ob der Body HTML ist oder nicht (default: true)</param>
        /// <param name="encoding">The Encoding of the message (default: utf8)</param>
        /// <returns></returns>
        public static bool SendMail(string[] addresses, string body, string subject, string from = null,
            bool html = true, Encoding encoding = null)
        {
            return SendMailWithAttachments(addresses, body, subject, from, html, encoding);
        }

        /// <summary>
        ///     Sends mail with attachments to one recipient
        /// </summary>
        /// <param name="address">Adress of recipient</param>
        /// <param name="body">The body.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="from">From. (default: emedia@tpcag.ch)</param>
        /// <param name="html">Gibt an ob der Body HTML ist oder nicht (default: true)</param>
        /// <param name="encoding">The Encoding of the message (default: utf8)</param>
        /// <param name="attachments">List of mail attachments</param>
        /// <param name="priority">Mail priority</param>
        /// <returns></returns>
        public static bool SendMailWithAttachments(string address, string body, string subject,
            string from = null, bool html = true,
            Encoding encoding = null,
            IEnumerable<Attachment> attachments = null,
            MailPriority priority = MailPriority.Normal)
        {
            return SendMailWithAttachments(new[] {address}, body, subject, from, html, encoding, attachments, priority);
        }

        /// <summary>
        ///     Sends mail with attachments to multiple recipients
        /// </summary>
        /// <param name="addresses">List of adresses to send the message to</param>
        /// <param name="body">The body.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="from">From. (default: emedia@tpcag.ch)</param>
        /// <param name="html">Gibt an ob der Body HTML ist oder nicht (default: true)</param>
        /// <param name="encoding">The Encoding of the message (default: utf8)</param>
        /// <param name="attachments">List of mail attachments</param>
        /// <param name="priority">Mail priority</param>
        /// <returns></returns>
        public static bool SendMailWithAttachments(string[] addresses, string body, string subject,
            string from = null, bool html = true,
            Encoding encoding = null,
            IEnumerable<Attachment> attachments = null,
            MailPriority priority = MailPriority.Normal)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            if (from == null) from = DefaultSender;

            try
            {
                var smtpClient = new SmtpClient(SmtpServer);
                if (!string.IsNullOrEmpty(DefaultUser) && !string.IsNullOrEmpty(DefaultPass))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(DefaultUser, DefaultPass);
                }

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(@from),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = html,
                    BodyEncoding = encoding,
                    Priority = priority
                };

                if (attachments != null)
                {
                    foreach (var attachment in attachments)
                        mailMessage.Attachments.Add(attachment);
                }

                foreach (var address in addresses)
                    mailMessage.Bcc.Add(address);
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
#if DEBUG || TEST
                throw ex;
#else
                return false;
#endif
            }
        }

        /// <summary>
        ///     Generiert ein mailto-Link (mailto:test@example)
        /// </summary>
        /// <param name="mailParams">
        ///     URL Parameter als Dictionay. D.h. der Name des Parameters mit dem entsprechenden Wert.
        ///     Mögliche Parameter sind:
        ///     - to  -> Empfänger(s) Sofern mehrere Empfänger müssen diese mit einem , getrennt sein
        ///     - cc  -> CC  (bei mehreren auch ,-getrennt)
        ///     - bcc -> BCC (bei mehreren auch ,-getrennt)
        ///     - subject -> Betref
        ///     - body -> E-Mail Text (Ist nur Plaintext möglich)
        /// </param>
        /// <returns>mailto-Link</returns>
        public static string GenerateMailToLink(Dictionary<string, string> mailParams)
        {
            var mailToLink = "mailto:{0}?cc={1}&bcc={2}&subject={3}&body={4}";
            var to = "";
            var cc = "";
            var bcc = "";
            var subject = "";
            var body = "";


            if (mailParams != null && mailParams.Any())
            {
                foreach (var mailParam in mailParams)
                {
                    if (mailParam.Key == "to")
                        to = !string.IsNullOrEmpty(mailParam.Value) ? mailParam.Value : "";

                    if (mailParam.Key == "cc")
                        cc = !string.IsNullOrEmpty(mailParam.Value) ? mailParam.Value : " ";

                    if (mailParam.Key == "bcc")
                        bcc = !string.IsNullOrEmpty(mailParam.Value) ? mailParam.Value : "";

                    if (mailParam.Key == "subject")
                        subject = !string.IsNullOrEmpty(mailParam.Value) ? mailParam.Value : "";

                    if (mailParam.Key == "body")
                        body = !string.IsNullOrEmpty(mailParam.Value) ? mailParam.Value : "";
                }

                mailToLink = mailToLink.Apply(to, cc, bcc, subject, body);
            }

            return mailToLink;
        }

        /// <summary>
        ///     Generiert ein mailto-Link (mailto:test@example)
        /// </summary>
        /// <param name="message">Mailnachricht</param>
        /// <returns></returns>
        public static string GenerateMailToLink(MailMessage message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            var mailParams = new Dictionary<string, string>
            {
                {"to", message.To.Any() ? message.To.Select(s => s.Address).Aggregate((a, b) => a + ";" + b) : ""},
                {"cc", message.CC.Any() ? message.CC.Select(s => s.Address).Aggregate((a, b) => a + ";" + b) : ""},
                {"bcc", message.Bcc.Any() ? message.Bcc.Select(s => s.Address).Aggregate((a, b) => a + ";" + b) : ""},
                {"subject", message.Subject},
                {"body", message.Body}
            };

            return GenerateMailToLink(mailParams);
        }
    }
}