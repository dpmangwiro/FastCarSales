using System.ComponentModel.DataAnnotations;

namespace FastCarSales.CustomAttributes.ValidationAttributes
{
    public class RangeWithCustomFormatAttribute : RangeAttribute
    {
        public RangeWithCustomFormatAttribute(double minimum, double maximum, string propertyDisplayName) : base(minimum,
            maximum)
        {
            this.ErrorMessage = $"The {propertyDisplayName} must be between {minimum:N0} and {maximum:N0}."; //1000000 ==> 1 000 000
        }
    }
}
