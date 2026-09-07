//using System;

using static Model.Write;
using static Model.Library;
using static Model.NN;

namespace Model;

public class LLEP // Layout Local Error Perceptron
{

    public string name { get; set; } = "Layout Local Error Perceptron";
    public string shortName { get; set; } = "llep";
    public ushort count { get; set; } = 2; // ushort = 2 байта, от 0 до 65535
    public int inputSize;
    public int outputSize;
    public int Size() => (inputSize + outputSize) * count / 2;
    
#pragma warning disable CS8618

    public double[] hiddenLayout;
    public double[][] width;
    public double[] bias;

    //public static double stud = 1;

    public LLEP(int inputSize, int outputSize)
    {
        this.inputSize = inputSize;
        this.outputSize = outputSize;

        hiddenLayout ??= Init.Zeros(inputSize);
        width ??= Init.Xavier(inputSize, outputSize);
        bias ??= Init.Zeros(outputSize);
        

        Msg($"Инициализация {name} закончена...");

        if (hiddenLayout == null || width == null || bias == null)
            Exc($"Слой, вес или биас {name} остались незаполненными!");
    }

#pragma warning restore CS8618

    // Самый обычный слой в перцептроне
    public double[] Pass(double[] inputLayout)
    {
        hiddenLayout = LLE.Pass(inputLayout, width, bias);
        return hiddenLayout;
    }

    // Возвращает локальную ошибку от следующего слоя (возможно выходного слоя)
    public double[] Error(double[][] nextWidth, double[] nextHiddenError)
    {
        return LLE.Error(hiddenLayout, width, nextWidth, nextHiddenError);
    }

    // Возвращает дельту от выходного слоя
    public double[] Delta(double[] outputLayout, double[] target)
    {
        return LLE.Delta(outputLayout, target);
    }

    public double[][] Width(double[] inputLayout, double[] Error) // Ошибка или дельта
    {
        width = LLE.Width(inputLayout, width, Error);
        return width;
    }

    public double[][] Width2(double[] inputLayout, double[] Error) // Ошибка или дельта
    {
        width = LLE.Width(inputLayout, width, Error);
        
        for(int i = 0; i < width.Length; i++)
            for(int j = 0; j < width[0].Length; j++)
                width[i][j] = width[i][j] > 0.97 ? 1
                            : width[i][j] < 0.03 ? 0
                            : width[i][j];

        return width;
    }

    public double[] Bias(double[] Error) // Ошибка или дельта
    {
        bias = LLE.Bias(bias, Error);
        return bias;
    }

}