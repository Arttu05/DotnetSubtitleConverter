using System.Text.RegularExpressions;

namespace DotnetSubtitleConverter.Subtitles
{
	internal static class ASS
	{

		const string formatNotFoundMessage = "could not find 'Format:'";

		const string defaultOutputSettings =
@"[Script Info]
; DotnetSubtitleConverter file
Title: DotnetSubtitleConverter file
ScriptType: v4.00+

[V4+ Styles]
Format: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, OutlineColour, BackColour, Bold, Italic, Underline, StrikeOut, ScaleX, ScaleY, Spacing, Angle, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, Encoding
Style: Default,Arial,22,&H00FFFFFF,&H000000FF,&HFA000000,&HFA000000,0,0,0,0,110,100,1,0,1,1,1,2,10,10,5,1";

		const string defaultOutputFormat = "Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text";

		// http://www.tcax.org/docs/ass-specs.htm

		public static List<SubtitleData> GetSubtitleData(ref StreamReader reader)
		{
			List<SubtitleData> subtitleDataList = new();
			ASSFormatContainer ASSFormat = new ASSFormatContainer();
			ReadToEvents(ref reader);

			bool foundFormat = false;
			while(foundFormat == false)
			{
				string expectedFormat = reader.ReadLine() ?? throw new InvalidSubtitleException(formatNotFoundMessage);
				ASSFormatContainer? potentialFormat = ReadASSFormat(expectedFormat);

				if (potentialFormat == null)
				{
					continue;
				}

				ASSFormat = (ASSFormatContainer)potentialFormat;
				foundFormat = true;
			}

			while(reader.EndOfStream == false)
			{
				string expectedDialogue = reader.ReadLine();

				//other section starts
				if(Regex.Match(expectedDialogue, "^\\[.*\\]$").Success)
				{
					break;
				}

				SubtitleData? possibleSubtitleData = ReadDialogue(expectedDialogue, ASSFormat);

				if (possibleSubtitleData == null) 
				{
					continue;
				}

				subtitleDataList.Add(possibleSubtitleData);

			}


			return subtitleDataList;
		}

		public static string GetConvertedString(List<SubtitleData> subtitleData)
		{
			string outputString = "";

			outputString += defaultOutputSettings;
			outputString += "\n\n";
			outputString += "[Events]";
			outputString += "\n";
			outputString += defaultOutputFormat;

			foreach(SubtitleData data in subtitleData)
			{
				outputString += $"\nDialogue: 0,{GetTimestampStringFromMillis(data.startInMillis)},{GetTimestampStringFromMillis(data.endInMillis)},Default,,0,0,0,,{data.subtitleContent.Replace("\n","\\n")}";
			}

			return outputString;
		}

		public static bool Check(ref StreamReader reader)
		{

			ASSFormatContainer format = new();

			List<SubtitleData> tempData = new();

			ReadToEvents(ref reader);

			bool formatFound = false;
			while(formatFound == false)
			{
				string expectedFormat = reader.ReadLine() ?? throw new InvalidSubtitleException(formatNotFoundMessage);
				ASSFormatContainer? potentialFormat = ReadASSFormat(expectedFormat);

				if (potentialFormat == null)
				{
					continue;
				}

				format = (ASSFormatContainer)potentialFormat;

				formatFound = true;

			}

			while(reader.EndOfStream == false)
			{

				string expectedDialogue = reader.ReadLine();

				//other section starts
				if(Regex.Match(expectedDialogue, "^\\[.*\\]$").Success)
				{
					break;
				}

				SubtitleData? possibleSubtitleData = ReadDialogue(expectedDialogue, format);

				if(possibleSubtitleData == null)
				{
					continue;
				}

				tempData.Add(possibleSubtitleData);

			}

			if(tempData.Count == 0)
			{
				return false;
			}

			return true;
		}

		internal static void ReadToEvents(ref StreamReader reader) 
		{ 
			string currentLine = reader.ReadLine() ?? throw new InvalidSubtitleException("[Events] was not found");

			while (currentLine != "[Events]")
			{
				currentLine = reader.ReadLine() ?? throw new InvalidSubtitleException("[Events] was not found");
			}		
		}

		internal static SubtitleData? ReadDialogue(string expectedDialogue, ASSFormatContainer format)
		{
			string regexPattern = "Dialogue: ?([^,]+),([^,]+),([^,]+),([^,]*),([^,]*),([^,]*),([^,]*),([^,]*),([^,]*),(.+)";

			Match dialogueMatch = Regex.Match(expectedDialogue, regexPattern);


			if(dialogueMatch.Success == false)
			{
				return null;
			}

			SubtitleData subtitle = GetDialogueDataFromMatch(dialogueMatch, format);



			return subtitle;
		}

		internal static ASSFormatContainer? ReadASSFormat(string expectedFormat)
		{
			string regexPattern = "Format:\\W*([^,]+),\\W*([^,]+),\\W*([^,]+),\\W*([^,]*),\\W*([^,]*),\\W*([^,]*),\\W*([^,]*),\\W*([^,]*),\\W*([^,]*),\\W*(.+)";

			Match formatRegexMatch = Regex.Match(expectedFormat, regexPattern);
			
			if(formatRegexMatch.Success == false)
			{
				return null;
			}

			ASSFormatContainer returnValue = new ASSFormatContainer();

			for(int i = 1; i <= 10; i++)
			{
				string currentGroup = formatRegexMatch.Groups[i].Value;

				switch (currentGroup)
				{
					case "Start":
						returnValue.startIndex = i;
						break;
					case "End":
						returnValue.endIndex = i;
						break;
					default: 
						break;
				}
			}


			if(returnValue.endIndex == null ||returnValue.startIndex == null)
			{
				return null;
			}

			return returnValue;

		}

		internal static SubtitleData GetDialogueDataFromMatch(Match dialogueMatch, ASSFormatContainer format)
		{

			string endString = dialogueMatch.Groups[(int)format.endIndex].Value;

			string startString = dialogueMatch.Groups[(int)format.startIndex].Value;

			// last value is always the text
			string dialogueText = dialogueMatch.Groups[dialogueMatch.Groups.Count - 1].Value;

			dialogueText = dialogueText.Replace("\\n", "\n");
			dialogueText = dialogueText.Replace("\\N", "\n");

			string overWrittenStylePattern = "\\{.*?\\}";

			dialogueText = Regex.Replace(dialogueText, overWrittenStylePattern, string.Empty);

			return new SubtitleData(){
				endInMillis = GetTimeInMillisFromTimestamp(endString),
				startInMillis = GetTimeInMillisFromTimestamp(startString),
				subtitleContent = dialogueText
			};
		}

		internal static int GetTimeInMillisFromTimestamp(string timestamp)
		{
			// in regexPattern replace "^(\\d+)" with "^(\\d)", if you want to only support "valid" timestamps
			// so timestamps that can only be up to 9 hours.
			string regexPattern = "^(\\d+):([0-5]\\d):([0-5]\\d)\\.(\\d{2})$";
			Match match = Regex.Match(timestamp, regexPattern);

			if (!int.TryParse(match.Groups[1].Value, out int hours) ||
				!int.TryParse(match.Groups[2].Value, out int minutes) ||
				!int.TryParse(match.Groups[3].Value, out int seconds) ||
				!int.TryParse(match.Groups[4].Value, out int centiseconds))
			{
				throw new InvalidSubtitleException("Invalid number format in timestamp");
			}

			int timeInMillis = CommonUtils.GetMillisFromTime(hours, minutes, seconds, (centiseconds * 10));

			return timeInMillis;

		}

		internal static string GetTimestampStringFromMillis(int timestampInMillis)
		{
			int millisAfterDivide = timestampInMillis;

			int hours = CommonUtils.GetIntFromDividedInt(millisAfterDivide, CommonUtils.hourInMillis);
			millisAfterDivide -= (hours * CommonUtils.hourInMillis);

			int minutes = CommonUtils.GetIntFromDividedInt(millisAfterDivide, CommonUtils.MinInMillis);
			millisAfterDivide -= (minutes * CommonUtils.MinInMillis);

			int seconds = CommonUtils.GetIntFromDividedInt(millisAfterDivide, CommonUtils.SecInMillis);
			millisAfterDivide -= seconds * CommonUtils.SecInMillis;

			string outputString = "";
			// start timestamp
			outputString += hours;
			outputString += ":";
			outputString += CommonUtils.GetTwoDigitStringFromInt(minutes);
			outputString += ":";
			outputString += CommonUtils.GetTwoDigitStringFromInt(seconds);
			outputString += ".";
			outputString += CommonUtils.GetTwoDigitStringFromInt((millisAfterDivide / 10));

			return outputString;
		}

	}

	internal struct ASSFormatContainer() 
	{
		// other fields index can also be added, but for now "start" and "end" are the only required ones
		public int? startIndex;
		public int? endIndex;
	}
}
