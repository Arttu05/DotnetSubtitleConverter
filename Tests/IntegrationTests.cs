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
			string output = SubtitleConverter.ConvertTo(Consts.SRT_EXAMPLE_FILE, SubtitleType.VTT);
			
			StreamWriter outputWriter = new StreamWriter(Consts.SRT_TO_VTT_PATH);
			outputWriter.Write(output);
			outputWriter.Close();

			SubtitleType outputSubtitleType = SubtitleConverter.GetSubtitleType(Consts.SRT_TO_VTT_PATH);
			
			Assert.That(outputSubtitleType, Is.EqualTo(SubtitleType.VTT));
		}

		[Test]
		public void VTT_To_SRT()
		{
			string output = SubtitleConverter.ConvertTo(Consts.	VTT_EXAMPLE_FILE, SubtitleType.SRT);
			StreamWriter sw = new StreamWriter(Consts.VTT_TO_SRT_PATH);
			sw.Write(output);
			sw.Close();

			SubtitleType outputSubtitleType = SubtitleConverter.GetSubtitleType(Consts.VTT_TO_SRT_PATH);

			Assert.That(outputSubtitleType, Is.EqualTo(SubtitleType.SRT));
		}

		[Test]
		public void VTT_To_SRT_WithOffset()
		{
			int offset = 100000;
			string output = SubtitleConverter.ConvertTo(Consts.VTT_EXAMPLE_FILE, SubtitleType.SRT,offset);
			StreamWriter sw = new StreamWriter(Consts.VTT_TO_SRT_WITH_OFFSET_PATH);
			sw.Write(output);
			sw.Close();

			SubtitleType outputSubtitleType = SubtitleConverter.GetSubtitleType(Consts.VTT_TO_SRT_WITH_OFFSET_PATH);

			Assert.That(outputSubtitleType, Is.EqualTo(SubtitleType.SRT));

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
			SubtitleType outputType = SubtitleConverter.GetSubtitleType(Consts.VTT_EXAMPLE_WITH_POSITION);

			Assert.That(outputType, Is.EqualTo(SubtitleType.VTT));

			StreamReader sr = new(Consts.VTT_EXAMPLE_WITH_POSITION);

			List<SubtitleData> subtitleDatas = DotnetSubtitleConverter.Subtitles.VTT.GetSubtitleData(ref sr);

			Assert.That(subtitleDatas.Count, Is.EqualTo(3));

		}

		[Test]
		public void Check_VTT_WithRegion()
		{
			SubtitleType outputType = SubtitleConverter.GetSubtitleType(Consts.VTT_EXAMPLE_WITH_REGION);

			Assert.That(outputType, Is.EqualTo(SubtitleType.VTT));

			StreamReader sr = new(Consts.VTT_EXAMPLE_WITH_REGION);

			List<SubtitleData> subtitleDatas = DotnetSubtitleConverter.Subtitles.VTT.GetSubtitleData(ref sr);

			Assert.That(subtitleDatas.Count, Is.EqualTo(6));
		}

		[Test]
		public void SBV_To_SRT()
		{

			string outputString = SubtitleConverter.ConvertTo(Consts.SBV_EXAMPLE_FILE, SubtitleType.SRT);

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

		[Test]
		public void VTT_To_SBV()
		{

			ConvertTest(Consts.VTT_EXAMPLE_FILE, SubtitleType.SBV);

		}

		[Test]
		public void ASS_To_SBV()
		{
			ConvertTest(Consts.ASS_EXAMPLE_FILE, SubtitleType.SBV);
		}

		[Test]
		public void VTT_To_ASS()
		{
			ConvertTest(Consts.VTT_EXAMPLE_FILE, SubtitleType.ASS, 10);
		}

		[Test]
		public void VTT_To_SRT_FileAndStringComparison()
		{
			StreamReader reader = new StreamReader(Consts.VTT_EXAMPLE_FILE);

			string fileString = reader.ReadToEnd();

			reader.Close();

			string outputWithString = SubtitleConverter.ConvertWithStringTo(fileString, SubtitleType.SRT);
			string outputWithFile = SubtitleConverter.ConvertTo(Consts.VTT_EXAMPLE_FILE, SubtitleType.SRT);
			
			Assert.That(outputWithFile, Is.EqualTo(outputWithString));
		}

		[Test]
		public void VTT_To_SRT_WithFileString()
		{
			StreamReader reader = new StreamReader(Consts.VTT_EXAMPLE_FILE);

			string fileString = reader.ReadToEnd();
			
			reader.Close();

			string outputString = SubtitleConverter.ConvertWithStringTo(fileString, SubtitleType.SRT);

			StreamReader originalReader = new StreamReader(TestUtils.GetStreamFromString(fileString));
			StreamReader convertedReader = new StreamReader(TestUtils.GetStreamFromString(outputString));

			List<SubtitleData> originalData = SRT.GetSubtitleData(ref convertedReader);

			List<SubtitleData> convertedData = VTT.GetSubtitleData(ref originalReader);

			CompareSubtitleDatas(originalData, convertedData);
		}


		/// <summary>
		/// Tries to convert given file to another format and then check that all timings and dialogues
		/// are the same
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="toType"></param>
		/// <param name="timingForgivness"> 
		/// Some formats like ASS are in centiseconds so values like 115ms cannot be represented in ASS format. 
		/// Instead the 115ms would be 110ms.
		/// "timingForgivness" will divide timings with given int.
		/// In case of ASS subtitles give value "10".
		/// </param>
		/// <exception cref="Exception"></exception>
		public void ConvertTest(string filePath, SubtitleType toType, int timingForgivness = 0)
		{
			string outputString = SubtitleConverter.ConvertTo(filePath, toType);

			Stream streamFromString = TestUtils.GetStreamFromString(outputString);

			StreamReader output_streamReader = new(streamFromString);
			StreamReader original_streamReader = new(filePath);


			List<SubtitleData> originalData;
			List<SubtitleData> converterData;

			switch (toType)
			{
				case SubtitleType.SBV:
					converterData = SBV.GetSubtitleData(ref output_streamReader);
					break;
				case SubtitleType.VTT:
					converterData = VTT.GetSubtitleData(ref output_streamReader);
					break;
				case SubtitleType.SRT:
					converterData = SRT.GetSubtitleData(ref output_streamReader);
					break;
				case SubtitleType.ASS:
					converterData = ASS.GetSubtitleData(ref output_streamReader);
					break;
				default:
					throw new Exception("ConvertTest() could not get subtitle type, make sure BOTH switch statements have all subtitle types as cases");
			}

			switch (SubtitleConverter.GetSubtitleType(filePath))
			{
				case SubtitleType.SBV:
					originalData = SBV.GetSubtitleData(ref original_streamReader);
					break;
				case SubtitleType.VTT:
					originalData = VTT.GetSubtitleData(ref original_streamReader);
					break;
				case SubtitleType.SRT:
					originalData = SRT.GetSubtitleData(ref original_streamReader);
					break;
				case SubtitleType.ASS:
					originalData = ASS.GetSubtitleData(ref original_streamReader);
					break;
				default:
					throw new Exception("ConvertTest() could not get subtitle type, make sure BOTH switch statements have all subtitle types as cases");
			}

			Assert.That(converterData.Count, Is.EqualTo(originalData.Count));

			for (int i = 0; i < originalData.Count; i++)
			{
				SubtitleData currentOriginalData = originalData[i];
				SubtitleData currentConvertedData = converterData[i];
				if(timingForgivness == 0)
				{
					Assert.That(currentConvertedData.startInMillis, Is.EqualTo(currentOriginalData.startInMillis));
					Assert.That(currentConvertedData.endInMillis, Is.EqualTo(currentOriginalData.endInMillis));
				}
				else
				{
					Assert.That((currentConvertedData.startInMillis / timingForgivness), Is.EqualTo((currentOriginalData.startInMillis / timingForgivness)));
					Assert.That((currentConvertedData.endInMillis / timingForgivness), Is.EqualTo((currentOriginalData.endInMillis / timingForgivness)));
					
				}

				Assert.That(currentConvertedData.subtitleContent, Is.EqualTo(currentOriginalData.subtitleContent));

			}
		}

		private void CompareSubtitleDatas(List<SubtitleData> listOne, List<SubtitleData> listTwo, int timingForgivness = 0)
		{
			Assert.That(listOne.Count, Is.EqualTo(listTwo.Count));

			for (int i = 0; i < listOne.Count; i++)
			{
				SubtitleData currentListOneData = listOne[i];
				SubtitleData currentListTwoData = listTwo[i];
				if (timingForgivness == 0)
				{
					Assert.That(currentListTwoData.startInMillis, Is.EqualTo(currentListOneData.startInMillis));
					Assert.That(currentListTwoData.endInMillis, Is.EqualTo(currentListOneData.endInMillis));
				}
				else
				{
					Assert.That((currentListTwoData.startInMillis / timingForgivness), Is.EqualTo((currentListOneData.startInMillis / timingForgivness)));
					Assert.That((currentListTwoData.endInMillis / timingForgivness), Is.EqualTo((currentListOneData.endInMillis / timingForgivness)));

				}

				Assert.That(currentListTwoData.subtitleContent, Is.EqualTo(currentListOneData.subtitleContent));

			}
		}

	}
}