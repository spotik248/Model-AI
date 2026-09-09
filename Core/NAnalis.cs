//using System;

using static Model.Write;
using static Model.Mist;
using static Model.Library;
using static Model.NManage;
using static Model.Learn;
using static Model.Global;

namespace Model;

static class NAnalis
{
    public static int[] ids = [];
    public static int[] idsQuest = [];
    public static int[] idsAnswer = [];

    public static (double[][]?, double[][]?, bool) Pre(string input) // dotnet build -c Release
    {
        MsgLine("Обработка слов...\n");

        string[] tokens = MyString.TextRegex(@"\w+|[^\w\s]", input).Split(' ');

        MsgLine($"Текст: {input}", $"\nText: {input}\n");
        //CheckIt($"\nСлово '{tokens[0]}' Условие исключения: {tokens.Length < 1}, {tokens[0] == ""}, {tokens[0] == " "} если всё False, то всё впорядке");

        //###  ИСКЛЮЧЕНИЕ  ###//
        if ((tokens.Length < 1 && (tokens[0] == "" || tokens[0] == " ")) || tokens.IsNull())
            return (null, null, false);
        else Debug("Исключение для PreNAnalis не произошло");
        //###  ИСКЛЮЧЕНИЕ  ###//

        MsgLine("Векторизация токенов...\n");

        AddWords(tokens);

        ids = TokensToId(tokens);

        double[][]? questVector;
        double[][]? answerVector;

        Mas($"Добавляем токены: ", tokens);

        int index = Array.IndexOf(tokens, "ответ");
        bool learn = index != -1;
        
        if(learn)
        {
            questVector = Quest(tokens, index);
            answerVector = Answer(tokens, index);
            
            idsQuest = Fill(ids, index);
            idsAnswer = Fill(ids, index, ids.Length);
        }
        else
        {
            questVector = Quest(tokens);
            answerVector = null;

            idsQuest = ids;
            idsAnswer = [];
        }

        if (answerVector == null) learn = false; // TODO: Исключение

        return (questVector, answerVector, learn);
    }

    public static void General(string input) // dotnet build -c Release
    {
        // ### Пре анализ ### //

        var (questVector, answerVector, learn) = Pre(input);
        
        if(questVector == null)
        {
            Err("Произошла ошибка во время обработки текста, для: "+input);
            return;
        }
        
        // ### Получение ответа ### //

        double[][]? answer = Learning(questVector, answerVector, learn);

        // ### Формирование слов из double[][] ### //

        if(answer == null || answer.Length == 0 || answer[0].Length == 0)
        {
            Err("Answer of Predict is null or lengthes are zeros");
            return;
        }

        //CheckIt("answer", answer);

        var (percentMean, indexWords, indexAnswer) = GetMeanNew(answer, 0.9); // Точность ответа в сотых (один процент)
        
        if (percentMean == null || indexWords == null || indexAnswer == null)
        {
            Err("Results of GetMean are null");
            return;
        }

        // ### Пост анализ ### //

        Post(percentMean, indexWords, indexAnswer, questVector);
    }

    public static double[][]? Learning(double[][] questVector, double[][]? answerVector, bool learn)
    {
        double[][]? answer = Init.Double<double>(size, dimension);

        if(learn && questVector != null && answerVector != null)
        {
            MsgLine("Обучение нейронной сети...");

            SetFull(1, 100, 1);

            double[][] Errors = new double[epoches][];

            fixedLearningRate = false;

            for (int epoch = 0; epoch < epoches; epoch++)
            {

                double[]? errors = Study(questVector, answerVector);
                if(errors == null) return null;


                MsgLine($"Скорость обучения: {learningRate}");
                MsgLine($"Общая ошибка нейросети: {errors[0]}");
                MsgLine($"Обучено на {100 * epoch / epoches}%");
                MsgLine($"Пройдено эпох: {epoch}/{epoches}");

                Errors[epoch] = errors;

                //MoveVectorWords(Matrix.Combinate(questVector, answerVector));
                UseUpdateLearningRate(3, errors, epoch);

                //if(epoch < epoches-1) Clear();
            }

            MsgLine($"Обучение окончено, прошло {epoches} эпох");
        }

        MsgLine("Получение ответа от нейронной сети...");
        if (questVector != null) answer = Predict(questVector);

        return answer;
    }

    public static (double[]?, int[]?, int[]?) GetMeanNew(double[][]? predictAnswers, double percent)
    {
        if (predictAnswers == null) return (null, null, null);
        
        double[][] filterAnswer = Math.Filter(predictAnswers, Init.Full(0.0, dimension), Init.Full(0.5, dimension), Init.Full(0.01, dimension));
        if (filterAnswer.Length == 0) return (null, null, null);

        var outPercent = new List<double>();
        var outWordIndexes = new List<int>();
        var outAnswerIndexes = new List<int>();

        // 2. Идем строго по порядку сгенерированных позиций (токенов) ответа
        for (int j = 0; j < filterAnswer.Length; j++)
        {
            double maxSimilarity = -1;
            int bestWordIndex = -1;

            // Ищем во всем словаре слово, вектор которого ближе всего к filterAnswer[j]
            for (int i = 0; i < words.Count(); i++)
            {
                double similarity = Math.MeanVector(words[i].vector, filterAnswer[j]); // возвращает 1 если абс. совпадение и 0 если точно нет
                
                if (similarity > maxSimilarity)
                {
                    maxSimilarity = similarity;
                    bestWordIndex = i;
                }
            }

            // 3. Отсекаем слишком слабые совпадения по вашему порогу процентов
            if (maxSimilarity >= percent && bestWordIndex != -1)
            {
                outPercent.Add(maxSimilarity);
                outWordIndexes.Add(bestWordIndex);
                outAnswerIndexes.Add(j); // Сохраняем исходный хронологический порядок!
            }
        }

        return (outPercent.ToArray(), outWordIndexes.ToArray(), outAnswerIndexes.ToArray());
    }

    public static (double[]?, int[]?, int[]?) GetMeanOld(double[][]? predictAnswers, double percent)
    {
        // После нахождения ответа и/или обучения:

        if (predictAnswers == null) return (null, null, null);
        double[][] filterAnswer = Math.Filter(predictAnswers, Init.Full(0.0, dimension), Init.Full(0.01, dimension), Init.Full(0.5, dimension));
        Msg("Answer Neuronetwork");

        double[][] meanPercent = Init.Double<double>(3, words.Count() * filterAnswer.Length); // Список средних процентных соотношений
        Msg("Percent same:");
        int meanLength = 0;

        // Поиск максимального процента сходства для каждого слова
        for (int i = 0; i < words.Count(); i++)
        {
            for (int j = 0; j < filterAnswer.Length; j++)
            {
                double meanWord = Math.MeanVector(words[i].vector, filterAnswer[j]);
                meanPercent[0][meanLength] = meanWord; // Среднее сходство
                meanPercent[1][meanLength] = i; // Индекс слова
                meanPercent[2][meanLength] = j; // Индекс ответа
                meanLength++;

                Msg($"{words[i].token} number {j} — {string.Join(" ", Math.Round(filterAnswer[j]))}. {Math.Round(meanWord * 100)}%");
            }
        }

        double veryMaxMean = 1 - Math.Max(meanPercent[0]);

        // Сбор наилучших соответствий
        double[][] meanPercentMax = Init.Double<double>(3, meanPercent[0].Length);
        for (int i = 0; i < meanPercent[0].Length; i++)
        {
            int maxIndex = Array.IndexOf(meanPercent[0], Math.Max(meanPercent[0]));

            meanPercentMax[0][i] = meanPercent[0][maxIndex];
            meanPercentMax[1][i] = meanPercent[1][maxIndex];
            meanPercentMax[2][i] = meanPercent[2][maxIndex];

            meanPercent[0][maxIndex] = 0;
        }

        CheckIt("MEAN PERCENT MAX", meanPercentMax[0]);

        int newLength = 0;
        for (int i = 0; i < meanPercentMax[0].Length; i++)
        {
            if (meanPercentMax[0][i] > 0.02 && meanPercentMax[0][i] + veryMaxMean >= percent)
            {
                meanPercentMax[0][i] = meanPercentMax[0][i];
                newLength++;
            }
            else
            {
                meanPercentMax[0][i] = 0;
            }
            meanPercentMax[1][i] = meanPercentMax[1][i];
            meanPercentMax[2][i] = meanPercentMax[2][i];
        }

        //Новый размер
        int indexMax = 0;
        double[][] meanPercentFinal = Init.Double<double>(3, newLength); // dotnet build -c Release
        for (int i = 0; i < meanPercentMax[0].Length; i++)
        {
            if (meanPercentMax[0][i] > 0)
            {
                meanPercentFinal[0][indexMax] = meanPercentMax[0][i];
                meanPercentFinal[1][indexMax] = meanPercentMax[1][i];
                meanPercentFinal[2][indexMax] = meanPercentMax[2][i];
                indexMax++;
            }
        }

        // Сбор наилучших соответствий
        double[][] meanSortedFinal = Init.Double<double>(3, meanPercentFinal[2].Length);
        for (int i = 0; i < meanPercentFinal[2].Length; i++)
        {
            int maxIndex = Array.IndexOf(meanPercentFinal[2], Math.Min(meanPercentFinal[2]));

            meanSortedFinal[0][i] = meanPercentFinal[0][maxIndex];
            meanSortedFinal[1][i] = meanPercentFinal[1][maxIndex];
            meanSortedFinal[2][i] = meanPercentFinal[2][maxIndex];

            meanPercentFinal[2][maxIndex] = double.PositiveInfinity;
        }

        return (meanSortedFinal[0], Math.RoundInt(meanSortedFinal[1]), Math.RoundInt(meanSortedFinal[2])); //Round нужен для перевода double в int
    }

    public static string[] Post(double[] percentMean, int[] indexWords, int[] indexAnswer, double[][] questVector)
    {
        string[] finallyAnswer = new string[indexWords.Length];

        for (int i = 0; i < indexWords.Length; i++)
            finallyAnswer[i] = words[indexWords[i]].token;
        

        Line($"Ответ: {string.Join(" ", finallyAnswer)}");


        double[][]? predict = Predict(questVector);
        if(predict == null) return finallyAnswer;
        double[][] predictSeparate = Math.Filter(predict, Init.Full(0.0, dimension), Init.Full(0.5, dimension));


        Msg("Try liken it with this:");
            
        Msg("Vectors words:");
        for (int i = 0; i < words.Count(); i++)
            Msg($"    {words[i].token}: {string.Join(" ", Math.Round(words[i].vector))}");
        Msg($"Number words from Library: {words.Count()}/{words.Length}");


        Msg("\nPersent same:");
        for (int i = 0; i < percentMean.Length; i++)
            Msg($"    {words[indexWords[i]].token} к {indexAnswer[i]} — {string.Join(" ", Math.Round(predictSeparate[indexAnswer[i]]))} {Math.Round(percentMean[i]) * 100}%");
        
        if (percentMean.Length <= 0)
            Err($"CHECK IT! MEAN PERCENT VERY SMALL. mp={percentMean.Length}");


        Msg("\nTry liken it with this short version");
        Msg($"    {string.Join(' ', Math.Round(Math.Flat(predictSeparate)))}");
        // Логи


        return finallyAnswer;
    }

    public static double[][]? Predict(double[][] quest) => Learn.Predict(quest);
    public static double[]? Study(double[][] input, double[][] output) => Learn.Study(input, output);

}