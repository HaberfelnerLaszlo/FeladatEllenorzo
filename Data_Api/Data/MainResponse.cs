using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Api.Data
{
	public class MainResponse
	{
		public bool IsSuccess { get; set; }
		public string ErrorMessage { get; set; }=string.Empty;
		public object? Content { get; set; }
		public void Clear() 
		{
			this.IsSuccess = false;
			this.ErrorMessage = string.Empty;
			this.Content = null;
		}
	}
}
