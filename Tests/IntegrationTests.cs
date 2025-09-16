using DotnetSubtitleConverter;
namespace IntegrationTests
{
	public class SubtitleTests
	{
		const string SRT_FILE = "./SRT_example.srt";
		const string SRT_TO_VTT_PATH = "./SRT_To_VTT.vtt";
		const string VTT_FILE = "./VTT_example.vtt";
		const string VTT_TO_SRT_PATH = "./VTT_To_SRT.srt";
		const string VTT_TO_SRT_WITH_OFFSET_PATH = "./VTT_To_SRT_With_Offset.srt";

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
			StreamWriter sw = new StreamWriter(SRT_TO_VTT_PATH);
			sw.WriteLine(output);
			sw.Close();
			Assert.Pass();
		}
		[Test]
		public void VTT_To_SRT()
		{
			string output = SubtitleConverter.ConvertTo(VTT_FILE, SubtitleConverter.SubtitleType.SRT);
			StreamWriter sw = new StreamWriter(VTT_TO_SRT_PATH);
			sw.WriteLine(output);
			sw.Close();
			Assert.Pass();
		}
		[Test]
		public void VTT_To_SRT_WithOffset()
		{
			string output = SubtitleConverter.ConvertTo(VTT_FILE, SubtitleConverter.SubtitleType.SRT,100000);
			StreamWriter sw = new StreamWriter(VTT_TO_SRT_WITH_OFFSET_PATH);
			sw.WriteLine(output);
			sw.Close();
			Assert.Pass();
		}
	}
}