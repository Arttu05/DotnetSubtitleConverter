using System.Text.RegularExpressions;

namespace DotnetSubtitleConverter.Subtitles
{
    internal static class VTT
    {
        public static List<SubtitleData> GetSubtitleData(ref StreamReader reader)
        {
			List<SubtitleData> outputList = new List<SubtitleData>();

			reader.ReadLine(); // TODO validate WEBVTT string
			reader.ReadLine();

			while (reader.EndOfStream == false)
			{
				SubtitleData currentSubtitleData = new SubtitleData();

				// start and end timestamps

				string? expectedTimeString = reader.ReadLine();

				int[] timeArray;

				try
				{
					timeArray = ReadTimeString(expectedTimeString);
				}
				catch (InvalidSubtitleException)
				{
					continue;
				}

				SetTimeArrayToClass(timeArray, ref currentSubtitleData);

				//reading subtitle content
				string subtitleContent = GetSubtitleContent(ref reader);
				currentSubtitleData.subtitleContent = subtitleContent;

				outputList.Add(currentSubtitleData);
			}

			return outputList;
		}

        public static string GetConvertedString(List<SubtitleData> subtitleDataArray)
        {
            string convertedString = "";

            convertedString += "WEBVTT\n\n";

			for (int i = 0; i < subtitleDataArray.Count; i++)
            {
				convertedString += GetTimeString(subtitleDataArray[i]);
				convertedString += "\n";
				convertedString += subtitleDataArray[i].subtitleContent;
				if ((i + 1) < subtitleDataArray.Count)
				{
					convertedString += "\n";
					convertedString += "\n";
				}
			}

            return convertedString;
        }

		public static bool Check(ref StreamReader reader)
		{
			string webttvLine = reader.ReadLine();
			if (webttvLine == null)
			{
				return false;
			} 

			if(webttvLine != "WEBVTT")
			{
				return false;
			}

			List<SubtitleData> tempDataList = new();

			while(reader.EndOfStream == false)
			{
				try
				{
					string? expectedTimeString = reader.ReadLine();

					if(expectedTimeString == null || expectedTimeString == "" || expectedTimeString == "\n") 
					{
						continue;	
					}

					int[] tempArr;

					try
					{
						tempArr = ReadTimeString(expectedTimeString);
					}
					catch (InvalidSubtitleException)
					{
						continue;
					}

					

					SubtitleData tempClass = new();
					SetTimeArrayToClass(tempArr, ref tempClass);

					tempClass.subtitleContent = GetSubtitleContent(ref reader);

					tempDataList.Add(tempClass);
				}
				catch (Exception e)
				{
					return false;
				}

			}

			if(tempDataList.Count <= 0)
			{
				return false;
			}

			return true;
		}


        //example output "00:00:00,000 --> 00:00:10,210"
        internal static string GetTimeString(SubtitleData subtitleData)
		{
			int startMillisAfterDivide = subtitleData.startInMillis;

			int startHour = CommonUtils.GetIntFromDividedInt(startMillisAfterDivide, CommonUtils.hourInMillis);
			startMillisAfterDivide -= (startHour * CommonUtils.hourInMillis);

			int startMinute = CommonUtils.GetIntFromDividedInt(startMillisAfterDivide, CommonUtils.MinInMillis);
			startMillisAfterDivide -= (startMinute * CommonUtils.MinInMillis);

			int startSecond = CommonUtils.GetIntFromDividedInt(startMillisAfterDivide, CommonUtils.SecInMillis);
			startMillisAfterDivide -= startSecond * CommonUtils.SecInMillis;

			// https://www.w3.org/TR/webvtt1/#file-structure
			string outputString = "";
			// start timestamp
			outputString += CommonUtils.GetTwoDigitStringFromInt(startHour);
			outputString += ":";
			outputString += CommonUtils.GetTwoDigitStringFromInt(startMinute);
			outputString += ":";
			outputString += CommonUtils.GetTwoDigitStringFromInt(startSecond);
			outputString += ".";
			outputString += CommonUtils.GetThreeDigitStringFromInt(startMillisAfterDivide);

			// "arrow"
			outputString += " --> ";

			int endMillisAfterDivide = subtitleData.endInMillis;

			int endHour = CommonUtils.GetIntFromDividedInt(endMillisAfterDivide, CommonUtils.hourInMillis);
			endMillisAfterDivide -= (endHour * CommonUtils.hourInMillis);

			int endMinute = CommonUtils.GetIntFromDividedInt(endMillisAfterDivide, CommonUtils.MinInMillis);
			endMillisAfterDivide -= (endMinute * CommonUtils.MinInMillis);

			int endSecond = CommonUtils.GetIntFromDividedInt(endMillisAfterDivide, CommonUtils.SecInMillis);
			endMillisAfterDivide -= endSecond * CommonUtils.SecInMillis;

			//end timestamp
			outputString += CommonUtils.GetTwoDigitStringFromInt(endHour);
			outputString += ":";
			outputString += CommonUtils.GetTwoDigitStringFromInt(endMinute);
			outputString += ":";
			outputString += CommonUtils.GetTwoDigitStringFromInt(endSecond);
			outputString += ".";
			outputString += CommonUtils.GetThreeDigitStringFromInt(endMillisAfterDivide);

			return outputString;
		}

		internal static int[] ReadTimeString(string? rawTimeString)
		{
			string regexPattern = "^\\d{2}:\\d{2}:\\d{2}.\\d{3} --> \\d{2}:\\d{2}:\\d{2}.\\d{3}";
			if (rawTimeString == null)
			{
				throw new InvalidSubtitleException("found null, expected timestamp");
			}
			if (Regex.Match(rawTimeString, regexPattern).Success == false)
			{
				throw new InvalidSubtitleException("VTT timestamp is incorrect");
			}

			return GetTimeArray(rawTimeString);

		}

		internal static int[] GetTimeArray(string validTimeString) // should ONLY be used with valitated string
		{
			int[] timeArray = new int[8];

			timeArray[0] = Convert.ToInt32(CombineChars([validTimeString[0], validTimeString[1]])); // start H
			timeArray[1] = Convert.ToInt32(CombineChars([validTimeString[3], validTimeString[4]])); // start M
			timeArray[2] = Convert.ToInt32(CombineChars([validTimeString[6], validTimeString[7]])); // start S
			timeArray[3] = Convert.ToInt32(CombineChars([validTimeString[9], validTimeString[10], validTimeString[11]])); // start MS

			timeArray[4] = Convert.ToInt32(CombineChars([validTimeString[17], validTimeString[18]])); // end H
			timeArray[5] = Convert.ToInt32(CombineChars([validTimeString[20], validTimeString[21]])); // end M
			timeArray[6] = Convert.ToInt32(CombineChars([validTimeString[23], validTimeString[24]])); // end S
			timeArray[7] = Convert.ToInt32(CombineChars([validTimeString[26], validTimeString[27], validTimeString[28]])); // end MS


			return timeArray;
		}

		internal static string CombineChars(char[] chars)
		{
			return new string(chars);
		}

		internal static void SetTimeArrayToClass(int[] timeArray, ref SubtitleData subtitleData)
		{
			/*
			subtitleData.startHour = timeArray[0];
			subtitleData.startMinute = timeArray[1];
			subtitleData.startSecond = timeArray[2];
			subtitleData.startMicrosecond = timeArray[3];
			subtitleData.endHour = timeArray[4];
			subtitleData.endMinute = timeArray[5];
			subtitleData.endSecond = timeArray[6];
			subtitleData.endMicrosecond = timeArray[7];
			*/

			subtitleData.startInMillis = CommonUtils.GetMillisFromTime(timeArray[0], timeArray[1] , timeArray[2] , timeArray[3]);
			subtitleData.endInMillis = CommonUtils.GetMillisFromTime(timeArray[4], timeArray[5] , timeArray[6] , timeArray[7]);
		}

		internal static string GetSubtitleContent(ref StreamReader reader)
		{

			string firstLine = reader.ReadLine() ?? throw new InvalidSubtitleException("found null, expected subtitle content");

			if (firstLine == "" || firstLine == null)
			{
				throw new InvalidSubtitleException("found empty, expected subtitle content");
			}

			string outputString = firstLine;
			string? currentLine = reader.ReadLine();
			while (currentLine != null && currentLine != "")
			{
				outputString += "\n";
				outputString += currentLine;
				currentLine = reader.ReadLine();
			}

			// removes "<i> </i>" type of tags
			outputString = Regex.Replace(outputString, @"<i>|<\/i>", string.Empty);

			return outputString;
		}

	}
}
