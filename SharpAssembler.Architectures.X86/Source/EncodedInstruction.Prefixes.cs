#region Copyright and License
/*
 * SharpAssembler
 * Library for .NET that assembles a predetermined list of
 * instructions into machine code.
 * 
 * Copyright (C) 2011 Daniël Pelsmaeker
 * 
 * This file is part of SharpAssembler.
 * 
 * SharpAssembler is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SharpAssembler is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SharpAssembler.  If not, see <http://www.gnu.org/licenses/>.
 */
#endregion

namespace SharpAssembler.Architectures.X86
{
	partial class EncodedInstruction
	{
		/// <summary>
		/// Specifies prefixes from group 1 allowed to precede an instruction.
		/// </summary>
		public enum PrefixLockRepeat
		{
			/// <summary>
			/// No prefix.
			/// </summary>
			None = 0x00,

			/// <summary>
			/// Lock prefix.
			/// </summary>
			/// <remarks>
			/// Represents the LOCK prefix.
			/// </remarks>
			Lock = 0xF0,
			/// <summary>
			/// Repeat when not equal or zero.
			/// </summary>
			/// <remarks>
			/// Represents the REPNE and REPNZ prefixes.
			/// </remarks>
			RepeatNotEqual = 0xF2,
			/// <summary>
			/// Repeat, repeat when equal or zero.
			/// </summary>
			/// <remarks>
			/// Represents the REP, REPE and REPZ prefixes.
			/// </remarks>
			RepeatEqual = 0xF3,
		}


		/// <summary>
		/// Specifies prefixes from group 2 allowed to precede an instruction.
		/// </summary>
		public enum PrefixSegmentBranch
		{
			/// <summary>
			/// No prefix.
			/// </summary>
			None = 0x00,

			/// <summary>
			/// CS segment override.
			/// </summary>
			CSOverride = 0x2E,
			/// <summary>
			/// SS segment override.
			/// </summary>
			SSOverride = 0x36,
			/// <summary>
			/// DS segment override.
			/// </summary>
			DSOverride = 0x3E,
			/// <summary>
			/// ES segment override.
			/// </summary>
			ESOverride = 0x26,
			/// <summary>
			/// FS segment override.
			/// </summary>
			FSOverride = 0x64,
			/// <summary>
			/// GS segment override.
			/// </summary>
			GSOverride = 0x65,
			/// <summary>
			/// Branch not taken.
			/// </summary>
			BranchNotTaken = 0x2E,
			/// <summary>
			/// Branch taken.
			/// </summary>
			BranchTaken = 0x3E,
		}


		/// <summary>
		/// Specifies prefixes from group 3 allowed to precede an instruction.
		/// </summary>
		public enum PrefixAddressSizeOverride
		{
			/// <summary>
			/// No prefix.
			/// </summary>
			None = 0x00,

			/// <summary>
			/// Address-size override prefix.
			/// </summary>
			AddressSizeOverride = 0x67,
		}


		/// <summary>
		/// Specifies prefixes from group 4 allowed to precede an instruction.
		/// </summary>
		public enum PrefixOperandSizeOverride
		{
			/// <summary>
			/// No prefix.
			/// </summary>
			None = 0x00,

			/// <summary>
			/// Operand-size override prefix.
			/// </summary>
			OperandSizeOverride = 0x66,
		}
	}
}
