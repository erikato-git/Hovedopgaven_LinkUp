namespace LinkUp_REST_API.Util
{
    public class ResultDTO
    {
        public bool isSucces { get; set; }
        public object? Data { get; set; }   // specify data during runtime
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }

        public static ResultDTO Succes(object data, int statuscode, string message)
        {
            return new ResultDTO
            {
                isSucces = true,
                Data = data,
                StatusCode = statuscode,
                Message = message
            };
        }

        public static ResultDTO Failure(int statuscode, string message)
        {
            return new ResultDTO
            {
                isSucces = false,
                StatusCode = statuscode,
                Message = message
            };
        }

    }
}
