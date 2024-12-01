namespace REST_API.Util
{
    public class ResultDTO
    {
        public bool isSuccess { get; set; }
        public object? Data { get; set; }   // needs to specify datatype at runtime instead of generics
        public String Message { get; set; } = String.Empty;

        public static ResultDTO SuccesResult(object data, String message)
        {
            return new ResultDTO
            { 
                isSuccess = true,
                Data = data, 
                Message = message 
            };
        }

        public static ResultDTO FailureResult(String message)
        {
            return new ResultDTO
            {
                isSuccess = false,
                Message = message,
                Data = null
            };
        }
    }
}
