using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Api.Repositories;
using Api.Services;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private IEmployeeRepository employeeRepository;

    public EmployeesController(IEmployeeRepository employeeRepository)
    {
        this.employeeRepository = employeeRepository;
    }


    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        var employee = this.employeeRepository.GetById(id);
        if (employee == null)
        {
            return NotFound();
        }

        return new ApiResponse<GetEmployeeDto>
        {
            Data = GetEmployeeDto.FromEmployee(employee),
            Success = true
        };
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        var employees = this.employeeRepository.GetAll();

        var result = new ApiResponse<List<GetEmployeeDto>>
        {
            Data = employees.Select(employee => GetEmployeeDto.FromEmployee(employee)).ToList(),
            Success = true
        };

        return result;
    }

    [SwaggerOperation(Summary = "Get paycheck for employee for a given pay date")]
    [HttpGet("{id}/paycheck")]
    public async Task<ActionResult<ApiResponse<Paycheck>>> GetPaycheck(int id, DateTime payDate)
    {
        var employee = this.employeeRepository.GetById(id);
        if (employee == null)
        {
            return NotFound();
        }

        return new ApiResponse<Paycheck>
        {
            Data = PaycheckCalculator.CalculatePaycheck(employee, payDate),
            Success = true
        };
    }
}
