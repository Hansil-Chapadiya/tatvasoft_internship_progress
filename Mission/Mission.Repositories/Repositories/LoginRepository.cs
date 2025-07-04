using Mission.Entities.context;
using Mission.Entities.Entities;
using Mission.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission.Repositories.Repositories
{
    public class LoginRepository(MissionDbContext missionDbContext): ILoginRepository
    {
        private readonly MissionDbContext _missionDbContext = missionDbContext;
        public User login(string EmailAddress , string Password)
        {
            var user = _missionDbContext.Users.Where(x => x.EmailAddress == EmailAddress && x.Password == Password).FirstOrDefault();
            if (user == null) {
                return null;
            }
            return user;
        }
    }
}
