using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;

namespace SharpAssembler.OpcodeWriter.X86
{
	partial class X86SpecWriter
	{
		/// <inheritdoc />
		protected override void WriteCodeOpcodeClassProperties(OpcodeSpec spec, TextWriter writer)
		{
			// CONTRACT: SpecWriter

			X86OpcodeSpec x86spec = (X86OpcodeSpec)spec;

			WriteCanLock(x86spec, writer);
			WriteIsValidIn64BitMode(x86spec, writer);

			base.WriteCodeOpcodeClassProperties(spec, writer);
		}

		/// <inheritdoc />
		protected override void WriteCodeUsingDirectives(TextWriter writer)
		{
			base.WriteCodeUsingDirectives(writer);
			writer.WriteLine("using SharpAssembler.Architectures.X86.Operands;");
		}


		/// <summary>
		/// Writes the <c>CanLock</c> property.
		/// </summary>
		/// <param name="spec">The opcode specification.</param>
		/// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
		protected void WriteCanLock(X86OpcodeSpec spec, TextWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(spec != null);
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			if (spec.CanLock)
				return;

			writer.WriteLine(T + T + "/// <inheritdoc />");
			writer.WriteLine(T + T + "public override bool CanLock");
			writer.WriteLine(T + T + "{");
			writer.WriteLine(T + T + T + "get { return true; }");
			writer.WriteLine(T + T + "}");
			writer.WriteLine();
		}

		/// <summary>
		/// Writes the <c>CanLock</c> property.
		/// </summary>
		/// <param name="spec">The opcode specification.</param>
		/// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
		protected void WriteIsValidIn64BitMode(X86OpcodeSpec spec, TextWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(spec != null);
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			if (spec.IsValidIn64BitMode)
				return;

			writer.WriteLine(T + T + "/// <inheritdoc />");
			writer.WriteLine(T + T + "public override bool IsValidIn64BitMode");
			writer.WriteLine(T + T + "{");
			writer.WriteLine(T + T + T + "get { return false; }");
			writer.WriteLine(T + T + "}");
			writer.WriteLine();
		}

		/// <inheritdoc />
		protected override void WriteCodeOpcodeVariant(OpcodeSpec spec, OpcodeVariantSpec variant, TextWriter writer)
		{
			var x86variant = (X86OpcodeVariantSpec)variant;
			var operands = x86variant.Operands.Cast<X86OperandSpec>();
			var operandNames = from o in operands select GetOperandManualName(o);

			writer.WriteLine(T + T + T + T + "// {0} {1}", spec.Mnemonic.ToUpperInvariant(), String.Join(", ", operandNames));
			writer.WriteLine(T + T + T + T + "new X86OpcodeVariant(");

			string opcodeBytes = String.Join(", ", from b in x86variant.OpcodeBytes select String.Format("0x{0:X2}", b));

			writer.Write(T + T + T + T + T + "new byte[] {{ {0} }},", opcodeBytes);

			// Either the fixed REG is not 0, or the opcode has a spot for the REG available as it uses the
			// ModRM byte (because it has a reg/mem operand) but does not specify the REG part (because it does not
			// have a reg operand).
			if (x86variant.FixedReg != 0 ||
				(operands.Any(o => o.Type == X86OperandType.RegisterOrMemoryOperand) &&
				!operands.Any(o => o.Type == X86OperandType.RegisterOperand)))
			{
				writer.WriteLine(" {0},", x86variant.FixedReg);
			}
			else
			{
				writer.WriteLine();
			}

			if (x86variant.Operands.Any())
			{
				WriteOperandDescriptor(operands.First(), writer);
				foreach (var operand in operands.Skip(1))
				{
					writer.WriteLine(",");
					WriteOperandDescriptor(operand, writer);
				}
				writer.WriteLine("),");
			}
		}

		/// <summary>
		/// Writes the operand descriptor.
		/// </summary>
		/// <param name="operand">The operand specification.</param>
		/// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
		private void WriteOperandDescriptor(X86OperandSpec operand, TextWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(operand != null);
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			switch (operand.Type)
			{
				case X86OperandType.RegisterOperand:
					writer.Write(T + T + T + T + T + "new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose{0}Bit)",
						operand.Size.GetBitCount());
					// TODO: Add ", OperandEncoding.OpcodeAdd" when there is no reg/mem.
					break;
				case X86OperandType.FixedRegister:
					writer.Write(T + T + T + T + T + "new OperandDescriptor(Register.{0})",
						Enum.GetName(typeof(Register), operand.FixedRegister).ToUpperInvariant());
					break;
				case X86OperandType.Immediate:
					writer.Write(T + T + T + T + T + "new OperandDescriptor(OperandType.Immediate, DataSize.Bit{0})",
						operand.Size.GetBitCount());
					break;
				case X86OperandType.MemoryOperand:
					writer.Write(T + T + T + T + T + "new OperandDescriptor(OperandType.MemoryOperand, DataSize.Bit{0})",
						operand.Size.GetBitCount());
					break;
				case X86OperandType.MemoryOffset:
					writer.Write(T + T + T + T + T + "new OperandDescriptor(OperandType.MemoryOffset, DataSize.Bit{0})",
						operand.Size.GetBitCount());
					break;
				case X86OperandType.FarPointer:
					writer.Write(T + T + T + T + T + "new OperandDescriptor(OperandType.FarPointer, DataSize.Bit{0})",
						operand.Size.GetBitCount());
					break;
				case X86OperandType.RegisterOrMemoryOperand:
					writer.Write(T + T + T + T + T + "new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose{0}Bit)",
						operand.Size.GetBitCount());
					break;
				case X86OperandType.RelativeOffset:
					writer.Write(T + T + T + T + T + "new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit{0})",
						operand.Size.GetBitCount());
					break;
				default:
					throw new NotSupportedException("The operand type is not supported.");
			}
		}

		/// <summary>
		/// Writes the documentation for the opcode variant method.
		/// </summary>
		/// <param name="spec">The opcode specification.</param>
		/// <param name="variant">The opcode variant.</param>
		/// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
		protected void WriteCodeInstrOpcodeVariantMethodDocumentation(X86OpcodeSpec spec,
			IEnumerable<Tuple<X86OperandSpec, String, String>> operands, TextWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(spec != null);
			Contract.Requires<ArgumentNullException>(operands != null);
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			writer.WriteLine(T + T + "/// <summary>");
			writer.WriteLine(T + T + "/// Creates a new {0} ({1}) instruction.", spec.Mnemonic.ToUpperInvariant(), spec.ShortDescription);
			writer.WriteLine(T + T + "/// </summary>");

			foreach (var operand in operands)
			{
				WriteOperandDocumentation(operand, writer);
			}

			writer.WriteLine(T + T + "/// <returns>The created instruction.</returns>");
		}

		/// <inheritdoc />
		protected override void WriteCodeInstrOpcodeVariantMethods(OpcodeSpec spec, TextWriter writer)
		{
			// CONTRACT: SpecWriter

			var x86spec = (X86OpcodeSpec)spec;

			// Determine all possible combinations of parameters.
			var operandArguments = x86spec.Variants
				.SelectMany(v => CartesianProduct(
					from o in v.Operands.Cast<X86OperandSpec>()
					select GetOperandArguments(o)));
			var operandTuples = operandArguments.Distinct(new OperandEnumerationEqualityComparer());


			if (operandTuples.Any())
			{
				WriteCodeInstrOpcodeVariantMethod(x86spec, operandTuples.First(), writer);
				foreach (var operands in operandTuples.Skip(1))
				{
					writer.WriteLine();
					WriteCodeInstrOpcodeVariantMethod(x86spec, operands, writer);
				}
			}
		}

		/// <summary>
		/// Writes a single opcode variant method.
		/// </summary>
		/// <param name="spec">The opcode specification.</param>
		/// <param name="operands">The operands.</param>
		/// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
		private void WriteCodeInstrOpcodeVariantMethod(X86OpcodeSpec spec,
			IEnumerable<Tuple<X86OperandSpec, String, String>> operands, TextWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(spec != null);
			Contract.Requires<ArgumentNullException>(writer != null);
			Contract.Requires<ArgumentNullException>(operands != null);
			#endregion

			WriteCodeInstrOpcodeVariantMethodDocumentation(spec, operands, writer);
			WriteCodeCLSCompliance(operands, writer);
			writer.WriteLine(T + T + "public static X86Instruction {0}({1})",
				GetOpcodeClassName(spec), String.Join(", ", from o in operands select o.Item2 + " " + o.Item1.Name));
			writer.WriteLine(T + T + "{{ return X86Opcode.{0}.CreateInstruction({1}); }}",
				GetOpcodeClassName(spec), String.Join(", ", from o in operands select String.Format(o.Item3, o.Item1.Name)));
		}

		/// <summary>
		/// Writes whether the variant method is CLS compliant.
		/// </summary>
		/// <param name="operands">The method's operands.</param>
		/// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
		private void WriteCodeCLSCompliance(IEnumerable<Tuple<X86OperandSpec, string, string>> operands, TextWriter writer)
		{
			if (operands.Any(t => !IsCLSCompliantType(t.Item2)))
			{
				writer.WriteLine(T + T + "[CLSCompliant(false)]");
			}
		}

		/// <summary>
		/// Returns whether the specified type name is CLS compliant.
		/// </summary>
		/// <param name="typeName">The name of the type.</param>
		/// <returns><see langword="true"/> when the type is CLS compliant;
		/// otherwise, <see langword="false"/>.</returns>
		private bool IsCLSCompliantType(string typeName)
		{
			switch(typeName)
			{
				case"sbyte":
				case "ushort":
				case "uint":
				case "ulong":
					return false;
				default:
					return true;
			}
		}

		/// <summary>
		/// Returns the carthesian product of a sequence of sequences.
		/// </summary>
		/// <typeparam name="T">The type of object in the sequences.</typeparam>
		/// <param name="sequences">The sequences.</param>
		/// <returns>The cartesian product of the sequences.</returns>
		private static IEnumerable<IEnumerable<T>> CartesianProduct<T>(IEnumerable<IEnumerable<T>> sequences)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(sequences != null);
			Contract.Ensures(Contract.Result<IEnumerable<IEnumerable<T>>>() != null);
			#endregion
			// This implementation is based on the code published by Eric Lippert on his blog at:
			// https://blogs.msdn.com/b/ericlippert/archive/2010/06/28/computing-a-cartesian-product-with-linq.aspx

			IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
			return sequences.Aggregate(
				emptyProduct,
				(accumulator, sequence) =>
					from accseq in accumulator
					from item in sequence
					select accseq.Concat(new[] { item }));
		}

		/// <summary>
		/// Gets the arguments for the specified operand.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <param name="clsCompliant">Whether the resulting method is CLS compliant.</param>
		/// <returns>An array of tuples. Each tuple specifies the type of the argument and the
		/// implementation as an operand. The latter uses <c>{0}</c> in place of the argument name.</returns>
		private IEnumerable<Tuple<X86OperandSpec, String, String>> GetOperandArguments(X86OperandSpec operand)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(operand != null);
			Contract.Ensures(Contract.Result<IEnumerable<Tuple<String, String>>>() != null);
			#endregion

			switch (operand.Type)
			{
				case X86OperandType.Immediate:
					if (operand.Size == DataSize.Bit8)
					{
						return new Tuple<X86OperandSpec, String, String>[]{
							new Tuple<X86OperandSpec, String, String>(operand, "byte", "new Immediate({0}, DataSize.Bit8)"),
							new Tuple<X86OperandSpec, String, String>(operand, "sbyte", "new Immediate({0}, DataSize.Bit8)"),
						};
					}
					else if (operand.Size == DataSize.Bit16)
					{
						return new Tuple<X86OperandSpec, String, String>[]{
							new Tuple<X86OperandSpec, String, String>(operand, "short", "new Immediate({0}, DataSize.Bit16)"),
							new Tuple<X86OperandSpec, String, String>(operand, "ushort", "new Immediate({0}, DataSize.Bit16)"),
						};
					}
					else if (operand.Size == DataSize.Bit32)
					{
						return new Tuple<X86OperandSpec, String, String>[]{
							new Tuple<X86OperandSpec, String, String>(operand, "int", "new Immediate({0}, DataSize.Bit32)"),
							new Tuple<X86OperandSpec, String, String>(operand, "uint", "new Immediate({0}, DataSize.Bit32)"),
						};
					}
					else if (operand.Size == DataSize.Bit64)
					{
						return new Tuple<X86OperandSpec, String, String>[]{
							new Tuple<X86OperandSpec, String, String>(operand, "long", "new Immediate({0}, DataSize.Bit64)"),
							new Tuple<X86OperandSpec, String, String>(operand, "ulong", "new Immediate({0}, DataSize.Bit64)"),
						};
					}
					else
						throw new NotSupportedException("The operand size is not supported.");
				case X86OperandType.MemoryOperand:
					return new Tuple<X86OperandSpec, String, String>[]{
						new Tuple<X86OperandSpec, String, String>(operand, "EffectiveAddress", "{0}"),
					};
				case X86OperandType.MemoryOffset:
					return new Tuple<X86OperandSpec, String, String>[]{
						new Tuple<X86OperandSpec, String, String>(operand, "MemoryOffset", "{0}"),
					};
				case X86OperandType.FarPointer:
					return new Tuple<X86OperandSpec, String, String>[]{
						new Tuple<X86OperandSpec, String, String>(operand, "FarPointer", "{0}"),
						new Tuple<X86OperandSpec, String, String>(operand, "EffectiveAddress", "{0}"),
					};
				case X86OperandType.RegisterOrMemoryOperand:
					return new Tuple<X86OperandSpec, String, String>[]{
						new Tuple<X86OperandSpec, String, String>(operand, "Register", "new RegisterOperand({0})"),
						new Tuple<X86OperandSpec, String, String>(operand, "EffectiveAddress", "{0}"),
					};
				case X86OperandType.RelativeOffset:
					return new Tuple<X86OperandSpec, String, String>[]{
						new Tuple<X86OperandSpec, String, String>(operand, "RelativeOffset", "{0}"),
					};
				case X86OperandType.RegisterOperand:
					return new Tuple<X86OperandSpec, String, String>[]{
						new Tuple<X86OperandSpec, String, String>(operand, "Register", "new RegisterOperand({0})"),
					};
				case X86OperandType.FixedRegister:
					return Enumerable.Empty<Tuple<X86OperandSpec, String, String>>();
				default:
					throw new NotSupportedException("The operand type is not supported.");
			}
		}

		/// <summary>
		/// Writes the operand documentation.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
		private void WriteOperandDocumentation(Tuple<X86OperandSpec, String, String> operand, TextWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(operand != null);
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			switch (operand.Item1.Type)
			{
				case X86OperandType.RegisterOperand:
					writer.WriteLine(T + T + "/// <param name=\"{0}\">A register.</param>",
						operand.Item1.Name);
					break;
				case X86OperandType.Immediate:
					writer.WriteLine(T + T + "/// <param name=\"{0}\">An immediate value.</param>",
						operand.Item1.Name);
					break;
				case X86OperandType.MemoryOperand:
					writer.WriteLine(T + T + "/// <param name=\"{0}\">A memory operand.</param>",
						operand.Item1.Name);
					break;
				case X86OperandType.MemoryOffset:
					writer.WriteLine(T + T + "/// <param name=\"{0}\">A memory offset.</param>",
						operand.Item1.Name);
					break;
				case X86OperandType.FarPointer:
					writer.WriteLine(T + T + "/// <param name=\"{0}\">A far pointer.</param>",
						operand.Item1.Name);
					break;
				case X86OperandType.RegisterOrMemoryOperand:
					writer.WriteLine(T + T + "/// <param name=\"{0}\">A register or memory operand.</param>",
						operand.Item1.Name);
					break;
				case X86OperandType.RelativeOffset:
					writer.WriteLine(T + T + "/// <param name=\"{0}\">A relative offset.</param>",
						operand.Item1.Name);
					break;
				case X86OperandType.FixedRegister:
					// No documentation.
				default:
					throw new NotSupportedException("The operand type is not supported.");
			}
		}

		private class OperandEnumerationEqualityComparer : IEqualityComparer<IEnumerable<Tuple<X86OperandSpec, String, String>>>
		{

			public bool Equals(IEnumerable<Tuple<X86OperandSpec, string, string>> x, IEnumerable<Tuple<X86OperandSpec, string, string>> y)
			{
				return Enumerable.SequenceEqual(x, y, new OperandEqualityComparer());
			}

			public int GetHashCode(IEnumerable<Tuple<X86OperandSpec, string, string>> obj)
			{
				// NOTE: Equality is often determined by looking at the hash codes.
				unchecked
				{
					int hash = 17;
					foreach (var e in obj)
					{
						hash = hash * 29 + e.Item2.GetHashCode();
					}
					return hash;
				}
			}
		}

		private class OperandEqualityComparer : IEqualityComparer<Tuple<X86OperandSpec, String, String>>
		{

			public bool Equals(Tuple<X86OperandSpec, string, string> x, Tuple<X86OperandSpec, string, string> y)
			{
				return x.Item2.Equals(y.Item2);
			}

			public int GetHashCode(Tuple<X86OperandSpec, string, string> obj)
			{
				return obj.Item2.GetHashCode();
			}
		}
	}
}
