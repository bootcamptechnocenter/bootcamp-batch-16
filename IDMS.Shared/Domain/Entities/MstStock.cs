using IDMS.Shared.Entities;

namespace IDMS.Shared.Domain.Entities
{
    public class MstStock : BaseEntity
    {
        public int ModelId { get; set; }
        public int JumlahStock { get; set; }
        public decimal Harga { get; set; }

        public MstModels? Model { get; set; }
    }
}