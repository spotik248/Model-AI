//using System;

using static Model.Write;
using static Model.Library;
using static Model.NN;
using static Model.Global;

namespace Model;

class Attention : INeuroComponent // Attention // Внимание
{

    //Из конструктора

    public string name { get; set; } = "Attention";
    public string shortName { get; set; } = "att";
    public ushort count { get; set; } = 4; // ushort = 2 байта, от 0 до 65535
    public int Size() => sizeAtt * count;

    public int sizeAtt;

#pragma warning disable CS0649
    
    //Внимание

    public double[][] attentionf; public double[][] attention; public double[][] softAttention; public double[][] actAttention;
    public double[][] dAttention; public double[][] dSoftAttention; public double[][] dActAttention;
    
    //Веса

    public double[][] widthQuery; public double[][] widthKey; public double[][] widthValue; public double[][] widthValuef;
    public double[][] dWidthQuery; public double[][] dWidthKey; public double[][] dWidthValue; public double[][] dWidthValuef;

    //Запросы, ключи и значения

    public double[][] query; public double[][] key; public double[][] value; public double[][] valuef;
    public double[][] dQuery; public double[][] dKey; public double[][] dValue; public double[][] dValuef;

#pragma warning disable CS8618

    public Attention(int size)
    {
        sizeAtt = size;
        
        widthQuery  ??= Init.Xavier(dimension, dimension); 
        widthKey    ??= Init.Xavier(dimension, dimension);
        widthValue  ??= Init.Xavier(dimension, dimension);
        widthValuef ??= Init.Xavier(dimension, dimension);

        dQuery  ??= Init.Zeros(sizeAtt, sizeAtt);
        dKey    ??= Init.Zeros(sizeAtt, sizeAtt);
        dValue  ??= Init.Zeros(sizeAtt, sizeAtt);
        dValuef ??= Init.Zeros(sizeAtt, sizeAtt);

        query ??= Init.Zeros(sizeAtt, dimension);
        key   ??= Init.Zeros(sizeAtt, dimension);
        value ??= Init.Zeros(sizeAtt, dimension);
        
        //Msg($"Инициализация {name} закончена...");
        Msg($"Initialization {name} complete");

        if (widthQuery == null || widthKey == null || widthValue == null || widthValuef == null)
            Exc($"Некоторые из параметров {name} остались незаполненными!");
    }
    
#pragma warning restore CS8618
#pragma warning restore CS0649

    public double[][] Pass(double[][] input)
    {
        // input имеет размер [sizeFNN x dimension]
        int rows = input.Length;       // Это size
        int cols = input[0].Length;    // Это dimension

        // 1. Получаем Q, K, V нужных размеров
        query = Matrix.MultMat(input, widthQuery); // [size x dim]
        key = Matrix.MultMat(input, widthKey);     // [size x dim]
        value = Matrix.MultMat(input, widthValue);   // [size x dim]

        //CheckIt("query", query);
        //CheckIt("key", key);
        //CheckIt("value", value);

        // 2. Вычисляем скалярное произведение Запросов и Ключей (Q * K^T)
        // Результат будет иметь размер [size x size] — это карта связей между строками
        double[][] scores = Matrix.MultMat(query, Matrix.Transpose(key));

        // 3. Масштабирование (Scaled Dot-Product)
        // Делим на корень из размерности признаков, чтобы предотвратить взрыв экспоненты в Softmax
        double scale = Math.Sqrt(cols);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                scores[i][j] /= scale;
            }
        }

        // 4. Применяем Softmax (или вашу функцию Act) ПОСТРОЧНО
        // Важно: функция активации должна нормализовать каждую СТРОКУ матрицы scores отдельно!
        CheckIt("scores", scores);
        actAttention = Act(scores); // [size x size]
        CheckIt("actAttention", actAttention);

        // 5. Умножаем веса внимания на значения (Scores * V)
        // [size x size] * [size x dim] = [size x dim]
        attention = Matrix.MultMat(actAttention, value);

        // Дополнительный слой трансформации (как у вас widthValuef), если он необходим:
        // Чтобы сохранить размеры [size x dim], widthValuef должен быть размером [dim x dim]
        attentionf = Matrix.MultMat(attention, widthValuef); // [size x dim]

        return attentionf; // Возвращает чистый [size x dimension] без NaN!
    }

    /*
    Умножаем матрицу на транспонированный вектор входа и получаем ответ,
    у которого размеры такая же, как и у веса.
              [a b]   [i]   [l m]
    W * I^t = [c d] x [j] = [n o]
              [e f]   [k]   [p r]
    */

    public double[][] PassMask(double[][] input)
    {
        Mas("input: ", input); Mas("size: ", sizeAtt);
        query = Matrix.MultMat(input, widthQuery); // dim x dim * dim x dim = dim x dim
        key = Matrix.MultMat(input, widthKey);
        value = Matrix.MultMat(input, widthValue);
        valuef = Matrix.MultMat(input, widthValuef);

        softAttention = Matrix.DivOne(Matrix.MultMat(Matrix.Transpose(query), key), key.Length); // dim x dim * dim x dim = dim x dim
        double[][] mask = Matrix.Triangle(double.NegativeInfinity, 0, softAttention.Length, softAttention[0].Length); // dim x dim
        softAttention = Matrix.Add(softAttention, mask); // dim x dim
        actAttention = Act(softAttention);
        attention = Matrix.MultMat(actAttention, Matrix.Transpose(value)); // dim x dim * dim x dim = dim x dim
        attentionf = Matrix.MultMat(attention, valuef); // dim x dim * dim x dim = dim x dim
        return attentionf; // dim x dim
    }

    public double[][] PassCross(double[][] input, double[][] hidden)
    {
        Mas("input: ", input); Mas("size: ", sizeAtt);
        query = Matrix.MultMat(hidden, widthQuery); // dim x dim * dim x dim = dim x dim
        key = Matrix.MultMat(input, widthKey); // dim x dim * dim x dim = dim x dim
        value = Matrix.MultMat(input, widthValue);
        valuef = Matrix.MultMat(input, widthValuef);

        softAttention = Matrix.DivOne(Matrix.MultMat(Matrix.Transpose(query), key), key.Length); // dim x dim * dim x dim = dim x dim
        double[][] mask = Matrix.Triangle(double.NegativeInfinity, 0, softAttention.Length, softAttention[0].Length); // dim x dim
        softAttention = Matrix.Add(softAttention, mask); // dim x dim
        actAttention = Act(softAttention);
        attention = Matrix.MultMat(actAttention, Matrix.Transpose(value)); // dim x dim * dim x dim = dim x dim
        attentionf = Matrix.MultMat(attention, valuef); // dim x dim * dim x dim = dim x dim
        return attentionf; // dim x dim
    }

    // public double[][] PassMaskSelf(double[][] hidden)
    // {
    //     query = Matrix.MultMat(hidden, widthQuery); // dim x dim * dim x dim = dim x dim
    //     key = Matrix.MultMat(hidden, widthKey);
    //     value = Matrix.MultMat(hidden, widthValue);
    //     valuef = Matrix.MultMat(hidden, widthValuef);

    //     softAttention = Matrix.DivOne(Matrix.MultMat(Matrix.Transpose(query), key), key.Length); // dim x dim * dim x dim = dim x dim
    //     double[][] mask = Matrix.Triangle(double.NegativeInfinity, 0, softAttention.Length, softAttention[0].Length); // dim x dim
    //     softAttention = Matrix.Add(softAttention, mask); // dim x dim
    //     actAttention = Act(softAttention);
    //     attention = Matrix.MultMat(actAttention, Matrix.Transpose(value)); // dim x dim * dim x dim = dim x dim
    //     attentionf = Matrix.MultMat(attention, valuef); // dim x dim * dim x dim = dim x dim
    //     return attentionf; // dim x dim
    // }

    public double[][] Update(double[][] beforeDelta, double[][] input)
    {
        // beforeDelta имеет размер [100 x 20] (градиент, пришедший сверху)
        // input имеет размер [100 x 20]
        int rows = input.Length;       // 100
        int cols = input[0].Length;    // 20
        double scale = Math.Sqrt(cols);

        double[][] inputTrans = Matrix.Transpose(input); // [20 x 100]

        // 1. Градиент по финальной трансформации widthValuef [20 x 20]
        // dWidthValuef = Transpose(attention) * beforeDelta => [20 x 100] * [100 x 20] = [20 x 20]
        double[][] dWidthValuef = Matrix.MultMat(Matrix.Transpose(attention), beforeDelta);
        
        // Градиент, прошедший сквозь widthValuef назад к слою attention
        // dAttention_layer = beforeDelta * Transpose(widthValuef) => [100 x 20] * [20 x 20] = [100 x 20]
        double[][] dAttentionLayer = Matrix.MultMat(beforeDelta, Matrix.Transpose(widthValuef));

        // 2. Градиенты по операциям Attention (Связка Scores и Value)
        // attention = actAttention * value => [100 x 100] * [100 x 20]
        // dValue = Transpose(actAttention) * dAttentionLayer => [100 x 100] * [100 x 20] = [100 x 20]
        double[][] dValue = Matrix.MultMat(Matrix.Transpose(actAttention), dAttentionLayer);
        
        // dActAttention = dAttentionLayer * Transpose(value) => [100 x 20] * [20 x 100] = [100 x 100]
        double[][] dActAttention = Matrix.MultMat(dAttentionLayer, Matrix.Transpose(value));

        // 3. Градиент через SoftMax (совмещаем шаг dSoftAttention)
        // Для каждой строки: dSoftMax = actAttention * (dActAttention - Sum(dActAttention * actAttention))
        double[][] dSoftAttention = Init.Double<double>(rows, rows); // [100 x 100]
        for (int i = 0; i < rows; i++)
        {
            double dotProduct = 0.0;
            for (int j = 0; j < rows; j++)
            {
                dotProduct += dActAttention[i][j] * actAttention[i][j];
            }
            for (int j = 0; j < rows; j++)
            {
                dSoftAttention[i][j] = actAttention[i][j] * (dActAttention[i][j] - dotProduct);
                // Учитываем масштабирование (scale) сразу здесь
                dSoftAttention[i][j] /= scale; 
            }
        }

        // 4. Градиенты по Query и Key (из scores = query * key^T)
        // dQuery = dSoftAttention * key => [100 x 100] * [100 x 20] = [100 x 20]
        double[][] dQuery = Matrix.MultMat(dSoftAttention, key);
        
        // dKey = Transpose(dSoftAttention) * query => [100 x 100] * [100 x 20] = [100 x 20]
        double[][] dKey = Matrix.MultMat(Matrix.Transpose(dSoftAttention), query);

        // 5. Градиенты для весовых матриц [20 x 20]
        // dW = Transpose(Input) * d(Q/K/V) => [20 x 100] * [100 x 20] = [20 x 20]
        double[][] dWidthQuery = Matrix.MultMat(inputTrans, dQuery);
        double[][] dWidthKey   = Matrix.MultMat(inputTrans, dKey);
        double[][] dWidthValue = Matrix.MultMat(inputTrans, dValue);

        // 6. Обновление весов (Градиентный спуск)
        widthQuery  = Width(widthQuery, dWidthQuery);
        widthKey    = Width(widthKey, dWidthKey);
        widthValue  = Width(widthValue, dWidthValue);
        widthValuef = Width(widthValuef, dWidthValuef);

        // 7. Финальный расчет дельты для предыдущего слоя (dX)
        // dX = dQuery * Wq^T + dKey * Wk^T + dValue * Wv^T
        // Каждый член: [100 x 20] * [20 x 20] = [100 x 20]
        double[][] delta = Matrix.SumFew(
            Matrix.MultMat(dQuery, Matrix.Transpose(widthQuery)),
            Matrix.MultMat(dKey, Matrix.Transpose(widthKey)),
            Matrix.MultMat(dValue, Matrix.Transpose(widthValue))
        );

        return delta; // Возвращает идеальный [100 x 20] для предыдущего слоя!
    }

    private static double[][] Width(double[][] oldthisWidth, double[][] grad)
    {
        for (int i = 0; i < oldthisWidth.Length; i++)
            for (int j = 0; j < oldthisWidth.Length; j++)
                oldthisWidth[i][j] = oldthisWidth[i][j] - learningRate * grad[i][j];

        return oldthisWidth;
    }

    private static double[][] Act(double[][] matrix) => FucAct.SoftMax(matrix);

    private static double[][] DAct(double[][] matrix) => FucAct.DSoftMax(matrix);

    /*
        Sigmoid,  DSigmoid,
        SoftSign, DSoftSign
        ReLu, DReLu,
        LReLu, DLReLu,
    */

    /*private void CheckWidth()
    {
        foreach (var width in new double[][][][] { widthQuery, widthKey, widthValue })
        {
            for (int i = 0; i < width.Length; i++)
            {
                for (int j = 0; j < width[0].Length; j++)
                {
                    for (int k = 0; k < width[0][0].Length; k++)
                    {
                        Msg(width[i][j][k]);
                    }
                }
            }
        }
    }

    private void CheckBias()
    {
        foreach (var bias in new double[][][] { biasQuery, biasKey, biasValue })
        {
            for (int i = 0; i < bias.Length; i++)
            {
                for (int j = 0; j < bias[0].Length; j++)
                {
                    Msg(bias[i][j]);
                }
            }
        }
    }
    */
    /*public Dictionary<string, object> ToKeyValue()
    {
        return new Dictionary<string, object>() {
                {"sizeQuery", sizeQuery},
                {"sizeKey", sizeKey},
                {"sizeValue", sizeValue},
                {"widthQuery", widthQuery},
                {"widthKey", widthKey},
                {"widthValue", widthValue},
                {"biasQuery", biasQuery},
                {"biasValue", biasValue},
                {"biasKey", biasKey},
                {"learningRate", learningRate},
                {"dimension", dimension},
                {"", }
            };
    }

    public void ToStandart(Dictionary<string, object> keyValue)
    {
        sizeQuery = (int)keyValue["sizeQuery"];
        sizeKey = (int)keyValue["sizeKey"];
        sizeValue = (int)keyValue["sizeValue"];
        widthQuery = (double[][])keyValue["widthQuery"];
        widthKey = (double[][])keyValue["widthKey"];
        widthValue = (double[][])keyValue["widthValue"];
        biasQuery = (double[])keyValue["biasQuery"];
        biasKey = (double[])keyValue["biasKey"];
        biasValue = (double[])keyValue["biasValue"];
        learningRate = (double)keyValue["learningRate"];
        dimension = (int)keyValue["dimension"];
         = (bool)keyValue[""];
    }*/
}