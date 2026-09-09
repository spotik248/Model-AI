//using System;

using static Model.Write;
using static Model.Library;
using static Model.NManage;
using static Model.Learn;
using static Model.Global;

namespace Model;

class FNN : INeuro // Function NeuroNetwork
{

    public string name { get; set; } = "Function NeuroNetwork";
    public string shortName { get; set; } = "fnn";
    public string desc { get; set; } = "NeuroNetwork which is supposed to working";
    public ushort countLayout { get; set; } = 6; // ushort = 2 байта, от 0 до 65535
    public int sizeNN { get; set; }
    public int Size() => sizeNN * countLayout + SizeSLP() + SizeAtt() + memory.Size();
    public int batches { get; set; } = 1;

    // Производные размеры
    public int SizeSLP() => SLPInput.Size() + SLPMemory.Size() + SLPHidden.Size() + SLPOutput.Size();
    public int SizeAtt() => SelfAttention.Size() + OutputAttention.Size();

    #region Init

#pragma warning disable CS0649

    //Многослойный проход
    public SLP SLPInput;
    public SLP SLPMemory;
    public SLP SLPHidden;
    public SLP SLPOutput;
    
    //Самосоздающееся веса
    public double[][] widthInput;
    public double[][] widthMemory;
    public double[][] widthHidden;
    public double[][] widthOutput;

    //Самосоздающееся биасы
    public double[][] biasInput;
    public double[][] biasMemory;
    public double[][] biasHidden;
    public double[][] biasOutput;

    //Слоя
    public double[][] layoutInput;
    public double[][] layoutMemory;
    public double[][] layoutHidden;
    public double[][] layoutOutput;

    //Дельты
    public double[][] deltaInput;
    public double[][] deltaMemory;
    public double[][] deltaHidden;
    public double[][] deltaOutput;

    
    //Классы
    public GRU memory;
    //Внимание
    public Attention SelfAttention; public Attention OutputAttention;
    public double[][] selfAttention; public double[][] outputAttention;
    public double[][] outputLayout;

    #endregion

#pragma warning disable CS8618

    public FNN(int size)
    {
        sizeNN = size;
        Msg("Инициирован размер size: "+ size);

        widthInput  ??= Init.Xavier(size, dimension); // size x dim
        widthMemory ??= Init.Xavier(size, dimension);
        widthHidden ??= Init.Xavier(size, dimension);
        widthOutput ??= Init.Xavier(size, dimension);

        biasInput  ??= Init.Zeros(size, dimension);
        biasMemory ??= Init.Zeros(size, dimension);
        biasHidden ??= Init.Zeros(size, dimension);
        biasOutput ??= Init.Zeros(size, dimension);

        layoutInput  ??= Init.Zeros(size, dimension);
        layoutMemory ??= Init.Zeros(size, dimension);
        layoutHidden ??= Init.Zeros(size, dimension);
        layoutOutput ??= Init.Zeros(size, dimension);

        deltaInput  ??= Init.Zeros(size, dimension);
        deltaMemory ??= Init.Zeros(size, dimension);
        deltaHidden ??= Init.Zeros(size, dimension);
        deltaOutput ??= Init.Zeros(size, dimension);

        SLPInput  ??= new SLP(size);
        SLPMemory ??= new SLP(size);
        SLPHidden ??= new SLP(size);
        SLPOutput ??= new SLP(size);

        SelfAttention ??= new Attention(size);
        OutputAttention ??= new Attention(size);

        memory ??= new GRU(size, Init.Zeros(size));

        //Msg($"Инициализация {name} закончена...");
        Msg($"Initialization {name} complete");

        if (widthInput == null || widthMemory == null || widthHidden == null || widthOutput == null ||
        biasInput == null || biasMemory == null || biasHidden == null || biasOutput == null)
            Exc($"Некоторые из весов или биасов {name} остались незаполненными!");
    }

#pragma warning restore CS8618
#pragma warning restore CS0649

    public double[][] Predict(double[][] input) // dotnet build -c Release
    {
        Mas("Input: ", input);

        double[][] quest = Matrix.ToSize(input, sizeNN, dimension);
        Mas("quest:", quest);

        // 0 attention. quest -> attention
        selfAttention = SelfAttention.Pass(quest);
        Mas("quest -> attention:", selfAttention);

        // 1 layout. attention -> quest
        layoutInput = MLP.Pass(selfAttention, widthInput, biasInput); // 100x20 => 100x20
        Mas("attention -> input layout:", layoutInput);

        // 2 layout. quest -> memory
        layoutMemory = MLP.Pass(layoutInput, widthMemory, biasMemory);
        Mas("input -> memory layout:", layoutMemory);

        // 3 layout. memory -> hidden
        layoutHidden = MLP.Pass(layoutMemory, widthHidden, biasHidden);
        Mas("memory -> hidden layout:", layoutHidden);

        // 4 layout. hidden -> output
        layoutOutput = MLP.Pass(layoutHidden, widthOutput, biasOutput);
        Mas("hidden -> output layout:", layoutOutput);

        // 5 layout. output -> attention
        outputLayout = OutputAttention.Pass(layoutOutput);
        Mas("output -> attention:", outputLayout);

        //memory.Pass(output);
        return outputLayout;
    }
    
    public double[] Study(double[][] input, double[][] output) // dotnet build -c Release
    {
        Predict(input);

        double[][] quest = Matrix.ToSize(input, sizeNN, dimension);
        double[][] answer = Matrix.ToSize(output, sizeNN, dimension);
        Mas("quest:", quest);
        Mas("answer:", answer);

        //Deltes
        Mas("Delta answer:", answer); Mas("Delta output:", outputLayout);
        
        double[][] delta = Matrix.Mult(Matrix.Sub(outputLayout, answer), DAct(outputLayout));

        Mas("Delta output layout:", delta);

        double[][] deltaOutAtt = OutputAttention.Update(delta, output);
        Mas("Delta Output Attention:", deltaOutAtt);

        deltaOutput = MLP.Delta(deltaOutAtt, layoutOutput, widthOutput);
        Mas("Delta hidden layout:", deltaHidden);

        deltaHidden = MLP.Delta(deltaOutput, layoutHidden, widthHidden);
        Mas("Delta hidden layout:", deltaHidden);

        deltaMemory = MLP.Delta(deltaHidden, layoutMemory, widthMemory);
        Mas("Delta understanding layout:", deltaMemory);

        deltaInput = MLP.Delta(deltaMemory, layoutInput, widthInput);
        Mas("Delta input layout:", deltaInput);

        SelfAttention.Update(deltaInput, quest);

        //Widthes
        widthOutput = MLP.Width(widthOutput, layoutHidden, deltaOutput);
        Mas("Update width output layout:", widthOutput);

        widthHidden = MLP.Width(widthHidden, layoutMemory, deltaHidden);
        Mas("Update width hidden layout:", widthHidden);

        widthMemory = MLP.Width(widthMemory, layoutInput, deltaMemory);
        Mas("Update width understanding layout:", widthMemory);

        widthInput = MLP.Width(widthInput, quest, deltaInput);
        Mas("update width input layout:", widthInput);

        //Biases
        biasOutput = MLP.Bias(biasOutput, deltaOutput);
        Mas("Update bias output layout:", biasOutput);

        biasHidden = MLP.Bias(biasHidden, deltaHidden);
        Mas("Update bias hidden layout:", biasHidden);

        biasMemory = MLP.Bias(biasMemory, deltaMemory);
        Mas("Update bias understanding layout:", biasMemory);

        biasInput = MLP.Bias(biasInput, deltaInput);
        Mas("Update bias input layout:", biasInput);
        Line("\n");

        //Errors and MSE
        double error = Matrix.AddAll(Matrix.Sub(answer, output));
        Mas("error neuroNetwork:", error);

        //memory.Update(deltaMemory);

        return [error, Matrix.AddAll(deltaOutAtt), Matrix.AddAll(deltaOutput), Matrix.AddAll(deltaMemory), Matrix.AddAll(deltaInput)];
    }

    private static double[] Act(double[] x) => FucAct.Sigmoid(x);
    private static double DAct(double x) => FucAct.DSigmoid(x);
    private static double[] DAct(double[] x) => FucAct.DSigmoid(x);
    private static double[][] DAct(double[][] x) => x.Select(FucAct.DSigmoid).ToArray();

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