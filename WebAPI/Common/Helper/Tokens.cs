namespace WebAPI.Common.Helper
{
    using Newtonsoft.Json;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using WebAPI.Common.Auth;
    using WebAPI.Common.Models;

    public class Tokens
    {
        public static async Task<string> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory, string userName, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings)
        {
            var response = new
            {
                id = identity.Claims.Single(c => c.Type == "id").Value,
                //fullname = identity.Claims.Single(c => c.Type == "fullname").Value,
                unique_name = userName,
                //picture = identity.Claims.Single(c => c.Type == "picture").Value,
                //roles = identity.Claims.Where(c => c.Type == ClaimTypes.Role).Select(r => r.Value).ToList(),
                //books = identity.Claims.Where(c => c.Type == "books").Select(r => r.Value).ToList(),
                auth_token = await jwtFactory.GenerateEncodedToken(userName, identity),
                expires_in = (int)jwtOptions.ValidFor.TotalSeconds
            };

            return JsonConvert.SerializeObject(response, serializerSettings);
        }
    }
}
