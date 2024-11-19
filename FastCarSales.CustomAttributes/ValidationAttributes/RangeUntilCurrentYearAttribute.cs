using System.ComponentModel.DataAnnotations;


namespace FastCarSales.CustomAttributes.ValidationAttributes
{
    public class RangeUntilCurrentYearAttribute : RangeAttribute
    {
        public RangeUntilCurrentYearAttribute(int minYear) : base(minYear, DateTime.UtcNow.Year)
        {
        }
    }
}
