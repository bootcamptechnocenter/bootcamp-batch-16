namespace WebApi.Modules.Master.Dto.Response
{
    public class ResMstModelDto
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Year { get; set; }
        public bool IsActive { get; set; }
    }
}