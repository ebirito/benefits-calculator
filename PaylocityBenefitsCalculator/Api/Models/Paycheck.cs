namespace Api.Models;

public class Paycheck
{
    public DateTime PayDate { get; set; }
    public decimal GrossAmount { get; set; }
    public decimal BaseBenefitCost { get; set; }
    public decimal DependentsBenefitCost { get; set; }
    public decimal LuxuryTax { get; set; }
    public decimal OldPersonTax { get; set; }
    public decimal TotalDeductions => BaseBenefitCost + DependentsBenefitCost + LuxuryTax + OldPersonTax;
    public decimal NetAmount => GrossAmount - TotalDeductions;
}
