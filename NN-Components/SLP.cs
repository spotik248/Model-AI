//using System;

using static Model.Write;
using static Model.Library;
using static Model.NN;

namespace Model;

public class SLP : INeuroComponent
{
    //Из конструктора

    public string name { get; set; } = "Second Layout Perceptron";
    public string shortName { get; set; } = "selp";
    public ushort count { get; set; } = 2; // ushort = 2 байта, от 0 до 65535
    public static int sizeNN;
    public int Size() => sizeNN * count;
    
    private static int size => size;
    private static int dimension => dimension;
    private static double learningRate = Global.learningRate; // Пока что (иначе придется создавать для каждого отдельный MLP, пока мне так не надо)

    public double[][] firstLayout; public double[][] secondLayout;
    public double[][] actFirstLayout; public double[][] actSecondLayout;
    public double[][] dFirstLayout; public double[][] dSecondLayout;
    public double[][] dActFirstLayout; public double[][] dActSecondLayout;

#pragma warning disable CS8618

    public SLP(int size)
    {
        sizeNN = size;
        
        firstLayout ??= Init.Zeros(size, size);
        secondLayout ??= Init.Zeros(size, size);
        actFirstLayout ??= Init.Zeros(size, size);
        actSecondLayout ??= Init.Zeros(size, size);

        dFirstLayout ??= Init.Zeros(size, size);
        dSecondLayout ??= Init.Zeros(size, size);
        dActFirstLayout ??= Init.Zeros(size, size);
        dActSecondLayout ??= Init.Zeros(size, size);

        //Msg($"Инициализация {name} закончена...");
        Msg($"Initialization {name} complete");

        if (firstLayout == null || secondLayout == null || actFirstLayout == null || actSecondLayout == null ||
        dFirstLayout == null || dSecondLayout == null || dActFirstLayout == null || dActSecondLayout == null)
            Exc($"Некоторые из слоев {name} остались незаполненными!");
    }

#pragma warning restore CS8618

    public double[][] Pass(double[][] beforeLayout, double[][][] thisWidth, double[][] bias)
    {
        firstLayout = Matrix.MultMat(thisWidth[0], beforeLayout);
        actFirstLayout = Act(firstLayout);
        secondLayout = Matrix.MultMat(thisWidth[1], actFirstLayout);
        actSecondLayout = Act(Matrix.Add(secondLayout, bias));

        // for(int i = 0; i < actSecondLayout.Length; i++)
        //     for(int j = 0; j < actSecondLayout[0].Length; j++)
        //         actSecondLayout[i][j] = double.IsNaN(actSecondLayout[i][j]) ? 1 : actSecondLayout[i][j];

        return Matrix.Add(actSecondLayout, beforeLayout);
    }

    public double[][] Delta(double[][] beforeDelta, double[][] thisLayout, double[][][] thisWidth, double[][] thisBias)
    {
        dActSecondLayout = beforeDelta;

        dSecondLayout = Matrix.Mult(dActSecondLayout, DAct(Matrix.Add(secondLayout, thisBias)));
        dActFirstLayout = Matrix.MultMat(dActSecondLayout, thisWidth[1]);
        dFirstLayout = Matrix.MultTrans(dActFirstLayout, DAct(firstLayout));

        double[][] sumDelta = Matrix.DivOne(beforeDelta, beforeDelta.Length);
        double[][] delta = Matrix.AddVec(Matrix.MultMat(thisWidth[0], thisLayout), Matrix.ToVector(sumDelta));

        return delta;
    }

    public double[][][] Width(double[][][] oldthisWidth, double[][] thisLayout)
    {
        double[][][] newthisWidth = Init.Triple<double>(oldthisWidth.Length, oldthisWidth[0].Length, oldthisWidth[0][0].Length);
        double[][] firstGrad = Init.Double<double>(dFirstLayout.Length, dActFirstLayout.Length);
        double[][] secondGrad = Init.Double<double>(dSecondLayout.Length, thisLayout.Length);

        for (int i = 0; i < dFirstLayout.Length; i++)
            for (int j = 0; j < dActFirstLayout.Length; j++)
                for (int k = 0; k < dFirstLayout[0].Length; k++)
                    firstGrad[i][j] = dFirstLayout[i][k] * dActFirstLayout[j][k];
        
        for (int i = 0; i < dSecondLayout.Length; i++)
            for (int j = 0; j < thisLayout.Length; j++)
                for (int k = 0; k < dSecondLayout[0].Length; k++)
                    secondGrad[i][j] = dSecondLayout[i][k] * thisLayout[j][k];
        
        newthisWidth[0] = Matrix.Add(oldthisWidth[0], Matrix.MultOne(firstGrad, learningRate));
        newthisWidth[1] = Matrix.Add(oldthisWidth[1], Matrix.MultOne(secondGrad, learningRate));

        return newthisWidth;
    }

    public double[][] Bias(double[][] oldBias)
    {
        double[][] newBias = Init.Double<double>(oldBias.Length, oldBias[0].Length);
        for (int i = 0; i < oldBias.Length; i++)
            for (int j = 0; j < oldBias[0].Length; j++)
                newBias[i][j] = oldBias[i][j] + learningRate * dFirstLayout[i][j];

        return newBias;
    }

    // dotnet build -c Release

    private static double[][] Act(double[][] matrix) => matrix.Select(FucAct.LReLu).ToArray();
    private static double[][] DAct(double[][] matrix) => matrix.Select(FucAct.DLReLu).ToArray();

    /*
        Sigmoid,  DSigmoid,
        SoftSign, DSoftSign
        ReLu, DReLu,
        LReLu, DLReLu,
    */
}