using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpAssembler.Tests.Stubs
{
	public class ObjectFileFormatStub : IObjectFileFormat
	{
		public string Name
		{
			get { return "stub"; }
		}

		private SectionFactory sectionFactory = new SectionFactory();
		public SectionFactory SectionFactory
		{
			get { return sectionFactory; }
		}

		public bool IsSupportedArchitecture(IArchitecture architecture)
		{
			return true;
		}

		public bool SupportsFeature(ObjectFileFeature feature)
		{
			return true;
		}

		public ObjectFile CreateObjectFile(IArchitecture architecture)
		{
			return new ObjectFile(this, architecture, null);
		}

		public ObjectFile CreateObjectFile(IArchitecture architecture, string name)
		{
			return new ObjectFile(this, architecture, name);
		}

		public ObjectFileAssembler CreateAssembler(ObjectFile objectFile)
		{
			throw new NotImplementedException();
		}
	}
}
