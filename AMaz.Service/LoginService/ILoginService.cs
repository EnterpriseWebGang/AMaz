using AMaz.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaz.Service
{
    public interface ILoginService
    {
        Task<bool> AuthenticateUserAsync(string email, string password);

        Task<bool> SignInUserAsync(string email);
    }
}
