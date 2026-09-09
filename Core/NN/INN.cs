//using System;

using static Model.Write;
using static Model.Library;
using static Model.NManage;
using static Model.Learn;

namespace Model;

class INN// : INeuro // Recurrent NeuroNetwork
{
    //Из конструктора
    public string name { get; set; } = "Image NeuroNetwork";
    public string shortName { get; set; } = "inn";
    public string desc { get; set; } = "NeuroNetwork which supposed to recognize an image as words";
    public ushort countLayout { get; set; } = 2; // ushort = 2 байта, от 0 до 65535
    public int sizeNN { get; set; }
    public int sizeInput;
    public int sizeHidden;
    public int sizeOutput;
    public int Size() => sizeInput + sizeHidden + sizeOutput;
    public int batches { get; set; } = 1;

    //CC веса
    public double[][] widthHidden;
    public double[][] widthOutput;

    //CC биасы
    public double[] biasHidden;
    public double[] biasOutput;

    //Слоя
    public double[] layoutHidden;
    public double[] layoutOutput;

    //Дельты
    public double[] deltaInput;
    public double[] deltaHidden;
    public double[] deltaOutput;

    //Классы
    //public GRU memory;

    private double learningRate => learningRate;
    private Random random;

#pragma warning disable CS8618
    public INN(int width = 100, int height = 100, int size = 100)
    {
        sizeNN = size;
        Msg("Инициирован размер size: "+ size);

        sizeInput = width * height * 4; // Длинна на высоту изображение на 4 канала цвета RGBA
        sizeHidden = size;
        sizeOutput = size;

        widthHidden ??= Init.ExtraRandomized(sizeInput, sizeHidden);
        widthOutput ??= Init.ExtraRandomized(sizeHidden, sizeOutput);

        biasHidden ??= Init.Zeros(sizeHidden);
        biasOutput ??= Init.Zeros(sizeOutput);

        deltaInput ??= Init.Zeros(sizeInput);
        deltaHidden ??= Init.Zeros(sizeHidden);
        deltaOutput ??= Init.Zeros(sizeOutput);

        layoutHidden ??= Init.Zeros(sizeHidden);
        layoutOutput ??= Init.Zeros(sizeOutput);

        //memory ??= new GRU(sizeMemory, Init.Zeros(sizeMemory));

        Msg($"Инициализация {name} закончена...");

        if (widthHidden == null || widthOutput == null || biasHidden == null || biasOutput == null)
            Exc($"Некоторые из весов или биасов {name} остались незаполненными!");
    }
#pragma warning restore CS8618

    // Конвертация ступенчатых матриц RGBA в один плоский вектор
    private double[] FlattenImages(byte[][] R, byte[][] G, byte[][] B, byte[][] A, int width, int height)
    {
        double[] input = new double[sizeInput];
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0, i = 0; x < width; x++)
            {
                input[i++] = R[x][y] / 255.0;
                input[i++] = G[x][y] / 255.0;
                input[i++] = B[x][y] / 255.0;
                input[i++] = A[x][y] / 255.0;
            }
        }
        return input;
    }
    
    /*public double[][] Predict(byte[][] R, byte[][] G, byte[][] B, byte[][] A, int width, int height)
    {
        Mas("R: ", R); Mas("G: ", G); Mas("B: ", B); Mas("A: ", A);
        Mas("width: ", width); Mas("height: ", height);

        double[] input = FlattenImages(R, G, B, A, width, height);

        return Predict(input, width, height);
    }

    public double[][] Predict(double[] input, int width, int height) // dotnet build -c Release
    {
        // 1 layout. input -> hidden
        for (int j = 0; j < sizeHidden; j++)
        {
            double sum = biasHidden[j];
            for (int i = 0; i < sizeInput; i++)
                sum += input[i] * widthHidden[i][j];
            layoutHidden[j] = Act(sum);
        }
        Mas("input -> hidden layout:", layoutHidden);

        // 2 layout. hidden -> output
        for (int k = 0; k < sizeOutput; k++)
        {
            double sum = biasOutput[k];
            for (int j = 0; j < sizeHidden; j++)
                sum += layoutHidden[j] * widthOutput[j][k];
            layoutOutput[k] = Act(sum);
        }
        Mas("hidden -> output layout:", layoutOutput);

        // 5 layout. output -> return
        //memory.Pass(layoutOutput);

        return Vector.ToMatrix(layoutOutput, Global.dimension);
    }
    
    public double[] Study(double[][] input, double[][] output) // dotnet build -c Release
    {
        Predict(quest);

        double[] questFlat = Math.Flat(quest);
        double[] questComb = Vector.ToSize(questFlat, sizeInput);
        double[] answerFlat = Math.Flat(answer);
        double[] answerComb = Vector.ToSize(answerFlat, sizeOutput);
        Mas("new answer:", answerComb);

        //Deltes
        Mas("BEFORE1 Delta output layout:", answerComb); Mas("BEFORE2 Delta output layout:", layoutOutput);
        deltaOutput = Vector.Mult(Vector.Sub(answerComb, layoutOutput), DAct(layoutOutput));
        Mas("Delta output layout:", deltaOutput);

        deltaHidden = VLP.Delta(deltaOutput, layoutHidden, widthHidden);
        Mas("Delta hidden layout:", deltaHidden);

        deltaMemory = VLP.Delta(deltaHidden, layoutMemory, widthMemory);
        Mas("Delta understanding layout:", deltaMemory);

        deltaInput = VLP.Delta(deltaMemory, layoutInput, widthInput);
        Mas("Delta input layout:", deltaInput);

        //Widthes
        widthOutput = VLP.Width(widthOutput, layoutHidden, deltaOutput);
        Mas("Update width output layout:", widthOutput);

        widthHidden = VLP.Width(widthHidden, layoutMemory, deltaHidden);
        Mas("Update width hidden layout:", widthHidden);

        widthMemory = VLP.Width(widthMemory, layoutInput, deltaMemory);
        Mas("Update width understanding layout:", widthMemory);

        widthInput = VLP.Width(widthInput, questComb, deltaInput);
        Mas("update width input layout:", widthInput);

        //Biases
        biasOutput = VLP.Bias(biasOutput, deltaOutput);
        Mas("Update bias output layout:", biasOutput);

        biasHidden = VLP.Bias(biasHidden, deltaHidden);
        Mas("Update bias hidden layout:", biasHidden);

        biasMemory = VLP.Bias(biasMemory, deltaMemory);
        Mas("Update bias understanding layout:", biasMemory);

        biasInput = VLP.Bias(biasInput, deltaInput);
        Mas("Update bias input layout:", biasInput);
        Line("\n");

        //Errors AI, dont work for backpass
        double error = Vector.AddAll(Vector.Sub(answerComb, layoutOutput));
        double MSE = Vector.AddAll(Math.MSE(answerComb, layoutOutput));
        Mas("error neuroNetwork:", error); Mas("MSE neuroNetwork:", MSE);

        //memory.Update(deltaMemory);

        return (error, MSE);

        //////
        
        double[] input = FlattenImages(R, G, B, A, width, height);
        Forward(R, G, B, A, width, height);

        // Дельта выходного слоя
        double[] deltaOutput = new double[outputSize];
        for (int k = 0; k < outputSize; k++)
        {
            double error = outputLayers[k] - target[k];
            deltaOutput[k] = error * SigmoidDerivative(outputLayers[k]);
        }

        // Дельта скрытого слоя
        double[] deltaHidden = new double[hiddenSize];
        for (int j = 0; j < hiddenSize; j++)
        {
            double error = 0;
            for (int k = 0; k < outputSize; k++)
                error += deltaOutput[k] * weightsHiddenOutput[j][k];
            
            deltaHidden[j] = error * SigmoidDerivative(hiddenLayers[j]);
        }

        // Обновление весов скрытый -> выход
        for (int j = 0; j < hiddenSize; j++)
        {
            for (int k = 0; k < outputSize; k++)
            {
                weightsHiddenOutput[j][k] -= learningRate * deltaOutput[k] * hiddenLayers[j];
            }
        }
        for (int k = 0; k < outputSize; k++)
            biasOutput[k] -= learningRate * deltaOutput[k];

        // Обновление весов вход -> скрытый
        for (int i = 0; i < inputSize; i++)
        {
            for (int j = 0; j < hiddenSize; j++)
            {
                weightsInputHidden[i][j] -= learningRate * deltaHidden[j] * input[i];
            }
        }
        for (int j = 0; j < hiddenSize; j++)
            biasHidden[j] -= learningRate * deltaHidden[j];
    }
    
    */

    private static double Act(double x) => FucAct.Sigmoid(x);
    private static double[] Act(double[] x) => FucAct.Sigmoid(x);
    private static double DAct(double x) => FucAct.DSigmoid(x);
    private static double[] DAct(double[] x) => FucAct.DSigmoid(x);

    // dotnet build -c Release

    /*
        Sigmoid,  DSigmoid,
        SoftSign, DSoftSign
        ReLu, DReLu,
        LReLu, DLReLu,
    */
    
    /*
    public void CheckWidthSmall()
    {
        double[][][] widths = [widthInput, widthMemory, widthHidden, widthOutput];
        for (int i = 0; i < widths.Length; i++)
        {
            Line(string.Join(" ", widths[i][0]));
        }
    }

    public void CheckBiasSmall()
    {
        double[][] biases = [biasInput, biasMemory, biasHidden, biasOutput];
        for (int i = 0; i < biases.Length; i++)
        {
            Line(string.Join(" ", biases[i]));
        }
    }

    public void CheckWidth()
    {
        foreach (var width in new double[][][] { widthInput, widthMemory, widthHidden, widthOutput })
        {
            for (int i = 0; i < width.Length; i++)
            {
                Line(string.Join(" ", width[i]));
            }
        }
    }

    public void CheckBias()
    {
        foreach (var bias in new double[][] { biasInput, biasMemory, biasHidden, biasOutput })
        {
            Line(string.Join(" ", bias));
        }
    }
    */
    /*
    public Dictionary<string, object> ToKeyValue()
    {
    return new Dictionary<string, object>() {
            {"sizeInput", sizeInput},
            {"sizeHidden", sizeHidden},
            {"sizeOutput", sizeOutput},
            {"widthInput", widthInput},
            {"widthHidden", widthHidden},
            {"widthOutput", widthOutput},
            {"biasInput", biasInput},
            {"biasHidden", biasHidden},
            {"biasOutput", biasOutput},
            {"learningRate", learningRate},
            {"dimension", dimension},
            {"", }
        };
    }

    public void ToStandart(Dictionary<string, object> keyValue)
    {
        sizeInput = (int)keyValue["sizeInput"];
        sizeMemory = (int)keyValue["sizeMemory"];
        sizeHidden = (int)keyValue["sizeHidden"];
        sizeOutput = (int)keyValue["sizeOutput"];
        widthInput = (double[][])keyValue["widthInput"];
        widthMemory = (double[][])keyValue["widthMemory"];
        widthHidden = (double[][])keyValue["widthHidden"];
        widthOutput = (double[][])keyValue["widthOutput"];
        biasInput = (double[])keyValue["biasInput"];
        biasMemory = (double[])keyValue["biasMemory"];
        biasHidden = (double[])keyValue["biasHidden"];
        biasOutput = (double[])keyValue["biasOutput"];
        learningRate = (double)keyValue["learningRate"];
        dimension = (int)keyValue["dimension"];
         = (bool)keyValue[""];
    }*/
}