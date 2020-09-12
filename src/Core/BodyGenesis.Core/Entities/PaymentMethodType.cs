using System.ComponentModel;

namespace BodyGenesis.Core.Entities
{
    public enum PaymentMethodType
    {
        [Description("Bank Account")]
        BankAccount = 0,

        [Description("Credit Card")]
        CreditCard = 1,

        [Description("Gift Card")]
        GiftCard = 2
    }
}
