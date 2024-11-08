namespace REST_API.Util
{
    public class ResultDTO<T>
    {
        public bool isSuccess { get; set; }
        public T? Data { get; set; }
        public String Message { get; set; } = String.Empty;

        public static ResultDTO<T> SuccesResult(T data, String message)
        {
            return new ResultDTO<T> 
            { 
                isSuccess = true,
                Data = data, 
                Message = message 
            };
        }

        public static ResultDTO<T> FailureResult(String message)
        {
            return new ResultDTO<T>
            {
                isSuccess = false,
                Message = message,
                Data = default              // default: can either represent null or 0
            };
        }
    }
}
