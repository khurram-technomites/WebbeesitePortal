using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class ResponseMessageDTO
    {
        public ResponseMessageDTO()
        {
            this.Status = "";
            this.Message = "";
            this.Result = null;
        }

        public ResponseMessageDTO(string status, string message, object obj)
		{
            this.Status = status;
            this.Message = message;
            this.Result = obj;
        }

        public string Status { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
    }
}
