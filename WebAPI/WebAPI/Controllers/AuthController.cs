using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Common.Helper;
using WebAPI.Common.Models;
using WebAPI.Common.Services;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService service;
        public AuthController(UserService _service)
        {
            service = _service;
        }

        

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] CredentialsViewModel credentials)        {
           
            var privateKey = "MIIBUwIBADANBgkqhkiG9w0BAQEFAASCAT0wggE5AgEAAkEA0vfvyTdGJkdbHkB8mp0f3FE0GYP3AYPaJF7jUd1M0XxFSE2ceK3k2kw20YvQ09NJKk+OMjWQl9WitG9pB6tSCQIDAQABAkA2SimBrWC2/wvauBuYqjCFwLvYiRYqZKThUS3MZlebXJiLB+Ue/gUifAAKIg1avttUZsHBHrop4qfJCwAI0+YRAiEA+W3NK/RaXtnRqmoUUkb59zsZUBLpvZgQPfj1MhyHDz0CIQDYhsAhPJ3mgS64NbUZmGWuuNKp5coY2GIj/zYDMJp6vQIgUueLFXv/eZ1ekgz2Oi67MNCk5jeTF2BurZqNLR3MSmUCIFT3Q6uHMtsB9Eha4u7hS31tj1UWE+D+ADzp59MGnoftAiBeHT7gDMuqeJHPL4b+kC+gzV4FGTfhR9q3tTbklZkD2A==";
            var password = RSAHelper.decryptByPrivateKey(privateKey, credentials.Password);
            var jwt = await service.Authenticate(credentials.UserName, password);
            if (jwt == null)
            {
                return Unauthorized();
            }
            return new OkObjectResult(jwt);
        }

        [Route("logout")]
        [HttpDelete]
        public async Task<IActionResult> logout()
        {
            return Ok();
        }

        // POST api/user
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User()
            {
                UserName = model.UserName,
                Email = model.Email
            };
            var result = await service.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest("fail");
            }
            return new OkObjectResult("Account created");
        }
    }
}
