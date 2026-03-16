using WebApi.Shared.Entities;

namespace WebView.Models
{
    public class ApiClientResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public Pagination? Pagination { get; set; }
    }
}