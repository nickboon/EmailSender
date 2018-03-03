using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;


namespace EmailSender.Integration
{
    public class EndToEndTests
    {
        readonly IWebHostBuilder _webHostBuilder = new WebHostBuilder().UseStartup<Startup>();
        readonly string _endpoint = "/Email";

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
            var content = new FormUrlEncodedContent(CreateEmailFormdata(string.Empty));

            using (var server = new TestServer(_webHostBuilder))
            using (var client = server.CreateClient())
            {
                var result = await client.PostAsync(_endpoint, content);
                Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            }
        }

        [Fact]
        public async Task ValidPostShouldResultIn204NoContent()
        {
            var content = new FormUrlEncodedContent(CreateEmailFormdata("Testing..."));

            using (var server = new TestServer(_webHostBuilder))
            using (var client = server.CreateClient())
            {
                var result = await client.PostAsync(_endpoint, content);
                Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
            }
        }

        Dictionary<string, string> CreateEmailFormdata(string body) => new Dictionary<string, string> { { "Body", body } };
    }
}
