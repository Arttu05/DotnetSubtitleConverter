using DotnetSubtitleConverter;
using DotnetSubtitleConverter.Subtitles;
using Tests;

namespace UnitTests
{
	internal class SRT_UnitTests
	{
		[OneTimeSetUp]
		public void setup()
		{
			Consts.CheckExampleFiles();
		}

		[Test]
		public void SRT_TimestampTest()
		{
			try
			{
				StreamReader sr = new StreamReader(Consts.SRT_EXAMPLE_FILE);
				sr.ReadLine();// skips num

				DotnetSubtitleConverter.Subtitles.SRT.ReadTimeString(ref sr);
				sr.Close();
			}
			catch (Exception ex) 
			{ 
				Assert.Fail(ex.Message); 
			}

		}

		[Test]
		public void SRT_LineTestWith2Lines()
		{
			string line1 = "test";
			string line2 = "test2";
			string line3 = "test3";

			string expectedString = $"{line1}\n{line2}";

			string lineToReadString = $"{line1}\n{line2}\n\n{line3}";

			Stream lineToReadStream = TestUtils.GetStreamFromString(lineToReadString);

			StreamReader sr = new StreamReader(lineToReadStream);

			string outputString = SRT.GetSubtitleContent(ref sr);

			Assert.That(outputString,Is.EqualTo(expectedString));

		}

		[Test]
		public void SRT_LineTestWith1Line()
		{
			string line1 = "test";
			string line2 = "test2";

			string expectedString = $"{line1}";

			string lineToReadString = $"{line1}\n\n{line2}\n";

			Stream lineToReadStream = TestUtils.GetStreamFromString(lineToReadString);

			StreamReader sr = new StreamReader(lineToReadStream);

			string outputString = SRT.GetSubtitleContent(ref sr);

			Assert.That(outputString,Is.EqualTo(expectedString));

		}

		[Test]
		public void SRT_LineTestWithEmptyLine()
		{
			string line = "test2";

			string lineToReadString = $"\n\n{line}\n";

			Stream lineToReadStream = TestUtils.GetStreamFromString(lineToReadString);

			StreamReader sr = new StreamReader(lineToReadStream);
			try
			{
				string outputString = SRT.GetSubtitleContent(ref sr);
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
		public void SRT_RemoveStylingFromLine()
		{
			string expectedString = "test2";

			string lineToReadString = $"<i>{{\\an8}}{expectedString}</i>\n\n";

			Stream lineToReadStream = TestUtils.GetStreamFromString(lineToReadString);

			StreamReader sr = new StreamReader(lineToReadStream);
			string outputString = SRT.GetSubtitleContent(ref sr);
			Assert.That(outputString, Is.EqualTo(expectedString));


		}


		[Test]
		public void SRT_ReadNumWithValidTest()
		{
			Assert.That(SRT.ReadNum("2"),Is.EqualTo(true));
		}
		[Test]
		public void SRT_ReadNumWithInValidTest()
		{
			Assert.That(SRT.ReadNum("k"),Is.EqualTo(false));
		}
		[Test]
		public void SRT_ReadNumWithNullTest()
		{
			Assert.That(SRT.ReadNum(null),Is.EqualTo(false));
		}
	}
}
