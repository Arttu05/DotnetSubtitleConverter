using DotnetSubtitleConverter;
using DotnetSubtitleConverter.Subtitles;
using Tests;

namespace UnitTests
{
	internal class SBV_UnitTests
	{

		const string normalTimestamp = "0:00:35.000,0:00:36.000";
		readonly SubtitleData expectedNormalData = new() 
		{ 
			startInMillis = 35 * 1000, 
			endInMillis = 36 * 1000 
		};

		const string with2HHTimestamp = "12:00:35.000,12:00:36.000";
		readonly SubtitleData expectedWith2HHData = new() 
		{ 
			startInMillis = (CommonUtils.hourInMillis * 12) + (35 * 1000), 
			endInMillis = (CommonUtils.hourInMillis * 12) + (36 * 1000) 
		};

		List<String> invalidTimestampList = new List<string>() 
		{
			"12:00:35,12:00:36.000", // start doesn't have milliseconds
			"12:00:35.000.12:00:36.000", // . instead of ,
			"223432512131214", // just random numbers
			":00:35.000,:00:36.000", // missing hours
			"",
			"0:00:35,000,0:00:36,000", // millisecond dots(.) replaced with ,

		};

		[Test]
		public void ReadingNormalTimestamp()
		{
			SubtitleData outputData = SBV.ReadTimestampString(normalTimestamp);

			Assert.That(outputData.startInMillis, Is.EqualTo(expectedNormalData.startInMillis));
			Assert.That(outputData.endInMillis, Is.EqualTo(expectedNormalData.endInMillis));

		}
		[Test]
		public void ReadingWith2HHTimestamp()
		{
			SubtitleData outputData = SBV.ReadTimestampString(with2HHTimestamp);

			Assert.That(outputData.startInMillis, Is.EqualTo(expectedWith2HHData.startInMillis));
			Assert.That(outputData.endInMillis, Is.EqualTo(expectedWith2HHData.endInMillis));

		}

		[Test]
		public void ReadingInvalidTimestamps()
		{
			foreach (var invalidTimestamp in invalidTimestampList) 
			{ 
				try
				{
					SubtitleData outputData = SBV.ReadTimestampString(invalidTimestamp);

					Assert.Fail("Didn't throw InvalidSubtitleException with invalid timestamp");
					return;
				}
				catch (InvalidSubtitleException)
				{
					continue;
				}
			
			}
		}

		[Test]
		public void CreateTimeStampFromData()
		{
			int startHour = 2;
			int endHour = 2;
			int startMin = 32;
			int endMin = 33;
			int startSecond = 43;
			int endSecond = 12;
			int startMilli = 200;
			int endMilli = 100;

			string expectedTimestampString = 
			$"{startHour}:{CommonUtils.GetTwoDigitStringFromInt(startMin)}:{CommonUtils.GetTwoDigitStringFromInt(startSecond)}.{CommonUtils.GetThreeDigitStringFromInt(startMilli)}"
			+$",{endHour}:{CommonUtils.GetTwoDigitStringFromInt(endMin)}:{CommonUtils.GetTwoDigitStringFromInt(endSecond)}.{CommonUtils.GetThreeDigitStringFromInt(endMilli)}";

			SubtitleData testData = new SubtitleData()
			{
				startInMillis = (startHour * CommonUtils.hourInMillis) + (startMin * CommonUtils.MinInMillis) + (startSecond * CommonUtils.SecInMillis ) + startMilli,
				endInMillis = (endHour * CommonUtils.hourInMillis) + (endMin * CommonUtils.MinInMillis) + (endSecond * CommonUtils.SecInMillis) + endMilli,
				subtitleContent = "test"
			};

			string actualTimestamp = SBV.GetTimestampString(testData);

			Assert.That(actualTimestamp, Is.EqualTo(expectedTimestampString));
		}

		[Test]
		public void ReadingOneLine()
		{
			string testLines = "line1\n\nline2";
			string exceptedString = "line1";

			Stream lineStream = TestUtils.GetStreamFromString(testLines);
			StreamReader reader = new StreamReader(lineStream);

			string actualString =  SBV.GetSubtitleContent(ref reader);

			Assert.That(actualString, Is.EqualTo(exceptedString));
		}

		[Test]
		public void ReadingTwoLines()
		{
			string testLines = "line1\nline2\n\nline3";
			string exceptedString = "line1\nline2";

			Stream lineStream = TestUtils.GetStreamFromString(testLines);
			StreamReader reader = new StreamReader(lineStream);

			string actualString = SBV.GetSubtitleContent(ref reader);

			Assert.That(actualString, Is.EqualTo(exceptedString));
		}

	}
}
