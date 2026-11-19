using System;

namespace HabitTracker
{
    public class Habit
    {
        public string Name { get; set; }
        public int Streak { get; set; }
        public bool IsCompletedToday { get; set; }

        public Habit(string name, int streak = 0, bool isCompletedToday = false)
        {
            Name = name;
            Streak = streak;
            IsCompletedToday = isCompletedToday;
        }

        public void MarkAsCompleted()
        {
            if (!IsCompletedToday)
            {
                IsCompletedToday = true;
                Streak++;
            }
        }

        public string ToCsvLine()
        {
            string safeName = Name.Replace(";", ",");
            return $"{safeName};{Streak};{IsCompletedToday}";
        }

        public static Habit FromCsvLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                throw new FormatException("Пустая строка.");

            string[] parts = line.Split(';');
            if (parts.Length != 3)
                throw new FormatException($"Неверное количество полей в строке: {line}");

            string name = parts.Trim();
            if (string.IsNullOrEmpty(name))
                throw new FormatException("Имя привычки не может быть пустым.");

            if (!int.TryParse(parts, out int streak) || streak < 0)
                throw new FormatException($"Некорректное значение серии дней: {parts}");

            if (!bool.TryParse(parts, out bool isCompleted))
                throw new FormatException($"Некорректное булево значение: {parts}");

            return new Habit(name, streak, isCompleted);
        }

        public override string ToString()
        {
            string status = IsCompletedToday ? "[Выполнено]" : "[Не выполнено]";
            return $"{status} {Name} — серия: {Streak} дн.";
        }
    }
}
