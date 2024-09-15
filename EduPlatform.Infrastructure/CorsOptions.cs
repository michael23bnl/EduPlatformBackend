using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPlatform.Infrastructure {
    public class CorsOptions {
        public bool AllowAnyOrigin { get; set; } = false;
        public string[] AllowedOrigins { get; set; } = [];
    }
}
