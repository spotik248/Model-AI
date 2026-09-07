--[
    Воспользоваться синонимом screen -> display
]--

--[
    UX (User Experience — пользовательский опыт) — это логика, удобство, структура и путь пользователя.
        Он отвечает на вопрос: «Понятно ли, как работает продукт и легко ли достичь цели?»

    UI (User Interface — пользовательский интерфейс) — это визуальное оформление.
        Он отвечает на вопрос: «Красиво ли это выглядит и приятно ли этим пользоваться?»
]--

--[
    Исопользовать с массивами:

    int[] numbers = { 1, 2, 3, 4 };
    int sum = numbers.Aggregate(0, (acc, curr) => acc + curr); // Результат: 10
]--

--[
    Для Model.cs:
    
    Команды должны иметь возможность принимать null ( info(null) ),
    Если же это не возможно (аргумент обязателен) — вызвать исключение.
    Тогда нет необходимости в ReplaceChar() для замены некоторых символов.
        
    Есть необходимость при вводе пустой строки давать ей вид null
]--

--[
    В нейронную сеть ( NAnalis.General() ) никогда не вводить пустые строки, null и тд.
    Т.е. переделать правила ввода данных в нейронку.
    Например: если она не может расссуждать, то не давать ей эту возможность
]--

--[
    --
    Добавить операторы +, -, /, * для Vector
    --

    Добавить операторы в структуры массивов:

    --
    public readonly struct Point
    {
        public double X { get; }
        public double Y { get; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Point operator +(Point a, Point b) 
            => new(a.X + b.X, a.Y + b.Y);

        public static Point operator -(Point a, Point b) 
            => new(a.X - b.X, a.Y - b.Y);

        public static Point operator *(Point a, Point b) 
            => new(a.X * b.X, a.Y * b.Y);

        public static Point operator /(Point a, Point b)
        {
            if (b.X == 0 || b.Y == 0)
                throw new DivideByZeroException("Деление на ноль невозможно.");
                
            return new(a.X / b.X, a.Y / b.Y);
        }

        public override string ToString() => $"({X}; {Y})";
    }
    --
]--