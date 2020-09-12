namespace BodyGenesis.Core.Entities
{
    public enum BillingPeriod
    {
        Month,
        Quarter,
        Week,
        Year
    }

    public static class BillingPeriodExtensions
    {
        public static string ToDisplayText(this BillingPeriod billingPeriod)
        {
            switch (billingPeriod)
            {
                case BillingPeriod.Month: return "monthly";
                case BillingPeriod.Quarter: return "quarterly";
                case BillingPeriod.Week: return "weekly";
                case BillingPeriod.Year: return "annually";
            }

            return string.Empty;
        }
    }
}
