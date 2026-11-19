using System;
using System.Collections.Generic;
using System.IO;

namespace HabitTracker
{
    public class HabitManager
    {
        private readonly string _filePath;
        private readonly List<Habit> _habits;

        public IReadOnlyList<Habit> Habits => _habits.AsReadOnly();

        public HabitManager(string filePath = "habits.txt")
        {
            _filePath = filePath;
            _habits = new List<Habit>();
        }

        public void LoadFromFile()
        {
            _habits.Clear();

            if (!File.Exists(_filePath))
            {
                Console.WriteLine("Файл с привычками не найден. Будет создан новый список.");
                return;
            }

            try
            {
                using (StreamReader reader = new StreamReader(_filePath))
                {
                    string line;
                    int lineNumber = 0;

                    while ((line = reader.ReadLine()) != null)
                    {
                        lineNumber++;
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        try
                        {
                            Habit habit = Habit.FromCsvLine(line);
                            _habits.Add(habit);
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine($"[Предупреждение] Ошибка чтения строки {lineNumber}: {ex.Message}");
                        }
                    }
                }
                Console.WriteLine($"Загружено привычек: {_habits.Count}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка доступа к файлу при чтении: {ex.Message}");
            }
        }

        public void SaveToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_filePath, append: false))
                {
                    foreach (var habit in _habits)
                    {
                        writer.WriteLine(habit.ToCsvLine());
                    }
                }
                Console.WriteLine("Данные успешно сохранены в файл.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка при сохранении файла: {ex.Message}");
            }
        }

        public void AddHabit(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название привычки не может быть пустым.");

            _habits.Add(new Habit(name.Trim()));
        }

        public bool CompleteHabit(int index)
        {
            if (index < 0 || index >= _habits.Count)
                return false;

            _habits[index].MarkAsCompleted();
            return true;
        }
    }
}
