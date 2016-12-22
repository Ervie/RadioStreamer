using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using RadioStreamer.Domain;
using RadioStreamer.Services;
using RadioStreamer.Common.Exceptions;
using RadioStreamer.Common.Structs;

namespace RadioStreamer.WebUI.Controllers
{
    public class HomeController : Controller
    {
		#region Main Views

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult About()
		{
			return View();
		}

		#endregion

		#region Ajax requests

		public JsonResult GetChannelList()
		{
			List<Channel> returnedChannels;

			using (ChannelService db = new ChannelService())
			{
				returnedChannels = db.GetAllChannels();
			}

			if (returnedChannels != null)
			{
				List<string> channelNames = returnedChannels.Select(x => x.Name).ToList();
				channelNames.Sort();
				string output = new JavaScriptSerializer().Serialize(channelNames);
				return Json(output, JsonRequestBehavior.AllowGet);
			}
			else
				return Json(string.Empty);
		}

		public JsonResult GetChannel()
		{
			if (string.IsNullOrEmpty(Request.QueryString["channelName"]))
				throw new InvalidAjaxRequestException();
			else
			{
				using (ChannelService db = new ChannelService())
				{
					string requestedChannelName = Request.QueryString["channelName"];

					Channel returnedChannel = db.GetChannelByName(requestedChannelName);

					if (returnedChannel != null)
					{
						string imgSrc = "Images/Icons/300px/" + returnedChannel.Name + ".png";
						ChannelInfo channelInfo = new ChannelInfo()
						{
							Name = returnedChannel.Name,
							StreamUrl = returnedChannel.StreamUrl,
							ImagePath = imgSrc
						};
						return Json(channelInfo, JsonRequestBehavior.AllowGet);
					}
					else
						return Json(string.Empty);


				}
			}
		}

		#endregion
	}
}