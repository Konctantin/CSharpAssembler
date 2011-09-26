; Part of the SharpAssembler project
; Intended to be assembled with YASM, as follows:
; yasm -f bin -o helloworld.bin helloworld.asm

[BITS 32]

SECTION .text
main:
	mov edx, len
	mov ecx, msg
	mov ebx, 1
	mov eax, 4
	int 0x80
	
	mov ebx, 0
	mov eax, 1
	int 0x80

SECTION .data align=16
msg: db "Hello world",10
len: equ $-msg