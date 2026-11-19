using System;

namespace HabitTracker
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            HabitManager manager = new HabitManager("habits.txt");
            manager.LoadFromFile();

            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine("\n=== ТРЕКЕР ПРИВЫЧЕК ===");
                Console.WriteLine("1. Показать все привычки");
                Console.WriteLine("2. Добавить новую привычку");
                Console.WriteLine("3. Отметить привычку выполненной");
                Console.WriteLine("0. Сохранить и выйти");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        ShowHabits(manager);
                        break;

                    case "2":
                        Console.Write("Введите название привычки: ");
                        string name = Console.ReadLine();
                        try
                        {
                            manager.AddHabit(name);
                            Console.WriteLine("Привычка добавлена!");
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }
                        break;

                    case "3":
                        ShowHabits(manager);
                        if (manager.Habits.Count > 0)
                        {
                            Console.Write("Введите номер выполненной привычки: ");
                            if (int.TryParse(Console.ReadLine(), out int num) && manager.CompleteHabit(num - 1))
                            {
                                Console.WriteLine("Отлично! Привычка отмечена, серия увеличена.");
                            }
                            else
                            {
                                Console.WriteLine("Некорректный номер привычки.");
                            }
                        }
                        break;

                    case "0":
                        manager.SaveToFile();
                        isRunning = false;
                        break;

                    default:
                        Console.WriteLine("Неизвестная команда. Попробуйте снова.");
                        break;
                }
            }
        }

        static void ShowHabits(HabitManager manager)
        {
            if (manager.Habits.Count == 0)
            {
                Console.WriteLine("Список привычек пока пуст.");
                return;
            }

            for (int i = 0; i < manager.Habits.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {manager.Habits[i]}");
            }
        }
    }
}
