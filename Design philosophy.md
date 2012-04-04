SharpAssembler is based on the workings of a two-pass assembler. In SharpAssembler, this means that any (pseudo-)instruction that can be part of the object file (e.g. instructions, labels, defines) has three possible states:
* Initial
* Constructed
* Emitted
Any (pseudo-)instruction in its initial state is a `Constructable`. Constructables can change in size and content, depending on the context and other factors. Some constructables have nested constructables, and they can contain many complex expressions. On the first pass of the assembler, each constructable has the responsibility to construct an emittable representation of themselves. These `IEmittable` objects are in the constructed state.

Emittables have very limited capabilities. Their size is fixed and known, they cannot be nested and have only simple expressions (a tuple of a symbol and a relative offset). On the second pass of the assembler, each emittable has the responsibility to emit its binary representation to the `BinaryWriter`. The resulting bytes are the (pseudo-)instructions in their emitted state. This state is not modeled by SharpAssembler.

