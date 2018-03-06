using System.Threading.Tasks;
using System.Security.Claims;
using SimplePMServices.Data;
using System.Collections.Generic;

namespace SimplePMServices.Auth
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id, IList<string> roles);
    }
}
