namespace API.Service.Mailing;

public interface IEmailTemplateService
{
    string GenerateEmailConfirmationMail(string userName, string email, string emailVerificationUri);
}