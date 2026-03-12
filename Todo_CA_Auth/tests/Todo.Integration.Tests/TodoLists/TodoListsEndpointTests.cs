using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Todo.Infrastructure.Persistence;

namespace Todo.Integration.Tests.TodoLists;

public class TodoListsEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TodoListsEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null) services.Remove(descriptor);

                services.AddDbContext<ApplicationDbContext>(opts =>
                    opts.UseSqlite("DataSource=:memory:"));

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>()
                     .Database.EnsureCreated();
            }))
            .CreateClient();
    }

    [Fact]
    public async Task Get_NonExistentTodoList_Returns404()
        => (await _client.GetAsync("/api/todolists/99999")).StatusCode
            .Should().Be(HttpStatusCode.NotFound);

    [Fact]
    public async Task Get_AllTodoLists_Returns200()
        => (await _client.GetAsync("/api/todolists")).StatusCode
            .Should().Be(HttpStatusCode.OK);
}
