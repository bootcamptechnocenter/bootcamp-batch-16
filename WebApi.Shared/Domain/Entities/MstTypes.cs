using WebApi.Shared.Entities;

namespace WebApi.Shared.Domain.Entities
{
    public class MstTypes : BaseEntity
    {
        public int BrandId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}