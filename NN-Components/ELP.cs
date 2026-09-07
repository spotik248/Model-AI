//using System;

using static Model.Write;
using static Model.Library;
using static Model.NN;
using static Model.Global;

namespace Model;

public class ELP : INeuroComponent
{
    //Из конструктора

    public string name { get; set; } = "Emotion Layout Perceptron";
    public string shortName { get; set; } = "elp";
    public ushort count { get; set; } = 1; // ushort = 2 байта, от 0 до 65535
    public int Size() => sizeELP * count;

    public static int sizeELP;

    //private static double learningRate = Global.learningRate; // Пока что (иначе придется создавать для каждого отдельный MLP, пока мне так не надо)

    public double[] layout;
    public double[][] width;
    public double[] bias;
    public double[] delta;

    public double[] newDelta;


#pragma warning disable CS8618

    public ELP(int Size)
    {
        sizeELP = Size;
        InitELP();
    }

#pragma warning restore CS8618

    public void InitELP()
    {
        layout ??= Init.Zeros(sizeELP);
        width ??= Init.Zeros(sizeELP, sizeELP);
        bias ??= Init.Zeros(sizeELP);
        delta ??= Init.Zeros(sizeELP);

        //Msg($"Инициализация {name} закончена...");
        Msg($"Initialization {name} complete");

        if (layout == null || width == null || bias == null || delta == null)
            Exc($"Некоторые из параметров {name} остались незаполненными!");
    }

    public double[] Pass(double[] beforeLayout)
    {
        layout = VLP.Pass(beforeLayout, width, bias);

        return layout;
    }

    public double[] Delta(double[] beforeDelta)
    {
        delta = beforeDelta;
        newDelta = VLP.Delta(beforeDelta, layout, width);

        return newDelta;
    }

    public void Width(double[] beforeLayout)
    {
        width = VLP.Width(width, beforeLayout, delta);
    }

    public void Bias()
    {
        bias = VLP.Bias(bias, delta);
    }

    // dotnet build -c Release

    private static double[][] Act(double[][] m) => m.Select(FucAct.LReLu).ToArray();
    private static double[][] DAct(double[][] m) => m.Select(FucAct.DLReLu).ToArray();

    /*
        Sigmoid,  DSigmoid,
        SoftSign, DSoftSign
        ReLu, DReLu,
        LReLu, DLReLu,
    */
}