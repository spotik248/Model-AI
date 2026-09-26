//using System;
using ArrayFire;

using static Model.Write;

namespace Model;

public static class Test
{
    public static Action[] tests = [
        TestMouse,
        TestMath,
        TestSound,
        TestMassive,
        TestWords,
        TestGPU
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

    public static void TestGPU()
    {
        try
        {
            // 1. Инициализация в вашем стиле (UPPERCASE)
            Device.SetBackend(Backend.OPENCL); 
            
            Console.WriteLine("ArrayFire успешно переключен на OpenCL!");

            // -------------------------------------------------------------
            // Задача 1: Перенос данных с CPU на GPU и их сложение
            // -------------------------------------------------------------
            double[] cpuA = [1.0, 2.0, 3.0, 4.0];
            double[] cpuB = [10.0, 20.0, 30.0, 40.0];

            // В старых версиях класс может называться Array (с заглавной) или AfArray.
            // Попробуйте один из вариантов ниже:
            var gpuA = Data.CreateArray(cpuA); // Или new AfArray(...)
            var gpuB = Data.CreateArray(cpuB);

            // Пробуем сложение через перегруженный оператор
            var gpuResultSum = gpuA + gpuB;

            Console.WriteLine("--- Результат сложения ---");
            ArrayFire.Util.Print(gpuResultSum);

            

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

}