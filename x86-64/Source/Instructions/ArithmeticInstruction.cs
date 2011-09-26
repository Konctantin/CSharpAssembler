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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SharpAssembler.x86.Operands;

namespace SharpAssembler.x86.Instructions
{
	/// <summary>
	/// An arithmetic instruction.
	/// </summary>
	public abstract class ArithmeticInstruction : Instruction, ILockInstruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ArithmeticInstruction"/> class.
		/// </summary>
		/// <param name="destination">The destination register operand.</param>
		/// <param name="source">The source immediate operand.</param>
		protected ArithmeticInstruction(RegisterOperand destination, Immediate source)
			: this((IRegisterOrMemoryOperand)destination, (ISourceOperand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentException>(destination.Register.IsGeneralPurposeRegister());
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArithmeticInstruction"/> class.
		/// </summary>
		/// <param name="destination">The destination memory operand.</param>
		/// <param name="source">The source immediate operand.</param>
		protected ArithmeticInstruction(EffectiveAddress destination, Immediate source)
			: this((IRegisterOrMemoryOperand)destination, (ISourceOperand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArithmeticInstruction"/> class.
		/// </summary>
		/// <param name="destination">The destination register operand.</param>
		/// <param name="source">The source register operand.</param>
		protected ArithmeticInstruction(RegisterOperand destination, RegisterOperand source)
			: this((IRegisterOrMemoryOperand)destination, (ISourceOperand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentException>(destination.Register.IsGeneralPurposeRegister());
			Contract.Requires<ArgumentNullException>(source != null);
			Contract.Requires<ArgumentException>(source.Register.IsGeneralPurposeRegister());
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArithmeticInstruction"/> class.
		/// </summary>
		/// <param name="destination">The destination memory operand.</param>
		/// <param name="source">The source register operand.</param>
		protected ArithmeticInstruction(EffectiveAddress destination, RegisterOperand source)
			: this((IRegisterOrMemoryOperand)destination, (ISourceOperand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			Contract.Requires<ArgumentException>(source.Register.IsGeneralPurposeRegister());
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArithmeticInstruction"/> class.
		/// </summary>
		/// <param name="destination">The destination register operand.</param>
		/// <param name="source">The source register or memory operand.</param>
		protected ArithmeticInstruction(RegisterOperand destination, EffectiveAddress source)
			: this((IRegisterOrMemoryOperand)destination, (ISourceOperand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentException>(destination.Register.IsGeneralPurposeRegister());
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}



		/// <summary>
		/// Initializes a new instance of the <see cref="ArithmeticInstruction"/> class.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		private ArithmeticInstruction(IRegisterOrMemoryOperand destination, ISourceOperand source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion

			this.destination = destination;
			this.source = source;
		}
		#endregion

		#region Properties
		private IRegisterOrMemoryOperand destination;
		/// <summary>
		/// Gets the destination operand of the instruction.
		/// </summary>
		/// <value>An <see cref="IRegisterOrMemoryOperand"/>.</value>
		public IRegisterOrMemoryOperand Destination
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<IRegisterOrMemoryOperand>() != null);
				#endregion
				return destination;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				destination = value;
			}
#endif
		}

		private ISourceOperand source;
		/// <summary>
		/// Gets the source operand of the instruction.
		/// </summary>
		/// <value>An <see cref="ISourceOperand"/>.</value>
		public ISourceOperand Source
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<ISourceOperand>() != null);
				#endregion
				return source;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				source = value;
			}
#endif
		}

		private bool lockInstruction = false;
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
		public bool Lock
		{
			get { return lockInstruction; }
			set { lockInstruction = value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Enumerates an ordered list of operands used by this instruction.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Operand"/> objects.</returns>
		public override IEnumerable<Operand> GetOperands()
		{
			// The order is important here!
			yield return (Operand)this.destination;
			yield return (Operand)this.source;
		}
		#endregion

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this.destination != null);
			Contract.Invariant(this.source != null);
		}
		#endregion
	}
}
