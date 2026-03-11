namespace WebApi.Modules.Master.Dto.Request
{
    public class ReqMstModelUpdateDto
    {
        public int? TypeId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? Year { get; set; }
        public bool? IsActive { get; set; }
    }
}