//using System;

namespace Model;

public struct Init
{   
    const int MaxNum = 100000;
    // TODO: Без константы, массивы созданые Init будучи слишком большими *ломают* или *замедляют* процесс ИИ
    public static Random Rand = new();
    public static double HE(double sizeble) => Math.Pow(4 / sizeble, 2);
    public static double HE(double reg, double sizeble) => Math.Pow(reg / sizeble, 2);
    

    /// <summary>
    /// Инициализирует многомерный массив указанного типа и размерности.
    /// </summary>
    /// <typeparam name="T">Тип элементов массива.</typeparam>
    /// <param name="dims">Размеры каждого измерения массива.</param>
    /// <returns>Инициализированный массив.</returns>

    public static T[][] Double<T>(int size, int dim)
    {
        if (size == 0 || dim == 0)
            Write.Exc($"Double Поддерживается только создание двухуровневого ступенчатого массива.  {size}, {dim}");
        if (size >= MaxNum || dim >= MaxNum)
            Write.Exc($"Very big vector. {size}, {dim}");

        T[][] result = new T[size][];

        for (int i = 0; i < size; i++)
            result[i] = new T[dim];
        
        return result;
    }

    public static T[][][] Triple<T>(int size, int dim, int deep)
    {
        if (size == 0 || dim == 0 || deep == 0)
            Write.Exc($"Triple Поддерживается только создание трёхуровневого ступенчатого массива. {size}, {dim}, {deep}");
        if (size >= MaxNum || dim >= MaxNum || deep >= MaxNum)
            Write.Exc($"Very big vector. {size}, {dim}, {deep}");

        T[][][] result = new T[size][][];

        for (int i = 0; i < size; i++)
        {
            result[i] = new T[dim][];
            for (int j = 0; j < dim; j++)
                result[i][j] = new T[deep];

        }

        return result;
    }

    public static T[][] Copy<T>(T[][] m)
    {
        if (m.Length == 0 || m[0].Length == 0)
            Write.Exc($"Copy Поддерживается только создание двухуровневого ступенчатого массива. {m.Length}, {m[0].Length}");
        if (m.Length >= MaxNum || m[0].Length >= MaxNum)
            Write.Exc($"Very big vector. {m.Length}, {m[0].Length}");

        T[][] result = new T[m.Length][];

        for (int i = 0; i < m.Length; i++)
            result[i] = new T[m[0].Length];
        
        return result;
    }

    // Рандомы
    public static double[] Randomized(int dim)
    {
        if (dim >= MaxNum) Write.Throw($"Very big vector. {dim}");

        double[] array = new double[dim];
        
        for (int i = 0; i < dim; i++)
            array[i] = Rand.NextDouble();
        
        return array;
    }

    public static double[][] Randomized(int size, int dim)
    {
        if (size >= MaxNum || dim >= MaxNum)
            Write.Throw($"Very big matrix. {size}, {dim}");
        
        double[][] array = Double<double>(size, dim);
        
        for (int i = 0; i < size; i++)
            for (int j = 0; j < dim; j++)
                array[i][j] = Rand.NextDouble();
        
        return array;
    }

    public static double[][][] Randomized(int size, int dim, int deep)
    {
        if (size >= MaxNum || dim >= MaxNum || deep >= MaxNum)
            Write.Throw($"Very big cube-matrix. {size}, {dim}, {deep}");

        double[][][] array = Triple<double>(size, dim, deep);
        
        for (int i = 0; i < size; i++)
            for (int j = 0; j < dim; j++)
                for (int k = 0; k < deep; k++)
                    array[i][j][k] = Rand.NextDouble();
        
        return array;
    }

    // Целое число Рандомы
    public static int[] RandomizedInt(int dim)
    {
        if (dim >= MaxNum) Write.Throw($"Very big vector. {dim}");

        int[] array = new int[dim];
        
        for (int i = 0; i < dim; i++)
            array[i] = Rand.Next();
        
        return array;
    }

    public static int[][] RandomizedInt(int size, int dim)
    {
        if (size >= MaxNum || dim >= MaxNum)
            Write.Throw($"Very big matrix. {size}, {dim}");
        
        int[][] array = Double<int>(size, dim);
        
        for (int i = 0; i < size; i++)
            for (int j = 0; j < dim; j++)
                array[i][j] = Rand.Next();
        
        return array;
    }

    public static int[][][] RandomizedInt(int size, int dim, int deep)
    {
        if (size >= MaxNum || dim >= MaxNum || deep >= MaxNum)
            Write.Throw($"Very big cube-matrix. {size}, {dim}, {deep}");

        int[][][] array = Triple<int>(size, dim, deep);
        
        for (int i = 0; i < size; i++)
            for (int j = 0; j < dim; j++)
                for (int k = 0; k < deep; k++)
                    array[i][j][k] = Rand.Next();
        
        return array;
    }

    // Байтные Рандомы

    public static byte[] RandomizedBytes(int dim)
    {
        if (dim >= MaxNum) Write.Throw($"Very big vector. {dim}");

        byte[] array = new byte[dim];
        
        for (int i = 0; i < dim; i++)
            array[i] = (byte)Rand.Next();
        
        return array;
    }

    public static byte[][] RandomizedBytes(int size, int dim)
    {
        if (size >= MaxNum || dim >= MaxNum)
            Write.Throw($"Very big matrix. {size}, {dim}");
        
        byte[][] array = Double<byte>(size, dim);
        
        for (int i = 0; i < size; i++)
            for (int j = 0; j < dim; j++)
                array[i][j] = (byte)Rand.Next();
        
        return array;
    }

    public static byte[][][] RandomizedBytes(int size, int dim, int deep)
    {
        if (size >= MaxNum || dim >= MaxNum || deep >= MaxNum)
            Write.Throw($"Very big cube-matrix. {size}, {dim}, {deep}");

        byte[][][] array = Triple<byte>(size, dim, deep);
        
        for (int i = 0; i < size; i++)
            for (int j = 0; j < dim; j++)
                for (int k = 0; k < deep; k++)
                    array[i][j][k] = (byte)Rand.Next();
        
        return array;
    }


    // Экстра Рандомы
    public static double[] ExtraRandomized(int dim)
    {
        if (dim >= MaxNum) Write.Throw($"Very big vector. {dim}");
        double[] array = new double[dim];
        
        for (int i = 0; i < dim; i++)
            array[i] = Math.ExtraRandom(Rand.NextDouble());
        
        return array;
    }

    public static double[][] ExtraRandomized(int size, int dim)
    {
        if (size >= MaxNum || dim >= MaxNum)
            Write.Throw($"Very big matrix. {size}, {dim}");

        double[][] array = Double<double>(size, dim);
        
        for (int i = 0; i < size; i++)
            for (int j = 0; j < dim; j++)
                array[i][j] = Math.ExtraRandom(Rand.NextDouble());
        
        return array;
    }

    public static double[][][] ExtraRandomized(int size, int dim, int deep)
    {
        if (size >= MaxNum || dim >= MaxNum || deep >= MaxNum)
            Write.Throw($"Very big cube-matrix. {size}, {dim}, {deep}");

        double[][][] array = Triple<double>(size, dim, deep);
        
        for (int i = 0; i < size; i++)
            for (int j = 0; j < dim; j++)
                for (int k = 0; k < deep; k++)
                    array[i][j][k] = Math.ExtraRandom(Rand.NextDouble());
        
        return array;
    }

    // Type Нули
    public static T[] Zeros<T>(int dim) 
    {
        if (dim >= MaxNum) 
            Write.Throw($"Very big vector. {dim}");
        
        T[] array = new T[dim]; 
        for (int i = 0; i < dim; i++) 
            array[i] = default;
        
        return array;
    }

    public static T[][] Zeros<T>(int size, int dim)
    {
        if (size >= MaxNum || dim >= MaxNum)
            Write.Throw($"Very big matrix. {size}, {dim}");

        T[][] array = Double<T>(size, dim);

        for (int i = 0; i < size; i++)
            for (int j = 0; j < dim; j++)
                array[i][j] = default;
        
        return array;
    }

    public static T[][][] Zeros<T>(int size, int dim, int deep)
    {
        if (size >= MaxNum || dim >= MaxNum || deep >= MaxNum)
            Write.Throw($"Very big matrix. {size}, {dim}, {deep}");

        T[][][] array = Triple<T>(size, dim, deep);
        
        for (int i = 0; i < size; i++)
            for (int j = 0; j < dim; j++)
                for (int k = 0; k < deep; k++)
                    array[i][j][k] = default;

        return array;
    }

    // Нули

    public static double[] Zeros(int dim)
    {
        if (dim >= MaxNum) Write.Throw($"Very big vector. {dim}");

        double[] array = new double[dim];

        for (int i = 0; i < dim; i++)
            array[i] = 0;
        
        return array;
    }

    public static double[][] Zeros(int size, int dim)
    {
        if (size >= MaxNum || dim >= MaxNum)
            Write.Throw($"Very big matrix. {size}, {dim}");

        double[][] array = Double<double>(size, dim);

        for (int i = 0; i < size; i++)
            for (int j = 0; j < dim; j++)
                array[i][j] = 0;
        
        return array;
    }

    public static double[][][] Zeros(int size, int dim, int deep)
    {
        if (size >= MaxNum || dim >= MaxNum || deep >= MaxNum)
            Write.Throw($"Very big matrix. {size}, {dim}, {deep}");

        double[][][] array = Triple<double>(size, dim, deep);
        
        for (int i = 0; i < size; i++)
            for (int j = 0; j < dim; j++)
                for (int k = 0; k < deep; k++)
                    array[i][j][k] = 0;

        return array;
    }

    // Половинки

    public static double[] Half(int dim)
    {
        if (dim >= MaxNum) Write.Throw($"Very big vector. {dim}");

        double[] array = new double[dim];

        for (int i = 0; i < dim; i++)
            array[i] = 0.5;
        
        return array;
    }

    public static double[][] Half(int size, int dim)
    {
        if (size >= MaxNum || dim >= MaxNum)
            Write.Throw($"Very big matrix. {size}, {dim}");

        double[][] array = Double<double>(size, dim);

        for (int i = 0; i < size; i++)
            for (int j = 0; j < dim; j++)
                array[i][j] = 0.5;
        
        return array;
    }

    public static double[][][] Half(int size, int dim, int deep)
    {
        if (size >= MaxNum || dim >= MaxNum || deep >= MaxNum)
            Write.Throw($"Very big matrix. {size}, {dim}, {deep}");

        double[][][] array = Triple<double>(size, dim, deep);
        
        for (int i = 0; i < size; i++)
            for (int j = 0; j < dim; j++)
                for (int k = 0; k < deep; k++)
                    array[i][j][k] = 0.5;

        return array;
    }

    // Type Фуллы

    public static T[] Full<T>(T num, int dim)
    {
        if (dim >= MaxNum)
            Write.Throw($"Very big vector. {dim}");

        T[] array = new T[dim];

        for (int i = 0; i < dim; i++)
            array[i] = num;
        
        return array;
    }

    public static T[][] Full<T>(T num, int size, int dim)
    {
        if (size >= MaxNum || dim >= MaxNum)
            Write.Throw($"Very big matrix. {size}, {dim}");

        T[][] array = Double<T>(size, dim);

        for (int i = 0; i < size; i++)
            for (int j = 0; j < dim; j++)
                array[i][j] = num;
        
        return array;
    }

    public static T[][][] Full<T>(T num, int size, int dim, int deep)
    {
        if (size >= MaxNum || dim >= MaxNum || deep >= MaxNum)
            Write.Throw($"Very big cube matrix. {size}, {dim}, {deep}");
        
        T[][][] array = Triple<T>(size, dim, deep);
        
        for (int i = 0; i < size; i++)
            for (int j = 0; j < dim; j++)
                for (int k = 0; k < deep; k++)
                    array[i][j][k] = num;
        
        return array;
    }


    // Фуллы

    public static double[] Full(double num, int dim)
    {
        if (dim >= MaxNum)
            Write.Throw($"Very big vector. {dim}");

        double[] array = new double[dim];

        for (int i = 0; i < dim; i++)
            array[i] = num;
        
        return array;
    }

    public static double[][] Full(double num, int size, int dim)
    {
        if (size >= MaxNum || dim >= MaxNum)
            Write.Throw($"Very big matrix. {size}, {dim}");

        double[][] array = Double<double>(size, dim);

        for (int i = 0; i < size; i++)
            for (int j = 0; j < dim; j++)
                array[i][j] = num;
        
        return array;
    }

    public static double[][][] Full(double num, int size, int dim, int deep)
    {
        if (size >= MaxNum || dim >= MaxNum || deep >= MaxNum)
            Write.Throw($"Very big cube matrix. {size}, {dim}, {deep}");
        
        double[][][] array = Triple<double>(size, dim, deep);
        
        for (int i = 0; i < size; i++)
            for (int j = 0; j < dim; j++)
                for (int k = 0; k < deep; k++)
                    array[i][j][k] = num;
        
        return array;
    }

    // Инициализация Ксавье (Xavier / Glorot) — для Softmax, Tanh, линейных слоев
    public static double[][] Xavier(int size, int dim)
    {
        if (size >= MaxNum || dim >= MaxNum)
            Write.Throw($"Very big matrix. {size}, {dim}");

        double[][] array = Double<double>(size, dim);
        
        // Формула: диапазон от -sqrt(6/(in+out)) до +sqrt(6/(in+out))
        double range = Math.Sqrt(6.0 / (size + dim));

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                // Берем ваше случайное число от 0 до 1, переводим в диапазон [-1; 1] и масштабируем
                double rawRandom = Rand.NextDouble();
                array[i][j] = (rawRandom * 2.0 - 1.0) * range;
            }
        }
        
        return array;
    }

    // Правильная инициализация He (Kaiming) — пригодится, если дальше используете ReLU
    public static double[][] HeCorrected(int size, int dim)
    {
        if (size >= MaxNum || dim >= MaxNum)
            Write.Exc($"Very big matrix. {size}, {dim}");

        double[][] array = Double<double>(size, dim);
        
        // Формула He: stdDev = sqrt(2 / размер_входа)
        // Для нормального распределения. Если делаем равномерное: range = sqrt(6 / размер_входа)
        double range = Math.Sqrt(6.0 / size);

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                double rawRandom = Rand.NextDouble();
                array[i][j] = (rawRandom * 2.0 - 1.0) * range;
            }
        }
        
        return array;
    }

    // Спины

    public static double[] Spin(int dim)
    {
        if (dim >= MaxNum) Write.Throw($"Very big vector. {dim}");
        
        double[] array = new double[dim];
        
        for (int i = 0; i < dim; i++)
            array[i] = Math.ExtraRandom(Rand.NextDouble()) * HE(dim);

        return array;
    }

    public static double[][] Spin(int size, int dim)
    {
        if (size >= MaxNum || dim >= MaxNum)
            Write.Throw($"Very big matrix. {size}, {dim}");
        
        double[][] array = Double<double>(size, dim);

        for (int i = 0; i < size; i++)
            for (int j = 0; j < dim; j++)
                array[i][j] = Math.ExtraRandom(Rand.NextDouble()) * HE(size * dim);
        
        return array;
    }

    public static double[][][] Spin(int size, int dim, int deep)
    {
        if (size >= MaxNum || dim >= MaxNum || deep >= MaxNum)
            Write.Throw($"Very big matrix. {size}, {dim}, {deep}");

        double[][][] array = Triple<double>(size, dim, deep);
        
        for (int i = 0; i < size; i++)
            for (int j = 0; j < dim; j++)
                for (int k = 0; k < deep; k++)
                    array[i][j][k] = Math.ExtraRandom(Rand.NextDouble()) * HE(size * dim * deep);
        
        return array;
    }

    public static double[][] SpinReg(double reg, int size, int dim)
    {
        if (size >= MaxNum || dim >= MaxNum)
            Write.Throw($"Very big matrix. {size}, {dim}");
        
        double[][] array = Double<double>(size, dim);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                array[i][j] = Math.ExtraRandom(Rand.NextDouble()) * HE(reg, size * dim);
            }
        }
        return array;
    }

    public static double[][] Spinec(params int[] dim)
    {
        double[][] array = Double<double>(dim.Length, dim[0]);
        for (int i = 0; i < dim.Length; i++)
        {
            if (dim[i] >= MaxNum)
                Write.Throw($"Very big vector: {dim}");

            double[] mas = Spin(dim[i]);

            for (int j = 0; j < dim[0]; j++)
                array[i][j] = mas[j];
        }

        return array;
    }
    
    public static double[][][] Spinec(params int[][] size)
    {
        double[][][] array = new double[size.Length][][];

        for (int i = 0; i < size.Length; i++)
        {
            array[i] = new double[size[i][0]][];

            for (int j = 0; j < size[i][0]; j++)
            {
                array[i][j] = new double[size[i][1]];

                if (size[i][0] >= MaxNum || size[i][1] >= MaxNum)
                    Write.Throw($"Very big vector: {size}");

                double[][] mas = Spin(size[i][0], size[i][1]);

                for (int k = 0; k < size[i][1]; k++)
                    array[i][j][k] = mas[j][k];
            }
        }

        return array;
    }

    public static double[][][] SpinecReg(int reg, params int[][] size)
    {
        double[][][] array = new double[size.Length][][];
        
        for (int i = 0; i < size.Length; i++)
        {
            array[i] = new double[size[i][0]][];

            for (int j = 0; j < size[i][0]; j++)
            {
                array[i][j] = new double[size[i][1]];

                if (size[i][0] >= MaxNum || size[i][1] >= MaxNum)
                    Write.Throw($"Very big vector: {size}");

                double[][] mas = SpinReg(reg, size[i][0], size[i][1]);

                for (int k = 0; k < size[i][1]; k++)
                    array[i][j][k] = mas[j][k];
            }
        }

        return array;
    }

    /*private static Array CreateJaggedArray(Type elementType, params int[] dims)
    {
        if (dims.Length == 0)
            Write.Throw("Количество измерений должно быть больше нуля.");
        
        var length = size;
        //Type arrayType = typeof(Array).MakeArrayType(elementType); // Создаем тип массива

        // Создаем первый уровень массива
        Array result = Array.CreateInstance(elementType, length);
    
        for (int i = 0; i < length; ++i)
        {
            // Если есть ещё размеры — создаем следующий уровень рекурсивно
            if (dims.Length > 1)
                result.SetValue(CreateJaggedArray(elementType, dims.Skip(1).ToArray()), i);
            else
                result.SetValue(Array.CreateInstance(elementType.GetType(), 0), i); // Пустой внутренний массив
        }

        return result;
    }*/

    /*public static T[][] array<T>(params int[] dims)
    {
        if (dims.Length != 2 || dims.Length == 0)
            Write.Throw("Метод поддерживает создание только двухмерных ступенчатых массивов");

        Array jaggedArray = CreateJaggedArray(typeof(T), dims);
        return (T[][])jaggedArray;
    }*/
}