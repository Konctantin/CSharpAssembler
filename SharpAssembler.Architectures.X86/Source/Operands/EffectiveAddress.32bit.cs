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
using System;
using SharpAssembler;

namespace SharpAssembler.Architectures.X86.Operands
{
	partial class EffectiveAddress
	{
		/// <summary>
		/// Encodes a 32-bit effective address.
		/// </summary>
		/// <param name="instr">The <see cref="EncodedInstruction"/> encoding the operand.</param>
		private void Encode32BitEffectiveAddress(EncodedInstruction instr)
		{
			instr.SetModRMByte();

			if (baseRegister == Register.None && indexRegister == Register.None)
			{
				// R/M
				instr.ModRM.RM = 0x05;
				// Mod
				instr.ModRM.Mod = 0x00;

				// Only 32-bit displacements can be encoded without a base and index register.
				instr.DisplacementSize = DataSize.Bit32;
				if (instr.Displacement == null)
					instr.Displacement = new SimpleExpression(0);
			}
			else if (baseRegister != Register.ESP && indexRegister == Register.None)
			{
				// R/M
				instr.ModRM.RM = (byte)((int)baseRegister & 0x07);

				// Displacement.
				if (instr.Displacement == null && baseRegister == Register.EBP)
				{
					// [EBP] will be represented as [EBP+disp8].
					instr.DisplacementSize = DataSize.Bit8;
					instr.Displacement = new SimpleExpression(0);
				}

				// Mod
				if (instr.DisplacementSize == DataSize.None)
					instr.ModRM.Mod = 0x00;
				else if (instr.DisplacementSize == DataSize.Bit8)
					instr.ModRM.Mod = 0x01;
				else if (instr.DisplacementSize <= DataSize.Bit32)
					instr.ModRM.Mod = 0x02;
			}
			else
			{
				// Encode the SIB byte too.
				instr.SetSIBByte();

				// R/M
				instr.ModRM.RM = 0x04;

				// Displacement
				if (instr.Displacement == null && baseRegister == Register.EBP)
				{
					// [EBP+REG*s] will be represented as [EBP+REG*s+disp8].
					instr.DisplacementSize = DataSize.Bit8;
					instr.Displacement = new SimpleExpression(0);
				}

				// Mod
				if (instr.DisplacementSize == DataSize.None)
					instr.ModRM.Mod = 0x00;
				else if (instr.DisplacementSize == DataSize.Bit8)
					instr.ModRM.Mod = 0x01;
				else if (instr.DisplacementSize <= DataSize.Bit32)
					instr.ModRM.Mod = 0x02;

				// Base
				instr.Sib.Base = (byte)((int)baseRegister & 0x07);
				if (baseRegister == Register.None)
					instr.Sib.Base = 0x05;

				// Index
				instr.Sib.Index = (byte)((int)indexRegister & 0x07);
				if (indexRegister == Register.None)
					instr.Sib.Index = 0x20;

				// Scale
				instr.Sib.Scale = (byte)((int)Math.Log(scale, 2));
			}
		}
	}
}
