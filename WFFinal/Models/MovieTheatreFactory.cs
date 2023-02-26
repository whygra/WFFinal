using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WFFinal.Infrastructure;

namespace WFFinal.Models
{
    public class MovieTheatreFactory
    {
        // названия
        public static string[] Names = new[]
        {
            "Звездочка",
            "Фунтура синема",
            "Кинокульт",
            "Дом Кино \"Шевченко\"",
            "Мультиплекс",
        };

        // часы работы
        public static readonly (TimeSpan From, TimeSpan To) WorkingHours = 
            (new(9, 0, 0), new(23, 0, 0));

        // период расписания киносеансов
        public static readonly (DateTime From, DateTime To) SchedulePeriod = 
            (new(2023, 3, 1), new(2023, 4, 1));

        // создать словарь из n случайных фильмов с расписанием сеансов
        public static Dictionary<Movie, List<DateTime>> GetMovieSessions(int nMovies = 8)
        {
            Dictionary<Movie, List<DateTime>> result = new();
            var movies = MovieFactory.GetRange(nMovies);

            // заполнение расписания
            var time = WorkingHours.From;
            for (DateTime date = SchedulePeriod.From; date < SchedulePeriod.To; )
            {
                // случайный фильм из списка
                var movie = Utils.SelectRand(movies);
                // в список сеансов выбранного фильма добавляется время
                result[movie].Add(date + time);
                // время увеличивается на продолжительность фильма
                time.Add(new(0, movie.Duration, 0));
                // если время за пределами часов работы,
                // переносимся на начало следующего дня
                if (time > WorkingHours.To || time < WorkingHours.From)
                {
                    date.AddDays(1);
                    time = WorkingHours.From;
                }
            }
            return result;
        }

        // возвращает объект со случайными значениями атрибутов
        public static MovieTheatre Get() =>
            new(Utils.SelectRand(Names), GetMovieSessions(), Utils.GetRand(10, 31));

        // коллекция кинотеатров
        public static List<MovieTheatre> GetRange(int n)
        {
            List<MovieTheatre> result = new();
            for (int i = 0; i < n; i++)
                result.Add(Get());
            return result;
        }

    }
}
