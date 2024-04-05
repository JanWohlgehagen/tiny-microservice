using APIGateway.Controllers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace UnitTests
{
    public class SearchWireMockTest
    {


        [Fact]
        public async Task TestAuthRequest()
        {
            using (var authServer = WireMockServer.Start())
            {
                // Define stub mappings for SearchUser service


                // Define stub mapping for Identity service
                authServer
                    .Given(Request.Create().WithPath("/Identity/Authenticate").UsingGet())
                    .RespondWith(
                    Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody("true"));
                // Create HttpClient pointing to WireMock server
                var httpClient = new HttpClient { BaseAddress = new Uri(authServer.Urls[0]) };

                // Send the Auth request to WireMock
                var authResponse = await httpClient.GetAsync("/Identity/Authenticate");

                // Assert on the response
                Assert.Equal(HttpStatusCode.OK, authResponse.StatusCode);
                var authResponseContent = await authResponse.Content.ReadAsStringAsync();
                Assert.Equal("true", authResponseContent);
            }
        }

        [Fact]
        public async Task TestFindUserRequest()
        {
            using (var searchUserServer = WireMockServer.Start())
            {
                // Define stub mappings for SearchUser service
                searchUserServer
                    .Given(Request.Create().WithPath("/SearchUser/FindUser").WithParam("searchString", "test").UsingGet())
                    .RespondWith(
                        Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyAsJson(new UserSimpleDTO { id = "1", name = "Test User", imageURL = "http://example.com/image.jpg" }));
                // Create HttpClient pointing to WireMock server
                var httpClient = new HttpClient { BaseAddress = new Uri(searchUserServer.Urls[0]) };

                // Send the FindUser request to WireMock
                var searchword = "test"; // Assuming "test" is the search word
                var findUserResponse = await httpClient.GetAsync($"/SearchUser/FindUser?searchString={searchword}");

                // Assert on the response
                Assert.Equal(HttpStatusCode.OK, findUserResponse.StatusCode);
                var findUserResponseContent = await findUserResponse.Content.ReadAsStringAsync();
                var userDto = JsonConvert.DeserializeObject<UserSimpleDTO>(findUserResponseContent);
                Assert.NotNull(userDto);
                Assert.Equal("1", userDto.id);
                Assert.Equal("Test User", userDto.name);
                Assert.Equal("http://example.com/image.jpg", userDto.imageURL);
            }

        }
    }
}
