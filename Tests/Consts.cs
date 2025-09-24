using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
	internal static class Consts
	{
		public const string SRT_EXAMPLE_FILE = "./subtitle_files/SRT_example.srt";
		public const string SRT_TO_VTT_PATH = "./subtitle_files/SRT_To_VTT.vtt";
		public const string VTT_EXAMPLE_FILE = "./subtitle_files/VTT_example.vtt";
		public const string VTT_TO_SRT_PATH = "./subtitle_files/VTT_To_SRT.srt";
		public const string VTT_TO_SRT_WITH_OFFSET_PATH = "./subtitle_files/VTT_To_SRT_With_Offset.srt";
	
		public static bool CheckExampleFiles()
		{
			if (File.Exists(SRT_EXAMPLE_FILE) == false)
			{
				return false;
			}
			if (File.Exists(VTT_EXAMPLE_FILE) == false)
			{
				return false;
			}


			return true;
		}

		public static bool DeleteConvertedFiles()
		{
			if (File.Exists(SRT_TO_VTT_PATH))
			{
				File.Delete(SRT_TO_VTT_PATH);
			}
			if (File.Exists(VTT_TO_SRT_WITH_OFFSET_PATH))
			{
				File.Delete(VTT_TO_SRT_WITH_OFFSET_PATH);
			}
			if (File.Exists(VTT_TO_SRT_PATH))
			{
				File.Delete(VTT_TO_SRT_PATH);
			}

			return true;
		}
	}
}
