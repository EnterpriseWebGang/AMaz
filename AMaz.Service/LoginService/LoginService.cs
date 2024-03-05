using AMaz.Entity;
using AMaz.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaz.Service
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;

        public LoginService(ILoginRepository userRepository)
        {
            _loginRepository = userRepository;
        }

        public async Task<bool> AuthenticateUserAsync(string email, string password)
        {
            User user = await _loginRepository.GetUserByEmailAsync(email);

            if (user != null && user.Password == password)
            {
                return true;
            }

            return false;
        }
    }
}
