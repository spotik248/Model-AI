//using System;

using static Model.Write;
using static Model.Global;

namespace Model;

public static class NN
{
    public static INeuro[] nn = [];
    public static int size;

    public static int id = 0;
    public static string name = "unknown";
    public static string shortName = "unk";
    public static string desc = "unk";

    

    public static void InitNN(int size) // Нейронные сети
    {
        NN.size = size;

        // L, F, V, T
        nn = [
            new LNN(size/2), // 75 => 750 => 75
            new FNN(size/5),
            new VNN(size),
            new TNN(size)
        ];

        Change(0);
    }



    public static void Next() => Change(id < nn.Length - 1 ? id + 1 : 0);

    public static void Change(string toNN) => Change(int.Parse(toNN));

    public static void Change(int id)
    {
        NN.id = id;

        name = nn[id].name;
        shortName = nn[id].shortName;
        desc = nn[id].desc;

        Line($"Тип нейронной сети изменен на {name} ({shortName})\n{desc}");
    }

    public static void Change() => Change(0);

    public static void SetLearnEpoch(double startLR, int epoches)
    {
        // Начало со множителем обучения
        learningRate = startLR == 0 ? Global.startLR : startLR;

        Global.epoches = epoches; //Медленный спуск
    }
    
    public static void UseUpdateLearningRate(int type, double[] errors, int epoch)
    {
        if(fixedLearningRate) return;
        
        switch (type)
        {
            case 0: learningRate = startLR; break;
            case 1: learningRate = 1.0/(1.0 + 0.01 * epoch); break;                         // Inverse Time Decay (обратное затухание во времени)
            case 2: learningRate = errors[0]/(1.0 + 0.01 * epoch); break;                   // 
            case 3: learningRate = startLR * Math.Pow(0.95, epoch / 100.0); break;            // Exponential Decay (Экспоненциальное затухание)
            case 4: learningRate = startLR * Math.Pow(0.5, Math.Floor(epoch / 500.0)); break; // Step Decay (Пошаговое падение)
            case 5: learningRate = minLR + 0.5 * (startLR - minLR) * (1.0 + Math.Cos(epoch * Math.PI / epoches )); break; //Cosine Annealing (Косинусное затухание)
        }
    }
    
    public static void SetBatch(int batches)
    {
        if(!DEV)
        try
        {
            nn[id].batches = batches;
        }
        catch(Exception ex)
        {
            Exc($"Ошибка при попытке установить количество батчей {nn[id].name}:", ex);
        }
        else nn[id].batches = batches;
    }
    public static double[][]? Predict(double[][] input)
    {
        if(!DEV)
        try
        {
            return nn[id].Predict(input);
        }
        catch(Exception ex)
        {
            Exc($"Ошибка при попытке получении ответа от {nn[id].name}:", ex);
        }
        else return nn[id].Predict(input);
        
        return null;
    }
    public static double[]? Study(double[][] input, double[][] output)
    {
        if(!DEV)
        try
        {
            return nn[id].Study(input, output);
        }
        catch(Exception ex)
        {
            Exc($"Ошибка при попытке обучения {nn[id].name}:", ex);
        }
        else return nn[id].Study(input, output);

        return null;
    }

}