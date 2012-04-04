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
	/// A base class for classes which can output assembler code from an object file and the structures
	/// within.
	/// </summary>
	public abstract class Language : ILanguage
	{
		/// <summary>
		/// Gets the name of the language.
		/// </summary>
		/// <value>The name of the language.</value>
		public abstract string Name
		{ get; }

		private bool insertComments = true;
		/// <summary>
		/// Gets or sets whether to insert comments into the code.
		/// </summary>
		/// <value><see langword="true"/> to insert comments into the code;
		/// otherwise, <see langword="false"/>.</value>
		public bool InsertComments
		{
			get { return insertComments; }
			set { insertComments = value; }
		}

		private bool insertNewlines = true;
		/// <summary>
		/// Gets or sets whether to insert extra newlines at appropriate locations.
		/// </summary>
		/// <value><see langword="true"/> to insert extra newlines into the code;
		/// otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
		/// <remarks>
		/// When the language does not support this, setting this property has no effect.
		/// </remarks>
		public bool InsertNewlines
		{
			get { return insertNewlines; }
			set { insertNewlines = value; }
		}

		private int maximumLineLength = 79;
		/// <summary>
		/// Gets or sets the maximum length of a single line.
		/// </summary>
		/// <value>The maximum number of characters on a line; or 0 to specify no limit.
		/// The default is 79.</value>
		/// <remarks>
		/// When an unsplittable word is longer than the available line space,
		/// the lines may get longer than this maximum.
		/// </remarks>
		public int MaximumLineLength
		{
			get { return maximumLineLength; }
			set { maximumLineLength = value; }
		}

		private readonly TextWriter writer;
		/// <summary>
		/// Gets the <see cref="TextWriter"/> used to write the assembler code to.
		/// </summary>
		/// <value>A <see cref="TextWriter"/> object.</value>
		public TextWriter Writer
		{
			get { return writer; }
		}

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Language"/> class.
		/// </summary>
		/// <param name="writer">The <see cref="TextWriter"/> used to write the assembler code to.</param>
		protected Language(TextWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			this.writer = writer;
		}
		#endregion

		/// <summary>
		/// Write the contents of the specified object file to the text writer .
		/// </summary>
		/// <param name="objectFile">The <see cref="ObjectFile"/> to write.</param>
		/// <remarks>
		/// The text writer is automatically flushed after writing the object file to it.
		/// </remarks>
		public void Write(ObjectFile objectFile)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(objectFile != null);
			#endregion
			VisitObjectFile(objectFile);
			writer.Flush();
		}

		#region Methods
		/// <inheritdoc />
		void IObjectFileVisitor.VisitObjectFile(ObjectFile objectFile)
		{ VisitObjectFile(objectFile); }

		/// <inheritdoc />
		protected virtual void VisitObjectFile(ObjectFile objectFile)
		{
			foreach (var section in objectFile.Sections)
			{
				section.Accept(this);
			}
		}

		/// <inheritdoc />
		void IObjectFileVisitor.VisitSection(Section section)
		{ VisitSection(section); }

		/// <inheritdoc />
		protected virtual void VisitSection(Section section)
		{
			foreach (var constructable in section.Contents)
			{
				constructable.Accept(this);
			}
		}

		/// <inheritdoc />
		void IObjectFileVisitor.VisitConstructable(Constructable constructable)
		{ VisitConstructable(constructable); }

		/// <inheritdoc />
		protected virtual void VisitConstructable(Constructable constructable)
		{ /* No implementation. */ }

		#region Specialized
		/// <inheritdoc />
		void IObjectFileVisitor.VisitCustomConstructable(CustomConstructable constructable)
		{ VisitCustomConstructable(constructable); }

		/// <inheritdoc />
		protected virtual void VisitCustomConstructable(CustomConstructable constructable)
		{ /* No implementation. */ }

		/// <inheritdoc />
		void IObjectFileVisitor.VisitInstruction(Instruction constructable)
		{ VisitInstruction(constructable); }

		/// <inheritdoc />
		protected virtual void VisitInstruction(Instruction constructable)
		{ /* No implementation. */ }

		/// <inheritdoc />
		void IObjectFileVisitor.VisitAlign(Align constructable)
		{ VisitAlign(constructable); }

		/// <inheritdoc />
		protected virtual void VisitAlign(Align constructable)
		{ /* No implementation. */ }

		/// <inheritdoc />
		void IObjectFileVisitor.VisitComment(Comment constructable)
		{ VisitComment(constructable); }

		/// <inheritdoc />
		protected virtual void VisitComment(Comment constructable)
		{ /* No implementation. */ }

		/// <inheritdoc />
		void IObjectFileVisitor.VisitDeclareData(DeclareData constructable)
		{ VisitDeclareData(constructable); }

		/// <inheritdoc />
		protected virtual void VisitDeclareData(DeclareData constructable)
		{ /* No implementation. */ }

		/// <inheritdoc />
		void IObjectFileVisitor.VisitDeclareData<T>(DeclareData<T> constructable)
		{ VisitDeclareData<T>(constructable); }

		/// <inheritdoc />
		protected virtual void VisitDeclareData<T>(DeclareData<T> constructable)
			where T : struct
		{ /* No implementation. */ }

		/// <inheritdoc />
		void IObjectFileVisitor.VisitDeclareString(DeclareString constructable)
		{ VisitDeclareString(constructable); }

		/// <inheritdoc />
		protected virtual void VisitDeclareString(DeclareString constructable)
		{ /* No implementation. */ }

		/// <inheritdoc />
		void IObjectFileVisitor.VisitDefine(Define constructable)
		{ VisitDefine(constructable); }

		/// <inheritdoc />
		protected virtual void VisitDefine(Define constructable)
		{ /* No implementation. */ }

		/// <inheritdoc />
		void IObjectFileVisitor.VisitExtern(Extern constructable)
		{ VisitExtern(constructable); }

		/// <inheritdoc />
		protected virtual void VisitExtern(Extern constructable)
		{ /* No implementation. */ }

		/// <inheritdoc />
		void IObjectFileVisitor.VisitGroup(Group constructable)
		{ VisitGroup(constructable); }

		/// <inheritdoc />
		protected virtual void VisitGroup(Group constructable)
		{
			foreach (var subconstructable in constructable.Content)
			{
				subconstructable.Accept(this);
			}
		}

		/// <inheritdoc />
		void IObjectFileVisitor.VisitLabel(Label constructable)
		{ VisitLabel(constructable); }

		/// <inheritdoc />
		protected virtual void VisitLabel(Label constructable)
		{ /* No implementation. */ }
		#endregion


#if false
		/// <summary>
		/// Gets a list of <see cref="Comment"/> objects which are associated with the
		/// specified <see cref="Constructable"/> object.
		/// </summary>
		/// <returns></returns>
		protected IEnumerable<Comment> GetComment(Constructable constructable)
		{
			Comment comment;

			// Get all the comments.
			Dictionary<Constructable, string> comments = new Dictionary<Constructable, string>();
			if (insertComments)
			{
				foreach (Constructable c in section)
				{
					comment = c as Comment;
					if (comment != null && comment.AssociatedConstructable != null)
					{
						if (comments.ContainsKey(comment.AssociatedConstructable))
							// Multiple comments about the same Constructable are put together.
							comments[comment.AssociatedConstructable] += "\n" + comment.Text;
						else
							// A comment about an uncommented Constructable.
							comments.Add(comment.AssociatedConstructable, comment.Text);
					}
				}
			}

			foreach (Constructable c in section)
			{
				comment = c as Comment;
				// Only use those comments which are not associated with a Constructable.
				if (comment != null)
					if (!insertComments || comment.AssociatedConstructable != null)
						continue;
				WriteConstructable(objectfile, section, c, comments);
			}
		}

#if false
		/// <summary>
		/// Writes the contents of the specified <see cref="ObjectFile"/>
		/// in the assembly language.
		/// </summary>
		/// <param name="objectfile">The <see cref="ObjectFile"/> to write.</param>
		public virtual void WriteObjectFile(ObjectFile objectfile)
		{
			foreach (Section s in objectfile)
				WriteSection(objectfile, s);
		}
#endif

		/// <summary>
		/// Writes the contents of the specified <see cref="Section"/>
		/// in the assembly language.
		/// </summary>
		/// <param name="objectfile">The <see cref="ObjectFile"/> containing the section.</param>
		/// <param name="section">The <see cref="Section"/> to write.</param>
		public virtual void WriteSection(ObjectFile objectfile, Section section)
		{
			
		}

#if false
		/// <summary>
		/// Writes the contents of the specified <see cref="Constructable"/>
		/// in the assembly language.
		/// </summary>
		/// <param name="objectfile">The <see cref="ObjectFile"/> containing the section.</param>
		/// <param name="section">The <see cref="Section"/> containing the constructable.</param>
		/// <param name="constructable">The <see cref="Constructable"/> to write.</param>
		/// <param name="comments">The comments about the code.</param>
		/// <returns>The number of characters used to write the <see cref="Constructable"/>.</returns>
		public virtual int WriteConstructable(ObjectFile objectfile, Section section, Constructable constructable, Dictionary<Constructable, string> comments)
		{
			// Nothing to do.
			return 0;
		}
#endif
#endif
		#endregion

		#region Invariants
		/// <summary>
		/// Asserts the invariants for this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvarant()
		{
			Contract.Invariant(this.writer != null);
		}
		#endregion
	}
}
