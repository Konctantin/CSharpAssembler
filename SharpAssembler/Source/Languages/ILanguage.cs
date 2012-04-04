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
using System.IO;
using SharpAssembler.Instructions;

namespace SharpAssembler.Languages
{
	/// <summary>
	/// An interface for classes which can output assembler code from an object file and the structures within.
	/// </summary>
	[ContractClass(typeof(Contracts.ILanguageContract))]
	public interface ILanguage : IObjectFileVisitor
	{
		/// <summary>
		/// Gets the name of the language.
		/// </summary>
		/// <value>The name of the language.</value>
		string Name
		{ get; }

		/// <summary>
		/// Gets or sets whether to insert comments into the code.
		/// </summary>
		/// <value><see langword="true"/> to insert comments into the code;
		/// otherwise, <see langword="false"/>.</value>
		bool InsertComments
		{ get; set; }

		/// <summary>
		/// Gets or sets whether to insert extra newlines at appropriate locations.
		/// </summary>
		/// <value><see langword="true"/> to insert extra newlines into the code;
		/// otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
		/// <remarks>
		/// When the language does not support this, setting this property has no effect.
		/// </remarks>
		bool InsertNewlines
		{ get; set; }

		/// <summary>
		/// Gets or sets the maximum length of a single line.
		/// </summary>
		/// <value>The maximum number of characters on a line; or 0 to specify no limit.
		/// The default is 79.</value>
		/// <remarks>
		/// When an unsplittable word is longer than the available line space,
		/// the lines may get longer than this maximum.
		/// </remarks>
		int MaximumLineLength
		{ get; set; }

		/// <summary>
		/// Gets the <see cref="TextWriter"/> used to write the assembler code to.
		/// </summary>
		/// <value>A <see cref="TextWriter"/> object.</value>
		TextWriter Writer
		{ get; }
	}

	#region Contract
	namespace Contracts
	{
		[ContractClassFor(typeof(ILanguage))]
		abstract class ILanguageContract : ILanguage
		{
			public string Name
			{
				get
				{
					Contract.Ensures(Contract.Result<string>() != null);
					return default(string);
				}
			}

			public bool InsertComments
			{
				get
				{
					return default(bool);
				}
				set
				{
				}
			}

			public bool InsertNewlines
			{
				get
				{
					return default(bool);
				}
				set
				{
				}
			}

			public int MaximumLineLength
			{
				get
				{
					Contract.Ensures(Contract.Result<int>() >= 0);
					return default(int);
				}
				set
				{
					Contract.Requires<ArgumentOutOfRangeException>(value >= 0);
				}
			}

			public TextWriter Writer
			{
				get
				{
					Contract.Ensures(Contract.Result<TextWriter>() != null);
					return default(TextWriter);
				}
			}


			public abstract void VisitObjectFile(ObjectFile objectfile);

			public abstract void VisitSection(Section section);

			public abstract void VisitConstructable(Constructable constructable);

			public abstract void VisitCustomConstructable(CustomConstructable customConstructable);

			public abstract void VisitInstruction(Instruction instruction);

			public abstract void VisitAlign(Align align);

			public abstract void VisitComment(Comment comment);

			public abstract void VisitDeclareData(DeclareData declaration);

			public abstract void VisitDeclareData<T>(DeclareData<T> declaration)
				where T : struct;

			public abstract void VisitDeclareString(DeclareString declaration);

			public abstract void VisitDefine(Define definition);

			public abstract void VisitExtern(Extern reference);

			public abstract void VisitGroup(Group group);

			public abstract void VisitLabel(Label label);
		}
	}
	#endregion
}
