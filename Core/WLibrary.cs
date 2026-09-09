//using System;
using static Model.Write;
using static Model.NManage;
using static Model.Learn;
using static Model.Global;

namespace Model;

public static class Library
{
    public static Word[] words = [];
    public static int dimension;

    static Library() => Start(100, 10);

    public static void Start(int wordsLength, int dimension)
    {
        words = new Word[wordsLength];
        Library.dimension = dimension;
    }

    public static void AddWords(string[] input)
    {
        if(words.Count() == words.Length) // Если библиотека полностью заполнена
        {
            Err($"Library of words is fully, cannot to add more words. ({words.Count()}/{words.Length})");
            return;
        }

        var wordsList = new List<Word>();
        for (int i = 0, w = 0; i < input.Length; i++, w = 0)
        {
            input[i] = input[i].Trim();
            if (input[i].Length > 0)
            {
                foreach(var word in words)
                    if (word.token == input[i]) w++; // Если слова еще не существует

                if(w == 0) wordsList.Add(new Word(input[i], Init.Randomized(dimension))); // Добавление новых слов
                //input[i] = "";
            }
            else Debug("Слово для добавления слишком короткое: "+input[i]);
        }

        Word[] wordsMas = wordsList.ToArray();

        Word[] wordsAdd = [];

        if(wordsMas.Length + words.Count() > words.Length)
        {
            Err($"Words for adds out of bounds: {wordsList.Count} + {words.Count()} > {words.Length} ({wordsList.Count + words.Count()} > {words.Length})");
            Mas($"Words for to add over more:", wordsMas);

            wordsAdd = Mist.Fill(wordsMas, words.Length - words.Count());

            Mas($"Will for to add these words:", wordsAdd);
        }
        else if(wordsMas.Length + words.Count() <= words.Length)
        {
            wordsAdd = wordsMas;
        }

        for (int i = 0, j = words.Count(); i < wordsAdd.Length; i++, j++)
            words[j] = wordsAdd[i]; // Добавление в библиотеку новых слов
        
        return;
    }
    
    public static void MoveVectorWords(int id, double[] error)
    {
        Word word = IdToWord(id);
        if(word.IsNull())
        {
            Exc("Word is Null");
            return;
        }

        double[] vector = word.vector;

        for(int i = 0; i < vector.Length; i++)
        {
            vector[i] -= learningRate * error[i];
        }
    }

    public static void MoveVectorWords(int[] id, double[][] error)
    {
        int min = Math.Min(id.Length, error.Length);

        //CheckIt("id.Length", id.Length); CheckIt("error.Length", error.Length);

        if(id.Length != error.Count())
            Exc($"Количество айди не соответствует длине ошибок: {id.Length} != {error.Count()}");

        Word[] words = IdToWord(id);

        if(words.IsNull() || words == null)
        {
            Exc("Local Param words is Null");
            return;
        }

        for(int i = 0; i < min; i++)
        {
            double[] vector = words[i].vector;
            //CheckIt("words[i].token", words[i].token);

            for(int j = 0; j < vector.Length; j++)
                vector[j] -= learningRate * error[i][j];

            Library.words[id[i]].vector = vector;
        }
    }

    public static double[][] Quest(string[] tokens, int index) // индекс всегда не равен -1
    {
        if(index == -1) return Quest(tokens);

        string[] quest = new string[index];
        for (int i = 0; i < index; i++)
            quest[i] = tokens[i];
        
        double[][] questVector = Init.Double<double>(index, dimension);

        for (int i = 0; i < words.Count(); i++)
            for (int j = 0; j < quest.Length; j++)
                if (quest[j] == words[i].token)
                    questVector[j] = words[i].vector; // Вектор Вопрос
        
        return questVector;
    }

    public static double[][] Answer(string[] tokens, int index) // индекс всегда не равен -1
    {
        if(index == -1) return [];

        string[] answer = new string[tokens.Length - (index + 1)];
        for (int i = index + 1, j = 0; i < tokens.Length; i++, j++)
            answer[j] = tokens[i];

        double[][] answerVector = Init.Double<double>(tokens.Length - (index + 1), dimension);

        for (int i = 0; i < words.Count(); i++)
            for (int j = 0; j < answer.Length; j++)
                if (answer[j] == words[i].token)
                    answerVector[j] = words[i].vector; // Вектор Ответ

        return answerVector;
    }

    public static double[][] Quest(string[] tokens)
    {
        string[] quest = tokens;
        double[][] questVector;

        questVector = Init.Double<double>(tokens.Length, dimension);

        MsgLine("Векторизация...\n", "Vectorisation...");
        Msg($"Quest: {string.Join(" ", quest)}");
        
        for (int i = 0; i < words.Count(); i++)
            for (int j = 0; j < quest.Length; j++)
                if (quest[j] == words[i].token)
                    questVector[j] = words[i].vector; // Вектор Вопрос

        return questVector;
    }

    public static Word IdToWord(int id)
    {
        if(id >= words.Count())
        {
            Exc("В библиотеке слов еще не существует такого id для words");
            return new();
        }
        else return words[id];
    }
    
    public static int TokenToId(string token)
    {
        for(int i = 0; i < words.Count(); i++)
            if(words[i].token == token)
                return i;

        return -1;
    }

    public static Word[] IdToWord(int[] id)
    {
        Word[] result = new Word[id.Length];

        for(int i = 0; i < id.Length; i++)
        {
            if(id[i] >= words.Count())
                result[i] = new();

            else result[i] = words[id[i]];
        }

        return result;
    }
    
    public static int[] TokensToId(string[] tokens)
    {
        int[] result = new int[tokens.Length];

        for(int j = 0, k = 0; j < tokens.Length; j++, k = 0)
        {
            for(int i = 0; i < words.Count(); i++)
                if(words[i].token == tokens[j])
                    k++;

            if(k == 1) result[j] = j;
            else result[j] = new();
        }
        return result;
    }

    public static void GetWords()
    {
        LineAdd("Токены: ");
        for (int i = 0; i < words.Count(); i++)
        {
            Msg(words[i].ToRound());
            LineAdd(words[i].token + ", ");
        }
        Line("\nДлина библиотеки: "+words.Count());
    }

}