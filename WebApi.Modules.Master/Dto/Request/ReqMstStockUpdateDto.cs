namespace WebApi.Modules.Master.Dto.Request
{
    public class ReqMstStockUpdateDto
    {
        public int? ModelId { get; set; }
        public int? Stock { get; set; }
        public decimal? Price { get; set; }
    }
}
