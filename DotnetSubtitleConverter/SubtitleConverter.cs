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

		public static bool ConvertTo(string filePath, SubtitleType subtitleType)
		{
			if (File.Exists(filePath) == false)
			{
				throw new FileNotFoundException($"File \"{filePath}\" not found");
			}

			StreamReader fileStream = GetFileStream(filePath);
			SubtitleType inputSubtitleType = GetSubtitleType(fileStream);
			List<SubtitleData> subtitleData = new List<SubtitleData>();

			switch (inputSubtitleType)
			{
				case SubtitleType.SRT:
					subtitleData = SRT.GetSubtitleData();
					break;
				case SubtitleType.VTT:
					subtitleData = VTT.GetSubtitleData();
					break;
			}

			return true;
		}

		public static SubtitleType GetSubtitleType(string filePath)
		{
			throw new NotImplementedException();
		}

		private static SubtitleType GetSubtitleType(StreamReader fileStream)
		{ 
			throw new NotImplementedException();
			
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
