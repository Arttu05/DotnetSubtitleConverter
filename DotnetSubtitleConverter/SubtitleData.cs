using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSubtitleConverter
{
	internal class SubtitleData
	{
		//start
		public required int startHour = 0;
		public required int startMinute = 0;
		public required int startSecond = 0;
		public required int startMicrosecond;
		//end
		public required int endHour = 0;
		public required int endMinute = 0;
		public required int endSecond = 0;
		public required int endMicrosecond;
		//content
		public required string subtitleContent;

	}
}
