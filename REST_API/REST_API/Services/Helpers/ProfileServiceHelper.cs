using REST_API.DTOs.ProfileDomain;
using REST_API.Models;
using REST_API.Repositories.Interfaces;

namespace REST_API.Services.Helpers
{
    public class ProfileServiceHelper : IProfileServiceHelper
    {
        public Profile? GetProfileFromAccount(Account account, Guid profileId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Profile>?> SearchProfiles(SearchQueryDTO searchQuery)
        {
            throw new NotImplementedException();
        }
    }
}
