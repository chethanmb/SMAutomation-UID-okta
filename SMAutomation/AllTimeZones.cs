using System;
using System.Collections.Generic;

namespace SMAutomation
{
	// Token: 0x02000002 RID: 2
	internal class AllTimeZones
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public AllTimeZones()
		{
			this.timeZones = new Dictionary<string, string>();
			this.timeZones.Add("(UTC) Casablanca", "Morocco Standard Time");
			this.timeZones.Add("(UTC) Coordinated Universal Time", "UTC");
			this.timeZones.Add("(UTC) Dublin, Edinburgh, Lisbon, London", "GMT Standard Time");
			this.timeZones.Add("(UTC) Monrovia, Reykjavik", "Greenwich Standard Time");
			this.timeZones.Add("(UTC+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna", "W. Europe Standard Time");
			this.timeZones.Add("(UTC+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague", "Central Europe Standard Time");
			this.timeZones.Add("(UTC+01:00) Brussels, Copenhagen, Madrid, Paris", "Romance Standard Time");
			this.timeZones.Add("(UTC+01:00) Sarajevo, Skopje, Warsaw, Zagreb", "Central European Standard Time");
			this.timeZones.Add("(UTC+01:00) West Central Africa", "W. Central Africa Standard Time");
			this.timeZones.Add("(UTC+01:00) Windhoek", "Namibia Standard Time");
			this.timeZones.Add("(UTC+02:00) Amman", "Jordan Standard Time");
			this.timeZones.Add("(UTC+02:00) Athens, Bucharest", "GTB Standard Time");
			this.timeZones.Add("(UTC+02:00) Beirut", "Middle East Standard Time");
			this.timeZones.Add("(UTC+02:00) Cairo", "Egypt Standard Time");
			this.timeZones.Add("(UTC+02:00) Damascus", "Syria Standard Time");
			this.timeZones.Add("(UTC+02:00) E. Europe", "E. Europe Standard Time");
			this.timeZones.Add("(UTC+02:00) Harare, Pretoria", "South Africa Standard Time");
			this.timeZones.Add("(UTC+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius", "FLE Standard Time");
			this.timeZones.Add("(UTC+02:00) Istanbul", "Turkey Standard Time");
			this.timeZones.Add("(UTC+02:00) Jerusalem", "Israel Standard Time");
			this.timeZones.Add("(UTC+02:00) Kaliningrad (RTZ 1)", "Kaliningrad Standard Time");
			this.timeZones.Add("(UTC+02:00) Tripoli", "Libya Standard Time");
			this.timeZones.Add("(UTC+03:00) Baghdad", "Arabic Standard Time");
			this.timeZones.Add("(UTC+03:00) Kuwait, Riyadh", "Arab Standard Time");
			this.timeZones.Add("(UTC+03:00) Minsk", "Belarus Standard Time");
			this.timeZones.Add("(UTC+03:00) Moscow, St. Petersburg, Volgograd (RTZ 2)", "Russian Standard Time");
			this.timeZones.Add("(UTC+03:00) Nairobi", "E. Africa Standard Time");
			this.timeZones.Add("(UTC+03:30) Tehran", "Iran Standard Time");
			this.timeZones.Add("(UTC+04:00) Abu Dhabi, Muscat", "Arabian Standard Time");
			this.timeZones.Add("(UTC+04:00) Baku", "Azerbaijan Standard Time");
			this.timeZones.Add("(UTC+04:00) Izhevsk, Samara (RTZ 3)", "Russia Time Zone 3");
			this.timeZones.Add("(UTC+04:00) Port Louis", "Mauritius Standard Time");
			this.timeZones.Add("(UTC+04:00) Tbilisi", "Georgian Standard Time");
			this.timeZones.Add("(UTC+04:00) Yerevan", "Caucasus Standard Time");
			this.timeZones.Add("(UTC+04:30) Kabul", "Afghanistan Standard Time");
			this.timeZones.Add("(UTC+05:00) Ashgabat, Tashkent", "West Asia Standard Time");
			this.timeZones.Add("(UTC+05:00) Ekaterinburg (RTZ 4)", "Ekaterinburg Standard Time");
			this.timeZones.Add("(UTC+05:00) Islamabad, Karachi", "Pakistan Standard Time");
			this.timeZones.Add("(UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi", "India Standard Time");
			this.timeZones.Add("(UTC+05:30) Sri Jayawardenepura", "Sri Lanka Standard Time");
			this.timeZones.Add("(UTC+05:45) Kathmandu", "Nepal Standard Time");
			this.timeZones.Add("(UTC+06:00) Astana", "Central Asia Standard Time");
			this.timeZones.Add("(UTC+06:00) Dhaka", "Bangladesh Standard Time");
			this.timeZones.Add("(UTC+06:00) Novosibirsk (RTZ 5)", "N. Central Asia Standard Time");
			this.timeZones.Add("(UTC+06:30) Yangon (Rangoon)", "Myanmar Standard Time");
			this.timeZones.Add("(UTC+07:00) Bangkok, Hanoi, Jakarta", "SE Asia Standard Time");
			this.timeZones.Add("(UTC+07:00) Krasnoyarsk (RTZ 6)", "North Asia Standard Time");
			this.timeZones.Add("(UTC+08:00) Beijing, Chongqing, Hong Kong, Urumqi", "China Standard Time");
			this.timeZones.Add("(UTC+08:00) Irkutsk (RTZ 7)", "North Asia East Standard Time");
			this.timeZones.Add("(UTC+08:00) Kuala Lumpur, Singapore", "Singapore Standard Time");
			this.timeZones.Add("(UTC+08:00) Perth", "W. Australia Standard Time");
			this.timeZones.Add("(UTC+08:00) Taipei", "Taipei Standard Time");
			this.timeZones.Add("(UTC+08:00) Ulaanbaatar", "Ulaanbaatar Standard Time");
			this.timeZones.Add("(UTC+09:00) Osaka, Sapporo, Tokyo", "Tokyo Standard Time");
			this.timeZones.Add("(UTC+09:00) Seoul", "Korea Standard Time");
			this.timeZones.Add("(UTC+09:00) Yakutsk (RTZ 8)", "Yakutsk Standard Time");
			this.timeZones.Add("(UTC+09:30) Adelaide", "Cen. Australia Standard Time");
			this.timeZones.Add("(UTC+09:30) Darwin", "AUS Central Standard Time");
			this.timeZones.Add("(UTC+10:00) Brisbane", "E. Australia Standard Time");
			this.timeZones.Add("(UTC+10:00) Canberra, Melbourne, Sydney", "AUS Eastern Standard Time");
			this.timeZones.Add("(UTC+10:00) Guam, Port Moresby", "West Pacific Standard Time");
			this.timeZones.Add("(UTC+10:00) Hobart", "Tasmania Standard Time");
			this.timeZones.Add("(UTC+10:00) Magadan", "Magadan Standard Time");
			this.timeZones.Add("(UTC+10:00) Vladivostok, Magadan (RTZ 9)", "Vladivostok Standard Time");
			this.timeZones.Add("(UTC+11:00) Chokurdakh (RTZ 10)", "Russia Time Zone 10");
			this.timeZones.Add("(UTC+11:00) Solomon Is., New Caledonia", "Central Pacific Standard Time");
			this.timeZones.Add("(UTC+12:00) Anadyr, Petropavlovsk-Kamchatsky (RTZ 11)", "Russia Time Zone 11");
			this.timeZones.Add("(UTC+12:00) Auckland, Wellington", "New Zealand Standard Time");
			this.timeZones.Add("(UTC+12:00) Coordinated Universal Time+12", "UTC+12");
			this.timeZones.Add("(UTC+12:00) Fiji", "Fiji Standard Time");
			this.timeZones.Add("(UTC+12:00) Petropavlovsk-Kamchatsky - Old", "Kamchatka Standard Time");
			this.timeZones.Add("(UTC+13:00) Nuku'alofa", "Tonga Standard Time");
			this.timeZones.Add("(UTC+13:00) Samoa", "Samoa Standard Time");
			this.timeZones.Add("(UTC+14:00) Kiritimati Island", "Line Islands Standard Time");
			this.timeZones.Add("(UTC-01:00) Azores", "Azores Standard Time");
			this.timeZones.Add("(UTC-01:00) Cabo Verde Is.", "Cape Verde Standard Time");
			this.timeZones.Add("(UTC-02:00) Coordinated Universal Time-02", "UTC-02");
			this.timeZones.Add("(UTC-02:00) Mid-Atlantic - Old", "Mid-Atlantic Standard Time");
			this.timeZones.Add("(UTC-03:00) Brasilia", "E. South America Standard Time");
			this.timeZones.Add("(UTC-03:00) Buenos Aires", "Argentina Standard Time");
			this.timeZones.Add("(UTC-03:00) Cayenne, Fortaleza", "SA Eastern Standard Time");
			this.timeZones.Add("(UTC-03:00) Greenland", "Greenland Standard Time");
			this.timeZones.Add("(UTC-03:00) Montevideo", "Montevideo Standard Time");
			this.timeZones.Add("(UTC-03:00) Salvador", "Bahia Standard Time");
			this.timeZones.Add("(UTC-03:30) Newfoundland", "Newfoundland Standard Time");
			this.timeZones.Add("(UTC-04:00) Asuncion", "Paraguay Standard Time");
			this.timeZones.Add("(UTC-04:00) Atlantic Time (Canada)", "Atlantic Standard Time");
			this.timeZones.Add("(UTC-04:00) Cuiaba", "Central Brazilian Standard Time");
			this.timeZones.Add("(UTC-04:00) Georgetown, La Paz, Manaus, San Juan", "SA Western Standard Time");
			this.timeZones.Add("(UTC-04:00) Santiago", "Pacific SA Standard Time");
			this.timeZones.Add("(UTC-04:30) Caracas", "Venezuela Standard Time");
			this.timeZones.Add("(UTC-05:00) Bogota, Lima, Quito, Rio Branco", "SA Pacific Standard Time");
			this.timeZones.Add("(UTC-05:00) Eastern Time (US & Canada)", "Eastern Standard Time");
			this.timeZones.Add("(UTC-05:00) Indiana (East)", "US Eastern Standard Time");
			this.timeZones.Add("(UTC-06:00) Central America", "Central America Standard Time");
			this.timeZones.Add("(UTC-06:00) Central Time (US & Canada)", "Central Standard Time");
			this.timeZones.Add("(UTC-06:00) Guadalajara, Mexico City, Monterrey", "Central Standard Time (Mexico)");
			this.timeZones.Add("(UTC-06:00) Saskatchewan", "Canada Central Standard Time");
			this.timeZones.Add("(UTC-07:00) Arizona", "US Mountain Standard Time");
			this.timeZones.Add("(UTC-07:00) Chihuahua, La Paz, Mazatlan", "Mountain Standard Time (Mexico)");
			this.timeZones.Add("(UTC-07:00) Mountain Time (US & Canada)", "Mountain Standard Time");
			this.timeZones.Add("(UTC-08:00) Baja California", "Pacific Standard Time (Mexico)");
			this.timeZones.Add("(UTC-08:00) Pacific Time (US & Canada)", "Pacific Standard Time");
			this.timeZones.Add("(UTC-09:00) Alaska", "Alaskan Standard Time");
			this.timeZones.Add("(UTC-10:00) Hawaii", "Hawaiian Standard Time");
			this.timeZones.Add("(UTC-11:00) Coordinated Universal Time-11", "UTC-11");
			this.timeZones.Add("(UTC-12:00) International Date Line West", "Dateline Standard Time");
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000029A4 File Offset: 0x00000BA4
		public string getTimeZoneByName(string zoneName)
		{
			bool flag = this.timeZones.ContainsKey(zoneName);
			string result;
			if (flag)
			{
				result = this.timeZones[zoneName];
			}
			else
			{
				result = "No Such Time zone";
			}
			return result;
		}

		// Token: 0x04000001 RID: 1
		private Dictionary<string, string> timeZones;
	}
}
