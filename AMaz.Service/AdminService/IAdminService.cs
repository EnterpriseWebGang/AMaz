using AMaz.Entity;
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
        AuthenticateResponse Create(CreateRequest model);
        
    }
}
