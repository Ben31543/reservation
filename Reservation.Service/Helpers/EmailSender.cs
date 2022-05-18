using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Reservation.Data.Entities;
using Reservation.Resources.Constants;

namespace Reservation.Service.Helpers
{
    public static class EmailSender
    {
        public static async Task SendEmailAboutReservationAsync(string emailTo, Reserving reservation, ServiceMember serviceMember)
        {
            string message =
                $"YOU HAVE A NEW ONLINE BOOKING #{reservation.Id}\n" +
                $"Booking date: {reservation.ReservationDate}\n" +
                $"Address: {reservation.ServiceMemberBranch.Name}, {reservation.ServiceMemberBranch.Address}\n" +
                $"Number of persons: {reservation.Tables}\n" +
                $"Dishes: {reservation.Dishes.ToProductsDisplayFormat()}\n" +
                $"Payment Type: {(reservation.IsOnlinePayment ? "Online" : "Cash")}" +
                $"\nAmount: {reservation.Amount}\n" +
                $"Take Out: {reservation.IsTakeOut}" +
                $"Notes: {reservation.Notes}";
            
            SendEmail(message, new (){emailTo});
        }

        public static async Task SendEmailAboutReservationCancelAsync(Reserving reserving)
        {
            string message = $"Reservation #{reserving.Id} for {reserving.ReservationDate.ToLongDateString()} has been cancelled.";
            SendEmail(message, new() {reserving.ServiceMember.Email});
        }

        public static async Task SendEmailFromUserAsync(string email, string memberId, string content)
        {
            string message = $"You have an email from {email} (member's Id: {memberId})\n {content}";
            SendEmail(message, new() {EmailCredentials.ReservationAdmin1Email, EmailCredentials.ReservationAdmin2Email});
        }
        
        private static void SendEmail(string text, List<string> emailReceivers)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(EmailCredentials.Username);

                foreach (var emailReceiver in emailReceivers)
                {
                    message.To.Add(emailReceiver);
                }

                message.Subject = "Online Reservation Service";
                message.Body = text;
                message.IsBodyHtml = true;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(EmailCredentials.Username, EmailCredentials.Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
