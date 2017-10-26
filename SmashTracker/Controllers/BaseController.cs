using SmashTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmashTracker.Controllers
{
	public class BaseController : Controller
	{
		protected SiteLayout SiteLayout;

		public BaseController()
		{
			SiteLayout = new SiteLayout
			{
				
			};
		}
	}
}