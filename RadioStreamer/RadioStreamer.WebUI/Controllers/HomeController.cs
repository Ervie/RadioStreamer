using RadioStreamer.Common.Exceptions;
using RadioStreamer.Common.Structs;
using RadioStreamer.Common.Utils;
using RadioStreamer.Domain;
using RadioStreamer.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace RadioStreamer.WebUI.Controllers
{
    [Authorize]
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

        public JsonResult GetSuggestions()
        {
            using (ChannelService db = new ChannelService())
			{

                Suggestions suggestions = new Suggestions()
						{
							FirstChannelName = "RMF FM Classic",
                            FirstChannelUrl = "http://195.150.20.243:8000/rmf_classic",
                            SecondChannelName = "Gensokyo Radio",
                            SecondChannelUrl = "http://stream.gensokyoradio.net:8000/stream/1/",
                            ThirdChannelName = "VGM Radio",
                            ThirdChannelUrl = "http://radio.vgmradio.com:8040/stream"
						};

				return Json(suggestions, JsonRequestBehavior.AllowGet);
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

            ViewBag.firstImagePathSmall = "Images/Icons/120px/" + Request.QueryString["firstChannelName"] + "120.png";
            ViewBag.secondImagePathSmall = "Images/Icons/120px/" + Request.QueryString["secondChannelName"] + "120.png";
            ViewBag.thirdImagePathSmall = "Images/Icons/120px/" + Request.QueryString["thirdChannelName"] + "120.png";

			return PartialView("~/Views/Shared/sidebarPartial.cshtml");	
        }

        public ActionResult AdditionalInfo()
        {
            if (Request.HttpMethod == "POST")
            {
                if (!string.IsNullOrEmpty(Request.Form["currentChannelName"]))
                {
                    double adjustedRating = double.Parse(Request.Form["value"], CultureInfo.InvariantCulture) * 2;

                    using (FavRatingService db = new FavRatingService())
                    {
                        db.SetRating("Forczu", Request.Form["currentChannelName"], (int)adjustedRating);
                    }

                    return Content("");
                }
                else
                    throw new InvalidAjaxRequestException();
            }
            else if (Request.HttpMethod == "GET")
            {
                using (FavRatingService db = new FavRatingService())
                {
                    Rating requestedRating = db.GetRating("Forczu", Request.QueryString["currentChannelName"]);
                    bool isFavorite = db.IsFavorite("Forczu", Request.QueryString["currentChannelName"]);

                    AdditionalInfo info = new AdditionalInfo()
                    {
                        IsFavorite = isFavorite,
                        Value = (requestedRating != null) ? requestedRating.Value / 2.0 : 0.0
                    };

                    return Json(info, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Content("");
        }

        public ActionResult GetFavouriteList()
        {
            List<Favourite> returnedFavorites;

            using (FavRatingService db = new FavRatingService())
            {
                if (Request.HttpMethod == "POST")
                {
                    if (Request.Form["currentChannelName"] != null)
                    {
                        if (Request.Form["operation"] == "Add")
                        {
                            db.AddFavorite("Forczu", Request.Form["currentChannelName"]);
                        }
                        else if (Request.Form["operation"] == "Delete")
                        {
                            db.DeleteFavourite("Forczu", Request.Form["currentChannelName"]);
                        }
                    }
                }

                returnedFavorites = db.GetAllFavourites("Forczu");
                

                if (returnedFavorites != null)
                {
                    List<string> channelNames = returnedFavorites.Select(x => x.Channel.Name).ToList();
                    channelNames.Sort();
                    string output = new JavaScriptSerializer().Serialize(channelNames);
                    return Json(output, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(string.Empty);

            }

        }

		#endregion Partial Rendering

        #region Other

        public ContentResult LogTime()
        {
            if (!string.IsNullOrEmpty(Request.Form["currentChannelName"]))
            {
                using (HistoryService db = new HistoryService())
                {
                    string channelName = Request.Form["currentChannelName"];
                    DateTime start = DateTime.Parse(Request.Form["startTimestamp"], null, System.Globalization.DateTimeStyles.RoundtripKind);
                    DateTime end = DateTime.Parse(Request.Form["endTimestamp"], null, System.Globalization.DateTimeStyles.RoundtripKind);

                    db.AddHistoryLog(channelName, "Forczu", start, end);
                }
            }

            return Content("");
        }

        #endregion
    }
}