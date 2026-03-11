using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatWebApi.common
{
    public class ApiResponse
    {
        public string Message {get; set;} = string.Empty;
        public bool Status {get; set;} = true;
        public T? Data {get; set;}
        public Pagination? Pagination {get; set;}

        public static ApiResponse Success<T>(T data, string message = "Ok", Pagination? pagination = null)
        {
            return new ApiResponse
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
                Message = message
            };
        }
    }
}