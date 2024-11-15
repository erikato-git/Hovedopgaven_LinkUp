using REST_API.DTOs;
using REST_API.Models;
using REST_API.Util;

namespace REST_API.Repositories
{
    public interface IPersonInformationRepository
    {
        Task GetByIdAsync(Guid id);
        Task GetAllAsync();
        Task AddAsync(PersonInformation personinformation);
        Task UpdateAsync(PersonInformation personinformation);
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync();

    }
}
