namespace WebAPI.Common.Helper
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using WebAPI.Common.Auth;
    using WebAPI.Common.Models;

    public class Tokens
    {
        public static async Task<string> GenerateJwt(ClaimsIdentity identity, List<Role> roles, IJwtFactory jwtFactory, string userName, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings)
        {
            var userInfo = new
            { 
                id = identity.Claims.Single(c => c.Type == "id").Value,
                unique_name = userName,
                //picture = identity.Claims.Single(c => c.Type == "picture").Value,
                roles = roles,
                //books = identity.Claims.Where(c => c.Type == "books").Select(r => r.Value).ToList(),
            };
            var jwtUserDto = new
            {
                roles = roles.Select(r => r.Name).ToList(),
                user = userInfo
            };
            var curToken = await jwtFactory.GenerateEncodedToken(userName, identity);
            var response = new
            {
                token = $"Bearer {curToken}" ,
                user = jwtUserDto,
                expires_in = (int)jwtOptions.ValidFor.TotalSeconds
            };

            return JsonConvert.SerializeObject(response, serializerSettings);
        }
    }
}
