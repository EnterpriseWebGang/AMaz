using AMaz.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaz.Service
{
    public interface IAdminService
    {
        bool AdminCheck();
        Task<IdentityResult> CreateAccount(CreateAccountRequest model);
        void DeleteAcount(Guid id);

    }
}
