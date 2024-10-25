using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSubtitleConverter.Subtitles
{
    internal static class VTT
    {

        public static string GetConvertedString(SubtitleData[] subtitleDataArray)
        {
            string convertedString = "";

            convertedString += "WEBVTT\n\n";

			for (int i = 0; i < subtitleDataArray.Length; i++)
            {
				convertedString += GetTimeString(subtitleDataArray[i]);
				convertedString += "\n";
				convertedString += subtitleDataArray[i].subtitleContent;
				convertedString += "\n";
            }

            return convertedString;
        }

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
			outputString = ".";
			outputString += subtitleData.startMicrosecond;

			// "arrow"
			outputString += " --> ";

			//end timestamp
			outputString += subtitleData.endHour;
			outputString += ":";
			outputString += subtitleData.endMinute;
			outputString += ":";
			outputString += subtitleData.endSecond;
			outputString = ".";
			outputString += subtitleData.endMicrosecond;

			return outputString;
		}

	}
}
