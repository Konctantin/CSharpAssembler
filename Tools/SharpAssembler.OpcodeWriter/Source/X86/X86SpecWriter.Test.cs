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
								 select GetOperandStrings(spec, variant, o);
			string operands = String.Join(", ", from o in operandStrings select o.Item1);

			string instruction = String.Format("{0} {1}", spec.Mnemonic.ToLowerInvariant(),
				String.Join(", ", from o in operandStrings select o.Item2));

			byte[] bytes16 = GetEncodedInstruction(DataSize.Bit16, instruction);
			byte[] bytes32 = GetEncodedInstruction(DataSize.Bit32, instruction);
			byte[] bytes64 = GetEncodedInstruction(DataSize.Bit64, instruction);

			writer.WriteLine(T + T + "[Test]");
			writer.WriteLine(T + T + "public void {0}()", String.Join("_", from o in variant.Operands.Cast<X86OperandSpec>()
																		   select GetOperandManualName(o)).Replace("/", ""));
			writer.WriteLine(T + T + "{");
			writer.WriteLine(T + T + T + "var instruction = Instr.{0}({1});",
				GetOpcodeClassName(spec), operands);
			writer.WriteLine();
			writer.WriteLine(T + T + T + "// " + instruction);
			if (bytes16 != null)
				writer.WriteLine(T + T + T + "AssertInstruction(instruction, DataSize.Bit16, new byte[] { " + String.Join(", ", from b in bytes16 select String.Format("0x{0:X2}", b)) + " });");
			else
				writer.WriteLine(T + T + T + "AssertInstructionFail(instruction, DataSize.Bit16);");
			if (bytes32 != null)
				writer.WriteLine(T + T + T + "AssertInstruction(instruction, DataSize.Bit32, new byte[] { " + String.Join(", ", from b in bytes32 select String.Format("0x{0:X2}", b)) + " });");
			else
				writer.WriteLine(T + T + T + "AssertInstructionFail(instruction, DataSize.Bit32);");
			if (bytes64 != null)
				writer.WriteLine(T + T + T + "AssertInstruction(instruction, DataSize.Bit64, new byte[] { " + String.Join(", ", from b in bytes64 select String.Format("0x{0:X2}", b)) + " });");
			else
				writer.WriteLine(T + T + T + "AssertInstructionFail(instruction, DataSize.Bit64);");
			writer.WriteLine(T + T + "}");
		}

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
					// TODO:
					throw new NotImplementedException();
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
					if (operand.Size == DataSize.Bit8)
						return new Tuple<string, string>("Register.CL", "cl");
					else if (operand.Size == DataSize.Bit16)
						return new Tuple<string, string>("Register.CX", "cx");
					else if (operand.Size == DataSize.Bit32)
						return new Tuple<string, string>("Register.ECX", "ecx");
					else if (operand.Size == DataSize.Bit64)
						return new Tuple<string, string>("Register.RCX", "rcx");
					else
						throw new NotSupportedException("The operand size is not supported.");
				default:
					throw new NotSupportedException("The operand type is not supported.");
			}
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
				case DataSize.Bit80:
				case DataSize.Bit128:
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
		/// <returns>The bytes; or <see langword="null"/> when it failed.</returns>
		private byte[] GetEncodedInstruction(DataSize mode, string instruction)
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
			string feedback;
			expected = RunAssembler(sb.ToString(), out feedback);
			Console.WriteLine("  {0}:", instruction);
			if (!String.IsNullOrWhiteSpace(feedback))
				Console.WriteLine("    {0}", feedback.Replace("test.asm:2: ", "").Trim().Replace("\n", "\n    "));
			if (expected != null)
				Console.WriteLine("    {0}", String.Join(" ", from e in expected select e.ToString("X2")));
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
			using (FileStream fs = File.Create("test.asm"))
			{
				fs.Write(encodedData, 0, encodedData.Length);
			}
			File.Delete("test.bin");
			ProcessStartInfo psi = new ProcessStartInfo(@"..\..\Assembler\yasm",
				"-a x86 -f bin -o test.bin test.asm");
			psi.RedirectStandardOutput = true;
			psi.RedirectStandardError = true;
			psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
			psi.UseShellExecute = false;

			Process process = System.Diagnostics.Process.Start(psi);
			StreamReader std = process.StandardOutput;
			StreamReader err = process.StandardError;
			process.WaitForExit();
			feedback = std.ReadToEnd() + err.ReadToEnd();

			byte[] result;
			try
			{
				using (FileStream fs = File.OpenRead("test.bin"))
				{
					result = new byte[fs.Length];
					fs.Read(result, 0, (int)fs.Length);
				}
			}
			catch (FileNotFoundException)
			{
				result = null;
			}
			return result;
		}
	}
}
