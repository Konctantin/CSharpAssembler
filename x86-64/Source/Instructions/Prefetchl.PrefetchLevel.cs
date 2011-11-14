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

namespace SharpAssembler.x86.Instructions
{
	partial class Prefetchl
	{
		/// <summary>
		/// Specifies the prefetch level for the <see cref="Prefetchl"/> instruction.
		/// </summary>
		public enum PrefetchLevel
		{
			/// <summary>
			/// No prefetch level specified.
			/// </summary>
			None = 0,
			/// <summary>
			/// Non-Temporal Access.
			/// </summary>
			NonTemporalAccess = 0x100 | 0,
			/// <summary>
			/// All cache levels.
			/// </summary>
			T0 = 0x100 | 1,
			/// <summary>
			/// Level 2 and higher.
			/// </summary>
			T1 = 0x100 | 2,
			/// <summary>
			/// Level 3 and higher.
			/// </summary>
			T2 = 0x100 | 3,
		}
	}
}
