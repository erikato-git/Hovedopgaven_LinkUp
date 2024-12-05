using LinkUp_REST_API.Models.Pending;
using LinkUp_REST_API.Models;
using System.ComponentModel.DataAnnotations;

namespace LinkUp_REST_API.DTOs.Requests.Completed
{
    public class ProfileUpdateInput
    {
        public Guid ProfileId { get; set; }
        public string? Profession { get; set; }
        public string? Title { get; set; }
        public string? AlternativeTitle { get; set; }
        public Media? ProfilePicture { get; set; }
        public string? ProfileDescription { get; set; }


        // Account
        public Guid AccountId { get; set; }


        // Keywork
        public string? Availability { get; set; }           // TODO: Enum
        public int? YearsOfExperience { get; set; }

        // Education (nested)
        public string? NameOfEducation { get; set; }
        public string? Institution { get; set; }
        public int? GraduationYear { get; set; }



        // Portfolio
        //public Guid? PortfolioId { get; set; }
        //public Portfolio? Portfolio { get; set; }


        // AudienceSpecification
        //public Guid? AudienceSpecificationId { get; set; }
        //public AudienceSpecification? AudienceSpecification { get; set; }


    }
}
