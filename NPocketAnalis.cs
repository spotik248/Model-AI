//using System;

using static Model.Write;
using static Model.Library;
using static Model.Global;

namespace Model;

static class NeuroPocketAnalis
{
    public static double[] errors = [];
    
    public static string[][] ReadLearnFolder()
    {
        // Чтение файлов в папке @"learnPocket"

        string learnFolder = Util.RootCombine("learnPocket"); // TODO: Переделать на folders

        string[] learnFiles = Directory.GetFiles(learnFolder, "*");

        MsgLine("Файлы для чтения: "+ learnFiles);

        string[][] learningStroke = learnFiles.Select(Util.ReadLines).ToArray();

        MsgLine("Строки прочитанных файлов", learningStroke);
        
        int ls = 0;

        for (int i = 0; i < learnFiles.Length; i++)
        {
            for (int j = 0; j < learningStroke[i].Length; j++)
            {
                learningStroke[i][j] = learningStroke[i][j].Trim().ToLower() ?? "";

                if (!learningStroke[i][j].IsNull() && learningStroke[i][j] != " ")
                {
                    if (learningStroke[i][j][0] == '/' && learningStroke[i][j][1] == '/')
                        learningStroke[i][j] = "";
                    ls++;
                }
                else learningStroke[i][j] = "";

                CheckIt("learningStroke[i][j]", learningStroke[i][j]);
            }
        }
        return learningStroke;
    }

    public static (double[][][]?, double[][][]?, bool[]) Pre(string[][] learningStroke)
    {
        List<double[][]> questList = new List<double[][]>();
        List<double[][]> answerList = new List<double[][]>();
        List<bool> learnList = new List<bool>();

        for (int i = 0; i < learningStroke.Length; i++)
        {
            for (int j = 0; j < learningStroke[i].Length; j++)
            {
                if (!learningStroke[i][j].IsNull() && learningStroke[i][j] != " ")
                {
                    CheckIt("NAnalis.Pre learningStroke[i][j]", learningStroke[i][j]);

                    // "Привет мир!" => [ "Привет", "мир", "!" ] => [ [0.1, 0.2], [0.3, 0.4], [0.5, 0.6] ] ТОЕСТЬ string => double[][]
                    var (questVector, answerVector, learn) = NAnalis.Pre(learningStroke[i][j]);

                    if (questVector == null)
                        Err($"Ошибка при обработке вопрос-ответ. Возвращено: {questVector}, {answerVector}, {learn}");
                    
                    questList.Add(questVector); // Добавляем в один пакет из всех файлов и строк
                    answerList.Add(answerVector);
                    learnList.Add(learn);
                }
            }
        }

        var questVectors = questList.ToArray();
        var answerVectors = answerList.ToArray();
        var learnVectors = learnList.ToArray();

        return (questVectors, answerVectors, learnVectors);
    }

    public static void General()
    {
        string[][] learningStroke = ReadLearnFolder();

        //learningStroke.Length - файл
        //learningStroke[0].Length - таблица
        //learningStroke[0][0].Length - строка

        var (questVectors, answerVectors, learnVectors) = Pre(learningStroke);

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

        // // ### Получение ответа ### //

        // double[][]? answer = Learn(questVector, answerVector, learn);

        // // ### Формирование слов из double[][] ### //

        // if(answer == null || answer.Length == 0 || answer[0].Length == 0)
        // {
        //     Err("Answer of Predict is null or lengthes are zeros");
        //     return;
        // }

        // CheckIt("answer", answer);

        // var (percentMean, indexWords, indexAnswer) = GetMeanNew(answer, 0.9); // Точность ответа в сотых (один процент)

        for (int pocket = 0; pocket < questVectors.Length; pocket++)
        {
            MsgLine($"\nВопрос для пакета {pocket}: {string.Join(" ", questVectors[pocket].Select(Vector.AddAll))}");

            MsgLine($"Попытка получения ответа для пакета {pocket} от нейронной сети...");

            if(questVectors[pocket] != null)
            {
                double[][]? answer = Predict(questVectors[pocket]);

                if(answer != null && answer.Length != 0 && answer[0].Length != 0)
                {
                    MsgLine($"Получение ответа для пакета {pocket} от нейронной сети...\n");

                    CheckIt("answer", answer);

                    var (percentMean, indexWords, indexAnswer) = NAnalis.GetMeanNew(answer, 0.90);

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

        NN.SetLearnEpoch(1, 1000);
        NN.SetBatch(questVectors.Length);
        

        for (int epoch = 0; epoch < epoches; epoch++)
        {
            for (int pocket = 0; pocket < questVectors.Length; pocket++)
            {

                double[][] questVector = questVectors[pocket];
                double[][] answerVector = answerVectors[pocket];


                double[][] Errors = new double[epoches][];


                if (learnVectors[pocket])
                    try
                    {
                        double[]? errors = Study(questVectors[pocket], answerVectors[pocket]);

                        if(errors == null) {
                            Exc($"Ошибка при обучении стала null: {errors}.");
                            return;
                        }

                        MsgLine($"Скорость обучения: {learningRate}");
                        MsgLine($"Общая ошибка нейросети: {errors[0]}");
                        MsgLine($"Обучено на {100 * epoch / epoches}%");
                        MsgLine($"Пройдено эпох: {epoch}/{epoches}");

                        Errors[epoch] = errors;

                        //MoveVectorWords(Matrix.Combinate(questVector, answerVector));
                        NN.UseUpdateLearningRate(3, errors, epoch);
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

    public static void SetBatch(int batches) => NN.SetBatch(batches);
    public static double[][]? Predict(double[][] input) => NN.Predict(input);
    public static double[]? Study(double[][] input, double[][] output) => NN.Study(input, output);

}