using DotnetSubtitleConverter;
using Tests;
namespace IntegrationTests
{
	public class SubtitleTests
	{

		[OneTimeSetUp]
		public void Setup()
		{
			if(Consts.CheckExampleFiles() == false)
			{
				Assert.Fail("Some example files were not found");
			}
			Consts.DeleteConvertedFiles();
		}

		[Test]
		public void SRT_To_VTT()
		{
			string output = SubtitleConverter.ConvertTo(Consts.SRT_EXAMPLE_FILE, SubtitleConverter.SubtitleType.VTT);
			
			StreamWriter outputWriter = new StreamWriter(Consts.SRT_TO_VTT_PATH);
			outputWriter.Write(output);
			outputWriter.Close();

			SubtitleConverter.SubtitleType outputSubtitleType = SubtitleConverter.GetSubtitleType(Consts.SRT_TO_VTT_PATH);
			
			Assert.That(outputSubtitleType, Is.EqualTo(SubtitleConverter.SubtitleType.VTT));
		}

		[Test]
		public void VTT_To_SRT()
		{
			string output = SubtitleConverter.ConvertTo(Consts.	VTT_EXAMPLE_FILE, SubtitleConverter.SubtitleType.SRT);
			StreamWriter sw = new StreamWriter(Consts.VTT_TO_SRT_PATH);
			sw.Write(output);
			sw.Close();

			SubtitleConverter.SubtitleType outputSubtitleType = SubtitleConverter.GetSubtitleType(Consts.VTT_TO_SRT_PATH);

			Assert.That(outputSubtitleType, Is.EqualTo(SubtitleConverter.SubtitleType.SRT));
		}

		[Test]
		public void VTT_To_SRT_WithOffset()
		{
			int offset = 100000;
			string output = SubtitleConverter.ConvertTo(Consts.VTT_EXAMPLE_FILE, SubtitleConverter.SubtitleType.SRT,offset);
			StreamWriter sw = new StreamWriter(Consts.VTT_TO_SRT_WITH_OFFSET_PATH);
			sw.Write(output);
			sw.Close();

			SubtitleConverter.SubtitleType outputSubtitleType = SubtitleConverter.GetSubtitleType(Consts.VTT_TO_SRT_WITH_OFFSET_PATH);

			Assert.That(outputSubtitleType, Is.EqualTo(SubtitleConverter.SubtitleType.SRT));

			StreamReader outputReader = new(Consts.VTT_TO_SRT_WITH_OFFSET_PATH);
			StreamReader originalFileReader = new(Consts.VTT_EXAMPLE_FILE);

			List<DotnetSubtitleConverter.SubtitleData> convertedData = DotnetSubtitleConverter.Subtitles.SRT.GetSubtitleData(ref outputReader);
			List<DotnetSubtitleConverter.SubtitleData> originalData = DotnetSubtitleConverter.Subtitles.VTT.GetSubtitleData(ref originalFileReader);

			Assert.That(convertedData.Count, Is.EqualTo(originalData.Count));

			for (int i = 0; i < convertedData.Count; i++)
			{
				Assert.That(convertedData[i].startInMillis, Is.EqualTo(originalData[i].startInMillis + offset));
				Assert.That(convertedData[i].endInMillis, Is.EqualTo(originalData[i].endInMillis + offset));
				Assert.That(convertedData[i].subtitleContent, Is.EqualTo(originalData[i].subtitleContent));

			}

		}
	}
}