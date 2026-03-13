using WebApi.Shared.Entities;

namespace WebApi.Shared.Domain.Entities
{
    public class MstUsers : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }
}