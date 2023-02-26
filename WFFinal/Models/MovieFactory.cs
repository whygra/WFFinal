using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFFinal.Infrastructure;

namespace WFFinal.Models
{
    public static class MovieFactory
    {
        // жанры
        public static string[] Genres = new[]
        {
            "не указан",    // 0
            "драма",        // 1
            "комедия",      // 2
            "триллер",      // 3
            "ужасы",        // 4
            "боевик",       // 5
            "приключения",  // 6
            "фентези",      // 7
            "фантастика",   // 8
        };

        // заготовки
        public static (string Title, string Director, int Year, string Genre, int AgeCategory, int Duration)[] Movies =
        {
            ("А теперь не смотри", "Роуг Н.", 1973, Genres[3], 18, 110),
            ("Варяг", "Эггерс Р.", 2022, Genres[7], 16, 137),
            ("Призрачная нить", "Андерсон П.Т.", 2017, Genres[1], 18, 130),
            ("Дюна", "Вильнёв Д.", 2021, Genres[8], 12, 155),
            ("Плоть + кровь", "Верховен П.", 1985, Genres[1], 16, 126),
            ("Залечь на дно в Брюгге", "Макдона М.", 2007, Genres[2], 18, 107),
            ("Сталкер", "Тарковский А.", 1979, Genres[8], 12, 163),
            ("Фаворитка", "Лантимос Й.", 2018, Genres[1], 18, 119),
            ("мама!", "Аронофски Д.", 2017, Genres[4], 18, 121),
            ("Седьмой континент", "Ханеке М.", 1989, Genres[1], 16, 104),
            ("Роль", "Лопушанский К.", 2013, Genres[1], 16, 132),
            ("Девичий источник", "Бергман И.", 1960, Genres[1], 16, 89),
            ("Туринская лошадь", "Тарр Б.", 2011, Genres[1], 16, 155),
            ("Ломка", "Бенсон Д.", 2012, Genres[4], 16, 93),
            ("Меланхолия", "фон Триер Л.", 2011, Genres[8], 16, 130),
            ("Необратимость", "Ноэ Г.", 2002, Genres[3], 18, 99),
            ("Заводной апельсин", "Кубрик С.", 1971, Genres[8], 18, 137),
            ("По ту сторону черной радуги", "Косматос П.", 2010, Genres[4], 18, 110),
            ("Забавные игры", "Ханеке М.", 2007, Genres[3], 18, 111),
            ("Самый главный босс", "фон Триер Л.", 2006, Genres[2], 16, 99),
            ("Однажды в… Голливуде", "Тарантино К.", 2019, Genres[1], 18, 161),
        };

        // случайный фильм
        public static Movie Get()
        {
            var m = Utils.SelectRand(Movies);
            return new(m.Title, m.Director, m.Year, m.Genre, m.AgeCategory, m.Duration);
        }

        // коллекция случайных фильмов
        public static List<Movie> GetRange(int n)
        {
            List<Movie> result = new();
            for (int i = 0; i < n; i++)
                result.Add(Get());
            return result;
        }
    }
}
