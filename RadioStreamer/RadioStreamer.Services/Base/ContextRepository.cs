using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RadioStreamer.Domain;
using RadioStreamer.Common.Exceptions;

namespace RadioStreamer.Services.Base
{
    abstract public class ContextRepository : IDisposable
    {
        protected RadioStreamerDBEntities context = new RadioStreamerDBEntities();

        private bool alreadyReconnected;

        protected void Initialize()
        {
            alreadyReconnected = false;
        }

        protected void SaveChanges()
        {
            if (context != null)
                context.SaveChanges();
        }

        public void Dispose()
        {
            if (context != null)
                context.Dispose();
        }

        /// <summary>
        /// Method that try connectiong to database. Can retry if user is willing to.
        /// </summary>
        public void connectToDB()
        {
            try
            {
            }

            catch (Exception)
            {
                if (alreadyReconnected)
                    throw new DBConnectionException();
                else
                {
                    alreadyReconnected = true;
                    connectToDB();
                }
            }

        }
    }
    
}
