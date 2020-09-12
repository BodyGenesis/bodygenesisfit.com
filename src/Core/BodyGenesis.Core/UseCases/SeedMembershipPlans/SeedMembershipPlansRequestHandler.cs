using System.Threading;
using System.Threading.Tasks;

using MediatR;

using BodyGenesis.Shared;
using BodyGenesis.Core.Entities;
using BodyGenesis.Core.Entities.Queries;

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
                BillingPeriod = BillingPeriod.Month,
                Name = "Basic Membership",
                Rate = 35
            },
            new MembershipPlan
            {
                BillingPeriod = BillingPeriod.Month,
                Name = "Student Membership",
                Rate = 25
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
