using DotnetSubtitleConverter;
using DotnetSubtitleConverter.Subtitles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
	internal class SRTUnitTest
	{
		[Test]
		public void PositiveOffset()
		{

			int offset = 2000;

			List<SubtitleData> subtitleDatas = new List<SubtitleData>();
			subtitleDatas.Add(new SubtitleData{
				startInMillis = 1000, 
				endInMillis = 5000, 
				subtitleContent = "test" 
			});

			
			List<SubtitleData> subtitleDatasWithOffset = new List<SubtitleData>();

			//Makes sure values aren't references to the "subtitleDatas" List's values
			subtitleDatasWithOffset.Add(new SubtitleData
			{
				startInMillis = subtitleDatas[0].startInMillis,
				endInMillis = subtitleDatas[0].endInMillis,
				subtitleContent = subtitleDatas[0].subtitleContent,
			});

			CommonUtils.GetSubtitleDataWithOffset(subtitleDatasWithOffset, offset);

			Assert.That(subtitleDatas[0].startInMillis + offset, Is.EqualTo(subtitleDatasWithOffset[0].startInMillis));
			Assert.That(subtitleDatas[0].endInMillis + offset, Is.EqualTo(subtitleDatasWithOffset[0].endInMillis));
		}

		[Test]
		public void NegativeOffset()
		{

			int offset = -2000;

			List<SubtitleData> subtitleDatas = new List<SubtitleData>();
			subtitleDatas.Add(new SubtitleData{
				startInMillis = 3000, 
				endInMillis = 5000, 
				subtitleContent = "test" 
			});

			
			List<SubtitleData> subtitleDatasWithOffset = new List<SubtitleData>();

			//Makes sure values aren't references to the "subtitleDatas" List's values
			subtitleDatasWithOffset.Add(new SubtitleData
			{
				startInMillis = subtitleDatas[0].startInMillis,
				endInMillis = subtitleDatas[0].endInMillis,
				subtitleContent = subtitleDatas[0].subtitleContent,
			});

			CommonUtils.GetSubtitleDataWithOffset(subtitleDatasWithOffset, offset);

			Assert.That(subtitleDatas[0].startInMillis + offset, Is.EqualTo(subtitleDatasWithOffset[0].startInMillis));
			Assert.That(subtitleDatas[0].endInMillis + offset, Is.EqualTo(subtitleDatasWithOffset[0].endInMillis));
		}

		[Test]
		public void NegativeOffsetWithOverflow()
		{

			int offset = -2000;

			List<SubtitleData> subtitleDatas = new List<SubtitleData>();
			subtitleDatas.Add(new SubtitleData
			{
				startInMillis = 1000,
				endInMillis = 5000,
				subtitleContent = "test"
			});


			List<SubtitleData> subtitleDatasWithOffset = new List<SubtitleData>();

			//Makes sure values aren't references to the "subtitleDatas" List's values
			subtitleDatasWithOffset.Add(new SubtitleData
			{
				startInMillis = subtitleDatas[0].startInMillis,
				endInMillis = subtitleDatas[0].endInMillis,
				subtitleContent = subtitleDatas[0].subtitleContent,
			});

			CommonUtils.GetSubtitleDataWithOffset(subtitleDatasWithOffset, offset);

			Assert.That(0, Is.EqualTo(subtitleDatasWithOffset[0].startInMillis));
			Assert.That(subtitleDatas[0].endInMillis + offset, Is.EqualTo(subtitleDatasWithOffset[0].endInMillis));
		}
	}
}
