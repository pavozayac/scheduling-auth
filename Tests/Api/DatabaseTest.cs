using Infrastructure.Contexts;

namespace Tests.Api;

public class DatabaseTest : IAsyncLifetime
{
    protected HttpClient Client;
    protected readonly UserContext UserContext;
    protected readonly Func<Task> _resetDatabase;

    public DatabaseTest(IntegrationTestFactory factory)
    {
        Client = factory.CreateClient();
        UserContext = factory.UserContext;
        _resetDatabase = factory.ResetDatabaseAsync;
    }

    public ValueTask InitializeAsync() => ValueTask.CompletedTask;

    public async ValueTask DisposeAsync() => await _resetDatabase();
}