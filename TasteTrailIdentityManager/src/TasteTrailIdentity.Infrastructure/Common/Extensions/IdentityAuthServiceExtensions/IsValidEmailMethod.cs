using TasteTrailIdentity.Core.Authentication.Services;

namespace TasteTrailIdentity.Infrastructure.Common.Extensions.IdentityAuthService;

public static class IsValidEmailMethod
{
    public static bool IsValidEmail(this IIdentityAuthService authservice, string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith(".")) {
            return false; 
        }
        try {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch {
            return false;
        }
    }
}
