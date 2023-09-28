namespace Api.Services
{
    public class PaydateCalculator
    {
        public static List<DateTime> CalculatePaydates(DateTime firstPayDate, int numberOfPaydates, int daysBetweenPayDates)
        {
            return Enumerable.Range(0, numberOfPaydates).Select(i => firstPayDate.AddDays(daysBetweenPayDates * i)).ToList();
        }
    }
}
