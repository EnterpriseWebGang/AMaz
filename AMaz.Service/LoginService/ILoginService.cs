using AMaz.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMaz.Entity;

namespace AMaz.Service
{
    public interface ILoginService
    {
        Task<AuthenticateResponse> AuthenticateUserAsync(AuthenticateRequest model);

        Task SignInUserAsync(string  email);
    }
}
