using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
				int[] timeArray = ReadTimeString(ref reader);
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
				convertedString += "\n";
				convertedString += "\n";
            }

            return convertedString;
        }

		public static bool Check(ref StreamReader reader)
		{
			string temp = reader.ReadLine();
			temp = reader.ReadLine();

			for (int i = 0; i < 1; i++)
			{
				try
				{
					int[] tempArr = ReadTimeString(ref reader);
					SubtitleData tempClass = new SubtitleData();
					SetTimeArrayToClass(tempArr, ref tempClass);

					GetSubtitleContent(ref reader);
				}
				catch (Exception e)
				{
					return false;
				}

			}

			return true;
		}


        //example output "00:00:00,000 --> 00:00:10,210"
        private static string GetTimeString(SubtitleData subtitleData)
		{
			// https://www.w3.org/TR/webvtt1/#file-structure
			string outputString = "";
			// start timestamp
			outputString += subtitleData.startHour;
			outputString += ":";
			outputString += subtitleData.startMinute;
			outputString += ":";
			outputString += subtitleData.startSecond;
			outputString += ".";
			outputString += subtitleData.startMicrosecond;

			// "arrow"
			outputString += " --> ";

			//end timestamp
			outputString += subtitleData.endHour;
			outputString += ":";
			outputString += subtitleData.endMinute;
			outputString += ":";
			outputString += subtitleData.endSecond;
			outputString += ".";
			outputString += subtitleData.endMicrosecond;

			return outputString;
		}

		private static int[] ReadTimeString(ref StreamReader reader)
		{
			string regexPattern = "^\\d{1,3}:\\d{1,2}:\\d{1,2}.\\d{3} --> \\d{1,3}:\\d{1,2}:\\d{1,2}.\\d{3}";
			string? rawTimeString = reader.ReadLine();
			if (rawTimeString == null)
			{
				throw new NullReferenceException();
			}
			if (Regex.Match(rawTimeString, regexPattern).Success == false)
			{
				throw new Exception("SRT timestamp is incorrect");
			}

			return GetTimeArray(rawTimeString);

		}

		private static int[] GetTimeArray(string validTimeString) // should ONLY be used with valitated string
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

		private static string CombineChars(char[] chars)
		{
			return new string(chars);
		}

		private static void SetTimeArrayToClass(int[] timeArray, ref SubtitleData subtitleData)
		{
			subtitleData.startHour = timeArray[0];
			subtitleData.startMinute = timeArray[1];
			subtitleData.startSecond = timeArray[2];
			subtitleData.startMicrosecond = timeArray[3];
			subtitleData.endHour = timeArray[4];
			subtitleData.endMinute = timeArray[5];
			subtitleData.endSecond = timeArray[6];
			subtitleData.endMicrosecond = timeArray[7];
		}

		private static string GetSubtitleContent(ref StreamReader reader)
		{
			string outputString = reader.ReadLine() ?? throw new NullReferenceException();
			string? currentLine = reader.ReadLine();
			while (currentLine != null && currentLine != "")
			{
				outputString += "\n";
				outputString += currentLine;
				currentLine = reader.ReadLine();
			}

			return outputString;
		}

	}
}
