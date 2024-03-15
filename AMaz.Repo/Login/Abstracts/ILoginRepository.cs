using AMaz.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaz.Repo
{
    public interface ILoginRepository
    {
        Task<User> GetUserByEmailAsync(string email);
    }
}
