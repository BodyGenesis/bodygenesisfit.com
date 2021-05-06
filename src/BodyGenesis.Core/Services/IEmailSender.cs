using System.Collections.Generic;
using System.Threading.Tasks;

namespace BodyGenesis.Core.Services
{
    public interface IEmailSender
    {
        Task Send(IEnumerable<string> toAddresses, string subject, string body);
    }
}
