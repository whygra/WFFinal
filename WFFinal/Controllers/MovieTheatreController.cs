using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFFinal.Models;

namespace WFFinal.Controllers
{
    public class MovieTheatreController
    {
        // компаратор сортировки фильмов
        public Comparer<Movie> SortComp { get; set; }

        // сравнение по названию
        public static Comparer<Movie> CompByTitle = Comparer<Movie>.Create(
            (m1, m2) => m1.Title.CompareTo(m2.Title)
            );

        // по режиссеру
        public static Comparer<Movie> CompByDirector = Comparer<Movie>.Create(
            (m1, m2) => m1.Director.CompareTo(m2.Director)
            );

        // по году выхода
        public static Comparer<Movie> CompByYear = Comparer<Movie>.Create(
            (m1, m2) => m1.Year.CompareTo(m2.Year)
            );

        public MovieTheatreController(MovieTheatre movieTheatre)
        {
            MovieTheatre = movieTheatre;
            SortComp = null!;
        }
        public MovieTheatreController() : this(new())
        { }

        // кинотеатр - открытое свойство
        public MovieTheatre MovieTheatre { get; set; }

        // обновить расписание сеансов
        // удалить лишние списки сеансов
        // добавить пустые списки для новых фильмов
        public void UpdateSessions()
        {
            foreach (var mv in MovieTheatre.Movies)
                if (!MovieTheatre.MovieSessions.ContainsKey(mv.Id))
                    // создать список сеансов для фильма
                    MovieTheatre.MovieSessions[mv.Id] = new();

            foreach (var id in MovieTheatre.MovieSessions.Keys)
                // удалить списки для отсутствующих фильмов
                if(MovieTheatre.Movies.FindIndex(m => m.Id == id) == -1)
                    MovieTheatre.MovieSessions.Remove(id);
        }

        // установить Id для нового фильма
        public void SetNewId()
        {
            // найти максимальный идентификатор
            int maxId = MovieTheatre.Movies.MaxBy(m => m.Id).Id;
            // назначить последнему элементу максимальный идентификатор +1
            // если он не является максимальным
            if (MovieTheatre.Movies.Last().Id != maxId) 
                MovieTheatre.Movies.Last().Id = ++maxId;
        }

        // сортировка коллекции с выбором порядка
        public void Sort(bool isAsc)
        {
            MovieTheatre.Movies.Sort(
                isAsc ?
                SortComp :
                Comparer<Movie>.Create((m1, m2) => SortComp.Compare(m2, m1))
                );
        }

        // вернуть расписание сеансов для фильма
        public List<DateTime> GetSessionsByMovieIndex(int index) => 
            MovieTheatre.MovieSessions[MovieTheatre.Movies[index].Id];

    }
}
