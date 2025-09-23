using DotnetSubtitleConverter;
namespace IntegrationTests
{
	public class SubtitleTests
	{
		const string SRT_FILE = "./subtitle_files/SRT_example.srt";
		const string SRT_TO_VTT_PATH = "./subtitle_files/SRT_To_VTT.vtt";
		const string VTT_FILE = "./subtitle_files/VTT_example.vtt";
		const string VTT_TO_SRT_PATH = "./subtitle_files/VTT_To_SRT.srt";
		const string VTT_TO_SRT_WITH_OFFSET_PATH = "./subtitle_files/VTT_To_SRT_With_Offset.srt";

		[OneTimeSetUp]
		public void Setup()
		{
			//SRT checks
			if (File.Exists(SRT_FILE) == false)
			{
				Assert.Fail("SRT file not found");
			}

			if (File.Exists(SRT_TO_VTT_PATH))
			{
				File.Delete(SRT_TO_VTT_PATH);
			}

			if (File.Exists(VTT_TO_SRT_WITH_OFFSET_PATH))
			{
				File.Delete(VTT_TO_SRT_WITH_OFFSET_PATH);
			}

			//VTT checks
			if (File.Exists(VTT_FILE) == false)
			{
				Assert.Fail("VTT file not found");
			}

			if (File.Exists(VTT_TO_SRT_PATH))
			{
				File.Delete(VTT_TO_SRT_PATH);
			}

		}

		[Test]
		public void SRT_To_VTT()
		{
			string output = SubtitleConverter.ConvertTo(SRT_FILE, SubtitleConverter.SubtitleType.VTT);
			
			StreamWriter outputWriter = new StreamWriter(SRT_TO_VTT_PATH);
			outputWriter.Write(output);
			outputWriter.Close();

			SubtitleConverter.SubtitleType outputSubtitleType = SubtitleConverter.GetSubtitleType(SRT_TO_VTT_PATH);
			
			Assert.That(outputSubtitleType, Is.EqualTo(SubtitleConverter.SubtitleType.VTT));
		}

		[Test]
		public void VTT_To_SRT()
		{
			string output = SubtitleConverter.ConvertTo(VTT_FILE, SubtitleConverter.SubtitleType.SRT);
			StreamWriter sw = new StreamWriter(VTT_TO_SRT_PATH);
			sw.Write(output);
			sw.Close();

			SubtitleConverter.SubtitleType outputSubtitleType = SubtitleConverter.GetSubtitleType(VTT_TO_SRT_PATH);

			Assert.That(outputSubtitleType, Is.EqualTo(SubtitleConverter.SubtitleType.SRT));
		}

		[Test]
		public void VTT_To_SRT_WithOffset()
		{
			int offset = 100000;
			string output = SubtitleConverter.ConvertTo(VTT_FILE, SubtitleConverter.SubtitleType.SRT,offset);
			StreamWriter sw = new StreamWriter(VTT_TO_SRT_WITH_OFFSET_PATH);
			sw.Write(output);
			sw.Close();

			SubtitleConverter.SubtitleType outputSubtitleType = SubtitleConverter.GetSubtitleType(VTT_TO_SRT_WITH_OFFSET_PATH);

			Assert.That(outputSubtitleType, Is.EqualTo(SubtitleConverter.SubtitleType.SRT));

			StreamReader outputReader = new(VTT_TO_SRT_WITH_OFFSET_PATH);
			StreamReader originalFileReader = new(VTT_FILE);

			List<DotnetSubtitleConverter.SubtitleData> convertedData = DotnetSubtitleConverter.Subtitles.SRT.GetSubtitleData(ref outputReader);
			List<DotnetSubtitleConverter.SubtitleData> originalData = DotnetSubtitleConverter.Subtitles.VTT.GetSubtitleData(ref originalFileReader);

			Assert.That(convertedData.Count, Is.EqualTo(originalData.Count));

			for (int i = 0; i < convertedData.Count; i++)
			{
				Assert.That(convertedData[i].startInMillis, Is.EqualTo(originalData[i].startInMillis + offset));
				Assert.That(convertedData[i].endInMillis, Is.EqualTo(originalData[i].endInMillis + offset));
				Assert.That(convertedData[i].subtitleContent, Is.EqualTo(originalData[i].subtitleContent));

			}

			string output2 = SubtitleConverter.ConvertTo(VTT_TO_SRT_WITH_OFFSET_PATH, SubtitleConverter.SubtitleType.VTT);

		}
	}
}