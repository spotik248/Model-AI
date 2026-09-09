//using System;

using static Model.Write;

namespace Model;

class GRU : INeuroComponent
{
    //Из конструктора

    public string name { get; set; } = "Gated Recurrent Units"; // Управляемые рекуррентные блоки 
    public string shortName { get; set; } = "gru";
    public ushort count { get; set; } = 3; // ushort = 2 байта, от 0 до 65535
    public int sizeGRU;
    public int Size() => sizeGRU * count;

    
    //Самосоздающееся
    public double[][] widthUpdate; public double[][] widthRestart; public double[][] widthHidden;
    public double[] biasUpdate; public double[] biasRestart; public double[] biasHidden;
    public double[] updateLayout; public double[] restartLayout;
    public double[] deltaUpdate; public double[] deltaRestart;
    //Классы
    public double[] memoryCell;

#pragma warning disable CS8618

    public GRU()
    {
        sizeGRU = 100;
        memoryCell = Init.Zeros(sizeGRU);
        InitGRU();
    }

    public GRU(int Size = 100)
    {
        sizeGRU = Size;
        memoryCell = Init.Zeros(sizeGRU);
        InitGRU();
    }

    public GRU(int Size, double[] MemoryCell)
    {
        sizeGRU = Size;
        memoryCell = MemoryCell ?? Init.Zeros(sizeGRU);
        InitGRU();
    }


#pragma warning restore CS8618

    public void InitGRU()
    {
        widthUpdate = Init.ExtraRandomized(sizeGRU, sizeGRU);
        widthRestart = Init.ExtraRandomized(sizeGRU, sizeGRU);
        widthHidden = Init.ExtraRandomized(sizeGRU, sizeGRU);

        biasUpdate = Init.Zeros(sizeGRU);
        biasRestart = Init.Zeros(sizeGRU);
        biasHidden = Init.Zeros(sizeGRU);

        deltaUpdate = Init.Zeros(sizeGRU);
        deltaRestart = Init.Zeros(sizeGRU);

        updateLayout = Init.Zeros(sizeGRU);
        restartLayout = Init.Zeros(sizeGRU);

        Msg("Инициализация GRU закончена...");

        if (widthUpdate == null || widthRestart == null || widthHidden == null ||
        biasUpdate == null || biasRestart == null || biasHidden == null)
            Exc("Некоторые из весов или биасов GRU остались незаполненными!");
    }

    public void Pass(double[] input)
    {
        // 1 layout. input -> combinate
        double[] combinateInput = Vector.Combinate(input, memoryCell);
        Mas("Комбинированный выходной слой со скрытым слоем(память):", combinateInput);

        // 2 layout. combinate -> update
        updateLayout = VLP.Pass(combinateInput, widthUpdate, biasUpdate);
        Mas("Обновляющий слой:", updateLayout);

        // 3 layout. combinate -> restart
        restartLayout = VLP.Pass(combinateInput, widthRestart, biasRestart);
        Mas("Перезагружающий слой:", restartLayout);

        // 4 layout. memoryCell, restart -> newcandidate
        double[] newCandidate = Vector.Mult(restartLayout, memoryCell); // candidate
        Mas("Новый кондидат:", newCandidate);

        // 5 layout. newcandidate, input -> candidate
        double[] candidate = Vector.Combinate(input, newCandidate);
        Mas("Пара вектор кондидат:", candidate);

        // 6 layout. memoryCell, restart -> candidateLayout
        double[] candidateLayout = VLP.Candidate(candidate, widthHidden, biasHidden);
        Mas("Кондидат слой:", candidateLayout);

        // 7 layout. candidateLayout, update, memoryCell -> finally
        double[] finalState = Vector.Mult(Vector.Complement(restartLayout), memoryCell); // старая информация, оставшаяся после Update Gate
        memoryCell = Vector.Add(finalState, Vector.Mult(updateLayout, candidateLayout)); // добавляем новую информацию
        Mas("Новый скрытый слой(память):", memoryCell);
    }

    public void Update(double[] delta)
    {
        //Widths
        widthUpdate = VLP.Width(widthUpdate, memoryCell, delta);
        Mas("Обновление весов обновляющего слоя:", widthUpdate);

        widthRestart = VLP.Width(widthRestart, memoryCell, delta);
        Mas("Обновление весов перезагружающего слоя:", widthRestart);

        widthHidden = VLP.Width(widthHidden, memoryCell, delta);
        Mas("Обновление весов скрытого слоя:", widthHidden);

        //Biases
        biasUpdate = VLP.Bias(biasUpdate, delta);
        Mas("Обновление биаса обновляющего слоя:", biasUpdate);

        biasRestart = VLP.Bias(biasRestart, delta);
        Mas("Обновление биаса перезагружающего слоя:", biasRestart);

        biasHidden = VLP.Bias(biasHidden, delta);
        Mas("Обновление биаса скрытого слоя:", biasHidden);
    }

    private static double[] Act(double[] vector) => FucAct.Sigmoid(vector);

    private static double DAct(double vector) => FucAct.DLReLu(vector);

    private static double[] Acth(double[] vector) => FucAct.Tanh(vector);

    /*
        Sigmoid,  DSigmoid,
        SoftSign, DSoftSign
        ReLu, DReLu,
        LReLu, DLReLu,
    */

    /*
    private void CheckWidth()
    {
        foreach (var width in new double[][][] { widthUpdate, widthRestart, widthHidden })
        {
            for (int i = 0; i < width.Length; i++)
            {
                for (int j = 0; j < width[0].Length; j++)
                {
                    Msg(width[i][j]);
                }
            }
        }
    }

    private void CheckBias()
    {
        foreach (var bias in new double[][] { biasUpdate, biasRestart, biasHidden, })
        {
            for (int i = 0; i < bias.Length; i++)
            {
                Msg(bias[i]);
            }
        }
    }
    */

    /*public Dictionary<string, object> ToKeyValue()
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
    }*/
}