using System;
using System.Collections.Generic;
using System.Linq;

namespace BodyGenesis.Core.Entities
{
    public class Customer : EntityBase
    {
        public string Auth0UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public List<MembershipSubscription> MembershipSubscriptions { get; set; } = new List<MembershipSubscription>();
        public List<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();
        public string PhoneNumber { get; set; } = string.Empty;

        public MembershipSubscription CurrentMembershipSubscription => MembershipSubscriptions.FirstOrDefault((subscription) =>
        {
            var now = DateTime.Now;

            return !subscription.CancellationCompleted && (now >= subscription.StartDate) && (subscription.EndDate == null || now <= subscription.EndDate);
        });
    }
}
