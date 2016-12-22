using RadioStreamer.Common.Exceptions;
using RadioStreamer.Common.Structs;
using RadioStreamer.Common.Utils;
using RadioStreamer.Domain;
using RadioStreamer.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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

		#endregion Main Views

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

		public JsonResult GetRandomChannel()
		{
			using (ChannelService db = new ChannelService())
			{
				Channel returnedChannel = db.GetRandomChannel();

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

		#endregion Ajax requests

		#region Partial Rendering

		public ActionResult Metadata()
		{
		
			string channelUrl = Request.QueryString["currentChannelUrl"];

			if (!string.IsNullOrWhiteSpace(channelUrl))
			{
				var metadata = MetadataWorker.SendRequest(channelUrl);
				ViewBag.Metadata = metadata;
			}
			return PartialView("~/Views/Shared/metadataPartial.cshtml");
				
		}

		public ActionResult Sidebar()
		{
			using (ChannelService db = new ChannelService())
			{
				ViewBag.firstImagePath = "Images/Icons/300px/RMF FM Classic.png";
				ViewBag.firstImagePathSmall = "Images/Icons/120px/RMF FM Classic120.png";
				ViewBag.firstChannelName = "RMF FM Classic";
				ViewBag.firstChannelUrl = "http://195.150.20.243:8000/rmf_classic";
                ViewBag.secondImagePath = "IImages/Icons/300px/Gensokyo Radio.png";
				ViewBag.secondImagePathSmall = "Images/Icons/120px/Gensokyo Radio120.png";
				ViewBag.secondChannelName = "Gensokyo Radio";
				ViewBag.secondChannelUrl = "http://stream.gensokyoradio.net:8000/stream/1/";
				ViewBag.thirdImagePath = "Images/Icons/300px/VGM Radio.png";
				ViewBag.thirdImagePathSmall = "Images/Icons/120px/VGM Radio120.png";
				ViewBag.thirdChannelName = "VGM Radio";
				ViewBag.thirdChannelUrl = "http://radio.vgmradio.com:8040/stream";
				
				return PartialView("~/Views/Shared/sidebarPartial.cshtml");
			}
			
		}

		#endregion Partial Rendering
	}
}