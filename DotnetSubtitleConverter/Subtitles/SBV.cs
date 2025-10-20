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
				currentSubtitleData.subtitleContent = GetSubtitleContent(ref reader);

				subtitleDataList.Add(currentSubtitleData);
			}

			return subtitleDataList;
		}

		public static string GetConvertedString(List<SubtitleData> subtitleData)
		{
			string outputString = "";


			foreach (SubtitleData currentData in subtitleData) 
			{
				outputString += $"{GetTimestampString(currentData)}\n";
				outputString += $"{currentData.subtitleContent}\n\n";
			}


			if(outputString == "")
			{
				throw new SubtitleWritingException();
			}

			return outputString;
		}

		public static bool Check(ref StreamReader reader)
		{

			try
			{
				List<SubtitleData> subtitleDataList = new List<SubtitleData>();

				while (reader.EndOfStream == false)
				{
					SubtitleData currentSubtitleData = new();

					string? expectedTimestamp = reader.ReadLine();

					if (expectedTimestamp == null || expectedTimestamp == "\n" || expectedTimestamp == "")
					{
						continue;
					}

					currentSubtitleData = ReadTimestampString(expectedTimestamp);
					currentSubtitleData.subtitleContent = GetSubtitleContent(ref reader);

					subtitleDataList.Add(currentSubtitleData);
				}

				if (subtitleDataList.Count <= 0) 
				{ 
					return false;
				}

				return true;
			}
			catch 
			{ 
				return false;
			}
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

			if( int.TryParse(timeStampMatch.Groups[1].Value, out startHour) == false ||
				int.TryParse(timeStampMatch.Groups[2].Value, out startMinute) == false ||
				int.TryParse(timeStampMatch.Groups[3].Value, out startSecond) == false ||
				int.TryParse(timeStampMatch.Groups[4].Value, out startMillisecond) == false ||
				int.TryParse(timeStampMatch.Groups[5].Value, out endHour) == false ||
				int.TryParse(timeStampMatch.Groups[6].Value, out endMinute) == false ||
				int.TryParse(timeStampMatch.Groups[7].Value, out endSecond) == false ||
				int.TryParse(timeStampMatch.Groups[8].Value, out endMillisecond) == false) 
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

		internal static string GetSubtitleContent(ref StreamReader reader) 
		{ 
			
			string firstLine = reader.ReadLine() ?? throw new InvalidSubtitleException("found null, instead of dialog");

			if (firstLine == "" || firstLine == "\n")
			{
				throw new InvalidSubtitleException("found empty line, instead of dialog");
			}

			string returnValue = $"{firstLine}";


			string? possibleLine;
			possibleLine = reader.ReadLine();
			while( possibleLine != "" && possibleLine != null)
			{
				returnValue += $"\n{possibleLine}";
				possibleLine = reader.ReadLine();
			}


			return returnValue;
		}

		internal static string GetTimestampString(SubtitleData subtitleData) 
		{
			int startMillisAfterDivide = subtitleData.startInMillis;

			int startHour = CommonUtils.GetIntFromDividedInt(startMillisAfterDivide, CommonUtils.hourInMillis);
			startMillisAfterDivide -= (startHour * CommonUtils.hourInMillis);

			int startMinute = CommonUtils.GetIntFromDividedInt(startMillisAfterDivide, CommonUtils.MinInMillis);
			startMillisAfterDivide -= (startMinute * CommonUtils.MinInMillis);

			int startSecond = CommonUtils.GetIntFromDividedInt(startMillisAfterDivide, CommonUtils.SecInMillis);
			startMillisAfterDivide -= startSecond * CommonUtils.SecInMillis;

			string outputString = "";
			// start timestamp
			outputString += startHour;
			outputString += ":";
			outputString += CommonUtils.GetTwoDigitStringFromInt(startMinute);
			outputString += ":";
			outputString += CommonUtils.GetTwoDigitStringFromInt(startSecond);
			outputString += ".";
			outputString += CommonUtils.GetThreeDigitStringFromInt(startMillisAfterDivide);

			// "arrow"
			outputString += ",";

			int endMillisAfterDivide = subtitleData.endInMillis;

			int endHour = CommonUtils.GetIntFromDividedInt(endMillisAfterDivide, CommonUtils.hourInMillis);
			endMillisAfterDivide -= (endHour * CommonUtils.hourInMillis);

			int endMinute = CommonUtils.GetIntFromDividedInt(endMillisAfterDivide, CommonUtils.MinInMillis);
			endMillisAfterDivide -= (endMinute * CommonUtils.MinInMillis);

			int endSecond = CommonUtils.GetIntFromDividedInt(endMillisAfterDivide, CommonUtils.SecInMillis);
			endMillisAfterDivide -= endSecond * CommonUtils.SecInMillis;

			//end timestamp
			outputString += endHour;
			outputString += ":";
			outputString += CommonUtils.GetTwoDigitStringFromInt(endMinute);
			outputString += ":";
			outputString += CommonUtils.GetTwoDigitStringFromInt(endSecond);
			outputString += ".";
			outputString += CommonUtils.GetThreeDigitStringFromInt(endMillisAfterDivide);

			return outputString;
		}
	}
}
