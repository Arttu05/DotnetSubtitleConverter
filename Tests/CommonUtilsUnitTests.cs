using DotnetSubtitleConverter;
using DotnetSubtitleConverter.Subtitles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
	internal class CommonUtilityTests
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

			Assert.That(subtitleDatasWithOffset[0].startInMillis, Is.EqualTo(0));
			Assert.That(subtitleDatas[0].endInMillis + offset, Is.EqualTo(subtitleDatasWithOffset[0].endInMillis));
		}

		[Test]
		public void TwoDigitIntWithThreeDigitValue()
		{
			int test_value = 100;
			try
			{
				CommonUtils.GetTwoDigitStringFromInt(test_value);
				
				Assert.Fail();
			}
			catch (InvalidSubtitleException)
			{
				Assert.Pass();
			}
			catch (Exception ex) 
			{ 
				Assert.Fail(ex.Message);
			}
		}

		[Test]
		public void TwoDigitIntWithOneDigitValue()
		{
			int test_value = 5;
			try
			{
				string valueFromFunction = CommonUtils.GetTwoDigitStringFromInt(test_value);

				Assert.That(valueFromFunction, Is.EqualTo($"0{test_value.ToString()}"));
			}
			catch (InvalidSubtitleException)
			{
				Assert.Fail("GetTwoDigitStringFromInt() threw InvalidSubtitleException");
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}

		[Test]
		public void TwoDigitIntWithTwoDigitValue()
		{
			int test_value = 55;
			try
			{
				string valueFromFunction = CommonUtils.GetTwoDigitStringFromInt(test_value);

				Assert.That(valueFromFunction, Is.EqualTo($"{test_value}"));
			}
			catch (InvalidSubtitleException)
			{
				Assert.Fail("GetTwoDigitStringFromInt() threw InvalidSubtitleException");
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}

		[Test]
		public void ThreeDigitIntWithFourDigitValue()
		{
			int test_value = 1000;
			try
			{
				string valueFromFunction = CommonUtils.GetThreeDigitStringFromInt(test_value);

				Assert.Fail("functions should have thrown exception");
			}
			catch (InvalidSubtitleException)
			{
				Assert.Pass();
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}
	}
}
