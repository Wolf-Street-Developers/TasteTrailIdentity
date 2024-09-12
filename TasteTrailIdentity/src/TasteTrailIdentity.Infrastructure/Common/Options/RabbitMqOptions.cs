#pragma warning disable CS8618

namespace TasteTrailIdentity.Infrastructure.Common.Options;

public class RabbitMqOptions
{
    public string HostName { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }
}