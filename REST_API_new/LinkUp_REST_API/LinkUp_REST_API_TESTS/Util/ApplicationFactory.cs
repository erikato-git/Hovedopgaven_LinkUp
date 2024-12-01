using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using LinkUp_REST_API.Data.DbContextConnections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMotions.Fake.Authentication.JwtBearer;
using Xunit;

namespace LinkUp_REST_API_TESTS.Util
{
    public class ApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
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

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<DataContext>));

                // RemoveDbContext()
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                var host = _mssqlContainer.Hostname;
                var port = _mssqlContainer.GetMappedPublicPort(MsSqlPort);

                services.AddDbContext<DataContext>(options =>
                {
                    options.UseSqlServer(
                        $"Server={host},{port};Database={Database};User Id={Username};Password={Password};TrustServerCertificate=True");
                });


                /*
                 * Neil, Microservices, Lecture: 196
                 * Will be used when testing with 'httpClient'
                 */
                services.AddAuthentication(FakeJwtBearerDefaults.AuthenticationScheme)
                .AddFakeJwtBearer(opt =>
                {
                    opt.BearerValueType = FakeJwtBearerBearerValueType.Jwt;
                });


                // EnsureCreated()
                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<DataContext>();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                DbInitializerForTests.InitDbForTests(db);
            });

        }





        public async Task InitializeAsync()
        {
            await _mssqlContainer.StartAsync();
        }

        Task IAsyncLifetime.DisposeAsync() => _mssqlContainer.DisposeAsync().AsTask();

    }
}
