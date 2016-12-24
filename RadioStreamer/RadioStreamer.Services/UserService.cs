using RadioStreamer.Domain;
using RadioStreamer.Services.Base;
using RadioStreamer.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioStreamer.Services
{
    public class UserService: ContextRepository
    {
        public void RegisterUser(User user)
        {
            string salt = Encryption.generateSalt();
            string hashedPassword = Encryption.HashPassword(user.Password, salt);

            user.Password = hashedPassword;
            user.Salt = salt;

            context.User.Add(user);
            SaveChanges();
        }

        public User LogInUser(User user)
        {
            User matchingUsernname = context.User.Where(u => u.Login.Equals(user.Login)).FirstOrDefault();

            string userSalt = matchingUsernname.Salt;

            if (userSalt != null)
            {
                string hashedPassword = Encryption.HashPassword(user.Password, userSalt);

                if (matchingUsernname.Password.Equals(hashedPassword))
                    return matchingUsernname;
            }

            return null;
   
        }

        public bool IsOccupied(string userName, string email)
        {
            return context.User.Any(x => x.Login == userName || x.Email == email);
        }
    }
    
}
