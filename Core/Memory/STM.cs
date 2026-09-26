//using System;

using static Model.Write;

namespace Model;

class STM : INeuroComponent
{
    //Из конструктора

    public string name { get; set; } = "Short Term Memory"; // Краткосрочная память
    public string shortName { get; set; } = "stm";
    public string desc { get; set; } = "Short Term Memory";
    public ushort count { get; set; } = 1; // ushort = 2 байта, от 0 до 65535
    public int sizeSTM; public int sizedSTM;
    public int Size() => sizeSTM * count;

    
    //Самосоздающееся
    public double[][][] memory;

#pragma warning disable CS8618

   
    public STM(int size, int sized)
    {
        if(size != 0) sizeSTM = size;
        if(sized != 0) sizedSTM = sized;
        Msg("Инициирован размер sizeSTM: "+ sizeSTM +", sizedSTM: "+ sizedSTM); // x400
        InitSTM();
    }


#pragma warning restore CS8618

    public void InitSTM()
    {
        memory = Init.Zeros(1, sizedSTM, sizeSTM);

        Msg("Инициализация STM закончена...");

        if (memory == null)
            Exc("Память STM осталась незаполненной!");
    }

    public double[][] Get()
    {
        return memory[0];
    }

    public void Set(double[][] Memory)
    {
        memory[0] = Memory;
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
                {"lr", lr},
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
        lr = (double)keyValue["lr"];
        dimension = (int)keyValue["dimension"];
         = (bool)keyValue[""];
    }*/
}