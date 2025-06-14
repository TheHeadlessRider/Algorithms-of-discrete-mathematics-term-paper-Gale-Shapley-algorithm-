using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class GaleShapleyAlgoritm
{
    public static void Main()
    {
        Console.Clear();
        Console.WriteLine("Визуализации алгоритма Гэйла-Шепли\n");

        // Инициализация данных: предпочтения мужчин и женщин
        var menPrefs = new Dictionary<string, List<string>>
        {
            {"Никита", new List<string> {"Тая", "Настя", "Маша"}},
            {"Миша", new List<string> {"Тая", "Настя", "Маша"}},
            {"Гоша", new List<string> {"Тая", "Маша", "Настя"}}
        };

        var womenPrefs = new Dictionary<string, List<string>>
        {
            {"Тая", new List<string> {"Никита", "Миша", "Гоша"}},
            {"Настя", new List<string> {"Миша", "Никита", "Гоша"}},
            {"Маша", new List<string> {"Гоша", "Никита", "Миша"}}
        };

        // Подготовка к работе алгоритма
        var freeMen = new Queue<string>(menPrefs.Keys); // Все мужчины начинают свободными
        var matches = new Dictionary<string, string>();  // Здесь будут храниться пары
        int step = 1;                                    // Счетчик шагов

        PrintState(step++, menPrefs, womenPrefs, matches, freeMen);

        // Основной цикл алгоритма
        while (freeMen.Count > 0)
        {
            var man = freeMen.Dequeue();
            var woman = menPrefs[man].First(); // Мужчина делает предложение самой предпочтительной женщине

            Console.WriteLine($"\n🔹 {man} делает предложение {woman}");
            Thread.Sleep(1500);

            // Обработка предложения
            if (!matches.ContainsValue(woman))
            {
                // Женщина свободна - принимаем предложение
                matches[man] = woman;
                Console.WriteLine($"   ✅ {woman} принимает предложение");
            }
            else
            {
                // Женщина уже в паре - сравниваем мужчин
                var currentPartner = matches.First(p => p.Value == woman).Key;
                if (womenPrefs[woman].IndexOf(man) < womenPrefs[woman].IndexOf(currentPartner))
                {
                    // Новый мужчина лучше - заменяем пару
                    Console.WriteLine($"   🔄 {woman} предпочитает {man} (был {currentPartner})");
                    freeMen.Enqueue(currentPartner);
                    matches.Remove(currentPartner);
                    matches[man] = woman;
                }
                else
                {
                    // Отклоняем предложение
                    Console.WriteLine($"   ❌ {woman} отклоняет предложение");
                    freeMen.Enqueue(man);
                }
            }

            // Обновляем данные
            menPrefs[man].Remove(woman); // Удаляем рассмотренное предложение
            PrintState(step++, menPrefs, womenPrefs, matches, freeMen);
            Thread.Sleep(2000);
        }

        // Вывод результатов
        Console.WriteLine("\n🎉 Все пары стабильны!");
        Console.WriteLine("Финальные пары:");
        foreach (var pair in matches)
        {
            Console.WriteLine($"💑 {pair.Key} ↔ {pair.Value}");
        }
    }

    // Метод для вывода текущего состояния системы
    static void PrintState(int step, Dictionary<string, List<string>> menPrefs,
                         Dictionary<string, List<string>> womenPrefs,
                         Dictionary<string, string> matches,
                         Queue<string> freeMen)
    {
        Console.WriteLine($"\n👣 Шаг {step}. Текущее состояние:");
        Console.WriteLine($"Свободные мужчины: {(freeMen.Count > 0 ? string.Join(", ", freeMen) : "нет")}");

        Console.WriteLine("Текущие пары:");
        foreach (var pair in matches)
        {
            Console.WriteLine($"   {pair.Key} → {pair.Value}");
        }

        Console.WriteLine("\nОставшиеся предпочтения мужчин:");
        foreach (var man in menPrefs)
        {
            Console.WriteLine($"   {man.Key}: [{string.Join(", ", man.Value)}]");
        }
    }
}