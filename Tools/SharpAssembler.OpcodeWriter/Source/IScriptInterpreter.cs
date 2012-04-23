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
	public interface IScriptInterpreter
	{
		/// <summary>
		/// Reads the script from the specified file
		/// and returns the opcode specifications it contains.
		/// </summary>
		/// <param name="path">The path to the file.</param>
		/// <returns>An enumerable collection of <see cref="OpcodeSpec"/> objects.</returns>
		IEnumerable<OpcodeSpec> ReadFrom(string path);

		/// <summary>
		/// Reads the script from the specified <see cref="Stream"/>
		/// and returns the opcode specifications it contains.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/>.</param>
		/// <returns>An enumerable collection of <see cref="OpcodeSpec"/> objects.</returns>
		IEnumerable<OpcodeSpec> ReadFrom(Stream stream);

		/// <summary>
		/// Reads the script from the specified <see cref="TextReader"/>
		/// and returns the opcode specifications it contains.
		/// </summary>
		/// <param name="reader">The <see cref="TextReader"/>.</param>
		/// <returns>An enumerable collection of <see cref="OpcodeSpec"/> objects.</returns>
		IEnumerable<OpcodeSpec> ReadFrom(TextReader reader);

		/// <summary>
		/// Reads the specified script.
		/// </summary>
		/// <param name="script">The script to read.</param>
		/// <returns>An enumerable collection of <see cref="OpcodeSpec"/> objects.</returns>
		IEnumerable<OpcodeSpec> Read(string script);
	}

	#region Contract
	namespace Contracts
	{
		[ContractClassFor(typeof(IScriptInterpreter))]
		abstract class IScriptReaderContract : IScriptInterpreter
		{
			public IEnumerable<OpcodeSpec> ReadFrom(string path)
			{
				Contract.Requires<ArgumentNullException>(path != null);
				Contract.Ensures(Contract.Result<IEnumerable<OpcodeSpec>>() != null);

				return default(IEnumerable<OpcodeSpec>);
			}

			public IEnumerable<OpcodeSpec> ReadFrom(Stream stream)
			{
				Contract.Requires<ArgumentNullException>(stream != null);
				Contract.Requires<ArgumentException>(stream.CanRead);
				Contract.Ensures(Contract.Result<IEnumerable<OpcodeSpec>>() != null);

				return default(IEnumerable<OpcodeSpec>);
			}

			public IEnumerable<OpcodeSpec> ReadFrom(TextReader reader)
			{
				Contract.Requires<ArgumentNullException>(reader != null);
				Contract.Ensures(Contract.Result<IEnumerable<OpcodeSpec>>() != null);

				return default(IEnumerable<OpcodeSpec>);
			}

			public IEnumerable<OpcodeSpec> Read(string script)
			{
				Contract.Requires<ArgumentNullException>(script != null);
				Contract.Ensures(Contract.Result<IEnumerable<OpcodeSpec>>() != null);

				return default(IEnumerable<OpcodeSpec>);
			}
		}
	}
	#endregion
}
