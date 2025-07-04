using Mission.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission.Services.IServices
{
    public interface ILoginService
    {
        User login(string username, string password);
    }
}
