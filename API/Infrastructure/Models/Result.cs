using System.Collections.Generic;

namespace API.Infrastructure.Models
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }

        public T Data { get; set; }

        public List<string> Errors { get; set; } = new List<string>();

        public Result(T data) : this() => this.Data = data;

        public Result() => this.IsSuccess = true;
    }
}
