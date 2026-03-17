namespace IDMS.Modules.Master.Dto.Response
{
    public class ResMstStockDto
    {
        public int Id { get; set; }

        // Stock own fields
        public string Color { get; set; } = string.Empty;
        public string PoliceNumber { get; set; } = string.Empty;
        public string NewUsed { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsReady { get; set; }

        // Model (parent)
        public int ModelId { get; set; }
        public string ModelCode { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;

        // Type (grandparent)
        public int TypeId { get; set; }
        public string TypeCode { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;

        // Brand (great-grandparent)
        public int BrandId { get; set; }
        public string BrandCode { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
    }
}