namespace IDMS.Modules.Master.Services
{
    public interface ICurrentUserService
    {
        Task<string?> GetCurrentUserFullNameAsync();
    }
}
