using Microsoft.Extensions.Options;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;
using VueJS.Services.Abstract;
using VueJS.Shared.Utilities.Results.Concrete;
using VueJS.Shared.Utilities.Results.Abstract;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using System.Net;
using System.Net.Mail;

namespace VueJS.Services.Concrete
{
    public class MailManager : IMailService
    {
        private readonly SmtpSettings _smtpSettings;

        public MailManager(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public IResult SendResetPassword(string email, string bodyHtml)
        {
            MailMessage message = new MailMessage
            {
                From = new MailAddress(_smtpSettings.SenderEmail, "Şifre Sıfırlama Talebi"),
                To = { new MailAddress(email) },
                Subject = "Şifre Sıfırlama",
                IsBodyHtml = true,
                Body = bodyHtml
            };
            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Server,
                Port = _smtpSettings.Port,
                EnableSsl = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            smtpClient.Send(message);
            return new Result(ResultStatus.Success, "E-Postanız başarılı bir şekilde gönderilmiştir.");
        }

        public IResult Send(EmailSendDto emailSendDto, string senderName)
        {
            MailMessage message = new MailMessage
            {
                From = new MailAddress(_smtpSettings.SenderEmail, senderName),
                To = { new MailAddress(emailSendDto.Email) },
                Subject = emailSendDto.Subject,
                IsBodyHtml = true,
                Body = emailSendDto.Message,
            };
            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Server,
                Port = _smtpSettings.Port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            smtpClient.Send(message);
            return new Result(ResultStatus.Success, "E-Postanız başarılı bir şekilde gönderilmiştir.");
        }

        public IResult SendContactEmail(EmailSendDto emailSendDto, string senderName)
        {
            MailMessage message = new MailMessage
            {
                From = new MailAddress(_smtpSettings.SenderEmail, senderName),
                To = { new MailAddress(_smtpSettings.SenderEmail) },
                Subject = emailSendDto.Subject,
                IsBodyHtml = true,
                Body = $"Gönderen Kişi: {emailSendDto.Name}<br/>Gönderen E-Posta Adresi: {emailSendDto.Email}<br/><br/>{emailSendDto.Message}",
            };
            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Server,
                Port = _smtpSettings.Port,
                EnableSsl = false,
                UseDefaultCredentials = _smtpSettings.IsEnableSsl,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            smtpClient.Send(message);

            return new Result(ResultStatus.Success, "E-Postanız başarılı bir şekilde gönderilmiştir.");
        }

        //public IResult ReCaptcha(CaptchaResponseDto captchaResponseDto, string senderName)
        //{
        //    MailMessage message = new MailMessage
        //    {
        //        From = new MailAddress(_smtpSettings.SenderEmail, senderName),
        //        To = { new MailAddress(_smtpSettings.SenderEmail) },
        //        Subject = emailSendDto.Subject,
        //        IsBodyHtml = true,
        //        Body = $"Gönderen Kişi: {emailSendDto.Name}<br/>Gönderen E-Posta Adresi: {emailSendDto.Email}<br/><br/>{emailSendDto.Message}",
        //    };
        //    SmtpClient smtpClient = new SmtpClient
        //    {
        //        Host = _smtpSettings.Server,
        //        Port = _smtpSettings.Port,
        //        EnableSsl = false,
        //        UseDefaultCredentials = _smtpSettings.IsEnableSsl,
        //        Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
        //        DeliveryMethod = SmtpDeliveryMethod.Network
        //    };
        //    smtpClient.Send(message);

        //    return new Result(ResultStatus.Success, "E-Postanız başarılı bir şekilde gönderilmiştir.");
        //}
    }
}