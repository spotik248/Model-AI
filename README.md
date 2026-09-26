# MODEL-AI 🚀

[![.NET](https://shields.io)](https://microsoft.com)
[![License: AGPL-3.0](https://shields.io)](https://gnu.org)

*Консольное приложение для обучения и использования разных нейронных сетей на C#.*

## ✨ Особенности (Features)
* **Множество ИИ** — полная поддержка распространненых и уникальных ИИ.
* **Кроссплатформенность** — работает на Windows, Linux, macOS.

## 🛠 Технологии и требования
* **Язык:** C# 12
* **Платформа:** .NET 8.0 SDK (или .NET Framework / .NET Standard)
* **Зависимости:** Newtonsoft.Json 13.0.3, NAudio 2.2.1

## 🚀 Быстрый старт (Getting Started)

### Шаг 1. Клонирование репозитория
```bash
git clone https://github.com
cd название-репозитория
```

### Шаг 2. Сборка проекта
Убедитесь, что у вас установлен [.NET SDK](https://microsoft.comdownload). Соберите проект через CLI:
```bash
dotnet build
```

### Шаг 3. Запуск
Для запуска консольного приложения или веб-API используйте команду:
```bash
dotnet run --project путь/к/основному/проекту.csproj
```

## 💻 Пример использования (Usage)

### Использование через консоль

```csharp
using Model;

var model = new Model();
Model.Main();

Console.WriteLine("Готово!");
```


### Отправить запрос, получить ответ

```csharp
using Model;

var model = new Model();

// Библиотека
Library.InitWords(100, 10); // 100 слов по 10 измерений
// Обучение
Learn.Start(); // Инициализация скорости обучения и количества эпох
// Файловая система
FileSystem.Start();
// Нейронная сеть
NManage.Start(0); // Начальная инициализация ИИ
NManage.Change("pnn"); // Меняем на нужную ИИ
NManage.SetSize(100) // Устанавливаем размер

string input = "What is an apple?"; // Запрос

// Здесь текст разбивается на эмбеддинги, отправляется в ИИ, получается ответ и переводится в обратно в слова

var (questVector, answerVector, learn) = Pre(input);

double[][]? answer = Learning(questVector, answerVector, learn);

var (percentMean, indexWords, indexAnswer) = GetMean(answer, 0.85); // Точность ответа в сотых (один процент)

string[] output = Post(percentMean, indexWords, indexAnswer, questVector);

Console.WriteLine($"Ответ: {string.Join(" ", finallyAnswer)}");

Console.WriteLine("Готово!");
```

## 🗺 Дорожная карта (Roadmap)
- [x] Сделать хаб.
- [x] Сделать рабочую ИИ.
- [ ] Разработать эффективную модель, использующую как можно меньше мощностей (gpu, cpu, mem).
- [ ] Разработать бизнес-Модели, которые можно будет легко интегировать в бизнес и на сервер.
- [ ] Сделать интеграцию с работой на серверах.

## 📄 Лицензия (License)
Этот проект распространяется под лицензией AGPL. Подробнее см. в файле [LICENSE](LICENSE).
