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
        readonly IWebHostBuilder webHostBuilder = new WebHostBuilder().UseStartup<Startup>();

        [Fact]
        public async Task NullPostShouldResultIn400BadRequest()
        {
            using (var server = new TestServer(webHostBuilder))
            using (var client = server.CreateClient())
            {
                var result = await client.PostAsync("/Email", null);
                Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            }
        }

        [Fact]
        public async Task InvalidPostShouldResultIn400BadRequest()
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string> { { "Body", string.Empty } });

            using (var server = new TestServer(webHostBuilder))
            using (var client = server.CreateClient())
            {
                var result = await client.PostAsync("/Email", content);
                Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            }
        }

        [Fact]
        public async Task ValidPostShouldResultIn204NoContent()
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string> { { "Body", "Testing..." } });

            using (var server = new TestServer(webHostBuilder))
            using (var client = server.CreateClient())
            {
                var result = await client.PostAsync("/Email", content);
                Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
            }
        }
    }
}
