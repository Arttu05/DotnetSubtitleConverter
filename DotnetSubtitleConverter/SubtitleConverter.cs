using DotnetSubtitleConverter.Subtitles;
using System.IO;
using System.Reflection.PortableExecutable;

namespace DotnetSubtitleConverter
{
	public static class SubtitleConverter
	{
		public enum SubtitleType
		{
			SRT,
			VTT
		}

		/// <summary>
		/// Used to convert subtitle file to another format. 
		/// </summary>
		/// <param name="filePath">path to the subtitle file</param>
		/// <param name="subtitleType">Output format.</param>
		/// <returns></returns>
		/// <exception cref="FileNotFoundException"></exception>
		public static string ConvertTo(string filePath, SubtitleType subtitleType)
		{
			if (File.Exists(filePath) == false)
			{
				throw new FileNotFoundException($"File \"{filePath}\" not found");
			}

			SubtitleType inputSubtitleType = GetSubtitleType(filePath);
			StreamReader fileStream = GetFileStream(filePath);
			List<SubtitleData> subtitleData = new List<SubtitleData>();

			switch (inputSubtitleType)
			{
				case SubtitleType.SRT:
					subtitleData = SRT.GetSubtitleData(ref fileStream);
					break;
				case SubtitleType.VTT:
					fileStream = GetFileStream(filePath);
					subtitleData = VTT.GetSubtitleData(ref fileStream);
					break;
			}
			fileStream.Close();

			string outputString = "";
			switch (subtitleType)
			{
				case SubtitleType.SRT:
					outputString = SRT.GetConvertedString(subtitleData);
					break;
				case SubtitleType.VTT:
					outputString = VTT.GetConvertedString(subtitleData);
					break;
			}


			return outputString;
		}

		public static SubtitleType GetSubtitleType(string filePath)
		{
			StreamReader fileStream = GetFileStream(filePath);
			if (SRT.Check(ref fileStream))
			{
				return SubtitleType.SRT;
			}
			fileStream = GetFileStream(filePath);
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
