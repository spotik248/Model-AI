//using System;
//using System.Collections.Generic;

using static Model.Write;

namespace Model;

public static class Mist // Работа с T параметрами
{

    public static T[] Fill<T>(T[] filler, int from, int max)
    {
        if(max > filler.Length)
        {
            Exc("Размер, который необходимо вырезать, гараздо больше размера обрезаемого массива.");
            return [];
        }
        if(max-from <= 0)
        {
            Exc("Размер, который необходимо вырезать, гараздо меньше 0.");
            return [];
        }
        
        T[] result = new T[max-from];

        //CheckIt("length", max-from);
        //CheckIt("max", max);
        //CheckIt("from", from);

        for(int i = 0; i < max-from; i++)
            result[i] = filler[i+from];

        return result;
    }

    public static T[] Fill<T>(T[] filler, int max)
    {
        T[] result = new T[max];

        for(int i = 0; i < max; i++)
            result[i] = filler[i];

        return result;
    }

    public static int Count<T>(this T[] objs)   
    {
        if(objs is null) return 0;

        int count = 0;
        foreach(var obj in objs)
        {
            if(!obj.IsNull()) count++;
        }
        return count;
    }

    public static bool IsNull<T>(this T obj)
    {
        return EqualityComparer<T>.Default.Equals(obj, default);
    }

    public static bool IsArray<T>(this T[] objs)
    {
        return objs.IsArray();
    }

}