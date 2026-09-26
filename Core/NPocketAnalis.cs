//using System;

using static Model.Write;
using static Model.Library;
using static Model.NManage;
using static Model.Learn;
using static Model.Global;

namespace Model;

static class NeuroPocketAnalis
{
    public static double[] errors = [];
    
    public static string[] ReadLearnFolder()
    {
        // Чтение файлов в папке @"learnPocket"

        string learnFolder = Util.RootCombine("learnPocket"); // TODO: Переделать на folders

        string[] learnFiles = Directory.GetFiles(learnFolder, "*");

        MsgLine("Файлы для чтения: ");
        Full(learnFiles);

        string[][] learningStroke = learnFiles.Select(Util.ReadLines).ToArray();
        var reLearningStroke = new List<string>();

        MsgLine("Строки прочитанных файлов: ");

        for (int i = 0; i < learnFiles.Length; i++) // Это файлы
        {
            for (int j = 0; j < learningStroke[i].Length; j++) // Это абзац со словами 
            {
                learningStroke[i][j] = learningStroke[i][j].Trim().ToLower() ?? "";

                if (!learningStroke[i][j].IsNull() && learningStroke[i][j] != " ")
                {
                    if (learningStroke[i][j][0] == '/' && learningStroke[i][j][1] == '/')
                        learningStroke[i][j] = "";
                }
                else learningStroke[i][j] = "";

                CheckIt("learningStroke[i][j]", learningStroke[i][j]);

                if(learningStroke[i][j] != "") reLearningStroke.Add(learningStroke[i][j]);
            }
        }

        return reLearningStroke.ToArray();
    }

    public static (double[][][]?, double[][][]?, bool[]) Pre(string[] learningStroke)
    {
        var questList = new List<double[][]>();
        var answerList = new List<double[][]>();
        var learnList = new List<bool>();

        for (int i = 0; i < learningStroke.Length; i++)
        {
            if (!learningStroke[i].IsNull() && learningStroke[i] != " ")
            {
                CheckIt("NAnalis.Pre learningStroke[i][j]", learningStroke[i]);

                // "Привет мир!" => [ "Привет", "мир", "!" ] => [ [0.1, 0.2], [0.3, 0.4], [0.5, 0.6] ] значит string => double[][]
                var (questVector, answerVector, learn) = NAnalis.Pre(learningStroke[i]);

                if (questVector == null)
                    Err($"Ошибка при обработке вопрос-ответ. Возвращено: {questVector}, {answerVector}, {learn}");
                    
                questList.Add(questVector); // Добавляем в один пакет из всех файлов и строк
                answerList.Add(answerVector);
                learnList.Add(learn);
            }
        }

        var questVectors = questList.ToArray();
        var answerVectors = answerList.ToArray();
        var learnVectors = learnList.ToArray();

        return (questVectors, answerVectors, learnVectors);
    }

    public static void General()
    {
        string[] learningStroke = ReadLearnFolder();

        //learningStroke.Length - файл
        //learningStroke[0].Length - столбец
        //learningStroke[0][0].Length - текст

        var (questVectors, answerVectors, learnVectors) = Pre(learningStroke);

        // Исключения

        if(questVectors == null)
        {
            Err("Произошла ошибка для всего пакета: "+questVectors);
            return;
        }

        if(answerVectors == null)
        {
            Err("Произошла ошибка для ответа на вопрос: "+answerVectors);
            answerVectors = [];
        }

        for(int i = 0; i < learningStroke.Length; i++)
        {
            CheckIt("learningStroke[i]", learningStroke[i]);
            CheckIt("questVectors[i]", questVectors[i]);
            
            if(questVectors[i] == null)
            {
                Err("Произошла ошибка во время обработки текста, для массива вектора строки: "+learningStroke[i]);
                return;
            }
        }

        CheckIt("questVectors", questVectors);

        CheckIt("questVectors.Length", questVectors.Length);
        CheckIt("questVectors[0].Length", questVectors[0].Length);
        CheckIt("questVectors[0][0].Length", questVectors[0][0].Length);
        
        
        PocketLearn(questVectors, answerVectors, learnVectors);

        // Получение ответа

        for (int pocket = 0; pocket < questVectors.Length; pocket++)
        {
            MsgLine($"\nВопрос для пакета {pocket}: {string.Join(" ", questVectors[pocket].Select(Vector.AddAll))}");

            MsgLine($"Попытка получения ответа для пакета {pocket} от нейронной сети...");

            if(questVectors[pocket] != null)
            {
                double[][]? answer = Predict(questVectors[pocket]);

                if(answer != null && answer.Length != 0 && answer[0].Length != 0)
                {
                    CheckIt("answer", answer);

                    MsgLine($"Получение ответа для пакета {pocket} от нейронной сети...\n");

                    //CheckIt("answer", answer);

                    var (percentMean, indexWords, indexAnswer) = NAnalis.GetMean(answer, 0.85);

                    if (percentMean != null && indexWords != null && indexAnswer != null)
                    {
                        NAnalis.Post(percentMean, indexWords, indexAnswer, questVectors[pocket]);
                    }
                    else MsgLine($"Нейросеть не может дать точного ответа для пакета {pocket}.");
                }
                else MsgLine($"Ответ нейронной сети для пакета {pocket} оказался пуст.");
            }
            else MsgLine($"Вопрос для пакета {pocket} оказался нулевым, нейросеть не может дать ответа.");
            
        }
    }

    public static void PocketLearn(double[][][] questVectors, double[][][] answerVectors, bool[] learnVectors)
    {
        MsgLine("Обучение нейронной сети...");

        SetFull(0.9, 500, questVectors.Length);
        
        for (int epoch = 0; epoch < epoches; epoch++)
        {
            CheckIt(nn[id].shortName, nn[id].onlyPocket);
            if(nn[id].onlyPocket)
            {
                try
                {
                    //if(learnVectors)
                    double[][][] Errors = new double[epoches][][];

                    // а есть смысл в learnVectors?
                    
                    double[][]? errors = Study(questVectors, answerVectors);

                    if(errors == null) {
                        Exc($"Ошибка при обучении стала null: {errors}.");
                        return;
                    }

                    MsgLine($"Скорость обучения: {lr}");
                    MsgLine($"Общая ошибка нейросети: {errors[0]}");
                    MsgLine($"Обучено на {100 * epoch / epoches}%");
                    MsgLine($"Пройдено эпох: {epoch}/{epoches}");

                    Errors[epoch] = errors;

                    //MoveVectorWords(Matrix.Combinate(questVector, answerVector));
                    UseUpdateLearningRate(3, Errors[epoch][0], epoch);
                }
                catch (Exception ex)
                {
                    MsgLine($"Ошибка при пакетном обучении: {ex} ");
                }

                Line($"Эпоха {epoch + 1}/{epoches}, полный пакет");
            }
            else
            for (int pocket = 0; pocket < questVectors.Length; pocket++)
            {

                //double[][] questVector = questVectors[pocket];
                //double[][] answerVector = answerVectors[pocket];


                double[][] Errors = new double[epoches][];


                if (learnVectors[pocket])
                    try
                    {
                        double[]? errors = Study(questVectors[pocket], answerVectors[pocket]);

                        if(errors == null) {
                            Exc($"Ошибка при обучении стала null: {errors}.");
                            return;
                        }

                        MsgLine($"Скорость обучения: {lr}");
                        MsgLine($"Общая ошибка нейросети: {errors[0]}");
                        MsgLine($"Обучено на {100 * epoch / epoches}%");
                        MsgLine($"Пройдено эпох: {epoch}/{epoches}");

                        Errors[epoch] = errors;

                        //MoveVectorWords(Matrix.Combinate(questVector, answerVector));
                        UseUpdateLearningRate(3, errors, epoch);
                    }
                    catch (Exception ex)
                    {
                        MsgLine($"Ошибка при пакетном обучении: {ex} ");
                    }

                else Debug("На этом невозможно обучится!");
                
                Line($"Эпоха {epoch + 1}/{epoches}, Пакет {pocket + 1}");
            }
        }

        Line($"Обучение на {epoches} эпох и {questVectors.Length} пакетов закончена");
    }

    public static double[][]? Predict(double[][] input) => Learn.Predict(input);
    public static double[]? Study(double[][] input, double[][] output) => Learn.Study(input, output);

    public static double[][][]? Predict(double[][][] input) => Learn.Predict(input);
    public static double[][]? Study(double[][][] input, double[][][] output) => Learn.Study(input, output);

}