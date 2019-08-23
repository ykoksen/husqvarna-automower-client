using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebserviceProxy
{
	public class GpsInfoList : IEnumerable<GpsInfo>
	{
		private List<GpsInfo> inner;

		public GpsInfoList()
		{
			inner = new List<GpsInfo>();
		}

		public GpsInfoList(IEnumerable<GpsInfo> infos)
		{
			inner = new List<GpsInfo>(infos);
		}

		public IEnumerator<GpsInfo> GetEnumerator()
		{
			return inner.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// The input should have been collected after the information in this list.
		/// </summary>
		/// <param name="infos"></param>
		public void Combine(IEnumerable<GpsInfo> infos)
		{
			List<GpsInfo> temp = new List<GpsInfo>(infos);

			int start = temp.Count > inner.Count ? 0 : inner.Count - temp.Count;

			int finding = 0;
			int i = start;
			for (; i < inner.Count; i++)
			{
				if (inner[i].Equals(temp[finding]))
				{
					finding++;
				}
				else if (finding > 0)
				{
					// after the loop one is added to i
					i = i - (finding);
					finding = 0;
				}

				if (finding == 4)
				{
					break;
				}
			}

			if (finding > 0)
			{
				int range = inner.Count - (i - finding + 1);
				inner.AddRange(temp.GetRange(range, temp.Count - range));
			}
			else
			{
				inner.AddRange(temp);
			}
		}
	}
}
