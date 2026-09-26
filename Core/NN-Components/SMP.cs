//using System;

using static Model.Write;
using static Model.Learn;

namespace Model;

public class SMP : INeuroComponent // Self Making Perceptron
{

    public string name { get; set; } = "Self Making Perceptron";
    public string shortName { get; set; } = "smp";
    public string desc { get; set; } = "Self Making Perceptron, needs some many lengthes for make layouts";
    public ushort count { get; set; } = 1; // ushort = 2 байта, от 0 до 65535
    public int[] sizes;
    public int Size() => (int)Vector.AddAll(Math.ToDouble(sizes)) * count / 2;
    
#pragma warning disable CS8618

    public double[][] inputs;
    public double[] outputLayout;
    public double[] errorOut;
    public double[][] errors;
    public LP[] lp;



    public SMP(params int[] Sizes)
    {
        sizes = Sizes;
        if(sizes.Length < 2) sizes = [sizes[0], sizes[0]];

        lp ??= new LP[sizes.Length-1];

        for(int i = 0; i < sizes.Length-1; i++)
            lp[i] = new LP(sizes[i], sizes[i+1]);   
        

        Msg($"Инициализация {name} закончена...");

        //if(sizes.Length < 1) убрать
        //    Exc($"Количество слоев в перцептроне меньше 1: "+sizes.Length);
    }

#pragma warning restore CS8618

    // Полное прохождение перцептрона

    public double[] Pass(double[] inputLayout)
    {
        inputs = new double[lp.Length][];
        double[] output = inputLayout;

        for(int i = 0; i < lp.Length; i++)
        {
            inputs[i] = output;
            output = lp[i].Pass(inputs[i]);
        }

        outputLayout = output;

        return outputLayout;
    }

    public void Update(double[] target)
    {
        errorOut = lp[^1].ErrorOut(outputLayout, target);

        errors = new double[lp.Length][];
        errors[^1] = errorOut;

        for(int i = lp.Length-1; i > 0; i--)
        {
            errors[i] = lp[i].Error(lp[i+1].width, errors[i+1]);
        }

        for(int i = lp.Length; i > 0; i--)
        {
            lp[i].Width(inputs[i], errors[i]);
            lp[i].Bias(errors[i]);
        }
    }

}