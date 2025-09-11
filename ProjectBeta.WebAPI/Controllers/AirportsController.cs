using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectBeta.Application.Dto.Request;
using ProjectBeta.Application.Services;

namespace ProjectBeta.WebAPI.Controllers;

[ApiController]
// [Authorize]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/airports")]
public class AirportsController(IAirportService service) : ControllerBase
{
    [HttpGet]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetAll() => Ok(await service.GetAllAsync());

    [HttpGet("{id:guid}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetById(Guid id) => Ok(await service.GetByIdAsync(id));

    [HttpPost]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> Create([FromBody] AirportRequest request) =>
        Ok(await service.CreateAsync(request));
}