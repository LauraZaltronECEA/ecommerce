
using api.models.Entities;
using api.services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace api.ecommerce.Controllers.v1
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _service;

        public ProductController(IProductRepository service)
        {
            _service = service;
        }

        //acceso general del usuario comun

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetProducts());
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _service.GetProductById(id));
        }

        //acceso solo para el admin
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Product product)
        {
            return Ok(await _service.CreateProduct(product));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(Product product)
        {
            return Ok(await _service.UpdateProduct(product));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _service.DeleteProduct(id));
        }
    }
}
