//using System;

using static Model.Write;
using static Model.Library;
using static Model.NManage;
using static Model.Learn;

namespace Model;

public static class VLP // Vector Layout Propagation
{
    private static double learningRate => Learn.learningRate;

    // Проход

    public static double[] Pass(double[] beforeLayout, double[][] thisWidth, double[] bias) // 20000, 20000x100, 100
    {
        if(beforeLayout.Length != thisWidth.Length || thisWidth[0].Length != bias.Length)
            Err($"Incorrect input sizes for Pass:\n({beforeLayout.Length} != {thisWidth.Length}) Or ({thisWidth[0].Length} != {bias.Length})");

        double[] allLayout = Init.Zeros(thisWidth[0].Length); 
        for(int i = 0; i < beforeLayout.Length; i++)
        {
            for(int j = 0; j < thisWidth[0].Length; j++)
                allLayout[j] += beforeLayout[i] * thisWidth[i][j];
        }
        // Если бы ты хотел добавлять bias сразу в for(), то тебе нужно было бы переворачивать всё.    
 
        return Act(Vector.Add(allLayout, bias));
    }

    public static double[] Pass(double[] beforeLayout, double[][] thisWidth)
    {
        Mas("GET: ", beforeLayout); Mas("GET: ", thisWidth);
        double[] allLayout = Init.Zeros(thisWidth.Length);
        for (int i = 0; i < thisWidth.Length; i++)
            allLayout = Vector.Add(allLayout, Vector.MultOne(thisWidth[i], beforeLayout[i]));

        return Act(allLayout);
    }

    //Общие дельты, веса и биасы

    public static double[] Delta(double[] beforeDelta, double[] thisLayout, double[][] thisWidth) // 100, 20000, 20000x100
    {
        if(thisWidth.Length != thisLayout.Length || thisWidth[0].Length != beforeDelta.Length)
            Err($"Incorrect input sizes for Delta:\n({thisWidth.Length} != {thisLayout.Length}) Or ({thisWidth[0].Length} != {beforeDelta.Length})");
        
        double[] delta = new double[thisWidth.Length];

        for (int i = 0; i < thisWidth.Length; i++)
        {
            for (int j = 0; j < thisWidth[0].Length; j++)
                delta[i] += thisWidth[i][j] * beforeDelta[j];
            
            delta[i] *= DAct(thisLayout[i]);
        }
            

        return delta;
    }

    public static double[][] Width(double[][] oldthisWidth, double[] beforeLayout, double[] thisDelta)
    {
        double[][] newthisWidth = Init.Double<double>(oldthisWidth[0].Length, oldthisWidth.Length);
        for (int i = 0; i < thisDelta.Length; i++)
            for (int j = 0; j < oldthisWidth.Length; j++)
                newthisWidth[j][i] = oldthisWidth[j][i] - learningRate * thisDelta[i] * beforeLayout[j];

        //Write.Line($"Прибавленно к весу: {Matrix.Mean(oldthisWidth)} += {Vector.MultAddAll(Init.Full(learningRate, beforeLayout.Length * thisDelta.Length), Vector.MultAllN(thisDelta, beforeLayout)) / (beforeLayout.Length * thisDelta.Length)}");
        return newthisWidth;
    }

    public static double[][] Width(double[][] oldthisWidth, double[] beforeLayout, double[] thisDelta, double learningRate)
    {
        double[][] newthisWidth = Init.Double<double>(oldthisWidth[0].Length, oldthisWidth.Length);
        for (int i = 0; i < thisDelta.Length; i++)
            for (int j = 0; j < oldthisWidth.Length; j++)
                newthisWidth[j][i] = oldthisWidth[j][i] - learningRate * thisDelta[i] * beforeLayout[j];

        //Write.Line($"Прибавленно к весу: {Matrix.Mean(oldthisWidth)} += {Vector.MultAddAll(Init.Full(learningRate, beforeLayout.Length * thisDelta.Length), Vector.MultAllN(thisDelta, beforeLayout)) / (beforeLayout.Length * thisDelta.Length)}");
        return newthisWidth;
    }

    public static double[] Bias(double[] oldBias, double[] delta)
    {
        double[] newBias = new double[oldBias.Length];
        for (int i = 0; i < delta.Length; i++)
            newBias[i] = oldBias[i] - learningRate * delta[i];

        return newBias;
    }

    //GRU

    public static double[] Candidate(double[] beforeLayout, double[][] thisWidth, double[] bias)
    {
        double[] allLayout = Init.Zeros(thisWidth.Length);
        for (int i = 0; i < thisWidth.Length; i++)
            allLayout = Vector.Add(allLayout, Vector.MultOne(thisWidth[i], beforeLayout[i]));

        return Acth(Vector.Add(allLayout, bias));;
    }

    // dotnet build -c Release

    private static double[] Act(double[] vector) => FucAct.Sigmoid(vector);
    private static double[][] Act(double[][] matrix) => matrix.Select(FucAct.Sigmoid).ToArray();
    private static double[][] ActR(double[][] matrix) => matrix.Select(FucAct.ReLu).ToArray();
    private static double[] Acth(double[] vector) => FucAct.Tanh(vector);
    private static double DAct(double x) => FucAct.DSigmoid(x);

    /*
        Sigmoid,  DSigmoid,
        SoftSign, DSoftSign
        ReLu, DReLu,
        LReLu, DLReLu,
    */
}