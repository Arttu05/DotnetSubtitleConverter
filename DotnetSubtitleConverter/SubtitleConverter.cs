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

			Stream fileStream = GetFileStream(filePath);
			SubtitleType inputSubtitleType = GetSubtitleType(fileStream);


			return true;
		}

		public static SubtitleType GetSubtitleType(string filePath)
		{
			throw new NotImplementedException();
		}

		private static SubtitleType GetSubtitleType(Stream filePath)
		{ 
			throw new NotImplementedException();
			
		}

		// private functions
		private static Stream GetFileStream(string filePath)
		{
			if (File.Exists(filePath) == false)
			{
				throw new FileNotFoundException($"File \"{filePath}\" not found");
			}

			StreamReader sr = new StreamReader(filePath);

			return sr.BaseStream;
		}
	}
}
