using RadioStreamer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RadioStreamer.WebAPI.Controllers
{
    public class HomeController : ApiController
    {
		private readonly ChannelService service = new ChannelService();

		[HttpGet]
		[Route("api/Channels")]
		public IHttpActionResult GetChannels()
		{
			var result = service.GetAllChannels().FirstOrDefault().Name;
			return Ok(result);
		}
	}
}
