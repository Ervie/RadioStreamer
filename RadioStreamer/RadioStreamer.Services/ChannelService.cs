﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RadioStreamer.Services.Base;
using RadioStreamer.Domain;

namespace RadioStreamer.Services
{
    class ChannelService : ContextRepository
    {
        public Channel GetChannelByName(string channelName)
        {
            return context.Channel.FirstOrDefault(x => x.Name.Equals(channelName));
        }
    }
}