namespace IDMS.Web.Models;

public class DashboardViewModel
{
    public int TotalBrands { get; set; }
    public int TotalTypes { get; set; }
    public int ActiveTypes { get; set; }
    public int InactiveTypes { get; set; }
    public int TotalModels { get; set; }
    public int TotalStockItems { get; set; }
    public int TotalQuantity { get; set; }
    public long TotalInventoryValue { get; set; }
    public int LowStockCount { get; set; }

    // Bar chart: quantity per brand
    public List<string> BrandLabels { get; set; } = new();
    public List<int> BrandQuantities { get; set; } = new();

    // Tables
    public List<DashboardStockRow> TopStocks { get; set; } = new();
    public List<DashboardStockRow> LowStockItems { get; set; } = new();
}

public class DashboardStockRow
{
    public string BrandName { get; set; } = string.Empty;
    public string TypeName { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
    public int Price { get; set; }
    public int Quantity { get; set; }
    public long TotalValue { get; set; }
}
