using TasteTrailData.Api.Common.Extensions.ServiceCollection;
using TasteTrailIdentity.Api.Common.Extensions.ServiceCollectionExtensions;
using TasteTrailIdentity.Api.Common.Extensions.WebApplicationExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InitAspnetIdentity(builder.Configuration);
builder.Services.InitAuth(builder.Configuration);
builder.Services.InitSwagger();
builder.Services.InitCors();
builder.Services.RegisterDependencyInjection();
builder.Services.RegisterBlobStorage(builder.Configuration);
builder.Services.AddMediatR();


builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
await app.SetupRoles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAllOrigins");

app.Run();