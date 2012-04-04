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

namespace SharpAssembler.Architectures.X86
{
	/// <summary>
	/// Specifies features which may or may not be used by the assembler.
	/// </summary>
	[Flags]
	public enum CpuFeatures : int
	{
		/// <summary>
		/// No features specified.
		/// </summary>
		None = 0x00000000,
		/// <summary>
		/// Floating Point Unit (FPU) instructions.
		/// </summary>
		Fpu = 0x00000001,
		/// <summary>
		/// MMX SIMD instructions.
		/// </summary>
		Mmx = 0x00000002,
		/// <summary>
		/// Streaming SIMD Extensions (SSE) instructions.
		/// </summary>
		Sse = 0x00000004,
		/// <summary>
		/// Streaming SIMD Extensions 2 (SSE2) instructions.
		/// </summary>
		Sse2 = 0x00000008,
		/// <summary>
		/// Streaming SIMD Extensions 3 (SSE3) instructions.
		/// </summary>
		Sse3 = 0x00000010,
		/// <summary>
		/// Supplemental Streaming SIMD Extensions 3 (SSSE3) instructions.
		/// </summary>
		Ssse3 = 0x00000020,
		/// <summary>
		/// Streaming SIMD Extensions 4.1 (SSE4.1), Penryn subset.
		/// </summary>
		Sse4Penryn = 0x00000040,
		/// <summary>
		/// Streaming SIMD Extensions 4.2 (SSE4.2), Nehalem subset.
		/// </summary>
		Sse4Nehalemn = 0x00000080,
		/// <summary>
		/// Streaming SIMD Extensions 4 (SSE4).
		/// </summary>
		Sse4 = Sse4Penryn | Sse4Nehalemn,
		/// <summary>
		/// AMD Streaming SIMD Extensions 4 (SSE4a).
		/// </summary>
		Sse4A = 0x00000100,
		/// <summary>
		/// Streaming SIMD Extensions 5 (SSE5).
		/// </summary>
		Sse5 = 0x00000200,
		/// <summary>
		/// XSAVE instructions.
		/// </summary>
		XSave = 0x00000400,
		/// <summary>
		/// Advanced Vector Extensions (AVX) instructions.
		/// </summary>
		Avx = 0x00000800,
		/// <summary>
		/// Fused Multiply-Add (FMA) instructions.
		/// </summary>
		Fma = 0x00001000,
		/// <summary>
		/// Advanced Encryption Standard (AES) instructions.
		/// </summary>
		Aes = 0x00002000,
		/// <summary>
		/// PCLMULQDQ instruction.
		/// </summary>
		PclMulQdq = 0x00004000,
		/// <summary>
		/// 3DNow! instructions.
		/// </summary>
		Amd3DNow = 0x00008000,
		/// <summary>
		/// Cyrix-specific instructions.
		/// </summary>
		Cyrix = 0x00010000,
		/// <summary>
		/// AMD-specific instructions (older than K6).
		/// </summary>
		Amd = 0x00020000,
		/// <summary>
		/// System Management Mode instructions.
		/// </summary>
		Smm = 0x00040000,
		/// <summary>
		/// Protected mode only instructions.
		/// </summary>
		ProtectedMode = 0x00080000,
		/// <summary>
		/// Undocumented instructions.
		/// </summary>
		Undocumented = 0x00100000,
		/// <summary>
		/// Obsolete instructions.
		/// </summary>
		Obsolete = 0x00200000,
		/// <summary>
		/// Privileged instructions.
		/// </summary>
		Privileged = 0x00400000,
		/// <summary>
		/// Secure Virtual Machine (SVM) instructions.
		/// </summary>
		Svm = 0x00800000,
		/// <summary>
		/// VIA PadLock instructions.
		/// </summary>
		PadLock = 0x01000000,
		/// <summary>
		/// Intel EM64T or better instructions (not necessarily 64-bit only).
		/// </summary>
		EM64T = 0x02000000,
	}
}
