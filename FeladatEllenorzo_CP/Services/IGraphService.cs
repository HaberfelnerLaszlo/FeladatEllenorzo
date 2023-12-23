// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Graph.Drives.Item.Items.Item.Preview;
using Microsoft.Graph.Models;

namespace FeladatEllenorzo_CP.Services
{
    /// <summary>
    /// Service that provides access to data in Microsoft Graph
    /// </summary>
    public interface IGraphService
    {
        /// <summary>
        /// Get the authenticated user's profile
        /// </summary>
        public Task<User> GetUserInfoAsync();

        /// <summary>
        /// Get the authenticated user's photo
        /// </summary>
        public Task<Stream> GetUserPhotoAsync();

        /// <summary>
        /// Get the authenticated user's time zone from their mailbox settings
        /// </summary>
        public Task<TimeZoneInfo> GetUserTimeZoneAsync();

        /// <summary>
        /// Get all events on the authenticated user's calendar in a given time period
        /// </summary>
        /// <param name="start">The start of the time period</param>
        /// <param name="end">The end of the time period</param>
        /// <param name="timeZone">The time zone to return date/time values in</param>
        public Task<EventCollectionResponse> GetCalendarForDateTimeRangeAsync(DateTime start, DateTime end, TimeZoneInfo timeZone);

        /// <summary>
        /// Create an event on the authenticated user's calendar
        /// </summary>
        public Task CreateEventAsync(Event newEvent);
        public Task<EducationClassCollectionResponse?> GetTaughtClasses(string id);
        public void InitializeGraphForAppOnlyAuth(ISettingsService settings);
        public Task<EducationUserCollectionResponse?> GetMembers(string id);
        public Task<EducationAssignmentCollectionResponse?> GetFeladatok(string id);
		public Task<EducationSubmissionCollectionResponse?> GetFeladat(string classId, string id);
		public Task<EducationSubmissionResourceCollectionResponse?> GetResourcesCount(string classId, string feladatid, string submissionId);
		public Task<EducationSubmissionCollectionResponse?> GetSubmittedFeladat(string classId, string id);
		public Task<EducationSubmissionCollectionResponse?> GetWorkingFeladat(string classId, string id);
		public Task<EducationSubmissionResourceCollectionResponse?> GetFeladatForras(string classId, string FeladatId, string BedandoId);
		public Task<EducationOutcomeCollectionResponse?> GetFeladatValasz(string classId, string FeladatId, string BedandoId);
		public Task<EducationOutcomeCollectionResponse?> GetFeladatPont(string classId, string FeladatId, string BedandoId);
		public Task<ItemPreviewInfo?> GetFile(string driveId, string itemId);
		public Task<bool> UpdateValasz(string classId, string FeladatId, string BeadandoId, string outcomeId, string valasz);
		public  Task<bool> UpdatePont(string classId, string FeladatId, string BeadandoId, string outcomeId, float pont);
		public Task<bool> Return(string classId, string FeladatId, string BeadandoId);
        public Task<bool> SendMe(string data);
	}
}
