// Copyright (c) .NET Foundation and Contributors. All Rights Reserved. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from https://github.com/llvm/llvm-project/tree/llvmorg-20.1.2/llvm/include/llvm-c
// Original source is Copyright (c) the LLVM Project and Contributors. Licensed under the Apache License v2.0 with LLVM Exceptions. See NOTICE.txt in the project root for license information.

using System.Runtime.InteropServices;

namespace LLVMSharp.Interop;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public unsafe delegate int LLVMOpInfoCallback(void* DisInfo, [NativeTypeName("uint64_t")] ulong PC, [NativeTypeName("uint64_t")] ulong Offset, [NativeTypeName("uint64_t")] ulong Size, int TagType, void* TagBuf);
