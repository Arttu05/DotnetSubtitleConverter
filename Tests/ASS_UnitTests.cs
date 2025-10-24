using DotnetSubtitleConverter;
using DotnetSubtitleConverter.Subtitles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests;

namespace UnitTests
{
	internal class ASS_UnitTests
	{
		[OneTimeSetUp]
		public void SetUp()
		{
			if (Consts.CheckExampleFiles() == false)
			{
				throw new Exception("could not find some test files");
			}
		}

		[Test]
		public void ASS_ReadTest()
		{
			StreamReader reader = new(Consts.ASS_EXAMPLE_FILE);

			List<SubtitleData> data = ASS.GetSubtitleData(ref reader);

			// TODO put path and the already calculated count of data in same struct
			if(data.Count != 511)
			{
				throw new Exception("Count of ASS_example.ass data wasn't correct");
			}

		}

		[Test]
		public void ASS_WriteTest()
		{
			List<SubtitleData> data = new List<SubtitleData>()
			{
				new SubtitleData()
				{
					startInMillis = 12330,
					subtitleContent = "test",
					endInMillis= 15000
				},
				new SubtitleData()
				{
					startInMillis = 505000,
					subtitleContent = "test 2",
					endInMillis= 506000
				},

			};

			string output = ASS.GetConvertedString(data);

			StreamReader reader = new(TestUtils.GetStreamFromString(output));

			List<SubtitleData> outputDataList = ASS.GetSubtitleData(ref reader);

			for(int i = 0; i < outputDataList.Count; i++)
			{
				Assert.That(outputDataList[i].startInMillis, Is.EqualTo(data[i].startInMillis));
				Assert.That(outputDataList[i].endInMillis, Is.EqualTo(data[i].endInMillis));
				Assert.That(outputDataList[i].subtitleContent, Is.EqualTo(data[i].subtitleContent));
			}
		}

	}
}
