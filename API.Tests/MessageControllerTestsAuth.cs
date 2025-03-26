using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace API.Tests;

public class MessagesControllerTestsAuth : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public MessagesControllerTestsAuth(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetMessages_NoToken_ReturnsUnauthorized()
    {
        // Act
        var response = await _client.GetAsync("/Messages");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
