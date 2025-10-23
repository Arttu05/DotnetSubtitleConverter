using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DotnetSubtitleConverter.Subtitles
{
	internal static class ASS
	{

		// http://www.tcax.org/docs/ass-specs.htm

		public static List<SubtitleData> GetSubtitleData(ref StreamReader reader)
		{
			List<SubtitleData> subtitleDataList = new();
			ASSFormatContainer ASSFormat = new ASSFormatContainer();
			ReadToEvents(ref reader);

			bool foundFormat = false;
			while(foundFormat == false)
			{
				string expectedFormat = reader.ReadLine() ?? throw new InvalidSubtitleException();
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
			throw new NotImplementedException();
		}

		public static bool Check(ref StreamReader reader)
		{
			throw new NotImplementedException();
		}

		internal static void ReadToEvents(ref StreamReader reader) 
		{ 
			string currentLine = reader.ReadLine() ?? throw new InvalidSubtitleException();

			while (currentLine != "[Events]")
			{
				currentLine = reader.ReadLine() ?? throw new InvalidSubtitleException();
			}		
		}

		internal static SubtitleData ReadDialogue(string expectedDialogue, ASSFormatContainer format)
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

	}

	internal struct ASSFormatContainer() 
	{
		// other fields index can also be added, but for now "start" and "end" are the only required ones
		public int? startIndex;
		public int? endIndex;
	}
}
