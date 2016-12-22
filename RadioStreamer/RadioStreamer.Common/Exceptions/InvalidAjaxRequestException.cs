using System;
using System.Runtime.Serialization;

namespace RadioStreamer.Common.Exceptions
{
	public class InvalidAjaxRequestException : Exception
	{
		public InvalidAjaxRequestException()
			: base()
		{
		}

		public InvalidAjaxRequestException(string message)
			: base("Missing ajax parameter in httpRequest")
		{
		}

		public InvalidAjaxRequestException(String message, Exception innerException)
			: base("Missing ajax parameter in httpRequest", innerException)
		{
		}

		protected InvalidAjaxRequestException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}