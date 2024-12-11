using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LinkUp_REST_API.Data.DbContextConnections;
using LinkUp_REST_API.DTOs.Requests.Completed;
using LinkUp_REST_API.DTOs.Responses.Completed;
using LinkUp_REST_API.Models;
using LinkUp_REST_API.Services.Interfaces.Completed;
using LinkUp_REST_API.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Account = CloudinaryDotNet.Account;

namespace LinkUp_REST_API.Services.Completed
{
    public class ProfileServiceHelper : IProfileServiceHelper
    {
        private readonly DataContext _dbContext;
        private readonly Cloudinary _cloudinary;

        public ProfileServiceHelper(DataContext dataContext, IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;

            _dbContext = dataContext;
        }

        public async Task<bool> DeleteMedia(string mediaId)
        {
            if (string.IsNullOrEmpty(mediaId.ToString()))
            {
                return false;
            }

            // Delete file from Cloudinary
            var deleteResult = await DeletePhotoFromCloudinary(mediaId.ToString());

            if (deleteResult == null)
            {
                return false;
            }

            // Delete media object from database
            var mediafound = await _dbContext.Medias.FirstOrDefaultAsync(x => x.MediaId == mediaId);

            if (mediafound == null)
            {
                return false;
            }

            _dbContext.Medias.Remove(mediafound);

            var saved = await _dbContext.SaveChangesAsync() > 0;

            if (!saved)
            {
                return false;
            }

            return true;
        }

        public async Task<Media?> SaveMedia(IFormFile file, Profile profile)
        {
            if (file == null || profile == null)
            {
                throw new ArgumentNullException("File or profile is invalid");
            }

            // Add file to Cloudinary
            var uploadResult = await AddPhotoToCloudinary(file, profile);

            if (uploadResult == null)
            {
                return null;
            }

            // Add media object to database
            profile.ProfilePicture = uploadResult;

            _dbContext.Profiles.Update(profile);
            var saved = await _dbContext.SaveChangesAsync() > 0;

            if (!saved)
            {
                return null;
            }

            return uploadResult;
        }


        private async Task<Media?> AddPhotoToCloudinary(IFormFile file, Profile profile)
        {
            if (file == null || profile == null)
            {
                throw new ArgumentNullException("File or profile is invalid");
            }

            if (file.Length > 0)
            {
                await using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.Name, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    throw new Exception(uploadResult.Error.Message);
                }

                var media = new Media
                {
                    MediaId = uploadResult.PublicId,
                    URL = uploadResult.Url.ToString(),
                    ProfileId = profile.ProfileId,
                    Profile = profile
                };
                
                return media;
            }

            return null;
        }

        public async Task<string?> DeletePhotoFromCloudinary(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result.Result == "ok" ? result.Result : null;
        }

        public async Task<IEnumerable<ProfileSearchQueryOutput>?> QuerySearchedProfiles(ProfileSearchQueryInput searchQuery)
        {
            if (searchQuery == null)
            {
                return null;
            }

            var profiles = _dbContext.Profiles
                .Include(p => p.Account)
                .ThenInclude(a => a.PersonInformation)
                .Include(p => p.Keyword)
                .ThenInclude(k => k.Education)
                .AsQueryable();

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
                profiles = profiles.Where(x =>
                    x.Keyword != null &&
                    x.Keyword.Education != null &&
                    EF.Functions.Like(x.Keyword.Education.Institution, $"%{searchQuery.Institution}%")
                );
            }

            // Sorting
            if (searchQuery.GraduationYear.HasValue)
            {
                profiles = profiles.OrderByDescending(x => x.Keyword != null && x.Keyword.Education != null
                    ? x.Keyword.Education.GraduationYear : 0);  // element will appear at the end of the list if Keyword and / or Education is null
            }

            if (searchQuery.YearsOfExperience.HasValue)
            {
                profiles = profiles.OrderByDescending(x => x.Keyword != null ? x.Keyword.YearsOfExperience : null);
            }

            if (searchQuery.Age.HasValue)
            {
                profiles = profiles.OrderBy(x => x.Account != null
                    ? x.Account.PersonInformation.BirthDate : DateOnly.MaxValue); // Use a high valid date
            }

            // TODO: Sorting Availability

            // Projection and Materialization
            var result = await profiles
                .Select(profile => new ProfileSearchQueryOutput
                {
                    ProfileId = profile.ProfileId, // Include the ProfileId

                    Profession = profile.Profession,

                    Title = profile.Title,

                    AlternativeTitle = profile.AlternativeTitle,

                    ProfilePicture = profile.ProfilePicture,

                    Availability = profile.Keyword != null && profile.Keyword.Availability != ""
                        ? profile.Keyword.Availability : null,

                    YearsOfExperience = profile.Keyword != null && profile.Keyword.YearsOfExperience.ToString() != ""
                        ? profile.Keyword.YearsOfExperience : 0,

                    Institution = profile.Keyword != null && profile.Keyword.Education != null && profile.Keyword.Education.Institution != ""
                        ? profile.Keyword.Education.Institution : "",

                    GraduationYear = profile.Keyword != null && profile.Keyword.Education != null && profile.Keyword.Education.GraduationYear.ToString() != ""
                        ? profile.Keyword.Education.GraduationYear : 0,

                    Age = profile.Account != null && profile.Account.PersonInformation != null
                          ? CalculateAge(profile.Account.PersonInformation.BirthDate)
                          : 0 // Calculating age
                })
                .ToListAsync();  // Materialize the query here

            return result;
        }

        public static int? CalculateAge(DateOnly? dateOfBirth)
        {
            if (dateOfBirth == null) return null;

            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - dateOfBirth.Value.Year;

            return age;
        }

    }

}
