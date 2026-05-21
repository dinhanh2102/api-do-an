using System;
namespace HRM.Common.Models
{
    public class ApiResultModel<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public int StatusCode { get; set; }       
    }

    public class ApiResultModel
    {
        public object Data { get; set; }
        public string MessageCode { get; set; }
        public bool IsStatus { get; set; } = false;
    }

    public class ApiResultConstaHRM
    {
        public const int StatusCodeSuccess = 200;
    }

    public class ErrorValidateModel
    {
        public string Message { get; set; }
    }
}