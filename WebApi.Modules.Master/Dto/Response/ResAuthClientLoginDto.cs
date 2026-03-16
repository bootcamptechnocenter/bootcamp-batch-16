namespace WebApi.Modules.Master.Dto.Response
{
    public class ResAuthClientLoginDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}