using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using BodyGenesis.Shared;
using BodyGenesis.Core.Entities;
using BodyGenesis.Core.Entities.Queries;
using System;

namespace BodyGenesis.Core.UseCases
{
    public class SeedMembershipPlansRequestHandler : IRequestHandler<SeedMembershipPlansRequest, Result>
    {
        private readonly IRepository<MembershipPlan> _membershipPlanRepository;

        public SeedMembershipPlansRequestHandler(IRepository<MembershipPlan> membershipPlanRepository)
        {
            _membershipPlanRepository = membershipPlanRepository;
        }

        public MembershipPlan[] Plans { get; } = new MembershipPlan[]
        {
            new MembershipPlan
            {
                Id = Guid.Parse("65c5f92a-763c-4f0c-b037-a10daf4645ce"),
                BillingPeriod = BillingPeriod.Month,
                Name = "Basic Membership",
                Rate = 35
            },
            new MembershipPlan
            {
                Id = Guid.Parse("b28c6e4e-7f37-46bf-8b7e-10766e61eff0"),
                BillingPeriod = BillingPeriod.Month,
                Name = "Student Membership",
                Rate = 25
            },
            new MembershipPlan
            {
                Id = Guid.Parse("3166559c-e8f2-4209-94b1-eadf18641ee5"),
                BillingPeriod = BillingPeriod.Month,
                Name = "Family Membership",
                Rate = 60,
                QuantityBasedRates = new List<MembershipPlan.QuantityBasedRate>
                {
                    new MembershipPlan.QuantityBasedRate
                    {
                        ApplicationStrategy = MembershipPlan.RateApplicationStrategy.Static,
                        Quantity = 2,
                        Rate = 60
                    },
                    new MembershipPlan.QuantityBasedRate
                    {
                        ApplicationStrategy = MembershipPlan.RateApplicationStrategy.Static,
                        Quantity = 3,
                        Rate = 75
                    },
                    new MembershipPlan.QuantityBasedRate
                    {
                        ApplicationStrategy = MembershipPlan.RateApplicationStrategy.MultiplyWithBaseRate,
                        Quantity = 4,
                        Rate = 10,
                        BaseRate = 75
                    }
                }
            },
            new MembershipPlan
            {
                Id = Guid.Parse("8cafdad8-5407-4a77-a29f-1b41061e4a45"),
                BillingPeriod = BillingPeriod.Year,
                Name = "Annual Basic Membership",
                Rate = 360
            },
            new MembershipPlan
            {
                Id = Guid.Parse("faefd81a-b4b5-4049-8faf-d698531decd4"),
                BillingPeriod = BillingPeriod.Year,
                Name = "Annual Family Membership",
                Rate = 600,
                QuantityBasedRates = new List<MembershipPlan.QuantityBasedRate>
                {
                    new MembershipPlan.QuantityBasedRate
                    {
                        ApplicationStrategy = MembershipPlan.RateApplicationStrategy.Static,
                        Quantity = 2,
                        Rate = 600
                    },
                    new MembershipPlan.QuantityBasedRate
                    {
                        ApplicationStrategy = MembershipPlan.RateApplicationStrategy.Static,
                        Quantity = 3,
                        Rate = 720
                    },
                    new MembershipPlan.QuantityBasedRate
                    {
                        ApplicationStrategy = MembershipPlan.RateApplicationStrategy.MultiplyWithBaseRate,
                        Quantity = 4,
                        Rate = 120,
                        BaseRate = 720
                    }
                }
            }
        };

        public async Task<Result> Handle(SeedMembershipPlansRequest request, CancellationToken cancellationToken)
        {
            var plans = await _membershipPlanRepository.Query(new AllMembershipPlans());

            if (plans.Count < Plans.Length)
            {
                foreach (var plan in Plans)
                {
                    await _membershipPlanRepository.Save(plan);
                }
            }

            return Result.Success();
        }
    }
}
