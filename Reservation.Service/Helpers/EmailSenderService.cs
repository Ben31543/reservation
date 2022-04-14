using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Service.Helpers
{
    public class EmailSenderService
    {
        public  static void NotifyServiceMemberAsync(string text, string toEmail)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp= new SmtpClient();
                message.From=new MailAddress("onlinereservationservice@gmail.com");
                message.To.Add(toEmail);
                message.Subject = "Online Booking";
                message.Body = text;
                message.IsBodyHtml = true;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("onlinereservationservice@gmail.com", "onlinereservationservice2022_");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
