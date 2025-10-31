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
			while (from >= divide)
			{

				from -= divide;

				i++;

			}

			return i;

		}	

		/// <summary>
		/// returns two(2) digit integer as a string. example: "time" value as 5 would return "05"
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		/// <exception cref="InvalidSubtitleException"></exception>
		public static string GetTwoDigitStringFromInt(int time)
		{
			if (time > 99) 
			{
				throw new InvalidSubtitleException();
			}

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
			if (milliseconds > 99 && milliseconds <= 999)
			{
				return milliseconds.ToString();
			}

			if(milliseconds > 999)
			{
				throw new InvalidSubtitleException();
			}

			string milliInString = "0";

			if (milliseconds <= 9)
			{
				milliInString += "0";
			}

			milliInString += milliseconds.ToString();

			return milliInString;
		}

		/// <summary>
		/// Returns integer that represents given hours, minutes, seconds and milliseconds compined to milliseconds.
		/// </summary>
		/// <param name="hours"></param>
		/// <param name="minutes"></param>
		/// <param name="seconds"></param>
		/// <param name="milliseconds"></param>
		/// <returns></returns>
		public static int GetMillisFromTime(int hours, int minutes, int seconds, int milliseconds)
		{
			int timeInMillis = 0;

			timeInMillis += milliseconds;


			timeInMillis += seconds * CommonUtils.SecInMillis;
			timeInMillis += minutes * CommonUtils.MinInMillis;
			timeInMillis += hours * CommonUtils.hourInMillis;

			return timeInMillis;
		}

		/// <summary>
		/// Offsets start and end timestamps, with given offset value. Offset value can be negative but if timestamp goes below the value 0, it will be set to 0,
		/// unless the "returnOnOverflow" is true, then exception will be thrown.
		/// </summary>
		/// <param name="subtitledataList"></param>
		/// <param name="msOffset"></param>
		/// <param name="returnOnOverflow"></param>
		/// <exception cref="OffsetOverFlowException"></exception>
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
