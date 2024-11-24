using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using REST_API.Data;

namespace REST_API_TESTS.Util
{
    /*
     * Reference: Cummings, Neil: "Build a Microservice app with .NET and NextJS from scratch", lecture: 192, Udemy
     * Reference: CodeMaze, https://code-maze.com/csharp-testing-using-testcontainers-for-net-and-docker/
     */

    public class RepositoryTestsWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private const string Database = "master";
        private const string Username = "sa";
        private const string Password = "yourStrong(!)Password";
        private const ushort MsSqlPort = 1433;

        private readonly IContainer _mssqlContainer = new ContainerBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")        // docker-image
            .WithPortBinding(MsSqlPort)
            .WithEnvironment("ACCEPT_EULA", "Y")
            .WithEnvironment("SQLCMDUSER", Username)
            .WithEnvironment("SQLCMDPASSWORD", Password)
            .WithEnvironment("MSSQL_SA_PASSWORD", Password)
            .Build();

        /*
         * ConfigureWebHost: Enable us to give access to ConfigureTestService, where we can configure the DI for the testing-instance of Program.cs
         * Description: 
         * 1. Finds and remove existing DbContext<MssqlContext> if any
         * 2. Add DbContext of test-container by the container's connectionstring
         * 3. Align the database schema in test-container with the DbContext
         * OBS: Docker must be running before executing tests with the test-container
         * TODO: Consider to refactor it like Neil in lecture: 193
         * TODO: Consider to use a CollectionDefinition like in lecture: 200, so I can avoid xUnit to start up a new test-database-server for each test-class
         */
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<MssqlDbContext>));

                // RemoveDbContext()
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                var host = _mssqlContainer.Hostname;
                var port = _mssqlContainer.GetMappedPublicPort(MsSqlPort);
                services.AddDbContext<MssqlDbContext>(options =>
                {
                    options.UseSqlServer(
                        $"Server={host},{port};Database={Database};User Id={Username};Password={Password};TrustServerCertificate=True");
                });

                // EnsureCreated()
                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<MssqlDbContext>();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                DbInitializerForTests.InitDbForTests(db);
            });

        }

        // Initialize test-container when tests start.
        public async Task InitializeAsync()
        {
            await _mssqlContainer.StartAsync();
        }

        // Dispose test-container when tests end.
        Task IAsyncLifetime.DisposeAsync() => _mssqlContainer.DisposeAsync().AsTask();

    }
}
