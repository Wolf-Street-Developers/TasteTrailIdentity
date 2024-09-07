

using TasteTrailData.Core.Filters.Enums;
using TasteTrailData.Core.Filters.Specifications;
using TasteTrailData.Core.Users.Models;
using TasteTrailData.Infrastructure.Filters;

namespace TasteTrailIdentity.Infrastructure.Common.Admin.Factories;

public class UserManipulationsFilterFactory
{
    public static IFilterSpecification<User>? CreateFilter(FilterType? filterType)
    {
        if (filterType is null)
            return null;

        return filterType switch
        {
            FilterType.NotBanned => new NotBannedFilter<User>(),
            FilterType.Banned => new BannedFilter<User>(),
            FilterType.NotMuted => new NotMutedFilter<User>(),
            FilterType.Muted => new MutedFilter<User>(),
            _ => throw new ArgumentException("Invalid filter type", filterType.ToString())
        };
    }

}
