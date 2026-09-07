//using System;
using System.Text.RegularExpressions; // Регулярные выражения

using static Model.Write;

namespace Model;

public static class MyString
{
    public static int Max(params string[] num)
    {
        int Length = 0;
        foreach(var n in num)
            Length = Math.Max(Length, n.Length);
        return Length;
    }
    
    public static int Min(params string[] num)
    {
        int Length = int.MaxValue;
        foreach(var n in num)
            Length = Math.Min(Length, n.Length);
        return Length;
    }
    
    public static string ReplaceStringSplit(string input, string arg, string to)
    {
        string[] array = input.Split("");
        string[] args = arg.Split("");
        for (int i = 0; i < array.Length; i++)
        {
            for (int j = 0; j < arg.Length; j++)
            {
                if (array[i] == args[j]) array[i] = to;
            }
        }
        return string.Join(" ", array);
    }

    public static string ReplaceString(string input, string arg, string to) => string.Join(" ", ReplaceStringSplit(input, arg, to));
    
    public static string ReplaceChar(string input, string arg, char to)
    {
        string result = "";
        char[] chars = input.ToCharArray();
        char[] args = arg.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            for (int j = 0; j < args.Length; j++)
                if (chars[i] == args[j]) chars[i] = to;
            
            result += chars[i].ToString();
        }
        return result;
    }

    public static string TextRegex(Regex regex, string input)
    {
        MatchCollection matches = regex.Matches(input);
        var list = new List<string>();
        foreach (Match match in matches)
        {
            list.Add(match.Value);
        }
        return string.Join(' ', list.ToArray()); ;
    }

    public static string TextRegex(string regex, string input) => TextRegex(new Regex(regex), input);

    public static string Remove(string text, int removeCount) // TODO: Посути дебаг, но тогда добавь Debug();
    {
        if(removeCount > text.Length) Debug("Размер текста для обрезания намного меньше самого текста");
        return text.Remove(0, Math.Min(removeCount, text.Length));
    }

    public static string ToString<T>(T[][] obj)
    {
        string res = "";
        foreach(var ob in obj)
        {
            foreach(var o in ob) res += o + ", ";
            res += "\n";
        }
        return res;
    }

    public static string ToString(object[][] obj)
    {
        string res = "";
        foreach(var ob in obj)
        {
            foreach(var o in ob) res += o + ", ";
            res += "\n";
        }
        return res;
    }

    public static string ToString<T>(T[] obj)
    {
        string res = "";
        foreach(var ob in obj) res += ob + ", ";
        return res;
    }

    public static string ToString(object[] obj)
    {
        string res = "";
        foreach(var ob in obj) res += ob + ", ";
        return res;
    }

}