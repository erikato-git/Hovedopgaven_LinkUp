namespace LinkUp_REST_API.DTOs.Requests.Completed
{
    public class ProfileMediaUpload
    {
        public Guid ProfileId { get; set; }
        public required IFormFile UploadFile { get; set; }
    }
}
