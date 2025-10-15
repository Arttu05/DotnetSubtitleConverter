using DotnetSubtitleConverter;
using DotnetSubtitleConverter.Subtitles;
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

		[Test]
		public void Check_VTT_WithPositions()
		{
			SubtitleConverter.SubtitleType outputType = SubtitleConverter.GetSubtitleType(Consts.VTT_EXAMPLE_WITH_POSITION);

			Assert.That(outputType, Is.EqualTo(SubtitleConverter.SubtitleType.VTT));

			StreamReader sr = new(Consts.VTT_EXAMPLE_WITH_POSITION);

			List<SubtitleData> subtitleDatas = DotnetSubtitleConverter.Subtitles.VTT.GetSubtitleData(ref sr);

			Assert.That(subtitleDatas.Count, Is.EqualTo(3));

		}

		[Test]
		public void Check_VTT_WithRegion()
		{
			SubtitleConverter.SubtitleType outputType = SubtitleConverter.GetSubtitleType(Consts.VTT_EXAMPLE_WITH_REGION);

			Assert.That(outputType, Is.EqualTo(SubtitleConverter.SubtitleType.VTT));

			StreamReader sr = new(Consts.VTT_EXAMPLE_WITH_REGION);

			List<SubtitleData> subtitleDatas = DotnetSubtitleConverter.Subtitles.VTT.GetSubtitleData(ref sr);

			Assert.That(subtitleDatas.Count, Is.EqualTo(6));
		}

		[Test]
		public void SBV_To_SRT()
		{

			string outputString = SubtitleConverter.ConvertTo(Consts.SBV_EXAMPLE_FILE, SubtitleConverter.SubtitleType.SRT);

			Stream streamFromString = TestUtils.GetStreamFromString(outputString);

			StreamReader SRT_streamReader = new(streamFromString);
			StreamReader SBV_streamReader = new(Consts.SBV_EXAMPLE_FILE);

			List<SubtitleData> original_SBV_Data = SBV.GetSubtitleData(ref SBV_streamReader);
			List<SubtitleData> converter_SRT_Data = SRT.GetSubtitleData(ref SRT_streamReader);

			Assert.That(converter_SRT_Data.Count, Is.EqualTo(original_SBV_Data.Count));

			for (int i = 0; i < original_SBV_Data.Count; i++) 
			{
				SubtitleData current_SBV_data = original_SBV_Data[i];
				SubtitleData current_SRT_data = converter_SRT_Data[i];

				Assert.That(current_SRT_data.startInMillis, Is.EqualTo(current_SBV_data.startInMillis));
				Assert.That(current_SRT_data.endInMillis, Is.EqualTo(current_SBV_data.endInMillis));
				Assert.That(current_SRT_data.subtitleContent, Is.EqualTo(current_SBV_data.subtitleContent));

			}


		}

	}
}