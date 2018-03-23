using AutoFixture;
using AutoFixture.Xunit2;
using EmailSender.Models;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace EmailSender.Tests
{
    public class EmailServiceTests
    {
        [Theory, ValidEmailConfiguratonData]
        public void Send_ShouldNotThrowException_IfCredentialsValid(EmailService sut, EmailModel email)
        {
            sut.Send(email);
        }

        class ValidEmailConfiguratonDataAttribute : AutoDataAttribute
        {
            public ValidEmailConfiguratonDataAttribute()
            : base(() => new Fixture().Customize(new ValidEmailConfigurationCutomization()))
            {
            }
        }

        class ValidEmailConfigurationCutomization : ICustomization
        {
            public void Customize(IFixture fixture) => fixture.Inject<IEmailConfiguration>(new ConfigurationBuilder()
                   .AddJsonFile("appsettings.json")
                   .Build()
                   .GetSection("EmailConfiguration")
                   .Get<EmailConfiguration>());
        }
    }
}
