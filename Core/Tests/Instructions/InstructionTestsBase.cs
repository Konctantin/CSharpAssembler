#region Copyright and License
/*
 * SharpAssembler
 * Library for .NET that assembles a predetermined list of
 * instructions into machine code.
 * 
 * Copyright (C) 2011 Daniël Pelsmaeker
 * 
 * This file is part of SharpAssembler.
 * 
 * SharpAssembler is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SharpAssembler is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SharpAssembler.  If not, see <http://www.gnu.org/licenses/>.
 */
#endregion
using System.IO;
using NUnit.Framework;

namespace SharpAssembler.Core.Tests.Instructions
{
	/// <summary>
	/// Base class for <see cref="Constructable"/> tests.
	/// </summary>
	public class InstructionTestsBase
	{
		private ObjectFileMock objectFile;
		private Section section;

		/// <summary>
		/// Setups the text fixture.
		/// </summary>
		[TestFixtureSetUp]
		public void Setup()
		{
			this.objectFile = new ObjectFileMock();
			this.section = new Section("test");
			this.objectFile.Add(this.section);

			this.context = new Context(objectFile);
			this.context.Section = section;
			this.stream = new MemoryStream(15);
			this.writer = new BinaryWriter(stream);
		}

		private Context context;
		/// <summary>
		/// Gets the <see cref="Context"/>.
		/// </summary>
		/// <value>A <see cref="Context"/>.</value>
		public Context Context
		{
			get { return context; }
		}

		private MemoryStream stream;
		/// <summary>
		/// Gets the <see cref="MemoryStream"/> to which instructions can be emitted.
		/// </summary>
		/// <value>A <see cref="MemoryStream"/>.</value>
		public MemoryStream Stream
		{
			get { return stream; }
		}

		private BinaryWriter writer;
		/// <summary>
		/// Gets the <see cref="BinaryWriter"/> to which instructions can be emitted.
		/// </summary>
		/// <value>A <see cref="BinaryWriter"/>.</value>
		public BinaryWriter Writer
		{
			get { return writer; }
		}

		/// <summary>
		/// Resets the <see cref="Writer"/> and <see cref="Stream"/>.
		/// </summary>
		protected void ResetWriter()
		{
			this.stream.SetLength(0);
		}
	}
}
