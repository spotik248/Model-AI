//using System;

using static Model.Write;
using static Model.Learn;

namespace Model;

public class LP : INeuroComponent // Layout of Perceptron
{

    public string name { get; set; } = "Layout of Perceptron";
    public string shortName { get; set; } = "lp";
    public string desc { get; set; } = "Standart Layout of Perceptron";
    public ushort count { get; set; } = 1; // ushort = 2 байта, от 0 до 65535
    public int inputSize;
    public int outputSize;
    public int Size() => (inputSize + outputSize) * count / 2;
    
#pragma warning disable CS8618

    public double[] hiddenLayout;
    public double[][] width;
    public double[] bias;

    //public static double stud = 1;

    public LP(int inputSize, int outputSize)
    {
        this.inputSize = inputSize;
        this.outputSize = outputSize;

        hiddenLayout ??= Init.Zeros(outputSize);
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
        hiddenLayout = Lot.Pass(inputLayout, width, bias);
        return hiddenLayout;
    }

    // Возвращает локальную ошибку от следующего слоя (возможно выходного слоя)
    public double[] Error(double[][] nextWidth, double[] nextHiddenError)
    {
        return Lot.Error(hiddenLayout, width, nextWidth, nextHiddenError);
    }

    public double[] ErrorOut(double[] outputLayout, double[] target)
    {
        return Lot.ErrorOut(outputLayout, target);
    }

    // Возвращает дельту от выходного слоя
    public double[] Delta(double[] outputLayout, double[] target)
    {
        return Lot.Delta(outputLayout, target);
    }

    public double[][] Width(double[] inputLayout, double[] Error) // Ошибка или дельта
    {
        width = Lot.Width(inputLayout, width, Error);
        return width;
    }

    public double[][] Width2(double[] inputLayout, double[] Error) // Ошибка или дельта
    {
        width = Lot.Width(inputLayout, width, Error);
        
        for(int i = 0; i < width.Length; i++)
            for(int j = 0; j < width[0].Length; j++)
                width[i][j] = width[i][j] > 0.97 ? 1
                            : width[i][j] < 0.03 ? 0
                            : width[i][j];

        return width;
    }

    public double[] Bias(double[] Error) // Ошибка или дельта
    {
        bias = Lot.Bias(bias, Error);
        return bias;
    }

    public int[] GetLengthes()
    {
        return [
            inputSize, outputSize, hiddenLayout.Length, width.Length, width[0].Length, bias.Length
        ];
    }

}