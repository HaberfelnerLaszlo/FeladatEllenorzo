using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeladatEllenorzo_CP.Data
{
	internal class DialogService : IDialogService
	{
		public async Task DisplayAlert(string title, string message, string accept)
		{
			await Application.Current.MainPage.DisplayAlert(title, message, accept);
		}

		public async Task<bool> DisplayConfirm(string title, string message, string accept, string cansel)
		{
			return await Application.Current.MainPage.DisplayAlert(title, message, accept, cansel);
		}
	}
}
