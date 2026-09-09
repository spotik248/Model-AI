//using System;
//using System.Linq;

using SMath = System.Math;

namespace Model;

public static class Math
{
   public const double E = SMath.E;
   public const double PI = SMath.PI;
   public const double Tau = SMath.Tau;
   public const double DegToRad = SMath.PI / 180;

   public static double ExtraRandom(double rand) => SMath.Min(1, rand*2) < 1 ? SMath.Min(1, rand*2) : rand / 2;
   public static int Length<T>(T[][] mas) => mas.Length * mas[0].Length;
   public static int Length<T>(T[][][] mas) => mas.Length * mas[0].Length * mas[0][0].Length;

   public static int Max(int x, int y) => SMath.Max(x, y);
   public static float Max(float x, float y) => SMath.Max(x, y);
   public static double Max(double x, double y) => SMath.Max(x, y);

   public static int Min(int x, int y) => SMath.Min(x, y);
   public static float Min(float x, float y) => SMath.Min(x, y);
   public static double Min(double x, double y) => SMath.Min(x, y);

   public static double Max(params double[] mas)
   {
      double max = double.NegativeInfinity;
      for (int i = 0; i < mas.Length; i++)
         max = SMath.Max(max, mas[i]);
        
      return max;
   }

   public static double Min(params double[] mas)
   {
      double min = double.PositiveInfinity;
      for (int i = 0; i < mas.Length; i++)
         min = SMath.Min(min, mas[i]);
        
      return min;
   }

   public static double Max(params double[][] mas)
   {
      double max = double.NegativeInfinity;
      for (int i = 0; i < mas.Length; i++)
         for (int j = 0; j < mas.Length; j++)
            max = Max(max, mas[i][j]);

      return max;
   }

   public static double Min(params double[][] mas)
   {
      double min = double.PositiveInfinity;
      for (int i = 0; i < mas.Length; i++)
         for (int j = 0; j < mas.Length; j++)
            min = Min(min, mas[i][j]);

      return min;
   }

   public static double MinMax(double num, double min, double max)
   {
      return Max(min, Min(max, num));
   }

   public static double Round(double x, int n = 3) => SMath.Round(x, n);
   public static double[] Round(double[] mas, int n = 3) => mas.Select(x => Round(x, n)).ToArray();
   public static double[][] Round(double[][] mat, int n = 3) => mat.Select(mas => mas.Select(x => Round(x, n)).ToArray()).ToArray();

   public static int RoundInt(double x, int n = 3) => (int)SMath.Round(x, n);
   public static int[] RoundInt(double[] mas, int n = 3) => mas.Select(x => (int)Round(x, n)).ToArray();

   public static double Ceil(double x) => SMath.Ceiling(x); // Floor(x)
   public static double[] Ceil(double[] mas) => mas.Select(x => SMath.Ceiling(x)).ToArray(); // Ceil(x)

   public static double Floor(double x) => SMath.Floor(x); // Floor(x)
   public static double[] Floor(double[] mas) => mas.Select(x => SMath.Floor(x)).ToArray(); // Floor(x)

   public static double Abs(double x) => SMath.Abs(x);

   public static int Abs(int x) => SMath.Abs(x);

   public static double[] Abs(double[] mas) => mas.Select(SMath.Abs).ToArray();

   public static double[] Flat(double[][] mas)
   {
      if (mas == null || mas.Length == 0) return [];
      
      double[] flatArray = new double[Length(mas)];

      for (int i = 0, k = 0; i < mas.Length; i++)
         for (int j = 0; j < mas[0].Length; j++, k++)
            flatArray[k] = mas[i][j];

      return flatArray;
   }

   public static double Pow(double pow, double to) => SMath.Pow(pow, to);
   public static double[] Pow(double[] pow, double to) => pow.Select(x => SMath.Pow(x, to)).ToArray();

   public static double Pow2(double pow) => SMath.Pow(pow, 2);

   public static double[] Pow2(double[] pow) => Pow(pow, 2);

   public static double Pow3(double pow) => SMath.Pow(pow, 3);

   public static double[] Pow3(double[] pow) => Pow(pow, 3);

   public static double Sqrt(double x) => SMath.Sqrt(x);
   public static double Exp(double x) => SMath.Exp(x);
   public static double Cos(double x) => SMath.Cos(x);
   public static double Sin(double x) => SMath.Sin(x);
   public static double Tan(double x) => SMath.Tan(x);
   public static double Ctg(double x) => 1/SMath.Tan(x);
   public static double Tanh(double x) => SMath.Tanh(x);

   public static int Sign(double x) => SMath.Sign(x);

   public static double[] MSE(double[] t, double[] a)
   {
      for(int i = 0; i < t.Length; i++)
         t[i] = Pow2(t[i]-a[i]) / 2; // (t - a) ^ 2 / 2 
      
      return t;
   }

   public static double[][] MSE(double[][] t, double[][] a)
   {
      for (int i = 0; i < t.Length; i++)
         for (int j = 0; j < t[0].Length; j++)
            t[i][j] = SMath.Pow(t[i][j] - a[i][j], 2) / 2;

      return t;
   }

   // Harder methods

   public static double[] MeanAllNVector(double[][] vector)
   {
      double[] result = new double[vector[0].Length];
      for (int j = 0; j < vector[0].Length; j++)
      {
         double allMassive = 0;
         for (int i = 0; i < vector.Length; i++)
            allMassive += vector[i][j];
         
         result[j] = allMassive / vector.Length;
      }
      return result;
   }

   public static double MeanVector(double[] mas1, double[] mas2)
   {
      if (mas1 == null || mas2 == null || mas1.Length != mas2.Length || mas1.Length == 0)
         return 0.0;

      double sumSimilarity = 0;

      for(int i = 0; i < mas1.Length; i++)
      {
         // 1 - |x - y| оценивает схожесть (1 - полное совпадение, 0 - абсолютное расхождение)
         sumSimilarity += 1.0 - Abs(mas1[i] - mas2[i]);
      }

      return sumSimilarity / mas1.Length; 
   }

   public static double[][] Filter(double[][] input, params double[][] filter)
   {
      if (input == null || input.Length == 0) return [];
      
      int dim = input[0].Length;
      var result = new List<double[]>();

      foreach (var row in input)
      {
         bool shouldExclude = false;

         foreach (var fRow in filter)
         {
            bool isFullMatch = true;
            for (int j = 0; j < dim; j++)
            {
               // Если хотя бы один элемент не совпал, то строка не подходит под этот фильтр
               if (SMath.Abs(row[j] - fRow[j]) > 1e-9)
               {
                  isFullMatch = false;
                  break; 
               }
            }

            // Если строка полностью совпала с одним из "запрещенных" фильтров
            if (isFullMatch)
            {
               shouldExclude = true;
               break; // Дальше фильтры проверять нет смысла, строку нужно выкинуть
            }
         }

         // Если строка не совпала ни с одним фильтром — сохраняем её
         if (!shouldExclude)
         {
            result.Add(row);
         }
      }
      return result.ToArray();
   }

   public static double[][] MoveFewVector(double moveOn, params double[][] vec)
   {
      double[][] result = Init.Double<double>(vec.Length, vec[0].Length);
      double[] mean = MeanAllNVector(vec);
      for (int i = 0; i < vec.Length; i++)
      {
         for (int j = 0; j < vec[0].Length; j++)
         {
            double part = mean[j] - vec[i][j];
            result[i][j] = vec[i][j] + part * moveOn;
         }
      }
      return result;
   }

   public static Word[] MoveVectorWords(Word[] words, double moveOn, double[][] word, double percent) // dotnet build -c Release
   {
      int dim = word[0].Length;
      double[][] filterWord = Filter(word, Init.Full(0.0, dim), Init.Full(0.5, dim));
      double[][] moveWord = MoveFewVector(moveOn, filterWord); // 1/100
      for (int i = 0; i < words.Count(); i++)
      {
         for (int j = 0; j < moveWord.Length; j++)
         {
            double mean = MeanVector(words[i].vector, filterWord[j]);
            if (mean >= percent)
            {
               //Write.Line($"{words[i].token} {Vector.AddAll(Math.Round(words[i].vector))} = {j} {Vector.AddAll(Math.Round(moveWord[j]))}");
               words[i].vector = moveWord[j];
            }
         }
      }
      return words;
   }

   public static double[][] VectorToMatrix(double[] vector, int dim)
   {
      double[][] newVector = Init.Double<double>(vector.Length / dim, dim);
      if (vector.Length % dim == 0)
      {
         for (int i = 0; i < newVector.Length; i++)
         {
            for (int j = 0; j < newVector[0].Length; j++)
            {
               newVector[i][j] = 0;
               newVector[i][j] = vector[i * dim + j];
            }
         }
         return newVector;
      }
      Write.Throw($"Не удалось преобразовать вектор в матрицу с размерами {vector.Length} в {vector.Length / dim}x{dim}");
      return newVector;
   }

   public static string MakeGrafic(double[] m, int del = 10)
   {
      double l = m.Length / del;
      double n = 0;
      double[] r = new double[del+1];

      for(int i = 0, j = 0; i < m.Length; i++)
      {
         if(i % l == 0)
         {
            if(j < del+1)
               r[j] = Round(n/del);
            n = 0;
            j++;
         }
         n+=m[i];
      }

      return string.Join(" ", r);
   }

   // Override

   public static int[] ToInt(double[] mas) => mas.Select(x => (int)x).ToArray();

   public static bool IsArray(object x) => x.GetType().IsArray;

}