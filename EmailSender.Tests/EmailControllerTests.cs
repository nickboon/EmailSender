using EmailSender.Controllers;
using EmailSender.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace EmailSender.Tests
{
    public class EmailControlerTests
    {
        [Fact]
        public void Post_ShouldThrow_NullReferenceExceptionIfNoEmail()
        {
            var controller = new EmailController();

            Assert.Throws<NullReferenceException>(() => controller.Post(null));
        }

        [Fact]
        public void Post_ShouldReturn_204NoContentIfEmailValid()
        {
            var controller = new EmailController();

            var result = controller.Post(NewEmail("Testing..."));

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Post_ShouldReturn_400BadRequestIfEmailInvalid()
        {
            var controller = new EmailController();

            var result = controller.Post(NewEmail(null));

            Assert.IsType<BadRequestResult>(result);
        }

        EmailModel NewEmail(string body) => new EmailModel { Body = body };
    }
}