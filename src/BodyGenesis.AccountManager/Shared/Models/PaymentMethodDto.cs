using System;

using BodyGenesis.Core.Entities;

namespace BodyGenesis.AccountManager.Shared.Models
{
    public class PaymentMethodDto
    {
        public string AccountNumberHint { get; set; } = string.Empty;
        public DateTime? ExpirationDate { get; set; }
        public bool Expires => ExpirationDate.HasValue;
        public PaymentMethodType Type { get; set; }
        public bool Primary { get; set; }
        public string NameOnCard { get; set; } = string.Empty;
        public string RoutingNumber { get; set; } = string.Empty;
    }
}
