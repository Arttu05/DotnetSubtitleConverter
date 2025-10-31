using DotnetSubtitleConverter;
using DotnetSubtitleConverter.Subtitles;
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

		[Test]
		public void ReadFormatTest()
		{
			string formatString = "Format: Layer, Start, Style, Name, End, MarginL, MarginR, MarginV, Effect, Text";
			int expectedStartIndex = 2;
			int expectedEndIndex = 5;

			ASSFormatContainer? format = ASS.ReadASSFormat(formatString);
		
			if(format == null)
			{
				Assert.Fail("format was null");
			}

			Assert.That(format.Value.startIndex, Is.EqualTo(expectedStartIndex));
			Assert.That(format.Value.endIndex, Is.EqualTo(expectedEndIndex));

		}

		[Test]
		public void ReadDialogueTest()
		{
			ASSFormatContainer format = new()
			{
				startIndex = 2,
				endIndex = 3,
			};

			string expectedDialogueText = "Ah. Got ya.";
			int expectedStartTime = 30110;
			int expectedEndTime = 31830;

			string testDialogue = $"Dialogue: 0,0:00:30.11,0:00:31.83,Default,,0,0,0,,{expectedDialogueText}";

			SubtitleData? dialogueData = ASS.ReadDialogue(testDialogue, format);

			if(dialogueData == null)
			{
				Assert.Fail("dialogfueData was null");
			}

			Assert.That(dialogueData.startInMillis, Is.EqualTo(expectedStartTime));
			Assert.That(dialogueData.endInMillis, Is.EqualTo(expectedEndTime));
			Assert.That(dialogueData.subtitleContent, Is.EqualTo(expectedDialogueText));


		}

		[Test]
		public void ReadDialogueWithStylingTest()
		{
			ASSFormatContainer format = new()
			{
				startIndex = 2,
				endIndex = 3,
			};

			string actualDialogueText = "Ah. Got ya.";
			int actualStartTime = 30110;
			int actualEndTime = 31830;

			string testDialogue = $"Dialogue: 0,0:00:30.11,0:00:31.83,Default,,0,0,0,,{{\\rAlternate}}{actualDialogueText}";

			SubtitleData? dialogueData = ASS.ReadDialogue(testDialogue, format);

			if(dialogueData == null)
			{
				Assert.Fail("dialogfueData was null");
			}

			Assert.That(dialogueData.startInMillis, Is.EqualTo(actualStartTime));
			Assert.That(dialogueData.endInMillis, Is.EqualTo(actualEndTime));
			Assert.That(dialogueData.subtitleContent, Is.EqualTo(actualDialogueText));


		}

		[Test]
		public void CreateTimestampString()
		{
			int hours = 2;
			int minutes = 12;
			int seconds = 41;
			int milliseconds = 315;
			int timestampInMillis = (CommonUtils.hourInMillis * hours) + (CommonUtils.MinInMillis * minutes) + (CommonUtils.SecInMillis * seconds) + milliseconds;

			string expectedTimestampString = $"{hours}:{CommonUtils.GetTwoDigitStringFromInt(minutes)}:{CommonUtils.GetTwoDigitStringFromInt(seconds)}.{CommonUtils.GetTwoDigitStringFromInt(milliseconds / 10)}";

			string outputTimestampString = ASS.GetTimestampStringFromMillis(timestampInMillis);

			Assert.That(outputTimestampString, Is.EqualTo(expectedTimestampString));
			


		}
	}
}
