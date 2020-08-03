using MimeKit;
using System;
using MailKit.Net.Smtp;
using System.Collections.Generic;
using System.Text;

namespace AzureMessageReceiver
{
    /// <summary>
    /// Class For SMTP .
    /// For Sending Email.
    /// </summary>
    public class SMTP
    {
        /// <summary>
        /// Function To Send Email.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mail"></param>
        /// <param name="data"></param>
        public void SendMail(string name, string mail, string data)
        {
            try
            {
                //Setting Additional Details With Message.
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Parking Lot", "shubhamdeulkar27@gmail.com"));
                message.To.Add(new MailboxAddress(name, mail));
                message.Subject = "Parking Information.";
                message.Body = new TextPart("plain")
                {
                    Text = data
                };

                //Connecting To Gmail Client and Sending Email.
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("shubhamdeulkar27@gmail.com", "nsdeulkar27");
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }
    }
}
