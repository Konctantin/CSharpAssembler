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

namespace SharpAssembler.x86
{
	/// <summary>
	/// An interface for instructions which can have a lock prefix.
	/// </summary>
	public interface ILockInstruction
	{
		/// <summary>
		/// Gets or sets whether the lock prefix is used.
		/// </summary>
		/// <value><see langword="true"/> to enable the lock prefix; otherwise, <see langword="false"/>.
		/// The default is <see langword="false"/>.</value>
		/// <remarks>
		/// When this property is set to <see langword="true"/>, the lock signal is asserted before accessing the
		/// specified memory location. When the lock signal has already been asserted, the instruction must wait for it
		/// to be released. Instructions without the lock prefix do not check the lock signal, and will be executed
		/// even when the lock signal is asserted by some other instruction.
		/// </remarks>
		bool Lock
		{ get; set; }
	}
}
