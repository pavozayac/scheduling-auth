using System.Net;
using System.Net.Http.Json;
using System.Net.Mime;
using Application.Handlers;
using Microsoft.AspNetCore.Http;

namespace Tests.Api.User;

[CollectionDefinition(nameof(DatabaseTestCollection))]
public class DatabaseTestCollection : ICollectionFixture<IntegrationTestFactory>;

[Collection(nameof(DatabaseTestCollection))]
public class RegistrationTests(IntegrationTestFactory factory) : DatabaseTest(factory)
{
    [Fact]
    public async Task ShouldRegisterUser()
    {
        var testEmail = "testuser0@example.com";
        
        var response = await Client.PostAsJsonAsync("/api/user/", new
        {
            email = testEmail
        }, TestContext.Current.CancellationToken);
        
        var usersList = UserContext.Users.ToList();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Contains(usersList, x => x.Email == testEmail);
    }
    
    [Fact]
    public async Task ShouldRetrieveRegisteredUser()
    {
        var testEmail = "testuser1@example.com";
        var testUser = new global::Domain.Model.User(testEmail);
        await UserContext.Users.AddAsync(testUser, TestContext.Current.CancellationToken);
        await UserContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var response = await Client.GetAsync($"/api/user/{testEmail}", TestContext.Current.CancellationToken);
        var body = await response.Content.ReadFromJsonAsync<UserRetrievedResponse>(TestContext.Current.CancellationToken);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(body);
        Assert.Equal(testEmail, body.Email);
        Assert.Equal(4, body.RecoveryCodes.Count);
        Assert.Null(body.OtpCode);
    }
    
    [Fact]
    public async Task ShouldNotRetrieveUnregisteredUser()
    {
        var testEmail = "testuser0@example.com";

        var response = await Client.GetAsync($"/api/user/{testEmail}", TestContext.Current.CancellationToken);
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}