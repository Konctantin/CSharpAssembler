#region Copyright and License
/*
 * SharpAssembler
 * Library for .NET that assembles a predetermined list of
 * instructions into machine code.
 * 
 * Copyright (C) 2011-2012 Daniël Pelsmaeker
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
using SharpAssembler;

namespace SharpAssembler.Architectures.X86.Operands
{
	partial class EffectiveAddress
	{
		/// <summary>
		/// Encodes a 16-bit effective address.
		/// </summary>
		/// <param name="instr">The <see cref="EncodedInstruction"/> encoding the operand.</param>
		private void Encode16BitEffectiveAddress(EncodedInstruction instr)
		{
			instr.SetModRMByte();

			// We order the registers in such way that reg1 has the register with the highest number,
			// and reg2 has the register with the lowest number. When a register is not provided, it is put in reg2.
			// This simplifies the following tests, for which the order does not matter.
			var baseReg = this.baseRegister;
			var indexReg = (this.scale == 1 ? indexRegister : Register.None);
			Register reg1 = (baseReg.GetValue() >= indexReg.GetValue() ? baseReg : indexReg);
			Register reg2 = (baseReg.GetValue() < indexReg.GetValue() ? baseReg : indexReg);

			if (scale != 1 && scale != 0)
				throw new AssemblerException("The specified scaling factor is not supported in a 16-bit effective address.");

			// Two cases together deviate from the standard MOD encoding.
			if (reg1 == Register.BP && reg2 == Register.None)
			{
				// [BP+...]
				instr.ModRM.RM = 0x06;
				instr.ModRM.Mod = (byte)(instr.DisplacementSize == DataSize.Bit8 ? 0x01 : 0x02);
			}
			else if (reg1 == Register.None && reg2 == Register.None)
			{
				// [...]
				instr.ModRM.RM = 0x06;
				instr.ModRM.Mod = 0x00;
			}
			else
			{
				// The other cases are straight forward.
				if (reg1 == Register.DI && reg2 == Register.BP)
					// [BP+DI+...]
					instr.ModRM.RM = 0x03;
				else if (reg1 == Register.DI && reg2 == Register.BX)
					// [BX+DI+...]
					instr.ModRM.RM = 0x01;
				else if (reg1 == Register.DI && reg2 == Register.None)
					// [DI+...]
					instr.ModRM.RM = 0x05;
				else if (reg1 == Register.SI && reg2 == Register.BP)
					// [BP+SI+...]
					instr.ModRM.RM = 0x02;
				else if (reg1 == Register.SI && reg2 == Register.BX)
					// [BX+SI+...]
					instr.ModRM.RM = 0x00;
				else if (reg1 == Register.SI && reg2 == Register.None)
					// [SI+...]
					instr.ModRM.RM = 0x04;
				else if (reg1 == Register.BX && reg2 == Register.None)
					// [BX+...]
					instr.ModRM.RM = 0x06;
				else
					throw new AssemblerException("The effective address cannot be encoded");

				switch(instr.DisplacementSize)
				{
					case DataSize.None:
						instr.ModRM.Mod = 0x00;
						break;
					case DataSize.Bit8:
						instr.ModRM.Mod = 0x01;
						break;
					case DataSize.Bit16:
					default:
						// The default is 16-bit, so larger values get truncated.
						instr.ModRM.Mod = 0x02;
						break;
				}
			}
		}
	}
}
