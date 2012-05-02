using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpAssembler.OpcodeWriter.X86
{
	/// <inheritdoc />
	public class X86SpecFactory : SpecFactory
	{
		/// <inheritdoc />
		public override OpcodeSpec CreateOpcodeSpec()
		{
			return new X86OpcodeSpec();
		}

		/// <inheritdoc />
		public override OpcodeVariantSpec CreateOpcodeVariantSpec()
		{
			return new X86OpcodeVariantSpec();
		}

		/// <inheritdoc />
		public override OperandSpec CreateOperandSpec(string type, object defaultValue)
		{
			var operandSpec = new X86OperandSpec();

			if (type.Equals("void"))
			{
				operandSpec.Type = X86OperandType.FixedRegister;
				operandSpec.FixedRegister = ToRegister(defaultValue);
			}
			else
			{
				DataSize size;
				if (type.EndsWith("128"))
				{
					size = DataSize.Bit128;
					type = type.Substring(0, type.Length - 3);
				}
				else if (type.EndsWith("64"))
				{
					size = DataSize.Bit64;
					type = type.Substring(0, type.Length - 2);
				}
				else if (type.EndsWith("32"))
				{
					size = DataSize.Bit32;
					type = type.Substring(0, type.Length - 2);
				}
				else if (type.EndsWith("16"))
				{
					size = DataSize.Bit16;
					type = type.Substring(0, type.Length - 2);
				}
				else if (type.EndsWith("8"))
				{
					size = DataSize.Bit8;
					type = type.Substring(0, type.Length - 1);
				}
				else
					throw new ScriptException(String.Format("Malformatted type {0}", type));
				operandSpec.Size = size;

				switch (type)
				{
					case "reg/mem":
						operandSpec.Type = X86OperandType.RegisterOrMemoryOperand;
						break;

					case "reg":
						operandSpec.Type = X86OperandType.RegisterOperand;
						break;

					case "moffset":
						operandSpec.Type = X86OperandType.MemoryOffset;
						break;

					case "imm":
						operandSpec.Type = X86OperandType.Immediate;
						break;

					case "mem":
						operandSpec.Type = X86OperandType.MemoryOperand;
						break;

					case "reloff":
						operandSpec.Type = X86OperandType.RelativeOffset;
						break;

					case "pntr16:":
						operandSpec.Type = X86OperandType.FarPointer;
						break;

					default:
						throw new ScriptException(String.Format("Unknown operand type {0}", type));
				}
			}

			return operandSpec;
		}

		/// <summary>
		/// Converts a given identifier to a <see cref="Register"/>.
		/// </summary>
		/// <param name="identifier">The register identifier.</param>
		/// <returns>The resulting <see cref="Register"/>.</returns>
		private Register ToRegister(object identifier)
		{
			string id = identifier.ToString();
			Register reg;
			if (!Enum.TryParse<Register>(id, true, out reg))
				throw new ScriptException(String.Format("Unknown register {0}", id));
			return reg;
		}
	}
}
