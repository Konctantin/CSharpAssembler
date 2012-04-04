#region Copyright and License
/*
 * SharpAssembler
 * Library for .NET that assembles a predetermined list of
 * instructions into machine code.
 * 
 * Copyright (C) 2011-2012 Daniël Pelsmaeker
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
using SharpAssembler;

namespace SharpAssembler.Architectures.X86.Tests
{
	/// <summary>
	/// Base class for assembler examples.
	/// </summary>
	public abstract class ExampleBase
	{
		/// <summary>
		/// Assembles the specified <see cref="ObjectFile"/> and returns the resulting object file as an array of
		/// bytes.
		/// </summary>
		/// <param name="objectFile">The <see cref="ObjectFile"/> to assemble.</param>
		/// <returns>The resulting object file as an array of bytes.</returns>
		protected byte[] Assemble(ObjectFile objectFile)
		{
			byte[] result = null;
			using(MemoryStream ms = new MemoryStream())
			{
				using (BinaryWriter writer = new BinaryWriter(ms))
				{
					objectFile.Format.CreateAssembler(objectFile).Assemble(writer);
					writer.Flush();
					result = ms.ToArray();
				}
			}
			return result;
		}

		/// <summary>
		/// Assembles the specified <see cref="ObjectFile"/>.
		/// </summary>
		/// <param name="objectFile">The <see cref="ObjectFile"/> to assemble.</param>
		/// <param name="filename">The name of the file to assemble to.</param>
		protected void AssembleToFile(ObjectFile objectFile, string filename)
		{
			using (FileStream fs = File.Create(filename))
			{
				using (BinaryWriter writer = new BinaryWriter(fs))
				{
					objectFile.Format.CreateAssembler(objectFile).Assemble(writer);
					writer.Flush();
				}
			}
		}
	}
}
