using DotnetSubtitleConverter;
using DotnetSubtitleConverter.Subtitles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		public void NormalTimestamp()
		{
			SubtitleData outputData = SBV.ReadTimestampString(normalTimestamp);

			Assert.That(outputData.startInMillis, Is.EqualTo(expectedNormalData.startInMillis));
			Assert.That(outputData.endInMillis, Is.EqualTo(expectedNormalData.endInMillis));

		}
		[Test]
		public void With2HHTimestamp()
		{
			SubtitleData outputData = SBV.ReadTimestampString(with2HHTimestamp);

			Assert.That(outputData.startInMillis, Is.EqualTo(expectedWith2HHData.startInMillis));
			Assert.That(outputData.endInMillis, Is.EqualTo(expectedWith2HHData.endInMillis));

		}

		[Test]
		public void InvalidTimestamps()
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

	}
}
