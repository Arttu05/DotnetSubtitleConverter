using DotnetSubtitleConverter.Subtitles;
using System.IO;
using System.Reflection.PortableExecutable;

namespace DotnetSubtitleConverter
{
	public static class SubtitleConverter
	{
		/// <summary>
		/// Subtitle formats.
		/// </summary>
		public enum SubtitleType
		{
			SRT,
			VTT,
			SBV
		}

		/// <summary>
		/// Used to convert subtitle file to another format. Throws "InvalidSubtitleException" if given subtitle is not in a supported format.
		/// 
		/// </summary>
		/// <param name="filePath">path to the subtitle file</param>
		/// <param name="subtitleType">Output format.</param>
		/// <param name="msOffset">Offsets subtitles timestamps in milliseconds. Value can be negative. </param>
		/// <param name="returnOnOffsetOverflow"> Throws exception if offset makes start and/or end timestamp below 0</param>
		/// <returns>Desired subtitle type as a string</returns>
		/// <exception cref="FileNotFoundException"></exception>
		/// <exception cref="InvalidSubtitleException"></exception>
		/// <exception cref="OffsetOverFlowException"></exception>
		public static string ConvertTo(string filePath, SubtitleType subtitleType, int msOffset = 0, bool returnOnOffsetOverflow = false)
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
				case SubtitleType.SBV:
					fileStream = GetFileStream(filePath);
					subtitleData = SBV.GetSubtitleData(ref fileStream);
					break;
			}
			fileStream.Close();

			CommonUtils.GetSubtitleDataWithOffset(subtitleData, msOffset, returnOnOffsetOverflow);

			string outputString = "";
			switch (subtitleType)
			{
				case SubtitleType.SRT:
					outputString = SRT.GetConvertedString(subtitleData);
					break;
				case SubtitleType.VTT:
					outputString = VTT.GetConvertedString(subtitleData);
					break;
				case SubtitleType.SBV:
					outputString = SBV.GetConvertedString(subtitleData);
					break;
			}


			return outputString;
		}


		/// <summary>
		/// Reads subtitle file. Validates and returns the subtitle type.
		/// If no subtitle type is found throws "InvalidSubtitleException"
		/// </summary>
		/// <param name="filePath">Path to the subtitle file</param>
		/// <returns>value from "SubtitleType" enum, indicating the subtitle format</returns>
		/// <exception cref="InvalidSubtitleException"></exception>
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
			fileStream = GetFileStream(filePath);
			if (SBV.Check(ref fileStream))
			{
				return SubtitleType.SBV;
			}

			throw new InvalidSubtitleException("subtitle is not valid or not in supported format");
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
