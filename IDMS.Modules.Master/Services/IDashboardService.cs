using IDMS.Modules.Master.Dto.Response;

namespace IDMS.Modules.Master.Services
{
    public interface IDashboardService
    {
        Task<ResDashboardDto> GetDashboardData();
    }
}