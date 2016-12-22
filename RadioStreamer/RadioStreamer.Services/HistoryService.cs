using RadioStreamer.Services.Base;
using RadioStreamer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioStreamer.Services
{
    public class HistoryService : ContextRepository
    {
        public void AddHistoryLog(string channelName, string userName, DateTime startDate, DateTime endDate)
        {
            int duration = (int)(endDate - startDate).TotalSeconds;

            User user = context.User.FirstOrDefault(x => x.Login.Equals(userName));
            Channel channel = context.Channel.FirstOrDefault(x => x.Name.Equals(channelName));

            if (channel != null && user != null)
            {
                History newObject = new History()
                {
                    UserId = user.Id,
                    ChannelId = channel.Id,
                    StartDate = startDate,
                    EndDate = endDate,
                    Duration = duration
                };

                context.History.Add(newObject);

                SaveChanges();
            }
        }
    }
}
