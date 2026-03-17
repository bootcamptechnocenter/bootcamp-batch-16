using WebApi.Shared.Entities;

namespace WebApi.Shared.Domain.Entities
{
    public class MstStocks : BaseEntity
    {
        public int ModelId { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
    }
}
