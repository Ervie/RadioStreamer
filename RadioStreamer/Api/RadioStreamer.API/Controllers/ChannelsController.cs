using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RadioStreamer.API.Controllers
{
    [Route("api/[controller]")]
    public class ChannelController : Controller
    {
        // GET api/channels
        [HttpGet]
        public IEnumerable<string> Get()
        {
            // temporary hardcoded streams

            return new string[] { "http://radio.vgmradio.com:8040/stream",
                                     "http://195.150.20.4:8000/rmf_classic",
                                     "http://redir.atmcdn.pl/sc/o2/Eurozet/live/antyradio.livx?audio=5" };
        }

        // GET api/channels/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/channels
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/channels/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/channels/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
