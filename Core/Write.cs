using System.Diagnostics;
using System.Reflection;
//using System.IO;

//using static Model.Global; //Не будет ли перегрузки стека?

namespace Model;

public static class Write
{
    
    //Начало
    #region Begin
    
    private const bool DEV = Global.DEV;
    public static bool writeCheckIt = DEV;
    public static string WriteLog => FileSystem.GetPathFile("line_write");
    public static string ColorLog => FileSystem.GetPathFile("line_color");
    public static string BatWriteLog => FileSystem.GetPathFile("read_line");
    public static string BatColorLog => FileSystem.GetPathFile("read_color");

    public static Dictionary<string, string> Color => Global.Color;
    public static Dictionary<string, string> Comb => Global.Comb;
    public static Dictionary<string, string> SComb => Global.SComb;

    //public static string FromExc(Exception exc) => $"Exc & Message: {exc.Message} & StackTrace: {exc.StackTrace} & Inner: {exc.InnerException} & Data: {exc.Data}";

    public static string FromExc(Exception exc) => exc.ToString();
    private static double Round(double x, int n = 3) => Math.Round(x, n);
    private static double[] Round(double[] mas, int n = 3) => Math.Round(mas, n);
    private static double[][] Round(double[][] mas, int n = 3) => Math.Round(mas, n);

    static Write() => InitWrite();

    public async static void InitWrite()
    {
        await Global.IsBeginAsync();
        await WaitFileExistsAsync(BatWriteLog, WriteLog);
        await WaitFileExistsAsync(BatColorLog, ColorLog);
    }

    public static async Task WaitFileExistsAsync(string batPath, string path)
    {
        for (int i = 0; (!File.Exists(batPath) || !File.Exists(path)) && i < 10; i++)
        {
            MsgLine($"waiting for the path to be created to logs... ({i * 0.1:0.0}s)");
            
            // Освобождает поток на 100 мс для других задач
            await Task.Delay(100);
        }

        File.WriteAllText(batPath, $"chcp 65001\ntype {path}\npause");
    }

    #endregion
    
    // Логи
    #region Logging

    // Если ввод только msg, то object?. Если кроме msg есть еще что-то то object.

    public static void Clear() => Console.Clear();
    public static void Space() => Console.WriteLine();
    public static void Line(object? msg) => Console.WriteLine(msg);
    public static void Line(params object[] msg) => Console.WriteLine(string.Join("\n", msg));
    public static void LineAdd(object? msg) => Console.Write(msg);
    public static void Full<T>(string? separator, T[][] obj) => Console.WriteLine(string.Join(separator+"\n", obj.Select(obj2 => string.Join(separator, obj2))));
    public static void Full<T>(string? separator, T[] obj) => Console.WriteLine(string.Join(separator, obj));
    public static void Full<T>(T[] obj) => Console.WriteLine($"[{string.Join(", ", obj)}]");
    public static void Full(double[] obj) => Console.WriteLine($"[{string.Join(", ", Math.Round(obj))}]");

    public static void Msg(object? msg, byte og = 2) => PreWrite("msg", msg, og);
    
    public static void MsgLine(object? msg, byte og = 2)
    {
        Line(msg);
        PreWrite("msgline", msg, og);
    }

    public static void MsgLine(object? msg1, object? msg2, byte og = 2)
    {
        Line(msg1);
        PreWrite("msgline", msg2, og);
    }

    public static void Debug(object? msg, byte og = 2) => PreWrite("debug", msg, og);

    public static void Err(object? msg, byte og = 2) => PreWriteSound("err", msg, og);

    public static void Mas<T>(object msg, T obj, byte og = 2) => PreWriteMas("mas", msg, obj, og);

    public static void Mas<T>(object msg, T[] obj, byte og = 2) => PreWriteMas("mas", msg, obj, og);

    public static void Mas<T>(object msg, T[][] obj, byte og = 2) => PreWriteMas("mas", msg, obj, og);

    public static void Mas(object msg, double obj, byte og = 2) => PreWriteMas("mas", msg, Round(obj), og);

    public static void Mas(object msg, double[] obj, byte og = 2) => PreWriteMas("mas", msg, Round(obj), og);

    public static void Mas(object msg, double[][] obj, byte og = 2) => PreWriteMas("mas", msg, Round(obj), og);

    public static void Exc(object? msg, Exception exc, byte og = 2) => PreWriteSound("exc", msg += "\n"+FromExc(exc), og);

    public static void Exc(object? msg, byte og = 2) => PreWriteSound("exc", msg, og);

    public static void Throw(object? msg, Exception exc, byte og = 2)
    {
        PreWriteSound("fatal", msg+"\n"+FromExc(exc), og);
        MsgLine($"Ой... Ошибка.\n{msg}...");
        Util.ExecuteFile(BatColorLog);
        Thread.Sleep(4000);
        Environment.Exit(0x800000);
    }

    public static void Throw(object? msg, byte og = 2)
    {
        PreWriteSound("fatal", msg, og);
        MsgLine($"Ой... Ошибка.\n{msg}...");
        Util.ExecuteFile(BatColorLog);
        Thread.Sleep(4000);
        Environment.Exit(0x800000);
    }

    public static void CheckIt(object? msg, byte og = 2) => PreWriteSound("checkit", msg, og);
    public static void CheckIt<T>(object msg, T obj, byte og = 2) => PreWriteMasSound("checkit", $"'{msg}'", obj, og);
    public static void CheckIt<T>(object msg, T[] obj, byte og = 2) => PreWriteMasSound("checkit", $"'{msg}'", obj, og);
    public static void CheckIt<T>(object msg, T[][] obj, byte og = 2) => PreWriteMasSound("checkit", $"'{msg}'", obj, og);
    public static void CheckIt(object msg, double obj, byte og = 2) => PreWriteMasSound("checkit", $"'{msg}'", Round(obj), og);
    public static void CheckIt(object msg, double[] obj, byte og = 2) => PreWriteMasSound("checkit", $"'{msg}'", Round(obj), og);
    public static void CheckIt(object msg, double[][] obj, byte og = 2) => PreWriteMasSound("checkit", $"'{msg}'", Round(obj), og);

    public static void QuestYesNo(object? quest, Action method1, Action method2, Action defaultMethod, byte og = 2)
    {
        PreWrite("quest", "QuestYesNoDefault =>", og);

        MsgLine(quest);
        MsgLine("[д] - Да, [н] - Нет");
        LineAdd("\n>");
        switch(Console.ReadLine()?.ToLower().Trim())
        {
            case "д": method1(); break;
            case "н": method2(); break;
            default: defaultMethod(); break;
        }
    }

    public static void QuestYesNo(object? quest, Action method1, Action method2, byte og = 2)
    {
        PreWrite("quest", "QuestYesNo =>", og);

        MsgLine(quest);
        MsgLine("[д] - Да, [н] - Нет");
        LineAdd("\n>");
        switch(Console.ReadLine()?.ToLower().Trim())
        {
            case "д": method1(); break;
            default: method2(); break;
        }
    }

    public static void Quest(object? quest, string[][] choose, Action defaultMethod, Action[] methods, byte og = 2)
    {
        Quest(quest, choose, methods, og+=1);

        defaultMethod.Invoke(); //TODO: Он не работает как исключение для Quest
    }

    // TODO: слишком долго висел TODO в Model, поэтому теперь он здесь...
    public static void Quest(object? quest, string[][] choose, Action[] methods, byte og = 2)
    {
        PreWrite("quest", "Quest =>", og);

        if(choose.Length != methods.Length || choose[0].Length != 2)
            Err($"Choose at Question not has right form\n({choose.Length} != {methods.Length} or {choose[0].Length} != 2)");

        string[] between = new string[choose.Length];

        for(int i = 0; i < choose.Length; i++)
            between[i] = $"[{choose[i][0]}] - {choose[i][1]}";

        string chooses = string.Join(", ", between);
        MsgLine("\n"+quest);
        MsgLine(chooses);
        LineAdd("\n>");
        string? choosing = Console.ReadLine()?.ToLower().Trim();
        for(int i = 0; i < choose.Length; i++)
            if(choose[i][0] == choosing)
            {
                methods[i]();
                Msg($"Choosed method {methods[i]}");
                return;
            }
    }

    #endregion
    
    // Пре
    #region Pre

    static void PreWriteMasSound<T>(string comb, object? msg, T mas, byte og)
    {
        Sound.PlayErrorSound();
        PreWriteMas(comb, msg, mas, og+=1);
    }

    static void PreWriteMasSound<T>(string comb, object? msg, T[] mas, byte og)
    {
        Sound.PlayErrorSound();
        PreWriteMas(comb, msg, mas, og+=1);
    }

    static void PreWriteMasSound<T>(string comb, object? msg, T[][] mas, byte og)
    {
        Sound.PlayErrorSound();
        PreWriteMas(comb, msg, mas, og+=1);
    }

    static void PreWriteMas<T>(string comb, object? msg, T mas, byte og)
    {
        PreWriteAdd(comb, msg, FormateMas(mas), og+=1);
    }

    static void PreWriteMas<T>(string comb, object? msg, T[] mas, byte og) 
    { 
        PreWriteAdd(comb, msg, FormateMas(mas), og+=1);
    }

    static void PreWriteMas<T>(string comb, object? msg, T[][] mas, byte og)
    {
        PreWriteAdd(comb, msg, FormateMas(mas), og+=1);
    }
    
    #endregion
    
    // Конечные Пре
    #region End Pre

    static void PreWriteSound(string comb, object? msg, byte og)
    {
        Sound.PlayErrorSound();

        var (logText, colorText) = Formate(comb, msg, og);
        if(writeCheckIt && comb == "checkit") Line(logText);
        WriteToLog(logText, colorText);
    }

    static void PreWriteAdd(string comb, object? msg, object? add, byte og)
    {
        var (logText, colorText) = Formate(comb, msg, og);
        if(writeCheckIt && comb == "checkit") Line(logText+=add);
        WriteToLog(logText+=add, colorText+=add);
    }

    static void PreWrite(string comb, object? msg, byte og)
    {
        var (logText, colorText) = Formate(comb, msg, og);
        
        if(writeCheckIt && comb == "checkit")
            Line(logText);

        if(DEV) WriteToLog(logText, colorText);
    }

    static void WriteToLog(string logText, string colorText)
    {
        if(DEV)
        {
            try
            {
                File.AppendAllText(WriteLog, logText+"\n");
                File.AppendAllText(ColorLog, colorText+"\n");
            }
            catch (Exception ex)
            {
                Exc($"Error during writing message to log: ", ex);
            }
        }
    }

    #endregion
    
    // Форматы
    #region Formates

    public static (string, string) Formate(string comb, object? msg, StackFrame? frame)
    {
        string time = Computer.Time();
        string name = GetName(frame);
        string text = $"{time} {name}: {msg}";
        string logText = $"{SComb[comb]} {text}";
        string colorText = $"{Comb[comb]} {text}";

        return (logText, colorText);
    }

    public static (string, string) Formate(string comb, object? msg, byte og)
    {
        string time = Computer.Time();
        string name = GetName(og+=1);
        string text = $"{time} {name}: {msg}";
        string logText = $"{SComb[comb]} {text}";
        string colorText = $"{Comb[comb]} {text}";

        return (logText, colorText);
    }

    public static string FormateMas<T>(T mas)
    {
        string add = $"\n   [{mas}]\n";

        return add;
    }

    public static string FormateMas<T>(T[] mas)
    {
        int l1 = mas.Length;
        string add = "\n    [EMPTY]"; // Значение по умолчанию на случай ошибки
        
        try
        {
            if (l1 == 1)
            {
                add = $"\n    [{mas[0]}]";
            }
            else if (l1 == 2)
            {
                add = $"\n    [{mas[0]}, {mas[l1-1]}]";
            }
            else if (l1 >= 3)
            {
                add = $"\n    [{mas[0]}, {mas[l1/2]}, {mas[l1-1]}]";
            }
        }
        catch(Exception ex)
        {
            Exc("Не удалось создать вектор-сообщение для логов: ", ex);
        }

        add += $"\n<--{l1}\n";

        return add;
    }

    public static string FormateMas<T>(T[][] mas)
    {
        int l1 = mas.Length;

        string add = $"\n   [EMPTY]"
                   + $"\n<--{l1}x{0}\n";

        try
        {
            if(l1 > 0)
            {
                int l2 = mas[0].Length;

                if (l1 == 1)
                {
                    if (l2 == 1)
                    {
                        add = $"\n    [{mas[0][0]}]"
                            + $"\n<--{l1}x{l2}\n";
                    }
                    else if (l2 == 2)
                    {
                        add = $"\n    [{mas[0]}, {mas[l1-1]}]"
                            + $"\n<--{l1}x{l2}\n";
                    }
                    else if (l2 >= 3)
                    {
                        add = $"\n    [{mas[0]}, {mas[l1/2]}, {mas[l1-1]}]"
                            + $"\n<--{l1}x{l2}\n";
                    }
                }
                else if (l1 == 2)
                {
                    if (l2 == 1)
                    {
                        add = $"\n    [{mas[0][0]}]"
                            + $"\n    [{mas[l1-1][0]}]"
                            + $"\n<--{l1}x{l2}\n";
                    }
                    else if (l2 == 2)
                    {
                        add = $"\n    [{mas[0][0]}, {mas[0][l2-1]}]"
                            + $"\n    [{mas[l1-1][0]}, {mas[l1-1][l2-1]}]"
                            + $"\n<--{l1}x{l2}\n";
                    }
                    else if (l2 >= 3)
                    {
                        add = $"\n    [{mas[0][0]}, {mas[0][l2/2]}, {mas[0][l2-1]}]"
                            + $"\n    [{mas[l1-1][0]}, {mas[l1-1][l2/2]}, {mas[l1-1][l2-1]}]"
                            + $"\n<--{l1}x{l2}\n";
                    }
                }
                else if (l1 >= 3)
                {
                    if (l2 == 1)
                    {
                        add = $"\n    [{mas[0][0]}]"
                            + $"\n    [{mas[l1/2][0]}]"
                            + $"\n    [{mas[l1-1][0]}]"
                            + $"\n<--{l1}x{l2}\n";
                    }
                    else if (l2 == 2)
                    {
                        add = $"\n    [{mas[0][0]}, {mas[0][l2/2]}]"
                            + $"\n    [{mas[l1/2][0]}, {mas[l1/2][l2/2]}]"
                            + $"\n    [{mas[l1-1][0]}, {mas[l1-1][l2/2]}]"
                            + $"\n<--{l1}x{l2}\n";
                    }
                    else if (l2 >= 3)
                    {
                        add = $"\n    [{mas[0][0]}, {mas[0][l2/2]}, {mas[0][l2-1]}]"
                            + $"\n    [{mas[l1/2][0]}, {mas[l1/2][l2/2]}, {mas[l1/2][l2-1]}]"
                            + $"\n    [{mas[l1-1][0]}, {mas[l1-1][l2/2]}, {mas[l1-1][l2-1]}]"
                            + $"\n<--{l1}x{l2}\n";
                    }
                }
            }
        }
        catch(Exception ex) {
            Exc("Не удалось создать матрицу-сообщение для логов: ", ex);
        }

        return add;
    }

    public static string GetName(MethodBase? callerMethod) => callerMethod?.ReflectedType?.Name ?? "unk";
    public static string GetName(StackFrame? stackFrame) => GetName(stackFrame?.GetMethod());
    public static string GetName(StackTrace stackTrace, byte og = 2) => GetName(stackTrace.GetFrame(og));
    public static string GetName(byte og) => GetName(new StackTrace().GetFrame(og+=1));
    
    #endregion

}