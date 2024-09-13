
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TasteTrailIdentity.Core.Users.Services;
using TasteTrailIdentity.Infrastructure.Common.BackgroundServices.Base;
using TasteTrailIdentity.Infrastructure.Common.Dtos;
using TasteTrailIdentity.Infrastructure.Common.Options;

namespace TasteTrailIdentity.Infrastructure.Common.BackgroundServices;

public class RoleRabbitMqService: BaseRabbitMqService, IHostedService
{
    public RoleRabbitMqService(IOptions<RabbitMqOptions> optionsSnapshot, IServiceScopeFactory serviceScopeFactory) :
        base(optionsSnapshot, serviceScopeFactory)
    {
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        base.StartListening("role_update_identity", async message => {
            using (var scope = base.serviceScopeFactory.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<IUserService>();

                var dto = JsonSerializer.Deserialize<UpdateUserRoleDto>(message)!;

                await userManager.UpdateUserRoleAsync(userId: dto.Id, roleId: dto.RoleId);
            }
        });



        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}


