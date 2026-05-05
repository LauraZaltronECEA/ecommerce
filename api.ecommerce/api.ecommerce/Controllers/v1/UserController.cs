using api.models.DTO;
using api.services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace api.ecommerce.Controllers.v1
{
    [Route("api/users")]//ruta base para el controlador
    [ApiController] //indica que es un controlador de API
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _service;

        public UserController(IUserRepository service)
        {
            _service = service;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserDTO user)
        {
            return Ok(await _service.Login(user));
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDTO user)
        {
            return Ok(await _service.Register(user));
        }

    }
}
