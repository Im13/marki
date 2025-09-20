using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace API.Tests.Controllers
{
    public class RecommendationsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public RecommendationsControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PopularEndpoint_Should_Return_200_And_List()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/recommendations/popular?days=30&take=5");

            // It will fail (404) until endpoint is implemented
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var items = await response.Content.ReadFromJsonAsync<object>();
            Assert.NotNull(items);
        }
    }
}


