//using System;

using static Model.Write;
using static Model.Global;

namespace Model;

public static class NManage
{
    public static INeuro[] nn = [];
    public static int size = 0;

    public static int id = 0;
    public static string name = "unknown";
    public static string shortName = "unk";
    public static string desc = "unk";

    static NManage() => Start(100);

    public static void Start(int sizelayout) // Нейронные сети
    {
        size = sizelayout;

        // L, F, V, T
        nn = [
            new LNN(size/2), // Рабочая    // 75 => 750 => 75
            new FNN(size/5), // Недоделана
            new VNN(size),   // Сломано
            new TNN(size)    // Недоделана
        ];

        Change(0);
    }

    public static void Next() => Change(id < nn.Length - 1 ? id + 1 : 0);

    public static void Change(string toNN) => Change(int.Parse(toNN));

    public static void Change(int idNN)
    {
        id = idNN;

        name = nn[id].name;
        shortName = nn[id].shortName;
        desc = nn[id].desc;

        Line($"Тип нейронной сети изменен на {shortName} ({name})\n{desc}");
    }

    public static void Change() => Change(0);
}