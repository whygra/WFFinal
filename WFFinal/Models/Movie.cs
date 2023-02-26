using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFFinal.Models
{
    public class Movie
    {

        public Movie(string title, string director, int year, string genre, int ageCategory, int duration)
        {
            Title = title;
            Director = director;
            Year = year;
            Genre = genre;
            AgeCategory = ageCategory;
            Duration = duration;
        }

        public Movie() : this("Не указано", "Не указан", 1910, "не указан", 0, 60) { }
        // название
        public string Title { get; set; }

        // режиссер
        public string Director { get; set; }

        // год выпуска
        int _year;
        public int Year {
            get => _year;
            set =>
                _year = value <= 1900
                    ? throw new Exception("недопустимый год ")
                    : value;
        }

        // жанр
        public string Genre { get; set; }

        // возрастная категория
        int _ageCategory;
        public int AgeCategory
        {
            get => _ageCategory;
            set =>
                _ageCategory = value <= 0
                    ? throw new Exception("недопустимая возрастная категория")
                    : value;
        }

        // длительность (мин.)
        int _duration;
        public int Duration
        {
            get => _duration;
            set =>
                _duration = value <= 0
                    ? throw new Exception("недопустимая длительность ")
                    : value;
        }
    }
}
