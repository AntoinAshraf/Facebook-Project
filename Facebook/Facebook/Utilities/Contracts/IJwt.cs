using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Contracts
{
    public interface IJwt
    {
        string GenerateToken(int userId);
        bool ValidateCurrentToken(string token);
        string GetId(string token);
    }
}
