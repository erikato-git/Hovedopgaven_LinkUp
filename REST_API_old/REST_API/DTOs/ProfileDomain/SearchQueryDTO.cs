namespace REST_API.DTOs.ProfileDomain
{
    public class SearchQueryDTO
    {
        public String Profession { get; set; }                      // filter
        public String Title { get; set; }                           // filter
        public String? AlternativeTitle { get; set; }               // filter
        public int? Age { get; set; }                               // sort
        public int? YearsOfExperience { get; set; }                 // sort
        public int? GraduationYear { get; set; }                    // sort
        public String? Availability { get; set; }                   // sort
        public String? Institution { get; set; }                    // filter
    }
}
