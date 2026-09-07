//using System;

using static Model.Write;

namespace Model;

public static class Test
{
    public static Action[] tests = [
        TestMouse,
        TestMath,
        TestSound,
        TestMassive,
        TestWords
    ];
    private static bool testmode = false;
    
    // Для тестов
    private static Dictionary<string, byte> Keys => Global.Keys;
    

    public static void TestMode()
    {
        testmode = !testmode;
        MsgLine($"Режим изменен на {testmode}");
    }

    public static void Testing(string[] arg)
    {
        if(arg.Length == 0) tests[0]();
        else
        {
            int i = int.Parse(arg[0]);
            if(i < tests.Length) tests[i]();
            else MsgLine("Такого теста еще не существует");
        }
    }

    public static void Testing(params int[] arg)
    {
        if(arg.Length == 0) tests[0]();
        else tests[arg[0]]();
    }

    public static void TestMouse()
    {
        MsgLine($"Тест запущен #1");
        Keyboard.StartHook();
        Mouse.StartHook();
        Thread.Sleep(1000);
        Keyboard.JumpKey(Keys["a"]);
        Mouse.MoveMouseTo(100, 100);
        Thread.Sleep(1000);
        Keyboard.JumpKey(Keys["b"]);
        Mouse.MoveMouseTo(400, 100);
        Thread.Sleep(1000);
        Keyboard.JumpKey(Keys["o"]);
        Mouse.MoveMouseTo(400, 400);
        Thread.Sleep(1000);
        Keyboard.JumpKey(Keys["b"]);
        Mouse.MoveMouseTo(100, 400);
        Thread.Sleep(1000);
        Keyboard.JumpKey(Keys["a"]);
        Mouse.MoveMouseTo(100, 100);
        Mouse.StopHook();
        Keyboard.StopHook();
        MsgLine($"Тест окончен #1");
    }

    public static void TestMath()
    {
        MsgLine($"Тест математики #1");
        double[] t = [ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 ];
        double a = Math.MeanVector(Init.Full(0.5, 10), t);
        MsgLine($"{a}");
        MsgLine($"Тест математики #1");
    }

    public static void TestSound()
    {
        float[] s = Sound.ReadMp3(@"Sounds/Err.mp3");
        double[] d = s.Select(x => (double)x).ToArray();
        double[] g = FucAct.Sigmoid(d);
        g = Math.Round(g);
        Mas("Звук: ", g);
        Full(g);
        Line(Math.MakeGrafic(g, 30));
        MsgLine("Тест на звук окончен");
    }

    public static void TestMassive()
    {
        Word[] words = new Word[10];
        
        words[0] = new("abc", Init.Randomized(10));
        words[1] = new("def", Init.Randomized(20));


        int Length = words.Count();

        MsgLine($"Массив: [] => ");
    }

    public static void TestWords()
    {
        Word a = new("apple", [1, 2, 3]);
        CheckIt("a to string", a.ToString());
    }

}