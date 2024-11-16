using DotnetSubtitleConverter.Subtitles;

namespace DotnetSubtitleConverter
{
	public static class SubtitleConverter
	{
		public enum SubtitleType
		{
			SRT,
			VTT
		}

		public static string ConvertTo(string filePath, SubtitleType subtitleType)
		{
			if (File.Exists(filePath) == false)
			{
				throw new FileNotFoundException($"File \"{filePath}\" not found");
			}

			StreamReader fileStream = GetFileStream(filePath);
			SubtitleType inputSubtitleType = GetSubtitleType(ref fileStream);
			List<SubtitleData> subtitleData = new List<SubtitleData>();

			switch (inputSubtitleType)
			{
				case SubtitleType.SRT:
					subtitleData = SRT.GetSubtitleData(ref fileStream);
					break;
				case SubtitleType.VTT:
					subtitleData = VTT.GetSubtitleData(ref fileStream);
					break;
			}

			string outputString = "";
			switch (subtitleType)
			{
				case SubtitleType.SRT:
					outputString = SRT.GetConvertedString();
					break;
				case SubtitleType.VTT:
					outputString = VTT.GetConvertedString(subtitleData);
					break;
			}


			return outputString;
		}

		public static SubtitleType GetSubtitleType(string filePath)
		{
			throw new NotImplementedException();
		}

		private static SubtitleType GetSubtitleType(ref StreamReader fileStream)
		{
			if(SRT.Check(ref fileStream))
			{
				return SubtitleType.SRT;
			}
			if (VTT.Check(ref fileStream))
			{
				return SubtitleType.VTT;
			}

			throw new Exception("subtitle is not valid or not in supported format");
		}

		// private functions
		private static StreamReader GetFileStream(string filePath)
		{
			if (File.Exists(filePath) == false)
			{
				throw new FileNotFoundException($"File \"{filePath}\" not found");
			}

			StreamReader sr = new StreamReader(filePath);
			return sr;
		}
	}
}
