using DotnetSubtitleConverter;
using DotnetSubtitleConverter.Subtitles;
using Tests;

namespace UnitTests
{
	internal class VTT_UnitTests
	{
		[OneTimeSetUp]
		public void setup()
		{
			Consts.CheckExampleFiles();
		}

		[Test]
		public void VTT_TimestampTest()
		{
			try
			{
				StreamReader sr = new StreamReader(Consts.VTT_EXAMPLE_FILE);
				sr.ReadLine();// WEBVTT
				sr.ReadLine();// Empty line

				string? expectedTimeString = sr.ReadLine();

				DotnetSubtitleConverter.Subtitles.VTT.ReadTimeString(expectedTimeString);
				sr.Close();
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}

		}

		[Test]
		public void VTT_LineTestWith2Lines()
		{
			string line1 = "test";
			string line2 = "test2";
			string line3 = "test3";

			string expectedString = $"{line1}\n{line2}";

			string lineToReadString = $"{line1}\n{line2}\n\n{line3}";

			Stream lineToReadStream = TestUtils.GetStreamFromString(lineToReadString);

			StreamReader sr = new StreamReader(lineToReadStream);

			string outputString = VTT.GetSubtitleContent(ref sr);

			Assert.That(outputString, Is.EqualTo(expectedString));

		}

		[Test]
		public void VTT_LineTestWith1Line()
		{
			string line1 = "test";
			string line2 = "test2";

			string expectedString = $"{line1}";

			string lineToReadString = $"{line1}\n\n{line2}\n";

			Stream lineToReadStream = TestUtils.GetStreamFromString(lineToReadString);

			StreamReader sr = new StreamReader(lineToReadStream);

			string outputString = VTT.GetSubtitleContent(ref sr);

			Assert.That(outputString, Is.EqualTo(expectedString));

		}

		[Test]
		public void VTT_LineTestWithEmptyLine()
		{
			string line = "test2";

			string lineToReadString = $"\n\n{line}\n";

			Stream lineToReadStream = TestUtils.GetStreamFromString(lineToReadString);

			StreamReader sr = new StreamReader(lineToReadStream);
			try
			{
				string outputString = VTT.GetSubtitleContent(ref sr);
				Assert.Fail("\"GetSubtitleContent\" should have thrown exception");
			}
			catch (InvalidSubtitleException)
			{ }
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}


		}


		[Test]
		public void VTT_LineWithTags()
		{
			string expectedLine = "some line";

			string lineToReadString = $"<i>{expectedLine}</i>\n\n";

			Stream lineToReadStream = TestUtils.GetStreamFromString(lineToReadString);

			StreamReader sr = new StreamReader(lineToReadStream);

			string outputString = VTT.GetSubtitleContent(ref sr);

			Assert.That(outputString, Is.EqualTo(expectedLine));

		}
	}
}
