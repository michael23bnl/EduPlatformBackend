using EduPlatform.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EduPlatform.Core.Abstractions {
    public interface IJwtProvider {
        public string GenerateToken(UserModel userModel);
    }
}
