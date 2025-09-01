using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Configurations
{
    public class JwtSettings
    {
        public string Key { get; set; }
        public int TokenValidityInMinutes { get; set; }
        public string ValidIssuer { get; set; }
        public bool ValidateIssuer { get; set; }
        public string ValidAudience { get; set; }
        public bool ValidateAudience { get; set; }
        public int RefreshTokenValidityInDays { get; set; }

    }
}
