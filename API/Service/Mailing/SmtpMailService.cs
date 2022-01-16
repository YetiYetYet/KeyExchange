using System.Net.Mail;

namespace API.Service.Mailing;

public class SmtpMailService : IMailService
{
    private IConfiguration _configuration;
    private MailSettings? _settings;
    private readonly ILogger<SmtpMailService> _logger;

    public SmtpMailService(IConfiguration configuration, ILogger<SmtpMailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _settings = _configuration.GetSection("MailSettings").Get<MailSettings>();
    }

    public void SendTestAsync()
    {
        Console.WriteLine("_settings.From");
        Console.WriteLine(_settings.From);
        var smtp = new Smtp.Smtp();
        smtp.SetHost(_settings.Host);
        smtp.SetPort(_settings.Port);
        smtp.SetUsername(_settings.UserName);
        smtp.SetPassword(_settings.Password);
        
        smtp.Mail(
            new MailAddress(_settings.From, _settings.DisplayName),
            new MailAddress("yeti931995@gmail.com", "YetiYetYet"),
            "Test SMTP",
            "UwU"
        );
    }
}