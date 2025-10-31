namespace Tests
{
	internal static class Consts
	{
		private const string subtitle_folder = "./subtitle_files";
		private const string SBV_folder_name = "/sbv";
		private const string ASS_folder_name = "/ass";

		public const string SRT_EXAMPLE_FILE = $"{subtitle_folder}/SRT_example.srt";
		public const string VTT_EXAMPLE_FILE = $"{subtitle_folder}/VTT_example.vtt";
		public const string SBV_EXAMPLE_FILE = $"{subtitle_folder}{SBV_folder_name}/SBV_example.sbv";
		public const string VTT_EXAMPLE_WITH_REGION = $"{subtitle_folder}/VTT_with_region.vtt";
		public const string VTT_EXAMPLE_WITH_POSITION = $"{subtitle_folder}/VTT_with_positions.vtt";
		public const string ASS_EXAMPLE_FILE = $"{subtitle_folder}{ASS_folder_name}/ASS_example.ass";

		public const string SRT_TO_VTT_PATH = $"{subtitle_folder}/SRT_To_VTT.vtt";
		public const string VTT_TO_SRT_PATH = $"{subtitle_folder}/VTT_To_SRT.srt";
		public const string VTT_TO_SRT_WITH_OFFSET_PATH = $"{subtitle_folder}/VTT_To_SRT_With_Offset.srt";
		public const string SBV_TO_SRT_PATH = $"{subtitle_folder}/SBV_To_SRT.srt";
		
		

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
			if (File.Exists(VTT_EXAMPLE_WITH_POSITION) == false)
			{
				return false;
			}
			if (File.Exists(VTT_EXAMPLE_WITH_REGION) == false)
			{
				return false;
			}
			if (File.Exists(SBV_EXAMPLE_FILE) == false) 
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
			if (File.Exists(SBV_TO_SRT_PATH))
			{
				File.Delete(SBV_TO_SRT_PATH);
			}

			return true;
		}
	}
}
