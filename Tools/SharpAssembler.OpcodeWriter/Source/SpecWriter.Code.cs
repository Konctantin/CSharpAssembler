using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;

namespace SharpAssembler.OpcodeWriter
{
	partial class SpecWriter
	{
		/// <summary>
		/// Writes the specification in code to the specified <see cref="TextWriter"/>.
		/// </summary>
		/// <param name="spec">The specification to write.</param>
		/// <param name="writer">The write to use.</param>
		protected virtual void WriteCode(OpcodeSpec spec, TextWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(spec != null);
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			WriteWarning(writer);
			WriteLicense(writer);
			WriteCodeUsingDirectives(writer);
			writer.WriteLine();
			WriteCodeOpcodeClassStart(spec, writer);
			WriteCodeOpcodeClassProperties(spec, writer);
			writer.WriteLine(T + T + "#region Constructors");
			WriteCodeOpcodeClassConstructors(spec, writer);
			writer.WriteLine(T + T + "#endregion");
			writer.WriteLine();
			WriteCodeOpcodeVariantMethod(spec, writer);
			WriteCodeOpcodeClassEnd(writer);

			WriteCodeInstrClassStart(spec, writer);
			WriteCodeInstrOpcodeVariantMethods(spec, writer);
			WriteCodeInstrClassEnd(writer);

			WriteWarning(writer);
		}

		/// <summary>
		/// Writes the <c>using</c>-directives.
		/// </summary>
		/// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
		/// <remarks>
		/// Override this method to add additional using directives.
		/// Call the base method in the process.
		/// </remarks>
		protected virtual void WriteCodeUsingDirectives(TextWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			// NOTE: Writing more using-directives than strictly needed is neither a problem,
			// nor a cause for warnings or errors.
			writer.WriteLine("using System;");
			writer.WriteLine("using System.Collections.Generic;");
			writer.WriteLine("using System.Linq;");
		}

		/// <summary>
		/// Writes the namespace declaration start and the opcode class declaration start.
		/// </summary>
		/// <param name="spec">The opcode specification.</param>
		/// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
		protected void WriteCodeOpcodeClassStart(OpcodeSpec spec, TextWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(spec != null);
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			writer.WriteLine("namespace {0}", OpcodeNamespace);
			writer.WriteLine("{");
			writer.WriteLine(T + "/// <summary>");
			writer.WriteLine(T + "/// The {0} ({1}) instruction opcode.", spec.Mnemonic.ToUpperInvariant(), spec.ShortDescription);
			writer.WriteLine(T + "/// </summary>");
			//writer.WriteLine(T + "/// <remarks>");

			//if (spec.OperandCount == 0)
			//{
			//    writer.WriteLine(T + "/// Instructions with this opcode expect zero operands.");
			//}
			//else
			//{

			//    writer.WriteLine(T + "/// Instructions with this opcode expect two operands that have the following semantics:");
			//    writer.WriteLine(T + "/// <list type=\"table\">");
			//    writer.WriteLine(T + "/// <listheader><term>Index</term><description>Semantics</description></listheader>");
			//    for (int i = 0; i < spec.Operands.Count; i++)
			//    {
			//        writer.WriteLine(T + "/// <item><term>{0}</term><description>{1}</description></item>", i, spec.Operands[i]);
			//    }
			//    writer.WriteLine(T + "/// </list>");
			//}

			//writer.WriteLine(T + "/// </remarks>");
			writer.WriteLine(T + "public class {0}Opcode : {1}", GetOpcodeClassName(spec), GetOpcodeBaseClassName());
			writer.WriteLine(T + "{");
		}

		/// <summary>
		/// Writes the properties for the opcode class.
		/// </summary>
		/// <param name="spec">The opcode specification.</param>
		/// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
		protected virtual void WriteCodeOpcodeClassProperties(OpcodeSpec spec, TextWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(spec != null);
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion
		}

		/// <summary>
		/// Writes the constructors for the opcode class.
		/// </summary>
		/// <param name="spec">The opcode specification.</param>
		/// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
		protected virtual void WriteCodeOpcodeClassConstructors(OpcodeSpec spec, TextWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(spec != null);
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			WriteCodeOpcodeClassMainConstructor(spec, writer);
		}

		/// <summary>
		/// Writes the main constructor for the opcode class.
		/// </summary>
		/// <param name="spec">The opcode specification.</param>
		/// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
		protected void WriteCodeOpcodeClassMainConstructor(OpcodeSpec spec, TextWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(spec != null);
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			int maxOperandCount = (from v in spec.Variants select v.Operands.Count).Max();

			writer.WriteLine(T + T + "/// <summary>");
			writer.WriteLine(T + T + "/// Initializes a new instance of the <see cref=\"{0}Opcode\"/> class.",
				GetOpcodeClassName(spec));
			writer.WriteLine(T + T + "/// </summary>");
			writer.WriteLine(T + T + "public {0}Opcode()", GetOpcodeClassName(spec));
			writer.WriteLine(T + T + T + ": base(\"{0}\", {1}, GetOpcodeVariants())", spec.Mnemonic.ToLowerInvariant(), maxOperandCount);
			writer.WriteLine(T + T + "{ /* Nothing to do. */ }");
		}

		/// <summary>
		/// Writes the <c>GetOpcodeVariants</c> method for the opcode class.
		/// </summary>
		/// <param name="spec">The opcode specification.</param>
		/// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
		protected void WriteCodeOpcodeVariantMethod(OpcodeSpec spec, TextWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(spec != null);
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			string variantClassName = GetOpcodeVariantClassName();

			writer.WriteLine(T + T + "/// <summary>");
			writer.WriteLine(T + T + "/// Returns the opcode variants of this opcode.");
			writer.WriteLine(T + T + "/// </summary>");
			writer.WriteLine(T + T + "/// <returns>An enumerable collection of <see cref=\"{0}\"/> objects.</returns>",
				variantClassName);
			writer.WriteLine(T + T + "private static IEnumerable<{0}> GetOpcodeVariants()", variantClassName);
			writer.WriteLine(T + T + "{");
			writer.WriteLine(T + T + T + "return new {0}[]{{", variantClassName);
			//writer.WriteLine(T + T + T + T + "#region Variants");

			foreach (var variant in spec.Variants)
			{
				WriteCodeOpcodeVariant(spec, variant, writer);
			}

			//writer.WriteLine(T + T + T + T + "#endregion");
			writer.WriteLine(T + T + T + "};");
			writer.WriteLine(T + T + "}");
		}

		/// <summary>
		/// Writes the opcode variant specification in the variants array.
		/// </summary>
		/// <param name="spec">The opcode specification.</param>
		/// <param name="variant">The opcode variant.</param>
		/// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
		protected abstract void WriteCodeOpcodeVariant(OpcodeSpec spec, OpcodeVariantSpec variant, TextWriter writer);

		/// <summary>
		/// Writes the end of the opcode class declaration and its namespace.
		/// </summary>
		/// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
		protected void WriteCodeOpcodeClassEnd(TextWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			writer.WriteLine(T + "}");
			writer.WriteLine("}");
			writer.WriteLine();
		}

		/// <summary>
		/// Writes the namespace declaration start and the <c>Instr</c> class declaration start.
		/// </summary>
		/// <param name="spec">The opcode specification.</param>
		/// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
		protected void WriteCodeInstrClassStart(OpcodeSpec spec, TextWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(spec != null);
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			writer.WriteLine("namespace {0}", BaseNamespace);
			writer.WriteLine("{");
			writer.WriteLine(T + "partial class Instr");
			writer.WriteLine(T + "{");
		}

		/// <summary>
		/// Writes an opcode variant method.
		/// </summary>
		/// <param name="spec">The opcode specification.</param>
		/// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
		protected abstract void WriteCodeInstrOpcodeVariantMethods(OpcodeSpec spec, TextWriter writer);

		/// <summary>
		/// Writes the end of the <c>Instr</c> class declaration and its namespace.
		/// </summary>
		/// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
		protected void WriteCodeInstrClassEnd(TextWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			writer.WriteLine(T + "}");
			writer.WriteLine("}");
			writer.WriteLine();
		}

		/// <summary>
		/// Returns the class name for the opcode class.
		/// </summary>
		/// <param name="spec">The opcode specification.</param>
		/// <returns>The opcode class name.</returns>
		protected virtual string GetOpcodeClassName(OpcodeSpec spec)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(spec != null);
			Contract.Ensures(Contract.Result<string>() != null);
			#endregion

			return Char.ToUpperInvariant(spec.Mnemonic[0]).ToString() + spec.Mnemonic.Substring(1);
		}

		/// <summary>
		/// Returns the class name of the base class for the opcode classes.
		/// </summary>
		/// <returns>The opcode base class name.</returns>
		protected abstract string GetOpcodeBaseClassName();

		/// <summary>
		/// Returns the class name of the opcode variant class.
		/// </summary>
		/// <returns>The opcode base class name.</returns>
		protected abstract string GetOpcodeVariantClassName();
	}

	#region Contract
	namespace Contracts
	{
		partial class SpecWriterContract : SpecWriter
		{
			protected override string GetOpcodeBaseClassName()
			{
				Contract.Ensures(Contract.Result<string>() != null);
				return default(string);
			}

			protected override string GetOpcodeVariantClassName()
			{
				Contract.Ensures(Contract.Result<string>() != null);
				return default(string);
			}

			protected override void WriteCodeOpcodeVariant(OpcodeSpec spec, OpcodeVariantSpec variant, TextWriter writer)
			{
				Contract.Requires<ArgumentNullException>(spec != null);
				Contract.Requires<ArgumentNullException>(variant != null);
				Contract.Requires<ArgumentNullException>(writer != null);
			}

			protected override void WriteCodeInstrOpcodeVariantMethods(OpcodeSpec spec, TextWriter writer)
			{
				Contract.Requires<ArgumentNullException>(spec != null);
				Contract.Requires<ArgumentNullException>(writer != null);
			}
		}
	}
	#endregion
}
