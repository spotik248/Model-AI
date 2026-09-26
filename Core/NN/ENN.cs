//using System;

using static Model.Write;
using static Model.Library;
using static Model.NManage;
using static Model.Learn;
using static Model.Global;

namespace Model;

class ENN : INeuro // Emotional NeuroNetwork
{

    public string name { get; set; } = "Emotional NeuroNetwork";
    public string shortName { get; set; } = "enn";
    public string desc { get; set; } = "Emotional NeuroNetwork";
    public bool onlyPocket { get; set; } = true;
    public ushort countLayout { get; set; } = 2; // ushort = 2 байта, от 0 до 65535
    public int sizeNN { get; set; }
    public int Size() => sizeNN * countLayout;
    public int batches { get; set; } = 1;



#pragma warning disable CS0649


    public LP hiddenE1;
    public LP hiddenE2;

    public LP hiddenM1;
    public LP hiddenM2;

    public LP hiddenA1;
    public LP hiddenA2;

    //public STM shortTermMemory;
    double[][][] shortTermMemory;

    double[] view;

    double[] future; // Будущее

    double[] emotion; // Эмоция, разница предсказания и действительности
            
    double[][] data; // Вся информация (сенсоры + эмоции)

    static int maxLogWriteMemory = 100; // Для записи в дл.ср.память
    double[][][] logWriteMemory;

    // Долгосрочная память, сжимает по признакам
    double[] longTermMemory;

    // Процедурная память, используется при автоматизме
    double[] autoMemory;


#pragma warning disable CS8618


    public ENN(int size) // TODO: Добавить hiddenSize для внутреннего пространства нейронки
    {
        if(size != 0) sizeNN = size;
        Msg("Инициирован размер sizeNN: "+ sizeNN); // 10
        
        int sizedim = sizeNN * dimension; // 10*10 = x100
        int maxDatas = 4;
        int size2 = sizedim * maxDatas; // x400

        // Размер данных
        int dataSize = sizedim + sizedim + size2 + size2; // x1000

        hiddenE1 ??= new(dataSize, size2);
        hiddenE2 ??= new(size2, size2);

        hiddenM1 ??= new(dataSize, size2);
        hiddenM2 ??= new(size2, sizedim);

        hiddenA1 ??= new(dataSize, size2);
        hiddenA2 ??= new(size2, sizedim);

        //shortTermMemory ??= new(sizedim, size2);
        shortTermMemory ??= Init.Zeros(1, maxDatas, sizedim);
        shortTermMemory[0] = [ Init.Zeros(sizedim), Init.Zeros(sizedim), Init.Zeros(size2), Init.Zeros(size2) ];

        view ??= Init.Full(0.5, sizedim); // x100

        // Будущее
        future ??= Init.Zeros(size2); // x100*4

        // Эмоция, разница предсказания и действительности
        emotion ??= Init.Full(0.5, size2); // x100*4

        // Вся информация (сенсоры + эмоции)
        data ??= Init.Zeros(4, sizedim); // 4x100

        double[][][] logWriteMemory = Init.Zeros(maxLogWriteMemory, 4, sizedim);
        for(int i = 0; i < maxLogWriteMemory; i++) // заполнитель
            logWriteMemory[i] = [ Init.Zeros(sizedim), Init.Zeros(sizedim), Init.Zeros(size2), Init.Zeros(size2) ];

        // Долгосрочная память, сжимает по признакам
        double[] longTermMemory = Init.Zeros(size2); // x400

        // Процедурная память, используется при автоматизме
        double[] autoMemory = Init.Zeros(size2); // x400


        Msg($"Инициализация {name} закончена...");

        if (hiddenE1 == null || hiddenE2 == null || hiddenM1 == null || hiddenM2 == null || hiddenA1 == null || hiddenA2 == null)
            Exc($"Некоторые из данных {name} остались незаполненными!");
    }


#pragma warning restore CS8618
#pragma warning restore CS0649


    public double[][] Predict(double[][] input) // dotnet build -c Release
    {
        // if(input.Length * input[0].Length > hidden1.inputSize * dimension)
        //     Exc("Общий размер input гораздо больше чем фиксированный общий размер size.");

        // if(input.Length > hidden1.inputSize)
        //     Exc("Размер столбца input гораздо больше чем фиксированный размер столбца size.");

        // if(input[0].Length != dimension)
        //     Exc("Измерения input и dimension не соответствуют.");


        double[] inputLayout = Math.Flat(
            Matrix.ToSizeHalf(input, sizeNN, dimension)
        );
        Mas("inputLayout", inputLayout);

        data = [inputLayout, view, emotion, future];

        // Загрузка из ShortTermMemory, подмешивание в данные
        for(int i = 0; i < data.Length; i++)
            for(int j = 0; j < data[i].Length; j++)
                data[i][j] += shortTermMemory[0][i][j] * 0.3;


        double[] inputEmoteLayout = Matrix.ToVector(data); // x100*4
        Mas("inputEmoteLayout", inputEmoteLayout);

        double[] hiddenEmoteLayout = hiddenE1.Pass(inputEmoteLayout);
        Mas("hiddenEmoteLayout", hiddenEmoteLayout);

        double[] outputEmoteLayout = hiddenE2.Pass(hiddenEmoteLayout);
        Mas("outputEmoteLayout", outputEmoteLayout);

        // Поиск разницы = эмоция (ошибка предсказания)
        emotion = Vector.Sub(future, Matrix.ToVector(data));
        Mas("emotion", emotion);

        // Условие новизны
        double conditionNew = Math.Abs(Vector.AddAll(emotion));
        bool conditionNewBool = conditionNew > 0.2;

        // Сохранение в дневной лог, для последующего обучения
        // if(logWriteMemory.Count() != logWriteMemory.Length && conditionNewBool)
        //     logWriteMemory[logWriteMemory.Count()] = data.Select(FucAct.Sigmoid).ToArray(); // Сигмойда


        // Слоя предсказания
        future = outputEmoteLayout;
        // Сохраняем новую эмоцию и новое предсказание
        data = [inputLayout, view, emotion, future]; 
        // Сохранение данных в кр.ср.память
        shortTermMemory[0] = data.Select(FucAct.Sigmoid).ToArray(); // Сигмойда

        double[] answer = [];

        // Условие новизны

        Msg("Условие новизны: "+conditionNew);
        if(conditionNewBool)
        {
            // Этап мышления
                    
            double[] inputThinkLayout = Matrix.ToVector(data);
            Mas("inputThinkLayout", inputThinkLayout);

            double[] hiddenThinkLayout = hiddenM1.Pass(inputThinkLayout);
            Mas("hiddenThinkLayout", hiddenThinkLayout);

            double[] outputThinkLayout = hiddenM2.Pass(hiddenThinkLayout);
            Mas("outputThinkLayout", outputThinkLayout);

            answer = outputThinkLayout;
        }
        else
        {
            // Этап автоматизма
                    
            double[] inputAutoLayout = Matrix.ToVector(data);
            Mas("inputAutoLayout", inputAutoLayout);

            double[] hiddenAutoLayout = hiddenM1.Pass(inputAutoLayout);
            Mas("hiddenAutoLayout", hiddenAutoLayout);

            double[] outputAutoLayout = hiddenM2.Pass(hiddenAutoLayout);
            Mas("outputAutoLayout", outputAutoLayout);

            answer = outputAutoLayout;
        }

        if(logWriteMemory.Count() == logWriteMemory.Length)
        {
            CheckIt("Скип получения данных");
        }

        return Vector.ToMatrix(answer, dimension);
    }

    public double[][][] PredictPocket(double[][][] input) {CheckIt("Пакетный ответ не возможен, выбран метод единичного ответа"); return [];}

    public double[] Study(double[][] input, double[][] output) {CheckIt("Обучение не возможно, выбран метод пакетного обучения"); return [];}

    public double[][] StudyPocket(double[][][] input, double[][][] output)
    {
        // if(output.Length * output[0].Length > input.Length * dimension)
        //     Exc("Общий размер input гораздо больше чем фиксированный общий размер size.");

        // if(input.Length > hidden1.inputSize)
        //     Exc("Размер столбца input гораздо больше чем фиксированный размер столбца size.");

        // if(input[0].Length != dimension)
        //     Exc("Измерения input и dimension не соответствуют.");


        // TODO: Сделать проверку на исключения.

        logWriteMemory = Init.Zeros(maxLogWriteMemory, 4, sizeNN * dimension);

        double[][] outputE = Init.Zeros(input.Length, hiddenE2.outputSize);
        double[][] outputM = Init.Zeros(input.Length, hiddenM2.outputSize);
        double[][] outputA = Init.Zeros(input.Length, hiddenA2.outputSize);

        for(int p = 0; p < input.Length; p++)
        {
            double[] inputLayout = Math.Flat(
                Matrix.ToSizeHalf(input[p], sizeNN, dimension)
            );
            Mas("inputLayout", inputLayout);

            data = [inputLayout, view, emotion, future];

            Mas("data", data);

            // Загрузка из ShortTermMemory, подмешивание в данные
            for(int i = 0; i < data.Length; i++)
                for(int j = 0; j < data[i].Length; j++)
                    data[i][j] += FucAct.Sigmoid(shortTermMemory[0][i][j] * 0.3);

            Mas("data", data);


            double[] inputEmoteLayout = Matrix.ToVector(data); // x100*4
            Mas("inputEmoteLayout", inputEmoteLayout);

            double[] hiddenEmoteLayout = hiddenE1.Pass(inputEmoteLayout);
            Mas("hiddenEmoteLayout", hiddenEmoteLayout);

            double[] outputEmoteLayout = hiddenE2.Pass(hiddenEmoteLayout);
            Mas("outputEmoteLayout", outputEmoteLayout);

            // Поиск разницы = эмоция (ошибка предсказания)
            emotion = Vector.Sub(future, Matrix.ToVector(data));
            Mas("emotion", emotion);
            
            // Запись для обучение
            outputE[p] = future;

            // Условие новизны
            double conditionNew = Math.Abs(Vector.AddAll(emotion));
            bool conditionNewBool = conditionNew > 0.2;

            // Сохранение в дневной лог, для последующего обучения
            if(p != maxLogWriteMemory && conditionNewBool)
            {
                logWriteMemory[p] = data.Select(FucAct.Sigmoid).ToArray(); // Сигмойда
                //CheckIt($"Данные сохранены для обучения на пакете {p}", logWriteMemory[p]);
            }
            
            // Слоя предсказания
            future = outputEmoteLayout;
            
            // Сохраняем новую эмоцию и новое предсказание
            data = [inputLayout, view, emotion, future];

            Mas("data", data);

            // Сохранение данных в кр.ср.память
            shortTermMemory[0] = data.Select(FucAct.Sigmoid).ToArray(); // Сигмойда

            // Условие новизны

            Msg("Условие новизны: "+conditionNew);
            //if(conditionNewBool)
            //{
                // Этап мышления
                
                double[] inputThinkLayout = Matrix.ToVector(data);
                Mas("inputThinkLayout", inputThinkLayout);

                double[] hiddenThinkLayout = hiddenM1.Pass(inputThinkLayout);
                Mas("hiddenThinkLayout", hiddenThinkLayout);

                double[] outputThinkLayout = hiddenM2.Pass(hiddenThinkLayout);
                Mas("outputThinkLayout", outputThinkLayout);
                
                outputM[p] = outputThinkLayout;
            //}
            //else // если действие предсказано
            //{
                // Этап автоматизма
                
                double[] inputAutoLayout = Matrix.ToVector(data);
                Mas("inputAutoLayout", inputAutoLayout);

                double[] hiddenAutoLayout = hiddenA1.Pass(inputAutoLayout);
                Mas("hiddenAutoLayout", hiddenAutoLayout);

                double[] outputAutoLayout = hiddenA2.Pass(hiddenAutoLayout);
                Mas("outputAutoLayout", outputAutoLayout);

                outputA[p] = outputAutoLayout;
            //}

            if(p == maxLogWriteMemory)
            {
                CheckIt("Скип получения данных");
                break;
            }

            MsgLine($"Прямой проход пройден на: {p}/{input.Length} ({100 * p / input.Length}%)");
        }

        //errorE - ошибка эмоций
        //logWriteMemory - входные данные
        //outputM - выход мышления
        //outputA - выход автоматизма
        //targets - цели

        double[][] errorsE = Init.Double<double>(input.Length, 2); // логи
        double[][] errorsM = Init.Double<double>(input.Length, 2);
        double[][] errorsA = Init.Double<double>(input.Length, 2);

        for(int p = 0; p < input.Length; p++) // Обучение на всем дне
        {
            double[] target = Math.Flat(
                Matrix.ToSizeHalf(output[p], sizeNN, dimension)
            );
            Mas("target", target);

            double[][] data = logWriteMemory[p];
            Mas("data", data);


            // ### Поиск ошибки эмоции ###

            // Есть ли смысл в batches?
            double[] errorE2 = Vector.Sub(outputE[p], Matrix.ToVector(data)); //a - t
            Mas("errorE2", errorE2);



            double[] deltaE = Vector.Mult(errorE2, FucAct.DSigmoid(outputE[p]));
            Mas("deltaE", deltaE);
            Mas("outputE[p], DSigmoid", [outputE[p], FucAct.DSigmoid(outputE[p])]);

            double[] errorE1 = hiddenE1.Error(hiddenE2.width, deltaE);


            // обновление эмоций


            hiddenE1.Width(Matrix.ToVector(data), errorE1);
            hiddenE1.Bias(errorE1);

            hiddenE2.Width(hiddenE1.hiddenLayout, deltaE);
            hiddenE2.Bias(deltaE);

            Msg("### end emote ###");

            // ### Поиск ошибки мышления ###
            
            // Есть ли смысл в batches?
            double[] errorM2 = Vector.Sub(outputM[p], target); //a - t
            Mas("errorM2", errorM2);


            double[] deltaM = Vector.Mult(errorM2, FucAct.DSigmoid(outputM[p]));
            Mas("deltaM", deltaM);
            Mas("outputM[p], DSigmoid", [outputM[p], FucAct.DSigmoid(outputM[p])]);

            double[] errorM1 = hiddenM1.Error(hiddenM2.width, deltaM);


            // обновление мышления


            hiddenM1.Width(Matrix.ToVector(data), errorM1);
            hiddenM1.Bias(errorM1);

            hiddenM2.Width(hiddenM1.hiddenLayout, deltaM);
            hiddenM2.Bias(deltaM);

            Msg("### end think ###");

            // ### Поиск ошибки автоматизма ###

            // Есть ли смысл в batches?
            double[] errorA2 = Vector.Sub(outputA[p], target); //a - t
            Mas("errorA2", errorA2);


            double[] deltaA = Vector.Mult(errorA2, FucAct.DSigmoid(outputA[p]));
            Mas("deltaA", deltaA);
            Mas("outputA[p], DSigmoid", [outputA[p], FucAct.DSigmoid(outputA[p])]);

            double[] errorA1 = hiddenA1.Error(hiddenA2.width, deltaA);


            // обновление автоматизма


            hiddenA1.Width(Matrix.ToVector(data), errorA1);
            hiddenA1.Bias(errorA1);

            hiddenA2.Width(hiddenA1.hiddenLayout, deltaA);
            hiddenA2.Bias(deltaA);

            Msg("### end auto ###");
            
            errorsE[p] = [Vector.AddAll(errorE2), Vector.AddAll(errorE1)];
            errorsM[p] = [Vector.AddAll(errorM2), Vector.AddAll(errorM1)];
            errorsA[p] = [Vector.AddAll(errorA2), Vector.AddAll(errorA1)];

            MsgLine($"Скорость обучения: {lr}");
            MsgLine($"Общая ошибка нейросети: {errorsE[p][0]}(эмоция), {errorsM[p][0]}(мышление), {errorsA[p][0]}(автоматизм).");
            MsgLine($"Обучение пройдено на {p}/{input.Length} ({100 * p / input.Length}%)");
        }

        return [
            [Vector.AddAll(errorsE[0]), Vector.AddAll(errorsE[^1])],
            [Vector.AddAll(errorsM[0]), Vector.AddAll(errorsM[^1])],
            [Vector.AddAll(errorsA[0]), Vector.AddAll(errorsA[^1])]
        ];
    }

}