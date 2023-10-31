using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Helpers
{
    public class Responses
    {
        public object Response { get; set; }
        public string Token { get; set; }
        public bool Error { get; set; }
        public string Message { get; set; }
    }

}
