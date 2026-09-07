//using System;
//using System.Linq;
//using System.Collections;
//using System.Collections.Generic;

namespace Model;

public static class Vector
{

    #region Выражения

    // Простые выражения
    public static double[] Add(double[] mas1, double[] mas2) // mas1 + mas2
    {
        double[] mas = new double[mas1.Length];

        for (int i = 0; i < mas1.Length; i++)
            mas[i] = mas1[i] + mas2[i];
        
        return mas;
    }

    public static double[] Sub(double[] mas1, double[] mas2) // mas1 - mas2
    {
        double[] mas = new double[mas1.Length];

        for (int i = 0; i < mas1.Length; i++)
            mas[i] = mas1[i] - mas2[i];
        
        return mas;
    }

    public static double[] Mult(double[] mas1, double[] mas2) // mas1 * mas2
    {
        double[] mas = new double[mas1.Length];

        for (int i = 0; i < mas1.Length; i++)
            mas[i] = mas1[i] * mas2[i];
        
        return mas;
    }

    public static double[] Div(double[] mas1, double[] mas2) // mas1 / mas2
    {
        double[] mas = new double[mas1.Length];

        for (int i = 0; i < mas1.Length; i++)
            mas[i] = mas1[i] / mas2[i];
        
        return mas;
    }

    // Mas выражения
    public static double[] AddOne(double[] mas, double one) // mas + one
    {
        double[] result = new double[mas.Length];

        for (int i = 0; i < mas.Length; i++)
            result[i] = mas[i] + one;
        
        return result;
    }

    public static double[] SubOne(double[] mas, double one) // mas - one
    {
        double[] result = new double[mas.Length];

        for (int i = 0; i < mas.Length; i++)
            result[i] = mas[i] - one;
        
        return result;
    }

    public static double[] MultOne(double[] mas, double one) // mas * one
    {
        double[] result = new double[mas.Length];

        for (int i = 0; i < mas.Length; i++)
            result[i] = mas[i] * one;
        
        return result;
    }

    public static double[] DivOne(double[] mas, double one) // mas / one
    {
        if(one == 0) return [];

        double[] result = new double[mas.Length];

        for (int i = 0; i < mas.Length; i++)
            result[i] = mas[i] / one;
        
        return result;
    }

    //All выражения
    public static double AddAll(double[] mas) // result += x
    {
        double result = 0;
        foreach (double one in mas) result += one;
        return result;
    }

    public static double SubAll(double[] mas) // result -= x
    {
        double result = 0;
        foreach (double one in mas) result -= one;
        return result;
    }

    public static double MultAll(double[] mas) // result *= x
    {
        double result = 1;
        foreach (double one in mas) result *= one;
        return result;
    }

    public static double DivAll(double[] mas) // result /= x
    {
        double result = 1;
        foreach (double one in mas) result /= one;
        return result;
    }

    // AddAll dsражения
    public static double AddAddAll(double[] mas1, double[] mas2) => AddAll(Add(mas1, mas2)); // AddAll(mas1 + mas2)
    
    public static double SubAddAll(double[] mas1, double[] mas2) => AddAll(Sub(mas1, mas2)); // AddAll(mas1 - mas2)

    public static double MultAddAll(double[] mas1, double[] mas2) => AddAll(Mult(mas1, mas2)); // AddAll(mas1 * mas2)
    
    public static double DivAddAll(double[] mas1, double[] mas2) => AddAll(Div(mas1, mas2)); // AddAll(mas1 / mas2)

    //Другие выражения
    public static double[] MultOuter(double[] mas1, double[] mas2) // mas1[i] * mas2[j] = matrix[i][j]
    {
        double[] result = new double[mas1.Length * mas2.Length];

        for (int i = 0, k = 0; i < mas1.Length; i++)
            for (int j = 0; j < mas2.Length; j++, k++)
                result[k] = mas1[i] * mas2[j];
        
        return result;
    }
    
    #endregion

    #region Легкие функции

    public static double[] Tanh(double[] vec) => vec.Select(Math.Tanh).ToArray(); // Tanh(x)

    public static double[] Exp(double[] vec) => vec.Select(Math.Exp).ToArray(); // Exp(x)

    public static double[] Complement(double[] vec) => vec.Select(x => 1 - x).ToArray(); // 1 - x
    
    public static double Mean(double[] mas) => AddAll(mas) / mas.Length; // AddAll(x) / x.Length

    #endregion

    #region Тяжелые функции

    public static double[] Combinate(double[] vec1, double[] vec2)
    {
        int size = vec1.Length + vec2.Length;

        double[] combinate = new double[size];

        for (int i = 0; i < size; i++)
        {
            if (i < vec1.Length)
            {
                combinate[i] = vec1[i];
            }
            else
            {
                combinate[i] = vec2[i - vec1.Length];
            }
        }
        return combinate;
    }

    public static double[] ToBiggest(double[] mas1, double[] mas2)
    {
        if (mas1.Length == mas2.Length) return mas1;

        double[] mas = mas1.Length >= mas2.Length ? mas1 : mas2;

        double[] resultMas = Init.Zeros(mas.Length);
        
        for (int i = 0; i < mas.Length; i++)
            resultMas[i] = mas[i];

        return resultMas;
    }

    public static double[] ToSize(double[] mas, int size)
    {
        int min = Math.Min(size, mas.Length);

        double[] result = Init.Zeros(size);

        for (int i = 0; i < min; i++)
            result[i] = mas[i];
        
        return result;
    }

    public static double[][] ToMatrix(double[] mas, int dimension)
    {
        if(mas.Length / dimension != 1)
            Write.Exc($"Размеры не подходят для перевода в матрицу:\n{mas.Length} / {dimension} = {mas.Length / dimension} должно быть целым числом (Integer)");

        double[][] result = Init.Double<double>(mas.Length / dimension, dimension);

        for (int i = 0, k = 0; i < mas.Length / dimension; i++)
            for (int j = 0; j < dimension; j++, k++)
                result[i][j] = mas[k];

        return result;
    }

    public static double[] ToDouble(int[] mas) => mas.Select(x => (double)x).ToArray();
    public static double[] ToDouble(object[] mas) => mas.Select(x => (double)x).ToArray();

    public static int[] ToInt(double[] mas) => mas.Select(x => (int)x).ToArray();

    #endregion

}