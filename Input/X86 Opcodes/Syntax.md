Syntax
======
This document describes the syntax used in the input files for the OpcodeWriter tool.

Basics
------

### Primitives ###

#### Comments ####
Single-line comments start with `//` and run to the end of the line. Multi-line comments start with `/*` and run to the next occurence of `*/`. Nested multi-line comments are not supported.

#### Registers ####
Sometimes it is necessary to specify a register. This can be done by specifying a percent sign followed by the name of the register (case insensitive). So, `%EAX` denotes the 32-bit EAX register on the x86 platform.

#### Strings ####
Strings start and end with a single double quote character (`"`). Using a backslash in a string requires it to be escaped with a backslash (`\\`). Single backslashes are not allowed, and no other escape sequences are defined. Strings cannot span multiple lines.

#### Integers ####
Integers can be written in decimal (`123`) and hexadecimal (`0x7B`).

#### Booleans ####
A boolean can only be `true` or `false`.

### Defining an opcode ###
An opcode definition starts with the keyword `opcode` followed by the special platform identifier. Then the actual mnemonic of the opcode, and the body of the opcode containing the properties and the opcode variants. Only one opcode definition per file is allowed.

    opcode x86 mov { ... }

#### Properties ####
Each property starts with the `set` keyword, followed by a name, an equals sign and a value. Each property ends with a semi-colon. Properties that are not supported are ignored.

    set ShortDescription = "Move";

#### Variants ####
Each variant starts with the `var` keyword followed by a space separated list of hexadecimal byte values (without the `0x` prefix), which represent the opcode bytes for the variant (e.g. `FE 4A`). Then follows a parenthesized comma-separated operand list, and it ends with either a semi-colon or curly brackets with properties.

    variant 0F AE F0 ();

Each operand specifies the type (e.g. `reg/mem64`), the name and optionally a value.

    moffset32 destination

The operand type is platform specific. When an operand has a fixed value, its type is `void` and the value must be specified.

    void destination = %EAX

Each variant ends with either a semi-colon, or a pair of curly brackets with a list of platform-specific additional properties. Properties that are not supported are ignored. 

Example
-------
An example file for the `mov` x86-64 instruction:

	opcode x86 mov
	{
		set ShortDescription = "Move";
		set IsValidIn64BitMode = true;
		set CanLock = true;
		
		var 88 (reg/mem8  destination, reg8  source);
		var 89 (reg/mem16 destination, reg16 source);
		var 89 (reg/mem32 destination, reg32 source);
		var 89 (reg/mem64 destination, reg64 source);
		
		var 8A (reg8  destination, reg/mem8  source);
		var 8B (reg16 destination, reg/mem16 source);
		var 8B (reg32 destination, reg/mem32 source);
		var 8B (reg64 destination, reg/mem64 source);
		
		//var 8C (reg16/32/64/mem16 destination, segReg source);
		//var 8E (segReg destination, reg/mem16 source);
		
		var A0 (void destination = %AL,  moffset8  source);
		var A1 (void destination = %AX,  moffset16 source);
		var A1 (void destination = %EAX, moffset32 source);
		var A1 (void destination = %RAX, moffset64 source);
		
		var A2 (moffset8  destination, void source = %AL);
		var A3 (moffset16 destination, void source = %AX);
		var A3 (moffset32 destination, void source = %EAX);
		var A3 (moffset64 destination, void source = %RAX);
		
		var B0 (reg8  destination, imm8  source);
		var B8 (reg16 destination, imm16 source);
		var B8 (reg32 destination, imm32 source);
		var B8 (reg64 destination, imm64 source);
		
		var C6 (reg/mem8  destination, imm8  source) { set FixedReg = 0; }
		var C7 (reg/mem16 destination, imm16 source) { set FixedReg = 0; }
		var C7 (reg/mem32 destination, imm32 source) { set FixedReg = 0; }
		var C7 (reg/mem64 destination, imm32 source) { set FixedReg = 0; }
	}