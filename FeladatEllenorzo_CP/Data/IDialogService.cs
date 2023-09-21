﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeladatEllenorzo_CP.Data
{
	internal interface IDialogService
	{
		Task<bool> DisplayConfirm(string title,string message, string accept, string cansel);
		Task DisplayAlert(string title, string message,string accept);
	}
}
