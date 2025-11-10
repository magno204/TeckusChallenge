using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekusChallenge.Domain.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(string username);
    DateTime GetTokenExpiration();
}
