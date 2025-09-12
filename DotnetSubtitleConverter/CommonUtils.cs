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

			if(from > divide)
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

		public static string GetStringFromTime(int time)
		{
			if(time > 9)
			{
				return time.ToString();
			}

			string returnVal = "0";
			returnVal += time.ToString();

			return returnVal;

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
	}
}
