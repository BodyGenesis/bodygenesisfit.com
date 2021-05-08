using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using BodyGenesis.Core.Entities;

namespace BodyGenesis.AccountManager.Server.Controllers
{
    [Route("api/membership-plans")]
    public class MembershipPlansController : ControllerBase
    {
        private readonly IRepository<MembershipPlan> _membershipPlansRepository;

        public MembershipPlansController(IRepository<MembershipPlan> membershipPlansRepository)
        {
            _membershipPlansRepository = membershipPlansRepository;
        }

        [HttpGet]
        [Route("")]
        public async Task<IReadOnlyCollection<MembershipPlan>> List()
        {
            return await _membershipPlansRepository.Query(p => !p.Deleted, sortExpression: p => p.Name);
        }
    }
}
