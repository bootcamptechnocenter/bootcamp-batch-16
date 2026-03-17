using IDMS.Shared.Entities;

namespace IDMS.Shared.Domain.Entities
{
    public class MstTypes : BaseEntity
    {
        public int BrandId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public MstBrands? Brand { get; set; }
        public ICollection<MstModels> Models { get; set; } = new List<MstModels>();
    }
}
