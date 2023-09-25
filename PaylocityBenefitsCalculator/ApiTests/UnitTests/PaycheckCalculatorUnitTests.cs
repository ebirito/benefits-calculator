using System;
using System.Collections.Generic;
using Xunit;
using Api.Services;
using Api.Models;
using FluentAssertions;
using System.Linq;

namespace ApiTests.UnitTests
{
    public class PaycheckCalculatorUnitTests
    {
        [Theory]
        [MemberData(nameof(GetEmployeeAgeAtPayDateData))]
        public void GetEmployeeAgeAtPayDate_Tests(DateTime employeeDOB, DateTime payDate, int expectedAge)
        {
            PaycheckCalculator.GetEmployeeAgeAtPayDate(employeeDOB, payDate).Should().Be(expectedAge);
        }

        public static IEnumerable<object[]> GetEmployeeAgeAtPayDateData => new List<object[]>
        {
            new object[] { new DateTime(2000,1,1), new DateTime(2020, 1, 1), 20 },
            new object[] { new DateTime(2000,1,2), new DateTime(2020, 1, 1), 19 },
            new object[] { new DateTime(2000,1,1), new DateTime(2020, 1, 2), 20 },
            new object[] { new DateTime(2000,2,29), new DateTime(2019, 2, 28), 18 },
            new object[] { new DateTime(2000,2,29), new DateTime(2019, 3, 1), 19 },
        };

        [Fact]
        public void CalculatePaycheck_WhenEmployeeHasNoDependentsAndNoAdditionalTaxes_ShouldJustDeductBaseBenefitCost()
        {
            // Arrange
            var payDate = new DateTime(2020, 1, 1);
            var employee = new Employee
            {
                Salary = 52000,
                DateOfBirth = payDate.AddYears(-49)
            };
            var expectedPaycheck = new Paycheck
            {
                PayDate = payDate,
                GrossAmount = 2000,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = 0
            };

            // Act
            var paycheck = PaycheckCalculator.CalculatePaycheck(employee, payDate);

            // Assert
            this.AssertPaycheck(expectedPaycheck, paycheck);
        }

        [Theory]
        [MemberData(nameof(GetCalculatePaycheckWhenEmployeeHasDependentsData))]
        public void CalculatePaycheck_WhenEmployeeHasDependents_ShouldAddAdditionalCostPerDependent(int numberOfDependents, decimal expectedDependentsBenefitCost)
        {
            // Arrange
            var payDate = new DateTime(2020, 1, 1);
            var employee = new Employee
            {
                Salary = 52000,
                DateOfBirth = payDate.AddYears(-49),
                Dependents = Enumerable.Range(0, numberOfDependents).Select(i => new Dependent()).ToList()
            };
            var expectedPaycheck = new Paycheck
            {
                PayDate = payDate,
                GrossAmount = 2000,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = expectedDependentsBenefitCost,
                LuxuryTax = 0,
                OldPersonTax = 0
            };

            // Act
            var paycheck = PaycheckCalculator.CalculatePaycheck(employee, payDate);

            // Assert
            this.AssertPaycheck(expectedPaycheck, paycheck);
        }

        public static IEnumerable<object[]> GetCalculatePaycheckWhenEmployeeHasDependentsData => new List<object[]>
        {
            new object[] { 1, 276.92M },
            new object[] { 2, 553.85M },
            new object[] { 3, 830.77M },
        };

        [Theory]
        [MemberData(nameof(GetCalculatePaycheckWhenEmployeeHasSalaryThatExceedsLuxuryTaxLimit))]
        public void CalculatePaycheck_WhenEmployeeHasSalaryThatExceeds80000_ShouldDeductLuxuryTax(decimal employeeSalary, decimal expectedGrossAmount, decimal expectedLuxuryTax)
        {
            // Arrange
            var payDate = new DateTime(2020, 1, 1);
            var employee = new Employee
            {
                Salary = employeeSalary,
                DateOfBirth = payDate.AddYears(-49)
            };
            var expectedPaycheck = new Paycheck
            {
                PayDate = payDate,
                GrossAmount = expectedGrossAmount,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = expectedLuxuryTax,
                OldPersonTax = 0
            };

            // Act
            var paycheck = PaycheckCalculator.CalculatePaycheck(employee, payDate);

            // Assert
            this.AssertPaycheck(expectedPaycheck, paycheck);
        }

        public static IEnumerable<object[]> GetCalculatePaycheckWhenEmployeeHasSalaryThatExceedsLuxuryTaxLimit => new List<object[]>
        {
            new object[] { 79999, 3076.88M, 0 },
            new object[] { 80000, 3076.92M, 0 },
            new object[] { 80000.01M, 3076.92M, 61.54M },
            new object[] { 80001, 3076.96, 61.54M },
        };

        [Theory]
        [MemberData(nameof(GetCalculatePaycheckWhenEmployeeIsOver50AtPayDate))]
        public void CalculatePaycheck_WhenEmployeeIsOver50AtPayDate_ShouldDeductOldPersonTax(DateTime employeeDOB, DateTime payDate, decimal expectedOldPersonTax)
        {
            // Arrange
            var employee = new Employee
            {
                Salary = 52000,
                DateOfBirth = employeeDOB
            };
            var expectedPaycheck = new Paycheck
            {
                PayDate = payDate,
                GrossAmount = 2000,
                BaseBenefitCost = 461.54M,
                DependentsBenefitCost = 0,
                LuxuryTax = 0,
                OldPersonTax = expectedOldPersonTax
            };

            // Act
            var paycheck = PaycheckCalculator.CalculatePaycheck(employee, payDate);

            // Assert
            this.AssertPaycheck(expectedPaycheck, paycheck);
        }

        public static IEnumerable<object[]> GetCalculatePaycheckWhenEmployeeIsOver50AtPayDate => new List<object[]>
        {
            new object[] { new DateTime(2000, 1, 2), new DateTime(2050, 1, 1), 0 },
            new object[] { new DateTime(2000, 1, 1), new DateTime(2050, 1, 1), 92.31M },
            new object[] { new DateTime(2000, 1, 1), new DateTime(2050, 1, 2), 92.31M },
            new object[] { new DateTime(2000, 1, 1), new DateTime(2051, 1, 1), 92.31M },
        };

        private void AssertPaycheck(Paycheck expected, Paycheck actual)
        {
            actual.PayDate.Should().Be(expected.PayDate);
            actual.GrossAmount.Should().Be(expected.GrossAmount);
            actual.BaseBenefitCost.Should().Be(expected.BaseBenefitCost);
            actual.DependentsBenefitCost.Should().Be(expected.DependentsBenefitCost);
            actual.LuxuryTax.Should().Be(expected.LuxuryTax);
            actual.OldPersonTax.Should().Be(expected.OldPersonTax);
        }
    }
}
