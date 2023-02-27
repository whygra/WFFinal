using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFFinal.Models
{
    public class FilmDistributor
    {
        public FilmDistributor(List<MovieTheatre> movieTheatres, string name, string address)
        {
            MovieTheatres = movieTheatres;
            Name = name;
            Address = address;
        }

        public FilmDistributor() : this(new(), "Не указано", "Не указан") { }

        // набор кинотеатров
        public List<MovieTheatre> MovieTheatres { get; set; }

        // название
        public string Name { get; set; }

        // адрес
        public string Address { get; set; }

    }
}
