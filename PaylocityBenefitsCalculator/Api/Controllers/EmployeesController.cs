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

    [SwaggerOperation(Summary = "Calculates paychecks for employee for a year, given the next pay date")]
    [HttpGet("{id}/paychecks")]
    public async Task<ActionResult<ApiResponse<List<Paycheck>>>> GetPaychecksForNextYear(int id, DateTime nextPayDate)
    {
        var employee = this.employeeRepository.GetById(id);
        if (employee == null)
        {
            return NotFound();
        }

        return new ApiResponse<List<Paycheck>>
        {
            Data = PaycheckCalculator.CalculatePaychecksForNextYear(employee, nextPayDate),
            Success = true
        };
    }
}
