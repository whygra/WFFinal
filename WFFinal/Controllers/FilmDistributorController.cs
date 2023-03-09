using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFFinal.Models;
using WFFinal.Infrastructure;

namespace WFFinal.Controllers
{
    public class FilmDistributorController
    {
        // компаратор для сортировки кинотеатров по названию
        public static Comparer<MovieTheatre> CompareByName =
            Comparer<MovieTheatre>.Create((mt1, mt2) => mt1.Name.CompareTo(mt2.Name));

        // сериализатор
        JsonSerializer<FilmDistributor> _serializer;
        // обрабатываемый объект
        public FilmDistributor FilmDistributor;
        // путь к файлу
        public string FilePath { get; set; }

        // компаратор сортировки,
        // для добавления нового критерия сортировки достаточно создать новый компаратор
        public Comparer<MovieTheatre> SortComp { get; set; }


        public FilmDistributorController(FilmDistributor filmDistributor, string filePath)
        {
            SortComp = CompareByName; // сортировка по названию
            FilmDistributor = filmDistributor;
            _serializer = new();
            FilePath = filePath;
        }

        public FilmDistributorController() : this(new(), Utils.TempFilePath)
        { }

        #region геттеры и сеттеры для представления

        // свойства для чтения данных компании
        public string Name => FilmDistributor.Name;
        public string Address => FilmDistributor.Address;

        // изменение названия
        public void SetName(string name) => FilmDistributor.Name = name;
        
        // изменение адреса
        public void SetAddress(string address) => FilmDistributor.Address = address;

        #endregion

        #region работа с файлом
        // инициализация - открытие/создание(+заполнение) файла по-умолчанию
        public void New(bool fill = false)
        {
            FilePath = Utils.TempFilePath;
            FilmDistributor = new();
            if (fill) Fill(5);
        }

        // проверка наличия файла
        public bool CreateFileIfNotExists()
        {
            bool created = false;
            // если файл отсутствует - создаем
            if (!Directory.Exists(Utils.DataPath))
                Directory.GetParent(FilePath).Create();
            if (!File.Exists(FilePath))
            {
                File.Create(FilePath).Close();
                created = true;
            }
            return created;
        }
        

        // сериализация
        public void Serialize() => _serializer.Save(FilmDistributor, FilePath);


        // десериализация
        public void Deserialize()
        {
            // загрузка данных
            var loaded = _serializer.Load(FilePath);
            
             FilmDistributor = loaded != null ? loaded :
                throw new Exception("Ошибка десериализации");
        }

        #endregion


        #region операции над коллекцией

        // заполнение коллекции кинотеатров
        public void Fill(int n)
        {
            FilmDistributor.MovieTheatres.Clear();

            FilmDistributor.MovieTheatres.AddRange(MovieTheatreFactory.GetRange(n));
        }

        // сортировка коллекции кинотеатров
        public void Sort(bool isAsc = true)
        {
            FilmDistributor.MovieTheatres.Sort(
                isAsc ?
                SortComp :
                Comparer<MovieTheatre>.Create((mt1, mt2) => SortComp.Compare(mt2, mt1))
                );
        }
        #endregion
    }
}
