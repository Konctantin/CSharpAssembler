using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using System.Globalization;

namespace SharpAssembler.OpcodeWriter
{
	/// <summary>
	/// Reads a test script.
	/// </summary>
	public class ScriptInterpreter : IScriptInterpreter
	{
		#region Constructors
		///// <summary>
		///// Initializes a new instance of the <see cref="ScriptInterpreter"/> class.
		///// </summary>
		//public ScriptInterpreter()
		//    :this(new ScriptTokenizer(), new SpecFactory())
		//{ /* Nothing to do. */ }

		/// <summary>
		/// Initializes a new instance of the <see cref="ScriptInterpreter"/> class.
		/// </summary>
		/// <param name="tokenizer">The tokenizer to use.</param>
		/// <param name="factoryDispenser">The factory dispenser to use.</param>
		public ScriptInterpreter(ScriptTokenizer tokenizer, SpecFactoryDispenser factoryDispenser)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(tokenizer != null);
			Contract.Requires<ArgumentNullException>(factoryDispenser != null);
			#endregion

			this.tokenizer = tokenizer;
			this.factoryDispenser = factoryDispenser;
		}
		#endregion

		/// <inheritdoc />
		public IEnumerable<OpcodeSpec> ReadFrom(string path)
		{
			// CONTRACT: IScriptReader

			using (var stream = File.OpenRead(path))
			{
				return ReadFrom(stream);
			}
		}

		/// <inheritdoc />
		public IEnumerable<OpcodeSpec> ReadFrom(Stream stream)
		{
			// CONTRACT: IScriptReader

			// TODO: Ensure that 'stream' is not closed afterwards.
			using (var reader = new StreamReader(stream))
			{
				return ReadFrom(reader);
			}
		}

		/// <inheritdoc />
		public IEnumerable<OpcodeSpec> ReadFrom(TextReader reader)
		{
			// CONTRACT: IScriptReader

			return Read(reader.ReadToEnd());
		}

		/// <inheritdoc />
		public IEnumerable<OpcodeSpec> Read(string script)
		{
			// CONTRACT: IScriptReader

			this.state = ReaderState.Initial;
			this.opcodespecs = new List<OpcodeSpec>();
			this.reader = new ScriptReader(this.tokenizer.Tokenize(script));
			Execute();
			return opcodespecs;
		}

		/// <summary>
		/// The tokenizer to use.
		/// </summary>
		private ScriptTokenizer tokenizer;

		/// <summary>
		/// The current reader state.
		/// </summary>
		private ReaderState state;

		/// <summary>
		/// A list to which all read <see cref="OpcodeSpec"/> objects are added.
		/// </summary>
		private List<OpcodeSpec> opcodespecs;

		/// <summary>
		/// A script reader.
		/// </summary>
		private ScriptReader reader;

		/// <summary>
		/// The <see cref="SpecFactoryDispenser"/>.
		/// </summary>
		private readonly SpecFactoryDispenser factoryDispenser;

		/// <summary>
		/// The current <see cref="SpecFactory"/>.
		/// </summary>
		private SpecFactory factory;

		
		/// <summary>
		/// Executes the state machine.
		/// </summary>
		private void Execute()
		{
			while (this.state != ReaderState.Finished)
			{
				switch (this.state)
				{
					case ReaderState.Initial:
						this.state = ReaderState.ObjectDefinition;
						break;
					case ReaderState.ObjectDefinition:
						ReadObjectDefinition();
						if (this.reader.Peek() == null)
							this.state = ReaderState.Finished;
						break;
					default:
						throw new NotImplementedException("In an unexpected state.");
				}
			}
		}

		/// <summary>
		/// Reads an object definition from the file.
		/// </summary>
		private void ReadObjectDefinition()
		{
			string keyword = this.reader.Read();
			switch(keyword)
			{
				case "opcode":
					ReadOpcodeDefinition();
					break;
				default:
					throw new ScriptException(String.Format("Unknown keyword {0}, expected one of: opcode", keyword));
			}
		}

		/// <summary>
		/// Sets the currently used <see cref="SpecFactory"/> to the correct implementation
		/// based on the platform ID.
		/// </summary>
		/// <param name="platform">The platform ID.</param>
		private void SetFactoryForPlatform(string platform)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(platform != null);
			#endregion
			this.factory = this.factoryDispenser.Get(platform);
		}

		/// <summary>
		/// Reads an opcode definition from the file.
		/// </summary>
		private void ReadOpcodeDefinition()
		{
			string platform = this.reader.ReadIdentifier();
			SetFactoryForPlatform(platform);
			var opcodeSpec = this.factory.CreateOpcodeSpec();
			this.opcodespecs.Add(opcodeSpec);
			opcodeSpec.Mnemonic = this.reader.ReadIdentifier();

			this.reader.ReadRegionAndRepeat(ScriptReader.RegionType.Body, () =>
			{
				string keyword = this.reader.Peek();
				switch (keyword)
				{
					case "set":
						ReadPropertySetting(opcodeSpec);
						break;
					case "var":
						ReadOpcodeVariant(opcodeSpec);
						break;
					default:
						throw new ScriptException(String.Format("Unknown keyword {0}, expected one of: set, var", keyword));
				}
			});
		}

		#region Property Setting
		/// <summary>
		/// Reads a property setting.
		/// </summary>
		/// <param name="target">The object on which the property is set.</param>
		private void ReadPropertySetting(object target)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(target != null);
			#endregion

			string keyword = this.reader.Read();
			if (!keyword.Equals("set"))
				throw new ScriptException(String.Format("Unknown keyword {0}, expected one of: set", keyword));
			string propertyName = this.reader.ReadIdentifier();
			string equalsSign = this.reader.Read();
			if (!equalsSign.Equals("="))
				throw new ScriptException(String.Format("Expected '=' operator, got {0}.", equalsSign));
			object value = ReadAnyValue();
			string terminator = this.reader.Read();
			if (!terminator.Equals(";"))
				throw new ScriptException(String.Format("Expected ';' terminator, got {0}.", terminator));

			SetProperty(target, propertyName, value);
		}

		/// <summary>
		/// Sets a property on the specified object.
		/// </summary>
		/// <param name="target">The target object.</param>
		/// <param name="name">The name of the property.</param>
		/// <param name="value">The value of the property.</param>
		private void SetProperty(object target, string name, object value)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(target != null);
			Contract.Requires<ArgumentException>(!String.IsNullOrWhiteSpace(name));
			#endregion
			var type = target.GetType();
			var property = type.GetProperty(name);

			var propertyType = property.PropertyType;
			if (propertyType.Equals(typeof(Byte)))
				value = Convert.ToByte(value);
			if (propertyType.Equals(typeof(SByte)))
				value = Convert.ToSByte(value);
			if (propertyType.Equals(typeof(Int16)))
				value = Convert.ToInt16(value);
			if (propertyType.Equals(typeof(UInt16)))
				value = Convert.ToUInt16(value);
			if (propertyType.Equals(typeof(Int32)))
				value = Convert.ToInt32(value);
			if (propertyType.Equals(typeof(UInt32)))
				value = Convert.ToUInt32(value);
			if (propertyType.Equals(typeof(Int64)))
				value = Convert.ToInt64(value);
			if (propertyType.Equals(typeof(UInt64)))
				value = Convert.ToUInt64(value);

			property.SetValue(target, value, null);
		}
		#endregion

		/// <summary>
		/// Reads an opcode variant.
		/// </summary>
		private void ReadOpcodeVariant(OpcodeSpec opcodeSpec)
		{
			string keyword = this.reader.Read();
			if (!keyword.Equals("var"))
				throw new ScriptException(String.Format("Unknown keyword {0}, expected one of: var", keyword));

			var opcodeVariantSpec = this.factory.CreateOpcodeVariantSpec();
			opcodeSpec.Variants.Add(opcodeVariantSpec);

			string opcodeBytesStr = this.reader.ReadIdentifier();
			opcodeVariantSpec.OpcodeBytes = ReadByteArray(opcodeBytesStr);

			this.reader.ReadListInRegion(ScriptReader.RegionType.Parentheses, ",", () =>
			{
				string operandType = this.reader.ReadIdentifier();
				string operandName = this.reader.ReadIdentifier();
				object defaultValue = null;

				if (this.reader.Peek().Equals("="))
				{
					this.reader.Read();
					defaultValue = ReadAnyValue();
				}

				var operandSpec = this.factory.CreateOperandSpec(operandType, defaultValue);
				opcodeVariantSpec.Operands.Add(operandSpec);
				operandSpec.Name = operandName;
			});

			this.reader.ReadRegionAndRepeat(ScriptReader.RegionType.Body, () =>
			{
				keyword = this.reader.Peek();
				switch (keyword)
				{
					case "set":
						ReadPropertySetting(opcodeVariantSpec);
						break;
					default:
						throw new ScriptException(String.Format("Unknown keyword {0}, expected one of: set", keyword));
				}
			});
		}

		/// <summary>
		/// Reads an array of bytes.
		/// </summary>
		/// <param name="str">The input string.</param>
		/// <returns>The array of bytes.</returns>
		private static byte[] ReadByteArray(string str)
		{
			var byteStrings = str.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
			byte[] bytes = new byte[byteStrings.Length];
			int index = 0;
			foreach (var s in byteStrings)
			{
				int value;
				if (!Int32.TryParse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
				{
					throw new ScriptException("Malformed opcode variant identifier. Expected a space-separated list " +
						"of hexadecimal byte values.");
				}
				bytes[index++] = (byte)value;
			}
			return bytes;
		}

		/// <summary>
		/// Reads a value of any type.
		/// </summary>
		/// <returns>The read value.</returns>
		private object ReadAnyValue()
		{
			string keyword = this.reader.Peek();
			switch(keyword)
			{
				case "true": this.reader.Read(); return true;
				case "false": this.reader.Read(); return false;
			}

			if (this.reader.PeekString() != null)
				return this.reader.ReadString();

			if (this.reader.PeekInteger().HasValue)
				return this.reader.ReadInteger();

			if (this.reader.PeekIdentifier() != null)
				return (Identifier)this.reader.ReadIdentifier();

			throw new ScriptException("Could not determine type of value.");
		}
	}
}
