using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSubtitleConverter
{
	internal class SubtitleData
	{

		public required DateTime time;
		public required string subtitleContent;
		public int subNum; //number above the timestamp in SRT subtitles

	}
}
