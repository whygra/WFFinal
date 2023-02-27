using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFFinal.Models
{
    public class MovieTheatre
    {
        public MovieTheatre
            (string name, List<Movie> movies, List<List<DateTime>> movieSessions, int numOfSeats)
        {
            Name = name;
            MovieSessions = movieSessions;
            Movies = movies;
            NumOfSeats = numOfSeats;
        }

        public MovieTheatre() : this("Не указано", new(), new(), 10) { }

        // название
        public string Name { get; set; }

        // коллекция времен начала сеансов для каждого фильма
        public List<List<DateTime>> MovieSessions;

        public List<Movie> Movies { get; set; }

        // количество мест в зрительном зале(не более 30)
        int _numOfSeats;
        public int NumOfSeats
        {
            get { return _numOfSeats; }
            set =>
                _numOfSeats = value <= 0 || value > 30
                    ? throw new Exception("")
                    : value;
        }
    }
}
