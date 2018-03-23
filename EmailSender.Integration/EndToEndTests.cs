using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace EmailSender.Integration
{
    public class EndToEndTests
    {
        readonly string _endpoint = "/Email";
        readonly IWebHostBuilder _webHostBuilder = new WebHostBuilder()
            .ConfigureAppConfiguration((hostingContext, config) =>config.AddJsonFile("appsettings.json"))
            .UseStartup<Startup>();
 
        [Fact]
        public async Task NullPostShouldResultIn400BadRequest()
        {
            using (var server = new TestServer(_webHostBuilder))
            using (var client = server.CreateClient())
            {
                var result = await client.PostAsync(_endpoint, null);
                Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            }
        }

        [Fact]
        public async Task InvalidPostShouldResultIn400BadRequest()
        {
            var content = new FormUrlEncodedContent(NewEmailFormdata(string.Empty));

            var result = await SendEmail(content);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Theory, AutoData]
        public async Task ValidPostShouldResultInAnEmailDelivery(string anonymousMessage)
        {
            var content = new FormUrlEncodedContent(NewEmailFormdata(anonymousMessage));

            var result = await SendEmail(content);

            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);

            var emailService = NewEmailService();
            Retry.WithExponentialBackOff(() => {
                var emails = emailService.SearchInboxByBody(anonymousMessage);

                Assert.NotEmpty(emails);
            }, 1000);

            emailService.MarkTestMessageForDeletionByBody(anonymousMessage);
        }

        async Task<HttpResponseMessage> SendEmail(FormUrlEncodedContent content)
        {
            using (var server = new TestServer(_webHostBuilder))
            using (var client = server.CreateClient())
                return await client.PostAsync(_endpoint, content);
        }

        IIntegrationEmailService NewEmailService() => new IntegrationEmailService(
            new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection("IntegrationEmailConfiguration")
                .Get<IntegrationEmailConfiguration>());

        Dictionary<string, string> NewEmailFormdata(string body) => new Dictionary<string, string> { { "Body", body }, { "From", "Annonymous@email.address" } };
    }
}
