namespace WebApi.Modules.Master.Dto.Request
{
    public class ReqMstTypeUpdateDto
    {
        public int? BrandId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
    }
}