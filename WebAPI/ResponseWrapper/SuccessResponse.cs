using HelperClasses.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.ResponseWrapper
{
    public class SuccessResponse<T> where T : class
    {
        public SuccessResponse(string Message, T Result)
        {
            this.Message = Message;
            this.Result = Result;
        }
        public string Status
        {
            get
            {
                return Enum.GetName(typeof(ResponseMessages), ResponseMessages.Success);
            }
        }
        public string Message { get; set; }
        public T Result { get; set; }
    }

    public class ErrorResponse
    {
        public ErrorResponse(string Message, object Result)
        {
            this.Message = Message;
            this.Result = Result;
        }
        public string Status
        {
            get
            {
                return Enum.GetName(typeof(ResponseMessages), ResponseMessages.Error);
            }
        }
        public string Message { get; set; }
        public object Result { get; set; }
    }

    public class FailureResponse
    {
        public FailureResponse(string Message, object Result)
        {
            this.Message = Message;
            this.Result = Result;
        }
        public string Status
        {
            get
            {
                return Enum.GetName(typeof(ResponseMessages), ResponseMessages.Failure);
            }
        }
        public string Message { get; set; }
        public object Result { get; set; }
    }
}
