//using System;

using static Model.Write;

namespace Model;

class LSTM : INeuroComponent
{
    //Из конструктора

    public string name { get; set; } = "Long Short-Term Memory"; // Долговременная память
    public string shortName { get; set; } = "lstm";
    public ushort count { get; set; } = 1; // ushort = 2 байта, от 0 до 65535
    public int sizeNN;
    public int Size() => sizeNN * count;

    public LSTM()
    {

    }

    public LSTM(int a)
    {
        
    }
}

/*public class GRU // Gated Recurrent Units // Управляемые рекуррентные блоки 
{
    //Из конструктора
    public int sizeUpdate; public int sizeRestart; public int sizeHidden; public double learningRate; public int dimension; public bool ;
    //Самосоздающееся
    public double[][] widthUpdate; public double[][] widthRestart; public double[][] widthHidden; public double[] biasUpdate; public double[] biasRestart; public double[] biasHidden;
    //Классы
    public double[] memoryCell;
    public GRU()
    {
        learningRate = 0.5;
        dimension = 10;
         = true;
        sizeUpdate = 70;
        sizeRestart = 70;
        sizeHidden = 70;
        memoryCell = Math.Zeros(sizeHidden);

        widthUpdate = Math.MatrixRandom(sizeUpdate, sizeUpdate);
        widthRestart = Math.MatrixRandom(sizeRestart, sizeRestart);
        widthHidden = Math.MatrixRandom(sizeHidden, sizeHidden);

        biasUpdate = Math.Zeros(sizeUpdate);
        biasRestart = Math.Zeros(sizeRestart);
        biasHidden = Math.Zeros(sizeHidden);

        Write.Line("Стандартная инициализация GRU закончена");

        if (widthUpdate == null || widthRestart == null || widthHidden == null ||
        biasUpdate == null || biasRestart == null || biasHidden == null)
        {
            Exc("Некоторые из весов или биасов GRU остались незаполненными!");
        }
    }

    public GRU(int SizeUpdate, int SizeRestart, int SizeHidden, double[] MemoryCell, double LearningRate, int Dimension, bool )
    {
        learningRate = LearningRate;
        dimension = Dimension;
         = ;
        sizeUpdate = SizeUpdate;
        sizeRestart = SizeRestart;
        sizeHidden = SizeHidden;
        memoryCell = MemoryCell;

        widthUpdate = Math.MatrixRandom(sizeUpdate, sizeUpdate);
        widthRestart = Math.MatrixRandom(sizeRestart, sizeRestart);
        widthHidden = Math.MatrixRandom(sizeHidden, sizeHidden);

        biasUpdate = Math.Zeros(sizeUpdate);
        biasRestart = Math.Zeros(sizeRestart);
        biasHidden = Math.Zeros(sizeHidden);

        Write.Line("Инициализация GRU закончена");

        if (widthUpdate == null || widthRestart == null || widthHidden == null ||
        biasUpdate == null || biasRestart == null || biasHidden == null)
        {
            Exc("Некоторые из весов или биасов GRU остались незаполненными!");
        }
    }

    public void Pass(double[] input)
    {
        double[] combinateInput = Math.ParaVectors(input, memoryCell);
        MsgErr($"Комбинированный выходной слой со скрытым слоем(память): {combinateInput}");

        double[] updateLayout = Layout(combinateInput, widthUpdate, biasUpdate, sizeUpdate, Math.Zeros(sizeUpdate));
        MsgErr($"Обновляющий слой: {updateLayout}");

        double[] restartLayout = Layout(combinateInput, widthRestart, biasRestart, sizeRestart, Math.Zeros(sizeRestart));
        MsgErr($"Перезагружающий слой: {restartLayout}");

        // Кондидаты \/

        double[] newCondidate = Vector.Mult(restartLayout, memoryCell); // Condidate
        MsgErr($"Новый кондидат: {newCondidate}");
        double[] candidate = Math.ParaVectors(input, newCondidate);
        MsgErr($"Пара вектор кондидат: {candidate}");

        double[] candidateLayout = Candidate(candidate, widthHidden, biasHidden, sizeHidden, Math.Zeros(sizeHidden));
        MsgErr($"кондидат слой: {candidateLayout}");

        double[] finalState = Vector.Mult(Vector.Complement(restartLayout), memoryCell); // старая информация, оставшаяся после Update Gate
        memoryCell = Vector.Add(finalState, Vector.Mult(updateLayout, candidateLayout)); // добавляем новую информацию
        MsgErr($"Новый скрытый слой(память): {memoryCell}");
    }

    public void Update(double[] delta)
    {
        widthUpdate = Width(widthUpdate, memoryCell, delta);
        MsgErr($"Обновление весов обновляющего слоя: {widthUpdate}");

        widthRestart = Width(widthRestart, memoryCell, delta);
        MsgErr($"Обновление весов перезагружающего слоя: {widthRestart}");

        widthHidden = Width(widthHidden, memoryCell, delta);
        MsgErr($"Обновление весов скрытого слоя: {widthHidden}");

        biasUpdate = Bias(biasUpdate, delta);
        MsgErr($"Обновление биаса обновляющего слоя: {biasUpdate}");

        biasRestart = Bias(biasRestart, delta);
        MsgErr($"Обновление биаса перезагружающего слоя: {biasRestart}");

        biasHidden = Bias(biasHidden, delta);
        MsgErr($"Обновление биаса скрытого слоя: {biasHidden}");
    }

    private static double[] Act(double[] vector)
    {
        return FucAct.SigmoidVector(vector);
    }

    private static double DAct(double vector)
    {
        return FucAct.DLReLu(vector);
    }

    private static double[] Acth(double[] vector)
    {
        return FucAct.ThVector(vector);
    }

    /
        sigmoid, sigmoidVector, dSigmoid, dSigmoidVector,
        softSign, softSignVector, dSoftSign, dSoftSignVector,
        ReLu, ReLuVector, dReLu, dReLuVector,
        lReLu, lReLuVector, dLReLu, dLReLuVector,
        Remoid, RemoidVector,
        smartAct, smartActVector, dSmartAct, dSmartActVector
    /

    private double[] Layout(double[] beforeLayout, double[][] thisWidth, double[] bias, int size, double[] addingData)
    {
        double[] allLayout = Math.Zeros(size);
        for (int i = 0; i < size; i++)
        {
            double[] newLayout = Vector.MultOne(thisWidth[i], beforeLayout[i] + addingData[i]);
            allLayout = Vector.Add(allLayout, newLayout);
        }
        double[] thisLayout = Vector.Add(allLayout, bias);
        thisLayout = Act(thisLayout);
        return thisLayout;
    }

    private double[] Candidate(double[] beforeLayout, double[][] thisWidth, double[] bias, int size, double[] addingData)
    {
        double[] allLayout = Math.Zeros(size);
        for (int i = 0; i < size; i++)
        {
            double[] newLayout = Vector.MultOne(thisWidth[i], beforeLayout[i] + addingData[i]);
            allLayout = Vector.Add(allLayout, newLayout);
        }
        double[] thisLayout = Vector.Add(allLayout, bias);
        thisLayout = Acth(thisLayout);
        return thisLayout;
    }

    public double[][] Width(double[][] oldthisWidth, double[] beforeLayout, double[] thisDelta)
    {
        double[][] newthisWidth = Init.Double<double>([oldthisWidth.Length, oldthisWidth[0].Length]);
        for (int i = 0; i < thisDelta.Length; i++)
        { //70
            for (int j = 0; j < beforeLayout.Length; j++)
            { //70
                newthisWidth[j][i] = oldthisWidth[j][i] + learningRate * thisDelta[i] * beforeLayout[j];
            }
        }
        return newthisWidth;
    }

    public double[] Bias(double[] oldBias, double[] delta)
    {
        double[] newBias = new double[oldBias.Length];
        for (int i = 0; i < delta.Length; i++)
        { // 70
            newBias[i] = oldBias[i] + learningRate * delta[i];
        }
        return newBias;
    }

    private void MsgErrSmall(string string1, params double[][] errors) // dotnet build -c Release
    {
         return;
        Write.Line(string1 + "IS SMALL!");
        try
        {
            Write.Line($"Length: {errors.Length}x{errors[0].Length}");
        }
        catch
        {
            Write.Line($"Length: {errors.Length}");
        }
        try
        {
            foreach (var innerArray in errors)
            {
                Write.Line($"    {string.Join(" ", innerArray)}");
            }
        }
        catch
        {
            //Write.Line($"{string.Join(" ", errors)}");
        }
    }

    private void MsgErr(string string1, params double[][] errors) // dotnet build -c Release
    {
         return;
        Write.Line(string1);
        try
        {
            Write.Line($"Length: {errors.Length}x{errors[0].Length}");
        }
        catch
        {
            Write.Line($"Length: {errors.Length}");
        }
        try
        {
            foreach (var innerArray in errors)
            {
                Write.Line($"    {string.Join(" ", innerArray)}");
            }
        }
        catch
        {
            //Write.Line($"{string.Join(" ", errors)}");
        }
    }

    private void MsgLength(string string1, object[] errors)
    {
        Write.Line(string1);
        if ()
        {
            try
            {
                Write.Line($"{errors.Length}, {errors.GetLength(1)}");
            }
            catch
            {
                Write.Line($"{errors.Length}");
            }
        }
    }

    private void CheckWidth()
    {
        foreach (var width in new double[][][] { widthUpdate, widthRestart, widthHidden })
        {
            for (int i = 0; i < width.Length; i++)
            {
                for (int j = 0; j < width[0].Length; j++)
                {
                    Write.Line(width[i][j]);
                }
            }
        }
    }

    private void CheckBias()
    {
        foreach (var bias in new double[][] { biasUpdate, biasRestart, biasHidden,  })
        {
            for (int i = 0; i < bias.Length; i++)
            {
                Write.Line(bias[i]);
            }
        }
    }

    public Dictionary<string, object> ToKeyValue()
    {
        return new Dictionary<string, object>() {
            {"sizeUpdate", sizeUpdate},
            {"sizeRestart", sizeRestart},
            {"sizeHidden", sizeHidden},
            {"widthUpdate", widthUpdate},
            {"widthRestart", widthRestart},
            {"widthHidden", widthHidden},
            {"biasUpdate", biasUpdate},
            {"biasHidden", biasHidden},
            {"biasRestart", biasRestart},
            {"learningRate", learningRate},
            {"dimension", dimension},
            {"", }
        };
    }

    public void ToStandart(Dictionary<string, object> keyValue)
    {
        sizeUpdate = (int)keyValue["sizeUpdate"];
        sizeRestart = (int)keyValue["sizeRestart"];
        sizeHidden = (int)keyValue["sizeHidden"];
        widthUpdate = (double[][])keyValue["widthUpdate"];
        widthRestart = (double[][])keyValue["widthRestart"];
        widthHidden = (double[][])keyValue["widthHidden"];
        biasUpdate = (double[])keyValue["biasUpdate"];
        biasRestart = (double[])keyValue["biasRestart"];
        biasHidden = (double[])keyValue["biasHidden"];
        learningRate = (double)keyValue["learningRate"];
        dimension = (int)keyValue["dimension"];
         = (bool)keyValue[""];
    }
}
*/