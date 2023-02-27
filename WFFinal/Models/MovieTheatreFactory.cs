using Newtonsoft.Json.Linq;
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

        // создать список сеансов для фильмов
        public static List<List<DateTime>> GetMovieSessions(List<Movie> movies)
        {
            List<List<DateTime>> sessions = Enumerable.Repeat(new List<DateTime>(), movies.Count).ToList();

            // заполнение расписания
            var time = WorkingHours.From;
            for (DateTime date = SchedulePeriod.From; date < SchedulePeriod.To; )
            {
                // случайный фильм из списка
                int index = Utils.GetRand(0, movies.Count);
                // в список сеансов выбранного фильма добавляется время
                sessions[index].Add(date + time);
                // время увеличивается на продолжительность фильма
                time += new TimeSpan(0, movies[index].Duration, 0);
                // если время за пределами часов работы,
                // переносимся на начало следующего дня
                if (time > WorkingHours.To || time < WorkingHours.From)
                {
                    date = date.AddDays(1);
                    time = WorkingHours.From;
                }
            }
            return sessions;
        }

        // возвращает объект со случайными значениями атрибутов
        public static MovieTheatre Get()
        {
            var movies = MovieFactory.GetRange(7);
            var sessions = GetMovieSessions(movies);
            return new(Utils.SelectRand(Names), movies, sessions, Utils.GetRand(10, 31));
        }

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
