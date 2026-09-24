using AsisyaApi.Application.DTOs.Categories;
using AsisyaApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsisyaApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct) =>
            Ok(await _service.GetAllAsync(ct));

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct) =>
            (await _service.GetByIdAsync(id, ct)) is { } dto ? Ok(dto) : NotFound();

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto dto, CancellationToken ct)
        {
            try
            {
                var created = await _service.CreateAsync(dto, ct);
                return CreatedAtAction(nameof(GetById), new { id = created.CategoryId }, created);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}