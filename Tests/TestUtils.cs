using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
	internal static class TestUtils
	{

		public static Stream GetStreamFromString(string stringToStream)
		{
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(stringToStream);
			
			writer.Flush();
			stream.Position = 0;
			
			return stream;

		}

	}
}
