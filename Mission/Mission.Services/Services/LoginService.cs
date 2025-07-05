using Mission.Entities.Entities;
using Mission.Repositories.IRepositories;
using Mission.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mission.Services.Helper;
using Mission.Entities.context;

//namespace Mission.Services.Services
//{
//    public class LoginService: ILoginService
//    {
//        private readonly ILoginRepository _loginRepository;
//        public LoginService(ILoginRepository loginRepository)
//        {
//            _loginRepository = loginRepository;
//        }
//        public User login(string username, string password)
//        {
//            return this._loginRepository.login(username, password);
//        }
//    }
//}

namespace Mission.Services.Services
{
    public class LoginService : ILoginService
    {
        private readonly MissionDbContext _context;
        public LoginService(MissionDbContext context)
        {
            _context = context;
        }

        public User login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.EmailAddress == email && !u.IsDeleted);

            if (user == null)
            {
                return null; // email not found
            }

            bool isValid = PasswordHasher.VerifyPassword(password, user.Password);
            if (!isValid)
            {
                return null; // invalid password
            }

            return user; // success
        }
    }
}
