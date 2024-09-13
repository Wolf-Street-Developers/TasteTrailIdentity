using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TasteTrailIdentity.Infrastructure.Common.Dtos;

public class UpdateBunUserDto
{
    public required string Id { get; set; }
    public required bool IsBanned { get; set; }
}
