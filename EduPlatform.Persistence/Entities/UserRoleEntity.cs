﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPlatform.Persistence.Entities {
    public class UserRoleEntity {
        public Guid UserId { get; set; }

        public int RoleId { get; set; }
    }
}
