using EmailSender.Controllers;
using EmailSender.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace DotNetCoreSampleApi.Tests
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

            var result = controller.Post(new EmailModel("Testing..."));

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Post_ShouldReturn_400BadRequestIfEmailInvalid()
        {
            var controller = new EmailController();

            var result = controller.Post(new EmailModel(null));

            Assert.IsType<BadRequestResult>(result);
        }
    }
}