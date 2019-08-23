using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebserviceProxy
{
	internal static class ProxyConverters
	{
		public static DateTime FromEpoch(long ticks)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(ticks);
		}
	}
}
