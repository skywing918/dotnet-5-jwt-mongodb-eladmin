using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Common.Models;
using WebAPI.Common.Services;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService service;
        public UserController(UserService _service)
        {
            service = _service;
        }

        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return service.GetUsers();
        }

        [AllowAnonymous]
        [Route("authenticate")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] CredentialsViewModelr credentials)
        {
            var jwt = await service.Authenticate(credentials.UserName, credentials.Password);
            if (jwt == null)
            {
                return Unauthorized();
            }
            return new OkObjectResult(jwt);
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
