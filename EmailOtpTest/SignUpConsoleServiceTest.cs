using EmailOTPConsoleApplication.Constants;
using EmailOTPConsoleApplication.Interfaces;
using EmailOTPConsoleApplication.Services;
using Moq;

namespace EmailOtpTest
{
    public class SignUpConsoleServiceTest
    {
        private readonly Mock<IEmailOneTimePasswordService> _mockEmailOneTimePasswordService;
        private readonly SignupConsoleService _service;
        public SignUpConsoleServiceTest()
        {
            _mockEmailOneTimePasswordService = new Mock<IEmailOneTimePasswordService>();
            _service = new SignupConsoleService(_mockEmailOneTimePasswordService.Object);
        }

        [Fact]
        public void GenerateEmailOneTimePassword_ValidEmail_SendsEmail_Positive()
        {
            // Arrange
            string validEmail = "test@example.com";
            (bool isValid, string? emailBody) expectedResult = (false, null);
            _mockEmailOneTimePasswordService.Setup(x => x.GenerateEmailOneTimePassword(validEmail)).Returns(expectedResult);

            var output = new StringWriter();
            Console.SetOut(output);

            var input = new StringReader(validEmail);
            Console.SetIn(input);

            // Act
            _service.GenerateEmailOneTimePassword();

            // Assert
            _mockEmailOneTimePasswordService.Verify(x => x.GenerateEmailOneTimePassword(validEmail), Times.Once);
            Assert.Contains(EmailOneTimePasswordConstants.EmailInvalid, output.ToString());
            output.Dispose();
            input.Dispose();
        }

        [Fact]
        public async Task CheckOneTimePassword_ValidOtp_ReturnsSuccess_Positive()
        {
            // Arrange
            string validOtp = "123456";

            var output = new StringWriter();
            Console.SetOut(output);

            _mockEmailOneTimePasswordService.Setup(x => x.CheckOneTimePassword(validOtp))
                .ReturnsAsync(OTPStatus.STATUS_OTP_OK);

            // Act
            Console.SetIn(new StringReader(validOtp + Environment.NewLine));
            await _service.CheckOneTimePassword();

            // Assert
            Assert.Contains(EmailOneTimePasswordConstants.OtpCodeValid, output.ToString());
        }

        [Fact]
        public async Task CheckOneTimePassword_InvalidOtp_ReturnInvalid_Positive()
        {
            // Arrange
            string validOtp = "78978978";

            var output = new StringWriter();
            Console.SetOut(output);

            _mockEmailOneTimePasswordService.Setup(x => x.CheckOneTimePassword(validOtp))
                .ReturnsAsync(OTPStatus.STATUS_OTP_OK);

            // Act
            // Simulate console input (you can use a mocking framework or directly set the input)
            Console.SetIn(new StringReader(validOtp + Environment.NewLine));
            await _service.CheckOneTimePassword();

            // Assert
            Assert.Contains(EmailOneTimePasswordConstants.InvalidOtp, output.ToString());
        }

        [Fact]
        public async Task CheckOneTimePassword_MaxTriesExceed_Positive()
        {
            // Arrange
            string validOtp = "7899789";

            var output = new StringWriter();
            Console.SetOut(output);

            _mockEmailOneTimePasswordService.Setup(x => x.CheckOneTimePassword(validOtp))
                .ReturnsAsync(OTPStatus.STATUS_OTP_OK);

            _service.tries = 11;

            // Act
            // Simulate console input (you can use a mocking framework or directly set the input)
            Console.SetIn(new StringReader(validOtp + Environment.NewLine));
            await _service.CheckOneTimePassword();

            // Assert
            Assert.Contains(EmailOneTimePasswordConstants.OtpIsWrongAfter10Tries, output.ToString());
        }
    }
}
