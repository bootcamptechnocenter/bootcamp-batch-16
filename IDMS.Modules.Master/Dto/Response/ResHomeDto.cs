namespace IDMS.Modules.Master.Dto.Response
{
    public class ResDashboardDto
    {
        public List<ResBrandStockDto> StockPerBrand { get; set; } = new();
        public int TotalReadyStock { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class ResBrandStockDto
    {
        public string BrandName { get; set; } = string.Empty;
        public int ReadyCount { get; set; }
    }

}