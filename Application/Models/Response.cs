using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Response<T>
    {
        public bool IsSuccess { get; set; }
        public T Result { get; set; }
        public object Errors { get; set; }
        public Response(bool isSuccess, object errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public Response(T result)
        {
            Result = result;
        }

        public Response()
        {
        
        }

    }
}
