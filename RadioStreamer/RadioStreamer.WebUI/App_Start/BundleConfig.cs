using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace RadioStreamer.WebUI
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{

			/******************Scripts*******************/

			bundles.Add(new ScriptBundle("~/script/jquery").Include(
						"~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/script/jqueryval").Include(
						"~/Scripts/jquery.validate*"));

			bundles.Add(new ScriptBundle("~/script/jquery.validate").Include(
						"~/Scripts/jquery.validate.unobtrusive.js"));

			bundles.Add(new ScriptBundle("~/script/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/script/bootstrap").Include(
					  "~/Scripts/bootstrap.js",
					  "~/Scripts/respond.js"));

			bundles.Add(new ScriptBundle("~/script/player").Include(
					  "~/Scripts/Player/jquery.jplayer.min.js",
					 "~/Scripts/Player/star-rating.js",
					 "~/Scripts/Player/gui-components.js",
					 "~/Scripts/Player/set.player.js",
					 "~/Scripts/Player/marquee.js" ));

			/******************Stylesheets*******************/


			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap.css",
					  "~/Content/Site.css"));

			bundles.Add(new StyleBundle("~/Content/player").Include(
					  "~/Content/Player/jplayer.pink.flag.css",
					  "~/Content/Player/star-rating.css"));
		}
	}
}