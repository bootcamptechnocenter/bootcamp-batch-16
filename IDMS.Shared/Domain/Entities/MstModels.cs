using IDMS.Shared.Entities;

namespace IDMS.Shared.Domain.Entities
{
    public class MstModels : BaseEntity
    {
        public int TypeId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Year { get; set; }
        public bool IsActive { get; set; } = true;

        public MstTypes? Type { get; set; }
        public ICollection<MstStock> Stocks { get; set; } = new List<MstStock>();
    }
}