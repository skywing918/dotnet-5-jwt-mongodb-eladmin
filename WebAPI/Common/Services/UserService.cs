namespace WebAPI.Common.Services
{
    using AspNetCore.Identity.MongoDbCore.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using WebAPI.Common.Auth;
    using WebAPI.Common.Helper;
    using WebAPI.Common.Models;

    public class UserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;

        public UserService(UserManager<User> userManager, RoleManager<Role> roleManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
        }

        public IQueryable<User> GetUsers() => _userManager.Users;

        public async Task<IdentityResult> CreateAsync(User user, string password)
        {

            var res = await _userManager.CreateAsync(user, password);
           
            return res;
        }

        public async Task<string> Authenticate(string userName, string password)
        {
            var identity = await GetClaimsIdentity(userName, password);
            if (identity == null)
            {
                return null;
            }
            var currRoles = identity.Claims.Where(c => c.Type == ClaimTypes.Role).Select(r => r.Value).ToList();
            var roles = _roleManager.Roles.ToList().Where(r => {
                var currid = r.Id.ToString();
                if (currRoles.Contains(currid))
                {
                    return true;
                }
                return false;                
                }).ToList();
            var permissions = new List<string> { "admin" };
            return await Tokens.GenerateJwt(identity, permissions, roles, _jwtFactory, userName, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userToVerify));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}
