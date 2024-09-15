using EduPlatform.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPlatform.Core.Abstractions {
    public interface IUsersService {
        public Task Register(string userName, string email, string password);

        public Task<string> Login(string email, string password);
    }
}
