using Microsoft.AspNetCore.Authentication;
using Microsoft.TeamsFx.Configuration;

using AuthenticationOptions = Microsoft.TeamsFx.Configuration.AuthenticationOptions;

namespace FeladatManagment
{
    public class ConfigOptions
    {
        public TeamsFxOptions TeamsFx { get; set; }
    }
    public class TeamsFxOptions
    {
        public AuthenticationOptions Authentication { get; set; }
    }
}
