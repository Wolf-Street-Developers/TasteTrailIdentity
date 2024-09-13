using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TasteTrailIdentity.Infrastructure.Common.Dtos;

public class UpdateUserRoleDto
{
    public required string Id { get; set; }
    public required string RoleId { get; set; }
}
