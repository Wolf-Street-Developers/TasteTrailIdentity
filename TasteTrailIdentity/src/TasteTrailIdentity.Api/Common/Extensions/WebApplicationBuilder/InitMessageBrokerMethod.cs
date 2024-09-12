
namespace TasteTrailIdentity.Api.Common.Extensions.WebApplicationBuilder;

using Microsoft.AspNetCore.Builder;
using TasteTrailIdentity.Infrastructure.Common.Options;

public static class InitMessageBrokerMethod
{
    public static void InitMessageBroker(this WebApplicationBuilder builder)
    {
        var rabbitMqSection = builder.Configuration.GetSection("RabbitMq");
        builder.Services.Configure<RabbitMqOptions>(rabbitMqSection);
    }
}
