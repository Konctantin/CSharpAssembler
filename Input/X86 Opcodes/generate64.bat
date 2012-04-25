@echo off
cd ..\..\Tools\SharpAssembler.OpcodeWriter
Source\bin\Debug\SharpAssembler.OpcodeWriter.exe -y "Assembler\yasm64.exe" -oc "..\..\SharpAssembler.Architectures.X86\Source\Opcodes" -ot "..\..\SharpAssembler.Architectures.X86\Tests\Opcodes" -r "..\..\Input\X86 Opcodes"
pause