using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WFFinal.Controllers;
using WFFinal.Infrastructure;
using WFFinal.Models;
using WFFinal.Views;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace WFFinal
{
    public partial class MainForm : Form
    {
        static readonly string Untitled = "Безымянный";
        FilmDistributorController _controller;
        MovieTheatreController _theatreController;


        // форма для вывода при старте приложения, форма "О программе"
        private SplashForm _splashForm = new SplashForm();

        public MainForm()
        {
            _controller = new();
            _theatreController = new(null!);
            InitializeComponent();

            // привязать _splashForm к этому окну, отобразить 
            AddOwnedForm(_splashForm);
            _splashForm.Show();

            // Для отображения содержимого окна-заставки
            Application.DoEvents();
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            // установить варианты выбора жанра и возрастной категории
            SetColumnOptions(dgvRight, "Genre", MovieFactory.Genres);
            SetColumnOptions(dgvRight, "AgeCategory", new[] { 0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20 });

            // прогрессбар
            _splashForm.SetPB(33);

            // сохранить данные в новый файл
            if (_controller.CreateFileIfNotExists())
                ProtectedSerialize();
            // или загрузить данные, убедившись в наличии файла
            else
                ProtectedDeserialize();

            // прогрессбар
            _splashForm.SetPB(66);

            this.Text = Untitled;
            // загрузить данные в форму
            LoadData();

            // прогрессбар
            _splashForm.SetPB(100);

            // задержка для отображения стартового окна
            Thread.Sleep(700);
            // Убираем заставку до появления главной формы
            _splashForm.Hide();
        }


        #region файл

        // новая запись
        private void New(object sender, EventArgs e)
        {
            // сброс контроллера
            _controller.New();
            
            this.Text = Untitled;

            LoadData();
        }

        // сохранение
        private void Save(object sender, EventArgs e)
        {
            ProtectedSerialize();
        }

        // сохранение с выбором пути
        private void SaveAs(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Сохранение";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _controller.FilePath = saveFileDialog1.FileName;
                this.Text = _controller.FilePath;
                ProtectedSerialize();
            }
            toolStripStatusLabel1.Text = "Готово";
        }

        // открыть файл
        private void Open(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _controller.FilePath = openFileDialog1.FileName;
                this.Text = _controller.FilePath;

                ProtectedDeserialize();
                LoadData();
            }
        }

        // открытие перетаскиванием
        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (string file in files)
                if (!System.IO.Path.GetExtension(file).Equals(".json"))
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

            e.Effect = e.Data.GetDataPresent(DataFormats.StringFormat) // строковые данные 
                || e.Data.GetDataPresent(DataFormats.FileDrop) ?  // файл данных - имя файла
                DragDropEffects.Copy :     // разрешена только операция Copy
                DragDropEffects.None;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            // Прием текста
            string[] paths = e.Data.GetData(DataFormats.FileDrop) as string[];
            _controller.FilePath = paths[0];

            ProtectedDeserialize();
            LoadData();

            this.Text = _controller.FilePath;
        }

        // защищенная сериализация, сохранение в файл
        private void ProtectedSerialize()
        {
            try { _controller.Serialize(); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка"); }
        }

        // защищенная десериализация, загрузка из файла
        private void ProtectedDeserialize()
        {
            try { _controller.Deserialize(); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка"); }
        }

        #endregion

        #region работа с коллекцией
        // заполнение
        private void Fill(object sender, EventArgs e)
        {
            _controller.Fill(5);
            ProtectedSerialize();
            LoadData();
        }

        // удаление
        // отключаем стандартное поведение 
        private void dgv_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;
        }

        // удаление по клавише
        private void dgv_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                // удаление
                case Keys.Delete:
                    RemoveSelected(sender, e);
                    break;
                default:
                    return;
            }
        }

        // собственно удаление
        private void RemoveSelected(object sender, EventArgs e)
        {
            // работаем с выбранным контейнером
            var dgv = sender as DataGridView;

            // не трогаем новую строку
            dgv.Rows[dgv.NewRowIndex].Selected = false;

            // при отсутствии выбранных элементов ничего не делать
            if (dgv.SelectedRows.Count == 0) return;

            // окно подтверждения
            ConfirmForm confirmForm = 
                new($"Вы действительно хотите удалить элементы ({dgv.SelectedRows.Count} шт.)");
            if (confirmForm.ShowDialog() != DialogResult.Yes)
                return;

            // удаление всех выделенных
            foreach (DataGridViewRow row in dgv.SelectedRows)
                dgv.Rows.Remove(row);

            // сохранение в файл
            ProtectedSerialize();
        }

        // удаление по кнопке
        private void btnRmvTheatre_Click(object sender, EventArgs e)
        {
            RemoveSelected(dgvLeft, e);
        }

        private void btnRmvMovie_Click(object sender, EventArgs e)
        {
            RemoveSelected(dgvRight, e);
        }

        // удалить список показов удаленного фильма
        private void dgvRight_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            _theatreController.UpdateSessions();
        }

        // установить идентификатор и создать список показов для добавленного фильма
        private void dgvRight_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            _theatreController.SetNewId();
            _theatreController.UpdateSessions();
        }


        // Проверка правильности данных на уровне ячейки таблицы
        private void dgvMovies_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // Не проверяем последнюю строку таблицы - т.е. новую строку
            if (dgvRight.Rows[e.RowIndex].IsNewRow) return;

            string err = "";    // сообщение об ошибке или пустая сторока, если ошибки нет
            string str = e.FormattedValue.ToString();  // текст из проверяемой ячейки

            // Проверка данных в зависимости от номера столбца
            switch (e.ColumnIndex)
            {
                case 1: // Title
                    if (string.IsNullOrWhiteSpace(str)) 
                        err = "Укажите название";
                    break;

                case 2: // Director
                    if (string.IsNullOrWhiteSpace(str))
                        err = "Укажите режиссера";
                    break;

                case 3: // Year
                    if (!int.TryParse(str, out int year))
                        err = "Недопустимый формат в поле 'год'";
                    else if (year <= 1900)
                        err = "Недопустимый год";
                    break;

                case 6: // Duration
                    if (!int.TryParse(str, out int duration))
                        err = "Недопустимый формат в поле 'продолжительность'";
                    else if (duration <= 0)
                        err = "Недопустимая продолжительность";
                    break;
            }

            // вывод сообщения об ошибке
            dgvRight.Rows[e.RowIndex].ErrorText = err;

            e.Cancel = err != "";
        }
        
        // проверка данных на уровне строки
        private void dgvMovies_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {

            // Не обрабатывать новую строку (нижнюю дополнительную строку)
            if (dgvRight.Rows[e.RowIndex].IsNewRow) return;

            string err = "";
            
            foreach(DataGridViewCell cell in dgvRight.Rows[e.RowIndex].Cells)
            {
                if (cell.ColumnIndex == 0) continue;
                if ((cell.Value as string) == "")
                {
                    err = $"Заполните поле \"{cell.OwningColumn.HeaderText}\"";
                    break;
                }
            }

            dgvRight.Rows[e.RowIndex].ErrorText = err;

             e.Cancel = err != "";
        }

        // сохранение после валидации
        private void dgv_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            ProtectedSerialize();
        }


        // обработка выделения элементов коллекции кинотеатров
        private void dgvLeft_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLeft.SelectedRows.Count == 0 || dgvLeft.SelectedRows[0].IsNewRow)
                return;
            // передать выбранный объект контроллеру
            _theatreController.MovieTheatre = (MovieTheatre)dgvLeft.SelectedRows[0].DataBoundItem;
            // 
            lblTheatre.Text = $"Фильмы кинотеатра '{_theatreController.MovieTheatre.Name}'";
        }

        // обработка выделения элементов коллекции фильмов
        private void dgvRight_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvSessions.DataSource = null;
            dgvSessions.AutoGenerateColumns = false;

            // если контроллер связан с объектом
            // передаем название фильма в lblMovie и список сеансов в dgvSessions
            if (_theatreController.MovieTheatre is not null)
            {
                // строковое представление выбранного фильма
                lblMovie.Text = _theatreController.MovieTheatre.Movies[e.RowIndex].ToString();
                // получаем список сеансов
                dgvSessions.DataSource = 
                    _theatreController.GetSessionsByMovieIndex(e.RowIndex);
            }
                    
        }

        #endregion


        #region сортировка

        // выбор поля для сортировки фильмов
        private void SelectMoviesSortField(object sender, EventArgs e)
        {
            if (cmbxSortField.SelectedItem == null)
                return;

            _theatreController.SortComp = cmbxSortField.Text switch
            {
                "названию" => MovieTheatreController.CompByTitle,
                "режиссеру" => MovieTheatreController.CompByDirector,
                "году" => MovieTheatreController.CompByYear,
                _ => null!
            };

            // выбрать порядок, если ещё не выбран
            if (cmbxMoviesSortOrder.SelectedIndex == -1)
                cmbxMoviesSortOrder.SelectedIndex = 0;

            SortMovies();
        }

        // выбор порядка сортировки фильмов
        private void SelectMoviesSortOrder(object sender, EventArgs e)
        {
            if (cmbxSortField.SelectedIndex == -1)
                cmbxSortField.Focus();
            else
                SortMovies();
        }

        // выбор порядка сортировки кинотеатров
        private void SelectTheatresSortOrder(object sender, EventArgs e)
        {
            SortTheatres();
        }

        // сортировка фильмов
        private void SortMovies()
        {
            _theatreController.Sort(cmbxMoviesSortOrder.SelectedIndex == 0);
            LoadData();
        }

        // сортировка кинотеатров
        private void SortTheatres()
        {
            _controller.Sort(cmbxTheatresSortOrder.SelectedIndex == 0);
            LoadData();
        }
        #endregion


        private void SetColumnOptions<T>(DataGridView dgv, string colName, T[] options)
        {

            // получить ссылку на столбец из таблицы dgView, попытка преобразовать к типу столбца 
            // комбобоксов 
            var cbc = dgv.Columns[colName] as DataGridViewComboBoxColumn;
            if (cbc == null)
            {
                MessageBox.Show(
                    $"Столбец {colName} имеет недопустимый тип для загрузки данных",
                    "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } // if

            // загрузка из массива строк
            cbc.DataSource = options;
        }


        // вывод данныx в форму
        private void LoadData()
        {
            txbxName.Text = _controller.Name;
            txbxAddress.Text = _controller.Address;
            bsrcTheatre.DataSource = null!;
            bsrcTheatre.DataSource = _controller.FilmDistributor.MovieTheatres;

            // 
            bsrcMovies.DataMember = "Movies";
        }

        // изменение названия и адреса
        private void SetName(object sender, EventArgs e)
        {
            _controller.SetName(txbxName.Text);
            ProtectedSerialize();
        }

        private void SetAddress(object sender, EventArgs e)
        {
            _controller.SetAddress(txbxAddress.Text);
            ProtectedSerialize();
        }

        // выход из приложения
        private void Exit(object sender, EventArgs e)
        {
            Close();
            return;
        }

        // о программе
        private void ShowAboutForm(object sender, EventArgs e)
        {
            AboutForm splashForm = new AboutForm();
            splashForm.ShowDialog();
        }

        // выбор шрифта
        private void fontBtn_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
                dgvLeft.Font = dgvRight.Font = fontDialog1.Font;
        }

        // выбор цвета фона
        private void backColorBtn_Click(object sender, EventArgs e)
        {

            if (colorDialog1.ShowDialog() == DialogResult.OK)
                dgvLeft.DefaultCellStyle.BackColor = dgvRight.DefaultCellStyle.BackColor = colorDialog1.Color;
        }

        // сворачивание в трей
        private void toTray(object sender, EventArgs e)
        {
            this.Hide();
            notifyIcon1.Visible = true;
        }

        private void Maximize(object sender, EventArgs e)
        {
            this.Show();
            notifyIcon1.Visible = false;
        }

        // выбор элемента по правому клику мыши
        private void dgv_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            var dgv = sender as DataGridView;
            foreach (DataGridViewRow row in dgv.SelectedRows) row.Selected = false;
            dgv.Rows[e.RowIndex].Selected = true;
        }
    }
}
