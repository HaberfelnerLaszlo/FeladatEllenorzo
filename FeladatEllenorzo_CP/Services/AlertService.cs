// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace FeladatEllenorzo_CP.Services
{
    /// <summary>
    /// Service to display alerts via MAUI DisplayAlert
    /// </summary>
    public class AlertService : IAlertService
    {
        public Task ShowAlertAsync(string message, string title, string buttonLabel)
        {
            return Application.Current.MainPage.DisplayAlert(title, message, buttonLabel);
        }
		public async Task<bool> DisplayConfirm(string title, string message, string accept, string cansel)
		{
			return await Application.Current.MainPage.DisplayAlert(title, message, accept, cansel);
		}
	}
}
