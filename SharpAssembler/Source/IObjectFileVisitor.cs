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
using System;
using System.Diagnostics.Contracts;
using SharpAssembler.Instructions;

namespace SharpAssembler
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
		/// <param name="objectFile">The <see cref="ObjectFile"/> being visited.</param>
		void VisitObjectFile(ObjectFile objectFile);

		/// <summary>
		/// Called when a <see cref="Section"/> is visited.
		/// </summary>
		/// <param name="section">The <see cref="Section"/> being visited.</param>
		void VisitSection(Section section);

		/// <summary>
		/// Called when a <see cref="Constructable"/> is visited.
		/// </summary>
		/// <param name="constructable">The <see cref="Constructable"/> being visited.</param>
		/// <remarks>
		/// This method is only called when no more specific <c>Visit*</c> method is called for the type of
		/// constructable.
		/// </remarks>
		void VisitConstructable(Constructable constructable);

		#region Specialized
		/// <summary>
		/// Called when a <see cref="CustomConstructable"/> is visited.
		/// </summary>
		/// <param name="customConstructable">The <see cref="CustomConstructable"/> being visited.</param>
		void VisitCustomConstructable(CustomConstructable customConstructable);

		/// <summary>
		/// Called when an <see cref="Instruction"/> is visited.
		/// </summary>
		/// <param name="instruction">The <see cref="Instruction"/> being visited.</param>
		void VisitInstruction(Instruction instruction);

		/// <summary>
		/// Called when an <see cref="Align"/> constructable is visited.
		/// </summary>
		/// <param name="align">The <see cref="Align"/> constructable being visited.</param>
		void VisitAlign(Align align);

		/// <summary>
		/// Called when a <see cref="Comment"/> constructable is visited.
		/// </summary>
		/// <param name="comment">The <see cref="Comment"/> constructable being visited.</param>
		void VisitComment(Comment comment);

		/// <summary>
		/// Called when a <see cref="DeclareData"/> constructable is visited.
		/// </summary>
		/// <param name="declaration">The <see cref="DeclareData"/> constructable being visited.</param>
		void VisitDeclareData(DeclareData declaration);

		/// <summary>
		/// Called when a <see cref="DeclareData{T}"/> constructable is visited.
		/// </summary>
		/// <param name="declaration">The <see cref="DeclareData{T}"/> constructable being visited.</param>
		void VisitDeclareData<T>(DeclareData<T> declaration)
			where T : struct;

		/// <summary>
		/// Called when a <see cref="DeclareString"/> constructable is visited.
		/// </summary>
		/// <param name="declaration">The <see cref="DeclareString"/> constructable being visited.</param>
		void VisitDeclareString(DeclareString declaration);

		/// <summary>
		/// Called when a <see cref="Define"/> constructable is visited.
		/// </summary>
		/// <param name="definition">The <see cref="Define"/> constructable being visited.</param>
		void VisitDefine(Define definition);

		/// <summary>
		/// Called when a <see cref="Extern"/> constructable is visited.
		/// </summary>
		/// <param name="reference">The <see cref="Extern"/> constructable being visited.</param>
		void VisitExtern(Extern reference);

		/// <summary>
		/// Called when a <see cref="Group"/> constructable is visited.
		/// </summary>
		/// <param name="group">The <see cref="Group"/> constructable being visited.</param>
		void VisitGroup(Group group);

		/// <summary>
		/// Called when a <see cref="Label"/> constructable is visited.
		/// </summary>
		/// <param name="label">The <see cref="Label"/> constructable being visited.</param>
		void VisitLabel(Label label);
		#endregion
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

			public void VisitCustomConstructable(CustomConstructable customConstructable)
			{
				Contract.Requires<ArgumentNullException>(customConstructable != null);
			}

			public void VisitInstruction(Instruction instruction)
			{
				Contract.Requires<ArgumentNullException>(instruction != null);
			}

			public void VisitAlign(Align align)
			{
				Contract.Requires<ArgumentNullException>(align != null);
			}

			public void VisitComment(Comment comment)
			{
				Contract.Requires<ArgumentNullException>(comment != null);
			}

			public void VisitDeclareData(DeclareData declaration)
			{
				Contract.Requires<ArgumentNullException>(declaration != null);
			}

			public void VisitDeclareData<T>(DeclareData<T> declaration)
				where T : struct
			{
				Contract.Requires<ArgumentNullException>(declaration != null);
			}

			public void VisitDeclareString(DeclareString declaration)
			{
				Contract.Requires<ArgumentNullException>(declaration != null);
			}

			public void VisitDefine(Define definition)
			{
				Contract.Requires<ArgumentNullException>(definition != null);
			}

			public void VisitExtern(Extern reference)
			{
				Contract.Requires<ArgumentNullException>(reference != null);
			}

			public void VisitGroup(Group group)
			{
				Contract.Requires<ArgumentNullException>(group != null);
			}

			public void VisitLabel(Label label)
			{
				Contract.Requires<ArgumentNullException>(label != null);
			}
		}
	}
	#endregion
}
