﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.Identity;

using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

using System.ComponentModel;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace FeladatEllenorzo_CP.Services
{
    public partial class AuthenticationService : IAuthenticationService, IAuthenticationProvider, INotifyPropertyChanged
    {
        private Lazy<Task<IPublicClientApplication>> _pca;
        private string _userIdentifier = string.Empty;
        private ISettingsService _settingsService;
        // App-ony auth token credential
        private static ClientSecretCredential? _clientSecretCredential;
        // Client configured with app-only authentication
        public GraphServiceClient GraphClient => new(this);

        private bool _isSignedIn = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsSignedIn
        {
            get => _isSignedIn;
            private set
            {
                _isSignedIn = value;
                OnPropertyChanged();
            }
        }
        public AuthenticationService(ISettingsService settingsService)
        {
            _pca = new Lazy<Task<IPublicClientApplication>>(InitializeMsalWithCache);
            _settingsService = settingsService;
             
       }

        /// <inheritdoc/>
        /// <remarks>
        /// Attempts to get a token silently from the cache. If this fails, the user needs to sign in.
        /// </remarks>
        public async Task<bool> IsAuthenticatedAsync()
        {
            var silentResult = await GetTokenSilentlyAsync();
            IsSignedIn = silentResult is not null;
            return IsSignedIn;
        }

        public async Task<bool> SignInAsync()
        {
            // First attempt to get a token silently
            var result = await GetTokenSilentlyAsync();
            if (result == null)
            {
                // If silent attempt didn't work, try an
                // interactive sign in
                result = await GetTokenInteractivelyAsync();
            }

            IsSignedIn = result is not null;
            return IsSignedIn;
        }

        public async Task SignOutAsync()
        {
            var pca = await _pca.Value;

            // Get all accounts (there should only be one)
            // and remove them from the cache
            var accounts = await pca.GetAccountsAsync();
            foreach (var account in accounts)
            {
                await pca.RemoveAsync(account);
            }

            // Clear the user identifier
            _userIdentifier = string.Empty;
            IsSignedIn = false;
        }
        /// <summary>
        /// Initializes a PublicClientApplication with a secure serialized cache.
        /// </summary>
        private async Task<IPublicClientApplication> InitializeMsalWithCache()
        {
            // Initialize the PublicClientApplication
            var builder = PublicClientApplicationBuilder
                .Create(_settingsService.ClientId)
                .WithRedirectUri(_settingsService.RedirectUri)
                .WithTenantId(_settingsService.TenantId);

            builder = AddPlatformConfiguration(builder);

            var pca = builder.Build();

            await RegisterMsalCacheAsync(pca.UserTokenCache);

            return pca;
        }
        private partial PublicClientApplicationBuilder AddPlatformConfiguration(PublicClientApplicationBuilder builder);
        private partial Task RegisterMsalCacheAsync(ITokenCache tokenCache);

        /// <summary>
        /// Get the user account from the MSAL cache.
        /// </summary>
        private async Task<IAccount> GetUserAccountAsync()
        {
            try
            {
                var pca = await _pca.Value;

                if (string.IsNullOrEmpty(_userIdentifier))
                {
                    // If no saved user ID, then get the first account.
                    // There should only be one account in the cache anyway.
                    var accounts = await pca.GetAccountsAsync();
                    var account = accounts.FirstOrDefault();

                    // Save the user ID so this is easier next time
                    _userIdentifier = account?.HomeAccountId.Identifier ?? string.Empty;
                    return account;
                }

                // If there's a saved user ID use it to get the account
                return await pca.GetAccountAsync(_userIdentifier);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Attempt to acquire a token silently (no prompts).
        /// </summary>
        private async Task<AuthenticationResult> GetTokenSilentlyAsync()
        {
            try
            {
                var pca = await _pca.Value;

                var account = await GetUserAccountAsync();
                if (account == null) return null;

                return await pca.AcquireTokenSilent(_settingsService.GraphScopes, account)
                    .ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
                return null;
            }
        }

        /// <summary>
        /// Attempts to get a token interactively using the device's browser.
        /// </summary>
        private async Task<AuthenticationResult> GetTokenInteractivelyAsync()
        {
            var pca = await _pca.Value;

            var result = await pca.AcquireTokenInteractive(_settingsService.GraphScopes)
                .WithUseEmbeddedWebView(false)
                .ExecuteAsync();

            // Store the user ID to make account retrieval easier
            _userIdentifier = result.Account.HomeAccountId.Identifier;
            return result;
        }

        public async Task AuthenticateRequestAsync(
            RequestInformation request,
            Dictionary<string, object> additionalAuthenticationContext = null,
            CancellationToken cancellationToken = default)
        {
            if (request.URI.Host == "graph.microsoft.com")
            {
                // First try to get the token silently
                var result = await GetTokenSilentlyAsync();
                if (result == null)
                {
                    // If silent acquisition fails, try interactive
                    result = await GetTokenInteractivelyAsync();
                }

                request.Headers.Add("Authorization", $"Bearer {result.AccessToken}");
            }
        }
        public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
