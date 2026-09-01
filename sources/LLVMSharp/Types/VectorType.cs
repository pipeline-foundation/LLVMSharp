// Copyright (c) .NET Foundation and Contributors. All Rights Reserved. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using LLVMSharp.Interop;

namespace LLVMSharp;

public sealed class VectorType : SequentialType
{
    internal VectorType(LLVMTypeRef handle) : base(handle, LLVMTypeKind.LLVMVectorTypeKind, LLVMTypeKind.LLVMScalableVectorTypeKind)
    {
    }

    public static VectorType Get(Type elementType, uint elementCount, bool scalable = false)
    {
        ArgumentNullException.ThrowIfNull(elementType);
        var context = elementType.Context;
        var handle = scalable
            ? LLVMTypeRef.CreateScalableVector(elementType.Handle, elementCount)
            : LLVMTypeRef.CreateVector(elementType.Handle, elementCount);
        return context.GetOrCreate<VectorType>(handle);
    }

    public bool IsScalable => Handle.Kind == LLVMTypeKind.LLVMScalableVectorTypeKind;

    public uint NumElements => Handle.VectorSize;
}
