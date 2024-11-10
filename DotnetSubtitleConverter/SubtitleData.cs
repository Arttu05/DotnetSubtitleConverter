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
		public int startHour = 0;
		public int startMinute = 0;
		public int startSecond = 0;
		public int startMicrosecond;
		//end
		public int endHour = 0;
		public int endMinute = 0;
		public int endSecond = 0;
		public int endMicrosecond;
		//content
		public string subtitleContent;

	}
}
