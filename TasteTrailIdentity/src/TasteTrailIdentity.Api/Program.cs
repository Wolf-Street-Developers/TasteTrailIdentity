using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TasteTrailData.Api.Common.Extensions.ServiceCollection;
using TasteTrailData.Infrastructure.Common.Data;
using TasteTrailIdentity.Api.Common.Extensions.ServiceCollectionExtensions;
using TasteTrailIdentity.Api.Common.Extensions.WebApplicationExtensions;

var builder = WebApplication.CreateBuilder(args);

var postgresConnectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
var azureConnectionString = Environment.GetEnvironmentVariable("BLOB_STORAGE_CONNECTION_STRING");
var jwt_key = Environment.GetEnvironmentVariable("JWT_KEY");
var jwt_life_time = Environment.GetEnvironmentVariable("JWT_LIFE_TIME_IN_MINUTES");
var jwt_issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
var jwt_audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

if (!string.IsNullOrEmpty(jwt_key)) {
    System.Console.WriteLine("\n\n\n\n\n\n\n" + jwt_key + "\n\n\n\n\n\n\n");
    builder.Configuration["Jwt:Key"] = jwt_key;
}
if (!string.IsNullOrEmpty(jwt_life_time)) {
    System.Console.WriteLine("\n\n\n\n\n\n\n" + jwt_life_time + "\n\n\n\n\n\n\n");
    builder.Configuration["Jwt:LifeTimeInMinutes"] =  jwt_life_time;
}
if (!string.IsNullOrEmpty(jwt_issuer)) {
    System.Console.WriteLine("\n\n\n\n\n\n\n" + jwt_issuer + "\n\n\n\n\n\n\n");
    builder.Configuration["Jwt:Issuer"] = jwt_issuer;
}
if (!string.IsNullOrEmpty(jwt_audience)) {
    System.Console.WriteLine("\n\n\n\n\n\n\n" + jwt_audience + "\n\n\n\n\n\n\n");
    builder.Configuration["Jwt:Audience"] = jwt_audience;
}


if (!string.IsNullOrEmpty(postgresConnectionString)) {
    System.Console.WriteLine("\n\n\n\n\n\n\n" + postgresConnectionString + "\n\n\n\n\n\n\n");
    builder.Configuration["ConnectionStrings:SqlConnection"] = postgresConnectionString;
}

if (!string.IsNullOrEmpty(azureConnectionString)) {
    System.Console.WriteLine("\n\n\n\n\n\n\n" + azureConnectionString + "\n\n\n\n\n\n\n");
    builder.Configuration["ConnectionStrings:AzureBlobStorage"] = azureConnectionString;
}


builder.Services.AddDbContext<TasteTrailDbContext>(
    (optionsBuilder) => optionsBuilder.UseNpgsql(
        
        connectionString: builder.Configuration.GetConnectionString("SqlConnection"),
        b => b.MigrationsAssembly("TasteTrailIdentity.Api")
    )
);

builder.Services.InitAspnetIdentity(builder.Configuration);
builder.Services.InitAuth(builder.Configuration);
builder.Services.InitSwagger();
// builder.Services.InitCors();

builder.Services.AddCors(delegate (CorsOptions options)
{
    options.AddPolicy("AllowAllOrigins", delegate (CorsPolicyBuilder builder)
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.RegisterDependencyInjection();
builder.Services.RegisterBlobStorage(builder.Configuration);
builder.Services.AddMediatR();


builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<TasteTrailDbContext>();

    dbContext.Database.Migrate();
}

await app.SetupRoles();


app.UseSwagger();
app.UseSwaggerUI();


app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAllOrigins");

app.Run();
