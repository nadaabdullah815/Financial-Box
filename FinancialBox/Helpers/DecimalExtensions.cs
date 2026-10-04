namespace FinancialBox.Helpers
{
    public static class DecimalExtensions
    {
        public static string ToSmartString(this decimal value)
        {
            return value % 1 == 0 ? value.ToString("N0") : value.ToString("N2");
        }
    }
}