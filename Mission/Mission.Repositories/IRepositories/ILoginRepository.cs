using Mission.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission.Repositories.IRepositories
{
    public interface ILoginRepository
    {
        User login(string EmailAddress, string Password);
    }
}
