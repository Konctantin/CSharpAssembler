using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpAssembler.Formats.Bin;
using SharpAssembler.Architectures.X86;
using System.IO;

namespace SharpAssembler.Tests
{
	/// <summary>
	/// Base class for test fixtures that require and use object files.
	/// </summary>
	public abstract class ObjectFileTestBase
	{
		/// <summary>
		/// Creates a new object file for use in tests.
		/// </summary>
		/// <returns>The object file that was created.</returns>
		/// <remarks>
		/// The default implementation creates a BIN x86-64 object file.
		/// </remarks>
		protected virtual ObjectFile CreateObjectFile()
		{
			var format = new BinObjectFileFormat();
			var architecture = new X86Architecture();
			var objectFile = new ObjectFile(format, architecture);

			return objectFile;
		}

		/// <summary>
		/// Assembles the specified object file into its binary form.
		/// </summary>
		/// <param name="objectFile">The object file to assemble.</param>
		/// <returns>The bytes that result from assembling the object file.</returns>
		protected virtual byte[] Assemble(ObjectFile objectFile)
		{
			byte[] data;
			var assembler = objectFile.Format.CreateAssembler(objectFile);
			using (MemoryStream ms = new MemoryStream())
			using (BinaryWriter writer = new BinaryWriter(ms))
			{
				assembler.Assemble(writer);
				data = ms.ToArray();
			}
			return data;
		}

		/// <summary>
		/// Returns a subarray from the specified array.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="startIndex">The zero-based start index.</param>
		/// <param name="count">The number of bytes.</param>
		/// <returns>The sub array.</returns>
		protected byte[] Subarray(byte[] array, int startIndex, int count)
		{
			byte[] subarray = new byte[count];
			Array.Copy(array, startIndex, subarray, 0, count);
			return subarray;
		}
	}
}
