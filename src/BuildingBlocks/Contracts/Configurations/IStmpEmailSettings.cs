using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Configurations
{
    public interface IStmpEmailSettings
    {
        string DisplayName { get; set; }
        bool EnableVerification { get; set; }   
        string From { get; set; }
        string StmpServer { get; set; }
        bool UseSsl { get; set; }
        int Port { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
    }
}
