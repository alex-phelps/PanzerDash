using System;
using System.Net;
using System.Net.Mail;

namespace BPA_Tank_Racer_Game
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();

            // For crash reporting, commented out for debug atm

            /*
            try
            {
                using (var game = new Game1())
                    game.Run();
            }
            catch (Exception exception)
            {
                //Send a crash report email

                string userMessage = Microsoft.VisualBasic.Interaction.InputBox(
                    "Panzer Dash Has Crashed!\n\nTo help us fix these errors," +
                    "please provide any information on the situation at the time of the crash.",
                    "Crash Report");

                var fromAddress = new MailAddress("panzerdashgame@gmail.com", "User");
                var toAddress = new MailAddress("panzerdashgame@gmail.com", "Me");
                string subject = "Crash Report";
                string body = userMessage + "\n\n" +
                    "Crash Info:\n\n" + exception.Message + "\n"  + exception.Data +  
                    "\n" + exception.StackTrace + "\n" + exception.Source;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromAddress.Address, "monogame4life"),
                    Timeout = 20000
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            }
            */
        }
    }
#endif
}
