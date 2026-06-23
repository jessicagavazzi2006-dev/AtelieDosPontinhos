using AtelieDosPontinhos.Application.Interfaces;
using AtelieDosPontinhos.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AtelieDosPontinhos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            var result = await _productService.SearchAsync(term);
            return Ok(result);
        }
    }
}