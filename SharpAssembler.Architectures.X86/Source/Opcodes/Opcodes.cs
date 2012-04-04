using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpAssembler.Architectures.X86.Opcodes;

namespace SharpAssembler.Architectures.X86
{
	partial class X86Opcode
	{
		/// <summary>
		/// The AAA (ASCII Adjust After Addition) instruction opcode.
		/// </summary>
		public static readonly X86Opcode Aaa = new AaaOpcode();

		/// <summary>
		/// The AAD (ASCII Adjust Before Division) instruction opcode.
		/// </summary>
		public static readonly X86Opcode Aad = new AadOpcode();

		/// <summary>
		/// The AAM (ASCII Adjust After Multiply) instruction opcode.
		/// </summary>
		public static readonly X86Opcode Aam = new AamOpcode();

		/// <summary>
		/// The AAS (ASCII Adjust After Subtraction) instruction opcode.
		/// </summary>
		public static readonly X86Opcode Aas = new AasOpcode();

		/// <summary>
		/// The ADC (Add with Carry) instruction opcode.
		/// </summary>
		public static readonly X86Opcode Adc = new AdcOpcode();

		/// <summary>
		/// The ADD (Signed or Unsigned Add) instruction opcode.
		/// </summary>
		public static readonly X86Opcode Add = new AddOpcode();

		/// <summary>
		/// The AND (Logical AND) instruction opcode.
		/// </summary>
		public static readonly X86Opcode And = new AndOpcode();





		/// <summary>
		/// The INT (Interrupt to Vector) instruction opcode.
		/// </summary>
		public static readonly X86Opcode Int = new IntOpcode();





		/// <summary>
		/// The MOV (Move) instruction opcode.
		/// </summary>
		public static readonly X86Opcode Mov = new MovOpcode();
	}
}
