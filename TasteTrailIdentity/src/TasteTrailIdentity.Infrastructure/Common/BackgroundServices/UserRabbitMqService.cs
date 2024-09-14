using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TasteTrailData.Core.Users.Models;
using TasteTrailIdentity.Core.Users.Services;
using TasteTrailIdentity.Infrastructure.Common.BackgroundServices.Base;
using TasteTrailIdentity.Infrastructure.Common.Dtos;
using TasteTrailIdentity.Infrastructure.Common.Options;

namespace TasteTrailIdentity.Infrastructure.Common.BackgroundServices;

public class UserRabbitMqService: BaseRabbitMqService, IHostedService
{
    public UserRabbitMqService(IOptions<RabbitMqOptions> optionsSnapshot, IServiceScopeFactory serviceScopeFactory) :
        base(optionsSnapshot, serviceScopeFactory)
    {
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        base.StartListening("user_toggleban_identity", async message => {
            using (var scope = base.serviceScopeFactory.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

                var dtoBan = JsonSerializer.Deserialize<UpdateBunUserDto>(message)!;

                await userService.UpdateBanAsync(userId: dtoBan.Id, dtoBan.IsBanned);
            }
        });

        base.StartListening("user_togglemute_identity", async message => {

            using (var scope = base.serviceScopeFactory.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

                var dtoMute = JsonSerializer.Deserialize<UpdateMuteUserDto>(message)!;

                await userService.UpdateMuteAsync(userId: dtoMute.Id, dtoMute.IsMuted);
            }
        });

        base.StartListening("role_update_identity", async message => {

            using (var scope = base.serviceScopeFactory.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

                var dtoMute = JsonSerializer.Deserialize<UpdateUserRoleDto>(message)!;

                await userService.UpdateUserRoleAsync(userId: dtoMute.Id, dtoMute.RoleId);
            }
        });


        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

