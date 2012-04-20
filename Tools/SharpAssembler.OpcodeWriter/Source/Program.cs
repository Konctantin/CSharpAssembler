using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpAssembler.OpcodeWriter.X86;
using System.IO;

namespace SharpAssembler.OpcodeWriter
{
	class Program
	{
		static void Main(string[] args)
		{
			X86OpcodeSpec spec = new X86OpcodeSpec()
			{
				Mnemonic = "mov",
				CanLock = true,
				IsValidIn64BitMode = true,
				ShortDescription = "Move",
			};

			X86OpcodeVariantSpec variant;

			variant = new X86OpcodeVariantSpec() { OpcodeBytes = new byte[] {  0x88 } };
			spec.Variants.Add(variant);
			variant.Operands.Add(new X86OperandSpec() { Name = "destination", Type = X86OperandType.RegisterOrMemoryOperand, Size = DataSize.Bit8 });
			variant.Operands.Add(new X86OperandSpec() { Name = "source", Type = X86OperandType.RegisterOperand, Size = DataSize.Bit8 });

			variant = new X86OpcodeVariantSpec() { OpcodeBytes = new byte[] { 0x89 } };
			spec.Variants.Add(variant);
			variant.Operands.Add(new X86OperandSpec() { Name = "destination", Type = X86OperandType.RegisterOrMemoryOperand, Size = DataSize.Bit16 });
			variant.Operands.Add(new X86OperandSpec() { Name = "source", Type = X86OperandType.RegisterOperand, Size = DataSize.Bit16 });

			variant = new X86OpcodeVariantSpec() { OpcodeBytes = new byte[] { 0x89 } };
			spec.Variants.Add(variant);
			variant.Operands.Add(new X86OperandSpec() { Name = "destination", Type = X86OperandType.RegisterOrMemoryOperand, Size = DataSize.Bit32 });
			variant.Operands.Add(new X86OperandSpec() { Name = "source", Type = X86OperandType.RegisterOperand, Size = DataSize.Bit32 });

			variant = new X86OpcodeVariantSpec() { OpcodeBytes = new byte[] { 0x89 } };
			spec.Variants.Add(variant);
			variant.Operands.Add(new X86OperandSpec() { Name = "destination", Type = X86OperandType.RegisterOrMemoryOperand, Size = DataSize.Bit64 });
			variant.Operands.Add(new X86OperandSpec() { Name = "source", Type = X86OperandType.RegisterOperand, Size = DataSize.Bit64 });



			variant = new X86OpcodeVariantSpec() { OpcodeBytes = new byte[] { 0x8A } };
			spec.Variants.Add(variant);
			variant.Operands.Add(new X86OperandSpec() { Name = "destination", Type = X86OperandType.RegisterOperand, Size = DataSize.Bit8 });
			variant.Operands.Add(new X86OperandSpec() { Name = "source", Type = X86OperandType.RegisterOrMemoryOperand, Size = DataSize.Bit8 });

			variant = new X86OpcodeVariantSpec() { OpcodeBytes = new byte[] { 0x8B } };
			spec.Variants.Add(variant);
			variant.Operands.Add(new X86OperandSpec() { Name = "destination", Type = X86OperandType.RegisterOperand, Size = DataSize.Bit16 });
			variant.Operands.Add(new X86OperandSpec() { Name = "source", Type = X86OperandType.RegisterOrMemoryOperand, Size = DataSize.Bit16 });

			variant = new X86OpcodeVariantSpec() { OpcodeBytes = new byte[] { 0x8B } };
			spec.Variants.Add(variant);
			variant.Operands.Add(new X86OperandSpec() { Name = "destination", Type = X86OperandType.RegisterOperand, Size = DataSize.Bit32 });
			variant.Operands.Add(new X86OperandSpec() { Name = "source", Type = X86OperandType.RegisterOrMemoryOperand, Size = DataSize.Bit32 });

			variant = new X86OpcodeVariantSpec() { OpcodeBytes = new byte[] { 0x8B } };
			spec.Variants.Add(variant);
			variant.Operands.Add(new X86OperandSpec() { Name = "destination", Type = X86OperandType.RegisterOperand, Size = DataSize.Bit64 });
			variant.Operands.Add(new X86OperandSpec() { Name = "source", Type = X86OperandType.RegisterOrMemoryOperand, Size = DataSize.Bit64 });

			// MOV reg16/32/64/mem16, segReg
			// MOV segReg, reg/mem16

			// TODO: Where did the variants with the fixed register go?
			// They should create an overload that accepts a Register for that parameter, which
			// is then removed to make it distinct with (reg/mem, reg).

			variant = new X86OpcodeVariantSpec() { OpcodeBytes = new byte[] { 0xA0 } };
			spec.Variants.Add(variant);
			variant.Operands.Add(new X86OperandSpec() { Name = "destination", Type = X86OperandType.FixedRegister, FixedRegister = Register.AL });
			variant.Operands.Add(new X86OperandSpec() { Name = "source", Type = X86OperandType.MemoryOffset, Size = DataSize.Bit8 });

			variant = new X86OpcodeVariantSpec() { OpcodeBytes = new byte[] { 0xA1 } };
			spec.Variants.Add(variant);
			variant.Operands.Add(new X86OperandSpec() { Name = "destination", Type = X86OperandType.FixedRegister, FixedRegister = Register.AX });
			variant.Operands.Add(new X86OperandSpec() { Name = "source", Type = X86OperandType.MemoryOffset, Size = DataSize.Bit16 });

			variant = new X86OpcodeVariantSpec() { OpcodeBytes = new byte[] { 0xA1 } };
			spec.Variants.Add(variant);
			variant.Operands.Add(new X86OperandSpec() { Name = "destination", Type = X86OperandType.FixedRegister, FixedRegister = Register.EAX });
			variant.Operands.Add(new X86OperandSpec() { Name = "source", Type = X86OperandType.MemoryOffset, Size = DataSize.Bit32 });

			variant = new X86OpcodeVariantSpec() { OpcodeBytes = new byte[] { 0xA1 } };
			spec.Variants.Add(variant);
			variant.Operands.Add(new X86OperandSpec() { Name = "destination", Type = X86OperandType.FixedRegister, FixedRegister = Register.RAX });
			variant.Operands.Add(new X86OperandSpec() { Name = "source", Type = X86OperandType.MemoryOffset, Size = DataSize.Bit64 });



			variant = new X86OpcodeVariantSpec() { OpcodeBytes = new byte[] { 0xA2 } };
			spec.Variants.Add(variant);
			variant.Operands.Add(new X86OperandSpec() { Name = "destination", Type = X86OperandType.MemoryOffset, Size = DataSize.Bit8 });
			variant.Operands.Add(new X86OperandSpec() { Name = "source", Type = X86OperandType.FixedRegister, FixedRegister = Register.AL });

			variant = new X86OpcodeVariantSpec() { OpcodeBytes = new byte[] { 0xA3 } };
			spec.Variants.Add(variant);
			variant.Operands.Add(new X86OperandSpec() { Name = "destination", Type = X86OperandType.MemoryOffset, Size = DataSize.Bit16 });
			variant.Operands.Add(new X86OperandSpec() { Name = "source", Type = X86OperandType.FixedRegister, FixedRegister = Register.AX });

			variant = new X86OpcodeVariantSpec() { OpcodeBytes = new byte[] { 0xA3 } };
			spec.Variants.Add(variant);
			variant.Operands.Add(new X86OperandSpec() { Name = "destination", Type = X86OperandType.MemoryOffset, Size = DataSize.Bit32 });
			variant.Operands.Add(new X86OperandSpec() { Name = "source", Type = X86OperandType.FixedRegister, FixedRegister = Register.EAX });

			variant = new X86OpcodeVariantSpec() { OpcodeBytes = new byte[] { 0xA3 } };
			spec.Variants.Add(variant);
			variant.Operands.Add(new X86OperandSpec() { Name = "destination", Type = X86OperandType.MemoryOffset, Size = DataSize.Bit64 });
			variant.Operands.Add(new X86OperandSpec() { Name = "source", Type = X86OperandType.FixedRegister, FixedRegister = Register.RAX });



			variant = new X86OpcodeVariantSpec() { OpcodeBytes = new byte[] { 0xB0 } };
			spec.Variants.Add(variant);
			variant.Operands.Add(new X86OperandSpec() { Name = "destination", Type = X86OperandType.RegisterOperand, Size = DataSize.Bit8 });
			variant.Operands.Add(new X86OperandSpec() { Name = "source", Type = X86OperandType.Immediate, Size = DataSize.Bit8 });

			variant = new X86OpcodeVariantSpec() { OpcodeBytes = new byte[] { 0xB8 } };
			spec.Variants.Add(variant);
			variant.Operands.Add(new X86OperandSpec() { Name = "destination", Type = X86OperandType.RegisterOperand, Size = DataSize.Bit16 });
			variant.Operands.Add(new X86OperandSpec() { Name = "source", Type = X86OperandType.Immediate, Size = DataSize.Bit16 });

			variant = new X86OpcodeVariantSpec() { OpcodeBytes = new byte[] { 0xB8 } };
			spec.Variants.Add(variant);
			variant.Operands.Add(new X86OperandSpec() { Name = "destination", Type = X86OperandType.RegisterOperand, Size = DataSize.Bit32 });
			variant.Operands.Add(new X86OperandSpec() { Name = "source", Type = X86OperandType.Immediate, Size = DataSize.Bit32 });

			variant = new X86OpcodeVariantSpec() { OpcodeBytes = new byte[] { 0xB8 } };
			spec.Variants.Add(variant);
			variant.Operands.Add(new X86OperandSpec() { Name = "destination", Type = X86OperandType.RegisterOperand, Size = DataSize.Bit64 });
			variant.Operands.Add(new X86OperandSpec() { Name = "source", Type = X86OperandType.Immediate, Size = DataSize.Bit64 });



			variant = new X86OpcodeVariantSpec() { OpcodeBytes = new byte[] { 0xC6 }, FixedReg = 0 };
			spec.Variants.Add(variant);
			variant.Operands.Add(new X86OperandSpec() { Name = "destination", Type = X86OperandType.RegisterOrMemoryOperand, Size = DataSize.Bit8 });
			variant.Operands.Add(new X86OperandSpec() { Name = "source", Type = X86OperandType.Immediate, Size = DataSize.Bit8 });

			variant = new X86OpcodeVariantSpec() { OpcodeBytes = new byte[] { 0xC7 }, FixedReg = 0 };
			spec.Variants.Add(variant);
			variant.Operands.Add(new X86OperandSpec() { Name = "destination", Type = X86OperandType.RegisterOrMemoryOperand, Size = DataSize.Bit16 });
			variant.Operands.Add(new X86OperandSpec() { Name = "source", Type = X86OperandType.Immediate, Size = DataSize.Bit16 });

			variant = new X86OpcodeVariantSpec() { OpcodeBytes = new byte[] { 0xC7 }, FixedReg = 0 };
			spec.Variants.Add(variant);
			variant.Operands.Add(new X86OperandSpec() { Name = "destination", Type = X86OperandType.RegisterOrMemoryOperand, Size = DataSize.Bit32 });
			variant.Operands.Add(new X86OperandSpec() { Name = "source", Type = X86OperandType.Immediate, Size = DataSize.Bit32 });

			variant = new X86OpcodeVariantSpec() { OpcodeBytes = new byte[] { 0xC7 }, FixedReg = 0 };
			spec.Variants.Add(variant);
			variant.Operands.Add(new X86OperandSpec() { Name = "destination", Type = X86OperandType.RegisterOrMemoryOperand, Size = DataSize.Bit64 });
			variant.Operands.Add(new X86OperandSpec() { Name = "source", Type = X86OperandType.Immediate, Size = DataSize.Bit32 });


			X86SpecWriter writer = new X86SpecWriter();
			writer.Write(spec, "code.cs", "test.cs");
			
			Console.WriteLine("Done!");
			Console.ReadLine();
		}
	}
}
