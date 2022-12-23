using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Helper
{
    public partial interface IApiHelper 
    {
        Task<(T?, int)> BaseApiCall<T>(string url, object obj, HttpMethod method);
    }
}
