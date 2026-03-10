using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Shared.Entities;

namespace IDMS.Common
{
    public class ApiResponse<T>
    {
        public bool Status { get; set; } = true;
        public string Message { get; set; } = "";
        public T? Data { get; set; }
        public Pagination? Pagination { get; set; }
        public static ApiResponse<T> Success(T data, string message = "Ok", Pagination? pagination = null)
        {
            return new ApiResponse<T>
            {
                Status = true,
                Message = message,
                Data = data,
                Pagination = pagination
            };
        }
        public static ApiResponse<T> Fail(string message = "Error")
        {
            return new ApiResponse<T>
            {
                Status = false,
                Message = message,
                Data = default,
                Pagination = null
            };
        }
    }
}