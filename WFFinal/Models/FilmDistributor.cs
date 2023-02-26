using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFFinal.Models
{
    public class FilmDistributor
    {
        // набор кинотеатров
        List<MovieTheatre> MovieTheatres { get; set; }

        // название
        public string Name { get; set; }

        // адрес
        public string Address { get; set; }

    }
}
