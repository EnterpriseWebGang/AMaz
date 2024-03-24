using AMaz.Common;
using AMaz.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaz.Service
{
    public interface IEmailService
    {
        Task Send(string to, string subject, string html);
        Task SendCreateResetPasswordEmail(User user);
        Task SendCreateAccountEmail(CreateAccountRequest request);
        Task SendCreateContributionEmail(Contribution contribution);
    }
}
