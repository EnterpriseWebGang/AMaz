using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMaz.DB;
using AMaz.Entity;
using Microsoft.EntityFrameworkCore;

namespace AMaz.Repo
{
    public class AdminReponsitory : IAdminResponsitory
    {
        private readonly AMazDbContext _db;

        public AdminReponsitory(AMazDbContext db)
        {
            _db = db;
        }

        public bool IsUserExistsAsync(string email)
        {
            return  _db.Users.Any(x => x.Email == email);
        }
        public void AddUser(User user)
        {
             _db.Users.Add(user);
        }

        public void SaveChanges()
        {
             _db.SaveChanges();
        }
         public void DeleteUser(User user)
        {
           _db.Users.Remove(user);
        }

       public User GetById (Guid id)
        {
            var account = _db.Users.Find(id);
            if (account == null) throw new KeyNotFoundException("Account not found");
            return account;
        }

        
    }
}
