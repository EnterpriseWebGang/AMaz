using AMaz.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaz.Repo
{
    public interface IAdminResponsitory
    {
        bool IsUserExistsAsync(string email);
        void AddUser(User user);
        void SaveChanges();

        void DeleteUser(User user);
        User GetById(Guid id);
    }
}
