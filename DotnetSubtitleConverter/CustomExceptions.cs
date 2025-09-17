using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSubtitleConverter
{
	public class InvalidSubtitleException : Exception
	{
		public InvalidSubtitleException() 
		{ }

		public InvalidSubtitleException(string message) 
			: base(message) 
		{ }

		public InvalidSubtitleException (string message, Exception innerException) 
			:base(message,innerException)
		{ }

	}

	public class OffsetOverFlowException : Exception
	{
		public OffsetOverFlowException()
		{ }

		public OffsetOverFlowException(string message)
			: base(message)
		{ }

		public OffsetOverFlowException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}
