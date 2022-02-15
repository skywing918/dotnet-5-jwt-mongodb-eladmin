using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Common.Helper;
using WebAPI.Common.Models;
using WebAPI.Common.Services;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService service;
        public AuthController(UserService _service)
        {
            service = _service;
        }


        [HttpGet]
        [Route("code")]
        public async Task<IActionResult> getCode()
        {
            var captcha = await Captcha.GenerateCaptchaImageAsync();
            Guid newuuid = Guid.NewGuid();
            var uuid = $"code-key-{newuuid}";

            HttpContext.Session.SetString(uuid, captcha.CaptchaCode);

            byte[] imageBytes = captcha.CaptchaMemoryStream.ToArray();
            string base64String = Convert.ToBase64String(imageBytes);

            var response = new
            {
                img = $"data:image/png;base64, {base64String}",
                uuid = uuid,
            };
            var body = JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });
            return new OkObjectResult(body);           
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] CredentialsViewModel credentials)        
        {
            var code = HttpContext.Session.GetString(credentials.UUID);
            if (code == null|| code!= credentials.Code)
            {
                var response = new
                {
                    message = "验证码错误",
                    status= 400
                };
                return BadRequest(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
            }
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
