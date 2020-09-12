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
                return (match.BaseRate + (match.Rate * (quantity - (match.Quantity - 1))));
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
            /// The provided quantity has the rate's quantity, minus one, subtracted from it.
            /// As an example, the rate is set up with a quantity of 4, base rate of 75, and a rate of 10.
            /// If GetRateForQuantity is called with a quantity of 5, the base rate has 20 added to it because we subtract 1 from 4, then subtract the result (3) from the provided quantity (5).
            /// </summary>
            MultiplyWithBaseRate,

            /// <summary>
            /// Rate is simply the quantity rate.
            /// </summary>
            Static
        }

    }
}
