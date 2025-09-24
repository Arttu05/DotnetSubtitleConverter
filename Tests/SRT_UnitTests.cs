using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
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

	}
}
