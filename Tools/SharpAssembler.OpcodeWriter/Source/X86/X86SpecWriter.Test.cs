using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;
using System.Diagnostics;
using System.ComponentModel;

namespace SharpAssembler.OpcodeWriter.X86
{
	partial class X86SpecWriter
	{
		/// <summary>
		/// The path to the YASM executable.
		/// </summary>
		private string yasmExecutablePath;

		/// <inheritdoc />
		protected override void WriteTestUsingDirectives(TextWriter writer)
		{
			// CONTRACT: SpecWriter

			base.WriteTestUsingDirectives(writer);
			writer.WriteLine("using SharpAssembler.Architectures.X86.Operands;");
		}

		/// <inheritdoc />
		protected override void WriteTestClassTests(OpcodeSpec spec, TextWriter writer)
		{
			// CONTRACT: SpecWriter
			var x86spec = (X86OpcodeSpec)spec;

			if (spec.Variants.Any())
			{
				WriteTest(x86spec, (X86OpcodeVariantSpec)spec.Variants.First(), writer);
				foreach (var variant in spec.Variants.Skip(1).Cast<X86OpcodeVariantSpec>())
				{
					writer.WriteLine();
					WriteTest(x86spec, variant, writer);
				}
			}
		}

		/// <summary>
		/// Writes a single test for a specific opcode variant.
		/// </summary>
		/// <param name="spec">The opcode spec.</param>
		/// <param name="variant">The opcode variant.</param>
		/// <param name="writer">The text writer to write to.</param>
		private void WriteTest(X86OpcodeSpec spec, X86OpcodeVariantSpec variant, TextWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(spec != null);
			Contract.Requires<ArgumentNullException>(variant != null);
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			var operandStrings = from o in variant.Operands.Cast<X86OperandSpec>()
								 select GetOperandStrings(o.Type, o.Size, o.FixedRegister, GetRandom(variant));
			string operands = String.Join(", ", from o in operandStrings select o.Item1);

			string instruction = spec.Mnemonic.ToUpperInvariant();
			if (operandStrings.Any())
				instruction += " " + String.Join(", ", from o in operandStrings select o.Item2);

			string feedback16;
			byte[] bytes16 = GetEncodedInstruction(DataSize.Bit16, instruction, out feedback16);
			string feedback32;
			byte[] bytes32 = GetEncodedInstruction(DataSize.Bit32, instruction, out feedback32);
			string feedback64;
			byte[] bytes64 = GetEncodedInstruction(DataSize.Bit64, instruction, out feedback64);

			string methodName = spec.Mnemonic.ToUpperInvariant();
			if (variant.Operands.Any())
			{
				methodName += "_" + String.Join("_", from o in variant.Operands.Cast<X86OperandSpec>()
													 select IdentifierValidation.Replace(GetOperandManualName(o), ""));
			}

			writer.WriteLine(T + T + "[Test]");
			writer.WriteLine(T + T + "public void {0}()", AsValidIdentifier(methodName));
			writer.WriteLine(T + T + "{");
			writer.WriteLine(T + T + T + "var instruction = Instr.{0}({1});",
				AsValidIdentifier(spec.Name), operands);
			writer.WriteLine();
			writer.WriteLine(T + T + T + "// " + instruction);
			if (bytes16 != null)
				writer.WriteLine(T + T + T + "AssertInstruction(instruction, DataSize.Bit16, new byte[] { " +
					String.Join(", ", from b in bytes16 select String.Format("0x{0:X2}", b)) + " });");
			else
				writer.WriteLine(T + T + T + "AssertInstructionFail(instruction, DataSize.Bit16);");
			if (bytes32 != null)
				writer.WriteLine(T + T + T + "AssertInstruction(instruction, DataSize.Bit32, new byte[] { " +
					String.Join(", ", from b in bytes32 select String.Format("0x{0:X2}", b)) + " });");
			else
				writer.WriteLine(T + T + T + "AssertInstructionFail(instruction, DataSize.Bit32);");
			if (bytes64 != null)
				writer.WriteLine(T + T + T + "AssertInstruction(instruction, DataSize.Bit64, new byte[] { " +
					String.Join(", ", from b in bytes64 select String.Format("0x{0:X2}", b)) + " });");
			else
				writer.WriteLine(T + T + T + "AssertInstructionFail(instruction, DataSize.Bit64);");

			if (bytes16 == null && bytes32 == null && bytes64 == null)
			{

				throw new ScriptException(String.Format("Assembling {0} for the tests failed with the following messages:\n" +
					"16-bit: {1}\n32-bit: {2}\n64-bit: {3}",
					instruction, ProcessFeedback(feedback16), ProcessFeedback(feedback32), ProcessFeedback(feedback64)));
			}

			writer.WriteLine(T + T + "}");
		}

		/// <summary>
		/// Removes some irrelevant information.
		/// </summary>
		/// <param name="fb"></param>
		/// <returns></returns>
		private string ProcessFeedback(string fb)
		{
			string errString = "error: ";
			int from = fb.IndexOf(errString);
			if (from < 0)
				return fb.Trim();
			else
				return fb.Substring(from + errString.Length).Trim();
		}

#if false
		/// <summary>
		/// Gets the strings used for the operand.
		/// </summary>
		/// <param name="spec">The opcode spec.</param>
		/// <param name="variant">The opcode variant.</param>
		/// <param name="operand">The operand.</param>
		/// <returns>A tuple with the C# code for the operand, followed by the YASM assembler code.</returns>
		private Tuple<string, string> GetOperandStrings(X86OpcodeSpec spec, X86OpcodeVariantSpec variant, X86OperandSpec operand)
		{
			Random rand = GetRandom(variant);

			switch (operand.Type)
			{
				case X86OperandType.Immediate:
					if (operand.Size == DataSize.Bit8)
					{
						byte value = (byte)rand.Next(0, 0x100);
						return new Tuple<string, string>(
							String.Format("(byte)0x{0:X2}", value),
							String.Format("BYTE 0x{0:X2}", value));
					}
					else if (operand.Size == DataSize.Bit16)
					{
						ushort value = (ushort)rand.Next(0x100, 0x10000);
						return new Tuple<string, string>(
							String.Format("(ushort)0x{0:X}", value),
							String.Format("WORD 0x{0:X}", value));
					}
					else if (operand.Size == DataSize.Bit32)
					{
						uint value = (uint)rand.Next(0x10000);
						return new Tuple<string, string>(
							String.Format("(uint)0x{0:X}", value),
							String.Format("DWORD 0x{0:X}", value));
					}
					else if (operand.Size == DataSize.Bit64)
					{
						var x = (ulong)rand.Next(0x10000);
						var y = (ulong)rand.Next(0x10000);
						ulong value = x | (y << 32);
						return new Tuple<string, string>(
							String.Format("(ulong)0x{0:X}", value),
							String.Format("QWORD 0x{0:X}", value));
					}
					else
						throw new NotSupportedException("The operand size is not supported.");
				case X86OperandType.FixedRegister:
					{
						string name = Enum.GetName(typeof(Register), operand.FixedRegister);
						return new Tuple<string, string>(
								String.Format("Register.{0}", name),
								name.ToLowerInvariant());
					}
				case X86OperandType.MemoryOffset:
					// TODO:
					return new Tuple<string, string>("new MemoryOffset()", "0");
				case X86OperandType.FarPointer:
					{
						if (operand.Size == DataSize.Bit16)
						{
							ushort selector = (ushort)rand.Next(0x100, 0x10000);
							ushort offset = (ushort)rand.Next(0x100, 0x10000);
							return new Tuple<string, string>(
								String.Format("new FarPointer(c => 0x{0:X}, c => 0x{1:X}, DataSize.Bit16)", selector, offset),
								String.Format("WORD 0x{0:X}:0x{1:X}", selector, offset));
						}
						else if (operand.Size == DataSize.Bit32)
						{
							uint selector = (uint)rand.Next(0x10000);
							uint offset = (uint)rand.Next(0x10000);
							return new Tuple<string, string>(
								String.Format("new FarPointer(c => 0x{0:X}, c => 0x{1:X}, DataSize.Bit32)", selector, offset),
								String.Format("DWORD 0x{0:X}:0x{1:X}", selector, offset));
						}
						else
							throw new NotImplementedException();
					}
				case X86OperandType.MemoryOperand:
				case X86OperandType.RegisterOrMemoryOperand:
					{
						ushort value = (ushort)rand.Next(0x100, 0x10000);
						return new Tuple<string, string>(
							String.Format("new EffectiveAddress(DataSize.Bit{1}, DataSize.None, c => new ReferenceOffset(0x{0:X}))",
								value, operand.Size.GetBitCount()),
							String.Format("{0} [0x{1:X}]", GetNasmSizeSpecifier(operand.Size), value));
					}
				case X86OperandType.RelativeOffset:
					if (operand.Size == DataSize.Bit8)
					{
						byte value = (byte)rand.Next(0, 0x100);
						return new Tuple<string, string>(
							String.Format("new RelativeOffset(c => 0x{0:X}, DataSize.Bit8)", value),
							String.Format("BYTE 0x{0:X}", value));
					}
					else if (operand.Size == DataSize.Bit16)
					{
						ushort value = (ushort)rand.Next(0x100, 0x10000);
						return new Tuple<string, string>(
							String.Format("new RelativeOffset(c => 0x{0:X}, DataSize.Bit16)", value),
							String.Format("WORD 0x{0:X}", value));
					}
					else if (operand.Size == DataSize.Bit32)
					{
						uint value = (uint)rand.Next(0x10000);
						return new Tuple<string, string>(
							String.Format("new RelativeOffset(c => 0x{0:X}, DataSize.Bit32)", value),
							String.Format("DWORD 0x{0:X}", value));
					}
					else if (operand.Size == DataSize.Bit64)
					{
						var x = (ulong)rand.Next(0x10000);
						var y = (ulong)rand.Next(0x10000);
						ulong value = x | (y << 32);
						return new Tuple<string, string>(
							String.Format("new RelativeOffset(c => 0x{0:X}, DataSize.Bit64)", value),
							String.Format("QWORD 0x{0:X}", value));
					}
					else
						throw new NotSupportedException("The operand size is not supported.");
				case X86OperandType.RegisterOperand:
					return RandomGPRegister(operand.Size, rand);
				default:
					throw new NotSupportedException("The operand type is not supported.");
			}
		}
#endif

		/// <summary>
		/// Gets the strings used for the operand.
		/// </summary>
		/// <param name="spec">The opcode spec.</param>
		/// <param name="variant">The opcode variant.</param>
		/// <param name="operand">The operand.</param>
		/// <returns>A tuple with the C# code for the operand, followed by the YASM assembler code.</returns>
		private Tuple<string, string> GetOperandStrings(X86OperandType operandType, DataSize operandSize, Register fixedRegister, Random rand)
		{
			switch (operandType)
			{
				case X86OperandType.Immediate:
					if (operandSize == DataSize.Bit8)
					{
						byte value = (byte)rand.Next(0, 0x100);
						return new Tuple<string, string>(
							String.Format("(byte)0x{0:X2}", value),
							String.Format("BYTE 0x{0:X2}", value));
					}
					else if (operandSize == DataSize.Bit16)
					{
						ushort value = (ushort)rand.Next(0x100, 0x10000);
						return new Tuple<string, string>(
							String.Format("(ushort)0x{0:X}", value),
							String.Format("WORD 0x{0:X}", value));
					}
					else if (operandSize == DataSize.Bit32)
					{
						uint value = (uint)rand.Next(0x10000);
						return new Tuple<string, string>(
							String.Format("(uint)0x{0:X}", value),
							String.Format("DWORD 0x{0:X}", value));
					}
					else if (operandSize == DataSize.Bit64)
					{
						var x = (ulong)rand.Next(0x10000);
						var y = (ulong)rand.Next(0x10000);
						ulong value = x | (y << 32);
						return new Tuple<string, string>(
							String.Format("(ulong)0x{0:X}", value),
							String.Format("QWORD 0x{0:X}", value));
					}
					else
						throw new NotSupportedException("The operand size is not supported.");
				case X86OperandType.FixedRegister:
					{
						string name = Enum.GetName(typeof(Register), fixedRegister);
						return new Tuple<string, string>(
								String.Format("Register.{0}", name),
								name.ToLowerInvariant());
					}
				case X86OperandType.MemoryOffset:
					// TODO:
					return new Tuple<string, string>("new MemoryOffset()", "0");
				case X86OperandType.FarPointer:
					{
						if (operandSize == DataSize.Bit16)
						{
							ushort selector = (ushort)rand.Next(0x100, 0x10000);
							ushort offset = (ushort)rand.Next(0x100, 0x10000);
							return new Tuple<string, string>(
								String.Format("new FarPointer(c => 0x{0:X}, c => 0x{1:X}, DataSize.Bit16)", selector, offset),
								String.Format("WORD 0x{0:X}:0x{1:X}", selector, offset));
						}
						else if (operandSize == DataSize.Bit32)
						{
							uint selector = (uint)rand.Next(0x10000);
							uint offset = (uint)rand.Next(0x10000);
							return new Tuple<string, string>(
								String.Format("new FarPointer(c => 0x{0:X}, c => 0x{1:X}, DataSize.Bit32)", selector, offset),
								String.Format("DWORD 0x{0:X}:0x{1:X}", selector, offset));
						}
						else
							throw new NotImplementedException();
					}
				case X86OperandType.MemoryOperand:
				case X86OperandType.RegisterOrMemoryOperand:
					{
						ushort value = (ushort)rand.Next(0x100, 0x10000);
						return new Tuple<string, string>(
							String.Format("new EffectiveAddress(DataSize.Bit{1}, DataSize.None, c => new ReferenceOffset(0x{0:X}))",
								value, operandSize.GetBitCount()),
							String.Format("{0} [0x{1:X}]", GetNasmSizeSpecifier(operandSize), value));
					}
				case X86OperandType.RelativeOffset:
					if (operandSize == DataSize.Bit8)
					{
						byte value = (byte)rand.Next(0, 0x100);
						return new Tuple<string, string>(
							String.Format("new RelativeOffset(c => 0x{0:X}, DataSize.Bit8)", value),
							String.Format("BYTE 0x{0:X}", value));
					}
					else if (operandSize == DataSize.Bit16)
					{
						ushort value = (ushort)rand.Next(0x100, 0x10000);
						return new Tuple<string, string>(
							String.Format("new RelativeOffset(c => 0x{0:X}, DataSize.Bit16)", value),
							String.Format("WORD 0x{0:X}", value));
					}
					else if (operandSize == DataSize.Bit32)
					{
						uint value = (uint)rand.Next(0x10000);
						return new Tuple<string, string>(
							String.Format("new RelativeOffset(c => 0x{0:X}, DataSize.Bit32)", value),
							String.Format("DWORD 0x{0:X}", value));
					}
					else if (operandSize == DataSize.Bit64)
					{
						var x = (ulong)rand.Next(0x10000);
						var y = (ulong)rand.Next(0x10000);
						ulong value = x | (y << 32);
						return new Tuple<string, string>(
							String.Format("new RelativeOffset(c => 0x{0:X}, DataSize.Bit64)", value),
							String.Format("QWORD 0x{0:X}", value));
					}
					else
						throw new NotSupportedException("The operand size is not supported.");
				case X86OperandType.RegisterOperand:
					return RandomGPRegister(operandSize, rand);
				default:
					throw new NotSupportedException("The operand type is not supported.");
			}
		}

		private static readonly string[] Registers = new[]{
			"AX", "CX", "DX", "BX", "SP", "BP", "SI", "DI",
			"8", "9", "10", "11", "12", "13", "14", "15"
		};

		/// <summary>
		/// Returns a random general purpose register.
		/// </summary>
		/// <param name="size">The size of the register.</param>
		/// <param name="rand">The random number generator.</param>
		/// <returns>A tuple with the full name as used in C#, and the register name as used by NASM.</returns>
		private Tuple<string, string> RandomGPRegister(DataSize size, Random rand)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(rand != null);
			Contract.Ensures(Contract.Result<Tuple<string, string>>() != null);
			#endregion

			// NOTE: The minimum is 1, so that AX, EAX and RAX are never possible registers.
			// This is to prevent accidently testing a fixed register.
			if (size == DataSize.Bit8)
			{
				string reg = Registers[rand.Next(1, 5)].Replace("X", "L");
				return new Tuple<string, string>("Register." + reg.ToUpperInvariant(), reg.ToLowerInvariant());
			}
			else if (size == DataSize.Bit16)
			{
				string reg = Registers[rand.Next(1, 9)];
				return new Tuple<string, string>("Register." + reg.ToUpperInvariant(), reg.ToLowerInvariant());
			}
			else if (size == DataSize.Bit32)
			{
				string reg = "E" + Registers[rand.Next(1, 9)];
				return new Tuple<string, string>("Register." + reg.ToUpperInvariant(), reg.ToLowerInvariant());
			}
			else if (size == DataSize.Bit64)
			{
				string reg = "R" + Registers[rand.Next(1, 17)];
				return new Tuple<string, string>("Register." + reg.ToUpperInvariant(), reg.ToLowerInvariant());
			}
			else
				throw new NotSupportedException("The register size is not supported.");
		}

		/// <summary>
		/// Returns the NASM size specifier.
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		private string GetNasmSizeSpecifier(DataSize size)
		{
			switch (size)
			{
				case DataSize.Bit8: return "BYTE";
				case DataSize.Bit16: return "WORD";
				case DataSize.Bit32: return "DWORD";
				case DataSize.Bit64: return "QWORD";
				case DataSize.Bit80: return "TWORD";
				case DataSize.Bit128: return "OWORD";
				case DataSize.Bit256:
					throw new NotSupportedException();
				case DataSize.None:
				default:
					throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Gets a random number generator.
		/// </summary>
		/// <param name="variant">The opcode variant.</param>
		/// <returns>A <see cref="Random"/> object.</returns>
		/// <remarks>
		/// The seed of the RNG depends on the operands used by the opcode variant,
		/// but is constant across uses.
		/// </remarks>
		private Random GetRandom(X86OpcodeVariantSpec variant)
		{
			string name = String.Join("_", from o in variant.Operands.Cast<X86OperandSpec>()
										   select GetOperandManualName(o));
			return new Random(name.GetHashCode());
		}

		/// <summary>
		/// Returns the bytes of the encoded instruction, as returned by the assembler.
		/// </summary>
		/// <param name="mode">The mode.</param>
		/// <param name="instruction">The instruction string.</param>
		/// <param name="feedback">The feedback.</param>
		/// <returns>The bytes; or <see langword="null"/> when it failed.</returns>
		private byte[] GetEncodedInstruction(DataSize mode, string instruction, out string feedback)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), mode));
			Contract.Requires<ArgumentNullException>(instruction != null);
			#endregion
			// Assemble the NASM instruction.
			byte[] expected = null;
			StringBuilder sb = new StringBuilder();
			switch (mode)
			{
				case DataSize.Bit16:
					sb.AppendLine("[BITS 16]");
					break;
				case DataSize.Bit32:
					sb.AppendLine("[BITS 32]");
					break;
				case DataSize.Bit64:
					sb.AppendLine("[BITS 64]");
					break;
				default:
					throw new NotSupportedException();
			}
			sb.AppendLine(instruction);
			expected = RunAssembler(sb.ToString(), out feedback);
			return expected;
		}

		/// <summary>
		/// Runs an assembler and returns the results.
		/// </summary>
		/// <param name="data">The string data to assemble.</param>
		/// <param name="feedback">The feedback from the assembler.</param>
		/// <returns>The binary data resulting from the assembling; or <see langword="null"/> when an error
		/// occurred.</returns>
		private byte[] RunAssembler(string data, out string feedback)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(data != null);
			#endregion

			byte[] encodedData = Encoding.UTF8.GetBytes(data);
			string tempAsmFile = Path.GetTempFileName();
			string tempBinFile = Path.GetTempFileName();
			using (FileStream fs = File.Create(tempAsmFile))
			{
				fs.Write(encodedData, 0, encodedData.Length);
			}

			string executable = Path.GetFullPath(yasmExecutablePath);

			ProcessStartInfo psi = new ProcessStartInfo(executable,
				"-a x86 -f bin -o " + tempBinFile + " " + tempAsmFile);
			psi.RedirectStandardOutput = true;
			psi.RedirectStandardError = true;
			psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
			psi.UseShellExecute = false;

			Process process;
			try
			{
				process = System.Diagnostics.Process.Start(psi);
			}
			catch (Win32Exception wex)
			{
				throw new FileNotFoundException(String.Format("Could not find file {0}.", executable), executable, wex);
			}
			StreamReader std = process.StandardOutput;
			StreamReader err = process.StandardError;
			process.WaitForExit();
			feedback = std.ReadToEnd() + err.ReadToEnd();

			byte[] result;
			try
			{
				using (FileStream fs = File.OpenRead(tempBinFile))
				{
					result = new byte[fs.Length];
					fs.Read(result, 0, (int)fs.Length);
				}
			}
			catch (FileNotFoundException)
			{
				result = null;
			}
			if (result.Length == 0)
				result = null;
			File.Delete(tempAsmFile);
			File.Delete(tempBinFile);
			return result;
		}
	}
}
