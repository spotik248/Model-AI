//using System;
using static Model.Write;
using static Model.NManage;
using static Model.Global;

namespace Model;

public static class Learn
{

    public static bool fixedLearningRate;
    public static double startLR;
    public static double learningRate;
    public static double minLR;
    public static int epoches;
    public static object[] learn = [];

    static Learn() => Start();

    public static void Start()
    {
        fixedLearningRate = false;
        startLR = 0.5;
        learningRate = startLR;
        minLR = 1E-40;
        epoches = 1;

        SetLearningRate(learningRate);

        SetEpoches(epoches);

        learn = [fixedLearningRate, startLR, learningRate, minLR, epoches];
    }

    public static void UseUpdateLearningRate(int type, double[] errors, int epoch)
    {
        if(fixedLearningRate) return;
        
        switch (type)
        {
            // Стандартная, не меняется, пропорциональна стартовой скорости обучения
            case 0: learningRate = startLR; break;
            // Inverse Time Decay (обратное затухание во времени)
            case 1: learningRate = 1.0/(1.0 + 0.01 * epoch); break;
            // Inverse Time Decay Error (обратное затухание во времени на основе ошибки)
            case 2: learningRate = errors[0]/(1.0 + 0.01 * epoch); break;
            // Exponential Decay (Экспоненциальное затухание)
            case 3: learningRate = startLR * Math.Pow(0.95, epoch / 100.0); break;
            // Step Decay (Пошаговое падение)
            case 4: learningRate = startLR * Math.Pow(0.5, Math.Floor(epoch / 500.0)); break;
            //Cosine Annealing (Косинусное затухание)
            case 5: learningRate = minLR + 0.5 * (startLR - minLR) * (1.0 + Math.Cos(epoch * Math.PI / epoches )); break;
        }
    }

    public static void SetLearningRate(double LearningRate)
    {
        if(LearningRate > 0) learningRate = LearningRate;
    }

    public static void SetEpoches(int Epoches)
    {
        epoches = Epoches;
    }
    
    public static void SetBatches(int Batches) // TODO
    {
        // batches = Batches;
        try
        {
            nn[id].batches = Batches;
        }
        catch(Exception ex)
        {
            Exc($"Ошибка при попытке установить количество батчей {nn[id].shortName}: ", ex);
        }
    }

    public static void SetFull(double LearningRate, int Epoches, int Batches)
    {
        SetLearningRate(LearningRate);
        SetEpoches(Epoches);
        SetBatches(Batches);
    }

    public static double[][]? Predict(double[][] input)
    {
        try
        {
            return nn[id].Predict(input);
        }
        catch(Exception ex)
        {
            Exc($"Ошибка при попытке получении ответа от {nn[id].name}: ", ex);
        }
        
        return null;
    }

    public static double[]? Study(double[][] input, double[][] output)
    {
        try
        {
            return nn[id].Study(input, output);
        }
        catch(Exception ex)
        {
            Exc($"Ошибка при попытке обучения {nn[id].name}: ", ex);
        }

        return null;
    }

}