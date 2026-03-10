using WebApi.Shared.Entities;

namespace WebApi.Common
{
    public class ApiResponse<T>
    {
        public bool Status { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public Pagination? Pagination { get; set; }
        public static ApiResponse<T> Success(T data, string message = "Success", Pagination? pagination = null)
        {
            return new ApiResponse<T>
            {
                Status = true,
                Message = message,
                Data = data,
                Pagination = pagination
            };
        }
        public static ApiResponse<T> Failure(string message = "Failure")
        {
            return new ApiResponse<T>
            {
                Status = false,
                Message = message,
                Data = default,
                Pagination = null
            };
        }
    }
}