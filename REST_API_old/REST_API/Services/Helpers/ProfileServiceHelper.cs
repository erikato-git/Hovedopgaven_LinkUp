using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using REST_API.DTOs.ProfileDomain;
using REST_API.Models;
using REST_API.Services.IHelpers;

namespace REST_API.Services.Helpers
{
    public class ProfileServiceHelper : IProfileServiceHelper
    {
        private readonly MssqlDbContext _dbContext;
        private IPhotoAccessor _photoAccessor;

        public ProfileServiceHelper(MssqlDbContext dbContext, IPhotoAccessor photoAccessor)
        {
            _dbContext = dbContext;
            _photoAccessor = photoAccessor;
        }

        public async Task<Profile?> GetProfileFromAccount(Account? account, Guid? profileId)
        {
            if (profileId == Guid.Empty || account == null)
            {
                return null;
            }

            var profile = await _dbContext.Profiles.FirstOrDefaultAsync(x => x.ProfileId == profileId);

            if (profile != null)
            {
                var containsProfile = account.AccountId.Equals(profile.AccountId);

                if (containsProfile)
                {
                    return profile;
                }
            }

            return null;
        }

        public async void SaveImageToCloudinary(IFormFile file)
        {
            var photoUploadFile = await _photoAccessor.AddPhoto(file);

            var photo = new Media
            {
                Url = photoUploadFile.Url,
                Id = photoUploadFile.PublicId,
            };

            // Consider to save to own database, lecture: 183, Neil: .NET and React
        }

        public async void DeleteImageFromCloudinary(Guid id)
        {
            var photoDeleted = await _photoAccessor.DeletePhoto(id.ToString());

            // If added to local database remove it, lecture: 185, Neil: .NET and React
        }




        /*
         * Reference: https://github1s.com/teddysmithdev/FinShark/blob/master/api/Repository/StockRepository.cs#L49
         * TODO: Descripe how I have implemented it, the filtering and sorting order, Linq: filtering - sorting - projection - materialize
         */
        public async Task<IEnumerable<ProfileSearchResponseDTO>?> SearchProfiles(SearchQueryDTO searchQuery)
        {
            if (searchQuery == null)
            {
                return null;
            }

            // Start with all profiles
            // TODO: make a projection of desired data I want to retrieve to client
            var profiles = _dbContext.Profiles
                .Include(p => p.Account)
                .Include(p => p.Keyword.Education)
                .AsQueryable();

            /*
                // Old solution
                // ChatGPT: https://chatgpt.com/c/67434af1-5130-8006-99e6-dd56907e2fcd
                //if (!string.IsNullOrWhiteSpace(searchQuery.Profession))
                //{
                //    profiles = profiles.Where(x => x.Profession.Contains(searchQuery.Profession, StringComparison.OrdinalIgnoreCase));          // use Contains() will compare the values partially, 'x => x.A == query.A' has to be exact
                //}    
             */

            // Filtering
            /*
             * EF Core's Like-function approach instead of Contains(), otherwise Error-occur
             */
            if (!string.IsNullOrWhiteSpace(searchQuery.Profession))
            {
                profiles = profiles.Where(x => EF.Functions.Like(x.Profession, $"%{searchQuery.Profession}%"));
            }

            if (!string.IsNullOrWhiteSpace(searchQuery.Title))
            {
                profiles = profiles.Where(x => EF.Functions.Like(x.Title, $"%{searchQuery.Title}%"));

            }

            if (!string.IsNullOrWhiteSpace(searchQuery.AlternativeTitle))
            {
                profiles = profiles.Where(x => EF.Functions.Like(x.AlternativeTitle, $"%{searchQuery.AlternativeTitle}%"));

            }

            if (!string.IsNullOrWhiteSpace(searchQuery.Institution))
            {
                profiles = profiles.Where(x => EF.Functions.Like(x.Keyword.Education.Institution, $"%{searchQuery.Institution}%"));

            }

            await profiles.ToListAsync();
            profiles.AsQueryable();


            // Sorting
            if (searchQuery.GraduationYear.HasValue)
            {
                profiles = profiles.OrderByDescending(x => x.Keyword.Education.GraduationYear);
            }

            await profiles.ToListAsync();
            profiles.AsQueryable();


            if (searchQuery.YearsOfExperience.HasValue)
            {
                profiles = profiles.OrderByDescending(x => x.Keyword.YearsOfExperience);
            }

            await profiles.ToListAsync();
            profiles.AsQueryable();


            if (searchQuery.Age.HasValue)
            {
                profiles = profiles.OrderBy(x => x.Account.PersonInformation.BirthDate); // Assumes sorting by youngest first
            }

            // TODO: Sorting Availability
            //if (!string.IsNullOrWhiteSpace(searchQuery.Availability))
            //{
            //    var availabilityOrder = new List<string> { "Available", "Part-Time", "Limited", "Unavailable" };
            //    profiles = profiles.OrderBy(x => availabilityOrder.IndexOf(x.Keyword.Availability));
            //}

            // Projection
            var result = await profiles
            .Select(x => new ProfileSearchResponseDTO
            {
                Profession = x.Profession,
                Title = x.Title,
                AlternativeTitle = x.AlternativeTitle,
                Age = x.Account.PersonInformation != null ? DateTime.Now.Year - x.Account.PersonInformation.BirthDate.Year : (int?)null, // Calculate Age based on BirthDate
                YearsOfExperience = x.Keyword.YearsOfExperience,
                //GraduationYear = x.Keyword.Education != null ? x.Keyword.Education.GraduationYear : (int?)null,
                Availability = x.Keyword.Availability,
                Institution = x.Keyword.Education != null ? x.Keyword.Education.Institution : null,
                ProfilePicture = x.ProfilePicture
            })
            .ToListAsync();


            return result.ToList();

        }




    }
}
