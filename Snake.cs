using System;
using System.Collections.Generic;
using System.Threading;

/// \brief Snake - консольная игра «Змейка».
///
/// \par Назначение
/// Игрок управляет змейкой на поле, собирает еду и увеличивает счёт.
/// Игра заканчивается при столкновении со стеной или с собственным хвостом.
///
/// \par Входные параметры
/// Нажатия клавиш-стрелок (↑ ↓ ← →). Размер поля задаётся константами
/// W и H (в клетках), скорость - константой Delay (в миллисекундах).
///
/// \par Выходные параметры
/// Игровое поле W x H клеток в окне консоли и счёт (в очках, 1 еда = 1 очко).
///
/// \par Вызываемые модули
/// System.Console, System.Collections.Generic.List, System.Random,
/// System.Threading.Thread.
///
/// \par Алгоритм
/// В цикле: отрисовка поля, чтение стрелок, вычисление новой головы,
/// проверка проигрыша, перемещение змейки (рост при съеденной еде).
///
/// \par Ограничения
/// Размер поля фиксирован; новая еда может появиться на теле змейки;
/// требуется консоль с поддержкой SetCursorPosition, .NET 7 или новее.
///
/// \author Фелионидов Данил Сергеевич
/// \author Щеглов Александр Константинович
/// \version 1.0
/// \date 09.10.2026
class Snake
{
    /// Ширина поля, клеток.
    const int W = 30;

    /// Высота поля, клеток.
    const int H = 15;

    /// Задержка между шагами змейки, мс.
    const int Delay = 150;

    /// \brief Точка входа: игровой цикл.
    /// Вход: нажатия стрелок. Выход: поле и счёт в консоли.
    static void Main()
    {
        Console.CursorVisible = false;
        Console.Clear();

        int dx = 1, dy = 0;                              // направление движения
        var body = new List<(int x, int y)> { (5, 5) };  // тело змейки, голова - body[0]
        var rnd = new Random();
        var food = (x: 10, y: 7);                        // координаты еды

        while (true)
        {
            // Отрисовка поля: тело - 'O', еда - '*', пустая клетка - '.'
            Console.SetCursorPosition(0, 0);
            for (int y = 0; y < H; y++)
            {
                for (int x = 0; x < W; x++)
                    Console.Write(body.Contains((x, y)) ? 'O'
                                : (x, y) == food ? '*' : '.');
                Console.WriteLine();
            }
            Console.WriteLine("Счёт: " + (body.Count - 1));
            Thread.Sleep(Delay);

            // Чтение стрелок (разворот на 180° запрещён)
            while (Console.KeyAvailable)
            {
                var k = Console.ReadKey(true).Key;
                if (k == ConsoleKey.UpArrow && dy == 0) (dx, dy) = (0, -1);
                if (k == ConsoleKey.DownArrow && dy == 0) (dx, dy) = (0, 1);
                if (k == ConsoleKey.LeftArrow && dx == 0) (dx, dy) = (-1, 0);
                if (k == ConsoleKey.RightArrow && dx == 0) (dx, dy) = (1, 0);
            }

            // Вычисление новой позиции головы
            var head = (x: body[0].x + dx, y: body[0].y + dy);

            // Проигрыш: выход за границы поля или столкновение с хвостом
            if (head.x < 0 || head.y < 0 || head.x >= W || head.y >= H
                || body.Contains(head))
                break;

            body.Insert(0, head);
            if (head == food)
                food = (rnd.Next(W), rnd.Next(H)); // еда съедена: змейка растёт
            else
                body.RemoveAt(body.Count - 1);     // обычный шаг: хвост подтягивается
        }
        Console.WriteLine("Игра окончена!");
    }
}
