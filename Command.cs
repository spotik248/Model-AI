//using System;

using static Model.Write;
using static Model.Library;
using static Model.NN;
using static Model.Global;

namespace Model;

class Command
{
    public static Cmd[] cmds = GetCmds();
    private static int neurons => nn[id].Size();
    private static string[][] thisfiles = sortedFiles;
    private static object[] os => Computer.os;
    
    public static string[] HelpThemas = [
        "Начальные",
        "Возможности",
        "Информация",
        "Глобальные"
    ];
    public static string[] ModelThemas = [
        "Модель",
        "Нейронная сеть",
        "Настройки обучения",
        "Файловая система",
        "Операционная система"
    ];

    public static string[] HelpTitles = MakeTitles("####", "  ", HelpThemas);
    public static string[] ModelTitles = MakeTitles("####", " ", ModelThemas);

    public static string[] MakeTitles(string border, string[] msg)
    {
        int Length = MyString.Max(msg) - 2;

        for(int i = 0; i < msg.Length; i++)
        {
            if(msg[i].Length % 2 != 0) msg[i] = msg[i] + " ";

            for(int j = msg[i].Length; j <= Length; j++)
                msg[i] = " " + msg[i] + " ";

            msg[i] = border + "  " + msg[i] + "  " + border;
        }

        return msg;
    }

    public static string[] MakeTitles(string border1, string border2, string[] msg)
    {
        int Length = MyString.Max(msg);
        //CheckIt("Length", Length);

        for(int i = 0; i < msg.Length; i++)
        {
            if(msg[i].Length % 2 != 0) 
                msg[i] = msg[i].Length >= Length ? " " + msg[i] : msg[i] + " ";

            for(int j = msg[i].Length; j <= Length; j = msg[i].Length)
                msg[i] = " " + msg[i] + " ";
            
            //CheckIt("msg[i]", msg[i]); CheckIt("msg[i].Length", msg[i].Length);
            msg[i] = border1 + border2 + msg[i] + border2 + border1;
        }

        return msg;
    }
    
    //public static string[] MakeTitles(string[] msg) => MakeTitles("====", "    ", msg);

    public static Cmd[] GetCmds()
    {
        int type = 0;
        
        Cmd[] cmds = [
            // Начальные
            new( // help
                "help", "Список команд.", type,
                [
                    ' ', $"Вывод списка команд.",
                    (string[]? arg) => {GetHelp(arg);}
                ]
            ),
            new( // restart
                "restart", "Перезагружает Модель.", type,
                [
                    ' ', $"Перезагружает полностью Модель, включая диалоги и перезагрузку данных.",
                    () => {Line("Рестарт..."); Model.Main(); }
                ]
            ),
            new( // reload
                "reload", "Перезагружает данные.", type++,
                [
                    ' ', $"Перезагружает данные Модели, но не начинает диалоговые окна.",
                    () => {Line("Перезагрузка..."); Init();}
                ]
            ),
            // Нейронка, возможности и тп.
            new( // change
                "change", "Изменяет тип нейронной сети.", type,
                [
                    ' ', $"Меняет на следующий тип.",
                    () => {NN.Next();}
                ],
                [
                    'n', $"Меняет на n-ый тип.",
                    (string[] arg) => {NN.Change(arg[0]);}
                ]
            ),
            new( // learn
                "learn", "Пакетное обучение.", type,
                [
                    ' ', $"Пакетное обучение нейронной сети из папки ./{/*folder["learnPocket"]*/files["data"]}/",
                    () => {NeuroPocketAnalis.General();}
                ]
            ),
            new( // window
                "window", "Запускает графическое окно.", type,
                [
                    ' ', $"Запускает графическое окно работающее на OpenGL4.",
                    () => {Global.StartForm();}
                ]
            ),
            new( // updateframe
                "updateframe", "Обновляет кадр графического окна.", type++,
                [
                    ' ', $"Насильно обновляет кадр графического окна.",
                    () => {canvas.UpdateFrame();}
                ]
            ),
            // Информация
            new( // info
                "info", "Вывод данных Модели.", type,
                [
                    ' ', $"Вывод всех данных Модели.",
                    () => {GetModel(0, 1, 2, 3, 4);}
                ],
                [
                    'p', $"Вывод данных по параметру типа данных Модели.",
                    (string[]? arg) => {GetModel(arg);}
                ]
            ),
            new( // infonn
                "infonn", "Вывод данных настройки Модели и нейронной сети.", type,
                [
                    ' ', $"Вывод данных настройки Модели и нейронной сети.",
                    () => {GetModel(0, 1, 2);}
                ]
            ),
            new( // infoos
                "infoos", "Вывод данных о системе пользователя.", type,
                [
                    ' ', $"Вывод данных о системе пользователя.",
                    () => {GetModel(4);}
                ]
            ),
            new( // words
                "words", "Вывод библиотеки слов.", type,
                [
                    ' ', $"Вывод всех добавленных слов из библиотеки.",
                    () => {GetWords();}
                ]
            ),
            new( // logs
                "logs", "Открытие файла логов.", type,
                [
                    ' ', $"Открытие текстового файла с логами.",
                    () => {Util.ExecuteFile(WriteLog);}
                ]
            ),
            new( // readlogs
                "readlogs", "Открытие батника логов.", type,
                [
                    ' ', $"Открытие батника логов.",
                    () => {Util.ExecuteFile(BatWriteLog);}
                ]
            ),
            new( // collogs
                "collogs", "Открытие батник с цветами логов.", type,
                [
                    ' ', $"Открытие батник с цветами логов.",
                    () => {Util.ExecuteFile(BatColorLog);}
                ]
            ),
            new( // clear
                "clear", "Очистка логов.", type,
                [
                    ' ', $"Очистка логов.",
                    () => {Util.Clear(WriteLog, ColorLog);}
                ]
            ),
            new( // input
                "input", "Выводит все написанные строки.", type,
                [
                    ' ', $"Выводит все написанные только что строки вместе с '/input'.",
                    (string? input) => {Line(input);}
                ]
            ),
            new( // fullarg
                "fullarg", "Выводит аргументы после '/arg'.", type,
                [
                    ' ', $".",
                    (string[]? argInput) => {Line($"[{string.Join(", ", argInput)}] => Length: {argInput.Length}");}
                ]
            ),
            new( // arg
                "arg", "Выводит аргументы после '/arg'.", type++,
                [
                    ' ', $".",
                    (string[]? arg) => {Line($"[{string.Join(", ", arg)}] => Length: {arg.Length}");}
                ]
            ),
            // Глобальное
            new( // tests
                "test", "Тесты.", type,
                [
                    ' ', $"Тест, показывает последнего возможность релиза.",
                    () => {Test.Testing(0);}
                ],
                [
                    'p', $"Тест, показывает параметральную возможность релиза.",
                    (string[]? arg) => {Test.Testing(arg);}
                ]
            ),
            new( // setting
                "setting", "Настройка Модели.", type,
                [
                    ' ', $"Показать настройки параметров Модели.",
                    (string[]? arg) => {Model.Setting(arg);}
                ]
            ),
            new( // save
                "save", "Сохранение Модели.", type,
                [
                    ' ', $"Сохранение всех параметров Модели в data-файлы.",
                    () => {MSLU.Save();}
                ], // try { Util.Save(arg[0], variable[arg[1]]); } catch { Save(); } 
                [
                    'p', $"По параметральное сохранение, в аргумент нужно писать 1) Путь к файлу json. 2) Специальное название параметра.",
                    (string[]? arg) => {
                        try { Util.Save(arg[0], variable[arg[1]]); }
                        catch(Exception ex) {Exc($"Ошибка при сохранении параметров из аргументов /p: [ {arg[0]}, {arg[1]} => variable={variable[arg[1]]} ]", ex);}
                    }
                ]
            ),
            new( // load
                "load", "Загрузка Модели.", type,
                [
                    ' ', $"",
                    () => {MSLU.Load();}
                ] // try { Type type = variable[arg[1]].GetType(); } catch { Load(); }
            ),
            new( // end
                "end", "Выход из программы.", type,
                [
                    ' ', $"Сохранение и выход из программы.",
                    () => {Model.finished = true;}
                ]
            ),
        ];

        // Action — выполняет действие и ничего не возвращает (метод имеет тип void).
		// Func — выполняет вычисления и обязательно возвращает результат.

        CheckCmds(cmds);

        return cmds;
    }

    public static void CheckCmds(Cmd[] cmds)
    {
        foreach(var cmd in cmds)
        {
            if(cmd.IsNull()) Err($"Команда /{cmd.name} имеет аргументы, которые являются null");
            if(!cmd.IsLengthFine()) Err($"Команда /{cmd.name} имеет неправильный размер первого из аргументов: {cmd.args[0].Length} (Должно быть 3)");
            if(!cmd.IsFormatted()) Err($"Команда /{cmd.name} имеет неправильный формат: {cmd.args[0][0].GetType()}, {cmd.args[0][1].GetType()}, {cmd.args[0][2].GetType()}");
        }
    }

    public static void Helloment()
    {
        Line(
            "\n\n",
            $"Вас приветствует Хаб, содержащая в себе библиотеку нейронных сетей.",
            $"   Версия: {Model.version}.\n"
        );
    }
    
    public static void EndDialog()
    {
        Line(
            "\nМодель готова к работе.",
            "\nЧтобы её использовать просто напишите запрос.",
            "\nЧтобы вызвать команду для этого в начале поставьте '/', подробнее в '/help'\n"
        );
    }

    public static void GetHelp(string[]? arg)
    {
        if(arg.IsNull() || arg.Length < 1)
        {
            GetCommandsNew();
        }
        else // Если указан arg
        {
            CheckIt("При запросе /help, были отправлены аргументы: ", arg);
            //GetCurrCmd(arg);
        }
    }

    // public static void GetCommandsOld()
    // {
    //     Line(
    //         "", HelpTitles[0], // Начальные
    //         "/help — Список команд.",
    //         "/restart — Перезагружает Модель полностью.",
    //         "/reload — Перезагружает данные Модели, не начинает диалоговые окна.",

    //         "", HelpTitles[1], // Нейронка, возможности и тп.
    //         "/change — Изменяет тип нейронной сети",
    //         "/learn — Пакетное обучение.",
    //         "/window — Запускает графическое окно.",

    //         "", HelpTitles[2], // Информация
    //         "/info — Вывод всех данных.",
    //         "/infonn — Вывод данных настройки Модели и нейронной сети.",
    //         "/infoos — Вывод данных о системе пользователя.",
    //         "/words — Библиотека слов.",
    //         "/logs — Открытие логов.",
    //         "/collogs — Выводит цветные логов.",
    //         "/input — Выведет все написанные только что строки вместе с '/input'",
    //         "/arg — Выводит аргументы после '/arg'",

    //         "", HelpTitles[3], // Глобальные
    //         "/test — Тест, показывает новые возможности релиза.",
    //         "/setting — Настройка Модели.",
    //         "/save — Сохранение Модели.",
    //         "/load — Загрузка Модели.",
    //         "/end — Сохранение и выход из программы."
    //     );
    // }

    public static void GetCommandsNew()
    {
        string[] result = new string[cmds.Length];
        string[][] str = Init.Double<string>(HelpTitles.Length, cmds.Length);
        for(int i = 0; i < cmds.Length; i++)
        {
            int count = str[cmds[i].type].Count(); // открываем каждую пустую ячейку массива
            str[cmds[i].type][count] = $"/{cmds[i].name} — {cmds[i].desc}"; // name — desc
        }
        for(int i = 0, k = 0; i < str.Length; i++)
        {
            str[i][0] = $"\n{HelpTitles[i]}\n"+str[i][0]; // Начало
            //str[i][str[i].Count()] = $"\n{HelpTitles[i]}\n"+str[i][0]; // Конец
            for(int j = 0; j < str[i].Count(); j++, k++)
            {
                result[k] = str[i][j];
            }
        }

        Line(result);
    }

    public static void DoCommand(string input, string[] argInput, string command) => DoCommandNew(input, argInput, command);

    public static void DoCommandOld(string input, string[] argInput)
    {
        CheckIt("argInput", argInput);
        CheckIt("argInput[0]", argInput[0]);
        string cmdLine = argInput[0].Replace("/", "");
        bool arged = argInput.Length >= 2;

        List<string> argList = new List<string>();
        if (arged)
            for(int i = 1; i < argInput.Length; i++)
                argList.Add(argInput[i]);
        
        string[] arg = argList.ToArray();
        
        Line("");

        switch (cmdLine)
        {
            // Начальные
            case "help": GetHelp(arg); break;
            case "restart": MsgLine("Рестарт..."); Model.Main(); break;
            case "reload":  MsgLine("Перезагрузка..."); Init(); break;

            // Нейронка, возможности и тп.
            case "change":  NN.Next(); break;
            case "learn":   NeuroPocketAnalis.General(); break;
            case "window":  Global.StartForm(); break;
            case "updateFrame": canvas.UpdateFrame(); break;
            case "rate":    learningRate = arged ? double.Parse(arg[0]) : learningRate; MsgLine($"Нейронная сеть обучается со скоростью: '{learningRate}'"); break;
            case "epoches": epoches = arged ? int.Parse(arg[0]) : epoches; MsgLine($"Нейронная сеть обучается '{epoches}' эпох."); break;
            
            // Информация
            case "info": GetModel(0, 1, 2, 3, 4); break;
            case "infonn": GetModel(0, 1, 2); break;
            case "infoos": GetModel(4); break;
            case "words":   GetWords(); break;
            case "logs":    Util.ExecuteFile(WriteLog); break;
            case "clogs":   Util.ExecuteFile(ColorLog); break;
            case "batlogs": Util.ExecuteFile(BatWriteLog); break;
            case "collogs": Util.ExecuteFile(BatColorLog); break;
            case "input":   MsgLine($"{input}"); break;
            case "fullarg": MsgLine($"[{string.Join(", ", argInput)}] => Length: {argInput.Length}"); break;
            case "arg":     MsgLine($"[{string.Join(", ", arg)}] => Length: {arg.Length}"); break;
            case "clear":   MsgLine("Очистка логов"); Util.Clear(WriteLog, ColorLog); break;

            // Глобальные
            case "test":    Test.Testing(arg); break;
            case "setting": Model.Setting(arg); break;
            case "save":    try { Util.Save(arg[0], variable[arg[1]]); } catch { MSLU.Save(); } break;
            case "load":    try { Type type = variable[arg[1]].GetType(); } catch { MSLU.Load(); } break;
            case "end":     Model.finished = true; break;

            default: MsgLine("Неизвестная команда"); break;
        }
    }

    public static void DoCommandNew(string input, string[] argInput, string command)
    {
        string cmdMsgLine = command.Replace("/", "");
        bool arged = argInput.Length > 1;

        List<string> argList = new List<string>();
        if (arged) // argInput.Length от 2 до бескон.
            for(int i = 1; i < argInput.Length; i++) argList.Add(argInput[i]);
        string[] arg = argList.ToArray();
        
        Line("");

        Cmd cmd = new();
    
        for(int i = 0; i < cmds.Length; i++)
        {
            if(cmdMsgLine == cmds[i].name) // Ищем соответсвие названия команды и написанного текста
            {
                cmd = cmds[i];
                break;
            }
        }

        if(!cmd.IsNull()) // Если такой команды даже не существует
        {
            if(arged) // Если аргументы есть
            {
                int i = 0; // пока что 0
                    
                if(arg[i].Length == 2 && arg[i][0] == '/') // Если размер 2 и в начале '/'
                {
                    foreach(var param in cmd.args)
                    {
                        if(arg[i][1] == (char)param[0]) // Если после '/' ('/x') стоит указатель параметра 'x'
                        {
                            CheckIt<object>("Совпадение: ", [arg[i], param]);
                        }
                    }
                }
            }
            else
            {
                CheckIt("Меньше одного аргумента");
                CheckIt($"Для команды /{cmd.name} используем: /{cmd.args[0][0]}. ('{cmd.args[0][1]}')");

                Delegate meth = (Delegate)cmd.args[0][2];

                Invoke(meth);
            }
        }
        else
        {
            MsgLine("Неизвестная команда");
        }
    }

    public static void GetModel(string[] arg)
    {
        foreach (var set in arg)
        {
            GetModel(int.Parse(set));
        }
    }

    public static void GetModel(params int[] arg)
    {
        foreach (var set in arg)
        {
            GetModel(set);
        }
    }

    public static void GetModel(int set)
    {
        switch (set)
        {
            case 0: // Модель
                Line(
                    "", ModelTitles[0],
                    $"Версия хаба: '{Model.version}'",
                    $"Описание хаба: '{Model.description}'",
                    $"Имя Модели: '{Model.model}'",
                    $"Имя пользователя: '{Model.user}'",
                    $"Время запуска: '{Model.time}'"
                );
                break;
            case 1: // Нейронка
                Line(
                    "", ModelTitles[1],
                    $"Выбранная нейронная сеть: {name} ({shortName})",
                    $"Измерения нейронной сети: {dimension}",
                    $"Кол-во данных библиотеки: {words.Count()}/{words.Length}",
                    $"Кол-во нейронов нейронной сети: {neurons}, размер всего: {neurons * dimension}",
                    $"Обьем потребления оперативной памяти\nнейронной сети: {neurons * dimension * 8 / 1e+6} мегабайт. (Примерно)"
                );
                break;
            case 2: // Обучение
                Line(
                    "", ModelTitles[2],
                    $"Точность обучения: {(int)learn[4] / (double)learn[1]}%",
                    $"Время обучения: {learn[4]} эпох.",
                    $"Эффективность обучения: {((double)learn[1]) * ((int)learn[4]) * 100}%"
                );
                break;
            case 3: // Файлы
                Line(
                    "", ModelTitles[3],
                    $"Корневая директория: {Util.root}", // C:/.../
                    $"Файлы системы ({thisfiles[0].Length}):\n   {string.Join("\n   ", thisfiles[0])}", // cs, exe, bat
                    $"Файлы данных ({thisfiles[1].Length}):\n   {string.Join("\n   ", thisfiles[1])}", // json
                    $"Файлы библиотек ({thisfiles[2].Length}):\n   {string.Join("\n   ", thisfiles[2])}", // dll
                    $"Текстовые файлы ({thisfiles[3].Length}):\n   {string.Join("\n   ", thisfiles[3])}", // txt, md
                    $"Остальные файлы ({thisfiles[4].Length}):\n   {string.Join("\n   ", thisfiles[4])}", // other
                    ""
                );
                break;
            case 4: // Комп
                Line(
                    "", ModelTitles[4],
                    $"Операционная система: {os[0]}",
                    $"Версия ядра: {os[1]}",
                    $"Платформа: {os[2]}",
                    $"Битность платформы: {os[3]}",
                    $"Общая оперативная память: {os[4]} ГБ",
                    $"Процессор: {os[5]}",
                    $"Количество ядер: {os[6]}",
                    $"{os[7]}: Всего {os[8]} Гб, свободно {os[9]} Гб",
                    $"CPU архитектура: {os[10]}",
                    $"Система запущена: {os[11]} часов",
                    $"Ваш IP адрес: {os[12]}"
                );
                break;
            default:
                MsgLine("\nНеизвестный параметр вывода информации");
                break;        
        }
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

    // Вспомогательные

    public static void Invoke(Delegate meth)
    {
        if (meth.Method.GetParameters().Length > 0)
        {
            // Если параметры есть (например, string[]), передаем null в качестве первого аргумента
            meth.DynamicInvoke([null]);
        }
        else
        {
            // Если параметров нет (как у restart), вызываем без аргументов
            meth.DynamicInvoke(null);
        }
    }

}