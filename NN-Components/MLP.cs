//using System;

using static Model.Write;
using static Model.Library;
using static Model.NN;

namespace Model;

public static class MLP // Matrix Layout Propagation
{
    private static double learningRate => Global.learningRate;

    //Проход

    public static double[][] Pass(double[][] beforeLayout, double[][] thisWidth, double[][] bias)
    {
        for (int i = 0; i < beforeLayout.Length; i++) // size 100
            for (int j = 0; j < beforeLayout[0].Length; j++) // dim 20 
                beforeLayout[i][j] = (beforeLayout[i][j] * thisWidth[i][j]) + bias[i][j]; // Поэлементное умножение и сложение

        return Act(beforeLayout);
    }

    public static double[][] Pass(double[][] beforeLayout, double[][][] thisWidth)
    {
        Mas("GET: ", beforeLayout); Mas("GET: ", thisWidth);
        double[][] allLayout = Init.Double<double>(thisWidth.Length, thisWidth[0].Length);
        allLayout = Matrix.Add(allLayout, CubeMatrix.ToMatrix(CubeMatrix.MultMat(thisWidth, beforeLayout))); 

        return Act(allLayout);
    }

    //Общие дельты и обновление

    public static double[][] Delta(double[][] beforeDelta, double[][] thisLayout, double[][] thisWidth)
    {
        int rows = thisWidth.Length;    // 100
        int cols = thisWidth[0].Length; // 20
        double[][] delta = Init.Double<double>(rows, cols);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                // dX = градиент_сверху * вес * производная_активации
                delta[i][j] = beforeDelta[i][j] * thisWidth[i][j] * DAct(thisLayout[i][j]);
            }
        }
        return delta;
    }

    public static double[][] Width(double[][] oldthisWidth, double[][] beforeLayout, double[][] thisDelta)
    {
        int rows = oldthisWidth.Length;    // 100
        int cols = oldthisWidth[0].Length; // 20
        double[][] newthisWidth = Init.Double<double>(rows, cols);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                // Новый_Вес = Старый_Вес - lr * Дельта * Вход_Слоя
                newthisWidth[i][j] = oldthisWidth[i][j] - learningRate * thisDelta[i][j] * beforeLayout[i][j];
            }
        }
        return newthisWidth;
    }

    public static double[][] Bias(double[][] oldBias, double[][] delta)
    {
        double[][] newBias = Init.Copy(oldBias);

        for (int i = 0; i < oldBias.Length; i++)
        {
            for (int j = 0; j < oldBias[0].Length; j++) // Исправлено: теперь идет до 20, а не до 100
            {
                newBias[i][j] = oldBias[i][j] - learningRate * delta[i][j];
            }
        }
        return newBias;
    }

    // dotnet build -c Release

    private static double[] Act(double[] vector) => FucAct.Sigmoid(vector);
    private static double[][] Act(double[][] matrix) => matrix.Select(FucAct.Sigmoid).ToArray();
    private static double DAct(double x) => FucAct.DSigmoid(x);

    /*
        Sigmoid,  DSigmoid,
        SoftSign, DSoftSign
        ReLu, DReLu,
        LReLu, DLReLu,
    */
}