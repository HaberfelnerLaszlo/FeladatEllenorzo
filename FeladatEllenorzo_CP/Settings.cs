using FeladatEllenorzo_CP.Data;

using Microsoft.Extensions.Configuration;
using Microsoft.Graph.Models;

using System.Reflection;

namespace FeladatEllenorzo_CP
{
	public class Settings
	{
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
		public string TenantId { get; set; }
        public string[] GraphScopes { get; set; }

        //public static Settings LoadSettings()
        //{
        //	// Load settings
        //	IConfiguration config = new ConfigurationBuilder()
        //		//.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
        //		// appsettings.json is required
        //		.AddJsonFile("appsettings.json", optional: false)
        //		// appsettings.Development.json" is optional, values override appsettings.json
        //		.AddJsonFile($"appsettings.Development.json", optional: true)
        //		// User secrets are optional, values override both JSON files
        //		//.AddUserSecrets<MauiProgram>()
        //		.Build();

        //	return config.GetRequiredSection("Settings").Get<Settings>() ??
        //		throw new Exception("Could not load app settings. See README for configuration instructions.");
        //}
    }
}
