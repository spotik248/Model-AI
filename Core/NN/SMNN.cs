//using System;

using static Model.Write;
using static Model.Library;
using static Model.NManage;
using static Model.Learn;
using static Model.Global;

namespace Model;

class SMNN : INeuro // Selp Making NeuroNetwork
{

    public string name { get; set; } = "Self Making NeuroNetwork";
    public string shortName { get; set; } = "smnn";
    public string desc { get; set; } = "NeuroNetwork";
    public bool onlyPocket { get; set; } = false;
    public ushort countLayout { get; set; } = 2; // ushort = 2 байта, от 0 до 65535
    public int sizeNN { get; set; }
    public int Size() => sizeNN * countLayout;
    public int batches { get; set; } = 1;



#pragma warning disable CS0649


    public SMP smp;


#pragma warning disable CS8618


    public SMNN(int size) // TODO: Добавить hiddenSize для внутреннего пространства нейронки
    {
        if(size != 0) sizeNN = size;
        Msg("Инициирован размер sizeNN: "+ sizeNN); // 50
        
        int sizedim = sizeNN * dimension; // 50*10 = 500
        int size2 = Math.Max(sizedim, (int)Math.Pow2(sizeNN)); // min500, 50*50 = 2500


        smp ??= new(sizedim, size2, sizedim);


        Msg($"Инициализация {name} закончена...");

        if (smp == null)
            Exc($"Некоторые из данных {name} остались незаполненными!");
    }


#pragma warning restore CS8618
#pragma warning restore CS0649


    public double[][] Predict(double[][] input) // dotnet build -c Release
    {
        if(input[0].Length != dimension)
            Exc("Измерения input и dimension не соответствуют.");


        double[] inputLayout = Math.Flat(
            Matrix.ToSizeHalf(input, sizeNN, dimension)
        );
        Mas("inputLayout", inputLayout);


        double[] outputLayout = smp.Pass(inputLayout);
        Mas("outputLayout", outputLayout);


        return Vector.ToMatrix(outputLayout, dimension);
    }

    public double[] Study(double[][] input, double[][] output)
    {
        if(output.Length * output[0].Length > input.Length * dimension)
            Exc("Общий размер input гораздо больше чем фиксированный общий размер size.");

        if(input.Length > smp.sizes[0])
            Exc("Размер столбца input гораздо больше чем фиксированный размер столбца size.");

        if(input[0].Length != dimension)
            Exc("Измерения input и dimension не соответствуют.");


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

        smp.Update(target);

        Msg("### end ###");

        return smp.errors.Select(Vector.AddAll).ToArray();
    }

    public double[][][] PredictPocket(double[][][] input) { return []; }
    public double[][] StudyPocket(double[][][] input, double[][][] output) { return []; }
    
}