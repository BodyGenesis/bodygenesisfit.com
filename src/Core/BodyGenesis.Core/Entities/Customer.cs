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
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string PreferredLocation { get; set; } = string.Empty;
        public List<MembershipSubscription> MembershipSubscriptions { get; set; } = new List<MembershipSubscription>();
        public List<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();

        public bool CanStartMembership => !string.IsNullOrWhiteSpace(Name)
            && !string.IsNullOrWhiteSpace(EmailAddress)
            && DateOfBirth.HasValue
            && !string.IsNullOrWhiteSpace(PhoneNumber)
            && !string.IsNullOrWhiteSpace(Address)
            && !string.IsNullOrWhiteSpace(City)
            && !string.IsNullOrWhiteSpace(State)
            && !string.IsNullOrWhiteSpace(ZipCode)
            && !string.IsNullOrWhiteSpace(PreferredLocation);

        public MembershipSubscription CurrentMembershipSubscription => MembershipSubscriptions.FirstOrDefault((subscription) =>
        {
            var now = DateTime.Now;

            return !subscription.CancellationCompleted && (now >= subscription.StartDate) && (subscription.EndDate == null || now <= subscription.EndDate);
        });
    }
}
