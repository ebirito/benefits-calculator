using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ApiTests.IntegrationTests;

public class EmployeeIntegrationTests : IntegrationTest, IClassFixture<WebApplicationFactory<Program>>
{
    public EmployeeIntegrationTests(WebApplicationFactory<Program> factory) : base(factory) { }

    [Fact]
    public async Task WhenAskedForAllEmployees_ShouldReturnAllEmployees()
    {
        var response = await HttpClient.GetAsync("/api/v1/employees");
        var employees = new List<GetEmployeeDto>
        {
            new()
            {
                Id = 1,
                FirstName = "LeBron",
                LastName = "James",
                Salary = 75420.99m,
                DateOfBirth = new DateTime(1984, 12, 30)
            },
            new()
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 92365.22m,
                DateOfBirth = new DateTime(1999, 8, 10),
                Dependents = new List<GetDependentDto>
                {
                    new()
                    {
                        Id = 1,
                        FirstName = "Spouse",
                        LastName = "Morant",
                        Relationship = Relationship.Spouse,
                        DateOfBirth = new DateTime(1998, 3, 3)
                    },
                    new()
                    {
                        Id = 2,
                        FirstName = "Child1",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2020, 6, 23)
                    },
                    new()
                    {
                        Id = 3,
                        FirstName = "Child2",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2021, 5, 18)
                    }
                }
            },
            new()
            {
                Id = 3,
                FirstName = "Michael",
                LastName = "Jordan",
                Salary = 143211.12m,
                DateOfBirth = new DateTime(1963, 2, 17),
                Dependents = new List<GetDependentDto>
                {
                    new()
                    {
                        Id = 4,
                        FirstName = "DP",
                        LastName = "Jordan",
                        Relationship = Relationship.DomesticPartner,
                        DateOfBirth = new DateTime(1974, 1, 2)
                    }
                }
            }
        };
        await response.ShouldReturn(HttpStatusCode.OK, employees);
    }

    [Fact]
    public async Task WhenAskedForAnEmployee_ShouldReturnCorrectEmployee()
    {
        var response = await HttpClient.GetAsync("/api/v1/employees/1");
        var employee = new GetEmployeeDto
        {
            Id = 1,
            FirstName = "LeBron",
            LastName = "James",
            Salary = 75420.99m,
            DateOfBirth = new DateTime(1984, 12, 30)
        };
        await response.ShouldReturn(HttpStatusCode.OK, employee);
    }
    
    [Fact]
    public async Task WhenAskedForANonexistentEmployee_ShouldReturn404()
    {
        var response = await HttpClient.GetAsync($"/api/v1/employees/{int.MinValue}");
        await response.ShouldReturn(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task WhenAskedForAnEmployeePaychecksForNextYear_ShouldCalculateAndReturnThem()
    {
        var response = await HttpClient.GetAsync("/api/v1/employees/1/paychecks?nextPayDate=2023-01-13");
        var expectedPaychecks = new List<Paycheck>
        {
            new Paycheck() {
                PayDate = new DateTime(2023, 1, 13),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 1, 27),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 2, 10),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 2, 24),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 3, 10),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 3, 24),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 4, 7),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 4, 21),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 5, 5),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 5, 19),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 6, 2),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 6, 16),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 6, 30),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 7, 14),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 7, 28),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 8, 11),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 8, 25),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 9, 8),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 9, 22),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 10, 6),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 10, 20),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 11, 3),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 11, 17),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 12, 1),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 12, 15),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            },
            new Paycheck() {
                PayDate = new DateTime(2023, 12, 29),
                GrossAmount = 2900.81M,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            }
        };
        await response.ShouldReturn(HttpStatusCode.OK, expectedPaychecks);
    }
}

