using AsisyaApi.Application.DTOs.Products;
using AsisyaApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsisyaApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductQueryDto query, CancellationToken ct) =>
            Ok(await _service.GetPagedAsync(query, ct));

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct) =>
            (await _service.GetByIdAsync(id, ct)) is { } dto ? Ok(dto) : NotFound();

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto dto, CancellationToken ct)
        {
            try
            {
                var created = await _service.CreateAsync(dto, ct);
                return CreatedAtAction(nameof(GetById), new { id = created.ProductId }, created);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("bulk")]
        public IActionResult Bulk(BulkCreateProductDto dto)
        {
            var id = _service.EnqueueBulkGeneration(dto);
            return Accepted(new { jobId = id, statusUrl = $"/api/Product/bulk/{id}" });
        }

        [Authorize]
        [HttpGet("bulk/{jobId:guid}")]
        public IActionResult BulkStatus(Guid jobId) =>
            _service.GetJobStatus(jobId) is { } status ? Ok(status) : NotFound();

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateProductDto dto, CancellationToken ct)
        {
            try
            {
                return await _service.UpdateAsync(id, dto, ct) ? NoContent() : NotFound();
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct) =>
            await _service.DeleteAsync(id, ct) ? NoContent() : NotFound();
    }
}