namespace REST_API.DTOs.ProfileDomain
{
    public class ProfileSearchResponseDTO
    {
        public String Profession { get; set; }                      
        public String Title { get; set; }                           
        public String? AlternativeTitle { get; set; }               
        public int? Age { get; set; }                               
        public int? YearsOfExperience { get; set; }                 
        public int? GraduationYear { get; set; }                    
        public String? Availability { get; set; }                   
        public String? Institution { get; set; }                    
        public String? ProfilePicture {  get; set; }
    }
}
