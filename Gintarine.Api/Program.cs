using Gintarine.Documentation;
using Gintarine.ExternalClients.Post;
using Gintarine.ExternalClients.Post.Client;
using Gintarine.Repositories;
using Gintarine.Repositories.Repositories;
using Gintarine.Services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IClientsService, ClientsService>();
builder.Services.AddOptions<PostSettings>()
    .BindConfiguration(nameof(PostSettings))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddHttpClient<IPostApiClient, PostApiClient>((serviceProvider, httpClient) =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<PostSettings>>();
    httpClient.BaseAddress = new Uri(settings.Value.Url);
});
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddDbContext<GintarineContext>(db =>
{
    db.UseSqlServer(
        builder.Configuration.GetConnectionString("GintarineConnectionString"));
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.ExampleFilters();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Gintarine Clients API", 
        Version = "v1"
    });
});
builder.Services.AddSwaggerExamplesFromAssemblyOf<ClientImportDtoExample>();

var app = builder.Build();
InitializeDatabase(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

void InitializeDatabase(IHost applicationHost)
{
    using var scope = applicationHost.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<GintarineContext>();
    context.Database.EnsureCreated();
}