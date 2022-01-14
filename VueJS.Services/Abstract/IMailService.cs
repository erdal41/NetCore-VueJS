using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;

namespace VueJS.Services.Abstract
{
    public interface IMailService
    {
        IResult SendResetPassword(string email, string bodyHtml);
        IResult Send(EmailSendDto emailSendDto, string senderName);
        IResult SendContactEmail(EmailSendDto emailSendDto, string senderName);
    }
}