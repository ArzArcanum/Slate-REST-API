using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;
using Jose;
using System;
using System.Collections.Generic;

namespace API.Tests;

public class MessagesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public MessagesControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    //[Fact]
    //public async Task GetMessages_ReturnsOk()
    //{
    //    // Act
    //    var response = await _client.GetAsync("/Messages");

    //    // Assert
    //    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    //}
}
