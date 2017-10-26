using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmashTracker.Models
{
	public class BaseEntity
	{
		public DateTime? Created { get; set; }
		public string UserCreated { get; set; }
		public DateTime? Modified { get; set; }
		public string UserModified { get; set; }
		public bool Deleted { get; set; }
		public string UserDeleted { get; set; }
	}
}