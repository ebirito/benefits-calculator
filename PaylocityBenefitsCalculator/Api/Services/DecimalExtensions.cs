namespace Api.Services
{
    public static class DecimalExtensions
    {
        public static decimal RoundToTwoDecimalPlaces(this decimal value)
        {
            return Math.Round(value, 2);
        }
    }
}
