using System.Web;
using System.Web.Optimization;

namespace SmashTracker
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{			
			bundles.Add(new StyleBundle("~/CSS/FrontEnd").Include(
					  "~/CSS/font-awesome.min.css",
					  "~/CSS/Styles.css"));

			bundles.Add(new StyleBundle("~/JS/FrontEnd").Include(
					  "~/Scripts/jquery-{version}.js",
					  "~/Scripts/jquery.validate*",
					  "~/Scripts/modernizr-*",
					  "~/JS/angular.min.js",
					  "~/JS/Site.js"));
		}
	}
}
