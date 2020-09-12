using System;
using System.Collections.Generic;
using System.Linq;

namespace BodyGenesis.Core.Entities
{
    public class MembershipPlan : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public BillingPeriod BillingPeriod { get; set; }
        public decimal Rate { get; set; }
        public List<QuantityBasedRate> QuantityBasedRates { get; set; } = new List<QuantityBasedRate>();
        public bool HasQuantityBasedRates => QuantityBasedRates.Count > 0;

        public decimal GetRateForQuantity(int quantity)
        {
            if (!HasQuantityBasedRates)
            {
                return Rate;
            }

            var match = QuantityBasedRates
                .OrderByDescending(r => r.Quantity)
                .FirstOrDefault(r => r.Quantity <= quantity);

            if (match == null)
            {
                return Rate;
            }

            if (match.ApplicationStrategy == RateApplicationStrategy.Multiply)
            {
                return (match.Rate * quantity);
            }

            else if (match.ApplicationStrategy == RateApplicationStrategy.MultiplyWithBaseRate)
            {
                return (match.BaseRate + (match.Rate * quantity));
            }

            return match.Rate;
        }

        public class QuantityBasedRate
        {
            public int Quantity { get; set; }
            public decimal Rate { get; set; }
            public decimal BaseRate { get; set; }
            public RateApplicationStrategy ApplicationStrategy { get; set; }
        }

        public enum RateApplicationStrategy
        {
            /// <summary>
            /// Rate is calculated by multiplying the quantity rate by the quantity.
            /// </summary>
            Multiply,

            /// <summary>
            /// Rate is calculated by multiplying the quantity rate by the quantity and adding the quantity base rate.
            /// </summary>
            MultiplyWithBaseRate,

            /// <summary>
            /// Rate is simply the quantity rate.
            /// </summary>
            Static
        }

    }
}
