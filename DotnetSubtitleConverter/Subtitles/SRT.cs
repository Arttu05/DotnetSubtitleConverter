using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSubtitleConverter.Subtitles
{
    internal static class SRT
    {

        public static List<SubtitleData> GetSubtitleData()
        {
            throw new NotImplementedException();
        }

        public static string GetConvertedString()
        {
            throw new NotImplementedException();
        }

        public static bool Check()
        {
            throw new NotImplementedException();
        }

        //example output "00:00:00,000 --> 00:00:10,210"
        private static string GetTimeString(SubtitleData subtitleData)
        {
            // https://www.w3.org/TR/webvtt1/#file-structure
            string outputString = "";
            // start timestamp
            outputString += subtitleData.startHour < 10 ? ("0" + subtitleData.startHour.ToString()) : subtitleData.startHour;
            outputString += ":";
            outputString += subtitleData.startMinute < 10 ? ("0" + subtitleData.startMinute.ToString()) : subtitleData.startMinute;
            outputString += ":";
            outputString += subtitleData.startSecond < 10 ? ("0" + subtitleData.startSecond.ToString()) : subtitleData.startSecond;
            outputString = ",";
            outputString += subtitleData.startMicrosecond < 10 ? ("0" + subtitleData.startMicrosecond.ToString()) : subtitleData.startMicrosecond;

            // "arrow"
            outputString += " --> ";

            //end timestamp
            outputString += subtitleData.endHour < 10 ? ("0" + subtitleData.endHour.ToString()) : subtitleData.endHour;
            outputString += ":";
            outputString += subtitleData.endMinute < 10 ? ("0" + subtitleData.endMinute.ToString()) : subtitleData.endMinute;
            outputString += ":";
            outputString += subtitleData.endSecond < 10 ? ("0" + subtitleData.endSecond.ToString()) : subtitleData.endSecond;
            outputString = ",";
            outputString += subtitleData.endMicrosecond < 10 ? ("0" + subtitleData.endMicrosecond.ToString()) : subtitleData.endMicrosecond;

            return outputString;
        }
    }
}
