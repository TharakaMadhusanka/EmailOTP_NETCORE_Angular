using EmailOTPConsoleApplication.Constants;
using EmailOTPConsoleApplication.Interfaces;
using System.Text.RegularExpressions;

namespace EmailOTPConsoleApplication.Services
{
    public sealed class EmailOneTimePasswordService : IEmailOneTimePasswordService
    {
        private string? Email { get; set; }

        // Use configuration file to read these
        private readonly string _otpDomain = "dso.org.sg";

        private readonly Regex _emailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));

        #region Generate Email OTP
        public (bool isValidEmail, string? emailBody) GenerateEmailOneTimePassword(string userEmail)
        {
            var y = userEmail.EndsWith(_otpDomain);
            if (!IsValidEmail(userEmail) || !userEmail.EndsWith(_otpDomain))
            {
                return (false, null);
            }
            Email = userEmail;
            string otpCode = GenerateRandomOTP();
            string emailBody = $"{EmailOneTimePasswordConstants.YourOTPCodeIs}{otpCode}{EmailOneTimePasswordConstants.OtpCodeValid}";

            return (true, emailBody);
        }

        private bool IsValidEmail(string email)
        {
            var x = _emailRegex.IsMatch(email);
            return x;
        }

        private static string GenerateRandomOTP()
        {
            Random random = new();
            return random.Next(100000, 999999).ToString();
        }
        #endregion

        #region Validate OTP
        public async Task<OTPStatus> CheckOneTimePassword(string inputOtp)
        {
            string generatedUserOtp;
            try
            {
                generatedUserOtp = await RetrieveOneTimePassword();
            }
            catch (TimeoutException)
            {
                return OTPStatus.STATUS_OTP_TIMEOUT;
            }

            if (string.Equals(inputOtp, generatedUserOtp, StringComparison.OrdinalIgnoreCase))
            {
                return OTPStatus.STATUS_OTP_OK;
            }

            return OTPStatus.STATUS_OTP_FAIL;

        }

        public async Task<string> RetrieveOneTimePassword()
        {
            // Retrieve the current OTP saved in DB 
            // to get the OTP sent out
            await Task.Delay(2000);
            return "123456";
        }
        #endregion

    }
}
