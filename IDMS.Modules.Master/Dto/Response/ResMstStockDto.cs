namespace IDMS.Modules.Master.Dto.Response
{
    public class ResMstStockDto
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public string ModelName { get; set; } = string.Empty;
        public int JumlahStock { get; set; }
        public decimal Harga { get; set; }
    }
}
