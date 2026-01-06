using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace HealthBite.Services
{
    public static class EmailService
    {
        private const string SmtpUser = "tunaatatedu@gmail.com";
        private const string SmtpPassword = "zxjh xwwy hecn jidc";
        private const string SenderName = "HealthBite Destek";

        /// <summary>
        /// Yeni kullanıcı kaydı için hesap doğrulama e-postası gönderir.
        /// </summary>
        public static async Task SendAccountVerificationEmailAsync(string recipientEmail, string recipientName, string verificationCode)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(SenderName, SmtpUser));
            message.To.Add(new MailboxAddress(recipientName, recipientEmail));
            message.Subject = "HealthBite Hesap Doğrulama Kodunuz";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                    <div style='font-family: Arial, sans-serif; text-align: center; color: #333;'>
                        <h2>HealthBite'a Hoş Geldiniz!</h2>
                        <p>Hesabınızı doğrulamak için lütfen aşağıdaki kodu kullanın:</p>
                        <p style='font-size: 24px; font-weight: bold; letter-spacing: 5px; background-color: #f2f2f2; padding: 10px; border-radius: 5px;'>
                            {verificationCode}
                        </p>
                        <p>Bu isteği siz yapmadıysanız, bu e-postayı görmezden gelebilirsiniz.</p>
                    </div>"
            };
            message.Body = bodyBuilder.ToMessageBody();

            await SendEmailAsync(message); // Ortak gönderim metodunu çağır
        }

        /// <summary>
        /// Şifresini unutan kullanıcı için şifre sıfırlama kodu gönderir.
        /// </summary>
        public static async Task SendPasswordResetEmailAsync(string recipientEmail, string recipientName, string verificationCode)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(SenderName, SmtpUser));
            message.To.Add(new MailboxAddress(recipientName, recipientEmail));
            message.Subject = "HealthBite Şifre Sıfırlama Kodunuz";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                    <div style='font-family: Arial, sans-serif; text-align: center; color: #333;'>
                        <h2>Şifre Sıfırlama İsteği</h2>
                        <p>Hesabınız için bir şifre sıfırlama talebinde bulundunuz. Sıfırlama kodunuz aşağıdadır:</p>
                        <p style='font-size: 24px; font-weight: bold; letter-spacing: 5px; background-color: #f2f2f2; padding: 10px; border-radius: 5px;'>
                            {verificationCode}
                        </p>
                        <p>Bu isteği siz yapmadıysanız, bu e-postayı görmezden gelebilirsiniz.</p>
                    </div>"
            };
            message.Body = bodyBuilder.ToMessageBody();
            
            await SendEmailAsync(message); // Ortak gönderim metodunu çağır
        }

        /// <summary>
        /// E-posta gönderme işlemini gerçekleştiren özel yardımcı metot.
        /// </summary>
        private static async Task SendEmailAsync(MimeMessage message)
        {
            if (string.IsNullOrEmpty(SmtpUser) || string.IsNullOrEmpty(SmtpPassword) || SmtpUser.StartsWith("SENIN-GMAIL"))
            {
                await App.Current.MainPage.DisplayAlert("Yapılandırma Hatası", "E-posta servisi ayarlanmamış.", "Tamam");
                return;
            }

            using (var client = new SmtpClient())
            {
                try
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(SmtpUser, SmtpPassword);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Gmail gönderme hatası: {ex.ToString()}");
                    await App.Current.MainPage.DisplayAlert("Detaylı Gönderim Hatası", ex.Message, "Tamam");
                    throw;
                }
            }
        }
    }
}
