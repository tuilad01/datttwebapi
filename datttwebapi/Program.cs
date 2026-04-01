using datttwebapi.Data;
using Asp.Versioning;
using NLog;
using NLog.Web;

// Early init of NLog to allow startup and exception logging, before host is built
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0); // Set the default version
        options.AssumeDefaultVersionWhenUnspecified = true; // Assume default if client doesn't specify a version
        /*
         * HTTP/1.1 200 OK
        allow: GET, POST, OPTIONS
        api-supported-versions: 1.0, 2.0, 3.0
         */
        options.ReportApiVersions = true; // Report the supported API versions in the response headers
        options.ApiVersionReader = ApiVersionReader.Combine(
         new HeaderApiVersionReader("api-version"),
         new QueryStringApiVersionReader("api-version")
        );
    }).AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true; // This option used with URL-segment API versioning like [Route("api/v{version:apiVersion}/[Controller]")]
        options.DefaultApiVersion = new ApiVersion(1);
        options.AssumeDefaultVersionWhenUnspecified = true;
    });

    //builder.Services.AddVersionedApiExplorer(options =>
    //{
    //    // Group the API version by a specific name, necessary for Swagger
    //    options.GroupNameFormat = "'v'VVV";
    //    // Substitute the version into the URL route (e.g., /api/v1/controller)
    //    options.SubstituteApiVersionInUrl = true;
    //});

    builder.Services.AddControllers();
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();
    builder.Services.AddAutoMapper(cfg => { }, typeof(Program));

    builder.Services.AddNpgsql<ApiVersioningDbContext>(builder.Configuration.GetConnectionString("DefaultConnection"));


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();

    app.MapControllers();

    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApiVersioningDbContext>();

        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    Email = "admin@example.com",
                    Phone = "123456789"
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin2",
                    Email = "admin2@example.com",
                    Phone = "123456789"
                }
            );

            context.SaveChanges();
        }
    }

    app.Run();
} catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
