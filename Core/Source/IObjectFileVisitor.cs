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
using System.Diagnostics.Contracts;

namespace SharpAssembler.Core
{
	/// <summary>
	/// Visitor for the <see cref="ObjectFile"/> and its parts.
	/// </summary>
	[ContractClass(typeof(Contracts.IObjectFileVisitorContract))]
	public interface IObjectFileVisitor
	{
		/// <summary>
		/// Called when an <see cref="ObjectFile"/> is visited.
		/// </summary>
		/// <param name="objectfile">The <see cref="ObjectFile"/> being visited.</param>
		void VisitObjectFile(ObjectFile objectfile);

		/// <summary>
		/// Called when a <see cref="Section"/> is visited.
		/// </summary>
		/// <param name="section">The <see cref="Section"/> being visited.</param>
		void VisitSection(Section section);

		/// <summary>
		/// Called when a <see cref="Constructable"/> is visited.
		/// </summary>
		/// <param name="constructable">The <see cref="Constructable"/> being visited.</param>
		void VisitConstructable(Constructable constructable);
	}

	#region Contract
	namespace Contracts
	{
		/// <summary>
		/// Contract class for the <see cref="IObjectFileVisitor"/> interface.
		/// </summary>
		[ContractClassFor(typeof(IObjectFileVisitor))]
		abstract class IObjectFileVisitorContract : IObjectFileVisitor
		{
			public void VisitObjectFile(ObjectFile objectfile)
			{
				Contract.Requires<ArgumentNullException>(objectfile != null);
			}

			public void VisitSection(Section section)
			{
				Contract.Requires<ArgumentNullException>(section != null);
			}

			public void VisitConstructable(Constructable constructable)
			{
				Contract.Requires<ArgumentNullException>(constructable != null);
			}
		}
	}
	#endregion
}
