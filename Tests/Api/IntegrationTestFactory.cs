using System.Data.Common;
using Infrastructure.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Testcontainers.PostgreSql;

namespace Tests.Api;

public sealed class IntegrationTestFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public UserContext UserContext { get; private set; } = null!;
    private Respawner _respawner = null!;
    private DbConnection _connection = null!;
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder().Build();


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<UserContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<UserContext>(options => { options.UseNpgsql(_container.GetConnectionString()); });
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_connection);
    }

    public async ValueTask InitializeAsync()
    {
        await _container.StartAsync();

        UserContext = Services.CreateScope().ServiceProvider.GetRequiredService<UserContext>();
        await UserContext.Database.EnsureCreatedAsync();
            
        _connection = UserContext.Database.GetDbConnection();
        await _connection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"],
        });
    }

    public override async ValueTask DisposeAsync()
    {
        await _connection.CloseAsync();
        await _container.DisposeAsync();
    }
}