using Api.Dtos.Dependent;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Api.Repositories;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private IDependentRepository dependentRepository;

    public DependentsController(IDependentRepository dependentRepository)
    {
        this.dependentRepository = dependentRepository;
    }

    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var dependent = this.dependentRepository.GetById(id);
        if (dependent == null)
        {
            return NotFound();
        }

        return new ApiResponse<GetDependentDto>
        {
            Data = GetDependentDto.FromDependent(dependent),
            Success = true
        };
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var dependents = this.dependentRepository.GetAll();

        var result = new ApiResponse<List<GetDependentDto>>
        {
            Data = dependents.Select(dependent => GetDependentDto.FromDependent(dependent)).ToList(),
            Success = true
        };

        return result;
    }
}
