// Copyright (c) .NET Foundation and Contributors. All Rights Reserved. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using LLVMSharp.Interop;

namespace LLVMSharp;

public sealed class FunctionType : Type
{
    internal FunctionType(LLVMTypeRef handle) : base(handle, LLVMTypeKind.LLVMFunctionTypeKind)
    {
    }

    public static FunctionType Get(Type returnType, Type[] parameterTypes, bool isVarArg = false)
    {
        ArgumentNullException.ThrowIfNull(parameterTypes);
        return Get(returnType, parameterTypes.AsSpan(), isVarArg);
    }

    public static FunctionType Get(Type returnType, ReadOnlySpan<Type> parameterTypes, bool isVarArg)
    {
        ArgumentNullException.ThrowIfNull(returnType);
        var context = returnType.Context;
        var handles = Type.GetHandles(parameterTypes, context, nameof(parameterTypes));
        var handle = LLVMTypeRef.CreateFunction(returnType.Handle, handles, isVarArg);
        return context.GetOrCreate<FunctionType>(handle);
    }

    public bool IsVarArg => Handle.IsFunctionVarArg;

    public uint NumParams => Handle.ParamTypesCount;

    public Type ReturnType => Context.GetOrCreate(Handle.ReturnType);

    public Type[] GetParams()
    {
        var handles = Handle.GetParamTypes();

        if (handles.Length == 0)
        {
            return [];
        }

        var context = Context;
        var result = new Type[handles.Length];

        for (var i = 0; i < result.Length; i++)
        {
            result[i] = context.GetOrCreate(handles[i]);
        }

        return result;
    }
}
