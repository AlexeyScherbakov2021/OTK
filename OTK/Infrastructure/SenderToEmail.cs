using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using OTK.Models;

namespace OTK.Infrastructure
{
    internal class SenderToEmail
    {
        private string _Email;


        public SenderToEmail(Users user)
        {
            _Email = user.UserEmail;
        }


        public async void SendMail(string Text)
        {
            //if (string.IsNullOrEmpty(_Email))
                return;

            string Message =
                "<html>" +
                    "<body>" +
                    "<p>" +
                        "Вам необходимо рассмотреть " + Text +
                        ". Ссылка на программу - <a href=\"file:///s:/Общие документы НГК/OTK.exe\">ПО ОТК</a>" +
                    "</p>" +
                    "<p>" +
                    "s:\\Общие документы НГК\\OTK.exe" +
                    "</p>" +
                    "</body>" +
                "</html>";


            MailAddress from = new MailAddress("otk@ngk-ehz.ru", "OTK PO");
            MailAddress to = new MailAddress(_Email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Оповещение";

            m.IsBodyHtml = true;
            m.Body = Message;


            SmtpClient smtp = new SmtpClient("zimbra.lancloud.ru", 587);

            smtp.Credentials = new NetworkCredential("a.scherbakov@ngk-ehz.ru", "Jjbr9uxa");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
        }


    }
}
