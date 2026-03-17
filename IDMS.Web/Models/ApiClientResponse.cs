using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using IDMS.Shared.Entities;

namespace IDMS.Web.Models
{
    public class ApiClientResponse<T>
    {
        public bool Status { get; set; }

        public string Message { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Pagination? pagination { get; set; }
    }
}