using EmailOTPConsoleApplication.Constants;

namespace EmailOTPConsoleApplication.Interfaces
{
    public interface IEmailOneTimePasswordService
    {
        public (bool isValidEmail, string? emailBody) GenerateEmailOneTimePassword(string userEmail);
        public Task<OTPStatus> CheckOneTimePassword(string inputOtp);
    }
}
