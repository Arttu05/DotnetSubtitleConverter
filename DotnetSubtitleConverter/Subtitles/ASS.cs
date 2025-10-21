using System;
using System.Collections.Generic;
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




			throw new NotImplementedException();
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

		internal static Match? ReadDialogue(string expectedDialogue)
		{
			string regexPattern = "Dialogue: ?([^,]+),([^,]+),([^,]+),([^,]*),([^,]*),([^,]*),([^,]*),([^,]*),([^,]*),(.+)";

			return null;
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

	}

	internal struct ASSFormatContainer() 
	{
		// other fields index can also be added, but for now "start" and "end" are the only required ones
		public int? startIndex;
		public int? endIndex;
	}
}
