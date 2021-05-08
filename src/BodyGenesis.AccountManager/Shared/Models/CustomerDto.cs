using System;
using System.Collections.Generic;

using BodyGenesis.Core.Entities;

namespace BodyGenesis.AccountManager.Shared.Models
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateDeleted { get; set; }
        public bool Deleted { get; set; }

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
        public List<PaymentMethodDto> PaymentMethods { get; set; } = new List<PaymentMethodDto>();

        public bool CanStartMembership => !string.IsNullOrWhiteSpace(Name)
            && !string.IsNullOrWhiteSpace(EmailAddress)
            && DateOfBirth.HasValue
            && !string.IsNullOrWhiteSpace(PhoneNumber)
            && !string.IsNullOrWhiteSpace(Address)
            && !string.IsNullOrWhiteSpace(City)
            && !string.IsNullOrWhiteSpace(State)
            && !string.IsNullOrWhiteSpace(ZipCode)
            && !string.IsNullOrWhiteSpace(PreferredLocation);

        public MembershipSubscription CurrentMembershipSubscription { get; set; }
    }
}
