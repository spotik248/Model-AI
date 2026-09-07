//using System;

namespace Model;

public struct Matrix
{

    #region Выражения

    // Простые выражения
    public static double[][] Add(double[][] mas1, double[][] mas2)
    {
        double[][] mas = Init.Double<double>(mas1.Length, mas1[0].Length);

        for (int i = 0; i < mas1.Length; i++)
            for (int j = 0; j < mas1[0].Length; j++)
                mas[i][j] = mas1[i][j] + mas2[i][j];

        return mas1;
    }

    public static double[][] Sub(double[][] mas1, double[][] mas2)
    {
        double[][] mas = Init.Double<double>(mas1.Length, mas1[0].Length);

        for (int i = 0; i < mas1.Length; i++)
            for (int j = 0; j < mas1[0].Length; j++)
                mas[i][j] = mas1[i][j] - mas2[i][j];

        return mas1;
    }
    
    public static double[][] Mult(double[][] mas1, double[][] mas2)
    {
        double[][] mas = Init.Double<double>(mas1.Length, mas1[0].Length);

        for (int i = 0; i < mas1.Length; i++)
            for (int j = 0; j < mas1[0].Length; j++)
                mas[i][j] = mas1[i][j] * mas2[i][j];

        return mas1;
    }

    public static double[][] Div(double[][] mas1, double[][] mas2)
    {
        double[][] mas = Init.Double<double>(mas1.Length, mas1[0].Length);

        for (int i = 0; i < mas1.Length; i++)
            for (int j = 0; j < mas1[0].Length; j++)
                mas[i][j] = mas1[i][j] / mas2[i][j];

        return mas1;
    }
    
    // Mas выражения
    public static double[][] AddOne(double[][] mas, double one)
    {
        double[][] result = Init.Double<double>(mas.Length, mas[0].Length);

        for (int i = 0; i < mas.Length; i++)
            for (int j = 0; j < mas[0].Length; j++)
                result[i][j] = mas[i][j] + one;

        return result;
    }

    public static double[][] SubOne(double[][] mas, double one)
    {
        double[][] result = Init.Double<double>(mas.Length, mas[0].Length);

        for (int i = 0; i < mas.Length; i++)
            for (int j = 0; j < mas[0].Length; j++)
                result[i][j] = mas[i][j] - one;

        return result;
    }

    public static double[][] MultOne(double[][] mas, double one)
    {
        double[][] result = Init.Double<double>(mas.Length, mas[0].Length);

        for (int i = 0; i < mas.Length; i++)
            for (int j = 0; j < mas[0].Length; j++)
                result[i][j] = mas[i][j] * one;

        return result;
    }

    public static double[][] DivOne(double[][] mas, double one)
    {
        if(one == 0) return [];

        double[][] result = Init.Double<double>(mas.Length, mas[0].Length);

        for (int i = 0; i < mas.Length; i++)
            for (int j = 0; j < mas[0].Length; j++)
                result[i][j] = mas[i][j] / one;

        return result;
    }
    
    // Vec выражения
    public static double[][] AddVec(double[][] mas, double[] vec)
    {
        double[][] result = Init.Double<double>(mas.Length, mas[0].Length);

        for (int i = 0; i < mas.Length; i++)
            for (int j = 0; j < mas[0].Length; j++)
                result[i][j] = mas[i][j] + vec[j];

        return result;
    }

    public static double[][] MultVec(double[][] mas1, double[] vec)
    {
        double[][] result = Init.Double<double>(mas1.Length, mas1[0].Length);
        
        for (int i = 0; i < mas1.Length; i++)
            for (int j = 0; j < mas1[0].Length; j++)
                result[i][j] = mas1[i][j] * vec[j];
        
        return result;
    }
    
    // All выражения
    public static double AddAll(double[][] mas)
    {
        double result = 0;

        foreach (double[] vec in mas)
            foreach (double one in vec)
                result += one;

        return result;
    }

    public static double SubAll(double[][] mas)
    {
        double result = 0;

        foreach (double[] vec in mas)
            foreach (double one in vec)
                result -= one;
        
        return result;
    }

    public static double MultAll(double[][] mas)
    {
        double result = 1;

        foreach (double[] vec in mas)
            foreach (double one in vec)
                result *= one;
            
        return result;
    }

    public static double DivAll(double[][] mas)
    {
        double result = 1;

        foreach (double[] vec in mas)
            foreach (double one in vec)
                result /= one;
        
        return result;
    }
    
    // AddAll выражения

    public static double AddAddAll(double[][] mas1, double[][] mas2)
    {
        double[][] result = Init.Double<double>(mas1.Length, mas1[0].Length);

        for (int i = 0; i < mas1.Length; i++)
            for (int j = 0; j < mas1[0].Length; j++)
                result[i][j] = mas1[i][j] + mas2[i][j];
        
        return AddAll(result);
    }

    public static double SubAddAll(double[][] mas1, double[][] mas2)
    {
        double[][] result = Init.Double<double>(mas1.Length, mas1[0].Length);

        for (int i = 0; i < mas1.Length; i++)
            for (int j = 0; j < mas1[0].Length; j++)
                result[i][j] = mas1[i][j] - mas2[i][j];
        
        return AddAll(result);
    }

    public static double MultAddAll(double[][] mas1, double[][] mas2)
    {
        double[][] result = Init.Double<double>(mas1.Length, mas1[0].Length);

        for (int i = 0; i < mas1.Length; i++)
            for (int j = 0; j < mas1[0].Length; j++)
                result[i][j] = mas1[i][j] * mas2[i][j];
        
        return AddAll(result);
    }
    
    public static double DivAddAll(double[][] mas1, double[][] mas2)
    {
        double[][] result = Init.Double<double>(mas1.Length, mas1[0].Length);

        for (int i = 0; i < mas1.Length; i++)
            for (int j = 0; j < mas1[0].Length; j++)
                result[i][j] = mas1[i][j] / mas2[i][j];
        
        return AddAll(result);
    }
    
    // другие выражения
    public static double[][] SumFew(params double[][][] mas)
    {
        double[][] result = Init.Double<double>(mas[0].Length, mas[0][0].Length);

        for (int i = 0; i < mas[0].Length; i++)
            for (int j = 0; j < mas[0][0].Length; j++)
                for (int k = 0; k < mas.Length; k++)
                    result[i][j] += mas[k][i][j];

        return result;
    }
    
    public static double[][] MultTrans(double[][] mas1, double[][] mas2)
    {
        double[][] result = Init.Double<double>(mas1.Length, mas1[0].Length);

        for (int i = 0; i < mas1.Length; i++)
            for (int j = 0; j < mas1[0].Length; j++)
                result[i][j] = mas1[i][j] * mas2[j][i];
        
        return result;
    }

    public static double[][] MultMat(double[][] mas1, double[][] mas2)
    {
        double[][] result = Init.Double<double>(mas1.Length, mas2[0].Length);

        for (int i = 0; i < mas1.Length; i++)
            for (int j = 0; j < mas2[0].Length; j++)
                for (int k = 0; k < mas1[0].Length; k++)
                    result[i][j] += mas1[i][k] * mas2[k][j];

        return result;
    }

    public static double[][] Transpose(double[][] mas)
    {
        double[][] result = Init.Double<double>(mas[0].Length, mas.Length);

        for (int i = 0; i < mas.Length; i++)
            for (int j = 0; j < mas[0].Length; j++)
                result[j][i] = mas[i][j];
        
        return result;
    }

    /*public static double[][] Displace(double[][] mat)
    {
        double[][] result = Init.Double<double>(mat[0].Length, mat.Length);
        double[] vec = new double[mat.Length * mat[0].Length];
        for (int i = 0, k = 0; i < mat.Length; i++)
        {
            for (int j = 0; j < mat[0].Length; j++, k++)
            {
                vec[k] = mat[i][j];
            }
        }
        
        for (int i = 0, k = 0; i < mat[0].Length; i++)
        {
            for (int j = 0; j < mat.Length; j++, k++)
            {
                result[i][j] = vec[k];
            }
        }

        return result;
    }*/
    
    #endregion

    #region Легкие функции

    public static double[][] Pow(double[][] mas, double to)
    {
        double[][] result = Init.Double<double>(mas.Length, mas[0].Length);

        for (int i = 0; i < mas.Length; i++)
            for (int j = 0; j < mas[0].Length; j++)
                result[i][j] = Math.Pow(mas[i][j], to);
        
        return result;
    }

    public static double[][] Abs(double[][] mas)
    {
        double[][] result = Init.Double<double>(mas.Length, mas[0].Length);

        for (int i = 0; i < mas.Length; i++)
            for (int j = 0; j < mas[0].Length; j++)
                result[i][j] = Math.Abs(mas[i][j]);
        
        return result;
    }

    public static double[][] Exp(double[][] mas)
    {
        double[][] result = Init.Double<double>(mas.Length, mas[0].Length);

        for (int i = 0; i < mas.Length; i++)
            for (int j = 0; j < mas[0].Length; j++)
                result[i][j] = Math.Exp(mas[i][j]);
        
        return result;
    }

    public static double[][] Complement(double[][] mas)
    {
        double[][] result = Init.Double<double>(mas.Length, mas[0].Length);

        for (int i = 0; i < mas.Length; i++)
            for (int j = 0; j < mas[0].Length; j++)
                result[i][j] = 1 - mas[i][j];
        
        return result;
    }

    public static double Mean(double[][] mas) => AddAll(mas) / Math.Length(mas);

    #endregion

    #region Тяжелые функции

    public static double[][] Combinate(double[][] vec1, double[][] vec2)
    {
        if (vec1[0].Length != vec2[0].Length)
            Write.Throw($"Разный размер измерений: {vec1[0].Length} != {vec2[0].Length}");

        int size = vec1.Length + vec2.Length;
        int dim = vec1[0].Length;

        double[][] combinate = Init.Double<double>(size, dim);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                if (i < vec1.Length)
                {
                    combinate[i][j] = vec1[i][j];
                }
                else
                {
                    combinate[i][j] = vec2[i - vec1.Length][j];
                }
            }
        }
        return combinate;
    }

    public static double[][] Triangle(double downNum, double upNum, int size, int dimension)
    {
        double[][] result = Init.Double<double>(size, dimension);

        for (int i = 0; i < size; i++)
            for (int j = 0; j < dimension; j++)
                result[i][j] = i - j > 0 ? downNum : upNum;

        return result;
    }

    public static double[][] ToBiggest(double[][] mas1, double[][] mas2)
    {
        double[][] mas = mas1.Length >= mas2.Length ? mas1 : mas2;

        if (mas1.Length != mas2.Length)
        {
            double[][] resultMas = Init.Zeros(mas.Length, mas[0].Length);

            for (int i = 0; i < mas1.Length; i++)
            {
                for (int j = 0; j < mas1[0].Length; j++)
                {
                    if (i < mas2.Length && j < mas2[0].Length)
                        resultMas[i][j] = mas2[i][j];
                    
                }
            }
            return resultMas;
        }
        
        Write.Debug("Исключение для массива");
        return mas; // TODO: Исключение
    }

    public static double[][] ToSize(double[][] mas, int size, int dim)
    {
        if (mas.Length == size && mas[0].Length == dim)
            return mas;

        int minSize = Math.Min(mas.Length, size);
        int minDim = Math.Min(mas[0].Length, dim);

        double[][] result = Init.Zeros(size, dim);

        for (int i = 0; i < minSize; i++)
            for (int j = 0; j < minDim; j++)
                    result[i][j] = mas[i][j];

        return result;
    }

    public static double[][] ToSizeHalf(double[][] mas, int size, int dim)
    {
        if (mas.Length == size && mas[0].Length == dim)
            return mas;

        int minSize = Math.Min(mas.Length, size);
        int minDim = Math.Min(mas[0].Length, dim);

        double[][] result = Init.Half(size, dim);

        for (int i = 0; i < minSize; i++)
            for (int j = 0; j < minDim; j++)
                    result[i][j] = mas[i][j];

        return result;
    }

    public static string ToString(double[][] mat)
    {
        string result = "";

        for (int i = 0; i < mat.Length; i++)
            result += string.Join(" ", mat[i]) + "\n";

        return result;
    }

    public static double[] ToVector(double[][] mas)
    {
        if(mas is null)
        {
            Write.Exc($"Был введен null для перевода в Vector: {mas}");
            return [];
        }
        
        double[] result = new double[mas.Length * mas[0].Length];

        for (int i = 0, k = 0; i < mas.Length; i++)
            for (int j = 0; j < mas[0].Length; j++, k++)
                result[k] = mas[i][j];
        
        return result;
    }

    public static double[][] ToDouble(int[][] mas) => mas.Select(x => x.Select(x => (double)x).ToArray()).ToArray();
    public static double[][] ToDouble(object[][] mas) => mas.Select(x => x.Select(x => (double)x).ToArray()).ToArray();
    
    public static int[][] ToInt(double[][] mas) => mas.Select(x => x.Select(x => (int)x).ToArray()).ToArray();

    #endregion

}