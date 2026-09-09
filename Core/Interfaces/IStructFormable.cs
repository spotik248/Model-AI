//using System;

namespace Model;

public interface IStructFormable
{
    // Уже инициируется с помощью ValueType, который обычное дело для Struct
    public string? ToString();

    // Указывается IsArray() для всех массивов
    public bool IsArray();

    // Просто ссылка на Mist.IsNull(this); Пишется как public static bool IsNull() => IsNull();
    public static abstract bool IsNull();
}