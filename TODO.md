## TODO

- Доделать WordLibrary.AddWords() Где TODO

- Переименовать ENN







## Archive TODO

- обновить Save, Load, Update для MSLU

- Добавить описание для каждой нейронной сети

- Переместить Msg("Initialisate...") в начало.
- Но оно ломается, потому что файлы создаются слишком поздно, а Write они необходимы.

- Переделать PocketNAnalis ближе к структуре NAnalis

- Протестить что вообще может Модель

- Доделать команды Command и Cmd

- Переработать логи
1) Сделать логи по настоящему публичными.
2) Убрать логи для NN.
3) Доделать логику для CheckIt

- Сделать окно чисто консоль команд и чисто нейронная сеть. => Model.cs

- Найти рабочую нейронку и поставить на default выбор

- Из EI, достать дефолтную

- Доделать звук NAudio

- Добавить время в логи.
``` cs
    DateTime now = DateTime.Now;

    int year = now.Year;
    int month = now.Month;
    int day = now.Day;
    int hour = now.Hour;
    int minute = now.Minute;
    int second = now.Second;
    
    string formatted = now.ToString("yyyy.MM.dd HH:mm:ss");

    // Пример вывода: 2026.05.17 18:49:00
```

- Убрать прием Exc, починить везде Write.Exc и Write.Throw

- Сделать еще более длинную завязку Model - MSLU - ... - Global для доп классов
- Фигня, тк логи ломаются

- Доделать команды в Model

- Доделать ENN как рабочую Модель

- Сделать NAnalis и NeuroPocketAnalis статическими (static)

- Переместить методы из Tokens в Library или Word.
- И вообще попробовать объединить всё это 