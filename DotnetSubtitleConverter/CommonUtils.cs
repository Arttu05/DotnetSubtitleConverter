using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSubtitleConverter
{
	internal static class CommonUtils
	{

		public static int hourInMillis = 3600000;
		public static int MinInMillis = 60000;
		public static int SecInMillis = 1000;

		public static int GetIntFromDividedInt(int from, int divide)
		{

			if(from < divide)
			{
				return 0;
			}

			int i = 0;
			while (from > divide)
			{

				from -= divide;

				i++;

			}

			return i;

		}	

		public static string GetTwoDigitStringFromInt(int time)
		{
			if(time > 9)
			{
				return time.ToString();
			}

			string returnVal = "0";
			returnVal += time.ToString();

			return returnVal;

		}

		/// <summary>
		/// Returns millisecond string for example milliseconds 2 would return "002"
		/// </summary>
		/// <param name="milliseconds"></param>
		/// <returns></returns>
		public static string GetThreeDigitStringFromInt(int milliseconds)
		{
			if (milliseconds > 99)
			{
				return milliseconds.ToString();
			}

			string milliInString = "0";

			if (milliseconds <= 9)
			{
				milliInString += "0";
			}

			milliInString += milliseconds.ToString();

			return milliInString;
		}

		public static int GetMillisFromTime(int hours, int minutes, int seconds, int milliseconds)
		{
			int timeInMillis = 0;

			timeInMillis += milliseconds;


			timeInMillis += seconds * CommonUtils.SecInMillis;
			timeInMillis += minutes * CommonUtils.MinInMillis;
			timeInMillis += hours * CommonUtils.hourInMillis;

			return timeInMillis;
		}


		public static void GetSubtitleDataWithOffset(List<SubtitleData> subtitledataList, int msOffset, bool returnOnOverflow = false)
		{
			if(msOffset == 0)
			{
				return;
			}

			foreach (SubtitleData subtitledata in subtitledataList)
			{

				if(subtitledata.startInMillis + msOffset < 0)
				{
					if (returnOnOverflow)
					{
						throw new OffsetOverFlowException("timestamp goes to negative after offset");
					}

					subtitledata.startInMillis = 0;
				}
				else
				{
					subtitledata.startInMillis += msOffset;
				}

				if(subtitledata.endInMillis + msOffset < 0)
				{
					if (returnOnOverflow)
					{
						throw new OffsetOverFlowException("timestamp goes to negative after offset");
					}

					subtitledata.endInMillis = 0;
				}
				else
				{
					subtitledata.endInMillis += msOffset;
				}

			}

			return;

		}
	}
}
