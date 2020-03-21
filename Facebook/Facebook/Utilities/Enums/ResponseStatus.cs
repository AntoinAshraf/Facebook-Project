using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Utilities.Enums
{
    public enum ResponseStatus
    {
        Success = 200,
        ValidationError = 400,
        Unauthorized = 401,
        NoDataFound = 404,
        Error = 500
      
    }
}
