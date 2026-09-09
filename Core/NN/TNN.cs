//using System;

using static Model.Write;
using static Model.Library;
using static Model.NManage;
using static Model.Learn;
using static Model.Global;

namespace Model;

class TNN : INeuro // Transformer NeuroNetwork
{
    public string name { get; set; } = "Transformer NeuroNetwork";
    public string shortName { get; set; } = "tnn";
    public string desc { get; set; } = "NeuroNetwork which supposed to be transformer";
    public ushort countLayout { get; set; } = 6; // ushort = 2 байта, от 0 до 65535
    public int sizeNN { get; set; }
    public int Size() => sizeInput + sizeMemory + sizeHidden + sizeCross + sizeSocial + sizeOutput + SizeSLP() + SizeAtt() + memory.Size();
    public int batches { get; set; } = 1;

    public int SizeSLP() => SLPInput.Size() + SLPMemory.Size() + SLPHidden.Size() + SLPCross.Size() + SLPSocial.Size() + SLPOutput.Size();
    public int SizeAtt() => SelfAttention.Size() + SaveAttention.Size() + CrossAttention.Size() + OutputAttention.Size();

    //Из конструктора
    public int sizeInput; public int sizeMemory;
    public int sizeHidden; public int sizeCross;
    public int sizeSocial; public int sizeOutput;

    //Внимание
    public double[][] selfAttention;
    public double[][] saveAttention;
    public double[][] crossAttention;
    public double[][] outputAttention;

    public double[][] dSelfAttention;
    public double[][] dSaveAttention;
    public double[][] dCrossAttention;
    public double[][] dOutputAttention;

    //Многослойный проход
    public SLP SLPInput;
    public SLP SLPMemory;
    public SLP SLPHidden;

    public SLP SLPCross;
    public SLP SLPSocial;
    public SLP SLPOutput;

    //Полносвязные слои
    public double[][] layoutInput;
    public double[][] layoutMemory;
    public double[][] layoutHidden;
    public double[][] layoutCross;
    public double[][] layoutSocial;
    public double[][] layoutOutput;

    //Другие
    public double[][] layoutConnect;
    public double[][] hidden;
    public int maxStep;

    //Дельты
    public double[][] dInput;
    public double[][] dMemory;
    public double[][] dHidden;
    public double[][] dCross;
    public double[][] dSocial;
    public double[][] dOutput;

    //Самосоздающееся веса
    public double[][][] widthInput;
    public double[][][] widthMemory;
    public double[][][] widthHidden;

    public double[][][] widthCross;
    public double[][][] widthSocial;
    public double[][][] widthOutput;

    //Самосоздающееся биасы
    public double[][] biasInput;
    public double[][] biasMemory;
    public double[][] biasHidden;

    public double[][] biasCross;
    public double[][] biasSocial;
    public double[][] biasOutput;

    //Классы
    public Attention SelfAttention;
    public Attention SaveAttention;
    public Attention CrossAttention;
    public Attention OutputAttention;

    public GRU memory;


#pragma warning disable CS8618

    public TNN(int size)
    {
        sizeNN = size;
        Msg("Инициирован размер size: "+ size);

        sizeInput = size;
        sizeMemory = size;
        sizeHidden = size;
        sizeCross = size;
        sizeSocial = size;
        sizeOutput = size;
        
        int reg = 80;

        widthInput ??= Init.SpinecReg(reg, [size, dimension], [dimension, size]); // 2 x size x dim, trans -> 2 x dim x size
        widthMemory ??= Init.SpinecReg(reg, [size, dimension], [dimension, size]);
        widthHidden ??= Init.SpinecReg(reg, [size, dimension], [dimension, size]);
        widthCross ??= Init.SpinecReg(reg, [size, dimension], [dimension, size]);
        widthSocial ??= Init.SpinecReg(reg, [size, dimension], [dimension, size]);
        widthOutput ??= Init.SpinecReg(reg, [size, dimension], [dimension, size]);

        biasInput ??= Init.Zeros(dimension, dimension);
        biasMemory ??= Init.Zeros(dimension, dimension);
        biasHidden ??= Init.Zeros(dimension, dimension);
        biasCross ??= Init.Zeros(dimension, dimension);
        biasSocial ??= Init.Zeros(dimension, dimension);
        biasOutput ??= Init.Zeros(dimension, dimension);

        layoutInput ??= Init.Zeros(size, dimension);
        layoutMemory ??= Init.Zeros(size, dimension);
        layoutHidden ??= Init.Zeros(size, dimension);
        layoutCross ??= Init.Zeros(size, dimension);
        layoutSocial ??= Init.Zeros(size, dimension);
        layoutOutput ??= Init.Zeros(size, dimension);

        SLPInput ??= new SLP(dimension);
        SLPMemory ??= new SLP(dimension);
        SLPHidden ??= new SLP(dimension);
        SLPCross ??= new SLP(dimension);
        SLPSocial ??= new SLP(dimension);
        SLPOutput ??= new SLP(dimension);

        SelfAttention ??= new Attention(size);
        SaveAttention ??= new Attention(size);
        CrossAttention ??= new Attention(size);
        OutputAttention ??= new Attention(size);

        layoutConnect ??= Init.Zeros(dimension, dimension);
        hidden ??= Init.Zeros(dimension, dimension);

        memory ??= new GRU(size, Init.Zeros(size));

        Msg($"Инициализация {name} закончена...");

        if (widthInput == null || widthMemory == null || widthHidden == null || widthOutput == null ||
        biasInput == null || biasMemory == null || biasHidden == null || biasOutput == null)
            Exc($"Некоторые из весов или биасов {name} остались незаполненными!");
    }

#pragma warning restore CS8618

    public double[][] Predict(double[][] input)
    {
        maxStep = dimension;
        double[][] crossAnswer = Encoder(input);
        double[][] finallyAnswer = Decoder(crossAnswer);
        return finallyAnswer;
    }

    public double[] Study(double[][] input, double[][] output)
    {
        maxStep = output.Length;
        
        double[][] crossAnswer = Encoder(input); // size x dim -> dim x dim
        double[][] finallyAnswer = Decoder(crossAnswer); // dim x dim -> dim x dim
        
        FullUpdate(output, finallyAnswer, crossAnswer, input); // dim x dim, answ x dim, dim x dim, size x dim ->
        
        double error = Matrix.AddAll(Matrix.Sub(output, finallyAnswer));
        return [error];
    }

    public double[][] Encoder(double[][] quest) // dotnet build -c Release
    {
        Mas("Question: ", quest);
        quest = Matrix.ToSize(quest, sizeNN, dimension);

        // self attention
        selfAttention = SelfAttention.Pass(quest); // size x dim -> dim x dim

        // 1 layout. attention -> input
        layoutInput = SLPInput.Pass(selfAttention, widthInput, biasInput); // dim x dim -> dim x dim
        Mas("quest -> input layout:", layoutInput);

        // 2 layout. input -> memory
        layoutMemory = SLPMemory.Pass(layoutInput, widthMemory, biasMemory); // memory.memoryCell
        Mas("input -> memory layout:", layoutMemory);

        // 3 layout. memory -> hidden
        layoutHidden = SLPHidden.Pass(layoutMemory, widthHidden, biasHidden); // dim x dim -> dim x dim
        Mas("memory -> hidden layout:", layoutHidden);

        // 4 layout. hidden -> residual connection
        layoutConnect = Matrix.Add(layoutHidden, selfAttention); // dim x dim + dim x dim -> dim x dim
        Mas("hidden -> connect layout:", layoutConnect);

        // save attention
        saveAttention = SaveAttention.Pass(layoutConnect);

        //Сюда может быть добавлен DropOut и еще че то

        return saveAttention;
    }

    public double[][] Decoder(double[][] cross) // dotnet build -c Release
    {
        Clear();
        hidden[0] = Init.ExtraRandomized(dimension);
        double max = 0.70;
        for (int i = 1; i < maxStep && max > 0.50; i++)
        {
            Line($"I={i}, MAX={max}");
            // cross attention
            crossAttention = CrossAttention.PassCross(cross, hidden); // dim x dim -> dim x dim

            // 1 layout. attention -> cross
            layoutCross = SLPCross.Pass(crossAttention, widthCross, biasCross); // dim x dim -> dim x dim
            Mas("attention -> cross layout:", layoutHidden);

            // 2 layout. cross -> social
            layoutSocial = SLPSocial.Pass(layoutCross, widthSocial, biasSocial); // dim x dim -> dim x dim
            Mas("cross -> social layout:", layoutSocial);

            // 3 layout. social -> output
            layoutOutput = SLPOutput.Pass(layoutSocial, widthOutput, biasOutput); // dim x dim -> dim x dim
            Mas("social -> output layout:", layoutOutput);

            // output attention
            outputAttention = OutputAttention.PassMask(cross); // dim x dim -> dim x dim

            //Проверка на NaN
            if (double.IsNaN(Matrix.AddAll(outputAttention)))
                CheckIt("Ответ стал NaN", Matrix.AddAll(outputAttention));

            // attention -> residual connection
            double[][] output = Matrix.Add(outputAttention, layoutOutput); // dim x dim + dim x dim -> dim x dim

            double[] meanToken = new double[words.Count()];
            for (int j = 0; j < words.Count(); j++)
                meanToken[j] = Math.MeanVector(words[j].vector, output[^1]);
            max = Math.Max(meanToken);
            for (int j = 0; j < words.Count(); j++)
                if (meanToken[j] == max) hidden[i] = words[j].vector;
        }

        return hidden; // size x dim
    }

    public void Clear() => hidden = Init.Zeros(dimension, dimension);

    public void FullUpdate(double[][] rigthAnswer, double[][] finallyAnswer, double[][] crossAnswer, double[][] quest)
    {
        quest = Matrix.ToSize(quest, sizeNN, dimension);
        double[][] delta = Matrix.Sub(Matrix.ToSize(rigthAnswer, finallyAnswer.Length, dimension), finallyAnswer);

        //Decoder
        dOutputAttention = OutputAttention.Update(delta, finallyAnswer);
        dOutput = SLPOutput.Delta(dOutputAttention, layoutOutput, widthOutput, biasOutput);
        dSocial = SLPSocial.Delta(dOutput, layoutSocial, widthSocial, biasSocial);
        dCross = SLPCross.Delta(dSocial, layoutCross, widthCross, biasCross);
        dCrossAttention = CrossAttention.Update(dCross, crossAnswer);

        //Encoder
        dSaveAttention = SaveAttention.Update(dCrossAttention, saveAttention);
        dHidden = SLPHidden.Delta(dCrossAttention, layoutHidden, widthHidden, biasHidden);
        dMemory = SLPMemory.Delta(dHidden, layoutMemory, widthMemory, biasMemory);
        dInput = SLPInput.Delta(dMemory, layoutInput, widthInput, biasInput);
        dSelfAttention = SelfAttention.Update(dInput, quest);

        //Widthes
        widthOutput = SLPOutput.Width(widthOutput, layoutSocial);
        widthSocial = SLPSocial.Width(widthSocial, layoutCross);
        widthCross = SLPCross.Width(widthCross, crossAttention);

        widthHidden = SLPHidden.Width(widthHidden, layoutMemory);
        widthMemory = SLPMemory.Width(widthMemory, layoutInput);
        widthInput = SLPInput.Width(widthInput, selfAttention);

        //Biases
        biasOutput = SLPOutput.Bias(biasOutput);
        biasSocial = SLPSocial.Bias(biasSocial);
        biasCross = SLPCross.Bias(biasCross);

        biasHidden = SLPHidden.Bias(biasHidden);
        biasMemory = SLPMemory.Bias(biasMemory);
        biasInput = SLPInput.Bias(biasInput);
    }

    private static double[] Act(double[] vector) => FucAct.Sigmoid(vector);
    private static double DAct(double x) => FucAct.DSigmoid(x);

    // public double[][] Normalize(double[][] input)
    // {
    //     double mean = Matrix.Average(input);
    //     double stdDev = Math.Sqrt(Matrix.Variance(input));
    //     return input.Select(row => row.Select(cell => (cell - mean) / stdDev)).ToArray();
    // }

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
        foreach (var width in Init.ExtraRandomized[] { widthInput, widthMemory, widthHidden, widthOutput })
        {
            for (int i = 0; i < width.Length; i++)
            {
                Line(string.Join(" ", width[i]));
            }
        }
    }

    public void CheckBias()
    {
        foreach (var bias in Init.ExtraRandomized { biasInput, biasMemory, biasHidden, biasOutput })
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