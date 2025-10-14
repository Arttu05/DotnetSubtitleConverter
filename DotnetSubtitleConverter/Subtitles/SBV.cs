using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DotnetSubtitleConverter.Subtitles
{
	internal static class SBV
	{
		
		public static List<SubtitleData> GetSubtitleData(ref StreamReader reader)
		{
			
			List<SubtitleData> subtitleDataList = new List<SubtitleData>();

			while(reader.EndOfStream == false)
			{

				SubtitleData currentSubtitleData = new();

 				string? expectedTimestamp = reader.ReadLine();

				if (expectedTimestamp == null || expectedTimestamp == "\n" || expectedTimestamp == "")
				{
					continue;
				}

				currentSubtitleData = ReadTimestampString(expectedTimestamp);

				
			}


			throw new NotImplementedException();
			return subtitleDataList;
		}

		public static string GetConvertedString(List<SubtitleData> subtitleData)
		{
			throw new NotImplementedException();
			return "";
		}

		public static bool Check(ref StreamReader reader)
		{
			throw new NotImplementedException();
			return false;
		}



		internal static SubtitleData ReadTimestampString(string timestampString)
		{

			Match timeStampMatch = ValidateTimestampString(timestampString) ?? throw new InvalidSubtitleException("timestamp is not valid");

			SubtitleData subtitleData = new SubtitleData();

			int startHour;
			int endHour;
			int startMinute;
			int endMinute;
			int startSecond;
			int endSecond;
			int startMillisecond;
			int endMillisecond;

			if( int.TryParse(timeStampMatch.Groups[0].Value, out startHour) == false ||
				int.TryParse(timeStampMatch.Groups[1].Value, out startMinute) == false ||
				int.TryParse(timeStampMatch.Groups[2].Value, out startSecond) == false ||
				int.TryParse(timeStampMatch.Groups[3].Value, out startMillisecond) == false ||
				int.TryParse(timeStampMatch.Groups[4].Value, out endHour) == false ||
				int.TryParse(timeStampMatch.Groups[5].Value, out endMinute) == false ||
				int.TryParse(timeStampMatch.Groups[6].Value, out endSecond) == false ||
				int.TryParse(timeStampMatch.Groups[7].Value, out endMillisecond) == false) 
			{
				throw new InvalidSubtitleException("failed to parse timestamp");
			}


			int startTimestamp = CommonUtils.GetMillisFromTime(startHour,startMinute,startSecond,startMillisecond);
			int endTimestamp = CommonUtils.GetMillisFromTime(endHour,endMinute,endSecond,endMillisecond);

			subtitleData.startInMillis = startTimestamp;
			subtitleData.endInMillis = endTimestamp;

			return subtitleData;

		}

		internal static Match? ValidateTimestampString(string timeStampString)
		{
			string regexPattern = "^(\\d{1,2}):(\\d{2}):(\\d{2})\\.(\\d{3}),(\\d{1,2}):(\\d{2}):(\\d{2})\\.(\\d{3})$";

			Match regexMatch = Regex.Match(timeStampString, regexPattern);

			if (regexMatch.Success == false)
			{
				return null;
			}

			return regexMatch;
		}
	}
}
