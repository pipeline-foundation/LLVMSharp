// Copyright (c) .NET Foundation and Contributors. All Rights Reserved. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using LLVMSharp.Interop;

namespace LLVMSharp;

public sealed class PointerType : Type
{
    internal PointerType(LLVMTypeRef handle) : base(handle, LLVMTypeKind.LLVMPointerTypeKind)
    {
    }

    public uint AddressSpace => Handle.PointerAddressSpace;

    public static PointerType Get(LLVMContext context, uint addressSpace = 0)
    {
        ArgumentNullException.ThrowIfNull(context);
        var handle = context.Handle.CreatePointerType(addressSpace);
        return context.GetOrCreate<PointerType>(handle);
    }
}
