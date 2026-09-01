// Copyright (c) .NET Foundation and Contributors. All Rights Reserved. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using LLVMSharp.Interop;

namespace LLVMSharp;

public sealed class ArrayType : SequentialType
{
    internal ArrayType(LLVMTypeRef handle) : base(handle, LLVMTypeKind.LLVMArrayTypeKind)
    {
    }

    public static ArrayType Get(Type elementType, ulong numElements)
    {
        ArgumentNullException.ThrowIfNull(elementType);
        var context = elementType.Context;
        var handle = LLVMTypeRef.CreateArray2(elementType.Handle, numElements);
        return context.GetOrCreate<ArrayType>(handle);
    }

    public ulong NumElements => Handle.ArrayLength2;
}
