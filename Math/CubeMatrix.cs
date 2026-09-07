//using System;

namespace Model;

public static class CubeMatrix
{
    public static double[][][] Add(double[][][] mas1, double[][][] mas2)
    {
        double[][][] result = Init.Triple<double>(mas1.Length, mas1[0].Length, mas1[0][0].Length);

        for (int i = 0; i < mas1.Length; i++)
            for (int j = 0; j < mas1[0].Length; j++)
                for (int k = 0; k < mas1[0][0].Length; k++)
                    result[i][j][k] = mas1[i][j][k] + mas2[i][j][k];

        return result;
    }

    public static double[][][] Sub(double[][][] mas1, double[][][] mas2)
    {
        double[][][] result = Init.Triple<double>(mas1.Length, mas1[0].Length, mas1[0][0].Length);

        for (int i = 0; i < mas1.Length; i++)
            for (int j = 0; j < mas1[0].Length; j++)
                for (int k = 0; k < mas1[0][0].Length; k++)
                    result[i][j][k] = mas1[i][j][k] - mas2[i][j][k];
                    
        return result;
    }

    public static double[][][] Mult(double[][][] mas1, double[][][] mas2)
    {
        double[][][] result = Init.Triple<double>(mas1.Length, mas1[0].Length, mas1[0][0].Length);

        for (int i = 0; i < mas1.Length; i++)
            for (int j = 0; j < mas1[0].Length; j++)
                for (int k = 0; k < mas1[0][0].Length; k++)
                    result[i][j][k] = mas1[i][j][k] * mas2[i][j][k];
                    
        return result;
    }

    public static double[][][] Div(double[][][] mas1, double[][][] mas2)
    {
        double[][][] result = Init.Triple<double>(mas1.Length, mas1[0].Length, mas1[0][0].Length);

        for (int i = 0; i < mas1.Length; i++)
            for (int j = 0; j < mas1[0].Length; j++)
                for (int k = 0; k < mas1[0][0].Length; k++)
                    result[i][j][k] = mas1[i][j][k] / mas2[i][j][k];
                    
        return result;
    }

    //One
    public static double[][][] AddOne(double[][][] mas, double one)
    {
        double[][][] result = Init.Triple<double>(mas.Length, mas[0].Length, mas[0][0].Length);

        for (int i = 0; i < mas.Length; i++)
            for (int j = 0; j < mas[0].Length; j++)
                for (int k = 0; k < mas[0][0].Length; k++)
                    result[i][j][k] = mas[i][j][k] + one;

        return result;
    }

    public static double[][][] SubOne(double[][][] mas, double one)
    {
        double[][][] result = Init.Triple<double>(mas.Length, mas[0].Length, mas[0][0].Length);

        for (int i = 0; i < mas.Length; i++)
            for (int j = 0; j < mas[0].Length; j++)
                for (int k = 0; k < mas[0][0].Length; k++)
                    result[i][j][k] = mas[i][j][k] - one;
                    
        return result;
    }

    public static double[][][] MultOne(double[][][] mas, double one)
    {
        double[][][] result = Init.Triple<double>(mas.Length, mas[0].Length, mas[0][0].Length);
        
        for (int i = 0; i < mas.Length; i++)
            for (int j = 0; j < mas[0].Length; j++)
                for (int k = 0; k < mas[0][0].Length; k++)
                    result[i][j][k] = mas[i][j][k] * one;

        return result;
    }

    public static double[][][] DivOne(double[][][] mas, double one)
    {
        double[][][] result = Init.Triple<double>(mas.Length, mas[0].Length, mas[0][0].Length);
        for (int i = 0; i < mas.Length; i++)
        {
            for (int j = 0; j < mas[0].Length; j++)
            {
                for (int k = 0; k < mas[0][0].Length; k++)
                {
                    result[i][j][k] = mas[i][j][k] / one;
                }
            }
        }
        return result;
    }

    //All
    public static double AddAll(double[][][] mas)
    {
        double result = 0;

        foreach (double[][] mat in mas)
            foreach (double[] vec in mat)
                foreach (double one in vec)
                    result += one;
        
        return result;
    }

    public static double SubAll(double[][][] mas)
    {
        double result = 0;

        foreach (double[][] mat in mas)
            foreach (double[] vec in mat)
                foreach (double one in vec)
                    result -= one;

        return result;
    }

    public static double MultAll(double[][][] mas)
    {
        double result = 1;

        foreach (double[][] mat in mas)
            foreach (double[] vec in mat)
                foreach (double one in vec)
                    result *= one;
        
        return result;
    }

    public static double DivAll(double[][][] mas)
    {
        double result = 1;

        foreach (double[][] mat in mas)
            foreach (double[] vec in mat)
                foreach (double one in vec)
                    result /= one;

        return result;
    }

    //AddAll
    public static double AddAddAll(double[][][] mas1, double[][][] mas2)
    {
        double[][][] result = Init.Triple<double>(mas1.Length, mas1[0].Length, mas1[0][0].Length);

        for (int i = 0; i < mas1.Length; i++)
            for (int j = 0; j < mas1[0].Length; j++)
                for (int k = 0; k < mas1[0][0].Length; k++)
                    result[i][j][k] = mas1[i][j][k] + mas2[i][j][k];

        return AddAll(result);
    }

    public static double SubAddAll(double[][][] mas1, double[][][] mas2)
    {
        double[][][] result = Init.Triple<double>(mas1.Length, mas1[0].Length, mas1[0][0].Length);

        for (int i = 0; i < mas1.Length; i++)
            for (int j = 0; j < mas1[0].Length; j++)
                for (int k = 0; k < mas1[0][0].Length; k++)
                    result[i][j][k] = mas1[i][j][k] - mas2[i][j][k];

        return AddAll(result);
    }

    public static double MultAddAll(double[][][] mas1, double[][][] mas2)
    {
        double[][][] result = Init.Triple<double>(mas1.Length, mas1[0].Length, mas1[0][0].Length);

        for (int i = 0; i < mas1.Length; i++)
            for (int j = 0; j < mas1[0].Length; j++)
                for (int k = 0; k < mas1[0][0].Length; k++)
                    result[i][j][k] = mas1[i][j][k] * mas2[i][j][k];

        return AddAll(result);
    }

    public static double DivAddAll(double[][][] mas1, double[][][] mas2)
    {
        double[][][] result = Init.Triple<double>(mas1.Length, mas1[0].Length, mas1[0][0].Length);

        for (int i = 0; i < mas1.Length; i++)
            for (int j = 0; j < mas1[0].Length; j++)
                for (int k = 0; k < mas1[0][0].Length; k++)
                    result[i][j][k] = mas1[i][j][k] / mas2[i][j][k];
        
        return AddAll(result);
    }

    //Other
    public static double[][][] MultMat(double[][][] mas, double[][] one)
    {
        double[][][] result = Init.Triple<double>(mas.Length, mas[0].Length, mas[0][0].Length);

        for (int i = 0; i < mas.Length; i++)
            for (int j = 0; j < mas[0].Length; j++)
                for (int k = 0; k < mas[0][0].Length; k++)
                    result[i][j][k] = mas[i][j][k] * one[i][j];
        
        return result;
    }

    //Simple functies
    public static double[][][] Transpon(double[][][] mas)
    {
        double[][][] result = Init.Triple<double>(mas[0].Length, mas.Length, mas[0][0].Length);
        for (int i = 0; i < mas.Length; i++)
        {
            for (int j = 0; j < mas[0].Length; j++)
            {
                for (int k = 0; k < mas[0][0].Length; k++)
                {
                    result[j][i][k] = mas[i][j][k];
                }
            }
        }
        return result;
    }

    public static double[][][] Pow(double[][][] mas, double to)
    {
        double[][][] result = Init.Triple<double>(mas.Length, mas[0].Length, mas[0][0].Length);
        for (int i = 0; i < mas.Length; i++)
        {
            for (int j = 0; j < mas[0].Length; j++)
            {
                for (int k = 0; k < mas[0][0].Length; k++)
                {
                    result[i][j][k] = Math.Pow(mas[i][j][k], to);
                }
            }
        }
        return result;
    }

    public static double[][][] Abs(double[][][] mas)
    {
        double[][][] result = Init.Triple<double>(mas.Length, mas[0].Length, mas[0][0].Length);
        for (int i = 0; i < mas.Length; i++)
        {
            for (int j = 0; j < mas[0].Length; j++)
            {
                for (int k = 0; k < mas[0][0].Length; k++)
                {
                    result[i][j][k] = Math.Abs(mas[i][j][k]);
                }
            }
        }
        return result;
    }

    public static double[][][] Complement(double[][][] mas)
    {
        for (int i = 0; i < mas.Length; i++)
            for (int j = 0; j < mas[0].Length; j++)
                for (int k = 0; k < mas[0][0].Length; k++)
                    mas[i][j][k] = 1.0 - mas[i][j][k];
                
        return mas;
    }

    public static double Mean(double[][][] mas) => AddAll(mas) / Math.Length(mas);

    //Hard functies // TODO: Некоторые функции могут работать некорректно, проверь их!
    public static double[][][] Combinate(double[][][] vec1, double[][][] vec2)
    {
        int size = vec1.Length + vec2.Length;
        int col = vec1[0].Length;
        int dim = vec1[0][0].Length;
        if (vec1[0].Length != vec2[0].Length || vec1[0][0].Length != vec2[0][0].Length)
            Write.Throw($"Разный размер измерений: {vec1[0].Length} != {vec2[0].Length} || {vec1[0][0].Length} != {vec2[0][0].Length}");

        double[][][] combinate = Init.Triple<double>(size, col, dim);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < col; j++)
            {
                for (int k = 0; k < dim; k++)
                {
                    if (i < vec1.Length)
                    {
                        combinate[i][j][k] = vec1[i][j][k];
                    }
                    else
                    {
                        combinate[i][j][k] = vec2[i - vec1.Length][j][k];
                    }
                }
            }
        }
        return combinate;
    }

    public static (double[][][] mas1, double[][][] mas2) ToBiggest(double[][][] mas1, double[][][] mas2)
    {
        if (mas1.Length > mas2.Length)
        {
            double[][][] resultMas1 = Init.Triple<double>(mas1.Length, mas1[0].Length, mas1[0][0].Length);
            double[][][] resultMas2 = Init.Triple<double>(mas1.Length, mas1[0].Length, mas1[0][0].Length);
            for (int i = 0; i < mas1.Length; i++)
            {
                for (int j = 0; j < mas1[0].Length; j++)
                {
                    for (int k = 0; k < mas1[0][0].Length; k++)
                    {
                        resultMas1[i][j][k] = mas1[i][j][k];
                        if (i < mas2.Length && j < mas2[0].Length && k < mas2[0][0].Length)
                        {
                            resultMas2[i][j][k] = mas2[i][j][k];
                        }
                        else
                        {
                            resultMas2[i][j][k] = 0;
                        }
                    }
                }
            }
            return (resultMas1, resultMas2);
        }
        else if (mas1.Length < mas2.Length)
        {
            double[][][] resultMas1 = Init.Triple<double>(mas2.Length, mas2[0].Length, mas2[0][0].Length);
            double[][][] resultMas2 = Init.Triple<double>(mas2.Length, mas2[0].Length, mas2[0][0].Length);
            for (int i = 0; i < mas2.Length; i++)
            {
                for (int j = 0; j < mas2[0].Length; j++)
                {
                    for (int k = 0; k < mas2[0][0].Length; k++)
                    {
                        resultMas2[i][j][k] = mas2[i][j][k];
                        if (i < mas1.Length && j < mas1[0].Length && k < mas1[0][0].Length)
                        {
                            resultMas1[i][j][k] = mas1[i][j][k];
                        }
                        else
                        {
                            resultMas1[i][j][k] = 0;
                        }
                    }
                }
            }
            return (resultMas1, resultMas2);
        }
        return (mas1, mas2);
    }

    public static double[][][] ConcatToSize(double[][][] mas, params int[] size)
    {
        if (size.Length != 3) Write.Throw($"Размерность не подходит для обрезания кубической матрицы: {size.Length} != 3");
        if (mas.Length == size[0] && mas[0].Length == size[1] && mas[0][0].Length == size[2]) return mas;
        double[][][] result = Init.Triple<double>(size[0], size[1], size[2]);
        for (int i = 0; i < size[0]; i++)
        {
            for (int j = 0; j < size[1]; j++)
            {
                for (int k = 0; k < size[2]; k++)
                {
                    if (i < mas.Length && j < mas[0].Length && k < mas[0][0].Length)
                    {
                        result[i][j][k] = mas[i][j][k];
                    }
                    else
                    {
                        result[i][j][k] = 0;
                    }
                }
            }
        }
        return result;
    }

    public static double[][][] MSE(double[][][] t, double[][][] a)
    {
        double[][][] result = Init.Triple<double>(t.Length, t[0].Length, t[0][0].Length);
        for (int i = 0; i < t.Length; i++)
        {
            for (int j = 0; j < t[0].Length; j++)
            {
                for (int k = 0; k < t[0][0].Length; k++)
                {
                    result[i][j][k] = Math.Pow(t[i][j][k] - a[i][j][k], 2) / 2;
                }
            }
        }
        return result;
    }

    public static double[][] ToMatrix(double[][][] mat)
    {
        double[][] result = Init.Double<double>(mat.Length, mat[0].Length);
        for (int i = 0; i < mat.Length; i++)
            for (int j = 0; j < mat[0].Length; j++)
                for (int k = 0; k < mat[0][0].Length; k++)
                    result[i][j] += mat[i][j][k];

        return result;
    }
    
    public static double[] ToVector(double[][][] mat)
    {
        double[] result = new double[mat.Length];
        for (int i = 0; i < mat.Length; i++)
        {
            for (int j = 0; j < mat[0].Length; j++)
            {
                for (int k = 0; k < mat[0][0].Length; k++)
                {
                    result[i] += mat[i][j][k];
                }
            }
        }
        return result;
    }
}