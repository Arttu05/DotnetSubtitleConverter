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

	}
}
