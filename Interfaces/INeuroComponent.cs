//using System;

namespace Model;

public interface INeuroComponent
{
    string name { get; set; }
    string shortName { get; set; }
    ushort count { get; set; }
    int Size();
}