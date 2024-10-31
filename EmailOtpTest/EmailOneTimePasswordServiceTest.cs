using EmailOTPConsoleApplication.Constants;
using EmailOTPConsoleApplication.Services;

namespace EmailOtpTest
{
    public class EmailOneTimePasswordServiceTest
    {

        #region Constraint Check
        public static IEnumerable<object[]> OneTimePasswordGenerateConstraintsCheckMemberData()
        {
            yield return new object[] { "InvalidEmail", "test" };
            yield return new object[] { "InvalidEmailWithNonAllowedOrg", "dosorgtest@gmail.com" };
        }

        [Theory]
        [MemberData(nameof(OneTimePasswordGenerateConstraintsCheckMemberData))]
        public void EmailOneTimePasswordService_CheckValidEmail_Positive(string scenario, string userInput)
        {
            switch (scenario)
            {
                case "InvalidEmail":
                    // Arrange
                    var emailService1 = new EmailOneTimePasswordService();

                    // Act
                    var (isValidEmail, _) = emailService1.GenerateEmailOneTimePassword(userInput);

                    // Assert
                    Assert.False(isValidEmail);
                    break;
                case "InvalidEmailWithNonAllowedOrg":
                    // Arrange
                    var emailService2 = new EmailOneTimePasswordService();

                    // Act
                    var results2 = emailService2.GenerateEmailOneTimePassword(userInput);

                    // Assert
                    Assert.False(results2.isValidEmail);
                    break;

            }


        }
        #endregion

        #region Constraint Check
        [Theory]
        [InlineData("testemail@dso.org.sg")]
        public void EmailOneTimePasswordService_GenerateEmail_Positive(string userInput)
        {
            // Arrange
            var service = new EmailOneTimePasswordService();

            // Act
            var (isValidEmail, emailBody) = service.GenerateEmailOneTimePassword(userInput);

            // Assert
            Assert.True(isValidEmail);
            Assert.Contains("Your OTP Code is ", emailBody!);
        }
        #endregion

        #region Check OTP
        [Fact]
        public async Task CheckOneTimePassword_ValidOtp_ReturnsSuccess()
        {
            // Arrange
            string validOtp = "123456";
            var service = new EmailOneTimePasswordService();

            // Act
            var result = await service.CheckOneTimePassword(validOtp);

            // Assert
            Assert.Equal(OTPStatus.STATUS_OTP_OK, result);
        }

        [Fact]
        public async Task CheckOneTimePassword_InvalidOtp_ReturnsFailure()
        {
            // Arrange
            string validOtp = "567567";
            var service = new EmailOneTimePasswordService();

            // Act
            var result = await service.CheckOneTimePassword(validOtp);

            // Assert
            Assert.Equal(OTPStatus.STATUS_OTP_FAIL, result);
        }
        #endregion
    }
}