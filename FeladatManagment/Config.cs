using Microsoft.AspNetCore.Authentication;
using Microsoft.TeamsFx.Configuration;

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
