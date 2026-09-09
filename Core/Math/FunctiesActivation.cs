//using System;

namespace Model;

public static class FucAct
{
    public static double Sigmoid(double x) => 1.0 / (1.0 + Math.Exp(-x));

    public static double[] Sigmoid(double[] x) => x.Select(Sigmoid).ToArray();

    public static double DXSigmoid(double sig) => sig * (1.0 - sig);

    public static double[] DXSigmoid(double[] sig) => sig.Select(DXSigmoid).ToArray();

    public static double DSigmoid(double x)
    {
        double sig = Sigmoid(x);
        return sig * (1.0 - sig);
    }

    public static double[] DSigmoid(double[] x) => x.Select(DSigmoid).ToArray();

    public static double Tanh(double x) => Math.Tanh(x);

    public static double[] Tanh(double[] x) => Vector.Tanh(x);

    public static double DThx(double x) => 1.0 - Math.Pow(Math.Tanh(x), 2);

    public static double[] DThx(double[] x) => x.Select(x => 1.0 - Math.Pow(Math.Tanh(x), 2)).ToArray();

    public static double SoftSign(double x) => x / 1.0 + Math.Abs(x);

    public static double[] SoftSign(double[] x) => x.Select(x => x / (1 + Math.Abs(x))).ToArray();

    public static double DSoftSign(double x) => 1.0 / Math.Pow(1 + Math.Abs(x), 2);

    public static double[] DSoftSign(double[] x) => x.Select(x => 1.0 / Math.Pow(1 + Math.Abs(x), 2)).ToArray();

    public static double ReLu(double x) => Math.Max(0, x);

    public static double[] ReLu(double[] x) => x.Select(ReLu).ToArray();

    public static double DReLu(double x) => x > 0 ? x : x * 0.01;

    public static double[] DReLu(double[] x) => x.Select(DReLu).ToArray();

    public static double LReLu(double x) => Math.Max(0.01, x);

    public static double[] LReLu(double[] x) => x.Select(LReLu).ToArray();

    public static double DLReLu(double x) => x > 0 ? 1.0 : 0.01;

    public static double[] DLReLu(double[] x) => x.Select(DLReLu).ToArray();

    public static int SoftPlus(double x) => Math.Sign(1.0 + Math.Exp(x));

    public static int[] SoftPlus(double[] x) => x.Select(x => Math.Sign(1.0 + Math.Exp(x))).ToArray();

    public static double[][] SoftMax(double[][] x)
    {
        double[][] result = Init.Double<double>(x.Length, x[0].Length);
        for (int i = 0; i < x.Length; i++)
        {
            double max = Math.Max(x[i]);
            double sumExp = Vector.AddAll(Vector.Exp(Vector.SubOne(x[i], max))); // Возведение в экспоненту и суммирование
            for (int j = 0; j < x[0].Length; j++)
                result[i][j] = Math.Exp(x[i][j] - max) / sumExp; // Нормированная вероятность
        }

        return result;
    }

    public static double[][] DSoftMax(double[][] act)
    {
        int rows = act.Length;       // 100
        int cols = act[0].Length;    // 100
        double[][] result = Init.Double<double>(rows, cols);

        for (int i = 0; i < rows; i++)
        {
            // Считаем производную независимо для каждой строки i
            for (int j = 0; j < cols; j++)
            {
                double sum = 0;
                for (int k = 0; k < cols; k++)
                {
                    if (j == k)
                    {
                        sum += act[i][j] * (1.0 - act[i][j]);
                    }
                    else
                    {
                        sum -= act[i][j] * act[i][k];
                    }
                }
                result[i][j] = sum;
            }
        }
        return result;
    }

}