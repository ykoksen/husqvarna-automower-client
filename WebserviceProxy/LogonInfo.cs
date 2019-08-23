using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebserviceProxy
{
	[DataContract]
	public class LogonInfo
	{
		[DataMember]
		public string email { get; set; }

		[DataMember]
		public string password { get; set; }

		[DataMember]
		public string language { get; set; }
	}


	public partial class GpsInfo : IEquatable<GpsInfo>
	{
		public double latitude { get; set; }
		public double longitude { get; set; }
		public string gpsStatus { get; set; }
		public long timestamp { get; set; }

		public DateTime Timpestamp
		{
			get
			{
				return ProxyConverters.FromEpoch(timestamp).ToLocalTime(); 
			}
		}

		public override bool Equals(object otherObj)
		{
			GpsInfo other = otherObj as GpsInfo;
			return Equals(other);
		}

		public bool Equals(GpsInfo other)
		{
			if (other == null)
			{
				return false;
			}

			return latitude == other.latitude &&
				longitude == other.longitude &&
				gpsStatus == other.gpsStatus;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17 + latitude.GetHashCode();
				hash = hash * 23 + longitude.GetHashCode();
				hash = hash * 23 + gpsStatus.GetHashCode();
				return hash;
			} 
		}
	}

	public class GeoInfos
	{
		public List<GpsInfo> gpsInfo { get; set; }
	}

	public class GeofenceCentralPoint
	{
		public long settingTimestamp { get; set; }
		public double latitude { get; set; }
		public double longitude { get; set; }
		public int altitude { get; set; }
	}

	public class GeofenceSensitivitySettings
	{
		public long settingTimestamp { get; set; }
		public int geofenceSensitivityLevel { get; set; }
		public int geofenceRadius { get; set; }
		public int timeOutsideGeofenceArea { get; set; }
	}

	public class GeoStatus
	{
		public GeoInfos geoInfos { get; set; }
		public GeofenceCentralPoint geofenceCentralPoint { get; set; }
		public GeofenceSensitivitySettings geofenceSensitivitySettings { get; set; }
	}

}
