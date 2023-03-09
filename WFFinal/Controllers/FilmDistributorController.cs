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
            SortComp = null!;
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
        public void Initialize(bool fill = false)
        {
            if (File.Exists(Utils.TempFilePath))
            {
                // загружаем данные из файла
                Deserialize();
                return;
            }

            // если файл отсутствует - создаем и заполняем
            if (!Directory.Exists(Utils.DataPath))
                Directory.CreateDirectory(Utils.DataPath);
            File.Create(Utils.TempFilePath).Close();

            // заполнение и запись коллекции в созданный файл
            if (fill)
            {
                Fill(15);
                Serialize();
            }
        }

        // сериализация
        public void Serialize() => _serializer.Save(FilmDistributor, FilePath);


        // десериализация
        public void Deserialize()
        {
            if (!File.Exists(FilePath))
                throw new Exception($"Файл {FilePath} не найден");

            // загрузка данных
            var loaded = _serializer.Load(FilePath);
            if (loaded == null)
                throw new Exception("Ошибка десериализации");
            FilmDistributor = loaded;
        }

        #endregion


        #region операции над коллекцией

        // заполнение коллекции кинотеатров
        public void Fill(int n)
        {
            FilmDistributor.MovieTheatres.Clear();

            FilmDistributor.MovieTheatres.AddRange(MovieTheatreFactory.GetRange(n));
        }
        #endregion
    }
}
