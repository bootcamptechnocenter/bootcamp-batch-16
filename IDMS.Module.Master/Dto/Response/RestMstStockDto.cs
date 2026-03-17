using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Shared.Domain.Entities;

namespace IDMS.Module.Master.Dto.Response
{
    public class RestMstStockDto
    {
        public int Id { get; set; }
        public int TotalStock { get; set; }
        public decimal Price { get; set; }
        public int MstModelId { get; set; }
        public MstModels? Model { get; set; }
    }
}