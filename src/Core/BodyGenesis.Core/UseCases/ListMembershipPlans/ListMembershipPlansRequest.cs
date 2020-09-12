using System.Collections.Generic;

using MediatR;

using BodyGenesis.Core.Entities;
using BodyGenesis.Shared;

namespace BodyGenesis.Core.UseCases
{
    public class ListMembershipPlansRequest : IRequest<Result<IReadOnlyCollection<MembershipPlan>>>
    {
    }
}
