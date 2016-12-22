using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioStreamer.Common.Structs
{
	public struct ChannelInfo
	{
		public string Name { get; set; }
		public string StreamUrl { get; set; }

		public string ImagePath { get; set; }
	}
}
