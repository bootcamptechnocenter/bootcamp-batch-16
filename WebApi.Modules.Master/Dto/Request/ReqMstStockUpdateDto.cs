using System.ComponentModel.DataAnnotations;

namespace WebApi.Modules.Master.Dto.Request
{
    public class ReqMstStockUpdateDto
    {
        public int? ModelId { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative number")]
        public int? Stock { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a non-negative number")]
        public decimal? Price { get; set; }
    }
}
