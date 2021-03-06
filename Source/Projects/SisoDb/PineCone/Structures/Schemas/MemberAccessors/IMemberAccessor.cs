﻿using System;

namespace SisoDb.PineCone.Structures.Schemas.MemberAccessors
{
    public interface IMemberAccessor
    {
        string Path { get; }
        Type DataType { get; }
    }
}