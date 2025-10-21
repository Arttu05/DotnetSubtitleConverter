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

			ReadToEvents(ref reader);



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

		// TODO Read "format:"

	}
}
