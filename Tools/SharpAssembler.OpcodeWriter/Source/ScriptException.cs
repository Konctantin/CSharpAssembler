﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpAssembler.OpcodeWriter
{
	[Serializable]	
	public class ScriptException : Exception
	{
		public ScriptException() { }
		public ScriptException(string message) : base(message) { }
		public ScriptException(string message, Exception inner) : base(message, inner) { }
		protected ScriptException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
