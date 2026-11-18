// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Android.App;
using Microsoft.Identity.Client;
using System.Diagnostics;

namespace FeladatEllenorzo_CP.Services
{
    public partial class AuthenticationService
    {
        private static bool IsBrokerAvailable()
        {
            var brokerPackages = new[]
            {
                "com.azure.authenticator",
                "com.microsoft.windowsintune.companyportal",
                "com.microsoft.identity.client.broker"
            };

            try
            {
                var packageManager = Android.App.Application.Context.PackageManager;
                foreach (var packageName in brokerPackages)
                {
                    try
                    {
                        packageManager.GetPackageInfo(packageName, 0);
                        return true;
                    }
                    catch (Exception)
                    {
                        // package is not installed, continue checking others
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Broker package check failed: {ex.Message}");
            }

            return false;
        }

        private partial Task RegisterMsalCacheAsync(ITokenCache tokenCache)
        {
            Debug.WriteLine("RegisterMsalCacheAsync - MSAL cache registration");
            // MSAL registers its own cache on Android
            return Task.CompletedTask;
        }

        private partial PublicClientApplicationBuilder AddPlatformConfiguration(PublicClientApplicationBuilder builder)
        {
            Debug.WriteLine("AddPlatformConfiguration - Android specific configuration");

            try
            {
                var activity = Platform.CurrentActivity;
                Debug.WriteLine($"Platform.CurrentActivity: {activity?.GetType().Name ?? "null"}");

                if (activity != null)
                {
                    var useBroker = IsBrokerAvailable();
                    Debug.WriteLine($"MSAL broker available: {useBroker}");

                    return builder
                        .WithParentActivityOrWindow(() => Platform.CurrentActivity)
                        .WithBroker(useBroker);
                }

                Debug.WriteLine("WARNING: Platform.CurrentActivity null!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR while resolving Platform.CurrentActivity: {ex.Message}");
            }

            // Fallback: use browser flow when the activity is unavailable or the broker is missing.
            return builder.WithBroker(false);
        }

#if ANDROID
        public string RedirectUri { get => "msauth://hu.haberfelner.feladatellenorzo/A2gt1lEwxmDQi0nNlYuzmBInmIc="; }
#else    
        public string RedirectUri { get => "http://localhost"; }
#endif
    }
}