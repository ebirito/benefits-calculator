using Api.Models;

namespace Api.Services
{
    public static class PaycheckCalculator
    {
        private const int PaychecksPerYear = 26;
        private const decimal BenefitsBaseCostMonthly = 1000;
        private const decimal BenefitsPerDependentCostMonthly = 600;
        private const decimal LuxuryTaxYearlySalaryLimit = 80000;
        private const decimal LuxuryTaxPercentageOfYearlySalary = 2;
        private const int OldPersonTaxLimitYears = 50;
        private const decimal OldPersonTaxMonthly = 200;

        public static Paycheck CalculatePaycheck(Employee employee, DateTime payDate)
        {
            return new Paycheck
            {
                PayDate = payDate,
                GrossAmount = (employee.Salary / PaychecksPerYear).RoundToTwoDecimalPlaces(),
                BaseBenefitCost = ConvertCostFromMonthlyToPayPeriod(BenefitsBaseCostMonthly),
                DependentsBenefitCost = ConvertCostFromMonthlyToPayPeriod(BenefitsPerDependentCostMonthly * employee.Dependents.Count()),
                LuxuryTax = employee.Salary > LuxuryTaxYearlySalaryLimit ? (((employee.Salary * LuxuryTaxPercentageOfYearlySalary) / 100) / PaychecksPerYear).RoundToTwoDecimalPlaces() : 0,
                OldPersonTax = GetEmployeeAgeAtPayDate(employee.DateOfBirth, payDate) >= OldPersonTaxLimitYears ? ConvertCostFromMonthlyToPayPeriod(OldPersonTaxMonthly) : 0
            };
        }

        private static decimal ConvertCostFromMonthlyToPayPeriod(decimal costPerMonth)
        {
            return ((costPerMonth * 12) / PaychecksPerYear).RoundToTwoDecimalPlaces();
        }

        public static int GetEmployeeAgeAtPayDate(DateTime employeeDOB, DateTime payDate)
        {
            // From https://stackoverflow.com/a/1404
            // Calculate the age.
            var age = payDate.Year - employeeDOB.Year;
            // Go back to the year in which the person was born.  if birthdate hasn't arrived yet, subtract one year.  Also handles leap year
            if (employeeDOB.Date > payDate.AddYears(-age)) age--;

            return age;
        }
    }
}
