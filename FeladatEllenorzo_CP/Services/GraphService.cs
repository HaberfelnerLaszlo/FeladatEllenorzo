// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.Identity;

using Microsoft.Graph;
using Microsoft.Graph.Drives.Item.Items.Item.Preview;
using Microsoft.Graph.Models;

using System.Runtime;

using TimeZoneConverter;

namespace FeladatEllenorzo_CP.Services
{
	public class GraphService : IGraphService
    {
        private IAuthenticationService _authenticationService;

        private User _user;
        private Stream _userPhoto;
        private TimeZoneInfo _userTimeZone;
        // App-ony auth token credential
        private static ClientSecretCredential? _clientSecretCredential;
        // Client configured with app-only authentication
        private static GraphServiceClient? _appClient;

        public GraphService(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        public void InitializeGraphForAppOnlyAuth(ISettingsService settings)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;

            if (accessType == NetworkAccess.Internet)
            {
                try
                {
                    if (_clientSecretCredential == null)
                    {
						var options = new ClientCertificateCredentialOptions()
						{
							SendCertificateChain = true,
						};
						_clientSecretCredential = new ClientSecretCredential(
                        settings.TenantId, settings.ClientId, settings.ClientSecret,options);
                    }
                    if (_appClient == null)
                    {
                        _appClient = new GraphServiceClient(_clientSecretCredential,
                            // Use the default scope, which will request the scopes
                            // configured on the app registration
                            new[] { "https://graph.microsoft.com/.default" });
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public Task CreateEventAsync(Event newEvent)
        {
            var graphClient = _authenticationService.GraphClient;

            return graphClient.Me.Events.PostAsync(newEvent);
        }

        public Task<EventCollectionResponse> GetCalendarForDateTimeRangeAsync(DateTime start, DateTime end, TimeZoneInfo timeZone)
        {
            var graphClient = _authenticationService.GraphClient;

            var timeZoneString = DeviceInfo.Current.Platform == DevicePlatform.WinUI ?
                timeZone.StandardName : timeZone.Id;
            
            return graphClient.Me
                .CalendarView
                .GetAsync(requestConfiguration =>
                {
                    requestConfiguration.Headers.Add("Prefer",
                        $"outlook.timezone=\"{timeZoneString}\"");
                    // Calendar view API sets the time period using query parameters
                    // ?startDatetime={start}&endDateTime={end}
                    requestConfiguration.QueryParameters.StartDateTime =
                        start.ToString("o");
                    requestConfiguration.QueryParameters.EndDateTime =
                        end.ToString("o");
                    requestConfiguration.QueryParameters.Select =
                        new[] { "subject", "organizer", "start", "end" };
                    requestConfiguration.QueryParameters.Orderby =
                        new[] { "start/DateTime" };
                    requestConfiguration.QueryParameters.Top = 50;
                });
        }

        public async Task<User> GetUserInfoAsync()
        {
            var graphClient = _authenticationService.GraphClient;

            if (_authenticationService.IsSignedIn)
            {
                if (_user == null)
                {
                    // Get the user, cache for subsequent calls
                    _user = await graphClient.Me.GetAsync(
                        requestConfiguration =>
                        {
                            requestConfiguration.QueryParameters.Select =
                                new[] { "displayName", "mail", "userPrincipalName", "id" };
                        });
                }
            }
            else
            {
                _user = null;
            }

            return _user;
        }

        public async Task<Stream> GetUserPhotoAsync()
        {
            var graphClient = _authenticationService.GraphClient;

            if (_authenticationService.IsSignedIn)
            {
                if (_userPhoto == null)
                {
                    // Get the user photo, cache for subsequent calls
                    _userPhoto = await graphClient.Me
                        .Photo
                        .Content
                        .GetAsync();
                }
            }
            else
            {
                _userPhoto = null;
            }

            return _userPhoto;
        }

        public async Task<TimeZoneInfo> GetUserTimeZoneAsync()
        {
            if (_authenticationService.IsSignedIn)
            {
                if (_userTimeZone == null)
                {
                    var user = await GetUserInfoAsync();
                    // Default to UTC if time zone is not set
                    _userTimeZone = TZConvert.GetTimeZoneInfo(user.MailboxSettings?.TimeZone ?? "UTC");
                }
            }
            else
            {
                _userTimeZone = null;
            }

            return _userTimeZone;
        }
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
            return _appClient.Education.Classes[id].Members.GetAsync(
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
            return _appClient.Education.Classes[id].Assignments.GetAsync((config) =>
            {
                // Only request specific properties
                config.QueryParameters.Select = new[] { "displayName", "id", "resources", "submissions", "assignTo", "dueDateTime" };
                config.QueryParameters.Expand = new[] { "*" };
            });
        }
        public Task<EducationSubmissionCollectionResponse?> GetFeladat(string classId, string id)
        {
            _ = _appClient ??
                throw new System.NullReferenceException("Graph has not been initialized for app-only auth");
            return _appClient.Education.Classes[classId].Assignments[id].Submissions.GetAsync();
        }
        public Task<EducationSubmissionResourceCollectionResponse?> GetResourcesCount(string classId, string feladatid, string submissionId)
        {
            _ = _appClient ??
                throw new System.NullReferenceException("Graph has not been initialized for app-only auth");
            return _appClient.Education.Classes[classId].Assignments[feladatid].Submissions[submissionId].SubmittedResources.GetAsync((config) => config.QueryParameters.Count = true);
        }
        public Task<EducationSubmissionCollectionResponse?> GetSubmittedFeladat(string classId, string id)
        {
            _ = _appClient ??
                throw new System.NullReferenceException("Graph has not been initialized for app-only auth");
            return _appClient.Education.Classes[classId].Assignments[id].Submissions.GetAsync((requestConfiguration) =>
            {
                requestConfiguration.QueryParameters.Filter = "status eq 'submitted'";
                requestConfiguration.QueryParameters.Count = true;
            });
        }
        public Task<EducationSubmissionCollectionResponse?> GetWorkingFeladat(string classId, string id)
        {
            _ = _appClient ??
                throw new System.NullReferenceException("Graph has not been initialized for app-only auth");
            return _appClient.Education.Classes[classId].Assignments[id].Submissions.GetAsync((requestConfiguration) =>
            {
                requestConfiguration.QueryParameters.Filter = "status eq 'working'";
                requestConfiguration.QueryParameters.Count = true;
            });
        }
        public Task<EducationSubmissionResourceCollectionResponse?> GetFeladatForras(string classId, string FeladatId, string BedandoId)
        {
            _ = _appClient ??
                throw new System.NullReferenceException("Graph has not been initialized for app-only auth");
            return _appClient.Education.Classes[classId].Assignments[FeladatId].Submissions[BedandoId].Resources.GetAsync();
        }
        public Task<EducationOutcomeCollectionResponse?> GetFeladatValasz(string classId, string FeladatId, string BedandoId)
        {
            _ = _appClient ??
                throw new System.NullReferenceException("Graph has not been initialized for app-only auth");
            return _appClient.Education.Classes[classId].Assignments[FeladatId].Submissions[BedandoId].Outcomes.GetAsync((requestConfiguration) =>
            {
                requestConfiguration.QueryParameters.Filter = "isof('microsoft.graph.educationFeedbackOutcome')";
            });
        }
        public Task<EducationOutcomeCollectionResponse?> GetFeladatPont(string classId, string FeladatId, string BedandoId)
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
            PreviewPostRequestBody body = new PreviewPostRequestBody();
            return _appClient.Drives[driveId].Items[itemId].Preview.PostAsync(body);
        }
        public async Task<bool> UpdateValasz(string classId, string FeladatId, string BeadandoId, string outcomeId, string valasz)
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
        public async Task<bool> UpdatePont(string classId, string FeladatId, string BeadandoId, string outcomeId, float pont)
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
        public async Task<bool> Return(string classId, string FeladatId, string BeadandoId)
        {
            _ = _appClient ??
                throw new System.NullReferenceException("Graph has not been initialized for app-only auth");

            var result = await _appClient.Education.Classes[classId].Assignments[FeladatId].Submissions[BeadandoId].Return.PostAsync();
            return result is not null;
        }

    }
}
