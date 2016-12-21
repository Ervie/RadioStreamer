using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RadioStreamer.Common.Exceptions
{
    public class DBConnectionException : Exception
    {

        public DBConnectionException()
            : base()
        {
        }

        public DBConnectionException(string message)
            : base("Problem occured during connection to database")
        {
        }

        public DBConnectionException(String message, Exception innerException)
            : base("Problem occured during connection to database", innerException)
        {
        }

        protected DBConnectionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}
