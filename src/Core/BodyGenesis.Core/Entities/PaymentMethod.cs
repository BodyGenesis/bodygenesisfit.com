using System;
using System.Security.Cryptography;
using System.Text;

namespace BodyGenesis.Core.Entities
{
    public class PaymentMethod
    {
        public string Name { get; set; } = string.Empty;
        public string AccountNumberHint { get; set; } = string.Empty;
        public DateTime? ExpirationDate { get; set; }
        public bool Expires => ExpirationDate.HasValue;
        public PaymentMethodType Type { get; set; }
        public bool Primary { get; set; }
        public byte[] AccountNumberIV { get; set; }
        public byte[] EncryptedAccountNumber { get; set; }
        public string NameOnCard { get; set; } = string.Empty;
        public string RoutingNumber { get; set; } = string.Empty;

        public string GetPlainTextAccountNumber(string key)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = AccountNumberIV;

                using (var cryptoTransform = aes.CreateDecryptor())
                {
                    byte[] buffer = cryptoTransform.TransformFinalBlock(EncryptedAccountNumber, 0, EncryptedAccountNumber.Length);

                    return Encoding.UTF8.GetString(buffer);
                }
            }
        }

        public void SetEncryptedAccountNumber(string plainTextAccountNumber, string key)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);

                aes.GenerateIV();

                AccountNumberIV = aes.IV;

                using (var cryptoTransform = aes.CreateEncryptor())
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(plainTextAccountNumber);

                    EncryptedAccountNumber = cryptoTransform.TransformFinalBlock(buffer, 0, buffer.Length);
                }
            }

            if (plainTextAccountNumber.Length < 4)
            {
                AccountNumberHint = new string('x', plainTextAccountNumber.Length);
            }

            else
            {
                AccountNumberHint = string.Concat(new string('x', plainTextAccountNumber.Length - 4), plainTextAccountNumber.Substring(plainTextAccountNumber.Length - 4));
            }
        }
    }
}
