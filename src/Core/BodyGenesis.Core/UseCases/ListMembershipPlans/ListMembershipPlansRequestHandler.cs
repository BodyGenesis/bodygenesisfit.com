using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using BodyGenesis.Core.Entities;
using BodyGenesis.Core.Entities.Queries;
using BodyGenesis.Shared;

namespace BodyGenesis.Core.UseCases
{
    public class ListMembershipPlansRequestHandler : IRequestHandler<ListMembershipPlansRequest, Result<IReadOnlyCollection<MembershipPlan>>>
    {
        private readonly IRepository<MembershipPlan> _membershipPlanRepository;

        public ListMembershipPlansRequestHandler(IRepository<MembershipPlan> membershipPlanRepository)
        {
            _membershipPlanRepository = membershipPlanRepository;
        }

        public async Task<Result<IReadOnlyCollection<MembershipPlan>>> Handle(ListMembershipPlansRequest request, CancellationToken cancellationToken)
        {
            var plans = await _membershipPlanRepository.Query(new AllMembershipPlans());

            return Result<IReadOnlyCollection<MembershipPlan>>.Success(plans);
        }
    }
}
