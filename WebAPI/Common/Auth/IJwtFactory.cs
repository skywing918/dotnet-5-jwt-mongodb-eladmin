using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Common.Models;

namespace WebAPI.Common.Auth
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);
        ClaimsIdentity GenerateClaimsIdentity(User user);
    }
}
