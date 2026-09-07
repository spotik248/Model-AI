// Copyright (C) 2026  [Ваше Имя / Никнейм]
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://gnu.org>.


using static Model.Write;
using static Model.MSLU;
using static Model.Global;


//dotnet build -c Release


namespace Model;

class Model
{
    internal static bool finished = false;

    public static string version = "v0.4.2";
    public static string description = "Ура, победа";
    public static string model = "Модель";
    public static string user = "Пользователь";
    public static string time = Computer.Time();

    
    internal static void Main()
    {
        InitGlobal();
        //Update(false); Update(true);

        
        if(!DEV) Dialog(); //TODO: Включить при пользовании пользователем


        while (!finished)
        {
            EscPush();
            if(finished) break;

            //Update(false);
            if(startForm) StartForm();


            //Функции перед вводом пользователя
            

            Line("");
            string input = Util.UnExcConsoleRead(); // Никогда не null

            
            if(false)
            {
                if(!DEV)
                {
                    try
                    {
                        Router(input);
                    }
                    catch(Exception ex)
                    {
                        Throw("Непредвиденная ошибка одного из маршрутизаторов Модели: ", ex);   
                    }
                }
                else Router(input);
            }
            else AnotherRouter(input);
            

            //Функции после ввода пользователя
            //Update(true);
        }
        
        //Save();

        Line($"Выключение Модели.");
        Thread.Sleep(100);
        Environment.Exit(0);
    }


    public static void Dialog()
    {
        Command.Helloment();

        QuestYesNo("Загрузить последнее сохранение?", Load, () => {Line("\nСтандартная загрузка");});

        Thread.Sleep(100);

        Quest("Какую нейронную сеть использовать?",
            [["л", "локальная"], ["ф", "функциональная"], ["в", "векторную"], ["т", "трансформер"]],
            [
                () => NN.Change(0),
                () => NN.Change(1),
                () => NN.Change(2),
                () => NN.Change(3)
            ]
        );

        Command.GetModel(0);

        Command.EndDialog();
    }

    static void Router(string input) // dotnet build -c Release
    {
        finished = false;

        string[] inputSplit = input.Split(" ");

        string command = inputSplit[0];
        

        if (input == "" || input == " ")
        {
            if(DEV)
                NAnalis.General("что такое яблоко? ответ зеленый фрукт");
        }
        else if (command[0] != '/')
        {
            NAnalis.General(input);
        }
        else if (command[0] == '/')
        {
            Command.DoCommand(input, inputSplit, command);
        }
    }

    static void AnotherRouter(string input)
    {
        finished = false;
        if(input != "end")
        {
            input = input == "" ? "1" : input;
            int parse = int.Parse(input);

            int dimension = Library.dimension;

            // double inputLayout = 0.5;
            // double width1 = 0.5;
            // double bias1 = 0.1;

            // double width2 = 0.2;
            // double bias2 = 0.1;
            
            // double target = 0.9;

            // for(int i = 0; i < parse; i++) 
            // {
            //     double hiddenLayout = FucAct.Sigmoid(inputLayout * width1 + bias1);
            //     CheckIt("hiddenLayout", hiddenLayout);

            //     double outputLayout = FucAct.Sigmoid(hiddenLayout * width2 + bias2); 
            //     CheckIt("outputLayout", outputLayout);

            //     double error = target - outputLayout;
            //     CheckIt("error", error);

            //     double Delta = error * FucAct.DSigmoid(outputLayout); //
            //     CheckIt("Delta", Delta);

            //     width1 += Delta * inputLayout;
            //     width2 += Delta * hiddenLayout;
            //     CheckIt("width1", width1);
            //     CheckIt("width2", width2);

            //     bias1 += Delta;
            //     bias2 += Delta;
            //     CheckIt("bias1", bias1);
            //     CheckIt("bias2", bias2);
            // }

            //############################# ВЕКТОРНОЕ ОБУЧЕНИЕ #############################

            // int size = 100;

            // double[] inputLayout = Vector.ToSize(Init.Randomized(size-50), size);//[0.5, 0.1, 0.3]; 

            // double[][] width1    = Init.Randomized(size, size); // w[0] это все связи 
            // double[] bias1       = Init.Randomized(size);

            // double[][] width2    = Init.Randomized(size, size);
            // double[] bias2       = Init.Randomized(size);
            
            // double[] target      = Vector.ToSize(Init.Randomized(size-80), size);//[0.1, 0.9, 0.1];

            // Full(inputLayout);
            // Full(target);

            // double[] lastError = [];

            // for(int p = 1; p < parse+1; p++)
            // {
            //     double[] hiddenLayout = Init.Zeros(width1[0].Length);
            //     for(int i = 0; i < width1[0].Length; i++)
            //     {
            //         double mem = 0;
            //         for(int j = 0; j < inputLayout.Length; j++)
            //             mem += inputLayout[j] * width1[j][i];

            //         hiddenLayout[i] = FucAct.Sigmoid(mem + bias1[i]);
            //     }
            //     CheckIt("hiddenLayout", hiddenLayout);

            //     double[] outputLayout = Init.Zeros(width2[0].Length);
            //     for(int i = 0; i < width2[0].Length; i++)
            //     {
            //         double mem = 0;
            //         for(int j = 0; j < hiddenLayout.Length; j++)
            //             mem += hiddenLayout[j] * width2[j][i];

            //         outputLayout[i] = FucAct.Sigmoid(mem + bias2[i]);
            //     }
            //     CheckIt("outputLayout", outputLayout);

            //     double stud = 1/p;
            //     CheckIt("stud", [stud, p]);

            //     double[] error = Vector.Sub(outputLayout, target); //a - t
            //     CheckIt("error", error);

            //     double[] Delta = Vector.Mult(error, FucAct.DSigmoid(outputLayout));
            //     CheckIt("Delta", Delta);

            //     for(int i = 0; i < inputLayout.Length; i++)
            //         for(int j = 0; j < Delta.Length; j++)
            //             width1[i][j] -= stud * Delta[j] * inputLayout[j];
            //     CheckIt("width1", width1);
                
            //     for(int i = 0; i < hiddenLayout.Length; i++)
            //         for(int j = 0; j < Delta.Length; j++)
            //             width2[i][j] -= stud * Delta[j] * hiddenLayout[j]; // hiddenLayout[j]
            //     CheckIt("width2", width2);

            //     bias1 = Vector.Sub(bias1, Delta);
            //     CheckIt("bias1", bias1);

            //     bias2 = Vector.Sub(bias2, Delta);
            //     CheckIt("bias2", bias2);

            //     lastError = error;
            // }

            // Full(inputLayout);
            // Line("");
            // Full(target);
            // Line("");
            // Full(Vector.Sub(inputLayout, target));
            // Line("Разница inputLayout и target:", Vector.AddAll(Vector.Sub(inputLayout, target)), "");

            // Full(lastError);
            // Line("Последная ошибка", Vector.AddAll(lastError), "");

            //############################# ПАКЕТНОЕ ОБУЧЕНИЕ #############################

            // DateTime dt1 = DateTime.Now;

            // int size = 30;
            // int maxPock = 10;

            // double[][] inputLayout = Matrix.ToSize(Init.Randomized(maxPock, size), maxPock, size);//[0.5, 0.1, 0.3]; 

            // double[][] width1      = Init.Randomized(size, size); // w[0] это все связи
            // double[] bias1         = Init.Randomized(size);

            // double[][] width2      = Init.Randomized(size, size);
            // double[] bias2         = Init.Randomized(size);
            
            // double[][] target      = Matrix.ToSize(Init.Randomized(maxPock, size), maxPock, size);//[0.1, 0.9, 0.1];

            // double[][] lastError = Init.Double<double>(maxPock, size);
            // double[][] fewErrors = Init.Double<double>(parse, size);

            // for(int p = 1, pock = 0; p < parse+1; p++, pock++)
            // {
            //     if(pock == maxPock) pock = 0;
            //     CheckIt("pock", pock);

            //     double[] hiddenLayout = Init.Zeros(width1[0].Length);
            //     for(int i = 0; i < width1[0].Length; i++)
            //     {
            //         double mem = 0.0;
            //         for(int j = 0; j < inputLayout[0].Length; j++)
            //             mem += inputLayout[pock][j] * width1[j][i];

            //         hiddenLayout[i] = FucAct.Sigmoid(mem + bias1[i]);
            //     }
            //     CheckIt("hiddenLayout", hiddenLayout);

            //     double[] outputLayout = Init.Zeros(width2[0].Length);
            //     for(int i = 0; i < width2[0].Length; i++)
            //     {
            //         double mem = 0.0;
            //         for(int j = 0; j < hiddenLayout.Length; j++)
            //             mem += hiddenLayout[j] * width2[j][i];

            //         outputLayout[i] = FucAct.Sigmoid(mem + bias2[i]);
            //     }
            //     CheckIt("outputLayout", outputLayout);

            //     double stud = 20/Math.Pow2(p);
            //     CheckIt("stud", [stud, p]);

            //     double[] error = Vector.Sub(outputLayout, target[pock]); //a - t
            //     CheckIt("error", error);

            //     double[] Delta = Vector.Mult(error, FucAct.DSigmoid(outputLayout));
            //     CheckIt("Delta", Delta);

            //     for(int i = 0; i < inputLayout[0].Length; i++)
            //         for(int j = 0; j < Delta.Length; j++)
            //             width1[i][j] -= stud * Delta[j] * inputLayout[pock][j];
            //     CheckIt("width1", width1);
                
            //     for(int i = 0; i < hiddenLayout.Length; i++)
            //         for(int j = 0; j < Delta.Length; j++)
            //             width2[i][j] -= stud * Delta[j] * hiddenLayout[j]; // hiddenLayout[j]
            //     CheckIt("width2", width2);

            //     bias1 = Vector.Sub(bias1, Delta);
            //     CheckIt("bias1", bias1);

            //     bias2 = Vector.Sub(bias2, Delta);
            //     CheckIt("bias2", bias2);

            //     lastError[pock] = error;
            //     fewErrors[p-1] = error;
            // }


            // DateTime dt2 = DateTime.Now;


            //############################# АНАЛИЗАТОР ОШИБКИ #############################


            // for(int i = 0; i < maxPock; i++)
            // {
            //     Line("\n############## Пакет номер "+(i+1)+" ##############", "\n");

            //     Line("Разница inputLayout и target:", Vector.AddAll(Vector.Sub(inputLayout[i], target[i])), "");

            //     Full(lastError[i]);
            //     Line("Последная ошибка пакета", Vector.AddAll(lastError[i]), "");
            // }
            
            // TimeSpan dif = dt2 - dt1;
            // Line("Операция нейронной сети длилась: "+dif.Minutes+":"+dif.Seconds+" минут и секунд");


            //############################# ПРОИЗВОДНАЯ В ТОЧКЕ ОШИБКИ #############################

            
            // double[][] x = Init.Double<double>(fewErrors.Length, size-2);
            
            // for(int i = 0; i < fewErrors.Length; i++)
            // {
            //     for(int j = 1; j < size-1; j++)
            //     {
            //         x[i][j-1] = (fewErrors[i][j+1] - fewErrors[i][j-1]) * 100 / 2;
            //     }
            // }

            // x = Matrix.Transpose(x);// Перевернув, мы сможем смотреть изменение какой-то части ошибки для всех эпох, с которых они были взяты

            // for(int i = 0; i < x.Length; i++)
            // {
            //     Line("\nСкорость изменения части "+i+" от ошибки:\n");
            //     double min = Math.Min(x[i]);
            //     //x[i] = Vector.SubOne(x[i], min);
            //     for(int j = 0; j < x[i].Length; j++)
            //     {
            //         if(x[i][j] >= 0)
            //         {
            //             for(int s = (int)min; s < 0; s++) LineAdd(" ");
            //             for(int s = 0; s <= x[i][j]; s++) LineAdd("#");
            //         }
            //         else
            //         {
            //             for(int s = (int)x[i][j]; s <= 0; s++) LineAdd(".");
            //         }
                    
            //         LineAdd("\n");
            //     }
            //     Line(".\n");
            // }

            //############################# ЭМОЦИИ И ИМИТАЦИЯ ЗРЕНИЯ #############################

            // int size = 50;

            // string state = "";

            // double[] bad         = Init.Full(0.01, size);
            // double[] good        = Init.Full(0.99, size);

            // double[] badFull     = Init.Full(0.01, size * 2);
            // double[] goodFull    = Init.Full(0.99, size * 2);

            // double[] imageBad    = Init.Full(0.2, size);//Init.Randomized(size);
            // double[] imageGood   = Init.Full(0.8, size);//Init.Randomized(size);

            // double[][] width3    = Init.Xavier(size, size); // w[0] это все связи 
            // double[] bias3       = Init.Zeros(size);

            // //

            // //double[] inputLayout = Vector.ToSize(Init.Randomized(size-50), size); //[0.5, 0.1, 0.3];
            // //double[] inputLayout = Init.Zeros(size * 2);

            // double[][] width1    = Init.Xavier(size * 2, size * 2); // w[0] это все связи 
            // double[] bias1       = Init.Zeros(size * 2);

            // double[][] width2    = Init.Xavier(size * 2, size * 2);
            // double[] bias2       = Init.Zeros(size * 2);
            
            // //double[] target    = Vector.ToSize(Init.Randomized(size-80), size); //[0.1, 0.9, 0.1];
            // double[] targetBad   = Vector.Add(Init.Full(0.8, size * 2), Vector.DivOne(Init.Randomized(size * 2), 10)); // Init.Full(0.8, size * 2);
            // double[] targetGood  = Vector.Add(Init.Full(0.2, size * 2), Vector.DivOne(Init.Randomized(size * 2), 10)); // Init.Full(0.2, size * 2);

            // double[][] lastInput      = Init.Zeros(2, size * 2);
            // double[][] lastErrorEmote = Init.Zeros(2, size);
            // double[][] lastError      = Init.Zeros(2, size * 2);
            // double[][] lastError2     = Init.Zeros(2, size * 2);

            // for(int p = 1; p < parse+1; p++)
            // {
            //     state = p % 2 == 0 ? "good" : "bad";
            //     CheckIt("state", state);

            //     double[] image = state == "good" ? imageGood : imageBad;
            //     double[] targEmote = state == "good" ? good : bad;
            //     double[] emoteFull = state == "good" ? goodFull : badFull;
            //     double[] target = state == "good" ? targetGood : targetBad;

            //     double[] emotion = Init.Zeros(width3[0].Length);
            //     for(int i = 0; i < width3[0].Length; i++)
            //     {
            //         double mem = 0.0;
            //         for(int j = 0; j < image.Length; j++)
            //             mem += image[j] * width3[j][i];

            //         emotion[i] = FucAct.Sigmoid(mem + bias3[i]);
            //     }
            //     CheckIt("emotion", emotion);



            //     double stud = 1.0/(1.0 + 0.01 * p);
            //     CheckIt("stud", [stud, p]);

            //     double[] errorEmote = Vector.Sub(emotion, targEmote); //a - t
            //     CheckIt("errorEmote", errorEmote);

            //     double[] DeltaEmote = Vector.Mult(errorEmote, FucAct.DSigmoid(emotion));
            //     CheckIt("DeltaEmote", DeltaEmote);



            //     for(int i = 0; i < image.Length; i++)
            //         for(int j = 0; j < DeltaEmote.Length; j++)
            //             width3[i][j] -= stud * DeltaEmote[j] * image[i]; // image[j]
            //     CheckIt("width3", width3);

            //     bias3 = Vector.Sub(bias3, DeltaEmote);
            //     CheckIt("bias3", bias3);

                
            //     // Объединение двух векторов в один [size], [size] => [size * 2]
            //     double[] inputLayout = Vector.Combinate(emotion, image);
            //     CheckIt("inputLayout", inputLayout);



            //     double[] hiddenLayout = Init.Zeros(width1[0].Length);
            //     for(int i = 0; i < width1[0].Length; i++)
            //     {
            //         double mem = 0.0;
            //         for(int j = 0; j < inputLayout.Length; j++)
            //             mem += inputLayout[j] * width1[j][i];

            //         hiddenLayout[i] = FucAct.Sigmoid(mem + bias1[i]);
            //     }
            //     CheckIt("hiddenLayout", hiddenLayout);

            //     double[] outputLayout = Init.Zeros(width2[0].Length);
            //     for(int i = 0; i < width2[0].Length; i++)
            //     {
            //         double mem = .0;
            //         for(int j = 0; j < hiddenLayout.Length; j++)
            //             mem += hiddenLayout[j] * width2[j][i];

            //         outputLayout[i] = FucAct.Sigmoid(mem + bias2[i]);
            //     }
            //     CheckIt("outputLayout", outputLayout);



                

            //     double[] error = Vector.Sub(outputLayout, target); //a - t
            //     CheckIt("error", error);

            //     double power = 1.0 + Math.Abs((Vector.AddAll(emoteFull) / emoteFull.Length) - 0.5) * 2;
            //     CheckIt("power", power);

            //     double[] error2 = Vector.MultOne(error, power);
                
            //     CheckIt("error2", error2);

            //     double[] Delta = Vector.Mult(error2, FucAct.DSigmoid(outputLayout));
            //     CheckIt("Delta", Delta);
            //     CheckIt("outputLayout, DSigmoid", [outputLayout, FucAct.DSigmoid(outputLayout)]);



            //     double[] hiddenError = Init.Zeros(width1.Length);

            //     for (int i = 0; i < hiddenLayout.Length; i++)
            //     {
            //         double sumFeedback = 0.0;
            //         for (int j = 0; j < Delta.Length; j++)
            //         {
            //             // Передаем влияние дельты обратно через веса связи
            //             sumFeedback += Delta[j] * width2[i][j]; 
            //         }
            //         hiddenError[i] = sumFeedback; // без DSigmoid
            //     }
            //     CheckIt("hiddenError", hiddenError);



            //     for(int i = 0; i < inputLayout.Length; i++)
            //         for(int j = 0; j < hiddenError.Length; j++)
            //             width1[i][j] -= stud * hiddenError[j] * inputLayout[i];
            //     CheckIt("width1", width1);
                
            //     for(int i = 0; i < hiddenLayout.Length; i++)
            //         for(int j = 0; j < Delta.Length; j++)
            //             width2[i][j] -= stud * Delta[j] * hiddenLayout[i]; // hiddenLayout[j]
            //     CheckIt("width2", width2);


            //     for(int i = 0; i < bias1.Length; i++)
            //         bias1[i] -= stud * hiddenError[i];
            //     CheckIt("bias1", bias1);

            //     for(int i = 0; i < bias2.Length; i++)
            //         bias2[i] -= stud * Delta[i];
            //     CheckIt("bias2", bias2);



            //     int index = state == "good" ? 0 : 1;

            //     CheckIt("index", index);

            //     lastInput[index] = inputLayout;
            //     lastErrorEmote[index] = errorEmote;
            //     lastError[index] = error;
            //     lastError2[index] = error2;

            //     CheckIt("### end ###");
            // }



            // Line("Разница imageGood и good:", Vector.AddAll(Vector.Sub(imageGood, good)), "");
            // Line("Разница imageBad и bad:", Vector.AddAll(Vector.Sub(imageBad, bad)), "\n");

            // Line("Разница inputLayoutGood и targetGood:", Vector.AddAll(Vector.Sub(lastInput[0], targetGood)), "");
            // Line("Разница inputLayoutBad и targetBad:", Vector.AddAll(Vector.Sub(lastInput[1], targetBad)), "\n");

            // Full(lastErrorEmote[0]);
            // Line("Последная ошибка памяти для good:", Vector.AddAll(lastErrorEmote[0]), "");
            // Full(lastErrorEmote[1]);
            // Line("Последная ошибка памяти для bad:", Vector.AddAll(lastErrorEmote[1]), "\n");

            // Full(lastError[0]);
            // Line("Последная ошибка good:", Vector.AddAll(lastError[0]));
            // Line("Последная ошибка2 good:", Vector.AddAll(lastError2[0]), "");
            
            // Full(lastError[1]);
            // Line("Последная ошибка bad:", Vector.AddAll(lastError[1]));
            // Line("Последная ошибка2 bad:", Vector.AddAll(lastError2[1]), "");

            //############################# РАБОЧИЙ ПЕРЦЕПТРОН LLEP #############################

            // int size = 5;
            // int sizedim = size * dimension; // 100
            // int size2 = (int)Math.Pow2(size); // 25

            // double[] inputLayout = Vector.ToSize(Init.Randomized((size-1) * dimension), sizedim); // sizedim
            // //double[] inputLayout = Init.Zeros(size * 2);

            // double[][] width1 = Init.Xavier(sizedim, size2); // 100x25
            // double[] bias1    = Init.Zeros(size2);           // x25

            // double[][] width2 = Init.Xavier(size2, sizedim);
            // double[] bias2    = Init.Zeros(sizedim);
            
            // double[] target   = Vector.ToSize(Init.Randomized((size-2) * dimension), sizedim); // sizedim

            // double[] lastError       = Init.Zeros(sizedim);
            // double[] lastHiddenError = Init.Zeros(sizedim);

            // for(int p = 1; p < parse+1; p++)
            // {

            //     double[] hiddenLayout = Init.Zeros(width1[0].Length);
            //     for(int i = 0; i < width1[0].Length; i++)
            //     {
            //         double mem = 0;
            //         for(int j = 0; j < inputLayout.Length; j++)
            //             mem += inputLayout[j] * width1[j][i];

            //         hiddenLayout[i] = FucAct.Sigmoid(mem + bias1[i]);
            //     }
            //     CheckIt("hiddenLayout", hiddenLayout);

            //     double[] outputLayout = Init.Zeros(width2[0].Length);
            //     for(int i = 0; i < width2[0].Length; i++)
            //     {
            //         double mem = 0;
            //         for(int j = 0; j < hiddenLayout.Length; j++)
            //             mem += hiddenLayout[j] * width2[j][i];

            //         outputLayout[i] = FucAct.Sigmoid(mem + bias2[i]);
            //     }
            //     CheckIt("outputLayout", outputLayout);



            //     double stud = 1.0/(1.0 + 0.1 * p);
            //     CheckIt("stud", [stud, p]);

            //     //double stud = 1.0/p;
            //     //CheckIt("stud", [stud, p]);

            //     double[] error = Vector.Sub(outputLayout, target); //a - t
            //     CheckIt("error", error);

            //     double[] Delta = Vector.Mult(error, FucAct.DSigmoid(outputLayout));
            //     CheckIt("Delta", Delta);
            //     CheckIt("outputLayout, DSigmoid", [outputLayout, FucAct.DSigmoid(outputLayout)]);




            //     double[] hiddenError = Init.Zeros(width1[0].Length);

            //     for (int i = 0; i < hiddenLayout.Length; i++)
            //     {
            //         double sumFeedback = 0;
            //         for (int j = 0; j < Delta.Length; j++)
            //         {
            //             // Передаем влияние дельты обратно через веса
            //             sumFeedback += Delta[j] * width2[i][j];
            //         }
            //         hiddenError[i] = sumFeedback; // без DSigmoid
            //     }
            //     CheckIt("hiddenError", hiddenError);



            //     for(int i = 0; i < inputLayout.Length; i++)
            //         for(int j = 0; j < hiddenError.Length; j++)
            //             width1[i][j] -= stud * hiddenError[j] * inputLayout[i];
            //     CheckIt("width1", width1);
                
            //     for(int i = 0; i < hiddenLayout.Length; i++)
            //         for(int j = 0; j < Delta.Length; j++)
            //             width2[i][j] -= stud * Delta[j] * hiddenLayout[i]; // hiddenLayout[j]
            //     CheckIt("width2", width2);


            //     for(int i = 0; i < bias1.Length; i++)
            //         bias1[i] -= stud * hiddenError[i];
            //     CheckIt("bias1", bias1);

            //     for(int i = 0; i < bias2.Length; i++)
            //         bias2[i] -= stud * Delta[i];
            //     CheckIt("bias2", bias2);



            //     lastError = error;
            //     lastHiddenError = hiddenError;

            //     CheckIt("### end ###");
            // }

            // Line("Разница inputLayout и target:", Vector.AddAll(Vector.Sub(inputLayout, target)), "\n");

            // Full(lastError);
            // Line("Последная выходная ошибка:", Vector.AddAll(lastError));

            // Full(lastHiddenError);
            // Line("Последная скрытая ошибка:", Vector.AddAll(lastHiddenError));

            //############################# Layout Simulating Perceptron #############################

            // int size = 2;
            // int size2 = size * 100; // 200

            // //double[] inputLayout = Vector.ToSize(Init.Randomized((size-1) * dimension), size); // size
            // //double[] inputLayout = Init.Zeros(size * 2);
            // double[] inputLayout = [0.3, 0.1];

            // double[][] width1 = Init.Xavier(size, size2); // 2x200
            // double[] bias1    = Init.Zeros(size2);           // x200

            // double[][] width2 = Init.Xavier(size2, size);
            // double[] bias2    = Init.Zeros(size);

            // int maxMap = 10;
            // //int[] finger = [3, 1];
            
            // //double[] target   = Vector.ToSize(Init.Randomized((size-2) * dimension), size); // size
            // double[] target   = [5, 5];

            // target = Vector.DivOne(target, maxMap);

            // double[] lastError       = Init.Zeros(size);
            // double[] lastHiddenError = Init.Zeros(size);

            // writeCheckIt = false;

            // for(int p = 1, step = 0; p < parse+1; p++)
            // {
            //     step++;
            //     // h = f(I * W + b)

            //     double[] hiddenLayout = Init.Zeros(width1[0].Length);
            //     for(int i = 0; i < width1[0].Length; i++)
            //     {
            //         double mem = 0;
            //         for(int j = 0; j < inputLayout.Length; j++)
            //             mem += inputLayout[j] * width1[j][i];

            //         hiddenLayout[i] = FucAct.Sigmoid(mem + bias1[i]);
            //     }
            //     CheckIt("hiddenLayout", hiddenLayout);


            //     double[] outputLayout = Init.Zeros(width2[0].Length);
            //     for(int i = 0; i < width2[0].Length; i++)
            //     {
            //         double mem = 0;
            //         for(int j = 0; j < hiddenLayout.Length; j++)
            //             mem += hiddenLayout[j] * width2[j][i];

            //         outputLayout[i] = FucAct.Sigmoid(mem + bias2[i]);
            //     }
            //     CheckIt("outputLayout", outputLayout);



            //     double stud = 0.5/(1.0 + 0.1 * p);
            //     CheckIt("stud", [stud, p]);

            //     //double stud = 1.0/p;
            //     //CheckIt("stud", [stud, p]);



            //     double[] error = Vector.Sub(inputLayout, target); //a - t
            //     CheckIt("error", error);

            //     double[] Delta = Vector.Mult(error, FucAct.DSigmoid(outputLayout));
            //     CheckIt("Delta", Delta);
            //     CheckIt("outputLayout, DSigmoid", [outputLayout, FucAct.DSigmoid(outputLayout)]);


            //     for(int i = 0; i < inputLayout.Length; i++) // Нормирование
            //     {
            //         inputLayout[i] += outputLayout[i]-0.5;

            //         inputLayout[i] = Math.MinMax(inputLayout[i], 0, 1);
            //     }


            //     double[] hiddenError = Init.Zeros(width1[0].Length);

            //     for (int i = 0; i < hiddenLayout.Length; i++)
            //     {
            //         double sumFeedback = 0;
            //         for (int j = 0; j < Delta.Length; j++)
            //         {
            //             // Передаем влияние дельты обратно через веса
            //             sumFeedback += Delta[j] * width2[i][j];
            //         }
            //         hiddenError[i] = sumFeedback; // без DSigmoid
            //     }
            //     CheckIt("hiddenError", hiddenError);



            //     for(int i = 0; i < inputLayout.Length; i++)
            //         for(int j = 0; j < hiddenError.Length; j++)
            //             width1[i][j] -= stud * hiddenError[j] * inputLayout[i];
            //     CheckIt("width1", width1);
                
            //     for(int i = 0; i < hiddenLayout.Length; i++)
            //         for(int j = 0; j < Delta.Length; j++)
            //             width2[i][j] -= stud * Delta[j] * hiddenLayout[i]; // hiddenLayout[j]
            //     CheckIt("width2", width2);


            //     for(int i = 0; i < bias1.Length; i++)
            //         bias1[i] -= stud * hiddenError[i];
            //     CheckIt("bias1", bias1);

            //     for(int i = 0; i < bias2.Length; i++)
            //         bias2[i] -= stud * Delta[i];
            //     CheckIt("bias2", bias2);


            //     //if(Vector.AddAll(error) < 0.001)
            //     if(Math.RoundInt(target[0]*maxMap) == Math.RoundInt(inputLayout[0]*maxMap)
            //     && Math.RoundInt(target[1]*maxMap) == Math.RoundInt(inputLayout[1]*maxMap))
            //     {
            //         Line("### GET IT! ###");
            //         Line($"Координаты нейронной сети: {inputLayout[0]}, {inputLayout[1]} ({Math.RoundInt(inputLayout[0]*maxMap)}, {Math.RoundInt(inputLayout[1]*maxMap)})");
            //         Line($"Координаты цели: {target[0]}, {target[1]} ({Math.RoundInt(target[0]*maxMap)}, {Math.RoundInt(target[1]*maxMap)})");

            //         for(int i = 0; i < 2; i++)
            //         {
            //             double rd = Init.Rand.NextDouble();

            //             target[i] = Math.Round(rd * maxMap) / maxMap;
            //         }
                    
            //         Line($"Новые координаты цели: {target[0]}, {target[1]} ({Math.RoundInt(target[0]*maxMap)}, {Math.RoundInt(target[1]*maxMap)})");
            //         Line($"Шагов понадобилось: {step}");
            //         step = 0;
            //     }


            //     LineAdd("\nКарта: ");
            //     Line(Vector.AddAll(error)+"\n");
            //     for(int y = maxMap+1; y > 0; y--)
            //     {
            //         string str = "";
            //         for(int x = 0; x <= maxMap; x++)
            //         {
            //             char c = '.';

            //             if      (x == Math.RoundInt(inputLayout[0]*maxMap) && y == Math.RoundInt(inputLayout[1]*maxMap))
            //                 c = '0';
            //             else if (x == Math.RoundInt(target[0]*maxMap)      && y == Math.RoundInt(target[1]*maxMap))
            //                 c = 'T';
            //             else
            //                 c = '.';

            //             str += c;
            //         }
            //         Line(str);
            //     }
                

            //     lastError = error;
            //     lastHiddenError = hiddenError;

            //     CheckIt("### end ###");
            // }


            // Line("\n");

            // Full(inputLayout);
            // Line("inputLayout:", Vector.AddAll(inputLayout));
            
            // Full(target);
            // Line("target:", Vector.AddAll(target));

            // Line("\nКарта: \n");
            // for(int y = maxMap; y >= 0; y--)
            // {
            //     string str = "";
            //     for(int x = 0; x <= maxMap; x++)
            //     {
            //         char c = '.';

            //         if      (x == Math.RoundInt(inputLayout[0]*maxMap) && y == Math.RoundInt(inputLayout[1]*maxMap))
            //             c = '0';
            //         else if (x == Math.RoundInt(target[0]*maxMap)      && y == Math.RoundInt(target[1]*maxMap))
            //             c = 'T';
            //         else
            //             c = '.';

            //         str += c;
            //     }
            //     Line(str);
            // }
            
            // Line("\n");

            // Line("Разница нормированного inputLayout и target:", Vector.AddAll(Vector.Sub(inputLayout, target)), "\n");

            // Full(lastError);
            // Line("Последная выходная ошибка:", Vector.AddAll(lastError));

            // Full(lastHiddenError);
            // Line("Последная скрытая ошибка:", Vector.AddAll(lastHiddenError));

            //############################# Glow NeuroNetwork (Чуть чуть не то, но работает) #############################

            // int group = 10;

            // Neuron[] neurons = new Neuron[group];

            // for(int i = 0; i < group; i++)
            // {
            //     neurons[i] = neurons[i].Zeros(group);
            //     neurons[i].power = 1;
            // }

            // neurons[0].bias = 1; neurons[9].bias = 1; // Первичный шум

            // int[] glowGroup = new int[group];
            // int glowGroupCount = 0;
            // sbyte[] impulGroup = new sbyte[group];

            // int[] lastGlow = new int[group];
            // sbyte[] lastImpul = new sbyte[group];

            // for(int p = 1; p < parse+1; p++)
            // {
            //     for(int i = 0, j = 0; i < group; i++) // Ищем нейроны, которые зажглись
            //     {
            //         if(neurons[i].bias > 0 || impulGroup[i] > 0)
            //         {
            //             CheckIt("Светится "+i+" нейрон");
            //             glowGroup[j] = i;
            //             j++;
            //             glowGroupCount = j;
            //         }
            //     }

            //     for(int i = 0; i < glowGroupCount; i++) // Объединяем таким весом, который пропорционален длинне связи и биасу
            //     {
            //         Line();
            //         if(glowGroupCount <= 1)
            //         {в
            //             CheckIt("Скипаем соединение связи: "+glowGroupCount);
            //             break; // Если засветилось меньше двух нейронов - скип объединения
            //         }

            //         Neuron nn = neurons[glowGroup[i]];
            //         int link = Math.Max(1, nn.bias);
            //         CheckIt("link: "+link);

            //         for(int j = 0; j < glowGroupCount; j++) // TODO: В будущем убрать j = i и заменить j = 0
            //         {
            //             if(j != i) // Нейрон не может создать связь сам с собой
            //             {
            //                 Line();
            //                 CheckIt("From "+glowGroup[i]+" to "+glowGroup[j]);
            //                 int len = glowGroup[j] - glowGroup[i];
            //                 CheckIt("len: "+len);

            //                 int offlen = 0;
            //                 int k = glowGroup[j];
                            
            //                 if(len > -link && len < link) // добавить, чтобы нейроны не могли передавать слишком далеко
            //                 {
            //                     Line();
            //                 }
            //                 else if(len >= link) //Если связь слишком длинная в плюс
            //                 {
            //                     Line();
            //                     offlen = link - len;
            //                     k = glowGroup[i]+link;
            //                 }
            //                 else if(len <= -link) //Если связь слишком длинная в минус
            //                 {
            //                     Line();
            //                     offlen = len - link;
            //                     k = glowGroup[i]-link;
            //                 }

            //                 Line();
                            
            //                 nn.width[k] = (byte)(1/(len+offlen) + nn.bias + impulGroup[k]); // сила веса

            //                 CheckIt("Offlen: "+offlen);
            //                 CheckIt("From "+glowGroup[i]+" to "+k);
            //                 CheckIt("1/(len+offlen): "+(1/(len+offlen)));
            //                 CheckIt("nn.bias*0.1: "+(nn.bias*0.1));
            //                 CheckIt("impulGroup[k]*0.1: "+(impulGroup[k]*0.1));
            //                 CheckIt("New width: "+nn.width[k]);
            //             }
            //             else nn.width[glowGroup[j]] = 0; // Направление к собственным весам должно равнятся нулю
            //         }
            //     }

            //     CheckIt("Удаление импульсов");
            //     for(int i = 0; i < group; i++) impulGroup[i] = 0; // Отчищаем предыдущие значения импульсов
            //     Line();

            //     for(int i = 0; i < group; i++) // Открываем веса каждого нейрона и отравляем импульсы
            //     {
            //         for(int j = 0; j < group; j++)
            //         {
            //             //CheckIt("i: "+i+", j: "+j+", neurons[i].width[j]: "+(neurons[i].width[j]*0.1)+", bool: "+((neurons[i].width[j]*0.1)>0));
            //             if(neurons[i].width[j] > 0)
            //             {
            //                 impulGroup[j] += (sbyte)(neurons[i].power * (neurons[i].width[j]*0.1) + neurons[i].bias);
            //                 CheckIt("Новый импульс "+impulGroup[j]+" для "+j);
            //             }
            //         }
            //     }

            //     lastGlow = glowGroup;
            //     lastImpul = impulGroup;
                
            //     CheckIt("### end ###");
            // }


            // // Line("Последнее зажигание (список): ");
            // // string lastGlowShort = "";
            // // for(int i = 0, lastNum = -1; i < lastGlow.Length; i++)
            // // {
            // //     if(lastNum == lastGlow[i])
            // //     {
            // //         if(lastGlow[i] == 0)
            // //             lastGlowShort += "*";
            // //         else
            // //             lastGlowShort += lastGlow[i] + "?";
            // //     }
            // //     else
            // //     {
            // //         lastGlowShort += lastGlow[i];
            // //     }

            // //     lastGlowShort += ", ";

            // //     lastNum = lastGlow[i];
            // // }
            // // Line(lastGlowShort);


            // Line("Последнее зажигание: ");
            // string lastGlowFull = "";
            // for(int i = 0, j = 0; i < lastGlow.Length; i++)
            // {
            //     if(i == lastGlow[j])
            //     {
            //         lastGlowFull += 1;
            //         j++;
            //     }
            //     else
            //     {
            //         lastGlowFull += 0;
            //     }
            //     lastGlowFull += ", ";
            // }
            // Line(lastGlowFull);


            // Line("Последние импульсы: "); 
            // Full(", ", lastImpul);

            //############################# Glow NeuroNetwork #############################

            // int group = 10;

            // bool[] powerGroup = new bool[group];
            // float[][] widthes = Init.Zeros<float>(group, group);
            // float[] biases = new float[group];


            // bool[] glowGroup = new bool[group];
            // float[] impulGroup = new float[group];


            // for(int i = 0; i < group; i++)
            // {
            //     powerGroup[i] = true; // значит 1, в противном случае -1
            // }

            // biases[0] = 2; biases[9] = 2; // Первичный шум


            // bool[] lastGlow = new bool[group];
            // float[] lastImpul = new float[group];

            // for(int p = 1; p < parse+1; p++)
            // { 
            //     for(int i = 0; i < group; i++) 
            //     {
            //         glowGroup[i] = false; // обнуление тех, кто зажегся до этого

            //         if(biases[i] + impulGroup[i] > 0)// Ищем нейроны, которые нужно зажечь
            //         {
            //             CheckIt("Светится "+i+" нейрон");
            //             glowGroup[i] = true;
            //         }
            //     }

            //     for(int i = 0; i < glowGroup.Length; i++) // Объединяем таким весом, который пропорционален длинне связи и биасу
            //     {
            //         if(glowGroup[i])
            //         {
            //             Line();

            //             bool activated = true;

            //             int link = (int)Math.Max(1, biases[i]);
            //             CheckIt("link: "+link);

            //             for(int j = 0; j < glowGroup.Length; j++)
            //             {
            //                 if(j != i) // Нейрон не может создать связь сам с собой
            //                 {
            //                     if(glowGroup[i] && glowGroup[j]) //с теми нейронами, которые светятся
            //                     if(activated) // За один ход можно создать только одну связь
            //                     {
                                    
            //                         Line();
            //                         int len = j - i;
            //                         CheckIt("Wanna from "+i+" to "+j+" make len: "+len);

            //                         int offlen = 0;
            //                         int k = -1;
                                    
            //                         if(len > -link && len < link) // добавить, чтобы нейроны не могли передавать слишком далеко
            //                         {
            //                             offlen = 0;
            //                             k = j;
            //                         }
            //                         else if(len >= link) //Если связь слишком длинная в плюс
            //                         {
            //                             offlen = link - len;
            //                             k = i+link;
            //                         }
            //                         else if(len <= -link) //Если связь слишком длинная в минус
            //                         {
            //                             offlen = len - link;
            //                             k = i-link;
            //                         }

            //                         Line();
                                    
            //                         widthes[i][k] = 1/(len+offlen) + biases[i]*0.1f + impulGroup[k]*0.1f; // сила веса
            //                         widthes[i][k] = Math.Max(widthes[i][k], 0.01f);

            //                         CheckIt("Offlen: "+offlen);
            //                         CheckIt("ReFrom "+i+" to "+k);
            //                         Line();
            //                         CheckIt("params: "+(1/(len+offlen))+" + "+(impulGroup[k]*0.1f)+" + "+(biases[i]*0.1f));
            //                         CheckIt("New width: "+widthes[i][k]);

            //                         activated = false;
            //                     } else break;
            //                 }
            //                 else widthes[i][j] = 0; // Направление к собственным весам должно равнятся нулю
            //             }
            //         }
            //     }

            //     Line();
            //     CheckIt("Удаление импульсов");
            //     for(int i = 0; i < group; i++) impulGroup[i] = 0; // Отчищаем предыдущие значения импульсов
            //     Line();

            //     for(int i = 0; i < group; i++) // Открываем веса каждого нейрона и отравляем импульсы, после запасаем часть в биас
            //     {
            //         for(int j = 0; j < group; j++)
            //         {
            //             //CheckIt("i: "+i+", j: "+j+", neurons[i].width[j]: "+(neurons[i].width[j]*0.1)+", bool: "+((neurons[i].width[j]*0.1)>0));
            //             if(widthes[i][j] > 0)
            //             {
            //                 int power = powerGroup[i] ? 1 : -1;
            //                 impulGroup[j] = power * widthes[i][j]*0.1f + biases[i]*0.1f;
            //                 CheckIt("Новый импульс "+impulGroup[j]+" от "+i+" для "+j);
                            
            //                 biases[j] += power * widthes[i][j]*0.1f + impulGroup[j]*0.1f;
            //                 CheckIt("Новый биас "+biases[j]+" от "+i+" для "+j);
            //             }
            //         }
            //     }

            //     lastGlow = glowGroup;
            //     lastImpul = impulGroup;
                
            //     CheckIt("### end ###\n");
            // }


            // Line("Последнее зажигание: ");
            // string lastGlowFull = "";
            // for(int i = 0; i < lastGlow.Length; i++)
            // {
            //     if(lastGlow[i])
            //         lastGlowFull += 1;
            //     else
            //         lastGlowFull += 0;
                
            //     lastGlowFull += ", ";
            // }
            // Line(lastGlowFull);


            // Line("Последние импульсы: "); 
            // Full(", ", lastImpul);

            // Line("Последние веса: "); 
            // Full(", ", widthes);

            // Line("Последние биасы: "); 
            // Full(", ", biases);

            //############################# РАБОЧИЙ ПЕРЦЕПТРОН LLEP с эмоциями, зрением, словами и памятью, человеко имитирующий #############################

            int size = 5;
            int sizedim = size * dimension * 2; // 100
            int size2 = (int)Math.Pow2(size); // 25

            double[] inputLayout = Vector.ToSize(Init.Randomized((size-1) * dimension), sizedim); // sizedim x100
            //double[] inputLayout = Init.Zeros(size * 2);

            double[] view = Init.Full(0.5, sizedim); // x100

            double[] data = Init.Zeros(sizedim*3); // x100*3 // Вся информация (сенсоры + эмоции)


            double[][] shortTermMemory = Init.Zeros(1, sizedim*3); // 1x100 * 3 // кр.ср.память (куча данных)


            double[] Future   = Init.Zeros(sizedim*3);   // x100 * 3 // для будущего
            double[] fromFuture = Init.Zeros(sizedim*3); // x100 * 3 // из будущего

            double[][] widthf1 = Init.Xavier(sizedim*3, sizedim); // 100*3x100
            double[] biasf1    = Init.Zeros(sizedim);               // x100

            double[][] widthf2 = Init.Xavier(sizedim, sizedim*3); // 100x100*3
            double[] biasf2    = Init.Zeros(sizedim*3);           // x100*3

            double[] emotion    = Init.Full(0.5, sizedim*3);   // Эмоция, то же что и предсказание


            int maxLogWriteMemory = 10; //
            double[][] logWriteMemory = Init.Zeros(maxLogWriteMemory, sizedim*3);  // Для записи в дл.ср.память


            double[] lastEmotion       = Init.Zeros(sizedim*3);
            double[] lastEmHiddenError = Init.Zeros(sizedim*3);


            double[][] width1 = Init.Xavier(sizedim, size2); // 100x25
            double[] bias1    = Init.Zeros(size2);           // x25

            double[][] width2 = Init.Xavier(size2, sizedim); // 25x100
            double[] bias2    = Init.Zeros(sizedim);         // x100

            
            double[] longTermMemory = Init.Zeros(sizedim);  // Долгосрочная память, сжимает по признакам
            double[] autoMemory = Init.Zeros(sizedim);      // Процедурная память, используется при автоматизме

            double[] target   = Vector.ToSize(Init.Randomized((size-2) * dimension), sizedim); // sizedim


            double[] lastError       = Init.Zeros(sizedim);
            double[] lastHiddenError = Init.Zeros(sizedim);

            /* Слушай, это вышло всё довольно трудно.
            // Я не использую transformer, у меня собственная нейронка, а все что ты написал это довольно непонятно.
            // Можно грубо разделить слова, зрение и эмоции на сенсоры, а у меня в нейронке они выходят как:
            // слова это просто матрица слов на эмбеддинги;
            // зрение тоже матрица заполненная от 0 до 1 (туда умещаются все цвета, а не черно-белый);
            // эмоции это матрица, сделана она не обычно, она делится на 3 вектора:
            // первый это эмоции полученные от слов,
            // второй это вектор эмоций от увиденного,
            // а третий это внутреннее состояние, типа как настроение у человека.
            // Числа в векторах эмоции от 0 до 1, пока что это означает лишь силу эмоции, но думаю в будущем получится сделать их не просто силой, а именно, что они что-то значат
            
            // Я пишу на c#.
            // Это очень простенькая работа памяти.
            // Яркое событие -> сжать данные -> сохранить.
            // Вспомнить -> поиск среди памяти -> загрузить в сеть.
            // Тебе не кажется?

            // Слушай, я думал, что прям нужно копировать всё с мозга,
            // типа, а как же краткосрочная память?
            // а как то, что в долгосрочной памяти
            // используется сжатие по признаку,
            // а не просто среднее число, разве нет?

            // Ага, только вот еще там прикол в том,
            // что в долгосрочную память попадают все эти моменты,
            // которые нужно запомнить,
            // только во время сна или нет?
            
            // Ого, вот как можно сделать обучение:
            // собирать яркие данные и кучей их обрабатывать,
            // обновлять веса
            // и сразу записывать в долгосрочную память

            // А автоматизм использует память? =
            // если так, то в неё можно сохранять слова-ответы или действия-ответы на слабые раздражители.
            // А за выбор будет отвечать как раз
            // тот самый механизм новизны, механизм который предсказывает будущее,
            // если всё предсказуемо - выбираем автоматизм, если нет решаем сложную задачу
            */

            

            for(int p = 1; p < parse+1; p++)
            {
                // Загрузка данных                

                data = Matrix.ToVector([inputLayout, view, emotion]);

                // Загрузка из ShortTermMemory, подмешивание в данные
                
                for(int i = 0; i < data.Length; i++)
                    data[i] += shortTermMemory[0][i] * 0.3;

                // Механизм предсказания
                // Вход: Любая информация (Слова, зрение, эмоции, внутреннее состояние, кр.ср. память)
                // Чем больше emotion, тем лучше запомнится это в долгосрочную память

                // Загрузка предсказания из прошлого
                

                double[] inputFutureLayout = data; // x100*3 // Входящие данные это данные сети
                fromFuture = Future; // x100*3 //Загрузка предсказания будущего из прошлого заранее

                double[] hiddenFutureLayout = Init.Zeros(widthf1[0].Length);
                for(int i = 0; i < widthf1[0].Length; i++)
                {
                    double mem = 0;
                    for(int j = 0; j < inputFutureLayout.Length; j++)
                        mem += inputFutureLayout[j] * widthf1[j][i];

                    hiddenFutureLayout[i] = FucAct.Sigmoid(mem + biasf1[i]);
                }
                CheckIt("hiddenFutureLayout", hiddenFutureLayout);

                double[] outputFutureLayout = Init.Zeros(widthf2[0].Length);
                for(int i = 0; i < widthf2[0].Length; i++)
                {
                    double mem = 0;
                    for(int j = 0; j < hiddenFutureLayout.Length; j++)
                        mem += hiddenFutureLayout[j] * widthf2[j][i];

                    outputFutureLayout[i] = FucAct.Sigmoid(mem + biasf2[i]);
                }
                CheckIt("outputFutureLayout", outputFutureLayout);

                // Слоя предсказания
                Future = outputFutureLayout;

                // Поиск разницы - эмоция (поиск ошибки)
                emotion = Vector.Sub(fromFuture, data);
                CheckIt("emotion", emotion);

                data = Matrix.ToVector([inputLayout, view, emotion]); // Сохраняем новую эмоцию

                shortTermMemory[0] = data; // Сохранение данных в кр.ср.память

                double[] emDelta = Vector.Mult(emotion, FucAct.DSigmoid(fromFuture));
                CheckIt("emDelta", emDelta);
                CheckIt("(emotion) outputLayout, DSigmoid", [fromFuture, FucAct.DSigmoid(fromFuture)]);


                double stud = 1.0/(1.0 + 0.1 * p); //double stud = 1.0/p;
                CheckIt("stud", [stud, p]);



                double[] emHiddenError = Init.Zeros(widthf1[0].Length);

                for (int i = 0; i < hiddenFutureLayout.Length; i++)
                {
                    double sumFeedback = 0;
                    for (int j = 0; j < emDelta.Length; j++)
                    {
                        // Передаем влияние дельты обратно через веса
                        sumFeedback += emDelta[j] * widthf2[i][j];
                    }
                    emHiddenError[i] = sumFeedback; // без DSigmoid
                }
                CheckIt("emHiddenError", emHiddenError);


                //Обновление эмоций

                for(int i = 0; i < inputFutureLayout.Length; i++)
                    for(int j = 0; j < emHiddenError.Length; j++)
                        widthf1[i][j] -= stud * emHiddenError[j] * inputFutureLayout[i];
                CheckIt("widthf1", widthf1);
                
                for(int i = 0; i < hiddenFutureLayout.Length; i++)
                    for(int j = 0; j < emDelta.Length; j++)
                        widthf2[i][j] -= stud * emDelta[j] * hiddenFutureLayout[i]; // hiddenLayout[j]
                CheckIt("widthf2", widthf2);


                for(int i = 0; i < biasf1.Length; i++)
                    biasf1[i] -= stud * emHiddenError[i];
                CheckIt("biasf1", biasf1);

                for(int i = 0; i < biasf2.Length; i++)
                    biasf2[i] -= stud * emDelta[i];
                CheckIt("biasf2", biasf2);



                lastEmotion = emotion;
                lastEmHiddenError = emHiddenError;

                // Условие новизны

                CheckIt("Условие новизны: "+Vector.AddAll(emotion));
                if(Vector.AddAll(emotion) > 0.2)
                {
                    // Этап мышления
                    

                    double[] hiddenLayout = Init.Zeros(width1[0].Length);
                    for(int i = 0; i < width1[0].Length; i++)
                    {
                        double mem = 0;
                        for(int j = 0; j < inputLayout.Length; j++)
                            mem += inputLayout[j] * width1[j][i];

                        hiddenLayout[i] = FucAct.Sigmoid(mem + bias1[i]);
                    }
                    CheckIt("hiddenLayout", hiddenLayout);

                    double[] outputLayout = Init.Zeros(width2[0].Length);
                    for(int i = 0; i < width2[0].Length; i++)
                    {
                        double mem = 0;
                        for(int j = 0; j < hiddenLayout.Length; j++)
                            mem += hiddenLayout[j] * width2[j][i];

                        outputLayout[i] = FucAct.Sigmoid(mem + bias2[i]);
                    }
                    CheckIt("outputLayout", outputLayout);



                }
                else
                {
                    // Этап автоматизма
                    

                    double[] hiddenLayout = Init.Zeros(width1[0].Length);
                    for(int i = 0; i < width1[0].Length; i++)
                    {
                        double mem = 0;
                        for(int j = 0; j < inputLayout.Length; j++)
                            mem += inputLayout[j] * width1[j][i];

                        hiddenLayout[i] = FucAct.Sigmoid(mem + bias1[i]);
                    }
                    CheckIt("hiddenLayout", hiddenLayout);

                    double[] outputLayout = Init.Zeros(width2[0].Length);
                    for(int i = 0; i < width2[0].Length; i++)
                    {
                        double mem = 0;
                        for(int j = 0; j < hiddenLayout.Length; j++)
                            mem += hiddenLayout[j] * width2[j][i];

                        outputLayout[i] = FucAct.Sigmoid(mem + bias2[i]);
                    }
                    CheckIt("outputLayout", outputLayout);



                }
                // #########


                

                // ##### В СОН #####

                //double stud = 1.0/(1.0 + 0.1 * p); 
                //CheckIt("stud", [stud, p]);

                //double stud = 1.0/p;
                //CheckIt("stud", [stud, p]);

                //double[] error = Vector.Sub(outputLayout, target); //a - t
                //CheckIt("error", error);

                //double[] Delta = Vector.Mult(error, FucAct.DSigmoid(outputLayout));
                //CheckIt("Delta", Delta);
                //CheckIt("outputLayout, DSigmoid", [outputLayout, FucAct.DSigmoid(outputLayout)]);




                // double[] hiddenError = Init.Zeros(width1[0].Length);

                // for (int i = 0; i < hiddenLayout.Length; i++)
                // {
                //     double sumFeedback = 0;
                //     for (int j = 0; j < Delta.Length; j++)
                //     {
                //         // Передаем влияние дельты обратно через веса
                //         sumFeedback += Delta[j] * width2[i][j];
                //     }
                //     hiddenError[i] = sumFeedback * FucAct.DSigmoid(hiddenLayout[i]); // без DSigmoid
                // }
                // CheckIt("hiddenError", hiddenError);



                // for(int i = 0; i < inputLayout.Length; i++)
                //     for(int j = 0; j < hiddenError.Length; j++)
                //         width1[i][j] -= stud * hiddenError[j] * inputLayout[i];
                // CheckIt("width1", width1);
                
                // for(int i = 0; i < hiddenLayout.Length; i++)
                //     for(int j = 0; j < Delta.Length; j++)
                //         width2[i][j] -= stud * Delta[j] * hiddenLayout[i]; // hiddenLayout[j]
                // CheckIt("width2", width2);


                // for(int i = 0; i < bias1.Length; i++)
                //     bias1[i] -= stud * hiddenError[i];
                // CheckIt("bias1", bias1);

                // for(int i = 0; i < bias2.Length; i++)
                //     bias2[i] -= stud * Delta[i];
                // CheckIt("bias2", bias2);



                // lastError = error;
                // lastHiddenError = hiddenError;

                CheckIt("### end ###");
            }

            Line("Разница inputLayout и target:", Vector.AddAll(Vector.Sub(inputLayout, target)), "\n");

            Full(lastError);
            Line("Последная выходная ошибка:", Vector.AddAll(lastError));

            Full(lastHiddenError);
            Line("Последная скрытая ошибка:", Vector.AddAll(lastHiddenError));
        }
        else finished = true;
    }

    static void NeuroRouter(string? input) // dotnet build -c Release
    {
        if (input == "" || input == " " || input == null)
        {
            NAnalis.General("что такое яблоко? ответ зеленый фрукт");
        }
        else
        {
            NAnalis.General(input);
        }
    }
    
    private static void InitGlobal()
    {
        try{
            Init();
        } catch(Exception ex) {
            Throw("Непредвиденная ошибка загрузки начальных данных, продолжение невозможно: ", ex);
        }
    }

    private static void EscPush()
    {
        if (Console.KeyAvailable)
            {
            // Читаем нажатую клавишу без вывода ее на экран
            var keyInfo = Console.ReadKey(intercept: true);
            if (keyInfo.Key == ConsoleKey.Escape)
            {
                Console.WriteLine("\nНажат ESC. Завершение работы программы...");
                finished = true;
            }
        }
    }

    internal static void Setting(string[] input)
    {
        Line("Настройки не сделаны :/");
    }

}