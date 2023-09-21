
using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Drives.Item.Items.Item.CreateLink;
using Microsoft.Graph.Drives.Item.Items.Item.Preview;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Abstractions;

namespace FeladatEllenorzo_CP.Data
{
class GraphService 
{
	private static Settings? _settings;
	// App-ony auth token credential
	private static ClientSecretCredential? _clientSecretCredential;
	// Client configured with app-only authentication
	private static GraphServiceClient? _appClient;
	// User auth token credential
	private static InteractiveBrowserCredential? _interactiveCredential;
	// Client configured with user authentication
	private static GraphServiceClient? _userClient;

	public static void InitializeGraphForUserAuth(Settings settings)
	{
		_settings = settings;
		var options = new InteractiveBrowserCredentialOptions
		{
			TenantId = settings.TenantId,
			ClientId = settings.ClientId,
			AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
			RedirectUri = new Uri("https://localhost"),
		};

		_interactiveCredential = new(options);
		_userClient = new GraphServiceClient(_interactiveCredential, settings.GraphUserScopes);
	}
	// </UserAuthConfigSnippet>
	// <GetUserTokenSnippet>
	public async Task<string> GetUserTokenAsync()
	{
		// Ensure credential isn't null
		_ = _interactiveCredential ??
			throw new System.NullReferenceException("Graph has not been initialized for user auth");

		// Ensure scopes isn't null
		_ = _settings?.GraphUserScopes ?? throw new System.ArgumentNullException("Argument 'scopes' cannot be null");

		// Request token with given scopes
		var context = new TokenRequestContext(_settings.GraphUserScopes);
		var response = await _interactiveCredential.GetTokenAsync(context);
		return response.Token;
	}
		// </GetUserTokenSnippet>

		public static void InitializeGraphForAppOnlyAuth(Settings settings)
	{
		_settings = settings;

		// Ensure settings isn't null
		_ = settings ??
			throw new System.NullReferenceException("Settings cannot be null");
		_settings = settings;

		if (_clientSecretCredential == null)
		{
			_clientSecretCredential = new ClientSecretCredential(
			_settings.TenantId, _settings.ClientId, _settings.ClientSecret);
		}

		if (_appClient == null)
		{
			_appClient = new GraphServiceClient(_clientSecretCredential,
				// Use the default scope, which will request the scopes
				// configured on the app registration
				new[] { "https://graph.microsoft.com/.default" });
		}
	}
	// </AppOnlyAuthConfigSnippet>

	// <GetAppOnlyTokenSnippet>
	public static async Task<string> GetAppOnlyTokenAsync()
	{
		// Ensure credential isn't null
		_ = _clientSecretCredential ??
			throw new System.NullReferenceException("Graph has not been initialized for app-only auth");

		// Request token with given scopes
		var context = new TokenRequestContext(new[] { "https://graph.microsoft.com/.default" });
		var response = await _clientSecretCredential.GetTokenAsync(context);
		return response.Token;
	}
	// </GetAppOnlyTokenSnippet>

	// <GetUsersSnippet>
	public Task<UserCollectionResponse?> GetUsersAsync()
	{
		// Ensure client isn't null
		_ = _appClient ??
			throw new System.NullReferenceException("Graph has not been initialized for app-only auth");

		return _appClient.Users.GetAsync((config) =>
		{
			// Only request specific properties
			config.QueryParameters.Select = new[] { "displayName", "id", "mail" };
			// Get at most 25 results
			config.QueryParameters.Top = 25;
			// Sort by display name
			config.QueryParameters.Orderby = new[] { "displayName" };
		});
	}
		// </GetUsersSnippet>

#pragma warning disable CS1998
		// <MakeGraphCallSnippet>
		// This function serves as a playground for testing Graph snippets
		// or other code
		public Task<User?> GetMe()
	{
			// Ensure client isn't null
			_ = _userClient ??
				throw new System.NullReferenceException("Graph has not been initialized for user auth");

			return _userClient.Me.GetAsync((config) =>
			{
				// Only request specific properties
				config.QueryParameters.Select = new[] { "displayName", "mail", "userPrincipalName","id" };
			});     // INSERT YOUR CODE HERE
		}
	// </MakeGraphCallSnippet>
	public Task<EducationClassCollectionResponse?> GetTaughtClasses(string id)
	{
		// Code snippets are only available for the latest version. Current version is 5.x

		_ = _appClient ??
			throw new System.NullReferenceException("Graph has not been initialized for app-only auth");

			return _appClient.Education.Users[id].TaughtClasses.GetAsync();
		//return _appClient.Education.Users[id].TaughtClasses.GetAsync((config) =>
		//	{
		//		// Only request specific properties
		//		config.QueryParameters.Select = new[] { "displayName", "id" };
		//	});
	}
	public Task<EducationUserCollectionResponse?> GetMembers(string id)
		{
		_ = _appClient ??
			throw new System.NullReferenceException("Graph has not been initialized for app-only auth");
			return  _appClient.Education.Classes[id].Members.GetAsync(
				(config) =>
			{
				// Only request specific properties
				config.QueryParameters.Select = new[] { "displayName", "id", "primaryRole" };
				//config.QueryParameters.Expand = new[] { "*"};
			}
			);

		}
	public Task<EducationAssignmentCollectionResponse?> GetFeladatok(string id)
		{
		_ = _appClient ??
			throw new System.NullReferenceException("Graph has not been initialized for app-only auth");
			return  _appClient.Education.Classes[id].Assignments.GetAsync((config) =>
			{
				// Only request specific properties
				config.QueryParameters.Select = new[] { "displayName", "id","resources", "submissions", "assignTo" };
				config.QueryParameters.Expand = new[] { "*"};
			});
		}
	public Task<EducationSubmissionCollectionResponse?> GetFeladat(string classId, string id)
		{
		_ = _appClient ??
			throw new System.NullReferenceException("Graph has not been initialized for app-only auth");
			return _appClient.Education.Classes[classId].Assignments[id].Submissions.GetAsync();
		}
	public Task<EducationSubmissionResourceCollectionResponse?> GetFeladatForras(string classId, string FeladatId,string BedandoId)
		{
		_ = _appClient ??
			throw new System.NullReferenceException("Graph has not been initialized for app-only auth");
			return _appClient.Education.Classes[classId].Assignments[FeladatId].Submissions[BedandoId].Resources.GetAsync();
		}
	public Task<EducationOutcomeCollectionResponse?> GetFeladatValasz(string classId, string FeladatId,string BedandoId)
		{
		_ = _appClient ??
			throw new System.NullReferenceException("Graph has not been initialized for app-only auth");
			return _appClient.Education.Classes[classId].Assignments[FeladatId].Submissions[BedandoId].Outcomes.GetAsync((requestConfiguration) =>
			{
				requestConfiguration.QueryParameters.Filter = "isof('microsoft.graph.educationFeedbackOutcome')";
			});
		}
	public Task<EducationOutcomeCollectionResponse?> GetFeladatPont(string classId, string FeladatId,string BedandoId)
		{
		_ = _appClient ??
			throw new System.NullReferenceException("Graph has not been initialized for app-only auth");
			return _appClient.Education.Classes[classId].Assignments[FeladatId].Submissions[BedandoId].Outcomes.GetAsync((requestConfiguration) =>
			{
				requestConfiguration.QueryParameters.Filter = "isof('microsoft.graph.educationPointsOutcome')";
			});
		}
	 public Task<ItemPreviewInfo?> GetFile(string driveId, string itemId)
		{
		_ = _appClient ??
			throw new System.NullReferenceException("Graph has not been initialized for app-only auth");
			PreviewPostRequestBody body= new PreviewPostRequestBody();
			return _appClient.Drives[driveId].Items[itemId].Preview.PostAsync(body);
		}
	public async Task<bool> UpdateValasz(string classId, string FeladatId,string BeadandoId,string outcomeId,string valasz)
		{
			// Code snippets are only available for the latest version. Current version is 5.x

		_ = _appClient ??
			throw new System.NullReferenceException("Graph has not been initialized for app-only auth");

			var requestBody = new EducationFeedbackOutcome
			{
				OdataType = "#microsoft.graph.educationFeedbackOutcome",
				Feedback = new EducationFeedback
				{
					Text = new EducationItemBody
					{
						Content = valasz,
						ContentType = BodyType.Text,
					},
				},
			};
			try
			{
				var result = await _appClient.Education.Classes[classId].Assignments[FeladatId].Submissions[BeadandoId].Outcomes[outcomeId].PatchAsync(requestBody);
				return result is not null;
			}
			catch (Exception e)
			{
				throw;
			}
			
		}
	public async Task<bool> UpdatePont(string classId, string FeladatId,string BeadandoId,string outcomeId,float pont)
		{
			// Code snippets are only available for the latest version. Current version is 5.x

		_ = _appClient ??
			throw new System.NullReferenceException("Graph has not been initialized for app-only auth");

			var requestBody = new EducationPointsOutcome
			{
				OdataType = "#microsoft.graph.educationPointsOutcome",
				Points = new EducationAssignmentPointsGrade
				{
					OdataType = "#microsoft.graph.educationAssignmentPointsGrade",
					Points = pont,
				},
			};
			var result = await _appClient.Education.Classes[classId].Assignments[FeladatId].Submissions[BeadandoId].Outcomes[outcomeId].PatchAsync(requestBody);
			return result is not null;
		}
	public async Task Return(string classId, string FeladatId,string BeadandoId)
		{
			// Code snippets are only available for the latest version. Current version is 5.x

		_ = _appClient ??
			throw new System.NullReferenceException("Graph has not been initialized for app-only auth");

			var result = await _appClient.Education.Classes[classId].Assignments[FeladatId].Submissions[BeadandoId].Return.PostAsync();
		}
}
}
