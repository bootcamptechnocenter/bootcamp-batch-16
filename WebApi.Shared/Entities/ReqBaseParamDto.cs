using Microsoft.AspNetCore.Mvc;

namespace WebApi.Shared.Entities
{
    public class ReqBaseParamDto
    {
        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;
        [FromQuery(Name = "limit")]
        public int Limit { get; set; } = 10;
        [FromQuery(Name = "search")]
        public string? Search { get; set; }
    }
}