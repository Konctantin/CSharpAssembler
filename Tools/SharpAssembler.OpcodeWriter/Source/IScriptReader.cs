using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;

namespace SharpAssembler.OpcodeWriter
{
	/// <summary>
	/// Reads scripts.
	/// </summary>
	[ContractClass(typeof(Contracts.IScriptReaderContract))]
	public interface IScriptReader
	{
		/// <summary>
		/// Reads the script from the specified file
		/// and returns the opcode specifications it contains.
		/// </summary>
		/// <param name="path">The path to the file.</param>
		/// <returns>An enumerable collection of <see cref="OpcodeSpec"/> objects.</returns>
		IEnumerable<OpcodeSpec> Read(string path);

		/// <summary>
		/// Reads the script from the specified <see cref="Stream"/>
		/// and returns the opcode specifications it contains.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/>.</param>
		/// <returns>An enumerable collection of <see cref="OpcodeSpec"/> objects.</returns>
		IEnumerable<OpcodeSpec> Read(Stream stream);

		/// <summary>
		/// Reads the file from the specified <see cref="TextReader"/>
		/// and returns the opcode specifications it contains.
		/// </summary>
		/// <param name="reader">The <see cref="TextReader"/>.</param>
		/// <returns>An enumerable collection of <see cref="OpcodeSpec"/> objects.</returns>
		IEnumerable<OpcodeSpec> Read(TextReader reader);
	}

	#region Contract
	namespace Contracts
	{
		[ContractClassFor(typeof(IScriptReader))]
		abstract class IScriptReaderContract : IScriptReader
		{
			public IEnumerable<OpcodeSpec> Read(string path)
			{
				Contract.Requires<ArgumentNullException>(path != null);
				Contract.Ensures(Contract.Result<IEnumerable<OpcodeSpec>>() != null);

				return default(IEnumerable<OpcodeSpec>);
			}

			public IEnumerable<OpcodeSpec> Read(Stream stream)
			{
				Contract.Requires<ArgumentNullException>(stream != null);
				Contract.Requires<ArgumentException>(stream.CanRead);
				Contract.Ensures(Contract.Result<IEnumerable<OpcodeSpec>>() != null);

				return default(IEnumerable<OpcodeSpec>);
			}

			public IEnumerable<OpcodeSpec> Read(TextReader reader)
			{
				Contract.Requires<ArgumentNullException>(reader != null);
				Contract.Ensures(Contract.Result<IEnumerable<OpcodeSpec>>() != null);

				return default(IEnumerable<OpcodeSpec>);
			}
		}
	}
	#endregion
}
