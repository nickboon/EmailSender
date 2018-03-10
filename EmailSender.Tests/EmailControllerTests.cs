using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using EmailSender.Controllers;
using EmailSender.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;

namespace EmailSender.Tests
{
    public class EmailControlerTests
    {
        [Theory, AutoConfiguredMoqData]
        public void Post_ShouldThrow_NullReferenceExceptionIfNoEmail(EmailController sut)
        {
            Assert.Throws<NullReferenceException>(() => sut.Post(null));
        }

        [Theory, AutoConfiguredMoqData]
        public void Post_ShouldReturn_204NoContentIfEmailValid(EmailController sut)
        {
            var result = sut.Post(NewEmail("Testing..."));

            Assert.IsType<NoContentResult>(result);
        }

        [Theory, AutoConfiguredMoqData]
        public void Post_ShouldCall_SendIfEmailValid(
            [Frozen] Mock<IEmailService> emailService,
            EmailController sut)
        {
            sut.Post(NewEmail("Testing..."));

            emailService.Verify(x => x.Send(It.IsAny<EmailModel>()), Times.Once);
        }

        [Theory, AutoConfiguredMoqData]
        public void Post_ShouldReturn_400BadRequestIfEmailInvalid(EmailController sut)
        {
            var result = sut.Post(NewEmail(null));

            Assert.IsType<BadRequestResult>(result);
        }

        EmailModel NewEmail(string body) => new EmailModel { Body = body };

        class AutoConfiguredMoqDataAttribute : AutoDataAttribute
        {
            public AutoConfiguredMoqDataAttribute()
                : base(() => new Fixture().Customize(new AutoConfiguredMoqCustomization()))
            {
            }
        }
    }
}