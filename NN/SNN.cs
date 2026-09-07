//using System;

using static Model.Write;
using static Model.Library;
using static Model.NN;
using static Model.Global;

namespace Model;

class SNN : INeuro // Simulating NeuroNetwork
{

    public string name { get; set; } = "Simulating NeuroNetwork";
    public string shortName { get; set; } = "snn";
    public string desc { get; set; } = "NeuroNetwork";
    public ushort countLayout { get; set; } = 2; // ushort = 2 байта, от 0 до 65535
    public int sizeNN { get; set; }
    public int Size() => sizeNN * countLayout;
    public int batches { get; set; } = 1;



#pragma warning disable CS0649


    public LLEP hidden1;
    public LLEP hidden2;


#pragma warning disable CS8618


    public SNN(int size) // TODO: Добавить hiddenSize для внутреннего пространства нейронки
    {
        if(size != 0) sizeNN = size;
        Msg("Инициирован размер sizeNN: "+ sizeNN); // 50
        
        int sizedim = sizeNN * dimension; // 50*10 = 500
        int size2 = Math.Max(sizedim, (int)Math.Pow2(sizeNN)); // min500, 50*50 = 2500


        hidden1 ??= new(sizedim, size2);
        hidden2 ??= new(size2, sizedim);


        Msg($"Инициализация {name} закончена...");

        if (hidden1 == null || hidden2 == null)
            Exc($"Некоторые из данных {name} остались незаполненными!");
    }


#pragma warning restore CS8618
#pragma warning restore CS0649


    public double[][] Predict(double[][] input) // dotnet build -c Release
    {
        if(input.Length * input[0].Length > hidden1.inputSize * dimension)
            Exc("Общий размер input гораздо больше чем фиксированный общий размер size.");

        if(input.Length > hidden1.inputSize)
            Exc("Размер столбца input гораздо больше чем фиксированный размер столбца size.");

        if(input[0].Length != dimension)
            Exc("Измерения input и dimension не соответствуют.");


        double[] inputLayout = Math.Flat(
            Matrix.ToSizeHalf(input, sizeNN, dimension)
        );
        Mas("inputLayout", inputLayout);


        double[] hiddenLayout1 = hidden1.Pass(inputLayout);
        Mas("hiddenLayout", hiddenLayout1);

        double[] outputLayout = hidden2.Pass(hiddenLayout1);
        Mas("outputLayout", outputLayout);

        return Vector.ToMatrix(outputLayout, dimension);
    }

    public double[] Study(double[][] input, double[][] output)
    {
        // if(output.Length * output[0].Length > input.Length * dimension)
        //     Exc("Общий размер input гораздо больше чем фиксированный общий размер size.");

        // if(input.Length > hidden1.inputSize)
        //     Exc("Размер столбца input гораздо больше чем фиксированный размер столбца size.");

        // if(input[0].Length != dimension)
        //     Exc("Измерения input и dimension не соответствуют.");


        // TODO: Сделать проверку на исключения.


        double[] inputLayout = Math.Flat(
            Matrix.ToSizeHalf(input, sizeNN, dimension)
        );
        Mas("inputLayout", inputLayout);
        

        double[] outputLayout = Math.Flat(
            Matrix.ToSizeHalf(Predict(input), sizeNN, dimension)
        );
        Mas("outputLayout", outputLayout);


        double[] target = Math.Flat(
            Matrix.ToSizeHalf(output, sizeNN, dimension)
        );
        Mas("target", target);


        double[] error2 = new double[outputLayout.Length];
        for(int i = 0; i < outputLayout.Length; i++)
            error2[i] = (outputLayout[i] - target[i]) / batches; //a - t
        Mas("error", error2);

        //Full(error);

        //for(int i = 0; i < error.Length; i++)
        //    error[i] = target[i] == 0 ? 0 : error[i];

        //Full(error2);

        double[] delta = Vector.Mult(error2, FucAct.DSigmoid(outputLayout));
        Mas("delta", delta);
        Mas("outputLayout, DSigmoid", [outputLayout, FucAct.DSigmoid(outputLayout)]);

        double[] error1 = hidden1.Error(hidden2.width, delta);

        // Движение слов в минимум ошибки
        MoveVectorWords(NAnalis.idsQuest, Vector.ToMatrix(error1, dimension));

        hidden1.Width(inputLayout, error1);
        hidden1.Bias(error1);

        hidden2.Width(hidden1.hiddenLayout, delta);
        hidden2.Bias(delta);
        

        Msg("### end ###");

        return [Vector.AddAll(error2), Vector.AddAll(error1)];
    }

}