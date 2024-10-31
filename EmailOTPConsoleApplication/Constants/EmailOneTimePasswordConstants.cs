namespace EmailOTPConsoleApplication.Constants
{
    public static class EmailOneTimePasswordConstants
    {
        public static readonly string YourOTPCodeIs = "Your OTP Code is ";
        public static readonly string OtpCodeValid = ". The code is valid for 1 minute.";

        public static readonly string OtpIsValidAndChecked = "OTP is valid and checked";
        public static readonly string OtpIsWrongAfter10Tries = "OTP is wrong after 10 tries";
        public static readonly string OtpTimeout = "Timeout after 1 min";

        public static readonly string EmailSendStatusOk = "Email containing OTP has been sent successfully.";
        public static readonly string EmailSendFail = "Email address does not exist or sending to the email has failed.";
        public static readonly string EmailInvalid = "Email address is invalid.";

        public static readonly string InvalidOtp = "Please enter a valid OTP.";
    }
}
