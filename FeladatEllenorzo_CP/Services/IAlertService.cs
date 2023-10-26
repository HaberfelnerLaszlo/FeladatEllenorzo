// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace FeladatEllenorzo_CP.Services
{
    public interface IAlertService
    {
        /// <summary>
        /// Show an alert to the user
        /// </summary>
        Task ShowAlertAsync(string message, string title, string buttonLabel);
        Task<bool> DisplayConfirm(string title, string message, string accept, string cansel);

	}
}
