using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TasteTrailIdentity.Infrastructure.Common.Dtos;

public class UpdateMuteUserDto
{
    public required string Id { get; set; }
    public required bool IsMuted { get; set; }
}
