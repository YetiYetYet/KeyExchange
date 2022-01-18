namespace API.Utils.Validator;

public class RegisterRequirementSettings
{
    public UsernameRequirement UsernameRequirement { get; set; }
    public PasswordRequirement PasswordRequirement { get; set; }
    public MailRequirement MailRequirement { get; set; }
}

public class UsernameRequirement
{
    public int MinimumLength { get; set; }
    public int MaxLength { get; set; }
    public bool Required { get; set; }
}

public class PasswordRequirement
{
    public int MinimumLength { get; set; }
    public int MaxLength { get; set; }
    public bool Required { get; set; }
}

public class MailRequirement
{
    public bool IsMail { get; set; }
    public bool Required { get; set; }
}
