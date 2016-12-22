using RadioStreamer.Domain;
using RadioStreamer.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RadioStreamer.Services
{
	public class ChannelService : ContextRepository
	{
		public Channel GetChannelByName(string channelName)
		{
			return context.Channel.FirstOrDefault(x => x.Name.Equals(channelName));
		}

		public List<Channel> GetAllChannels()
		{
			return context.Channel.ToList();
		}

		public Channel GetRandomChannel()
		{
			// Random record
			return context.Channel.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
		}
	}
}