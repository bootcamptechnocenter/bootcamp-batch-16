namespace WebApi.Modules.Master.Dto.Response
{
    public class ResAuthClientDto
    {
        public bool Success { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}