using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayPals.Responses
{
    public class PagedResponse<T>
    {
        public List<T> Data { get; set; }
        public Dictionary<string, string> Links { get; set; }
        public Dictionary<string, object> Meta { get; set; }

        public PagedResponse(List<T> data)
        {
            Data = data;
            Links = new Dictionary<string, string>();
            Meta = new Dictionary<string, object>();
        }
    }
}