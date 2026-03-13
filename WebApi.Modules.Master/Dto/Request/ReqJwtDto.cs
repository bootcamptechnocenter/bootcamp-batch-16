namespace WebApi.Modules.Master.Dto.Request
{
    public class ReqJwtDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;   
    }
}