using System.ComponentModel;

namespace BodyGenesis.Core.Entities
{
    public enum PaymentMethodType
    {
        [Description("Bank Account")]
        BankAccount,

        [Description("Credit Card")]
        CreditCard,

        [Description("Gift Card")]
        GiftCard
    }
}
