﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DotnetSubtitleConverter.Subtitles
{
    internal static class SRT
    {

        public static List<SubtitleData> GetSubtitleData(ref StreamReader reader)
        {
            List<SubtitleData> outputList = new List<SubtitleData>();

            while (reader.EndOfStream == false) 
            {
                SubtitleData currentSubtitleData = new SubtitleData(); 
                
                // validates the subtitle num
                if(ReadNum(ref reader) == false) 
                {
                    throw new Exception("incorrect SRT subtitle num");
                }

                // start and end timestamps
                int[] timeArray =  ReadTimeString(ref reader);
                SetTimeArrayToClass(timeArray, ref currentSubtitleData);

                //reading subtitle content
                string subtitleContent = GetSubtitleContent(ref reader);
                currentSubtitleData.subtitleContent = subtitleContent;
                
                outputList.Add(currentSubtitleData);
            }

            return outputList;
        }

        public static string GetConvertedString(List<SubtitleData> subtitleData)
        {
			string outputString = "";

			for (int i = 0; i < subtitleData.Count; i++)
			{
				outputString += (i + 1); //subtitle num
				outputString += "\n";
				outputString += GetTimeString(subtitleData[i]);
				outputString += "\n";
				outputString += subtitleData[i].subtitleContent;
				outputString += "\n";
				outputString += "\n";
			}

			return outputString;
		}

        public static bool Check(ref StreamReader reader)
        {
            for (int i = 0; i < 1;i++)
            {
                try
                {
                    ReadNum(ref reader);

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
            outputString += subtitleData.startHour < 10 ? ("0" + subtitleData.startHour.ToString()) : subtitleData.startHour;
            outputString += ":";
            outputString += subtitleData.startMinute < 10 ? ("0" + subtitleData.startMinute.ToString()) : subtitleData.startMinute;
            outputString += ":";
            outputString += subtitleData.startSecond < 10 ? ("0" + subtitleData.startSecond.ToString()) : subtitleData.startSecond;
            outputString += ",";
            outputString += subtitleData.startMicrosecond < 10 ? ("0" + subtitleData.startMicrosecond.ToString()) : subtitleData.startMicrosecond;

            // "arrow"
            outputString += " --> ";

            //end timestamp
            outputString += subtitleData.endHour < 10 ? ("0" + subtitleData.endHour.ToString()) : subtitleData.endHour;
            outputString += ":";
            outputString += subtitleData.endMinute < 10 ? ("0" + subtitleData.endMinute.ToString()) : subtitleData.endMinute;
            outputString += ":";
            outputString += subtitleData.endSecond < 10 ? ("0" + subtitleData.endSecond.ToString()) : subtitleData.endSecond;
            outputString += ",";
            outputString += subtitleData.endMicrosecond < 10 ? ("0" + subtitleData.endMicrosecond.ToString()) : subtitleData.endMicrosecond;

            return outputString;
        }


        private static bool ReadNum(ref StreamReader reader)
        {
            string? rawNum = reader.ReadLine();
            Debug.Print(rawNum);
            Debug.Print(rawNum.Length.ToString());
            int convertedInt = 0;

            if(rawNum == null)
            {
                return false;
            }
            if(int.TryParse(rawNum, out convertedInt) == false)
            {
                return false;
            }

            return true;
        }

        // [0-3] starting times h m s ms
        // [4-7] end times      h m s ms
        private static int[] ReadTimeString(ref StreamReader reader)
        {
            string regexPattern = "^\\d{1,3}:\\d{1,2}:\\d{1,2}.\\d{3} --> \\d{1,3}:\\d{1,2}:\\d{1,2}.\\d{3}";
			string? rawTimeString = reader.ReadLine();
            if(rawTimeString == null)
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
            while(currentLine != null && currentLine != "")
            {
                outputString += "\n";
                outputString += currentLine;
                currentLine = reader.ReadLine();
            }

            return outputString;
        }
    }
}
