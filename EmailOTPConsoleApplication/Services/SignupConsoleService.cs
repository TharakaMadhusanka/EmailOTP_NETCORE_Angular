using EmailOTPConsoleApplication.Constants;
using EmailOTPConsoleApplication.Interfaces;

namespace EmailOTPConsoleApplication.Services
{
    public class SignupConsoleService(IEmailOneTimePasswordService emailOneTimePasswordService) : ISignUpConsole
    {
        private readonly IEmailOneTimePasswordService _emailOneTimePasswordService = emailOneTimePasswordService;
        private readonly int _maxRetries = 10;
        private readonly TimeSpan _otpTimeout = TimeSpan.FromMinutes(1);
        public int tries = 1;

        public async Task Start()
        {
            GenerateEmailOneTimePassword();
            Console.WriteLine("*****************************************************");
            await CheckOneTimePassword();
        }

        #region Generate OTP and Send Email
        public void GenerateEmailOneTimePassword()
        {
            Console.WriteLine("Please enter your email");
            string? userEmail = Console.ReadLine();
            if (userEmail == null)
            {
                return;
            }
            userEmail = userEmail.Trim();
            var (isValidEmail, emailBody) = _emailOneTimePasswordService.GenerateEmailOneTimePassword(userEmail);
            if (!isValidEmail)
            {
                Console.WriteLine(EmailOneTimePasswordConstants.EmailInvalid);
                return;
            }
            var emailStatus = SendEmail(userEmail, emailBody!);
            Console.WriteLine(emailStatus);
        }

        public static string SendEmail(string emailAddress, string emailBody)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Check and Validate OTP
        public async Task CheckOneTimePassword()
        {
            using var cts = new CancellationTokenSource(_otpTimeout);
            var token = cts.Token;

            while (tries <= _maxRetries && !token.IsCancellationRequested)
            {
                Console.Write("Enter OTP: ");

                string? otp = Console.ReadLine();

                if (string.IsNullOrEmpty(otp) || otp.Length > 6)
                {
                    Console.WriteLine(EmailOneTimePasswordConstants.InvalidOtp);
                    return;
                }

                var status = await _emailOneTimePasswordService.CheckOneTimePassword(otp!);

                if (status == OTPStatus.STATUS_OTP_OK)
                {
                    Console.WriteLine(EmailOneTimePasswordConstants.OtpCodeValid);
                    return;
                }

                if (status == OTPStatus.STATUS_OTP_TIMEOUT)
                {
                    Console.WriteLine(EmailOneTimePasswordConstants.OtpTimeout);
                    return;
                }
                tries++;
            }

            Console.WriteLine(EmailOneTimePasswordConstants.OtpIsWrongAfter10Tries);
        }
        #endregion
    }
}
