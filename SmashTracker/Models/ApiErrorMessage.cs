using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmashTracker.Models
{
	public class ApiErrorMessage
	{
		public string Message { get; set; }

		public string MessageDetail { get; set; }

		public object Data { get; set; }
	}
}