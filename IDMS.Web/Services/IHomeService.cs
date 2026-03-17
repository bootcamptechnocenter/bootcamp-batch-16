using IDMS.Modules.Master.Dto.Response;

namespace IDMS.Web.Services
{
    public interface IHomeService
    {
        Task<ResDashboardDto> GetDashboardData();
    }
}