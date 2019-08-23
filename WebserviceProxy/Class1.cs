using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace WebserviceProxy
{
	public class Class1 : IDisposable
	{
		private readonly HttpClient client;

		public Class1()
		{
			client = new HttpClient();
		}

		public async Task Test()
		{
			LogonInfo info = new LogonInfo { email = "morten@lindhart.com", password = "nahNotHere", language = "da-DK" };

			string token = await Login(info);

			client.DefaultRequestHeaders.Add("Session-Token", token);

			client.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
			HttpResponseMessage geoStatus = await client.GetAsync("https://tracker-api-ws.husqvarna.net/services/robot/162205377-162600101/geoStatus/");
			GeoStatus status = JsonConvert.DeserializeObject<GeoStatus>(await geoStatus.Content.ReadAsStringAsync());

			await Task.Delay(5000);

			HttpResponseMessage geoStatus2 = await client.GetAsync("https://tracker-api-ws.husqvarna.net/services/robot/162205377-162600101/geoStatus/");
			GeoStatus status2 = JsonConvert.DeserializeObject<GeoStatus>(await geoStatus2.Content.ReadAsStringAsync());


			GpsInfoList infos = new GpsInfoList(status.geoInfos.gpsInfo);
			infos.Combine(status2.geoInfos.gpsInfo);

		}

		private static DateTime FromEpoch(int ticks)
		{
			return new DateTime(1970, 1, 1).AddSeconds(ticks);
		}

		private async Task<string> Login(LogonInfo info)
		{
			int maxTries = 5;
			int tries = 0;
			string token = null;

			while (true)
			{
				tries++;

				try
				{
					HttpResponseMessage response = await TryLogin(info);
					token = response.Headers.FirstOrDefault(x => x.Key.Contains("Session-Token")).Value?.FirstOrDefault();
				}
				catch (Exception)
				{
					if (tries >= maxTries)
					{
						throw;
					}
				}

				if (!string.IsNullOrEmpty(token))
				{
					return token;
				}
				
				if (tries >= maxTries)
				{
					break;
				}
			}

			throw new LoginException("Unable to login - never recieved session token.");
		}

		private async Task<HttpResponseMessage> TryLogin(LogonInfo info)
		{
			string postBody = JsonConvert.SerializeObject(info);
			HttpResponseMessage response = await client.PostAsync("https://tracker-id-ws.husqvarna.net/imservice/rest/im/login", new StringContent(postBody, Encoding.UTF8, "application/json"));
			response.EnsureSuccessStatusCode();
			return response;
		}

		/// <summary>
		/// Serialize Person object to Json string
		/// </summary>
		/// <param name="objectToSerialize">Person object instance</param>
		/// <returns>return Json String</returns>
		public static string JsonSerializerasf<T>(T objectToSerialize)
		{
			if (objectToSerialize == null)
			{
				throw new ArgumentException("objectToSerialize must not be null");
			}

			DataContractJsonSerializer serializer = new DataContractJsonSerializer(objectToSerialize.GetType());
			using (MemoryStream ms = new MemoryStream())
			{
				serializer.WriteObject(ms, objectToSerialize);
				ms.Seek(0, SeekOrigin.Begin);
				using (StreamReader sr = new StreamReader(ms))
				{
					return sr.ReadToEnd();
				}
			}
		}

		public void Dispose()
		{
			client?.Dispose();
		}
	}
}
