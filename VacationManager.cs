using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkelonTask5
{
    public class VacationManager
    {
        private const int WEEK = 7;
        private const int HALF_A_MONTH = 14;
        private const int ALLOTED_DAYS_OF_VACATION = 28;

        public enum Weekend
        {
            Saturday = 0,
            Sunday = 1,
        }

        public static Weekend[] weekend = new Weekend[]
        {
            Weekend.Saturday,
            Weekend.Sunday,
        };

        static Dictionary<string, List<DateTime>> InitializeVacationDictionary()
        {
            var vacationDictionary = new Dictionary<string, List<DateTime>>
            {
                { "Иванов Иван Иванович", new List<DateTime>() },
                { "Петров Петр Петрович", new List<DateTime>() },
                { "Юлина Юлия Юлиановна", new List<DateTime>() },
                { "Сидоров Сидор Сидорович", new List<DateTime>() },
                { "Павлов Павел Павлович", new List<DateTime>() },
                { "Георгиев Георг Георгиевич", new List<DateTime>() }
            };

            return vacationDictionary;
        }
        public static void SetVacationDuration()
        {
            // словарь для отпусков
            Dictionary<string, List<DateTime>> vacationDictionary = InitializeVacationDictionary();

            var rnd = new Random();

            foreach (var vacationDaysList in vacationDictionary)
            {
                var vacationDays = vacationDaysList.Value;
                int allotedDaysOfVacation = ALLOTED_DAYS_OF_VACATION;

                while (allotedDaysOfVacation > 0)
                {
                    // случайная дата в пределах текущего года
                    DateTime startOfVacation = RandomDateInCurrentYear();

                    int difference;
                    DateTime endOfVacation;
                    DetermineDurationOfVacation(allotedDaysOfVacation, startOfVacation, out endOfVacation, out difference);

                    // проверка условий по отпуску

                    if (IsPossibleCreateVacation(vacationDays, startOfVacation, endOfVacation))
                    {
                        for (DateTime dt = startOfVacation; dt < endOfVacation; dt = dt.AddDays(1))
                        {
                            vacationDaysList.Value.Add(dt);
                        }

                        allotedDaysOfVacation -= difference;
                    }
                }
            }

            DisplayVacationDictionary(vacationDictionary);
        }

        private static void DisplayVacationDictionary(Dictionary<string, List<DateTime>> vacationDictionary)
        {
            foreach (var vacationDaysList in vacationDictionary)
            {
                var vacationDays = vacationDaysList.Value;
                var employeeName = vacationDaysList.Key;

                Console.WriteLine("Дни отпуска " + employeeName + " : ");
                foreach (var vacation in vacationDays)
                {
                    Console.WriteLine(vacation.Date.ToShortDateString());
                }
                Console.WriteLine('\n');
            }
            Console.ReadKey();
        }

        private static void DetermineDurationOfVacation(int allotedDaysOfVacation, DateTime startOfVacation, out DateTime endOfVacation, out int difference)
        {
            Random rnd = new Random();

            int[] availableVacationDurations = { WEEK, HALF_A_MONTH };
            int vacationDuration = availableVacationDurations[rnd.Next(availableVacationDurations.Length)];

            difference = vacationDuration;
            endOfVacation = startOfVacation.AddDays(vacationDuration);
            if (allotedDaysOfVacation <= WEEK)
            {
                if (vacationDuration == HALF_A_MONTH)
                {
                    // если осталось меньше 7 дней отпуска, уменьшаем продолжительность
                    vacationDuration = WEEK;
                    endOfVacation = startOfVacation.AddDays(vacationDuration);
                    difference = vacationDuration;
                }
                else
                {
                    endOfVacation = startOfVacation.AddDays(allotedDaysOfVacation);
                    difference = allotedDaysOfVacation;
                }
            }
        }

        private static bool IsPossibleCreateVacation(List<DateTime> vacations, DateTime startOfVacation, DateTime endOfVacation)
        {
            if (vacations.Any(d => d >= startOfVacation && d <= endOfVacation))
            {
                return false;
            }

            bool existStart = vacations.Any(d => d.AddMonths(1) >= startOfVacation && d.AddMonths(1) <= endOfVacation);
            bool existEnd = vacations.Any(d => d.AddMonths(-1) <= startOfVacation && d.AddMonths(-1) >= endOfVacation);

            if (existStart || existEnd)
            {
                return false;
            }

            return true;
        }

        private static DateTime RandomDateInCurrentYear()
        {
            Random rnd = new Random();

            var startOfYear = new DateTime(DateTime.Now.Year, 1, 1);
            var endOfYear = new DateTime(DateTime.Now.Year, 12, 31);
            int daysPerYear = (endOfYear - startOfYear).Days;

            var startOfVacation = startOfYear.AddDays(rnd.Next(daysPerYear));
            var currentDayOfWeek = (Weekend)startOfVacation.DayOfWeek;

            while (weekend.Contains(currentDayOfWeek))
            {
                startOfVacation = startOfYear.AddDays(rnd.Next(daysPerYear));
                currentDayOfWeek = (Weekend)startOfVacation.DayOfWeek;
            }

            return startOfVacation;
        }
    }
}
