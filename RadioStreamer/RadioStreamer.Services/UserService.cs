using RadioStreamer.Domain;
using RadioStreamer.Services.Base;
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
            context.User.Add(user);
            SaveChanges();
        }

        public User LogInUser(User user)
        {
            return context.User.Where(u => u.Login == user.Login && u.Password == user.Password).FirstOrDefault();        
        }

        public bool CheckIfUserExistByLogin(string userName)
        {
            return !context.User.Any(x => x.Login == userName);
        }
    }
    
}
