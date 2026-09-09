//using System;

using static Model.Write;
using static Model.Library;
using static Model.NManage;
using static Model.Learn;
using static Model.Global;

namespace Model;

class VNN : INeuro // Vector NeuroNetwork
{
    //Из конструктора
    public string name { get; set; } = "Vector NeuroNetwork";
    public string shortName { get; set; } = "vnn";
    public string desc { get; set; } = "NeuroNetwork which is supposed to working as Perceptron on Vector";
    public ushort countLayout { get; set; } = 4; // ushort = 2 байта, от 0 до 65535
    public int sizeNN { get; set; }
    public int sizeInput;
    public int sizeMemory;
    public int sizeHidden;
    public int sizeOutput;
    public int Size() => sizeInput + sizeMemory + sizeHidden + sizeOutput + memory.Size();
    public int batches { get; set; } = 1;

    //Самосоздающееся веса
    public double[][] widthInput;
    public double[][] widthMemory;
    public double[][] widthHidden;
    public double[][] widthOutput;

    //Самосоздающееся биасы
    public double[] biasInput;
    public double[] biasMemory;
    public double[] biasHidden;
    public double[] biasOutput;

    //Слоя
    public double[] layoutInput;
    public double[] layoutMemory;
    public double[] layoutHidden;
    public double[] layoutOutput;

    //Дельты
    public double[] deltaInput;
    public double[] deltaMemory;
    public double[] deltaHidden;
    public double[] deltaOutput;

    //Классы
    public GRU memory;

#pragma warning disable CS8618

    public VNN(int size)
    {
        sizeNN = size;
        Msg("Инициирован размер size: "+ size);

        sizeInput  = size*dimension; // 100 * 10 = x1000
        sizeMemory = size*dimension;
        sizeHidden = size*dimension;
        sizeOutput = size*dimension;
        
        widthInput  ??= Init.Xavier(sizeInput, sizeInput); // 1000x1000
        widthMemory ??= Init.Xavier(sizeInput, sizeMemory);
        widthHidden ??= Init.Xavier(sizeMemory, sizeHidden);
        widthOutput ??= Init.Xavier(sizeHidden, sizeOutput);

        biasInput  ??= Init.Zeros(sizeInput); // x1000
        biasMemory ??= Init.Zeros(sizeMemory);
        biasHidden ??= Init.Zeros(sizeHidden);
        biasOutput ??= Init.Zeros(sizeOutput);

        deltaInput  ??= Init.Zeros(sizeInput); // x1000
        deltaMemory ??= Init.Zeros(sizeMemory);
        deltaHidden ??= Init.Zeros(sizeHidden);
        deltaOutput ??= Init.Zeros(sizeOutput);

        layoutInput  ??= Init.Zeros(sizeInput); // x1000
        layoutMemory ??= Init.Zeros(sizeMemory);
        layoutHidden ??= Init.Zeros(sizeHidden);
        layoutOutput ??= Init.Zeros(sizeOutput);

        memory ??= new GRU(sizeMemory, Init.Zeros(sizeMemory));

        Msg($"Инициализация {name} закончена...");

        if (widthInput == null || widthMemory == null || widthHidden == null || widthOutput == null ||
        biasInput == null || biasMemory == null || biasHidden == null || biasOutput == null)
            Exc($"Некоторые из весов или биасов {name} остались незаполненными!");
    }

#pragma warning restore CS8618

    public double[][] Predict(double[][] input) // dotnet build -c Release
    {
        Mas("Question: ", input); // 4x10

        double[] quest = Math.Flat(
            Matrix.ToSize(input, sizeNN, dimension) // 4x10 => 100x10
        );
        Mas("quest", quest); // 100x10 => 1000

        // 1 layout. quest -> input
        layoutInput = VLP.Pass(quest, widthInput, biasInput); // x1000, 1000x1000, x1000
        Mas("quest -> input layout:", layoutInput);

        // 2 layout. input -> memory
        layoutMemory = VLP.Pass(layoutInput, widthMemory, biasMemory); // и тд с теми же размерами
        Mas("input -> memory layout:", layoutMemory);

        // 3 layout. memory -> hidden
        layoutHidden = VLP.Pass(layoutMemory, widthHidden, biasHidden);
        Mas("memory -> hidden layout:", layoutHidden);

        // 4 layout. hidden -> output
        layoutOutput = VLP.Pass(layoutHidden, widthOutput, biasOutput);
        Mas("hidden -> output layout:", layoutOutput);

        // 5 layout. output -> return
        //memory.Pass(layoutOutput);
        return Vector.ToMatrix(layoutOutput, dimension); // 1000 => 100x10
    }
    
    public double[] Study(double[][] input, double[][] output) // dotnet build -c Release
    {
        Predict(input);

        Mas("input: ", input); // 4x20
        Mas("output: ", output); // 3x20


        double[] quest = Math.Flat(
            Matrix.ToSize(input, sizeNN, dimension) // 4x10 => 100x10
        );
        Mas("quest", quest); // 100x10 => 1000

        double[] answer = Math.Flat(
            Matrix.ToSize(output, sizeNN, dimension) // 3x10 => 100x10
        );
        Mas("answer", answer); // 100x10 => 1000


        //Deltes
        Mas("layoutOutput:", layoutOutput); Mas("answer:", answer);
        double[] error = Vector.Sub(layoutOutput, answer); // d = a - t // 1000, 1000

        deltaOutput = Vector.Mult(error, DAct(layoutOutput)); // d * f`(a) // то же
        Mas("Delta output layout:", deltaOutput);

        deltaHidden = VLP.Delta(deltaOutput, layoutHidden, widthHidden); // 1000, 1000, 1000x1000
        Mas("Delta hidden layout:", deltaHidden);

        deltaMemory = VLP.Delta(deltaHidden, layoutMemory, widthMemory);
        Mas("Delta memory layout:", deltaMemory);

        deltaInput = VLP.Delta(deltaMemory, layoutInput, widthInput);
        Mas("Delta input layout:", deltaInput);

        //Widthes
        widthOutput = VLP.Width(widthOutput, layoutHidden, deltaOutput, learningRate);
        Mas("Update width output layout:", widthOutput);

        widthHidden = VLP.Width(widthHidden, layoutMemory, deltaHidden, learningRate*size*dimension);
        Mas("Update width hidden layout:", widthHidden);

        widthMemory = VLP.Width(widthMemory, layoutInput, deltaMemory, learningRate*size*dimension * 3);
        Mas("Update width understanding layout:", widthMemory);

        widthInput = VLP.Width(widthInput, quest, deltaInput, learningRate*size*dimension * 5);
        Mas("Update width input layout:", widthInput);

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

        //memory.Update(deltaMemory);

        return [Vector.AddAll(error), Vector.AddAll(deltaOutput), Vector.AddAll(deltaHidden), Vector.AddAll(deltaMemory), Vector.AddAll(deltaInput)];
    }

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
            {"checkErrors", checkErrors}
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
        checkErrors = (bool)keyValue["checkErrors"];
    }*/
}