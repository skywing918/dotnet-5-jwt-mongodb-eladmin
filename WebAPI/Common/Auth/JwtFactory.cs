﻿using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Principal;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Configuration;
using WebAPI.Common.Models;
using WebAPI.Common.Helper;

namespace WebAPI.Common.Auth
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtIssuerOptions _jwtOptions;
        public IConfiguration Configuration { get; }

        public JwtFactory(IOptions<JwtIssuerOptions> jwtOptions, IConfiguration configuration)
        {
            Configuration = configuration;
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
        }

        public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity)
        {
            var claims = new[]
             {
                 new Claim(JwtRegisteredClaimNames.Sub, userName),
                 new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),

                 identity.FindFirst(Constants.Strings.JwtClaimIdentifiers.Id)
             };

            var currAllClaims = claims.Union(identity.FindAll(ClaimTypes.Role));

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: currAllClaims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public ClaimsIdentity GenerateClaimsIdentity(User user)
        {
            var defaultPic = Configuration.GetSection("UserSettings")["DefaultPhoto"];
            var currClaims = user.Roles.Select(r => new Claim(ClaimTypes.Role, r.ToString())).ToList();
            currClaims.Add(new Claim(Constants.Strings.JwtClaimIdentifiers.Id, user.Id.ToString()));
            //currClaims.Add(new Claim(Constants.Strings.JwtClaimIdentifiers.FullName, $"{user.FirstName} {user.LastName}"));
            currClaims.Add(new Claim(ClaimTypes.Name, user.UserName));
            currClaims.Add(new Claim(ClaimTypes.Email, user.Email));
            
            return new ClaimsIdentity(new GenericIdentity(user.UserName, "Token"), currClaims);
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }
    }
}
