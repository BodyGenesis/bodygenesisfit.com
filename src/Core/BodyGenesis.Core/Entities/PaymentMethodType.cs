using System.ComponentModel;

namespace BodyGenesis.Core.Entities
{
    public enum PaymentMethodType
    {
        BankAccount = 0,
        CreditCard = 1,
        GiftCard = 2
    }

    public static class PaymentMethodTypeExtensions
    {
        public static string ToDisplayText(this PaymentMethodType paymentMethodType)
        {
            switch (paymentMethodType)
            {
                case PaymentMethodType.BankAccount: return "Bank Account";
                case PaymentMethodType.CreditCard: return "Credit Card";
                case PaymentMethodType.GiftCard: return "Gift Card";
                default: return string.Empty;
            }
        }
    }
}
