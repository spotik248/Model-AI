//using System;

using static Model.Write;

//dotnet build -c Release

namespace Model;

internal class Global
{
    #pragma warning disable CS8618

    public static void Start()
    {
        isBegin = false;
        Line("Инициализация...");

        // Модель

        // Библиотека
        // Library.InitWords(100, 10); // 100 слов по 10 размера
        // Обучение
        // Learn.Start();
        // Файловая система
        // FileSystem.Start();
        // Форма
        // CanvasManage.Start();
        // Нейронная сеть
        // NManage.Start(100);

        Msg("##########XXXXXXXXXXX##########");
        Msg("########## NEW START ##########");
        Msg("##########XXXXXXXXXXX##########");
        
        MsgLine("Инициализация Данных закончена...");

        isBegin = true;
    }

    //Глобальные переменные

    public static bool isBegin { get; private set; } = false;
    internal const bool DEV = true;
    //public static Cmd[] cmds;
    public static Dictionary<string, object> variable;

    #pragma warning restore CS8618


    public async static Task IsBeginAsync()
    {
        while (isBegin == false) await Task.Delay(100); // Ждем загрузки Global
    }

    public static void IsBegin()
    {
        while (isBegin == false) Thread.Sleep(100); // Ждем загрузки Global
    }
    
    public static async Task WaitLoadThreadForm(Thread threadForm)
    {
        while (!threadForm.IsAlive || Application.OpenForms.Count <= 0)
        {
            // Освобождает поток на 100 мс для других задач
            await Task.Delay(100); // Ждем запуска формы
        }
    }

    public static async Task WaitLoadForm(Form form)
    {
        while (form == null)
        {
            // Освобождает поток на 100 мс для других задач
            await Task.Delay(100); // Ждем запуска формы
        }
    }

    public static void ExcThrow(Action method, string ExcMsg)
    {
        try
        {
            method.Invoke();
        }
        catch(Exception ex)
        {
            Throw(ExcMsg, ex);
        }
    }


    public static string CombineFull(string color1, string type, string color2) => $"[{Color[color1]}{type}{Color[color2]}]";
    public static string CombineFull(string color, string type) => $"[{Color[color]}{type}{Color[color]}]";
    public static string Combine(string color, string type) => $"[{Color[color]}{type}{Color[""]}]";
    public static string Combine(string type) => $"[{type}]";

    public static Dictionary<string, string> Color = new()
    {
        {"", "\x1b[0m"},
        {"normal", "\x1b[0m"},
        {"black", "\x1b[30m"},
        {"red", "\x1b[31m"},
        {"green", "\x1b[32m"},
        {"yellow", "\x1b[33m"},
        {"blue", "\x1b[34m"},
        {"purple", "\x1b[35m"},
        {"cyan", "\x1b[36m"},
        {"white", "\x1b[37m"},
        {"bgblack", "\x1b[38m"},
        {"bgred", "\x1b[39m"},
        {"bggreen", "\x1b[40m"},
        {"bgyellow", "\x1b[41m"},
        {"bgblue", "\x1b[42m"},
        {"bgpurple", "\x1b[43m"},
        {"bgcyan", "\x1b[44m"},
        {"bgwhite", "\x1b[45m"},
    };
    
    public static Dictionary<string, string> Comb = new()
    {
        {"", ""},
        {"line", $"[{Color["green"]}Line{Color[""]}]"},

        {"msg", $"[{Color["blue"]}Msg{Color[""]}]"},
        {"debug", $"[{Color["yellow"]}Debug{Color[""]}]"},
        {"err", $"[{Color["red"]}Err{Color[""]}]"},

        {"msgline", $"[{Color["blue"]}Msg{Color["green"]}Line{Color[""]}]"},
        {"mas", $"[{Color["cyan"]}Mas{Color[""]}]"},
        {"quest", $"[{Color["cyan"]}Quest{Color[""]}]"},

        {"checkit", $"[{Color["purple"]}CHECKIT!{Color[""]}]"},
        {"exc", $"[{Color["purple"]}Exc{Color[""]}]"},
        {"fatal", $"[{Color["purple"]}Fatal{Color[""]}]"}
    };

    public static Dictionary<string, string> SComb = new()
    {
        {"", ""},
        {"line", $"[Line]"},

        {"msg", $"[Msg]"},
        {"debug", $"[Debug]"},
        {"err", $"[Err]"},

        {"msgline", $"[MsgLine]"},
        {"mas", $"[Mas]"},
        {"quest", $"[Quest]"},

        {"checkit", $"[CHECKIT!]"},
        {"exc", $"[Exc]"},
        {"fatal", $"[Fatal]"}
    };

    public static Dictionary<string, byte> Keys = new()
    {
        {"0", 0x30}, {"1", 0x31}, {"2", 0x32}, {"3", 0x33}, {"4", 0x34},
        {"5", 0x35}, {"6", 0x36}, {"7", 0x37}, {"8", 0x38}, {"9", 0x39},
        {"a", 0x41}, {"b", 0x42}, {"c", 0x43}, {"d", 0x44}, {"e", 0x45},
        {"f", 0x46}, {"g", 0x47}, {"h", 0x48}, {"i", 0x49}, {"j", 0x4A},
        {"k", 0x4B}, {"l", 0x4C}, {"m", 0x4D}, {"n", 0x4E}, {"o", 0x4F},
        {"p", 0x50}, {"q", 0x51}, {"r", 0x52}, {"s", 0x53}, {"t", 0x54},
        {"u", 0x55}, {"v", 0x56}, {"w", 0x57}, {"x", 0x58}, {"y", 0x59},
        {"z", 0x5A},
        {"back", 0x08}, {"shift", 0x10}, {"ctrl", 0x11}, {"alt", 0x12},
        {"esc", 0x1B}, {"space", 0x20}
    };

    public enum KeyMessages
    {
        WM_KEYDOWN = 0x0100,
        WM_KEYUP = 0x0101,
        WM_LBUTTONDOWN = 0x0201,
        WM_RBUTTONDOWN = 0x0204,
        WM_MBUTTONDOWN = 0x0207,
        WM_LBUTTONUP = 0x0202,
        WM_RBUTTONUP = 0x0205,
        WM_MBUTTONUP = 0x0208
    }

}