namespace bookingEvent.DTO
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public static ApiResponse<T> SuccessResponse(
            T? data,
            string message,
            int statusCode = StatusCodes.Status200OK)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message,
                StatusCode = statusCode,
                Errors = null
            };
        }

        public static ApiResponse<T> ErrorResponse(
            string message,
            List<string>? errors = null,
            int statusCode = StatusCodes.Status400BadRequest)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Data = default,
                Message = message,
                StatusCode = statusCode,
                Errors = errors
            };
        }
    }
}
