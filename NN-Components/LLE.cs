//using System;

using static Model.Write;
using static Model.Library;
using static Model.NN;

namespace Model;

public static class LLE // Layout Local Error
{
    public static double stud = Global.learningRate;
    

    // Самый обычный слой в перцептроне
    public static double[] Pass(double[] inputLayout, double[][] width, double[] bias)
    {
        double[] hiddenLayout = Init.Zeros(width[0].Length);
        for(int i = 0; i < width[0].Length; i++)
        {
            double mem = 0;
            for(int j = 0; j < inputLayout.Length; j++)
                mem += inputLayout[j] * width[j][i];

            hiddenLayout[i] = Act(mem + bias[i]);
        }
        return hiddenLayout;
    }

    // Возвращает локальную ошибку от следующего слоя (возможно выходного слоя)
    public static double[] Error(double[] hiddenLayout, double[][] width, double[][] nextWidth, double[] nextHiddenError)
    {
        double[] hiddenError = Init.Zeros(width[0].Length); // Ошибка для нового, более раннего слоя

        for (int i = 0; i < hiddenLayout.Length; i++) // Идем по нейронам нового слоя
        {
            double sumFeedback = 0;
            for (int j = 0; j < nextHiddenError.Length; j++) // Собираем ошибку со следующего скрытого слоя
            {
                sumFeedback += nextHiddenError[j] * nextWidth[i][j]; 
            }
            hiddenError[i] = sumFeedback;
        }
        return hiddenError;
    }

    // Возвращает дельту от выходного слоя
    public static double[] Delta(double[] outputLayout, double[] target)
    {
        double[] error = Vector.Sub(outputLayout, target);
        double[] delta = Vector.Mult(error, DAct(outputLayout));

        return delta;
    }

    public static double[][] Width(double[] inputLayout, double[][] width, double[] Error) // Ошибка или дельта
    {
        for(int i = 0; i < inputLayout.Length; i++)
            for(int j = 0; j < Error.Length; j++)
                width[i][j] -= stud * Error[j] * inputLayout[i];

        return width;
    }

    public static double[] Bias(double[] bias, double[] Error) // Ошибка или дельта
    {
        for(int i = 0; i < bias.Length; i++)
            bias[i] -= stud * Error[i];
        return bias;
    }

    

    private static double Act(double x) => FucAct.Sigmoid(x);
    private static double[] Act(double[] vector) => FucAct.Sigmoid(vector);
    private static double[] DAct(double[] vector) => FucAct.DSigmoid(vector);
}