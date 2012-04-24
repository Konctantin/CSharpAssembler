Syntax
======
This document describes the syntax used in the input files for the OpcodeWriter tool.

Basics
------

### Primitives ###

#### Comments ####
Single-line comments start with `//` and run to the end of the line. Multi-line comments start with `/*` and run to the next occurence of `*/`. Nested multi-line comments are not supported.

#### Registers ####
Sometimes it is necessary to specify a register. This can be done by specifying just the name of the register (case insensitive). So, `EAX` denotes the 32-bit EAX register on the x86 platform. The names of allowed registers depend on the platform.

#### Enumeration members ####
Platform-specific properties can be set to a specific enumeration member. These are specified by the name of the enumeration member (case insensitive).

#### Strings ####
Strings start and end with a single double quote character (`"`). Using a backslash in a string requires it to be escaped with a backslash (`\\`). Single backslashes are not allowed, and no other escape sequences are defined. Strings cannot span multiple lines.

#### Integers ####
Integers can be written in decimal (`123`) and hexadecimal (`0x7B`).

#### Booleans ####
A boolean can only be `true` or `false`.

### Aliases ###
At the root of the file, aliases can be specified. Whenever an identifier is used that is specified as an alias, the original is used instead. Aliases are specified by the `alias` keyword, followed by the identifier to replace, an equals sign, and the identifier to replace it by.

	alias d = destination;

### Defining an opcode ###
An opcode definition starts with the keyword `opcode` followed by the special platform identifier. Then the actual mnemonic of the opcode, and the body of the opcode containing the properties and the opcode variants.

    opcode x86 mov { ... }

#### Annotations ####
To annotate an object, specification or definition, precede it with a comma-separated list of annotations inside square brackets. Each annotation is a name, an equals sign and a value. All annotations must be supported.

    [ShortDescription = "Move"]

#### Variants ####
*Note: the order of the variants **does** matter!* The earliest mentioned variant that is a match is used, regardless of whether it is the most efficient one.

Each variant starts with the `var` keyword followed by a space separated list of hexadecimal byte values (without the `0x` prefix), which represent the opcode bytes for the variant (e.g. `FE 4A`). Then follows a parenthesized comma-separated operand list, and it ends with either a semi-colon or curly brackets with properties.

    var `0F AE F0` ();

Each operand specifies the type (e.g. `reg/mem64`), the name and optionally a value.

    moffset32 destination

The operand type is platform specific. When an operand has a fixed value, its type is `void` and the value must be specified.

    void destination = %EAX

Each variant ends with a semi-colon.

Example
-------
An example file for the `mov` x86-64 instruction:

	alias d = destination;
	alias s = source;
	
	[ShortDescription = "Move"]
	[IsValidIn64BitMode = true]
	[CanLock = true]
	opcode x86 mov
	{
		/* A comment
		on multiple lines */
		var `88` (reg/mem8  d, reg8  s);
		var `89` (reg/mem16 d, reg16 s);
		var `89` (reg/mem32 d, reg32 s);
		var `89` (reg/mem64 d, reg64 s);
		
		var `8A` (reg8  d, reg/mem8  s);
		var `8B` (reg16 d, reg/mem16 s);
		var `8B` (reg32 d, reg/mem32 s);
		var `8B` (reg64 d, reg/mem64 s);
		
		//var `8C` (reg16/32/64/mem16 d, segReg s);
		//var `8E` (segReg d, reg/mem16 s);
		
		//var `A0` (void d = %AL,  moffset8  s);
		//var `A1` (void d = %AX,  moffset16 s);
		//var `A1` (void d = %EAX, moffset32 s);
		//var `A1` (void d = %RAX, moffset64 s);
		
		//var `A2` (moffset8  d, void s = %AL);
		//var `A3` (moffset16 d, void s = %AX);
		//var `A3` (moffset32 d, void s = %EAX);
		//var `A3` (moffset64 d, void s = %RAX);
		
		var `B0` (reg8  d, imm8  s);
		var `B8` (reg16 d, imm16 s);
		var `B8` (reg32 d, imm32 s);
		var `B8` (reg64 d, imm64 s);
		
		[FixedReg = 0]
		var `C6` (reg/mem8  d, imm8  s);
		[FixedReg = 0]
		var `C7` (reg/mem16 d, imm16 s);
		[FixedReg = 0]
		var `C7` (reg/mem32 d, imm32 s);
		[FixedReg = 0]
		var `C7` (reg/mem64 d, imm32 s);
	}
	