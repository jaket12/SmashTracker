using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmashTracker.Controllers
{
	public class HomeController : BaseController
	{
		public ActionResult Index()
		{
			return View(SiteLayout);
		}
		
		public ActionResult NewMatch()
		{
			return View(SiteLayout);
		}

		public ActionResult NewPlayer()
		{
			return View(SiteLayout);
		}

		public ActionResult BeltMatch()
		{
			return View(SiteLayout);
		}
		
	}
}