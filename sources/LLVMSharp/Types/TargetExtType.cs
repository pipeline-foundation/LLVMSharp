// Copyright (c) .NET Foundation and Contributors. All Rights Reserved. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using LLVMSharp.Interop;

namespace LLVMSharp;

public sealed class TargetExtType : Type
{
    internal TargetExtType(LLVMTypeRef handle) : base(handle, LLVMTypeKind.LLVMTargetExtTypeKind)
    {
    }

    public string Name => Handle.TargetExtTypeName;

    public uint NumIntParameters => Handle.TargetExtTypeNumIntParams;

    public uint NumTypeParameters => Handle.TargetExtTypeNumTypeParams;

    public static TargetExtType Get(LLVMContext context, string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        return Get(context, name.AsSpan(), [], []);
    }

    public static TargetExtType Get(LLVMContext context, string name, Type[] typeParameters, uint[] intParameters)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(typeParameters);
        ArgumentNullException.ThrowIfNull(intParameters);
        return Get(context, name.AsSpan(), typeParameters.AsSpan(), intParameters.AsSpan());
    }

    public static TargetExtType Get(LLVMContext context, string name, ReadOnlySpan<Type> typeParameters, ReadOnlySpan<uint> intParameters)
    {
        ArgumentNullException.ThrowIfNull(name);
        return Get(context, name.AsSpan(), typeParameters, intParameters);
    }

    public static TargetExtType Get(LLVMContext context, ReadOnlySpan<char> name, ReadOnlySpan<Type> typeParameters, ReadOnlySpan<uint> intParameters)
    {
        ArgumentNullException.ThrowIfNull(context);
        var handles = Type.GetHandles(typeParameters, context, nameof(typeParameters));
        var handle = context.Handle.CreateTargetExtType(name, handles, intParameters);
        return context.GetOrCreate<TargetExtType>(handle);
    }

    public uint GetIntParameter(uint index) => Handle.GetTargetExtTypeIntParam(index);

    public uint[] GetIntParameters()
    {
        var parameters = new uint[NumIntParameters];

        for (var i = 0; i < parameters.Length; i++)
        {
            parameters[i] = GetIntParameter((uint)i);
        }

        return parameters;
    }

    public Type GetTypeParameter(uint index) => Context.GetOrCreate(Handle.GetTargetExtTypeTypeParam(index));

    public Type[] GetTypeParameters()
    {
        var parameters = new Type[NumTypeParameters];

        for (var i = 0; i < parameters.Length; i++)
        {
            parameters[i] = GetTypeParameter((uint)i);
        }

        return parameters;
    }
}
