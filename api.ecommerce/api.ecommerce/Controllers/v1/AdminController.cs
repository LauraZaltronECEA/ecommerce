using api.services.Repositories;
using api.services.v1;
using Microsoft.AspNetCore.Mvc;

namespace api.ecommerce.Controllers.v1
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserRepository _userService;
        private readonly ISaleRepository _saleService;

        public AdminController(IUserRepository userService, ISaleRepository saleService)
        {
            _userService = userService;
            _saleService = saleService;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _userService.GetAllUsers());
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            return Ok(await _userService.DeleteUser(id));
        }

        [HttpGet("GetAllSales")]
        public async Task<IActionResult> GetAllSales()
        {
            return Ok(await _saleService.GetAllSales());
        }
    }
}
