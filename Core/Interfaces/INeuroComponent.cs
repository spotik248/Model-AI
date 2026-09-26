//using System;

namespace Model;

public interface INeuroComponent : INames
{
    ushort count { get; set; }
    int Size();
}