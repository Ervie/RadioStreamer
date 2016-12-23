using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RadioStreamer.Services.Base;
using RadioStreamer.Domain;
using System.Data.Entity;

namespace RadioStreamer.Services
{
    public class FavRatingService : ContextRepository
    {

        public bool IsFavorite(string userName, string channelName)
        {
            User user = context.User.FirstOrDefault(x => x.Login.Equals(userName));
            Channel channel = context.Channel.FirstOrDefault(x => x.Name.Equals(channelName));

            if (channel != null && user != null)
            {
                Favourite fav = context.Favourite.FirstOrDefault(x => x.ChannelId.Equals(channel.Id) && x.UserId.Equals(user.Id));

                return (fav == null) ? false : true;
            }
            return false;
        }

        public Rating GetRating(string userName, string channelName)
        {
            User user = context.User.FirstOrDefault(x => x.Login.Equals(userName));
            Channel channel = context.Channel.FirstOrDefault(x => x.Name.Equals(channelName));

            if (channel != null && user != null)
                return context.Rating.FirstOrDefault(x => x.ChannelId.Equals(channel.Id) && x.UserId.Equals(user.Id));
            else
                return null;
        }

        public void SetRating(string userName, string channelName, int value)
        {
            Rating existingRating = GetRating(userName, channelName);

            // Update
            if (existingRating != null && value != 0)
            {
                existingRating.Value = value;
                context.Entry(existingRating).State = EntityState.Modified;
            }
            // Delete
            else if (existingRating != null)
            {
                context.Rating.Remove(existingRating);
            }
            // Add
            else
            {
                User user = context.User.FirstOrDefault(x => x.Login.Equals(userName));
                Channel channel = context.Channel.FirstOrDefault(x => x.Name.Equals(channelName));
                Rating newObject = new Rating()
                {
                    UserId = user.Id,
                    ChannelId = channel.Id,
                    Value = value
                };

                context.Rating.Add(newObject);
            }

            SaveChanges();
        }

        public void AddFavorite(string userName, string channelName)
        {
            User user = context.User.FirstOrDefault(x => x.Login.Equals(userName));
            Channel channel = context.Channel.FirstOrDefault(x => x.Name.Equals(channelName));

            if (channel != null && user != null)
            {
                Favourite existingFavorite = context.Favourite.FirstOrDefault(x => x.ChannelId.Equals(channel.Id) && x.UserId.Equals(user.Id));

                if (existingFavorite == null)
                {
                    Favourite newObject = new Favourite()
                    {
                        ChannelId = channel.Id,
                        UserId = user.Id
                    };

                    context.Favourite.Add(newObject);

                    SaveChanges();
                }
            }
        }

        public void DeleteFavourite(string userName, string channelName)
        {
            User user = context.User.FirstOrDefault(x => x.Login.Equals(userName));
            Channel channel = context.Channel.FirstOrDefault(x => x.Name.Equals(channelName));

            if (channel != null && user != null)
            {
                Favourite existingFavorite = context.Favourite.FirstOrDefault(x => x.ChannelId.Equals(channel.Id) && x.UserId.Equals(user.Id));

                if (existingFavorite != null)
                {
                    context.Favourite.Remove(existingFavorite);

                    context.SaveChanges();
                }
            }
        }

        public List<Favourite> GetAllFavourites(string userName)
        {
            User user = context.User.FirstOrDefault(x => x.Login.Equals(userName));

            if (user != null)
                return context.Favourite.ToList();
            else
                return null;
        }
    }
}
