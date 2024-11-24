using REST_API.DTOs.ProfileDomain;
using REST_API.Models;
using REST_API.Services.IHelpers;

namespace REST_API.Services.Helpers
{
    public interface IProfileServiceHelper
    {
        Profile? GetProfileFromAccount(Account account, Guid profileId);
        Task<IEnumerable<Profile>?> SearchProfiles(SearchQueryDTO searchQuery);
    }
}
